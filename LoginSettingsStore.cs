using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace GreatValueArchivesManager;

internal sealed record SavedLoginSettings(
    string Host,
    string Username,
    bool UseTls,
    bool RememberCredentials,
    string Password);

internal static class LoginSettingsStore
{
    private const string CredentialTarget = "GreatValueArchivesManager.Ftp";
    private static readonly string SettingsDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "GreatValueArchivesManager");
    private static readonly string SettingsPath = Path.Combine(SettingsDirectory, "login.json");

    public static SavedLoginSettings Load()
    {
        LoginPreferences preferences = LoadPreferences();
        string password = string.Empty;
        string username = preferences.Username ?? string.Empty;

        if (preferences.RememberCredentials && TryReadCredential(out string savedUsername, out string savedPassword))
        {
            if (!string.IsNullOrWhiteSpace(savedUsername))
            {
                username = savedUsername;
            }
            password = savedPassword;
        }

        return new SavedLoginSettings(
            preferences.Host ?? "gvarchive.com",
            username,
            preferences.UseTls,
            preferences.RememberCredentials,
            password);
    }

    public static void Save(string host, string username, string password, bool useTls, bool rememberCredentials)
    {
        Directory.CreateDirectory(SettingsDirectory);

        LoginPreferences preferences = new()
        {
            Host = host,
            Username = username,
            UseTls = useTls,
            RememberCredentials = rememberCredentials
        };

        File.WriteAllText(
            SettingsPath,
            JsonSerializer.Serialize(preferences, new JsonSerializerOptions { WriteIndented = true }));

        if (rememberCredentials)
        {
            WriteCredential(username, password);
        }
        else
        {
            DeleteCredential();
        }
    }

    private static LoginPreferences LoadPreferences()
    {
        try
        {
            if (!File.Exists(SettingsPath))
            {
                return new LoginPreferences();
            }

            return JsonSerializer.Deserialize<LoginPreferences>(File.ReadAllText(SettingsPath))
                ?? new LoginPreferences();
        }
        catch
        {
            // A corrupt or stale preferences file should never prevent login.
            return new LoginPreferences();
        }
    }

    private static void WriteCredential(string username, string password)
    {
        byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
        IntPtr passwordPtr = Marshal.AllocCoTaskMem(passwordBytes.Length);

        try
        {
            Marshal.Copy(passwordBytes, 0, passwordPtr, passwordBytes.Length);

            CREDENTIAL credential = new()
            {
                Type = CRED_TYPE_GENERIC,
                TargetName = CredentialTarget,
                CredentialBlobSize = (uint)passwordBytes.Length,
                CredentialBlob = passwordPtr,
                Persist = CRED_PERSIST_LOCAL_MACHINE,
                UserName = username
            };

            if (!CredWrite(ref credential, 0))
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(),
                    "Windows Credential Manager could not save the FTP credentials.");
            }
        }
        finally
        {
            if (passwordBytes.Length > 0)
            {
                Array.Clear(passwordBytes, 0, passwordBytes.Length);
            }
            Marshal.FreeCoTaskMem(passwordPtr);
        }
    }

    private static bool TryReadCredential(out string username, out string password)
    {
        username = string.Empty;
        password = string.Empty;

        if (!CredRead(CredentialTarget, CRED_TYPE_GENERIC, 0, out IntPtr credentialPtr))
        {
            return false;
        }

        try
        {
            CREDENTIAL credential = Marshal.PtrToStructure<CREDENTIAL>(credentialPtr);
            username = credential.UserName ?? string.Empty;

            if (credential.CredentialBlob != IntPtr.Zero && credential.CredentialBlobSize > 0)
            {
                password = Marshal.PtrToStringUni(
                    credential.CredentialBlob,
                    checked((int)credential.CredentialBlobSize / 2)) ?? string.Empty;
            }

            return true;
        }
        finally
        {
            CredFree(credentialPtr);
        }
    }

    private static void DeleteCredential()
    {
        if (!CredDelete(CredentialTarget, CRED_TYPE_GENERIC, 0))
        {
            int error = Marshal.GetLastWin32Error();
            const int ErrorNotFound = 1168;
            if (error != ErrorNotFound)
            {
                throw new System.ComponentModel.Win32Exception(error,
                    "Windows Credential Manager could not remove the saved FTP credentials.");
            }
        }
    }

    private sealed class LoginPreferences
    {
        public string? Host { get; set; } = "gvarchive.com";
        public string? Username { get; set; } = string.Empty;
        public bool UseTls { get; set; } = true;
        public bool RememberCredentials { get; set; }
    }

    private const uint CRED_TYPE_GENERIC = 1;
    private const uint CRED_PERSIST_LOCAL_MACHINE = 2;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct CREDENTIAL
    {
        public uint Flags;
        public uint Type;
        public string TargetName;
        public string? Comment;
        public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
        public uint CredentialBlobSize;
        public IntPtr CredentialBlob;
        public uint Persist;
        public uint AttributeCount;
        public IntPtr Attributes;
        public string? TargetAlias;
        public string UserName;
    }

    [DllImport("advapi32.dll", EntryPoint = "CredWriteW", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CredWrite([In] ref CREDENTIAL userCredential, uint flags);

    [DllImport("advapi32.dll", EntryPoint = "CredReadW", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CredRead(string target, uint type, uint reservedFlag, out IntPtr credentialPtr);

    [DllImport("advapi32.dll", EntryPoint = "CredDeleteW", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CredDelete(string target, uint type, uint flags);

    [DllImport("advapi32.dll")]
    private static extern void CredFree(IntPtr buffer);
}
