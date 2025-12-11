using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using AzangaraTools.Enums;
using AzangaraTools.Extensions;
using AzangaraTools.Structs;
using StbImageSharp;

namespace AzangaraTools.Models;

public class Static
{
    public StaticDefinition Definition { get; set; }
    public Geometry Model { get; set; } = null!;
    public string ModelPath { get; set; } = null!;
    public ImageResult Texture { get; set; } = null!;
    public string TexturePath { get; set; } = null!;
    
    internal static unsafe Static Load(FolderReader folderReader, StaticDefinition staticDefinition)
    {
        var res = new Static
        {
            Definition = staticDefinition,
            ModelPath = ((IntPtr)staticDefinition.Model).GetString(256),
            TexturePath = ((IntPtr)staticDefinition.Texture).GetString(256),
        };
        res.Model = folderReader.GetModel(res.ModelPath);
        res.Texture = folderReader.GetImage(res.TexturePath);
        
        return res;
    }
}