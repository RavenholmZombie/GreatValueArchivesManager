using System.Drawing;
using SixLabors.ImageSharp;

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

        using SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(bytes);
        using MemoryStream pngStream = new();
        image.SaveAsPng(pngStream);
        pngStream.Position = 0;

        using System.Drawing.Image decoded = System.Drawing.Image.FromStream(pngStream);
        return new Bitmap(decoded);
    }
}
