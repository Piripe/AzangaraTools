using System.Runtime.InteropServices;

namespace AzangaraTools.Structs;


[StructLayout(LayoutKind.Explicit, Size=4, CharSet=CharSet.Ansi)]
public struct Color
{
    [FieldOffset(0)] public byte R;
    [FieldOffset(1)] public byte G;
    [FieldOffset(2)] public byte B;
    [FieldOffset(3)] public byte A;
}