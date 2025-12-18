using AzangaraConverter.Storage;
using AzangaraTools;

namespace AzangaraConverter.Actions;

public class CreatePak
{
    public static void Run(List<string> args)
    {
        if (args.Count < 2)
        {
            Help.Run(["create_pak"]);
            return;
        }
        
        var inputPath = args[0];
        var outputPath = args[1];
        
        if (!outputPath.EndsWith(".pak"))
        {
            Console.WriteLine("WARNING: Only .pak files are supported as output");
            Help.Run(["create_pak"]);
            return;
        }
        
        Console.WriteLine($"Enumerating files...");
        HashSet<string> files = [];
        Queue<string> filesToRead = [];
        filesToRead.Enqueue(inputPath);

        while (filesToRead.Count > 0)
        {
            var file = filesToRead.Dequeue();
            if (Directory.Exists(file))
            {
                foreach (var newFile in Directory.EnumerateFileSystemEntries(file))
                {
                    filesToRead.Enqueue(newFile);
                }
            }
            if (File.Exists(file)) files.Add(file);
        }

        var storage = new BaseStorageProvider();
        
        using var s = new MemoryStream();
        
        PakHelper.Write(s, files.Select(x=>storage.GetFile(x)).ToArray());
                    
        storage.WriteFile(outputPath, s.ToArray());
    }
}