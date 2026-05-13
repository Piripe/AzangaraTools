using System.Reflection;

namespace AzangaraTools.Script.TypeResolvers;

public class ObjectResolver : ITypeResolver
{
    public bool CanHandle(Type type) => !type.IsPrimitive && type != typeof(string) && type.IsClass;

    public void Write(object value, ScriptWriter writer, int depth)
    {
        if (depth > 0) writer.WriteBlockStart();

        foreach (var prop in GetProperties(value.GetType()))
        {
            var propValue = prop.GetValue(value);
            if (propValue == null)  continue;
            
            var resolver = GetResolverForProperty(prop);
            if (resolver == null) continue;
            
            string fieldName = GetFieldName(prop);
            
            writer.WriteIdentifier(fieldName);
            resolver.Write(propValue, writer, depth + 1);
            writer.WriteNewLine();
        }
        
        if (depth > 0) writer.WriteBlockEnd();
    }

    public object? Read(Type type, ScriptReader reader, int depth)
    {
        var instance = Activator.CreateInstance(type)
            ?? throw new Exception($"Can't create instance of {type}");
        var propMap = GetProperties(type).ToDictionary(GetFieldName);

        reader.SkipNewLines();
        if (depth > 0)
        {
            reader.Consume(ScriptTokenType.LBrace);
            reader.SkipNewLines();
        }

        while (reader.Current.Type != ScriptTokenType.RBrace && reader.Current.Type != ScriptTokenType.EOF)
        {
            if (!propMap.TryGetValue(reader.Consume(ScriptTokenType.Identifier).Value, out var prop)) continue;
            
            var resolver = GetResolverForProperty(prop);
            if (resolver == null) continue;
            
            prop.SetValue(instance, resolver.Read(prop.PropertyType, reader, depth + 1));
            reader.SkipNewLines();
        }

        reader.Consume(depth > 0 ? ScriptTokenType.RBrace : ScriptTokenType.EOF);

        return instance;
    }
    
    private static IEnumerable<PropertyInfo> GetProperties(Type type) =>
        type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => 
                p.CanRead && 
                p.CanWrite && 
                p.GetCustomAttributes(typeof(ScriptIgnoreAttribute), false).Length == 0
                );

    private static string GetFieldName(PropertyInfo prop)
    {
        var attr = prop.GetCustomAttributes(typeof(ScriptPropertyNameAttribute), false)
            .FirstOrDefault() as ScriptPropertyNameAttribute;
        return attr?.Name ?? prop.Name;
    }

    private static ITypeResolver? GetResolverForProperty(PropertyInfo prop)
    {
        var resolver = TypeResolverRegistry.Find(prop.PropertyType);

        if (resolver is not CollectionResolver) return resolver;
        
        var attr = prop.GetCustomAttributes(typeof(ScriptArrayItemAttribute), false)
            .FirstOrDefault() as ScriptArrayItemAttribute;

        return new CollectionResolver(attr);

    }
}