namespace AzangaraTools.Models.File;

public class PakFile(BinaryReader stream, string path, int offset, int size) : IFile
{
    public string Path => path;
    public int Size => size;

    public byte[] ReadAllBytes()
    {
        stream.BaseStream.Seek(offset, SeekOrigin.Begin);
        var buffer = stream.ReadBytes(size);
        
        return path.EndsWith(".txt") ? buffer.Select(x=>(byte)((x<<4)|(x>>4))).ToArray() : buffer;
    }
    public Stream OpenRead()
    {
        return new OffsetReadOnlyStream(stream.BaseStream, offset, size);
    }

    public void CloseRead()
    {
        
    }
}