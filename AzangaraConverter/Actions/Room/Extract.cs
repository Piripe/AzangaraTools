using AzangaraConverter.Helpers;
using AzangaraConverter.Storage;

namespace AzangaraConverter.Actions.Room;

public class Extract
{

    public static void Run(List<string> args, IStorageProvider storage)
    {
        string? geometryPath = null, geometryBackPath = null, geometryLmPath = null, lmPath = null;
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
                    default:
                        Console.WriteLine("WARNING: Unknown argument " + arg);
                        Help.Run(["room","extract"]);
                        return;

                }

                args.RemoveAt(0);
            }
        }
        
        if (args.Count < 1)
        {
            Help.Run(["room","extract"]);
            return;
        }
        
        var inputPath = args[0];

        if (!inputPath.EndsWith(".room"))
        {
            Console.WriteLine("WARNING: Only .room files are supported as input");
            Help.Run(["room","extract"]);
            return;
        }
        
        var room = storage.GetRoom(inputPath);

        if (geometryPath != null)
        {
            if (geometryPath.EndsWith(".obj"))
            {
                using var s = new MemoryStream();
                ObjHelper.WriteFrameToObj(room.Geometry.Frames[0], s);
                    
                storage.WriteFile(geometryPath, s.ToArray());
            }
            else if (geometryPath.EndsWith(".obj"))
            {
                using var s = new MemoryStream();
                room.Geometry.Export(s);
                    
                storage.WriteFile(geometryPath, s.ToArray());
            }

            Console.WriteLine("WARNING: Only .obj and .mmd files are supported as geometry output");
            Help.Run(["room","extract"]);
            return;
        }
        
        if (geometryBackPath != null)
        {
            if (geometryBackPath.EndsWith(".obj"))
            {
                using var s = new MemoryStream();
                ObjHelper.WriteFrameToObj(room.GeometryBack.Frames[0], s);
                    
                storage.WriteFile(geometryBackPath, s.ToArray());
            }
            else if (geometryBackPath.EndsWith(".obj"))
            {
                using var s = new MemoryStream();
                room.GeometryBack.Export(s);
                    
                storage.WriteFile(geometryBackPath, s.ToArray());
            }

            Console.WriteLine("WARNING: Only .obj and .mmd files are supported as geometry back output");
            Help.Run(["room","extract"]);
            return;
        }
        
        if (geometryLmPath != null)
        {
            if (geometryLmPath.EndsWith(".obj"))
            {
                using var s = new MemoryStream();
                ObjHelper.WriteFrameToObj(room.GeometryLightMap.Frames[0], s);
                    
                storage.WriteFile(geometryLmPath, s.ToArray());
            }
            else if (geometryLmPath.EndsWith(".obj"))
            {
                using var s = new MemoryStream();
                room.GeometryLightMap.Export(s);
                    
                storage.WriteFile(geometryLmPath, s.ToArray());
            }

            Console.WriteLine("WARNING: Only .obj and .mmd files are supported as geometry light map output");
            Help.Run(["room","extract"]);
            return;
        }
        
        if (lmPath != null)
        {
            if (!lmPath.EndsWith(".png"))
            {
                Console.WriteLine("WARNING: Only .png files are supported as light map output");
                Help.Run(["room","extract"]);
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