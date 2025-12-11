using System.Runtime.InteropServices;

namespace AzangaraTools.Structs;


[StructLayout(LayoutKind.Explicit, Size=16, CharSet=CharSet.Ansi)]
public struct ColorF
{
    [FieldOffset(0)] public float R;
    [FieldOffset(4)] public float G;
    [FieldOffset(8)] public float B;
    [FieldOffset(12)] public float A;
}