using AzangaraTools.Models;
using AzangaraTools.Models.File;
using StbImageSharp;

namespace AzangaraTools;

public class FolderReader
{
    private const uint MagicNumber = 0x4B434150; // "PACK"
    private const ushort Version = 0x0101;

    public Dictionary<string,IFile> LoadedFiles = [];
    public Dictionary<string,Room> LoadedRooms = [];
    public Dictionary<string,Geometry> LoadedModels = [];
    public Dictionary<string,ImageResult> LoadedImages = [];

    public static FolderReader ReadFolder(string path)
    {
        var fullPath = Path.GetFullPath(path);
        var rootFiles = Directory.EnumerateFiles(path);

        HashSet<IFile> files = [];
        
        foreach (var file in rootFiles.Where(x=>x.EndsWith(".pak")))
        {
            
            var fs = File.OpenRead(file);
            var pakFiles = PakHelper.Read(fs);

            foreach (var pakFile in pakFiles)
            {
                files.RemoveWhere(x=>x.Path==pakFile.Path);
                files.Add(pakFile);
            }
        }

        List<string> readDirs = [fullPath];

        while (readDirs.Count > 0)
        {
            var dir = readDirs[0];
            readDirs.RemoveAt(0);
            readDirs.AddRange(Directory.GetDirectories(dir));
            foreach (var file in Directory.EnumerateFiles(dir).Where(x => !x.EndsWith(".pak")))
            {
                var filePath = file.Replace(fullPath, "");
                if (Path.IsPathRooted(filePath)) filePath = filePath.Remove(0,1);
                var newFile = new NormalFile(fullPath, filePath );
                
                files.RemoveWhere(x=>x.Path==newFile.Path);
                files.Add(newFile);
            }
        }
        
        var fr = new FolderReader
        {
            LoadedFiles = files.ToDictionary(x=>x.Path, x=>x)
        };
        
        return fr;
    }

    public Room GetRoom(string path)
    {
        if (LoadedRooms.TryGetValue(path, out var room))
            return room;

        if (!LoadedFiles.TryGetValue(path, out var file))
            throw new FileNotFoundException($"File {path} not found");
        
        room = Room.Load(this, file);
        LoadedRooms.Add(path, room);
        return room;
    }
    public Geometry GetModel(string path)
    {
        if (LoadedModels.TryGetValue(path, out var model))
            return model;

        if (!LoadedFiles.TryGetValue(path, out var file))
            throw new FileNotFoundException($"File {path} not found");
        
        model = Geometry.Load(file.OpenRead());
        file.CloseRead();
        
        LoadedModels.Add(path, model);
        
        return model;
    }

    public ImageResult GetImage(string path)
    {
        if (LoadedImages.TryGetValue(path, out var image))
            return image;

        if (!LoadedFiles.TryGetValue(path, out var file))
            throw new FileNotFoundException($"File {path} not found");
        
        image = ImageResult.FromStream(file.OpenRead());
        file.CloseRead();
        
        LoadedImages.Add(path, image);
        
        return image;
    }
    
}