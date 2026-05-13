using System.Globalization;
using System.Text;

namespace AzangaraTools.Script;

public class ScriptWriter(Stream stream)
{
    private readonly StreamWriter _sw = new(stream);
    private int _indent = 0;
    private const string IndentUnit = "\t";
    
    private string Pad => string.Concat(Enumerable.Repeat(IndentUnit, _indent));
    
    public void WriteIdentifier(string name)
    {
        _sw.Write($"{Pad}${name} ");
    }

    public void WriteNewLine()
    {
        _sw.WriteLine();
    }

    public void WriteLBrace()
    {
        _sw.Write($"{Pad}{{");
    }
    public void WriteRBrace()
    {
        _sw.Write($"{Pad}}}");
    }

    public void WriteBlockStart()
    {
        WriteNewLine();
        WriteLBrace();
        WriteNewLine();
        _indent++;
    }
    public void WriteBlockEnd()
    {
        _indent--;
        WriteRBrace();
    }
    public void WriteString(string text)
    {
        _sw.Write($"\"{text}\" ");
    }
    public void WriteInt(int value)
    {
        _sw.Write($"{value} ");
    }
    public void WriteFloat(float value)
    {
        _sw.Write($"{value.ToString(CultureInfo.InvariantCulture)} ");
    }

    public override string ToString()
    {
        _sw.Flush();
        stream.Seek(0, SeekOrigin.Begin);
        return new StreamReader(stream).ReadToEnd();
    }
}