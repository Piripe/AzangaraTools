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
            case "convert_room":
                switch (args.Count == 1 ? "help" : args[1])
                {
                    case "glb":
                        Console.WriteLine("""
                                          Usage: AzangaraConverter.exe convert_room glb [arguments] <room_file> <output_file>

                                          Convert room file to glb.
                                          If used with in_folder action, every statics will be converted.
                                          
                                          Arguments:
                                          
                                          --texture (-t)        - Set texture image
                                          --back-texture (-b)   - Set back texture image
                                          """);
                        break;
                    default:
                    Console.WriteLine("""
                                      Usage: AzangaraConverter.exe convert_room <action>
                                      
                                      Actions:
                                      
                                      extract_geometry - Convert room's geometry to obj or glb
                                      extract_geometry_back - Convert room's geometry back to obj or glb
                                      extract_geometry_lm - Convert room's geometry light map to obj or glb
                                      extract_lm - Convert room's light map to png
                                      glb - Convert room file to glb 
                                      patch_geometry - Convert room's geometry to obj or glb in both ways
                                      patch_geometry_back - Convert room's geometry back to obj or glb in both ways
                                      patch_geometry_lm - Convert room's geometry  to obj or glb in both ways
                                      patch_lm - Convert room's geometry to obj or glb in both ways
                                      """);
                        break;
                }

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