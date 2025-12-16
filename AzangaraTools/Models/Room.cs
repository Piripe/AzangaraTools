using System.Numerics;
using System.Runtime.InteropServices;
using AzangaraTools.Structs;
using AzangaraTools.Extensions;
using AzangaraTools.Models.File;
using StbImageSharp;

namespace AzangaraTools.Models;

public class Room
{
    public RoomHeader Header { get; set; }
    public RoomCell[][] RoomGeometry { get; set; } = null!;
    public Geometry Geometry { get; set; } = null!;
    public Geometry GeometryLightMap { get; set; } = null!;
    public Geometry GeometryBack { get; set; } = null!;
    public ImageResult BitmapLightMap { get; set; } = null!;
    public Bbox[] Bbox { get; set; } = null!;
    public Triangle[] LightTri { get; set; } = null!;
    public Triangle[][] ShadTri { get; set; } = null!;
    public LightDefinition[] Lights { get; set; } = null!;
    public StaticDefinition[] Statics { get; set; } = null!;
    private Static[]? _loadedStatics;
    private FolderReader _folderReader = null!;

    internal static Room Load(FolderReader folderReader, IFile roomFile)
    {
        var stream = roomFile.OpenRead();

        var startPos = stream.Position;
        var room = new Room
        {
            Header = stream.ReadStruct<RoomHeader>(),
            _folderReader =  folderReader
        };

        if (room.Header.MagicNumber != 0x524D)
        {
            throw new Exception("Invalid room magic number.");
        }

        if (room.Header.Version != 5)
        {
            throw new Exception("Invalid room version.");
        }

        stream.Seek(startPos + room.Header.ArrayOffset, SeekOrigin.Begin);
        room.RoomGeometry = stream.Read2DArray<RoomCell>(29, 40);

        stream.Seek(startPos + room.Header.GeometryOffset, SeekOrigin.Begin);
        room.Geometry = Models.Geometry.Load(stream);

        stream.Seek(startPos + room.Header.GeometryLmOffset, SeekOrigin.Begin);
        room.GeometryLightMap = Models.Geometry.Load(stream);

        stream.Seek(startPos + room.Header.GeometryBackOffset, SeekOrigin.Begin);
        room.GeometryBack = Models.Geometry.Load(stream);

        stream.Seek(startPos + room.Header.BitmapLmOffset, SeekOrigin.Begin);
        room.BitmapLightMap = new ImageResult()
        {
            Width = 256,
            Height = 256,
            SourceComp = ColorComponents.RedGreenBlueAlpha,
            Comp = ColorComponents.RedGreenBlueAlpha,
            Data = stream.ReadArray<byte>(256 * 256 * 4)
        };
        //room.BitmapLightMap = stream.Read2DArray<Color>(256, 256);

        stream.Seek(startPos + room.Header.BboxOffset, SeekOrigin.Begin);
        room.Bbox = stream.ReadArray<Bbox>((int)room.Header.BboxCount);

        stream.Seek(startPos + room.Header.LightTriOffset, SeekOrigin.Begin);
        room.LightTri = stream.ReadArray<Triangle>((int)room.Header.LightTriCount);

        stream.Seek(startPos + room.Header.ShadTriOffset, SeekOrigin.Begin);
        room.ShadTri = Enumerable.Range(0, 29).Select(x => stream.ReadArray<Triangle>((int)room.Header.ShadTriCount[x]))
            .ToArray();

        stream.Seek(startPos + room.Header.LightsOffset, SeekOrigin.Begin);
        room.Lights = stream.ReadArray<LightDefinition>((int)room.Header.LightsCount);

        stream.Seek(startPos + room.Header.StaticsOffset, SeekOrigin.Begin);
        room.Statics = stream.ReadArray<StaticDefinition>((int)room.Header.StaticsCount);

        

        return room;
    }

    public VirtualFile Export(string path)
    {
        var stream = new MemoryStream();

        Export(stream);
        
        return new VirtualFile(path, stream.GetBuffer());
    }

    public void Export(Stream stream)
    {
        stream.Seek(200, SeekOrigin.Begin);
        var headers = Header with
        {
            ArrayOffset = (uint)stream.Position
        };
        stream.Write2DArray(RoomGeometry);
        headers.GeometryOffset = (uint)stream.Position;
        Geometry.Export(stream);
        headers.GeometryOffset = (uint)stream.Position;
        GeometryLightMap.Export(stream);
        headers.BitmapLmOffset = (uint)stream.Position;
        stream.Write(BitmapLightMap.Data);
        headers.GeometryBackOffset = (uint)stream.Position;
        GeometryBack.Export(stream);
        headers.BboxOffset = (uint)stream.Position;
        headers.BboxCount = (uint)Bbox.Length;
        foreach (var x in Bbox)
        {
            stream.WriteStruct(x);
        }
        headers.LightTriOffset = (uint)stream.Position;
        headers.LightTriCount = (uint)LightTri.Length;
        foreach (var x in LightTri)
        {
            stream.WriteStruct(x);
        }
        headers.ShadTriOffset = (uint)stream.Position;
        for (var i = 0; i < 29; i++)
        {
            var x = ShadTri[i];
            headers.ShadTriCount[i] = (uint)x.Length;
            foreach (var y in x)
            {
                stream.WriteStruct(y);
            }
        }
        headers.LightsOffset = (uint)stream.Position;
        headers.LightsCount = (uint)Lights.Length;
        
        foreach (var x in Lights)
        {
            stream.WriteStruct(x);
        }
        headers.StaticsOffset = (uint)stream.Position;
        headers.StaticsCount = (uint)Statics.Length;
        
        foreach (var x in Statics)
        {
            stream.WriteStruct(x);
        }
        
        stream.Seek(0, SeekOrigin.Begin);
        stream.WriteStruct(headers);
    }

    public Static[] GetStatics()
    {
        _loadedStatics ??= Statics.Select(x => Static.Load(_folderReader, x)).ToArray();
        return _loadedStatics;
    }
}