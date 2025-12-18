using System.Numerics;
using AzangaraConverter.Actions;
using AzangaraTools;
using AzangaraTools.Enums;
using AzangaraTools.Models;
using SharpGLTF.Animations;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Memory;
using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using StbImageSharp;

namespace AzangaraConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var actions = new Dictionary<string, Action<List<string>>>()
            {
                {"convert_mmd", ConvertMmd.Run},
                {"convert_room", ConvertRoom.Run},
                {"create_pak", CreatePak.Run},
                {"help",  Help.Run},
                {"in_folder", InFolder.Run},
                {"unpack_pak", UnpackPak.Run},
            };

            if (args.Length > 0 && actions.TryGetValue(args[0], out var action))
            {
                action(args.Skip(1).ToList());
                return;
            }
            
            Help.Run([]);
            return;

        

        try
        {
                Console.WriteLine("Folder: ");
                var fr = FolderReader.ReadFolder("/media/hdd/Games/Azangara/Azangara/" ?? Console.ReadLine() ?? Environment.CurrentDirectory);
                Console.WriteLine(string.Join('\n', fr.LoadedFiles.Keys.Where(x => x.StartsWith("models"))));
                
                {
                    
                    Console.WriteLine("Model file: ");
                    var modelPath = "models/player.mmd" ?? Console.ReadLine() ?? "models/player.mmd";
                    Console.WriteLine("Texture image: ");
                    var geometryPath = "textures/player_skin.jpg" ?? Console.ReadLine() ?? "textures/player_skin.jpg";
                    
                    var originModel = fr.GetModel(modelPath);
                    
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        static Frame MergeFrames(params Frame[] frames)
        {
            var offset = 0;
            return new Frame(
                frames.SelectMany(x => x.Vertices).ToArray(),
                frames.SelectMany(x =>
                {
                    var offset1 = offset;
                    offset += x.Vertices.Length;
                    return x.Indices.Select(y => (ushort)(y + offset1));
                }).ToArray());
        }

        
    }
}
