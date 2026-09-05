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
                    BuildFtpErrorMessage(ex),
                    "FTP Connection Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                lblStatus.Text = "Connection failed.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    ex.Message,
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

        private static string BuildFtpErrorMessage(WebException ex)
        {
            if (ex.Response is FtpWebResponse ftp)
            {
                return $"The FTP server returned {ftp.StatusCode}: {ftp.StatusDescription.Trim()}";
            }

            return ex.Message;
        }

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
