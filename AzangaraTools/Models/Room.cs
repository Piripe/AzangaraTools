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
            Data =stream.ReadArray<byte>(256 * 256 * 4)
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

    public Static[] GetStatics()
    {
        _loadedStatics ??= Statics.Select(x => Static.Load(_folderReader, x)).ToArray();
        return _loadedStatics;
    }
}