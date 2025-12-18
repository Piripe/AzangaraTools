using AzangaraConverter.Storage;
using AzangaraTools;

namespace AzangaraConverter.Actions;

public class UnpackPak
{
    public static void Run(List<string> args)
    {
        if (args.Count < 2)
        {
            Help.Run(["unpack_pak"]);
            return;
        }
        
        var inputPath = args[0];
        var outputPath = args[1];
        
        if (!inputPath.EndsWith(".pak"))
        {
            Console.WriteLine("WARNING: Only .pak files are supported as input");
            Help.Run(["unpack_pak"]);
            return;
        }

        var storage = new BaseStorageProvider();

        var pakFile = storage.GetFile(inputPath);
        foreach (var file in PakHelper.Read(pakFile.OpenRead()))
        {
            storage.WriteFile(Path.Combine(outputPath, file.Path), file.ReadAllBytes());
        }
        pakFile.CloseRead();
    }
}