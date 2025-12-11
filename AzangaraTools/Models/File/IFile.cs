namespace AzangaraTools.Models.File;

public interface IFile
{
    public string Path { get; }
    public int Size { get; }
    public byte[] ReadAllBytes();
    public Stream OpenRead();
    public void CloseRead();
}