using FluentFTP;
using System.Security.Authentication;

namespace GreatValueArchivesManager;

public sealed record ArchiveItem(
    string FileName,
    string Category,
    string FolderName,
    string RemotePath,
    bool IsVideo,
    string? PublicUrl);

public sealed class ArchiveFtpClient : IAsyncDisposable
{
    private static readonly string[] ImageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp", ".avif"];
    private static readonly string[] VideoExtensions = [".mp4", ".webm", ".ogg", ".mov", ".m4v"];
    private const int MaxDiscoveryDepth = 6;
    private const int MaxDiscoveryDirectories = 250;
    private const int ConnectionHealthTimeoutMs = 3000;

    public static readonly IReadOnlyDictionary<string, string> CategoryFolders =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Food"] = "Food",
            ["Beverages"] = "Beverages",
            ["Non-Food Items"] = "Non-Food-Items",
            ["Archive Datasheets"] = "PADS",
            ["Special Submissions"] = "Special",
            ["Unsorted Archive Submissions"] = "Misc",
            ["Concepts"] = "Concepts",
            ["Videos"] = "Videos"
        };

    private readonly AsyncFtpClient _client;

    public ArchiveFtpClient(string host, string username, string password, bool useTls, int port = 21)
    {
        Host = NormalizeHost(host);
        Port = port;
        UseTls = useTls;

        _client = new AsyncFtpClient(Host, username, password, port);
        _client.Config.EncryptionMode = useTls ? FtpEncryptionMode.Explicit : FtpEncryptionMode.None;
        _client.Config.DataConnectionEncryption = useTls;
        _client.Config.DataConnectionType = FtpDataConnectionType.AutoPassive;
        _client.Config.SslProtocols = SslProtocols.None;
        _client.Config.ConnectTimeout = 15000;
        _client.Config.ReadTimeout = 15000;
        _client.Config.DataConnectionConnectTimeout = 15000;
        _client.Config.DataConnectionReadTimeout = 15000;
        _client.Config.RetryAttempts = 2;

        // Namecheap can silently close an idle FTP control connection while the
        // manager is busy doing HTTP thumbnail work. FluentFTP can test the
        // control channel before commands and reconnect automatically when needed.
        _client.Config.NoopTestConnectivity = true;
    }

    public string Host { get; }
    public int Port { get; }
    public bool UseTls { get; }
    public string? MediaRoot { get; private set; }

    public async Task ConnectAndDiscoverAsync(CancellationToken cancellationToken = default)
    {
        await _client.Connect(cancellationToken);

        string[] preferredCandidates =
        [
            "/public_html/viewer/media",
            "/viewer/media",
            "/media",
            "/"
        ];

        foreach (string candidate in preferredCandidates)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (await LooksLikeArchiveRootAsync(candidate, cancellationToken))
            {
                MediaRoot = NormalizeRemoteDirectory(candidate);
                return;
            }
        }

        string? discovered = await FindArchiveRootRecursivelyAsync(cancellationToken);
        if (!string.IsNullOrWhiteSpace(discovered))
        {
            MediaRoot = NormalizeRemoteDirectory(discovered);
            return;
        }

        throw new InvalidOperationException(
            $"FTP login succeeded, but the Archive Viewer media directory could not be found. " +
            $"The manager searched up to {MaxDiscoveryDepth} directory levels and {MaxDiscoveryDirectories} directories, " +
            "looking for the Great Value Archives category-folder signature (Food, Beverages, Non-Food-Items, PADS, Misc, Concepts, etc.).");
    }

    private async Task<string?> FindArchiveRootRecursivelyAsync(CancellationToken cancellationToken)
    {
        Queue<(string Path, int Depth)> pending = new();
        HashSet<string> visited = new(StringComparer.OrdinalIgnoreCase);
        pending.Enqueue(("/", 0));

        int examined = 0;

        while (pending.Count > 0 && examined < MaxDiscoveryDirectories)
        {
            cancellationToken.ThrowIfCancellationRequested();

            (string path, int depth) = pending.Dequeue();
            path = NormalizeRemoteDirectory(path);

            if (!visited.Add(path))
            {
                continue;
            }

            examined++;

            FtpListItem[] listing;
            try
            {
                listing = await _client.GetListing(path, cancellationToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                continue;
            }

            if (HasArchiveFolderSignature(listing.Select(item => item.Name)))
            {
                return path;
            }

            if (depth >= MaxDiscoveryDepth)
            {
                continue;
            }

            IEnumerable<FtpListItem> directories = listing
                .Where(item => item.Type == FtpObjectType.Directory)
                .Where(item => item.Name is not "." and not "..")
                .OrderByDescending(item => GetDiscoveryPriority(item.Name))
                .ThenBy(item => item.Name, StringComparer.OrdinalIgnoreCase);

            foreach (FtpListItem directory in directories)
            {
                string child = NormalizeRemoteDirectory(directory.FullName);
                if (!visited.Contains(child))
                {
                    pending.Enqueue((child, depth + 1));
                }
            }
        }

        return null;
    }

    private static bool HasArchiveFolderSignature(IEnumerable<string> names)
    {
        HashSet<string> set = new(names, StringComparer.OrdinalIgnoreCase);

        if (!set.Contains("Food") || !set.Contains("Beverages"))
        {
            return false;
        }

        string[] supportingFolders = ["Non-Food-Items", "PADS", "Special", "Misc", "Concepts", "Videos"];
        return supportingFolders.Count(set.Contains) >= 2;
    }

    private static int GetDiscoveryPriority(string name)
    {
        return name.ToLowerInvariant() switch
        {
            "public_html" => 100,
            "www" => 95,
            "gvarchive.com" => 90,
            "viewer" => 85,
            "media" => 80,
            "htdocs" => 75,
            "httpdocs" => 70,
            _ => 0
        };
    }

    public async Task<IReadOnlyList<ArchiveItem>> ListCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        EnsureReady();
        await EnsureConnectionAliveAsync(cancellationToken);

        if (category.Equals("Overview", StringComparison.OrdinalIgnoreCase))
        {
            List<ArchiveItem> all = [];
            foreach ((string displayName, _) in CategoryFolders)
            {
                all.AddRange(await ListCategoryAsync(displayName, cancellationToken));
            }
            return all.OrderBy(i => i.FileName, StringComparer.OrdinalIgnoreCase).ToArray();
        }

        if (category.Equals("Trash", StringComparison.OrdinalIgnoreCase))
        {
            string trashPath = CombineRemote(MediaRoot!, ".Trash");
            if (!await _client.DirectoryExists(trashPath, cancellationToken))
            {
                return [];
            }

            return await ListItemsFromFolderAsync("Trash", ".Trash", trashPath, cancellationToken);
        }

        if (!CategoryFolders.TryGetValue(category, out string? folderName))
        {
            return [];
        }

        string folderPath = CombineRemote(MediaRoot!, folderName);
        return await ListItemsFromFolderAsync(category, folderName, folderPath, cancellationToken);
    }

    public async Task UploadFileAsync(string localFilePath, string category, CancellationToken cancellationToken = default)
    {
        EnsureReady();
        await EnsureConnectionAliveAsync(cancellationToken);

        string folderName = GetFolderName(category);
        string remotePath = CombineRemote(MediaRoot!, folderName, Path.GetFileName(localFilePath));

        if (await _client.FileExists(remotePath, cancellationToken))
        {
            throw new IOException($"A file named '{Path.GetFileName(remotePath)}' already exists in the destination.");
        }

        FtpStatus status = await _client.UploadFile(localFilePath, remotePath, FtpRemoteExists.Skip, true, FtpVerify.None, null, cancellationToken);
        if (status != FtpStatus.Success)
        {
            throw new IOException($"FTP upload failed for '{Path.GetFileName(localFilePath)}'.");
        }
    }

    public async Task RenameAsync(ArchiveItem item, string newFileName, CancellationToken cancellationToken = default)
    {
        EnsureReady();
        await EnsureConnectionAliveAsync(cancellationToken);
        ValidateFileName(newFileName);

        string target = CombineRemote(GetRemoteDirectory(item.RemotePath), newFileName);

        if (await _client.FileExists(target, cancellationToken))
        {
            throw new IOException($"A file named '{newFileName}' already exists in the destination.");
        }

        await _client.MoveFile(item.RemotePath, target, FtpRemoteExists.Skip, cancellationToken);
    }

    public async Task MoveAsync(ArchiveItem item, string targetCategory, CancellationToken cancellationToken = default)
    {
        EnsureReady();
        await EnsureConnectionAliveAsync(cancellationToken);

        string folderName = GetFolderName(targetCategory);
        string target = CombineRemote(MediaRoot!, folderName, item.FileName);

        if (await _client.FileExists(target, cancellationToken))
        {
            throw new IOException($"A file named '{item.FileName}' already exists in the destination.");
        }

        await _client.MoveFile(item.RemotePath, target, FtpRemoteExists.Skip, cancellationToken);
    }

    public async Task MoveToTrashAsync(ArchiveItem item, CancellationToken cancellationToken = default)
    {
        EnsureReady();
        await EnsureConnectionAliveAsync(cancellationToken);

        string trashPath = CombineRemote(MediaRoot!, ".Trash");
        if (!await _client.DirectoryExists(trashPath, cancellationToken))
        {
            await _client.CreateDirectory(trashPath, true, cancellationToken);
        }

        string candidate = CombineRemote(trashPath, item.FileName);
        if (await _client.FileExists(candidate, cancellationToken))
        {
            string stem = Path.GetFileNameWithoutExtension(item.FileName);
            string extension = Path.GetExtension(item.FileName);
            candidate = CombineRemote(trashPath, $"{stem}_{DateTime.Now:yyyyMMdd_HHmmss}{extension}");
        }

        await _client.MoveFile(item.RemotePath, candidate, FtpRemoteExists.Skip, cancellationToken);
    }

    public async Task PermanentlyDeleteAsync(ArchiveItem item, CancellationToken cancellationToken = default)
    {
        EnsureReady();
        await EnsureConnectionAliveAsync(cancellationToken);
        await _client.DeleteFile(item.RemotePath, cancellationToken);
    }

    public async Task<byte[]> DownloadFileAsync(string remotePath, CancellationToken cancellationToken = default)
    {
        EnsureReady();
        await EnsureConnectionAliveAsync(cancellationToken);

        using MemoryStream ms = new();
        bool downloaded = await _client.DownloadStream(ms, remotePath, 0, null, cancellationToken);
        if (!downloaded)
        {
            throw new IOException($"FTP download failed for '{Path.GetFileName(remotePath)}'.");
        }
        return ms.ToArray();
    }

    private async Task EnsureConnectionAliveAsync(CancellationToken cancellationToken)
    {
        bool alive = false;

        try
        {
            alive = await _client.IsStillConnected(ConnectionHealthTimeoutMs, cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            alive = false;
        }

        if (alive)
        {
            return;
        }

        cancellationToken.ThrowIfCancellationRequested();
        await _client.Connect(cancellationToken);
    }

    private async Task<IReadOnlyList<ArchiveItem>> ListItemsFromFolderAsync(
        string category,
        string folderName,
        string folderPath,
        CancellationToken cancellationToken)
    {
        FtpListItem[] listing = await _client.GetListing(folderPath, cancellationToken);
        List<ArchiveItem> items = [];

        foreach (FtpListItem entry in listing.Where(entry => entry.Type == FtpObjectType.File))
        {
            string extension = Path.GetExtension(entry.Name).ToLowerInvariant();
            bool isImage = ImageExtensions.Contains(extension);
            bool isVideo = VideoExtensions.Contains(extension);
            if (!isImage && !isVideo)
            {
                continue;
            }

            string? publicUrl = category.Equals("Trash", StringComparison.OrdinalIgnoreCase)
                ? null
                : BuildPublicUrl(folderName, entry.Name);

            items.Add(new ArchiveItem(
                entry.Name,
                category,
                folderName,
                NormalizeRemoteDirectory(entry.FullName),
                isVideo,
                publicUrl));
        }

        return items.OrderBy(i => i.FileName, StringComparer.OrdinalIgnoreCase).ToArray();
    }

    private async Task<bool> LooksLikeArchiveRootAsync(string path, CancellationToken cancellationToken)
    {
        try
        {
            if (!await _client.DirectoryExists(path, cancellationToken))
            {
                return false;
            }

            FtpListItem[] listing = await _client.GetListing(path, cancellationToken);
            return HasArchiveFolderSignature(listing.Where(item => item.Type == FtpObjectType.Directory).Select(item => item.Name));
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return false;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_client.IsConnected)
        {
            await _client.Disconnect(CancellationToken.None);
        }
        _client.Dispose();
    }

    private static string BuildPublicUrl(string folderName, string fileName) =>
        $"https://gvarchive.com/viewer/media/{Uri.EscapeDataString(folderName)}/{Uri.EscapeDataString(fileName)}";

    private string GetFolderName(string category)
    {
        if (category.Equals("Trash", StringComparison.OrdinalIgnoreCase))
        {
            return ".Trash";
        }

        if (!CategoryFolders.TryGetValue(category, out string? folderName))
        {
            throw new ArgumentException($"'{category}' is not a writable archive category.", nameof(category));
        }
        return folderName;
    }

    private void EnsureReady()
    {
        if (string.IsNullOrWhiteSpace(MediaRoot))
        {
            throw new InvalidOperationException("The FTP client has not connected to the archive yet.");
        }
    }

    private static string GetRemoteDirectory(string remotePath)
    {
        string normalized = remotePath.Replace('\\', '/');
        int slash = normalized.LastIndexOf('/');
        return slash <= 0 ? "/" : normalized[..slash];
    }

    private static string CombineRemote(params string[] parts) =>
        "/" + string.Join('/', parts.SelectMany(p => p.Replace('\\', '/').Split('/', StringSplitOptions.RemoveEmptyEntries)));

    private static string NormalizeRemoteDirectory(string path)
    {
        string normalized = "/" + string.Join('/', path.Replace('\\', '/').Split('/', StringSplitOptions.RemoveEmptyEntries));
        return normalized.Length == 0 ? "/" : normalized;
    }

    private static void ValidateFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName) || fileName != Path.GetFileName(fileName) || fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            throw new ArgumentException("Enter a valid file name without a path.", nameof(fileName));
        }
    }

    private static string NormalizeHost(string host)
    {
        string value = host.Trim();
        if (Uri.TryCreate(value, UriKind.Absolute, out Uri? uri) && !string.IsNullOrWhiteSpace(uri.Host))
        {
            value = uri.Host;
        }
        return value.Trim('/');
    }
}
