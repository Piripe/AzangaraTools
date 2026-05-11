namespace AzangaraTools.Script;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ScriptIgnoreAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property)]
public sealed class ScriptPropertyName(string name)
{
    public string Name { get; } = name;
} 