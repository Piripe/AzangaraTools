using System.Numerics;
using System.Runtime.InteropServices;

namespace AzangaraTools.Structs;

[StructLayout(LayoutKind.Explicit, Size=36, CharSet=CharSet.Ansi)]
public struct Triangle
{
    [FieldOffset(0)] public Vector3 Point1;
    [FieldOffset(12)] public Vector3 Point2;
    [FieldOffset(24)] public Vector3 Point3;
}