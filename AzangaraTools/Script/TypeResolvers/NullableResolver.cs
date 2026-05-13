namespace AzangaraTools.Script.TypeResolvers;

public class NullableResolver : ITypeResolver
{
    public bool CanHandle(Type type) => Nullable.GetUnderlyingType(type) != null;

    public void Write(object value, ScriptWriter writer, int depth)
    {
        var inner = Nullable.GetUnderlyingType(value.GetType())!;
        TypeResolverRegistry.Find(inner)?.Write(value, writer, depth);
    }

    public object? Read(Type type, ScriptReader reader, int depth)
    {
        var inner = Nullable.GetUnderlyingType(type)!;
        return TypeResolverRegistry.Find(inner)?.Read(inner, reader, depth);
    }
}