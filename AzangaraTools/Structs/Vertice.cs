using System.Numerics;
using System.Runtime.InteropServices;

namespace AzangaraTools.Structs;

[StructLayout(LayoutKind.Explicit, Size=32, CharSet=CharSet.Ansi)]
public record struct Vertice
{
    [FieldOffset(0)] public Vector3 Pos;
    [FieldOffset(12)] public Vector3 Normal;
    [FieldOffset(24)] public float U;
    [FieldOffset(28)] public float V;
}