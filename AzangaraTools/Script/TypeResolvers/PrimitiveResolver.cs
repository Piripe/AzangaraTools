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

    public object? Read(Type type, ScriptReader reader)
    {
        throw new NotImplementedException();
    }
}