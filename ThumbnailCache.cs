using System.Security.Cryptography;
using System.Text;

namespace GreatValueArchivesManager;

internal static class ThumbnailCache
{
    private static readonly string CacheDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "GreatValueArchivesManager",
        "ThumbnailCache");

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
            return new Bitmap(image);
        }
        catch
        {
            TryDelete(path);
            return null;
        }
    }

    public static void Save(string cacheKey, Bitmap bitmap)
    {
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
            if (Directory.Exists(CacheDirectory))
            {
                Directory.Delete(CacheDirectory, recursive: true);
            }
        }
        catch
        {
        }
    }

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
