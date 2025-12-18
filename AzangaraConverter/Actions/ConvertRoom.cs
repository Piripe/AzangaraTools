using AzangaraConverter.Actions.Room;
using AzangaraConverter.Storage;

namespace AzangaraConverter.Actions;

public class ConvertRoom
{
    public static void Run(List<string> args)
    {
        Run(args,new BaseStorageProvider());
    }
    public static void Run(List<string> args, IStorageProvider storage)
    {
        var actions = new Dictionary<string, Action<List<string>, IStorageProvider>>()
        {
            {"extract", Extract.Run},
            {"glb", Glb.Run},
        };

        if (args.Count >= 1 && actions.TryGetValue(args[0], out var action))
        {
            action(args.Skip(1).ToList(), storage);
            return;
        }
            
        Help.Run(["convert_room"]);
    }
    
    /*
     
     if (false)
       {
           Console.WriteLine("Room file: ");
           var roomPath = "levels/rooms_6/001.room" ?? Console.ReadLine() ?? "levels/rooms_6/001.room";
           var room = fr.GetRoom(roomPath);
           Console.WriteLine("Geometry image: ");
           var geometryPath = "textures/rooms/5_01_floor.jpg" ??
                              Console.ReadLine() ?? "textures/rooms/5_01_floor.jpg";
           Console.WriteLine("Back image: ");
           var backPath = "textures/rooms/5_01_wall.jpg" ??
                          Console.ReadLine() ?? "textures/rooms/5_01_wall.jpg";

           var model = new SceneBuilder();

           // Generate the geometries
           ProcessGeometry("Geometry", null, room.Geometry.Frames[0], fr.GetImage(geometryPath), model);
           ProcessGeometry("Back", null, room.GeometryBack.Frames[0], fr.GetImage(backPath), model);
           ProcessGeometry("LightMap", null, room.GeometryLightMap.Frames[0], room.BitmapLightMap, model,
               true);
           int staticIndex = 0;
           foreach (var obj in room.GetStatics())
           {
               ProcessGeometry(
                   "Static" + (staticIndex++),
                   obj.TexturePath,
                   new Frame(
                       obj.Model.Frames[0].GetTransformedVertices(obj.Definition.Transform),
                       obj.Model.Frames[0].Indices),
                   obj.Texture,
                   model,
                   obj.Definition.Alight == AlightMode.Multiply);
           }


           // Save the GLTF file
           //string outputFileName = Path.ChangeExtension(args[0], ".gltf");
           model.ToGltf2().Save(Path.GetFileNameWithoutExtension(roomPath) + ".glb");
       }
     
     */
}