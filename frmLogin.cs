using System.Diagnostics;

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

            txtBoxUser.BackColor = Color.FromArgb(45, 45, 48);
            txtBoxUser.ForeColor = Color.WhiteSmoke;
            txtBoxPass.BackColor = Color.FromArgb(45, 45, 48);
            txtBoxPass.ForeColor = Color.WhiteSmoke;

            btnLogin.BackColor = Color.FromArgb(0, 122, 204);
            btnLogin.ForeColor = Color.White;
            btnCPanel.BackColor = Color.FromArgb(45, 45, 48);
            btnCPanel.ForeColor = Color.WhiteSmoke;
        }

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBoxUser.Text) || string.IsNullOrWhiteSpace(txtBoxPass.Text))
            {
                MessageBox.Show(
                    this,
                    "Enter your FTP username and password before continuing.",
                    "Login Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            // FTP authentication will be wired here. For now this establishes the intended login-first flow.
            Hide();
            using frmManager manager = new();
            manager.ShowDialog(this);
            Close();
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
