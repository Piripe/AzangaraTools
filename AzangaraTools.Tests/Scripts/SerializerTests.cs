using System.Numerics;
using System.Xml.Serialization;
using AzangaraTools.Models.Script;
using AzangaraTools.Script;

namespace AzangaraTools.Tests.Scripts;

public class SerializerTests
{
    class TestObject
    {
        [ScriptPropertyName("test")]
        public TestObject2 Test { get; set; }
    }

    class TestObject2
    {
        [ScriptPropertyName("string")]
        public string String { get; set; }
        [ScriptPropertyName("int")]
        public int Int { get; set; }
        [ScriptPropertyName("float")]
        public float Float { get; set; }
        [ScriptPropertyName("vec_two")]
        public Vector2 Vec2 { get; set; }
        [ScriptPropertyName("array"), ScriptArrayItem("element")]
        public string[] Array { get; set; }
        [ScriptPropertyName("int_dict")]
        public Dictionary<string, int> IntDict { get; set; }
    }
    
    TestObject testObject = new()
    {
	    Test = new TestObject2
	    {
		    String = "Test string",
		    Int = 1,
		    Float = 1.5f,
		    Vec2 = new(1.5f,-5.0f),
		    Array = [
			    "one", "two"
		    ],
		    IntDict = new() {["one"] = 1, ["two"] = 2}
	    }
    };

    private string testObjectScript = """
                                      $test 
                                      {
                                      	$string "Test string" 
                                      	$int 1 
                                      	$float 1.5 
                                      	$vec_two 1.5 -5 
                                      	$array 
                                      	{
                                      		$element "one" 
                                      		$element "two" 
                                      	}
                                      	$int_dict 
                                      	{
                                      		$one 1 
                                      		$two 2 
                                      	}
                                      }

                                      """;
    private string testObjectScriptWithComments = """
                                      // comment
                                      $test 
                                      {
                                      	$string "Test string" 
                                      	$int 1 
                                      	$float 1.5 
                                      	$vec_two 1.5 -5.0
                                      	
                                      	$array 
                                      	{
                                      		$element "one" 
                                      		$element "two" 
                                      	}
                                      	$int_dict 
                                      	{
                                      		$one 1 
                                      		$two 2 
                                      	}
                                      } // another comment
                                      """;

    private string testLevelScript = """
                                     $header
                                     {
                                     	$next_level "levels/level_2_1_b.txt"
                                     	$background ""
                                     	$music "music/light4.ogg"
                                     	$bonus 0
                                     	$bonus_time 0
                                     	$bonus_count 0
                                     	$level_time 120
                                     	$level_number 1
                                     //	$level_name "�$001���$029�$029��$029�$022�����$0221"
                                     //	$level_name "�$001���$029�$029��$029�"
                                     //	$start_title "��������"
                                     	$start_title "BLUE PALACE"
                                     //	$start_title " "
                                     //	$end_title "�������� ��������"
                                     	$end_title "COMPLETED"
                                     
                                     	$quest_list
                                     	{
                                     		$item "knife"
                                     	}
                                     
                                     	$menu_stat "scripts/statistic_2.txt"	//���� ���������� ��� ������
                                     	$use_for_demo 0			//���� 1, �� ������� ������������ ������ ��� ������ �����
                                     }
                                     
                                     $rooms
                                     {
                                     	$room
                                     	{
                                     		$id 01
                                     		$room_file "levels/rooms_2_1/001.room"
                                     		$floor_tex "textures/rooms/2_01_floor.jpg"
                                     		$wall_tex "textures/rooms/2_01_wall.jpg"
                                     		$active_items "levels/ctrls_2_1/001_ctrl.txt"
                                     
                                     		$type "secret"
                                     
                                     		$monsters "levels/rooms_2_1/001_monster.txt"
                                     		$bonuses "levels/rooms_2_1/001_bonus.txt"
                                     
                                     		$music "music/light3.ogg"
                                     	}
                                     //---------------------- 1 -----------------------------
                                     	$room
                                     	{
                                     		$id 03
                                     		$room_file "levels/rooms_2_1/003.room"
                                     		$floor_tex "textures/rooms/2_01_floor.jpg"
                                     		$wall_tex "textures/rooms/2_04_wall.jpg"
                                     		$active_items "levels/ctrls_2_1/003_ctrl.txt"
                                     
                                     		$type "light"
                                     
                                     		$monsters "levels/rooms_2_1/003_monster.txt"
                                     		$bonuses "levels/rooms_2_1/003_bonus.txt"
                                     
                                     		$music "music/light3.ogg"
                                     	}
                                     
                                     	$room
                                     	{
                                     		$id 04
                                     		$room_file "levels/rooms_2_1/004.room"
                                     		$floor_tex "textures/rooms/2_01_floor.jpg"
                                     		$wall_tex "textures/rooms/2_04_wall.jpg"
                                     		$active_items "levels/ctrls_2_1/004_ctrl.txt"
                                     
                                     		$type "light"
                                     
                                     		$monsters "levels/rooms_2_1/004_monster.txt"
                                     		$bonuses "levels/rooms_2_1/004_bonus.txt"
                                     
                                     //		$demo "levels/rooms_2_1/004_demo.txt"
                                     
                                     		$music "music/light3.ogg"
                                     	}
                                     
                                     	$room
                                     	{
                                     		$id 05
                                     		$room_file "levels/rooms_2_1/005.room"
                                     		$floor_tex "textures/rooms/2_01_floor.jpg"
                                     		$wall_tex "textures/rooms/2_04_wall.jpg"
                                     		$active_items "levels/ctrls_2_1/005_ctrl.txt"
                                     
                                     		$type "light"
                                     
                                     		$monsters "levels/rooms_2_1/005_monster.txt"
                                     		$bonuses "levels/rooms_2_1/005_bonus.txt"
                                     
                                     		$music "music/light3.ogg"
                                     	}
                                     //---------------------- 2 -----------------------------
                                     	$room
                                     	{
                                     		$id 12
                                     		$room_file "levels/rooms_2_1/012.room"
                                     		$floor_tex "textures/rooms/2_02_floor.bmp"
                                     		$wall_tex "textures/rooms/2_01_wall.jpg"
                                     		$active_items "levels/ctrls_2_1/012_ctrl.txt"
                                     
                                     		$type "light"
                                     
                                     		$monsters "levels/rooms_2_1/012_monster.txt"
                                     		$bonuses "levels/rooms_2_1/012_bonus.txt"
                                     
                                     		$music "music/light4.ogg"
                                     	}
                                     
                                     	$room
                                     	{
                                     		$id 13
                                     		$room_file "levels/rooms_2_1/013.room"
                                     		$floor_tex "textures/rooms/2_02_floor.bmp"
                                     		$wall_tex "textures/rooms/2_01_wall.jpg"
                                     		$active_items "levels/ctrls_2_1/013_ctrl.txt"
                                     
                                     		$type "light"
                                     
                                     		$monsters "levels/rooms_2_1/013_monster.txt"
                                     		$bonuses "levels/rooms_2_1/013_bonus.txt"
                                     
                                     		$music "music/light4.ogg"
                                     	}
                                     
                                     	$room
                                     	{
                                     		$id 14
                                     		$room_file "levels/rooms_2_1/014.room"
                                     		$floor_tex "textures/rooms/2_02_floor.bmp"
                                     		$wall_tex "textures/rooms/2_01_wall.jpg"
                                     		$active_items "levels/ctrls_2_1/014_ctrl.txt"
                                     
                                     		$type "light"
                                     
                                     		$monsters "levels/rooms_2_1/014_monster.txt"
                                     		$bonuses "levels/rooms_2_1/014_bonus.txt"
                                     
                                     		$music "music/light4.ogg"
                                     	}
                                     
                                     	$room
                                     	{
                                     		$id 15
                                     		$room_file "levels/rooms_2_1/015.room"
                                     		$floor_tex "textures/rooms/2_02_floor.bmp"
                                     		$wall_tex "textures/rooms/2_01_wall.jpg"
                                     		$active_items "levels/ctrls_2_1/015_ctrl.txt"
                                     
                                     		$type "light"
                                     
                                     		$monsters "levels/rooms_2_1/015_monster.txt"
                                     		$bonuses "levels/rooms_2_1/015_bonus.txt"
                                     
                                     		$music "music/light4.ogg"
                                     	}
                                     
                                     	$room
                                     	{
                                     		$id 16
                                     		$room_file "levels/rooms_2_1/016.room"
                                     		$floor_tex "textures/rooms/2_02_floor.bmp"
                                     		$wall_tex "textures/rooms/2_01_wall.jpg"
                                     		$active_items "levels/ctrls_2_1/016_ctrl.txt"
                                     
                                     		$type "light"
                                     
                                     		$monsters "levels/rooms_2_1/016_monster.txt"
                                     		$bonuses "levels/rooms_2_1/016_bonus.txt"
                                     
                                     		$demo "levels/rooms_2_1/016_demo.txt"
                                     
                                     		$music "music/light4.ogg"
                                     	}
                                     
                                     	$room
                                     	{
                                     		$id 17
                                     		$room_file "levels/rooms_2_1/017.room"
                                     		$floor_tex "textures/rooms/2_02_floor.bmp"
                                     		$wall_tex "textures/rooms/2_01_wall.jpg"
                                     		$active_items "levels/ctrls_2_1/017_ctrl.txt"
                                     
                                     		$type "light"
                                     
                                     		$monsters "levels/rooms_2_1/017_monster.txt"
                                     		$bonuses "levels/rooms_2_1/017_bonus.txt"
                                     
                                     		$demo "levels/rooms_2_1/017_demo.txt"
                                     
                                     		$music "music/light4.ogg"
                                     	}
                                     //---------------------- 3 -----------------------------
                                     	$room
                                     	{
                                     		$id 22
                                     		$room_file "levels/rooms_2_1/022.room"
                                     		$floor_tex "textures/rooms/2_02_wall.bmp"
                                     		$wall_tex "textures/rooms/2_03_wall.bmp"
                                     		$active_items "levels/ctrls_2_1/022_ctrl.txt"
                                     
                                     		$type "dark"
                                     
                                     		$monsters "levels/rooms_2_1/022_monster.txt"
                                     		$bonuses "levels/rooms_2_1/022_bonus.txt"
                                     
                                     		$music "music/dark.ogg"
                                     	}
                                     
                                     	$room
                                     	{
                                     		$id 23
                                     		$room_file "levels/rooms_2_1/023.room"
                                     		$floor_tex "textures/rooms/2_02_wall.bmp"
                                     		$wall_tex "textures/rooms/2_03_wall.bmp"
                                     		$active_items "levels/ctrls_2_1/023_ctrl.txt"
                                     
                                     		$type "secret"
                                     
                                     		$monsters "levels/rooms_2_1/023_monster.txt"
                                     		$bonuses "levels/rooms_2_1/023_bonus.txt"
                                     
                                     		$music "music/dark.ogg"
                                     	}
                                     
                                     	$room
                                     	{
                                     		$id 24
                                     		$room_file "levels/rooms_2_1/024.room"
                                     		$floor_tex "textures/rooms/2_02_wall.bmp"
                                     		$wall_tex "textures/rooms/2_03_wall.bmp"
                                     		$active_items "levels/ctrls_2_1/024_ctrl.txt"
                                     
                                     		$type "light"
                                     
                                     		$monsters "levels/rooms_2_1/024_monster.txt"
                                     		$bonuses "levels/rooms_2_1/024_bonus.txt"
                                     
                                     		$music "music/dark.ogg"
                                     	}
                                     
                                     	$room
                                     	{
                                     		$id 25
                                     		$room_file "levels/rooms_2_1/025.room"
                                     		$floor_tex "textures/rooms/2_02_wall.bmp"
                                     		$wall_tex "textures/rooms/2_03_wall.bmp"
                                     		$active_items "levels/ctrls_2_1/025_ctrl.txt"
                                     
                                     		$type "dark"
                                     
                                     		$monsters "levels/rooms_2_1/025_monster.txt"
                                     		$bonuses "levels/rooms_2_1/025_bonus.txt"
                                     
                                     		$music "music/dark.ogg"
                                     	}
                                     
                                     	$room
                                     	{
                                     		$id 26
                                     		$room_file "levels/rooms_2_1/026.room"
                                     		$floor_tex "textures/rooms/2_02_wall.bmp"
                                     		$wall_tex "textures/rooms/2_03_wall.bmp"
                                     		$active_items "levels/ctrls_2_1/026_ctrl.txt"
                                     
                                     		$type "dark"
                                     
                                     		$monsters "levels/rooms_2_1/026_monster.txt"
                                     		$bonuses "levels/rooms_2_1/026_bonus.txt"
                                     
                                     		$music "music/dark.ogg"
                                     	}
                                     
                                     	$room
                                     	{
                                     		$id 27
                                     		$room_file "levels/rooms_2_1/027.room"
                                     		$floor_tex "textures/rooms/2_02_wall.bmp"
                                     		$wall_tex "textures/rooms/2_03_wall.bmp"
                                     		$active_items "levels/ctrls_2_1/027_ctrl.txt"
                                     
                                     		$type "dark"
                                     
                                     		$monsters "levels/rooms_2_1/027_monster.txt"
                                     		$bonuses "levels/rooms_2_1/027_bonus.txt"
                                     
                                     		$music "music/dark.ogg"
                                     	}
                                     }
                                     
                                     $maze
                                     {
                                     $row "##,01,##,##,##,##"
                                     $row "##,03,04,05,##,##"
                                     $row "12,13,14,15,16,17"
                                     $row "22,23,24,25,26,27"
                                     }
                                     
                                     $player_start
                                     {
                                     //	$room_pos 1 0		//01
                                     //	$pos 20 17
                                     
                                     //	$room_pos 1 1		//03
                                     //	$pos 27.5 8
                                     
                                     //	$room_pos 2 1		//04
                                     //	$pos 1 14
                                     
                                     //	$room_pos 3 1		//05
                                     //	$pos 22 10
                                     
                                     //	$room_pos 0 2		//12
                                     //	$pos 37 6
                                     
                                     //	$room_pos 1 2		//13
                                     //	$pos 38 12
                                     
                                     //	$room_pos 2 2		//14
                                     //	$pos 35 8
                                     
                                     //	$room_pos 3 2		//15
                                     //	$pos 37 15
                                     
                                     //	$room_pos 4 2		//16
                                     //	$pos 25 12
                                     
                                     	$room_pos 5 2		//17
                                     	$pos 39 16
                                     
                                     //	$room_pos 0 3		//22
                                     //	$pos 33 10
                                     
                                     //	$room_pos 1 3		//23
                                     //	$pos 35 20
                                     
                                     //	$room_pos 2 3		//24
                                     //	$pos 15 20
                                     
                                     //	$room_pos 3 3		//25
                                     //	$pos 3 11
                                     
                                     //	$room_pos 4 3		//26
                                     //	$pos 20 20
                                     
                                     //	$room_pos 5 3		//27
                                     //	$pos 19 20
                                     }
                                     
                                     """;
    
    
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void SerializeObject()
    {
        Console.WriteLine(ObjectDumper.Dump(testObject));
        var res = ScriptSerializer.Serialize(testObject);
        Console.WriteLine(res);
        Assert.That(res, Is.EqualTo(testObjectScript));
    }
    [Test]
    public void DeserializeObject()
    {
	    var res = ScriptSerializer.Deserialize<TestObject>(testObjectScriptWithComments);
	    Console.WriteLine(testObjectScriptWithComments);
	    Console.WriteLine(ObjectDumper.Dump(res));
	    Assert.That(ObjectDumper.Dump(res), Is.EqualTo(ObjectDumper.Dump(testObject)));
    }
    [Test]
    public void DeserializeLevel()
    {
	    var res = ScriptSerializer.Deserialize<Level>(testLevelScript);
	    Console.WriteLine(ObjectDumper.Dump(res));
	    Assert.That(ObjectDumper.Dump(res), Is.EqualTo("""
	                                                   {Level}
	                                                     Header: {LevelHeader}
	                                                       Background: ""
	                                                       Bonus: false
	                                                       BonusTime: 0
	                                                       BonusCount: 0
	                                                       EnableCastle: null
	                                                       EndTitle: "COMPLETED"
	                                                       LevelName: null
	                                                       LevelNumber: 1
	                                                       LevelTime: 120
	                                                       MenuStat: "scripts/statistic_2.txt"
	                                                       Music: "music/light4.ogg"
	                                                       MusicStart: 0
	                                                       NextLevel: "levels/level_2_1_b.txt"
	                                                       QuestList: {string[1]}
	                                                         "knife"
	                                                       StartTitle: "BLUE PALACE"
	                                                       UseForDemo: false
	                                                     Rooms: {LevelRoom[16]}
	                                                       {LevelRoom}
	                                                         ActiveItems: "levels/ctrls_2_1/001_ctrl.txt"
	                                                         Bonuses: "levels/rooms_2_1/001_bonus.txt"
	                                                         Demo: false
	                                                         FloorTexture: "textures/rooms/2_01_floor.jpg"
	                                                         Id: 1
	                                                         Monsters: "levels/rooms_2_1/001_monster.txt"
	                                                         Music: "music/light3.ogg"
	                                                         RoomFile: "levels/rooms_2_1/001.room"
	                                                         Type: "secret"
	                                                         WallTexture: "textures/rooms/2_01_wall.jpg"
	                                                       {LevelRoom}
	                                                         ActiveItems: "levels/ctrls_2_1/003_ctrl.txt"
	                                                         Bonuses: "levels/rooms_2_1/003_bonus.txt"
	                                                         Demo: false
	                                                         FloorTexture: "textures/rooms/2_01_floor.jpg"
	                                                         Id: 3
	                                                         Monsters: "levels/rooms_2_1/003_monster.txt"
	                                                         Music: "music/light3.ogg"
	                                                         RoomFile: "levels/rooms_2_1/003.room"
	                                                         Type: "light"
	                                                         WallTexture: "textures/rooms/2_04_wall.jpg"
	                                                       {LevelRoom}
	                                                         ActiveItems: "levels/ctrls_2_1/004_ctrl.txt"
	                                                         Bonuses: "levels/rooms_2_1/004_bonus.txt"
	                                                         Demo: false
	                                                         FloorTexture: "textures/rooms/2_01_floor.jpg"
	                                                         Id: 4
	                                                         Monsters: "levels/rooms_2_1/004_monster.txt"
	                                                         Music: "music/light3.ogg"
	                                                         RoomFile: "levels/rooms_2_1/004.room"
	                                                         Type: "light"
	                                                         WallTexture: "textures/rooms/2_04_wall.jpg"
	                                                       {LevelRoom}
	                                                         ActiveItems: "levels/ctrls_2_1/005_ctrl.txt"
	                                                         Bonuses: "levels/rooms_2_1/005_bonus.txt"
	                                                         Demo: false
	                                                         FloorTexture: "textures/rooms/2_01_floor.jpg"
	                                                         Id: 5
	                                                         Monsters: "levels/rooms_2_1/005_monster.txt"
	                                                         Music: "music/light3.ogg"
	                                                         RoomFile: "levels/rooms_2_1/005.room"
	                                                         Type: "light"
	                                                         WallTexture: "textures/rooms/2_04_wall.jpg"
	                                                       {LevelRoom}
	                                                         ActiveItems: "levels/ctrls_2_1/012_ctrl.txt"
	                                                         Bonuses: "levels/rooms_2_1/012_bonus.txt"
	                                                         Demo: false
	                                                         FloorTexture: "textures/rooms/2_02_floor.bmp"
	                                                         Id: 12
	                                                         Monsters: "levels/rooms_2_1/012_monster.txt"
	                                                         Music: "music/light4.ogg"
	                                                         RoomFile: "levels/rooms_2_1/012.room"
	                                                         Type: "light"
	                                                         WallTexture: "textures/rooms/2_01_wall.jpg"
	                                                       {LevelRoom}
	                                                         ActiveItems: "levels/ctrls_2_1/013_ctrl.txt"
	                                                         Bonuses: "levels/rooms_2_1/013_bonus.txt"
	                                                         Demo: false
	                                                         FloorTexture: "textures/rooms/2_02_floor.bmp"
	                                                         Id: 13
	                                                         Monsters: "levels/rooms_2_1/013_monster.txt"
	                                                         Music: "music/light4.ogg"
	                                                         RoomFile: "levels/rooms_2_1/013.room"
	                                                         Type: "light"
	                                                         WallTexture: "textures/rooms/2_01_wall.jpg"
	                                                       {LevelRoom}
	                                                         ActiveItems: "levels/ctrls_2_1/014_ctrl.txt"
	                                                         Bonuses: "levels/rooms_2_1/014_bonus.txt"
	                                                         Demo: false
	                                                         FloorTexture: "textures/rooms/2_02_floor.bmp"
	                                                         Id: 14
	                                                         Monsters: "levels/rooms_2_1/014_monster.txt"
	                                                         Music: "music/light4.ogg"
	                                                         RoomFile: "levels/rooms_2_1/014.room"
	                                                         Type: "light"
	                                                         WallTexture: "textures/rooms/2_01_wall.jpg"
	                                                       {LevelRoom}
	                                                         ActiveItems: "levels/ctrls_2_1/015_ctrl.txt"
	                                                         Bonuses: "levels/rooms_2_1/015_bonus.txt"
	                                                         Demo: false
	                                                         FloorTexture: "textures/rooms/2_02_floor.bmp"
	                                                         Id: 15
	                                                         Monsters: "levels/rooms_2_1/015_monster.txt"
	                                                         Music: "music/light4.ogg"
	                                                         RoomFile: "levels/rooms_2_1/015.room"
	                                                         Type: "light"
	                                                         WallTexture: "textures/rooms/2_01_wall.jpg"
	                                                       {LevelRoom}
	                                                         ActiveItems: "levels/ctrls_2_1/016_ctrl.txt"
	                                                         Bonuses: "levels/rooms_2_1/016_bonus.txt"
	                                                         Demo: false
	                                                         FloorTexture: "textures/rooms/2_02_floor.bmp"
	                                                         Id: 16
	                                                         Monsters: "levels/rooms_2_1/016_monster.txt"
	                                                         Music: "music/light4.ogg"
	                                                         RoomFile: "levels/rooms_2_1/016.room"
	                                                         Type: "light"
	                                                         WallTexture: "textures/rooms/2_01_wall.jpg"
	                                                       {LevelRoom}
	                                                         ActiveItems: "levels/ctrls_2_1/017_ctrl.txt"
	                                                         Bonuses: "levels/rooms_2_1/017_bonus.txt"
	                                                         Demo: false
	                                                         FloorTexture: "textures/rooms/2_02_floor.bmp"
	                                                         Id: 17
	                                                         Monsters: "levels/rooms_2_1/017_monster.txt"
	                                                         Music: "music/light4.ogg"
	                                                         RoomFile: "levels/rooms_2_1/017.room"
	                                                         Type: "light"
	                                                         WallTexture: "textures/rooms/2_01_wall.jpg"
	                                                       {LevelRoom}
	                                                         ActiveItems: "levels/ctrls_2_1/022_ctrl.txt"
	                                                         Bonuses: "levels/rooms_2_1/022_bonus.txt"
	                                                         Demo: false
	                                                         FloorTexture: "textures/rooms/2_02_wall.bmp"
	                                                         Id: 22
	                                                         Monsters: "levels/rooms_2_1/022_monster.txt"
	                                                         Music: "music/dark.ogg"
	                                                         RoomFile: "levels/rooms_2_1/022.room"
	                                                         Type: "dark"
	                                                         WallTexture: "textures/rooms/2_03_wall.bmp"
	                                                       {LevelRoom}
	                                                         ActiveItems: "levels/ctrls_2_1/023_ctrl.txt"
	                                                         Bonuses: "levels/rooms_2_1/023_bonus.txt"
	                                                         Demo: false
	                                                         FloorTexture: "textures/rooms/2_02_wall.bmp"
	                                                         Id: 23
	                                                         Monsters: "levels/rooms_2_1/023_monster.txt"
	                                                         Music: "music/dark.ogg"
	                                                         RoomFile: "levels/rooms_2_1/023.room"
	                                                         Type: "secret"
	                                                         WallTexture: "textures/rooms/2_03_wall.bmp"
	                                                       {LevelRoom}
	                                                         ActiveItems: "levels/ctrls_2_1/024_ctrl.txt"
	                                                         Bonuses: "levels/rooms_2_1/024_bonus.txt"
	                                                         Demo: false
	                                                         FloorTexture: "textures/rooms/2_02_wall.bmp"
	                                                         Id: 24
	                                                         Monsters: "levels/rooms_2_1/024_monster.txt"
	                                                         Music: "music/dark.ogg"
	                                                         RoomFile: "levels/rooms_2_1/024.room"
	                                                         Type: "light"
	                                                         WallTexture: "textures/rooms/2_03_wall.bmp"
	                                                       {LevelRoom}
	                                                         ActiveItems: "levels/ctrls_2_1/025_ctrl.txt"
	                                                         Bonuses: "levels/rooms_2_1/025_bonus.txt"
	                                                         Demo: false
	                                                         FloorTexture: "textures/rooms/2_02_wall.bmp"
	                                                         Id: 25
	                                                         Monsters: "levels/rooms_2_1/025_monster.txt"
	                                                         Music: "music/dark.ogg"
	                                                         RoomFile: "levels/rooms_2_1/025.room"
	                                                         Type: "dark"
	                                                         WallTexture: "textures/rooms/2_03_wall.bmp"
	                                                       {LevelRoom}
	                                                         ActiveItems: "levels/ctrls_2_1/026_ctrl.txt"
	                                                         Bonuses: "levels/rooms_2_1/026_bonus.txt"
	                                                         Demo: false
	                                                         FloorTexture: "textures/rooms/2_02_wall.bmp"
	                                                         Id: 26
	                                                         Monsters: "levels/rooms_2_1/026_monster.txt"
	                                                         Music: "music/dark.ogg"
	                                                         RoomFile: "levels/rooms_2_1/026.room"
	                                                         Type: "dark"
	                                                         WallTexture: "textures/rooms/2_03_wall.bmp"
	                                                       {LevelRoom}
	                                                         ActiveItems: "levels/ctrls_2_1/027_ctrl.txt"
	                                                         Bonuses: "levels/rooms_2_1/027_bonus.txt"
	                                                         Demo: false
	                                                         FloorTexture: "textures/rooms/2_02_wall.bmp"
	                                                         Id: 27
	                                                         Monsters: "levels/rooms_2_1/027_monster.txt"
	                                                         Music: "music/dark.ogg"
	                                                         RoomFile: "levels/rooms_2_1/027.room"
	                                                         Type: "dark"
	                                                         WallTexture: "textures/rooms/2_03_wall.bmp"
	                                                     Maze: {string[4]}
	                                                       "##,01,##,##,##,##"
	                                                       "##,03,04,05,##,##"
	                                                       "12,13,14,15,16,17"
	                                                       "22,23,24,25,26,27"
	                                                     PlayerStart: {LevelPlayerStart}
	                                                       RoomPos: {Vector2}
	                                                   
	                                                       Pos: {Vector2}
	                                                   
	                                                   """));
    }
}