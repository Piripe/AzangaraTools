using System.Runtime.InteropServices;

namespace AzangaraTools.Structs;

[StructLayout(LayoutKind.Explicit, Size=200, CharSet=CharSet.Ansi)]
public struct RoomHeader
{
    [FieldOffset(0)] public ushort MagicNumber;
    [FieldOffset(2)] public ushort Version;
    [FieldOffset(4)] public uint FileSize;
    [FieldOffset(8)] public uint ArrayOffset;
    [FieldOffset(12)] public uint GeometryOffset;
    [FieldOffset(16)] public uint GeometryLmOffset;
    [FieldOffset(20)] public uint BitmapLmOffset;
    [FieldOffset(24)] public uint BackWall;
    [FieldOffset(28)] public uint GeometryBackOffset;
    [FieldOffset(32)] public uint BboxOffset;
    [FieldOffset(36)] public uint BboxCount;
    [FieldOffset(40)] public uint LightTriOffset;
    [FieldOffset(44)] public uint LightTriCount;
    [FieldOffset(48)] public uint ShadTriOffset;
    [FieldOffset(52)] public ShadTriCount ShadTriCount;
    [FieldOffset(168)] public uint LightsOffset;
    [FieldOffset(172)] public uint LightsCount;
    [FieldOffset(176)] public uint StaticsOffset;
    [FieldOffset(180)] public uint StaticsCount;
    [FieldOffset(184)] public ColorF LightAmbiance;
}
[System.Runtime.CompilerServices.InlineArray(29)]
public struct ShadTriCount
{
    private uint _element0;
}