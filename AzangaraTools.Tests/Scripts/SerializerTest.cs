using System.Xml.Serialization;
using AzangaraTools.Script;

namespace AzangaraTools.Tests.Scripts;

public class SerializerTest
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
        [ScriptPropertyName("array"), ScriptArrayItem("element")]
        public string[] Array { get; set; }
        [ScriptPropertyName("int_dict")]
        public Dictionary<string, int> IntDict { get; set; }
    }
    
    
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void SerializeObject()
    {
        var src = new TestObject()
        {
            Test = new TestObject2
            {
                String = "Test string",
                Int = 1,
                Float = 1.5f,
                Array = [
                    "one", "two"
                ],
                IntDict = new() {["one"] = 1, ["two"] = 2}
            }
        };
        Console.WriteLine(ObjectDumper.Dump(src));
        Console.WriteLine(ScriptSerializer.Serialize(src));
    }
    [Test]
    public void DeserializeObject()
    {
        var src = """
                  // comment
                  $test 
                  {
                  	$string "Test string" 
                  	$int 1 
                  	$float 1.5 
                  	
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
        Console.WriteLine(src);
        Console.WriteLine(ObjectDumper.Dump(ScriptSerializer.Deserialize<TestObject>(src)));
    }
}