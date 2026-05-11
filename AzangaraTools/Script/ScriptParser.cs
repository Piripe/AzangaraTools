using System.Globalization;
using System.Numerics;

namespace AzangaraTools.Script;

public class ScriptParser(List<ScriptToken> tokens)
{
    private readonly List<ScriptToken> _tokens = tokens;
    private int _pos;
    
    private ScriptToken Current => _tokens[_pos];
    private ScriptToken Peek(int offset = 1) => _tokens[Math.Min(_pos + offset, _tokens.Count - 1)];

    private ScriptToken Consume(ScriptTokenType excepted)
    {
        if (Current.Type != excepted)
            throw new Exception($"Excepted {excepted} but got '{Current.Value}' at {Current.Line}:{Current.Col}");
        return _tokens[_pos++];
    }
    
    private void SkipNewLines()
    {
        while (Current.Type == ScriptTokenType.NewLine) _pos++;
    }

    public ScriptDocument ParseDocument()
    {
        var nodes = new List<ScriptNode>();
        SkipNewLines();

        while (Current.Type != ScriptTokenType.EOF)
        {
            nodes.Add(ParseNode());
            SkipNewLines();
        }
        return new ScriptDocument(nodes);
    }

    private ScriptNode ParseNode()
    {
        string key = Consume(ScriptTokenType.Identifier).Value;
        
        SkipNewLines();

        if (Current.Type == ScriptTokenType.LBrace)
        {
            _pos++; // skip {
            SkipNewLines();
            var nodes = new List<ScriptNode>();

            while (Current.Type is not ScriptTokenType.EOF and not ScriptTokenType.RBrace)
            {
                nodes.Add(ParseNode());
                SkipNewLines();
            }
            
            Consume(ScriptTokenType.RBrace);
            return new ScriptBlockNode(key, nodes);
        }
        else
        {
            return new ScriptFieldNode(key, ParseValue());
        }
    }

    private ScriptNode ParseValue() => Current.Type switch
    {
        ScriptTokenType.Integer => ParseNumber(ScriptTokenType.Integer),
        ScriptTokenType.Float => ParseNumber(ScriptTokenType.Float),
        ScriptTokenType.String => new ScriptStringNode(Consume(ScriptTokenType.String).Value),
        _ => throw new Exception($"Unexpected token '{Current.Value}' at {Current.Line}:{Current.Col}")
    };

    private ScriptNode ParseNumber(ScriptTokenType excepted)
    {
        var number = Consume(excepted).Value;
        if (Current.Type == excepted)
        {
            var number2 = Consume(excepted).Value;
            if (Peek(2).Type == excepted)
            {
                var number3 = Consume(excepted).Value;
                return new ScriptVec3Node(new Vector3(
                    float.Parse(number, CultureInfo.InvariantCulture),
                    float.Parse(number2, CultureInfo.InvariantCulture),
                    float.Parse(number3, CultureInfo.InvariantCulture)
                    ));
            }

            return new ScriptVec2Node(new Vector2(
                float.Parse(number, CultureInfo.InvariantCulture),
                float.Parse(number2, CultureInfo.InvariantCulture)
            ));
        }

        return excepted == ScriptTokenType.Integer ? 
            new ScriptIntNode(int.Parse(number)) : 
            new ScriptFloatNode(float.Parse(number, CultureInfo.InvariantCulture));
    }
}