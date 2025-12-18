namespace AzangaraConverter.Actions;

public class Help
{
    public static void Run(List<string> args)
    {
        switch (args.Count == 0 ? "help" : args[0])
        {
            case "convert_mmd":
                Console.WriteLine("""
                                  Usage: AzangaraConverter.exe convert_mmd [arguments] <input_file> <output_file>

                                  You can use .mmd, .obj or .glb file as input or output.

                                  Arguments:
                                  
                                  --texture (-t)   - Set texture image (only with glb output)
                                  
                                  """);
                break;
            case "in_folder":
                Console.WriteLine("""
                                  Usage: AzangaraConverter.exe in_folder <game_folder> <action>

                                  Run an action within a folder in the way the game does. Allowing to load room statics.
                                  
                                  Actions:
                                  
                                  convert_mmd - Convert mmd file to obj or glb in both ways
                                  convert_room - Convert room file to obj or glb in both ways
                                  """);
                break;
            default:
                Console.WriteLine("""
                                  Usage: AzangaraConverter.exe <action>
                                  
                                  Actions:
                                  
                                  convert_mmd - Convert mmd file to obj or glb in both ways
                                  convert_room - Convert room file to obj or glb in both ways
                                  create_pak - Create a pak file from a folder
                                  help - Show help for an action
                                  in_folder - Run an action within a folder in the way the game does
                                  unpack_pak - Extract the content of a pak file
                                  
                                  """);
            break;
        }
    }
}