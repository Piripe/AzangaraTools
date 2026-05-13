using AzangaraTools.Script;

namespace AzangaraTools.Models.Script;

public class LevelHeader
{
    /// <summary>
    /// Not used
    /// </summary>
    [ScriptPropertyName("background")]
    public string? Background { get; set; }
    /// <summary>
    /// This determines whether or not this level is a bonus level.
    /// </summary>
    [ScriptPropertyName("bonus")]
    public bool Bonus { get; set; }
    /// <summary>
    /// The amount of time in seconds you have to complete the level
    /// </summary>
    /// <remarks>
    /// Only works with <see cref="Bonus"/> set to <see langword="true"/>.
    /// </remarks>
    [ScriptPropertyName("bonus_time")]
    public int BonusTime { get; set; }
    /// <summary>
    /// The amount of diamonds in the level
    /// </summary>
    /// <remarks>
    /// Only works with <see cref="Bonus"/> set to <see langword="true"/>.
    /// </remarks>
    [ScriptPropertyName("bonus_count")]
    public int BonusCount { get; set; }
    /// <summary>
    /// This determines what level is unlocked upon completing the level. Only relevant in map packs
    /// </summary>
    [ScriptPropertyName("enable_castle")]
    public string? EnableCastle { get; set; }
    /// <summary>
    /// Determines the text shown at the end of the level
    /// </summary>
    /// <remarks>
    /// Only caps supported
    /// </remarks>
    [ScriptPropertyName("end_title")]
    public string? EndTitle { get; set; }
    /// <summary>
    /// Determines the text shown in the loading screen
    /// </summary>
    /// <remarks>
    /// Only caps supported
    /// </remarks>
    [ScriptPropertyName("level_name")]
    public string? LevelName { get; set; }
    /// <summary>
    /// Determines the number of the level
    /// </summary>
    /// <remarks>
    /// Works only if <see cref="StartTitle"/> is null
    /// </remarks>
    [ScriptPropertyName("level_number")]
    public int LevelNumber { get; set; }
    /// <summary>
    /// Not used
    /// </summary>
    [ScriptPropertyName("level_time")]
    public int LevelTime { get; set; }
    /// <summary>
    /// Determines the statistic screen for the level. This is the screen when you press (Tab) key or finish the level
    /// </summary>
    [ScriptPropertyName("menu_stat")]
    public string? MenuStat { get; set; }
    /// <summary>
    /// The sound played once the level is loaded. Usually use this if you want the starting jingle
    /// </summary>
    [ScriptPropertyName("music")]
    public string? Music { get; set; }
    /// <summary>
    /// Indicates how long the music is delayed for at the start of the level
    /// </summary>
    [ScriptPropertyName("music_start")]
    public int MusicStart { get; set; }
    /// <summary>
    /// This is used if there is another part to the level, such as a bonus level. Put the path to the next level’s level file here if any
    /// </summary>
    [ScriptPropertyName("next_level")]
    public string? NextLevel { get; set; }
    /// <summary>
    /// Determines what bonuses can be stored in quests
    /// Put in the bonus IDs.
    /// </summary>
    [ScriptPropertyName("quest_list"), ScriptArrayItem("item")]
    public string[]? QuestList { get; set; }
    /// <summary>
    /// Determines the text shown at the beginning of the level
    /// </summary>
    /// <remarks>
    /// Only caps supported
    /// </remarks>
    [ScriptPropertyName("start_title")]
    public string? StartTitle { get; set; }
    /// <summary>
    /// This determines whether or not the level starts by pressing the DEMO button from the menu. Only relevant in map packs
    /// </summary>
    [ScriptPropertyName("use_for_demo")]
    public bool? UseForDemo { get; set; }
    
}