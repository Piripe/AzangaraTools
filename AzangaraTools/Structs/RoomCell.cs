using System.Runtime.InteropServices;
using AzangaraTools.Enums;

namespace AzangaraTools.Structs;

[StructLayout(LayoutKind.Explicit, Size=8, CharSet=CharSet.Ansi)]
public struct RoomCell
{
    [FieldOffset(0)] public RoomCellItem Item;
    [FieldOffset(1)] public bool Used;
    [FieldOffset(2)] public bool EnabledWall;
    [FieldOffset(3)] public bool Bbox;
    [FieldOffset(4)] public BboxType BboxType;
}