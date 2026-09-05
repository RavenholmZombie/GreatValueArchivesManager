using System.Net;
using System.Text;

namespace GreatValueArchivesManager;

internal sealed record ArchiveItem(
    string FileName,
    string Category,
    string FolderName,
    string RemotePath,
    bool IsVideo,
    string? PublicUrl);

internal sealed class ArchiveFtpClient
{
    private static readonly string[] ImageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp", ".avif"];
    private static readonly string[] VideoExtensions = [".mp4", ".webm", ".ogg", ".mov", ".m4v"];

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

    private readonly NetworkCredential _credentials;

    public ArchiveFtpClient(string host, string username, string password, bool useTls, int port = 21)
    {
        Host = NormalizeHost(host);
        Port = port;
        UseTls = useTls;
        _credentials = new NetworkCredential(username, password);
    }

    public string Host { get; }
    public int Port { get; }
    public bool UseTls { get; }
    public string? MediaRoot { get; private set; }

    public async Task ConnectAndDiscoverAsync(CancellationToken cancellationToken = default)
    {
        await ListDirectoryNamesAsync("/", cancellationToken);

        string[] candidates =
        [
            "/public_html/viewer/media",
            "/viewer/media",
            "/media",
            "/"
        ];

        foreach (string candidate in candidates)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await LooksLikeArchiveRootAsync(candidate, cancellationToken))
            {
                MediaRoot = candidate == "/" ? "/" : candidate.TrimEnd('/');
                return;
            }
        }

        throw new InvalidOperationException(
            "FTP login succeeded, but the Archive Viewer media directory could not be found. " +
            "The manager looked for public_html/viewer/media, viewer/media, media, and an FTP account rooted directly in the media directory.");
    }

    public async Task<IReadOnlyList<ArchiveItem>> ListCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        EnsureReady();

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
            if (!await DirectoryExistsAsync(trashPath, cancellationToken))
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
        string folderName = GetFolderName(category);
        string remotePath = CombineRemote(MediaRoot!, folderName, Path.GetFileName(localFilePath));
        await EnsureTargetDoesNotExistAsync(remotePath, cancellationToken);

        await using FileStream input = File.OpenRead(localFilePath);
        FtpWebRequest request = CreateRequest(remotePath, WebRequestMethods.Ftp.UploadFile);
        request.ContentLength = input.Length;

        await using Stream output = await request.GetRequestStreamAsync().WaitAsync(cancellationToken);
        await input.CopyToAsync(output, cancellationToken);
        await output.FlushAsync(cancellationToken);
        output.Close();

        using FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync().WaitAsync(cancellationToken);
    }

    public async Task RenameAsync(ArchiveItem item, string newFileName, CancellationToken cancellationToken = default)
    {
        EnsureReady();
        ValidateFileName(newFileName);
        string target = CombineRemote(GetRemoteDirectory(item.RemotePath), newFileName);
        await EnsureTargetDoesNotExistAsync(target, cancellationToken);
        await RenameRemoteAsync(item.RemotePath, target, cancellationToken);
    }

    public async Task MoveAsync(ArchiveItem item, string targetCategory, CancellationToken cancellationToken = default)
    {
        EnsureReady();
        string folderName = GetFolderName(targetCategory);
        string target = CombineRemote(MediaRoot!, folderName, item.FileName);
        await EnsureTargetDoesNotExistAsync(target, cancellationToken);
        await RenameRemoteAsync(item.RemotePath, target, cancellationToken);
    }

    public async Task MoveToTrashAsync(ArchiveItem item, CancellationToken cancellationToken = default)
    {
        EnsureReady();
        string trashPath = CombineRemote(MediaRoot!, ".Trash");
        await EnsureDirectoryAsync(trashPath, cancellationToken);

        string candidate = CombineRemote(trashPath, item.FileName);
        if (await FileExistsAsync(candidate, cancellationToken))
        {
            string stem = Path.GetFileNameWithoutExtension(item.FileName);
            string extension = Path.GetExtension(item.FileName);
            candidate = CombineRemote(trashPath, $"{stem}_{DateTime.Now:yyyyMMdd_HHmmss}{extension}");
        }

        await RenameRemoteAsync(item.RemotePath, candidate, cancellationToken);
    }

    public async Task PermanentlyDeleteAsync(ArchiveItem item, CancellationToken cancellationToken = default)
    {
        FtpWebRequest request = CreateRequest(item.RemotePath, WebRequestMethods.Ftp.DeleteFile);
        using FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync().WaitAsync(cancellationToken);
    }

    public async Task<byte[]> DownloadFileAsync(string remotePath, CancellationToken cancellationToken = default)
    {
        FtpWebRequest request = CreateRequest(remotePath, WebRequestMethods.Ftp.DownloadFile);
        using FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync().WaitAsync(cancellationToken);
        await using Stream responseStream = response.GetResponseStream();
        using MemoryStream ms = new();
        await responseStream.CopyToAsync(ms, cancellationToken);
        return ms.ToArray();
    }

    private async Task<IReadOnlyList<ArchiveItem>> ListItemsFromFolderAsync(
        string category,
        string folderName,
        string folderPath,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<string> names = await ListDirectoryNamesAsync(folderPath, cancellationToken);
        List<ArchiveItem> items = [];

        foreach (string name in names)
        {
            string extension = Path.GetExtension(name).ToLowerInvariant();
            bool isImage = ImageExtensions.Contains(extension);
            bool isVideo = VideoExtensions.Contains(extension);
            if (!isImage && !isVideo)
            {
                continue;
            }

            string? publicUrl = category.Equals("Trash", StringComparison.OrdinalIgnoreCase)
                ? null
                : BuildPublicUrl(folderName, name);

            items.Add(new ArchiveItem(
                name,
                category,
                folderName,
                CombineRemote(folderPath, name),
                isVideo,
                publicUrl));
        }

        return items.OrderBy(i => i.FileName, StringComparer.OrdinalIgnoreCase).ToArray();
    }

    private async Task<bool> LooksLikeArchiveRootAsync(string path, CancellationToken cancellationToken)
    {
        try
        {
            IReadOnlyList<string> names = await ListDirectoryNamesAsync(path, cancellationToken);
            HashSet<string> set = new(names, StringComparer.OrdinalIgnoreCase);
            return set.Contains("Food") && set.Contains("Beverages");
        }
        catch (WebException ex) when (IsMissingPath(ex))
        {
            return false;
        }
    }

    private async Task<bool> DirectoryExistsAsync(string path, CancellationToken cancellationToken)
    {
        try
        {
            await ListDirectoryNamesAsync(path, cancellationToken);
            return true;
        }
        catch (WebException ex) when (IsMissingPath(ex))
        {
            return false;
        }
    }

    private async Task EnsureDirectoryAsync(string path, CancellationToken cancellationToken)
    {
        if (await DirectoryExistsAsync(path, cancellationToken))
        {
            return;
        }

        FtpWebRequest request = CreateRequest(path, WebRequestMethods.Ftp.MakeDirectory);
        using FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync().WaitAsync(cancellationToken);
    }

    private async Task EnsureTargetDoesNotExistAsync(string remotePath, CancellationToken cancellationToken)
    {
        if (await FileExistsAsync(remotePath, cancellationToken))
        {
            throw new IOException($"A file named '{Path.GetFileName(remotePath)}' already exists in the destination.");
        }
    }

    private async Task<bool> FileExistsAsync(string remotePath, CancellationToken cancellationToken)
    {
        try
        {
            FtpWebRequest request = CreateRequest(remotePath, WebRequestMethods.Ftp.GetFileSize);
            using FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync().WaitAsync(cancellationToken);
            return true;
        }
        catch (WebException ex) when (IsMissingPath(ex))
        {
            return false;
        }
    }

    private async Task RenameRemoteAsync(string sourceRemotePath, string targetRemotePath, CancellationToken cancellationToken)
    {
        FtpWebRequest request = CreateRequest(sourceRemotePath, WebRequestMethods.Ftp.Rename);
        request.RenameTo = targetRemotePath.TrimStart('/');
        using FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync().WaitAsync(cancellationToken);
    }

    private async Task<IReadOnlyList<string>> ListDirectoryNamesAsync(string remotePath, CancellationToken cancellationToken)
    {
        FtpWebRequest request = CreateRequest(remotePath, WebRequestMethods.Ftp.ListDirectory);
        using FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync().WaitAsync(cancellationToken);
        await using Stream stream = response.GetResponseStream();
        using StreamReader reader = new(stream, Encoding.UTF8);

        List<string> names = [];
        while (!reader.EndOfStream)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string? line = await reader.ReadLineAsync().WaitAsync(cancellationToken);
            if (!string.IsNullOrWhiteSpace(line) && line is not "." and not "..")
            {
                names.Add(line.Trim());
            }
        }
        return names;
    }

#pragma warning disable SYSLIB0014
    private FtpWebRequest CreateRequest(string remotePath, string method)
    {
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(BuildUri(remotePath));
        request.Method = method;
        request.Credentials = _credentials;
        request.EnableSsl = UseTls;
        request.UseBinary = true;
        request.UsePassive = true;
        request.KeepAlive = false;
        request.Timeout = 20000;
        request.ReadWriteTimeout = 30000;
        return request;
    }
#pragma warning restore SYSLIB0014

    private Uri BuildUri(string remotePath)
    {
        string path = remotePath.Replace('\\', '/').Trim('/');
        string escaped = string.Join('/', path.Split('/', StringSplitOptions.RemoveEmptyEntries).Select(Uri.EscapeDataString));
        return new Uri($"ftp://{Host}:{Port}/{escaped}");
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

    private static void ValidateFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName) || fileName != Path.GetFileName(fileName) || fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            throw new ArgumentException("Enter a valid file name without a path.", nameof(fileName));
        }
    }

    private static bool IsMissingPath(WebException ex) =>
        ex.Response is FtpWebResponse ftp && ftp.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable;

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
