using AzangaraTools.Script.TypeResolvers;

namespace AzangaraTools.Script;

public static class TypeResolverRegistry
{
    private static readonly List<ITypeResolver> _resolvers =
    [
    ];
    
    public static ITypeResolver? Find(Type type) => _resolvers.FirstOrDefault(x => x.CanHandle(type));
}