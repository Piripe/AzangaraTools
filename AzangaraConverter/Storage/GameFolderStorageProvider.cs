using AzangaraTools;
using AzangaraTools.Models;
using AzangaraTools.Models.File;
using StbImageSharp;

namespace AzangaraConverter.Storage;

public class GameFolderStorageProvider(string folder) : BaseStorageProvider
{
    private FolderReader _reader = FolderReader.ReadFolder(folder);

    public override IFile GetFile(string path)
    {
        Console.WriteLine("Reading file {0}", path);
        return _reader.LoadedFiles.TryGetValue(path, out var file) ? file : base.GetFile(path);
    }

    public override Geometry GetModel(string path)
    {
        if (_reader.LoadedFiles.ContainsKey(path))
        {
            Console.WriteLine("Loading model {0}", path);
            return _reader.GetModel(path);
        }
        return base.GetModel(path);
    }

    public override ImageResult GetImage(string path)
    {
        if (_reader.LoadedFiles.ContainsKey(path))
        {
            Console.WriteLine("Loading image {0}", path);
            return _reader.GetImage(path);
        }
        return base.GetImage(path);
    }

    public override void WriteFile(string path, byte[] content)
    {
        if (Path.IsPathRooted(path))
        {
            base.WriteFile(path, content);
            return;
        }
        
        base.WriteFile(Path.Combine(folder, path), content);
    }
}