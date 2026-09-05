using System.Drawing;
using Imazen.WebP;

namespace WebP.Net;

internal static class WebPDecoder
{
    public static Bitmap Decode(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);

        if (bytes.Length == 0)
        {
            throw new ArgumentException("The WebP image data is empty.", nameof(bytes));
        }

        var decoder = new SimpleDecoder();
        return decoder.DecodeFromBytes(bytes, bytes.LongLength);
    }
}
