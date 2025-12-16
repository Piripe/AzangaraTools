using System.Runtime.InteropServices;

namespace AzangaraTools.Extensions;

internal static class StreamExtensions
{
    internal static T ReadStruct<T>(this Stream stream) where T : struct
    {
        Span<byte> buffer = stackalloc byte[Marshal.SizeOf<T>()];
        stream.ReadExactly(buffer);
        return MemoryMarshal.Cast<byte, T>(buffer)[0];
    }
    internal static void WriteStruct<T>(this Stream stream, T obj) where T : struct
    {
        Span<byte> buffer = stackalloc byte[Marshal.SizeOf<T>()];
        stream.Write(MemoryMarshal.Cast<T, byte>([obj]));
    }
    internal static void Write2DArray<T>(this Stream stream, T[][] obj) where T : struct
    {
        foreach (var x in obj)
        {
            
            foreach (var y in x)
            {
                WriteStruct(stream, y);
            }
        }
    }
    internal static T[] ReadArray<T>(this Stream stream, int count = 1) where T : struct
    {
        int size = Marshal.SizeOf<T>() * count;
        var buffer = new byte[size];
        stream.ReadExactly(buffer, 0, size);
    
        /*if (bytesRead < size)
        {
            throw new EndOfStreamException($"Could not read the entire array. Expected {size} bytes, but read {bytesRead} bytes.");
        }*/
    
        var array = new T[count];
        MemoryMarshal.Cast<byte, T>(buffer).CopyTo(array);
        return array;
    }
    internal static T[][] Read2DArray<T>(this Stream stream, int rows, int columns) where T : struct
    {
        int elementSize = Marshal.SizeOf<T>();
        int totalSize = elementSize * rows * columns;
    
        var buffer = new byte[totalSize];
        int totalBytesRead = 0;
        while (totalBytesRead < totalSize)
        {
            int bytesRead = stream.Read(buffer, totalBytesRead, totalSize - totalBytesRead);
            if (bytesRead == 0) break;
            totalBytesRead += bytesRead;
        }
    
        if (totalBytesRead < totalSize)
        {
            throw new EndOfStreamException($"Could not read the entire 2D array. Expected {totalSize} bytes, but read {totalBytesRead} bytes.");
        }
    
        var array = new T[rows][];
        Span<T> flatSpan = MemoryMarshal.Cast<byte, T>(buffer);
    
        for (int i = 0; i < rows; i++)
        {
            array[i] = new T[columns];
            flatSpan.Slice(i * columns, columns).CopyTo(array[i]);
        }
    
        return array;
    }
}