using System.Diagnostics;
using System.Text;

namespace AzangaraTools.Script;

public class ScriptLexer
{
    private readonly Stream _stream;
    private int _line = 1, _col = 1;

    public ScriptLexer(Stream source)
    {
        _stream = source;
        Current = (char)_stream.ReadByte();
    }
    public ScriptLexer(string source)
    {
        _stream = new MemoryStream(Encoding.UTF8.GetBytes(source));
        Current = (char)_stream.ReadByte();
    }
    
    private char Current { get; set; }// _pos < _src.Length ? _sr.BaseStream _src[_pos] : '\0';
    private char Peek(int offset = 1)
    {
        _stream.Seek(offset-1, SeekOrigin.Current);
        char result = (char)_stream.ReadByte();
        _stream.Seek(-offset, SeekOrigin.Current);
        return result;
        //return (_pos + offset) < _src.Length ? _src[_pos + offset] : '\0';
    }

    private char Advance()
    {
        char c = Current;
        _col++;
        Current = (char)_stream.ReadByte();
        
        if (c == '\n')
        {
            _line++; _col = 1;}
        return c;
    }

    public List<ScriptToken> Tokenize()
    {
        var tokens = new List<ScriptToken>();

        while (_stream.Position <= _stream.Length)
        {
            if (_stream.Position > _stream.Length) break;
            SkipWhitespace();
            int line = _line, col = _col;
            char c = Current;
            if (c == (char)0xffff) break;

            if (c == '/' && Peek() == '/')
            {
                SkipComment();
                continue;
            }

            ScriptToken? tok = c switch
            {
                '"' => ReadString(line, col),
                '{' => Make(ScriptTokenType.LBrace, "{", line, col),
                '}' => Make(ScriptTokenType.RBrace, "}", line, col),
                '$' => ReadIdentifier(line, col),
                '\n' => Make(ScriptTokenType.NewLine, "\n", line, col),
                _ => char.IsDigit(c) || c == '-' ? ReadNumber(line,col)
                    : throw new Exception($"Unexcepted char '{c}' at {line}:{col}")
            };
            
            if (tok != null) tokens.Add(tok);
        }
        
        tokens.Add(new ScriptToken(ScriptTokenType.EOF, "", _line, _col));
        return tokens;
    }

    private ScriptToken Make(ScriptTokenType type, string val, int line, int col)
    {
        Advance();
        return new ScriptToken(type, val, line, col);
    }

    private void SkipWhitespace()
    {
        while (_stream.Position <= _stream.Length && Current is ' ' or '\t' or '\r') Advance();
    }

    private void SkipComment()
    {
        while (_stream.Position <= _stream.Length && Current != '\n' && Current != (char)0xffff) Advance();
    }

    private ScriptToken ReadIdentifier(int line, int col)
    {
        Advance(); // skip starting $
        
        if (!char.IsLetter(Current) && Current != '_')
            throw new Exception($"Excepted identifier after '$' at {line}:{col}");
        
        var sb = new StringBuilder();
        while (_stream.Position <= _stream.Length && (char.IsLetterOrDigit(Current)  || Current == '_'))
        {
            sb.Append(Advance());
        }

        return new ScriptToken(ScriptTokenType.Identifier, sb.ToString(), line, col);
    }
    
    private ScriptToken ReadString(int line,  int col)
    {
        Advance(); // skip "
        var sb = new StringBuilder();
        while (_stream.Position <= _stream.Length && Current != '"')
        {
            sb.Append(Advance());
        }
        Advance(); // skip "
        return new ScriptToken(ScriptTokenType.String, sb.ToString(), line, col);
    }

    private ScriptToken ReadNumber(int line, int col)
    {
        var sb = new StringBuilder();
        if (Current == '-') sb.Append(Advance());
        while (_stream.Position <= _stream.Length && char.IsDigit(Current)) sb.Append(Advance());

        bool isFloat = Current == '.' && char.IsDigit(Peek());
        if (isFloat)
        {
            sb.Append(Advance());
            while (_stream.Position <= _stream.Length && char.IsDigit(Current)) sb.Append(Advance());
        }
        
        return new ScriptToken(isFloat ? ScriptTokenType.Float : ScriptTokenType.Integer, sb.ToString(), line, col);
    }
}