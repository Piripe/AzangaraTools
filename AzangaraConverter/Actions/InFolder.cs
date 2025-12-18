using AzangaraConverter.Storage;

namespace AzangaraConverter.Actions;

public class InFolder
{
    public static void Run(List<string> args)
    {
        var actions = new Dictionary<string, Action<List<string>, IStorageProvider>>()
        {
            {"convert_mmd", ConvertMmd.Run},
            {"convert_room", ConvertRoom.Run},
        };

        if (args.Count >= 2 && actions.TryGetValue(args[1], out var action))
        {
            action(args.Skip(2).ToList(), new GameFolderStorageProvider(args[0]));
            return;
        }
            
        Help.Run(["in_folder"]);
    }
}