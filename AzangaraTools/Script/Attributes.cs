namespace AzangaraTools.Script;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ScriptIgnoreAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property)]
public sealed class ScriptPropertyNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
} 
[AttributeUsage(AttributeTargets.Property)]
public sealed class ScriptArrayItemAttribute(string elementName) : Attribute
{
    public string ElementName { get; } = elementName;
} 
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface)]
public sealed class ScriptPolymorphicAttribute(bool elementNameInArray = true) : Attribute
{
    public bool ElementNameInArray { get; } = elementNameInArray;
} 
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true)]
public sealed class ScriptDerivedTypeAttribute(Type type, string descriptor) : Attribute
{
    public Type Type { get; } = type;
    public string Descriptor { get; } = descriptor;
} 