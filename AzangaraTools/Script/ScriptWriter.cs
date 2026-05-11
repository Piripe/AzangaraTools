using System.Globalization;
using System.Text;

namespace AzangaraTools.Script;

public class ScriptWriter
{
    private readonly StringBuilder _sb = new();
    private int _indent = 0;
    private const string IndentUnit = "\t";
    
    private string Pad => string.Concat(Enumerable.Repeat(IndentUnit, _indent));
    
    public void WriteIdentifier(string name)
    {
        _sb.Append($"{Pad}${name} ");
    }

    public void WriteNewLine()
    {
        _sb.AppendLine();
    }

    public void WriteLBrace()
    {
        _sb.Append($"{Pad}{{");
    }
    public void WriteRBrace()
    {
        _sb.Append($"{Pad}}}");
    }

    public void WriteBlockStart(string name)
    {
        WriteIdentifier(name);
        WriteNewLine();
        WriteLBrace();
        WriteNewLine();
        _indent++;
    }
    public void WriteBlockEnd()
    {
        _indent--;
        WriteRBrace();
        WriteNewLine();
    }
    public void WriteString(string text)
    {
        _sb.Append($"\"{text}\" ");
    }
    public void WriteInt(int value)
    {
        _sb.Append($"{value} ");
    }
    public void WriteFloat(float value)
    {
        _sb.Append($"{value.ToString(CultureInfo.InvariantCulture)} ");
    }

    public override string ToString() => _sb.ToString();
}