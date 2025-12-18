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

                                  --texture (-t) <file>   - Set texture image (only with glb output)

                                  """);
                break;
            case "convert_room":
                switch (args.Count == 1 ? "help" : args[1])
                {
                    case "extract":
                        Console.WriteLine("""
                                          Usage: AzangaraConverter.exe convert_room glb [arguments] <room_file>

                                          Convert room file to glb.
                                          If used with in_folder action, every statics will be converted.
                                          
                                          Arguments:
                                          
                                          --geometry (-g) <obj_file>      - Export geometry to obj file
                                          --geometry-back (-b) <obj_file> - Export geometry back to obj file
                                          --geometry-lm (-m) <obj_file>   - Export geometry light map to obj file
                                          --lm (-l) <png_file>            - Export light map to png file
                                          """);
                        break;
                    case "glb":
                        Console.WriteLine("""
                                          Usage: AzangaraConverter.exe convert_room glb [arguments] <room_file> <output_file>

                                          Convert room file to glb.
                                          If used with in_folder action, every statics will be converted.
                                          
                                          Arguments:
                                          
                                          --texture (-t) <file>      - Set texture image
                                          --back-texture (-b) <file> - Set back texture image
                                          """);
                        break;
                    default:
                    Console.WriteLine("""
                                      Usage: AzangaraConverter.exe convert_room <action>
                                      
                                      Actions:
                                      
                                      extract - Extract room's geometry to obj or room's light map to png
                                      glb - Convert room file to glb 
                                      patch - Patch room's geometry or light map from obj or png files
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