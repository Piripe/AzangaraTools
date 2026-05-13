using System.Collections;

namespace AzangaraTools.Script.TypeResolvers;

public class CollectionResolver : ITypeResolver
{
    private readonly ScriptArrayItemAttribute _itemAttribute;
    public CollectionResolver(ScriptArrayItemAttribute? attribute = null)
    {
        _itemAttribute = attribute ?? new ScriptArrayItemAttribute("item");   
    }
    public bool CanHandle(Type type) => GetElementType(type) != null;

    public void Write(object value, ScriptWriter writer, int depth)
    {
        var elementType = GetElementType(value.GetType())!;
        var resolver = TypeResolverRegistry.Find(elementType);
        if (resolver == null) return;

        var items = (IEnumerable)value;
        
        if (depth > 0) writer.WriteBlockStart();
        foreach (var item in items)
        {
            writer.WriteIdentifier(_itemAttribute.ElementName);
            resolver.Write(item, writer, depth + 1);
            writer.WriteNewLine();
        }
        if (depth > 0) writer.WriteBlockEnd();
    }

    public object? Read(Type type, ScriptReader reader, int depth)
    {
        var elementType = GetElementType(type)!;
        var resolver = TypeResolverRegistry.Find(elementType)
                       ?? throw new Exception($"No resolver for type {elementType}");

        var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType))!;
        
        
        reader.SkipNewLines();
        if (depth > 0)
        {
            reader.Consume(ScriptTokenType.LBrace);
            reader.SkipNewLines();
        }
        
        while (reader.Current.Type != ScriptTokenType.RBrace && reader.Current.Type != ScriptTokenType.EOF)
        {
            if (reader.Consume(ScriptTokenType.Identifier).Value != _itemAttribute.ElementName) continue;
            list.Add(resolver.Read(elementType, reader, depth + 1));
            reader.SkipNewLines();
        }
        
        reader.Consume(depth > 0 ? ScriptTokenType.RBrace : ScriptTokenType.EOF);

        if (!type.IsArray) return list;
        
        var arr = Array.CreateInstance(elementType, list.Count);
        list.CopyTo(arr, 0);
        return arr;

    }

    private static Type? GetElementType(Type type)
    {
        if (type.IsArray) return type.GetElementType();
        if (!type.IsGenericType) return null;
        
        var def = type.GetGenericTypeDefinition();
        if (def == typeof(List<>) || def == typeof(IList<>) ||
            def == typeof(IEnumerable<>) || def == typeof(ICollection<>))
            return type.GetGenericArguments()[0];
        
        return null;
    }
}