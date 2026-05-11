using System.Diagnostics;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using AzangaraTools.Script;

namespace AzangaraTools.Tests.Scripts;

public class ParserTests
{

    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void GeneralParser()
    {
        var src = """
                  // Test file
                  $test {
                      $string "Test string"
                      $int 1
                      $float -1.5
                      $vec_two 1.5 -5.0
                      $array {
                         $item "one"
                         $item "two"
                      }
                  }
                  """;
        var document = new ScriptParser(new ScriptLexer(src).Tokenize()).ParseDocument();

        Console.WriteLine(ObjectDumper.Dump(document));
        
        Assert.That(ObjectDumper.Dump(document), Is.EqualTo(ObjectDumper.Dump(new ScriptDocument([
            new ScriptBlockNode("test", [
                new ScriptFieldNode("string", new ScriptStringNode("Test string")),
                new ScriptFieldNode("int", new ScriptIntNode(1)),
                new ScriptFieldNode("float", new ScriptFloatNode(-1.5f)),
                new ScriptFieldNode("vec_two", new ScriptVec2Node(new Vector2(1.5f, -5.0f))),
                new ScriptBlockNode("array",
                [
                    new ScriptFieldNode("item", new ScriptStringNode("one")),
                    new ScriptFieldNode("item", new ScriptStringNode("two")),
                ]),
            ])
        ]))));
    }
}