using System.Numerics;
using AzangaraTools;
using AzangaraTools.Enums;
using AzangaraTools.Models;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Memory;
using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using StbImageSharp;
using AlphaMode = SharpGLTF.Materials.AlphaMode;

namespace AzangaraRoomToGltf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Folder: ");
                var fr = FolderReader.ReadFolder("/media/hdd/Games/Azangara/Azangara/" ?? Console.ReadLine() ?? Environment.CurrentDirectory);
                Console.WriteLine(string.Join('\n', fr.LoadedFiles.Keys.Where(x => x.StartsWith("models"))));
                Console.WriteLine("Room file: ");
                var roomPath = "levels/rooms_6/001.room" ?? Console.ReadLine() ?? "levels/rooms_6/001.room";
                var room = fr.GetRoom(roomPath);
                Console.WriteLine("Geometry image: ");
                var geometryPath = "textures/rooms/5_01_floor.jpg" ?? Console.ReadLine() ?? "textures/rooms/5_01_floor.jpg";
                Console.WriteLine("Back image: ");
                var backPath = "textures/rooms/5_01_wall.jpg" ?? Console.ReadLine() ?? "textures/rooms/5_01_wall.jpg";
                
                var model = new SceneBuilder();

                // Generate the geometries
                ProcessGeometry("Geometry", null, room.Geometry.Frames[0], fr.GetImage(geometryPath), model);
                ProcessGeometry("Back", null, room.GeometryBack.Frames[0], fr.GetImage(backPath), model);
                ProcessGeometry("LightMap", null, room.GeometryLightMap.Frames[0], room.BitmapLightMap, model, true);
                int staticIndex = 0;
                foreach (var obj in room.GetStatics())
                {
                    ProcessGeometry(
                        "Static"+(staticIndex++), 
                        obj.TexturePath,
                        new Frame(
                            obj.Model.Frames[0].GetTransformedVertices(obj.Definition.Transform),
                            obj.Model.Frames[0].Indices), 
                        obj.Texture, 
                        model,
                        obj.Definition.Alight == AlightMode.Multiply);
                }
                

                // Save the GLTF file
                //string outputFileName = Path.ChangeExtension(args[0], ".gltf");
                model.ToGltf2().Save(Path.GetFileNameWithoutExtension(roomPath) + ".glb");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        static Frame MergeFrames(params Frame[] frames)
        {
            var offset = 0;
            return new Frame(
                frames.SelectMany(x => x.Vertices).ToArray(),
                frames.SelectMany(x =>
                {
                    var offset1 = offset;
                    offset += x.Vertices.Length;
                    return x.Indices.Select(y => (ushort)(y + offset1));
                }).ToArray());
        }

        
        static void ProcessGeometry(string name, string? objType, Frame frame, ImageResult? texture, SceneBuilder model, bool transparent = false)
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

            model.AddRigidMesh(meshBuilder, Matrix4x4.Identity);
        }

        private static int materialCounter = 0;
        private static Dictionary<string, MaterialBuilder> materials = [];
        static MaterialBuilder CreateMaterial(string? name, Frame? frame, ImageResult? texture, SceneBuilder model, bool transparent = false)
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
                Console.WriteLine(s.Length + " bytes written.");
                File.WriteAllBytes((name?.Replace('/','_') ?? "Material" + (materialCounter)) + ".png", s.GetBuffer());
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
}
