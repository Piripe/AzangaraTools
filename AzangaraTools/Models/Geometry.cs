using AzangaraTools.Extensions;
using AzangaraTools.Structs;

namespace AzangaraTools.Models;

public class Geometry
{
    public GeometryHeader Header { get; set; }
    public Frame[] Frames { get; set; } = null!;

    public static Geometry Load(Stream stream)
    {
        Geometry geom = new Geometry();
        
        geom.Header = stream.ReadStruct<GeometryHeader>();

        geom.Frames = Enumerable.Repeat(0, geom.Header.Frames).Select(_ =>
        new Frame(stream.ReadArray<Vertice>(geom.Header.VCount),stream.ReadArray<ushort>(geom.Header.ICount))
        ).ToArray();
        
        return geom;
    }

    public void Export(Stream stream)
    {
        stream.WriteStruct(Header with
        {
            Frames = Frames.Length,
            VCount = Frames[0].Vertices.Length,
            ICount = Frames[0].Indices.Length
        });
        foreach (var x in Frames)
        {
            foreach (var y in x.Vertices)
            {
                stream.WriteStruct(y);
            }
            foreach (var y in x.Indices)
            {
                stream.WriteStruct(y);
            }
        }
    }
}