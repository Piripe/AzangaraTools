using System.Numerics;
using System.Runtime.InteropServices;

namespace AzangaraTools.Structs;

[StructLayout(LayoutKind.Explicit, Size=544, CharSet=CharSet.Ansi)]
public unsafe struct LightDefinition
{
    [FieldOffset(0)] public fixed byte Model[256];
    [FieldOffset(256)] public fixed byte Texture[256];
    [FieldOffset(512)] public ColorF Color;
    [FieldOffset(528)] public float Range;
    [FieldOffset(532)] public Vector3 Pos;
}