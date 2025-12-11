using System.Numerics;
using System.Runtime.InteropServices;
using AzangaraTools.Enums;

namespace AzangaraTools.Structs;

[StructLayout(LayoutKind.Explicit, Size=28, CharSet=CharSet.Ansi)]
public struct Bbox
{
    [FieldOffset(0)] public BboxType Type;
    [FieldOffset(4)] public Vector3 Min;
    [FieldOffset(16)] public Vector3 Max;
}