using System.Numerics;
using AzangaraTools.Models;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Memory;
using SharpGLTF.Scenes;
using StbImageSharp;
using AlphaMode = SharpGLTF.Materials.AlphaMode;
using VPosNorm = SharpGLTF.Geometry.VertexTypes.VertexPositionNormal;
using VTex = SharpGLTF.Geometry.VertexTypes.VertexTexture1;
using VJoints = SharpGLTF.Geometry.VertexTypes.VertexJoints4;

namespace AzangaraConverter.Helpers;

public class GltfHelper
{
    
        public static void ProcessGeometry(string name, string? objType, Frame frame, ImageResult? texture, SceneBuilder model, bool transparent = false)
        {
            var meshBuilder = new MeshBuilder<VertexPositionNormal, VertexTexture1>(name);
            var vertices = frame.Vertices.Select(v => new VertexBuilder<VertexPositionNormal, VertexTexture1, VertexEmpty>(new VertexPositionNormal(v.Pos, v.Normal), new VertexTexture1(new Vector2(v.U,v.V)))).ToArray();

            // Add the vertices
            var primitiveBuilder = meshBuilder.UsePrimitive(CreateMaterial(objType, frame, texture, model, transparent), 3);

            for (int i = 0; i < frame.Indices.Length; i += 3)
            {
                primitiveBuilder.AddTriangle(vertices[frame.Indices[i]],vertices[frame.Indices[i+1]],vertices[frame.Indices[i+2]]);
            }

            // Add UVs
            //primitiveBuilder.UseTextureCoordinate(0, frame.Vertices.Select(v => new Vector2(v.U, 1 - v.V)).ToArray());

            // Add normals
            //primitiveBuilder.UseNormals(frame.Vertices.Select(v => new Vector3(v.Normal.X, v.Normal.Y, v.Normal.Z)).ToArray());



            model.AddRigidMesh(meshBuilder,
                Matrix4x4.Identity); //.UseScene("0").ToSceneBuilder().AddRigidMesh(meshBuilder, Matrix4x4.Identity);
        }

        public static void ProcessGeometryAnimation(string name, string? objType, Frame[] frames, ImageResult? texture,
            SceneBuilder model, bool transparent = false)
        {
            if (frames == null || frames.Length == 0) return;

            // 1. Setup MeshBuilder with specific Vertex attributes
            // VJoints is crucial: We use it to store the 'Original Vertex Index' (Joints.x)
            // This ensures vertices aren't merged incorrectly and lets us map Deltas back to Frames.
            var meshBuilder = new MeshBuilder<VPosNorm, VTex, VJoints>(name);

            var primitive = meshBuilder.UsePrimitive(CreateMaterial(objType, frames[0], texture, model, transparent));

            // 2. Build Base Geometry (Frame 0)
            var baseFrame = frames[0];

            // Create VertexBuilders for all vertices in Frame 0
            var vertexBuilders = new VertexBuilder<VPosNorm, VTex, VJoints>[baseFrame.Vertices.Length];
            for (int i = 0; i < baseFrame.Vertices.Length; i++)
            {
                var v = baseFrame.Vertices[i];
                // Store 'i' (original index) in Joints.x
                vertexBuilders[i] = new VertexBuilder<VPosNorm, VTex, VJoints>(
                    new VPosNorm(v.Pos, v.Normal),
                    new VTex(new Vector2(v.U, v.V)),
                    new VJoints(i)
                );
            }

            // Add Triangles to the Primitive
            for (int i = 0; i < baseFrame.Indices.Length; i += 3)
            {
                primitive.AddTriangle(
                    vertexBuilders[baseFrame.Indices[i]],
                    vertexBuilders[baseFrame.Indices[i + 1]],
                    vertexBuilders[baseFrame.Indices[i + 2]]
                );
            }

            // 3. Apply Morph Targets (Deltas)
            // We cast to IPrimitiveBuilder to access the low-level SetVertexDelta method
            var primitiveReader = (IPrimitiveBuilder)primitive;
            
            var armature = new NodeBuilder();
            armature.LocalTransform = Matrix4x4.Identity;//.CreateTranslation(0, 0, 0);
            var inst = model.AddRigidMesh(meshBuilder, armature);

            var morphTargets = frames.Skip(1).Select((_, i) => meshBuilder.UseMorphTarget(i)).ToList();

            // Iterate over the ACTUAL vertices currently in the primitive 
            // (Note: SharpGLTF might have reordered them or deduplicated exact matches)
            for (int i = 0; i < primitive.Vertices.Count; i++)
            {
                var vertex = primitive.Vertices[i];

                // Retrieve the Original Index we stored in Joints.x
                // Joints are stored as (x,y,z,w), we used x.
                int originalIndex = (int)vertex.Skinning.Joints.X;

                var baseVert = baseFrame.Vertices[originalIndex];

                // For each target frame (1..N), calculate and set the delta
                for (int f = 1; f < frames.Length; f++)
                {
                    var targetFrame = frames[f];
                    int morphTargetIndex = f - 1; // glTF Morph Targets are 0-indexed

                    var targetVert = targetFrame.Vertices[originalIndex];

                    // Calculate Delta (Target - Base)
                    Vector3 posDelta = targetVert.Pos - baseVert.Pos;
                    Vector3 normDelta = targetVert.Normal - baseVert.Normal;

                    // Apply Delta
                    // SetVertexDelta(morphIndex, vertexIndex, GeometryDelta, MaterialDelta)
                    // We use default for MaterialDelta (Colors/UVs) as we are only morphing Geometry.
                    /*primitiveReader.SetVertexDelta(
                        morphTargetIndex,
                        i,
                        new VertexGeometryDelta(posDelta, normDelta, Vector3.Zero),
                        default
                    );*/
                    //Console.WriteLine($"{morphTargets[morphTargetIndex].Vertices.Count} {primitive.Vertices.Count} {i}");
                    
                    
                    morphTargets[morphTargetIndex].SetVertexDelta(primitive.Vertices[i].Position, new VertexGeometryDelta(posDelta, normDelta, Vector3.Zero));
                }
            }
            
            // 4. Create Node and Assign Mesh
            //var node = model.AddNode().DefaultScene.CreateNode(); //new NodeBuilder("AnimNode_" + Guid.NewGuid());
            //node.WithMesh(meshBuilder);
            
            int targetCount = frames.Length-1;
            
            inst.Content.UseMorphing().SetValue(new float[targetCount]);
            
            var track = inst.Content.UseMorphing().UseTrackBuilder("Default");

            // 5. Generate Animation Track
            // We animate the weights: Frame 0 -> Weights=0; Frame 1 -> Weight[0]=1, etc.
            var timeStep = 1.0f / 12;
            //var animation = new List<(float, float[])>();

            for (int f = 0; f < frames.Length; f++)
            {
                float time = f * timeStep;
                float[] weights = new float[targetCount];

                // Activate the specific morph target for this frame
                if (f > 0) weights[f - 1] = 1.0f;

                track.SetPoint(time, weights, true); //, weights, false);
                //animation.Add((time, weights));
            }


            /*var mesh = model.CreateMesh(meshBuilder);
        node
            .WithMesh(mesh)
            ;*/
        //Console.WriteLine(node.MorphWeights.Count); //.WithMorphingAnimation("GeometryPlayback", animation.CreateSampler(isLinear:false));
        //model.AddNode(node);
        
        //.CreateNode(meshName); //.ToSceneBuilder().AddRigidMesh(Matrix4x4.Identity);
        }

        private static int materialCounter = 0;
        private static Dictionary<string, MaterialBuilder> materials = [];
        public static MaterialBuilder CreateMaterial(string? name, Frame? frame, ImageResult? texture, SceneBuilder model, bool transparent = false)
        {
            if (materials.TryGetValue(name??"", out var mb)) return mb;
            var materialBuilder = new MaterialBuilder(name ?? "Material" + (++materialCounter));

            if (texture != null)
            {
                var s = new MemoryStream();
                new StbImageWriteSharp.ImageWriter().WritePng(
                    texture.Data, 
                    texture.Width, 
                    texture.Height,
                    (StbImageWriteSharp.ColorComponents)texture.Comp, 
                    s);
                //Console.WriteLine(s.Length + " bytes written.");
                //File.WriteAllBytes((name?.Replace('/','_') ?? "Material" + (materialCounter)) + ".png", s.GetBuffer());
                materialBuilder
                    .WithBaseColor(ImageBuilder.From(new MemoryImage(s.GetBuffer())),Vector4.One)
                    .WithEmissive(ImageBuilder.From(new MemoryImage(s.GetBuffer())),Vector3.One, 1);
                if (transparent) materialBuilder.WithAlpha(AlphaMode.BLEND);
            }
            else materialBuilder.WithEmissive(new Vector3(1, 0, 1));
            if (name != null) materials.Add(name, materialBuilder);
            // Define material properties as needed, e.g., colors, textures.
            return materialBuilder;
        }
}