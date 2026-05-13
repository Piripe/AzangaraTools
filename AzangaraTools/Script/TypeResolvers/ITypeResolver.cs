namespace AzangaraTools.Script.TypeResolvers;

public interface ITypeResolver
{
    bool CanHandle(Type type);
    void Write(object value, ScriptWriter writer, int depth);
    object? Read(Type type, ScriptReader reader, int depth);
}