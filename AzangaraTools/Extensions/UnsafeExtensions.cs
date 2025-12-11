using System.Runtime.InteropServices;
using System.Text;

namespace AzangaraTools.Extensions;

internal static unsafe class UnsafeExtensions
{
    internal static string GetString(this IntPtr source, int length)
    {
        
        var arr = new byte[length];
        Marshal.Copy(source, arr, 0, length);
        return Encoding.UTF8.GetString(arr[..Array.IndexOf(arr, (byte)0)]);
    }
}