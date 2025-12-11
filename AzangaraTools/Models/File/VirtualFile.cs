using CommunityToolkit.HighPerformance;

namespace AzangaraTools.Models.File;

public class VirtualFile(string path, Memory<byte> content) : IFile
{
    public string Path => path;
    public int Size => content.Length;

    public byte[] ReadAllBytes()
    {
        return content.ToArray();
    }
    public Stream OpenRead()
    {
        return content.AsStream();
    }

    public void CloseRead()
    {
        
    }
}