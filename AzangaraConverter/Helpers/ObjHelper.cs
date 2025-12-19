using AzangaraTools.Models;

namespace AzangaraConverter.Helpers;
using System.Globalization;

public class ObjHelper
{
    public static void WriteFrameToObj(Frame frame, Stream s)
    {
        using var writer = new StreamWriter(s);

        writer.WriteLine("# Generated OBJ");
        writer.WriteLine();

        // ---- Write vertices (v x y z) ----
        foreach (var v in frame.Vertices)
        {
            writer.WriteLine($"v {v.Pos.X.ToString(CultureInfo.InvariantCulture)} {v.Pos.Y.ToString(CultureInfo.InvariantCulture)} {v.Pos.Z.ToString(CultureInfo.InvariantCulture)}");
        }

        writer.WriteLine();

        // ---- Write UVs (vt u v) ----
        foreach (var v in frame.Vertices)
        {
            writer.WriteLine($"vt {v.U.ToString(CultureInfo.InvariantCulture)} {(1-v.V).ToString(CultureInfo.InvariantCulture)}");
        }

        writer.WriteLine();

        // ---- Write normals (vn x y z) ----
        foreach (var v in frame.Vertices)
        {
            writer.WriteLine($"vn {v.Normal.X.ToString(CultureInfo.InvariantCulture)} {v.Normal.Y.ToString(CultureInfo.InvariantCulture)} {v.Normal.Z.ToString(CultureInfo.InvariantCulture)}");
        }

        writer.WriteLine();

        // ---- Faces (triangles) ----
        // OBJ format is 1-based indexing
        for (int i = 0; i < frame.Indices.Length; i += 3)
        {
            int a = frame.Indices[i] + 1;
            int b = frame.Indices[i + 1] + 1;
            int c = frame.Indices[i + 2] + 1;

            // f v/vt/vn
            writer.WriteLine($"f {a}/{a}/{a} {b}/{b}/{b} {c}/{c}/{c}");
        }
    }
}