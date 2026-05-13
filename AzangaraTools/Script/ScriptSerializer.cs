using System.Text;

namespace AzangaraTools.Script;

public class ScriptSerializer
{
    public static void Serialize<T>(Stream stream, T obj) where T : class
    {
        var writer = new ScriptWriter(stream);
        
        var resolver = TypeResolverRegistry.Find(obj.GetType());
        if (resolver == null) throw new Exception("Can't find resolver for root object");
        
        resolver.Write(obj, writer, 0);
    }

    public static string Serialize<T>(T obj) where T : class
    {
        var writer = new ScriptWriter(new MemoryStream());
        
        var resolver = TypeResolverRegistry.Find(obj.GetType());
        if (resolver == null) throw new Exception("Can't find resolver for root object");
        
        resolver.Write(obj, writer, 0);
        return writer.ToString();
    }

    public static T? Deserialize<T>(Stream script) where T : class
    {
        var reader = new ScriptReader(new ScriptLexer(script).Tokenize());
        
        var resolver = TypeResolverRegistry.Find(typeof(T));
        if (resolver == null) throw new Exception("Can't find resolver for object");
        
        return (T?)resolver.Read(typeof(T), reader, 0);
    }

    public static T? Deserialize<T>(string script) where T : class
    {
        var reader = new ScriptReader(new ScriptLexer(script).Tokenize());
        
        var resolver = TypeResolverRegistry.Find(typeof(T));
        if (resolver == null) throw new Exception("Can't find resolver for object");
        
        return (T?)resolver.Read(typeof(T), reader, 0);
    }
}