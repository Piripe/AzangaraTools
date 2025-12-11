using System.Runtime.InteropServices;

namespace AzangaraTools.Structs;

[StructLayout(LayoutKind.Explicit, Size=20, CharSet=CharSet.Ansi)]
public struct GeometryHeader
{
    [FieldOffset(0)] public ushort Version;
    [FieldOffset(4)] public int Frames;
    [FieldOffset(8)] public int VCount;
    [FieldOffset(12)] public int FCount;
    [FieldOffset(16)] public int ICount;
}