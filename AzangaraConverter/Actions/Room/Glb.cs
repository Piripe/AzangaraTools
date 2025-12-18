using AzangaraConverter.Helpers;
using AzangaraConverter.Storage;
using AzangaraTools.Enums;
using AzangaraTools.Models;
using SharpGLTF.Scenes;
using StbImageSharp;

namespace AzangaraConverter.Actions.Room;

public class Glb
{

    public static void Run(List<string> args, IStorageProvider storage)
    {
        
        ImageResult? texture = null;
        ImageResult? backTexture = null;

        while(args.Count > 2) {
            var arg = args[0];
            if (arg.StartsWith('-'))
            {
                args.RemoveAt(0);
                switch (arg)
                {
                    case "--texture":
                    case "-t":
                        texture = storage.GetImage(args[0]);
                        args.RemoveAt(0);
                        break;
                    case "--back-texture":
                    case "-b":
                        backTexture = storage.GetImage(args[0]);
                        args.RemoveAt(0);
                        break;
                    default:
                        Console.WriteLine("WARNING: Unknown argument " + arg);
                        Help.Run(["convert_room","glb"]);
                        return;

                }
            }
        }
        
        if (args.Count < 2)
        {
            Help.Run(["convert_room","glb"]);
            return;
        }
        
        var inputPath = args[0];
        var outputPath = args[1];

        if (!inputPath.EndsWith(".room"))
        {
            Console.WriteLine("WARNING: Only .room files are supported as input");
            Help.Run(["convert_room","glb"]);
            return;
        }
        if (!outputPath.EndsWith(".glb"))
        {
            Console.WriteLine("WARNING: Only .glb files are supported as output");
            Help.Run(["convert_room","glb"]);
            return;
        }
        
        var room = storage.GetRoom(inputPath);
        
        
        var model = new SceneBuilder();

        GltfHelper.ProcessGeometry("Geometry", null, room.Geometry.Frames[0], texture, model);
        GltfHelper.ProcessGeometry("Back", null, room.GeometryBack.Frames[0], backTexture, model);
        GltfHelper.ProcessGeometry("LightMap", null, room.GeometryLightMap.Frames[0], room.BitmapLightMap, model,
            true);
        var staticIndex = 0;
        foreach (var obj in room.GetStatics())
        {
            GltfHelper.ProcessGeometry(
                "Static" + (staticIndex++),
                obj.TexturePath,
                new Frame(
                    obj.Model.Frames[0].GetTransformedVertices(obj.Definition.Transform),
                    obj.Model.Frames[0].Indices),
                obj.Texture,
                model,
                obj.Definition.Alight == AlightMode.Multiply);
        }

        using var s = new MemoryStream();
        model.ToGltf2().WriteGLB(s);
                    
        storage.WriteFile(outputPath, s.ToArray());
    }
}