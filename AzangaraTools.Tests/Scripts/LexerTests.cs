using AzangaraTools.Script;

namespace AzangaraTools.Tests.Scripts;

public class LexerTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GeneralTokenize()
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
        var tokens = new ScriptLexer(src).Tokenize();
        Assert.That(tokens.Count, Is.EqualTo(30));
        Assert.That(tokens.Select(x=>x.Type), Is.EqualTo([
            ScriptTokenType.NewLine,
            ScriptTokenType.Identifier, ScriptTokenType.LBrace, ScriptTokenType.NewLine,
            ScriptTokenType.Identifier, ScriptTokenType.String, ScriptTokenType.NewLine,
            ScriptTokenType.Identifier, ScriptTokenType.Integer, ScriptTokenType.NewLine,
            ScriptTokenType.Identifier, ScriptTokenType.Float, ScriptTokenType.NewLine,
            ScriptTokenType.Identifier, ScriptTokenType.Float, ScriptTokenType.Float, ScriptTokenType.NewLine,
            ScriptTokenType.Identifier, ScriptTokenType.LBrace, ScriptTokenType.NewLine,
            ScriptTokenType.Identifier, ScriptTokenType.String, ScriptTokenType.NewLine,
            ScriptTokenType.Identifier, ScriptTokenType.String, ScriptTokenType.NewLine,
            ScriptTokenType.RBrace, ScriptTokenType.NewLine,
            ScriptTokenType.RBrace, ScriptTokenType.EOF,
        ]));
    }
}