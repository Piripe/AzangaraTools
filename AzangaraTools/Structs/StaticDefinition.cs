using System.Numerics;
using System.Runtime.InteropServices;
using AzangaraTools.Enums;

namespace AzangaraTools.Structs;

[StructLayout(LayoutKind.Explicit, Size=585, CharSet=CharSet.Ansi)]
public unsafe struct StaticDefinition
{
    [FieldOffset(0)] public fixed byte Model[256];
    [FieldOffset(256)] public fixed byte Texture[256];
    [FieldOffset(512)] public float Fps;
    [FieldOffset(516)] public Matrix4x4 Transform;
    [FieldOffset(580)] public AlightMode Alight;
    [FieldOffset(584)] public byte Alpha;
}