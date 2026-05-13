using AzangaraTools.Script.TypeResolvers;

namespace AzangaraTools.Script;

public static class TypeResolverRegistry
{
    private static readonly List<ITypeResolver> _resolvers =
    [
        new PrimitiveResolver(),
        new VectorResolver(),
        new CollectionResolver(),
        new DictionaryResolver(),
        new NullableResolver(),
        new ObjectResolver()
    ];
    
    public static ITypeResolver? Find(Type type) => _resolvers.FirstOrDefault(x => x.CanHandle(type));
}