namespace AzangaraTools.Script;

public class ScriptReader(List<ScriptToken> tokens)
{
    private readonly List<ScriptToken> _tokens = tokens;
    private int _pos;
    
    public ScriptToken Current => _tokens[_pos];

    public ScriptToken Consume()
    {
        var tok = _tokens[_pos++];
        return tok;
    }

    public ScriptToken Consume(ScriptTokenType excepted)
    {
        return Current.Type != excepted ? 
            throw new Exception($"Excepted {excepted} but got '{Current.Value}' at {Current.Line}:{Current.Col}") : 
            Consume();
    }

    public void SkipNewLines()
    {
        while (Current.Type == ScriptTokenType.NewLine) _pos++;
    }
    
    public bool IsAtBlockStart => Current.Type == ScriptTokenType.LBrace;
}