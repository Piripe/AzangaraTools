using AzangaraTools;
using AzangaraTools.Models;
using StbImageSharp;
using ColorComponents = StbImageWriteSharp.ColorComponents;

namespace  AzangaraRoomToObj;
// RoomToObjConverter.cs (Header‑aware version)
// Reads geometry offsets directly from the Azangara .room header.
// Usage: dotnet run -- <input.room> <output.obj> <output_lm.png>

using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Png;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            //var fs = File.OpenRead(args[0]);
            Console.WriteLine("Folder: ");
            var fr = FolderReader.ReadFolder("/media/hdd/Games/Azangara/Azangara/" ?? Console.ReadLine() ?? Environment.CurrentDirectory);
            Console.WriteLine(string.Join('\n', fr.LoadedFiles.Keys.Where(x => x.StartsWith("models"))));
            Console.WriteLine("Room file: ");
            var room = fr.GetRoom("levels/rooms_6/001.room" ?? Console.ReadLine() ?? "levels/rooms_6/001.room"); //Room.Load(fr);
            
            Console.WriteLine(fr.LoadedFiles.ContainsKey("models/lit_horz_03.mmd"));

            //fs.Close();

            //Console.WriteLine(string.Join('\n',room.RoomGeometry.Select(x=>new string(x.Select(y=>(char)y.Item).ToArray()))));

            WriteFrameToObj(room.Geometry.Frames[0], "geometry.obj");
            WriteFrameToObj(room.GeometryBack.Frames[0], "back.obj");
            WriteFrameToObj(room.GeometryLightMap.Frames[0], "lightmap.obj");
            WriteFrameToObj(
                MergeFrames(room.GetStatics().Select(x =>
                    new Frame(x.Model.Frames[0].GetTransformedVertices(x.Definition.Transform),
                        x.Model.Frames[0].Indices)).ToArray()), "statics.obj");
            foreach (var x in room.GetStatics())
            {
                var xfs = File.OpenWrite(x.TexturePath.Replace('/','_'));
                new StbImageWriteSharp.ImageWriter().WritePng(x.Texture.Data, 256, 256,
                    ColorComponents.RedGreenBlueAlpha, xfs);
                xfs.Close();
            }

            var fs = File.OpenWrite("lm.png");
            new StbImageWriteSharp.ImageWriter().WritePng(room.BitmapLightMap.Data, 256, 256,
                ColorComponents.RedGreenBlueAlpha, fs);
            fs.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    static Frame MergeFrames(params Frame[] frames)
    {
        var offset = 0;
        return new Frame(
            frames.SelectMany(x=>x.Vertices).ToArray(), 
            frames.SelectMany(x =>
                {
                    var offset1 = offset;
                    offset += x.Vertices.Length;
                    return x.Indices.Select(y => (ushort)(y + offset1));
                }).ToArray());
    }
    
    public static void WriteFrameToObj(Frame frame, string filePath)
    {
        using var writer = new StreamWriter(filePath);

        writer.WriteLine("# Generated OBJ");
        writer.WriteLine();

        // ---- Write vertices (v x y z) ----
        foreach (var v in frame.Vertices)
        {
            writer.WriteLine($"v {v.Pos.X} {v.Pos.Y} {v.Pos.Z}");
        }

        writer.WriteLine();

        // ---- Write UVs (vt u v) ----
        foreach (var v in frame.Vertices)
        {
            writer.WriteLine($"vt {v.U} {1-v.V}");
        }

        writer.WriteLine();

        // ---- Write normals (vn x y z) ----
        foreach (var v in frame.Vertices)
        {
            writer.WriteLine($"vn {v.Normal.X} {v.Normal.Y} {v.Normal.Z}");
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
