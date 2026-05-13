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