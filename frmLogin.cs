using System.Diagnostics;
using System.Net;

namespace GreatValueArchivesManager
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            ApplyPalette();
            btnLogin.Click += btnLogin_Click;
            btnCPanel.Click += btnCPanel_Click;
        }

        private void ApplyPalette()
        {
            BackColor = Color.FromArgb(62, 62, 66);
            ForeColor = Color.WhiteSmoke;

            txtBoxHost.BackColor = Color.FromArgb(45, 45, 48);
            txtBoxHost.ForeColor = Color.WhiteSmoke;
            txtBoxUser.BackColor = Color.FromArgb(45, 45, 48);
            txtBoxUser.ForeColor = Color.WhiteSmoke;
            txtBoxPass.BackColor = Color.FromArgb(45, 45, 48);
            txtBoxPass.ForeColor = Color.WhiteSmoke;

            btnLogin.BackColor = Color.FromArgb(0, 122, 204);
            btnLogin.ForeColor = Color.White;
            btnCPanel.BackColor = Color.FromArgb(45, 45, 48);
            btnCPanel.ForeColor = Color.WhiteSmoke;
            chkUseTls.ForeColor = Color.WhiteSmoke;
        }

        private async void btnLogin_Click(object? sender, EventArgs e)
        {
            string host = txtBoxHost.Text.Trim();
            string username = txtBoxUser.Text.Trim();
            string password = txtBoxPass.Text;

            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    this,
                    "Enter the FTP host, username, and password before continuing.",
                    "Login Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            SetBusy(true, "Connecting to the archive and locating /viewer/media...");
            ArchiveFtpClient client = new(host, username, password, chkUseTls.Checked);

            try
            {
                await client.ConnectAndDiscoverAsync();
                lblStatus.Text = $"Connected. Media root: {client.MediaRoot}";

                Hide();
                using frmManager manager = new(client);
                manager.ShowDialog(this);
                Show();
                txtBoxPass.Clear();
                lblStatus.Text = "Disconnected.";
            }
            catch (WebException ex)
            {
                MessageBox.Show(
                    this,
                    BuildFtpErrorMessage(ex, host, chkUseTls.Checked),
                    "FTP Connection Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                lblStatus.Text = "Connection failed.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    BuildConnectionErrorMessage(ex, host, chkUseTls.Checked),
                    "Connection Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                lblStatus.Text = "Connection failed.";
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void SetBusy(bool busy, string? statusText = null)
        {
            txtBoxHost.Enabled = !busy;
            txtBoxUser.Enabled = !busy;
            txtBoxPass.Enabled = !busy;
            chkUseTls.Enabled = !busy;
            btnLogin.Enabled = !busy;
            btnCPanel.Enabled = !busy;
            UseWaitCursor = busy;

            if (statusText is not null)
            {
                lblStatus.Text = statusText;
            }
        }

        private static string BuildFtpErrorMessage(WebException ex, string host, bool useTls)
        {
            if (useTls && IsCertificateNameMismatch(ex))
            {
                return BuildCertificateNameMismatchMessage(host);
            }

            if (ex.Response is FtpWebResponse ftp)
            {
                return $"The FTP server returned {ftp.StatusCode}: {ftp.StatusDescription.Trim()}";
            }

            return ex.Message;
        }

        private static string BuildConnectionErrorMessage(Exception ex, string host, bool useTls)
        {
            if (useTls && IsCertificateNameMismatch(ex))
            {
                return BuildCertificateNameMismatchMessage(host);
            }

            return ex.Message;
        }

        private static bool IsCertificateNameMismatch(Exception ex)
        {
            for (Exception? current = ex; current is not null; current = current.InnerException)
            {
                if (current.Message.Contains("RemoteCertificateNameMismatch", StringComparison.OrdinalIgnoreCase) ||
                    current.Message.Contains("certificate name mismatch", StringComparison.OrdinalIgnoreCase) ||
                    current.Message.Contains("remote certificate is invalid", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static string BuildCertificateNameMismatchMessage(string host) =>
            $"The FTP server answered, but its TLS certificate does not match '{host}'.\r\n\r\n" +
            "Namecheap recommends using the hosting server's actual hostname for FTPES/FTPS, " +
            "such as server123.web-hosting.com, rather than your website domain.\r\n\r\n" +
            "Open cPanel and look under General Information > Server Information for the Server Name, " +
            "then enter the full server hostname in the FTP Host box and try again.\r\n\r\n" +
            "Do not disable TLS just to bypass this warning unless you intentionally want an unencrypted FTP connection.";

        private void btnCPanel_Click(object? sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.namecheap.com/myaccount/login/",
                UseShellExecute = true
            });
        }
    }
}
