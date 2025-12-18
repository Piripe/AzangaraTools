using AzangaraConverter.Helpers;
using AzangaraConverter.Storage;
using AzangaraTools.Models;
using SharpGLTF.Scenes;
using StbImageSharp;

namespace AzangaraConverter.Actions;

public class ConvertMmd
{
    public static void Run(List<string> args)
    {
        Run(args,new BaseStorageProvider());
    }
    public static void Run(List<string> args, IStorageProvider storage)
    {
        ImageResult? texture = null;

        while(args.Count > 2) {
           var arg = args[0];
           if (arg.StartsWith("-"))
           {
               args.RemoveAt(0);
               switch (arg)
               {
                   case "--texture":
                   case "-t":
                       texture = storage.GetImage(args[0]);
                       args.RemoveAt(0);
                       break;
                   default:
                       Console.WriteLine("WARNING: Unknown argument " + arg);
                       Help.Run(["convert_mmd"]);
                       return;

               }
           }
        }
        
        if (args.Count < 2)
        {
            Help.Run(["convert_mmd"]);
            return;
        }
        
        var inputPath = args[0];
        var outputPath = args[1];
        
        Geometry? input = null;

        switch (Path.GetExtension(inputPath))
        {
            case ".mmd":
                input = storage.GetModel(inputPath);
                break;
            case ".obj":
                var objFile = storage.GetFile(inputPath);
                input = ObjModel.FromStream(objFile.OpenRead());
                objFile.CloseRead();
                break;
            case ".glb":
                var glbFile = storage.GetFile(inputPath);
                //SharpGLTF.Schema2.ModelRoot.ReadGLB(glbFile.OpenRead()).LogicalMeshes.Select(x=>x.Primitives.Select(x=>x.VertexAccessors.Values.Select(x=>x.)));
                //glbFile.CloseRead();
                Console.WriteLine("WARNING: GLB input unsupported at the time (will be supported later)");
                break;
        }

        if (input == null)
        {
            Console.WriteLine("ERROR: Invalid input file: " + inputPath);
            return;
        }
        
        
        switch (Path.GetExtension(outputPath))
        {
            case ".mmd":
                using (var s = new MemoryStream()) {
                    input.Export(s);
                    storage.WriteFile(outputPath, s.ToArray());
                }
                break;
            case ".obj":
                using (var s = new MemoryStream()) {
                    
                    ObjHelper.WriteFrameToObj(input.Frames[0], s);
                    
                    storage.WriteFile(outputPath, s.ToArray());
                }
                break;
            case ".glb":
                using (var s = new MemoryStream()) {
                    var model = new SceneBuilder();
                    GltfHelper.ProcessGeometryAnimation(Path.GetFileNameWithoutExtension(inputPath), null, input.Frames, texture, model);
                    
                    model.ToGltf2().WriteGLB(s);
                    
                    storage.WriteFile(outputPath, s.ToArray());
                }
                break;
            default:
                Console.WriteLine("ERROR: Invalid output file: " + outputPath);
                break;
        }
    }
}