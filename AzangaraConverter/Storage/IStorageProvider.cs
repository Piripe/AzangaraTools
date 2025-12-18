using AzangaraTools.Models;
using AzangaraTools.Models.File;
using StbImageSharp;

namespace AzangaraConverter.Storage;

public interface IStorageProvider
{
    public IFile GetFile(string path);
    public ImageResult GetImage(string path);
    public Geometry GetModel(string path);
    public Room GetRoom(string path);
    public void WriteFile(string path, byte[] content);
}