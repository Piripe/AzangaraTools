using AzangaraTools.Script;

namespace AzangaraTools.Models.Script;

public class LevelRoom
{
    /// <summary>
    /// This locates the ctrls file for the room. Input the path to the ctrls file. Make sure that you use the correct ctrls file, i.e. the one that is to be used in this room.
    /// </summary>
    [ScriptPropertyName("active_items")]
    public string? ActiveItems { get; set; }
    /// <summary>
    /// This locates the bonuses file for the room. Input the path to the bonuses file.
    /// </summary>
    [ScriptPropertyName("bonuses")]
    public string? Bonuses { get; set; }
    /// <summary>
    /// This tells the game if a demo segment should be played out in this room. Input the path to the demo segment or leave it blank if there is none.
    /// </summary>
    [ScriptPropertyName("demo")]
    public bool Demo { get; set; }
    /// <summary>
    /// This locates the texture for the floor. Here you should input the path to the texture for the specific room.
    /// </summary>
    [ScriptPropertyName("floor_tex")]
    public required string FloorTexture { get; set; }
    /// <summary>
    /// The id of the room is used in determining the layout in the <see cref="Level.Maze"/>. Generally, you can label them starting at 1 and then 2, 3 etc.
    /// </summary>
    [ScriptPropertyName("id")]
    public int Id { get; set; }
    /// <summary>
    /// This locates the monsters file for the room. Input the path to the monsters file.
    /// </summary>
    [ScriptPropertyName("monsters")]
    public string? Monsters { get; set; }
    /// <summary>
    /// This tells the game what music to play in this room.
    /// </summary>
    [ScriptPropertyName("music")]
    public string? Music { get; set; }
    /// <summary>
    /// This locates the .room file. Here you should input the path to the .room file for the specific room.
    /// </summary>
    [ScriptPropertyName("room_file")]
    public required string RoomFile { get; set; }
    /// <summary>
    /// This determines the type of room. The type of room determines how the room is shown in the statistics menu and whether a torch will be used in this room. There are four types: <c>light</c>, <c>dark</c>, <c>secret</c>, and <c>hidden</c>. Torches are only used in dark rooms. Light, dark, and secret rooms are shown in the stats. Hidden rooms are not.
    /// </summary>
    [ScriptPropertyName("type")]
    public required string Type { get; set; }
    /// <summary>
    /// This locates the texture for the wall. Here you should input the path to the texture for the specific room.
    /// </summary>
    [ScriptPropertyName("wall_tex")]
    public required string WallTexture { get; set; }
}