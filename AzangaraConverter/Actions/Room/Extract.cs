using AzangaraConverter.Helpers;
using AzangaraConverter.Storage;

namespace AzangaraConverter.Actions.Room;

public class Extract
{

    public static void Run(List<string> args, IStorageProvider storage)
    {
        string? geometryPath = null, geometryBackPath = null, geometryLmPath = null, lmPath = null;
        while(args.Count > 2) {
            var arg = args[0];
            if (arg.StartsWith('-'))
            {
                args.RemoveAt(0);
                switch (arg)
                {
                    case "--geometry":
                    case "-g":
                        geometryPath = args[0];
                        args.RemoveAt(0);
                        break;
                    case "--geometry-back":
                    case "-b":
                        geometryBackPath = args[0];
                        args.RemoveAt(0);
                        break;
                    case "--geometry-lm":
                    case "-m":
                        geometryLmPath = args[0];
                        args.RemoveAt(0);
                        break;
                    case "--lm":
                    case "-l":
                        lmPath = args[0];
                        args.RemoveAt(0);
                        break;
                    default:
                        Console.WriteLine("WARNING: Unknown argument " + arg);
                        Help.Run(["convert_room","extract"]);
                        return;

                }
            }
        }
        
        if (args.Count < 1)
        {
            Help.Run(["convert_room","extract"]);
            return;
        }
        
        var inputPath = args[0];

        if (!inputPath.EndsWith(".room"))
        {
            Console.WriteLine("WARNING: Only .room files are supported as input");
            Help.Run(["convert_room","extract"]);
            return;
        }
        
        var room = storage.GetRoom(inputPath);

        if (geometryPath != null)
        {
            if (!geometryPath.EndsWith(".obj"))
            {
                Console.WriteLine("WARNING: Only .obj files are supported as geometry output");
                Help.Run(["convert_room","extract"]);
                return;
            }

            using var s = new MemoryStream();
            ObjHelper.WriteFrameToObj(room.Geometry.Frames[0], s);
                    
            storage.WriteFile(geometryPath, s.ToArray());
        }
        
        if (geometryBackPath != null)
        {
            if (!geometryBackPath.EndsWith(".obj"))
            {
                Console.WriteLine("WARNING: Only .obj files are supported as geometry back output");
                Help.Run(["convert_room","extract"]);
                return;
            }

            using var s = new MemoryStream();
            ObjHelper.WriteFrameToObj(room.GeometryBack.Frames[0], s);
                    
            storage.WriteFile(geometryBackPath, s.ToArray());
        }
        
        if (geometryLmPath != null)
        {
            if (!geometryLmPath.EndsWith(".obj"))
            {
                Console.WriteLine("WARNING: Only .obj files are supported as geometry light map output");
                Help.Run(["convert_room","extract"]);
                return;
            }

            using var s = new MemoryStream();
            ObjHelper.WriteFrameToObj(room.GeometryLightMap.Frames[0], s);
                    
            storage.WriteFile(geometryLmPath, s.ToArray());
        }
        
        if (lmPath != null)
        {
            if (!lmPath.EndsWith(".png"))
            {
                Console.WriteLine("WARNING: Only .png files are supported as light map output");
                Help.Run(["convert_room","extract"]);
                return;
            }

            using var s = new MemoryStream();
            
            new StbImageWriteSharp.ImageWriter().WritePng(
                room.BitmapLightMap.Data, 
                room.BitmapLightMap.Width, 
                room.BitmapLightMap.Height,
                (StbImageWriteSharp.ColorComponents)room.BitmapLightMap.Comp, 
                s);
                    
            storage.WriteFile(lmPath, s.ToArray());
        }
    }
}