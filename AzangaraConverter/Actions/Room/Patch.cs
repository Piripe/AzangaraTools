using AzangaraConverter.Helpers;
using AzangaraConverter.Storage;
using StbImageSharp;

namespace AzangaraConverter.Actions.Room;

public class Patch
{

    public static void Run(List<string> args, IStorageProvider storage)
    {
        string? geometryPath = null, geometryBackPath = null, geometryLmPath = null, lmPath = null, outputPath = null;
        while(args.Count > 1) {
            var arg = args[0];
            if (arg.StartsWith('-'))
            {
                args.RemoveAt(0);
                switch (arg)
                {
                    case "--geometry":
                    case "-g":
                        geometryPath = args[0];
                        break;
                    case "--geometry-back":
                    case "-b":
                        geometryBackPath = args[0];
                        break;
                    case "--geometry-lm":
                    case "-m":
                        geometryLmPath = args[0];
                        break;
                    case "--lm":
                    case "-l":
                        lmPath = args[0];
                        break;
                    case "--output":
                    case "-o":
                        outputPath = args[0];
                        break;
                    default:
                        Console.WriteLine("WARNING: Unknown argument " + arg);
                        Help.Run(["room","patch"]);
                        return;

                }

                args.RemoveAt(0);
            }
        }
        
        if (args.Count < 1)
        {
            Help.Run(["room","patch"]);
            return;
        }
        
        var inputPath = args[0];

        if (!inputPath.EndsWith(".room"))
        {
            Console.WriteLine("WARNING: Only .room files are supported as input");
            Help.Run(["room","patch"]);
            return;
        }
        
        var room = storage.GetRoom(inputPath);

        if (geometryPath != null)
        {
            if (!geometryPath.EndsWith(".obj"))
            {
                Console.WriteLine("WARNING: Only .obj files are supported as geometry input");
                Help.Run(["room","patch"]);
                return;
            }

            var objFile = storage.GetFile(geometryPath);
            room.Geometry = ObjModel.FromStream(objFile.OpenRead());
            objFile.CloseRead();
        }
        
        if (geometryBackPath != null)
        {
            if (!geometryBackPath.EndsWith(".obj"))
            {
                Console.WriteLine("WARNING: Only .obj files are supported as geometry back input");
                Help.Run(["room","patch"]);
                return;
            }

            var objFile = storage.GetFile(geometryBackPath);
            room.GeometryBack = ObjModel.FromStream(objFile.OpenRead());
            objFile.CloseRead();
        }
        
        if (geometryLmPath != null)
        {
            if (!geometryLmPath.EndsWith(".obj"))
            {
                Console.WriteLine("WARNING: Only .obj files are supported as geometry light map input");
                Help.Run(["room","patch"]);
                return;
            }

            var objFile = storage.GetFile(geometryLmPath);
            room.GeometryLightMap = ObjModel.FromStream(objFile.OpenRead());
            objFile.CloseRead();
        }
        
        if (lmPath != null)
        {
            if (!lmPath.EndsWith(".png"))
            {
                Console.WriteLine("WARNING: Only .png files are supported as light map input");
                Help.Run(["room","patch"]);
                return;
            }

            var pngFile = storage.GetFile(lmPath);
            room.BitmapLightMap = ImageResult.FromStream(pngFile.OpenRead());
            pngFile.CloseRead();
        }

        if (outputPath != null && !outputPath.EndsWith(".room"))
        {
            
            Console.WriteLine("WARNING: Only .room files are supported as output");
            Help.Run(["room","patch"]);
            return;
        }
        using var result = new MemoryStream();
        room.Export(result);
        storage.WriteFile(outputPath ?? inputPath, result.ToArray());
    }
}