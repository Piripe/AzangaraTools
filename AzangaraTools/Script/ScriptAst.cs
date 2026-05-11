using System.Numerics;

namespace AzangaraTools.Script;

// Base node
public abstract record ScriptNode();

// Leaf values
public record ScriptIntNode(int Value) : ScriptNode;
public record ScriptFloatNode(float Value) : ScriptNode;
public record ScriptVec2Node(Vector2 Value) : ScriptNode;
public record ScriptVec3Node(Vector3 Value) : ScriptNode;
public record ScriptVec2INode(Vector2 Value) : ScriptNode;
public record ScriptVec3INode(Vector3 Value) : ScriptNode;
public record ScriptStringNode(string Value) : ScriptNode;
public record ScriptIdentifierNode(string Name) : ScriptNode;

// Compound nodes
public record ScriptFieldNode(string Key, ScriptNode Value) : ScriptNode;
public record ScriptBlockNode(string Key, List<ScriptNode> Value) : ScriptNode;
public record ScriptDocument(List<ScriptNode> Nodes) : ScriptNode;