using System.Globalization;

namespace AzangaraTools.Script.TypeResolvers;

public class PrimitiveResolver : ITypeResolver
{
    private static readonly HashSet<Type> _types =
    [
        typeof(int), typeof(long), typeof(short), typeof(byte),
        typeof(uint), typeof(ulong), typeof(ushort),
        typeof(float), typeof(double), typeof(decimal),
        typeof(bool), typeof(string), typeof(char)
    ];
    public bool CanHandle(Type type) => _types.Contains(type);

    public void Write(object value, ScriptWriter writer, int depth)
    {
        switch (value)
        {
            case float:
            case double:
            case decimal:
                writer.WriteFloat((float)value);
                break;
            case int:
            case long:
            case short:
            case byte:
            case uint:
            case ulong:
            case ushort:
                writer.WriteInt((int)value);
                break;
            default:
                writer.WriteString(value.ToString()??"");
                break;
                
        }
    }

    public object? Read(Type type, ScriptReader reader, int depth)
    {
        var tok = reader.Consume();
        return type switch
        {
            _ when type == typeof(string) => tok.Value,
            _ when type == typeof(int) => int.Parse(tok.Value),
            _ when type == typeof(uint) => uint.Parse(tok.Value),
            _ when type == typeof(long) => long.Parse(tok.Value),
            _ when type == typeof(ulong) => ulong.Parse(tok.Value),
            _ when type == typeof(short) => short.Parse(tok.Value),
            _ when type == typeof(ushort) => ushort.Parse(tok.Value),
            _ when type == typeof(byte) || type == typeof(char) => byte.Parse(tok.Value),
            _ when type == typeof(bool) => bool.TryParse(tok.Value, out var b) ? b : int.TryParse(tok.Value, out var i) ? i == 1 : null,
            _ when type == typeof(float) => float.Parse(tok.Value, CultureInfo.InvariantCulture),
            _ when type == typeof(double) => double.Parse(tok.Value, CultureInfo.InvariantCulture),
            _ when type == typeof(decimal) => decimal.Parse(tok.Value, CultureInfo.InvariantCulture),
            _ => throw new Exception($"Unsupported primitive type: {type}")
        };
    }
}