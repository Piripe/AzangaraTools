using System.Numerics;
using AzangaraTools.Script;

namespace AzangaraTools.Models.Script;

public class LevelPlayerStart
{
    /// <summary>
    /// The room coordinates in which the player will spawn.
    /// </summary>
    [ScriptPropertyName("room_pos")]
    public Vector2 RoomPos {get; set;}
    /// <summary>
    /// The cell position where the player will spawn.
    /// </summary>
    [ScriptPropertyName("pos")]
    public Vector2 Pos {get; set;}
}