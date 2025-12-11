namespace AzangaraTools.Models.File;

public class NormalFile(string root, string path) : IFile
{
    public string Path => path;
    public int Size => (int)new FileInfo(System.IO.Path.Combine(root, path)).Length;

    public byte[] ReadAllBytes()
    {
        return System.IO.File.ReadAllBytes(System.IO.Path.Combine(root, path));
    }

    private FileStream? _fs;
    public Stream OpenRead()
    {
        if (_fs == null)
        {
            _fs = System.IO.File.OpenRead(System.IO.Path.Combine(root, path));
        }
        else if (!_fs.CanSeek)
        {
            _fs.Close();
            _fs = System.IO.File.OpenRead(System.IO.Path.Combine(root, path));
        }
        return _fs;
    }

    public void CloseRead()
    {
        if (_fs != null)
        {
            _fs.Close();
            _fs = null;
        }
    }
}