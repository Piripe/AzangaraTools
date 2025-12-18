using AzangaraTools.Models.File;

namespace AzangaraTools;

public class PakHelper
{
    private const uint MagicNumber = 0x4B434150; // "PACK"
    private const ushort Version = 0x0101;
    
    public static IFile[] Read(Stream stream)
    {
        var reader = new BinaryReader(stream);
        
        if (reader.ReadUInt32() != MagicNumber) throw new Exception("Invalid PAK file");
        if (reader.ReadUInt16() != Version) throw new Exception("Invalid PAK version");
        
        var fileCount =  reader.ReadInt32() / 136;
        
        var files = new IFile[fileCount];
        
        for (var i = 0; i < fileCount; i++)
        {
            var filePath = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(128).TakeWhile(x=>x!=0).ToArray());
            var fileOffset = reader.ReadInt32();
            var fileLength = reader.ReadInt32();
            files[i] = new PakFile(reader, filePath, fileOffset, fileLength);
            
        }

        return files;
    }
    public static void Write(Stream stream, IFile[] files)
    {
        var writer = new BinaryWriter(stream);
        
        writer.Write(MagicNumber);
        writer.Write(Version);
        
        writer.Write(files.Length * 136);

        var offset = files.Length * 136 + 10;
        
        foreach (var file in files)
        {
            var path = new byte[128];
            System.Text.Encoding.UTF8.GetBytes(file.Path, path);
            writer.Write(path);
            writer.Write(offset);
            writer.Write(file.Size);
            offset += file.Size;
        }
        
        foreach (var file in files)
        {
            if (file.Path.EndsWith(".txt"))
            {
                writer.Write(file.ReadAllBytes().Select(x=>(byte)((x<<4)|(x>>4))).ToArray());
                continue;
            }

            writer.Write(file.ReadAllBytes());
        }
    }

}