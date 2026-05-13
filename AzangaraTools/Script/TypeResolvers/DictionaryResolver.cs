using System.Collections;

namespace AzangaraTools.Script.TypeResolvers;

public class DictionaryResolver : ITypeResolver
{
    public bool CanHandle(Type type) =>
        type.IsGenericType &&
        (type.GetGenericTypeDefinition() == typeof(IDictionary<,>) ||
        type.GetGenericTypeDefinition() == typeof(Dictionary<,>)) &&
        type.GetGenericArguments()[0] == typeof(string);

    public void Write(object value, ScriptWriter writer, int depth)
    {
        var valueType = value.GetType().GetGenericArguments()[1];
        var resolver = TypeResolverRegistry.Find(valueType);
        if (resolver == null) return;
        
        var dict = (IDictionary)value;
        
        if (depth > 0) writer.WriteBlockStart();

        foreach (DictionaryEntry entry in dict)
        {
            if (entry.Value == null) continue;
            
            writer.WriteIdentifier(entry.Key.ToString()!);
            resolver.Write(entry.Value, writer, depth + 1);
            writer.WriteNewLine();
        }
        
        if (depth > 0)writer.WriteBlockEnd();
    }

    public object? Read(Type type, ScriptReader reader, int depth)
    {
        var valueType = type.GetGenericArguments()[1];
        
        var resolver = TypeResolverRegistry.Find(valueType)
                       ?? throw new Exception($"No resolver for type {valueType}");

        var dictType = typeof(Dictionary<,>).MakeGenericType(typeof(string), valueType);
        var dict = (IDictionary)Activator.CreateInstance(dictType)!;
        
        
        reader.SkipNewLines();
        if (depth > 0)
        {
            reader.Consume(ScriptTokenType.LBrace);
            reader.SkipNewLines();
        }

        while (reader.Current.Type != ScriptTokenType.RBrace && reader.Current.Type != ScriptTokenType.EOF)
        {
            dict[reader.Consume(ScriptTokenType.Identifier).Value] = resolver.Read(valueType, reader, depth + 1);
            reader.SkipNewLines();
        }

        reader.Consume(depth > 0 ? ScriptTokenType.RBrace : ScriptTokenType.EOF);

        return dict;
    }
}