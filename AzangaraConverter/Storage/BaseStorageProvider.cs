using AzangaraTools.Models;
using AzangaraTools.Models.File;
using StbImageSharp;

namespace AzangaraConverter.Storage;

public class BaseStorageProvider : IStorageProvider
{
    public virtual IFile GetFile(string path)
    {
        return new NormalFile(Path.GetDirectoryName(path) ?? ".", Path.GetFileName(path));
    }

    public virtual ImageResult GetImage(string path)
    {
        var file = GetFile(path);
        var image = ImageResult.FromStream(file.OpenRead());
        file.CloseRead();
        return image;
    }

    public virtual Geometry GetModel(string path)
    {
        var file = GetFile(path);
        
        var model = Geometry.Load(file.OpenRead());
        file.CloseRead();
        
        return model;
    }

    public virtual void WriteFile(string path, byte[] content)
    {
        File.WriteAllBytes(path, content);
    }
}