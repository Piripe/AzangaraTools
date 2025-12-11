using System.Numerics;
using AzangaraTools.Structs;

namespace AzangaraTools.Models;

public class Frame(Vertice[] vertices, ushort[] indices)
{
    public Vertice[] Vertices { get; set; } = vertices;
    public ushort[] Indices { get; set; } = indices;

    public Vertice[] GetTransformedVertices(Matrix4x4 matrix)
    {
        return Vertices.Select(x => x with { Pos = Vector3.Transform(x.Pos, matrix) }).ToArray();
    }
}