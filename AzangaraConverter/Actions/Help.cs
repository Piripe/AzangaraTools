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
            case "create_pak":
                Console.WriteLine("""
                                  Usage: AzangaraConverter.exe unpack_pak <in_dir> <pak_file>

                                  Create a pak file from a folder.
                                  """);
                break;
            case "in_folder":
                Console.WriteLine("""
                                  Usage: AzangaraConverter.exe in_folder <game_folder> <action>

                                  Run an action within a folder in the way the game does. Allowing to load room statics.
                                  
                                  Actions:
                                  
                                  convert_mmd - Convert mmd file to obj or glb in both ways
                                  room - Room extraction and patching utilities
                                  """);
                break;
            case "room":
                switch (args.Count == 1 ? "help" : args[1])
                {
                    case "extract":
                        Console.WriteLine("""
                                          Usage: AzangaraConverter.exe room extract [arguments] <room_file>

                                          Extract room's geometry to obj or room's light map to png.
                                          
                                          Arguments:
                                          
                                          --geometry (-g) <out_file>      - Export geometry to obj or mmd file
                                          --geometry-back (-b) <out_file> - Export geometry back to obj or mmd file
                                          --geometry-lm (-m) <out_file>   - Export geometry light map to obj or mmd file
                                          --lm (-l) <png_file>            - Export light map to png file
                                          """);
                        break;
                    case "glb":
                        Console.WriteLine("""
                                          Usage: AzangaraConverter.exe room glb [arguments] <room_file> <output_file>

                                          Convert room file to glb.
                                          If used with in_folder action, every statics will be converted.
                                          
                                          Arguments:
                                          
                                          --texture (-t) <file>      - Set texture image
                                          --back-texture (-b) <file> - Set back texture image
                                          """);
                        break;
                    case "patch":
                        Console.WriteLine("""
                                          Usage: AzangaraConverter.exe room patch [arguments] <room_file>

                                          Patch room's geometry or light map from obj or png files.
                                          
                                          Arguments:
                                          
                                          --geometry (-g) <obj_file>      - Patch geometry from obj file
                                          --geometry-back (-b) <obj_file> - Patch geometry back from obj file
                                          --geometry-lm (-m) <obj_file>   - Patch geometry light map from obj file
                                          --lm (-l) <png_file>            - Patch light map from png file
                                          --output (-o) <room_file>       - Set output room file (if not set, input file will be overwritten)
                                          """);
                        break;
                    default:
                    Console.WriteLine("""
                                      Usage: AzangaraConverter.exe room <action>
                                      
                                      Actions:
                                      
                                      extract - Extract room's geometry to obj or room's light map to png
                                      glb - Convert room file to glb 
                                      patch - Patch room's geometry or light map from obj or png files
                                      """);
                        break;
                }

                break;
            case "unpack_pak":
                Console.WriteLine("""
                                  Usage: AzangaraConverter.exe unpack_pak <pak_file> <out_dir>

                                  Extract the content of a pak file to a folder.
                                  """);
                break;
            default:
                Console.WriteLine("""
                                  Usage: AzangaraConverter.exe <action>
                                  
                                  Actions:
                                  
                                  convert_mmd - Convert mmd file to obj or glb in both ways
                                  create_pak - Create a pak file from a folder
                                  help - Show help for an action
                                  in_folder - Run an action within a folder in the way the game does
                                  room - Room extraction and patching utilities
                                  unpack_pak - Extract the content of a pak file
                                  
                                  """);
            break;
        }
    }
}