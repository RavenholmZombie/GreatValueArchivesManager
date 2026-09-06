using System.Security.Cryptography;
using System.Text;

namespace GreatValueArchivesManager;

internal static class ThumbnailCache
{
    private static readonly string CacheDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "GreatValueArchivesManager",
        "ThumbnailCache-v2");

    public static Bitmap? TryLoad(string cacheKey)
    {
        string path = GetCachePath(cacheKey);
        if (!File.Exists(path))
        {
            return null;
        }

        try
        {
            using FileStream stream = new(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using Image image = Image.FromStream(stream);
            Bitmap bitmap = new(image);

            // Older builds could accidentally cache the generic IMAGE placeholder as if
            // it were a successful thumbnail. Never return one of those as real content.
            if (LooksLikePlaceholder(bitmap))
            {
                bitmap.Dispose();
                TryDelete(path);
                return null;
            }

            return bitmap;
        }
        catch
        {
            TryDelete(path);
            return null;
        }
    }

    public static void Save(string cacheKey, Bitmap bitmap)
    {
        // A failed decode must never poison the persistent cache with our placeholder.
        if (LooksLikePlaceholder(bitmap))
        {
            return;
        }

        try
        {
            Directory.CreateDirectory(CacheDirectory);
            string path = GetCachePath(cacheKey);
            string tempPath = path + ".tmp";

            bitmap.Save(tempPath, System.Drawing.Imaging.ImageFormat.Png);
            File.Move(tempPath, path, overwrite: true);
        }
        catch
        {
            // Thumbnail caching is only an optimization. Never fail archive browsing over it.
        }
    }

    public static void Clear()
    {
        try
        {
            string appDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "GreatValueArchivesManager");

            if (!Directory.Exists(appDirectory))
            {
                return;
            }

            foreach (string directory in Directory.EnumerateDirectories(appDirectory, "ThumbnailCache*"))
            {
                try
                {
                    Directory.Delete(directory, recursive: true);
                }
                catch
                {
                }
            }
        }
        catch
        {
        }
    }

    private static bool LooksLikePlaceholder(Bitmap bitmap)
    {
        if (bitmap.Width != 160 || bitmap.Height != 120)
        {
            return false;
        }

        // CreatePlaceholder() paints this exact dark background and border. Sampling
        // several quiet pixels avoids mistaking a normal 160x120 image for the placeholder.
        Color background = Color.FromArgb(37, 37, 38);
        Color border = Color.FromArgb(62, 62, 66);

        return SameRgb(bitmap.GetPixel(10, 10), background) &&
               SameRgb(bitmap.GetPixel(149, 10), background) &&
               SameRgb(bitmap.GetPixel(10, 109), background) &&
               SameRgb(bitmap.GetPixel(0, 0), border);
    }

    private static bool SameRgb(Color left, Color right) =>
        left.R == right.R && left.G == right.G && left.B == right.B;

    private static string GetCachePath(string cacheKey)
    {
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(cacheKey));
        return Path.Combine(CacheDirectory, Convert.ToHexString(hash) + ".png");
    }

    private static void TryDelete(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch
        {
        }
    }
}