using AzangaraTools.Models;
using AzangaraTools.Models.File;
using StbImageSharp;

namespace AzangaraConverter.Storage;

public class BaseStorageProvider : IStorageProvider
{
    public virtual IFile GetFile(string path)
    {
        Console.WriteLine("Reading file {0}", path);
        return new NormalFile(Path.GetDirectoryName(path) ?? ".", Path.GetFileName(path));
    }

    public virtual ImageResult GetImage(string path)
    {
        Console.WriteLine("Loading image {0}", path);
        var file = GetFile(path);
        var image = ImageResult.FromStream(file.OpenRead());
        file.CloseRead();
        return image;
    }

    public virtual Geometry GetModel(string path)
    {
        Console.WriteLine("Loading model {0}", path);
        var file = GetFile(path);
        
        var model = Geometry.Load(file.OpenRead());
        file.CloseRead();
        
        return model;
    }

    public virtual void WriteFile(string path, byte[] content)
    {
        Console.WriteLine("Writing {0}", path);
        File.WriteAllBytes(path, content);
    }
}