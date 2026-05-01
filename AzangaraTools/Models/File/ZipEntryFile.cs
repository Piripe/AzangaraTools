using System.IO.Compression;

namespace AzangaraTools.Models.File;

public class ZipEntryFile(ZipArchiveEntry file) : IFile
{
    public string Path => file.FullName;
    public int Size => (int)file.Length;
    public byte[] ReadAllBytes()
    {
        var stream = new BinaryReader(file.Open());
        var buffer = stream.ReadBytes(Size);
        stream.Close();
        
        return buffer;
    }

    Stream? _stream;
    public Stream OpenRead()
    {
        _stream = file.Open();
        return _stream;
    }

    public void CloseRead()
    {
        _stream?.Close();
    }
}