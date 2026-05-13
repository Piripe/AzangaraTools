using AzangaraTools.Script;

namespace AzangaraTools.Models.Script;

public class Level
{
    [ScriptPropertyName("header")]
    public required LevelHeader Header { get; set; }
    [ScriptPropertyName("rooms"), ScriptArrayItem("room")]
    public required LevelRoom[] Rooms { get; set; }
    [ScriptPropertyName("maze"), ScriptArrayItem("row")]
    public required string[] Maze { get; set; }
    [ScriptPropertyName("player_start")]
    public required LevelPlayerStart PlayerStart { get; set; }
    
}