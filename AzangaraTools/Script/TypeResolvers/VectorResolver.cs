using System.Globalization;
using System.Numerics;

namespace AzangaraTools.Script.TypeResolvers;

public class VectorResolver : ITypeResolver
{
    private static readonly HashSet<Type> _types =
    [
        typeof(Vector2), typeof(Vector3)
    ];
    public bool CanHandle(Type type) => _types.Contains(type);

    public void Write(object value, ScriptWriter writer, int depth)
    {
        switch (value)
        {
            case Vector2 v2:
                writer.WriteFloat(v2.X);
                writer.WriteFloat(v2.Y);
                break;
            case Vector3 v3:
                writer.WriteFloat(v3.X);
                writer.WriteFloat(v3.Y);
                writer.WriteFloat(v3.Z);
                break;
                
        }
    }

    public object? Read(Type type, ScriptReader reader, int depth)
    {
        float ReadFloat() => float.Parse(reader.Consume().Value, CultureInfo.InvariantCulture);
        return type switch
        {
            _ when type == typeof(Vector2) => new Vector2(ReadFloat(), ReadFloat()),
            _ when type == typeof(Vector3) => new Vector3(ReadFloat(), ReadFloat(), ReadFloat()),
            _ => throw new Exception("Unsupported primitive type: {type}")
        };
    }
}