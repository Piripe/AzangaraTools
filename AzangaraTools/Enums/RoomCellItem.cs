namespace AzangaraTools.Enums;

public enum RoomCellItem : byte
{
    Air = (byte)' ',
    WhiteDoor = (byte)'A',
    BlueDoor = (byte)'B',
    RedDoor = (byte)'C',
    LadderCross = RedDoor,
    GreenDoor = (byte)'D',
    BrownDoor = (byte)'E',
    Exit = BrownDoor,
    Fire = (byte)'F',
    Chain = (byte)'I',
    InvisibleChain = (byte)'i',
    Wall = (byte)'W',
    Airway = (byte)'Y',
    NoBonus = (byte)'1',
    Ladder = (byte)'-',
    InvisibleLadder = (byte)'=',
    Pipe = (byte)'|',
    InvisiblePipe = (byte)'t',
}