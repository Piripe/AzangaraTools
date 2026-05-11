namespace AzangaraTools.Script;

public enum ScriptTokenType
{
    Identifier, String, Integer, Float,
    LBrace, RBrace,
    NewLine, EOF,
}
public record ScriptToken(ScriptTokenType Type, string Value, int Line, int Col);