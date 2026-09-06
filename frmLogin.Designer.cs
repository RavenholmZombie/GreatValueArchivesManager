namespace GreatValueArchivesManager
{
    partial class frmLogin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            txtBoxHost = new ReaLTaiizor.Controls.CrownTextBox();
            txtBoxUser = new ReaLTaiizor.Controls.CrownTextBox();
            txtBoxPass = new ReaLTaiizor.Controls.CrownTextBox();
            lblIntro = new Label();
            lblHost = new Label();
            lblUser = new Label();
            lblPass = new Label();
            chkUseTls = new CheckBox();
            chkRememberCredentials = new CheckBox();
            lblStatus = new Label();
            btnLogin = new ReaLTaiizor.Controls.CrownButton();
            btnCPanel = new ReaLTaiizor.Controls.CrownButton();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackgroundImage = Properties.Resources.aurora;
            panel1.BackgroundImageLayout = ImageLayout.None;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(433, 100);
            panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.None;
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = Properties.Resources.banner;
            pictureBox1.Location = new Point(69, 25);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(295, 50);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // txtBoxHost
            // 
            txtBoxHost.BackColor = Color.FromArgb(45, 45, 48);
            txtBoxHost.BorderStyle = BorderStyle.FixedSingle;
            txtBoxHost.ForeColor = Color.FromArgb(220, 220, 220);
            txtBoxHost.Location = new Point(69, 174);
            txtBoxHost.Name = "txtBoxHost";
            txtBoxHost.Size = new Size(295, 23);
            txtBoxHost.TabIndex = 1;
            txtBoxHost.Text = "gvarchive.com";
            // 
            // txtBoxUser
            // 
            txtBoxUser.BackColor = Color.FromArgb(45, 45, 48);
            txtBoxUser.BorderStyle = BorderStyle.FixedSingle;
            txtBoxUser.ForeColor = Color.FromArgb(220, 220, 220);
            txtBoxUser.Location = new Point(69, 229);
            txtBoxUser.Name = "txtBoxUser";
            txtBoxUser.Size = new Size(295, 23);
            txtBoxUser.TabIndex = 2;
            // 
            // txtBoxPass
            // 
            txtBoxPass.BackColor = Color.FromArgb(45, 45, 48);
            txtBoxPass.BorderStyle = BorderStyle.FixedSingle;
            txtBoxPass.ForeColor = Color.FromArgb(220, 220, 220);
            txtBoxPass.Location = new Point(69, 284);
            txtBoxPass.Name = "txtBoxPass";
            txtBoxPass.Size = new Size(295, 23);
            txtBoxPass.TabIndex = 3;
            txtBoxPass.UseSystemPasswordChar = true;
            // 
            // lblIntro
            // 
            lblIntro.AutoSize = true;
            lblIntro.Location = new Point(75, 121);
            lblIntro.Name = "lblIntro";
            lblIntro.Size = new Size(283, 15);
            lblIntro.TabIndex = 10;
            lblIntro.Text = "Connect to the Great Value Archives FTP to begin.";
            // 
            // lblHost
            // 
            lblHost.AutoSize = true;
            lblHost.Location = new Point(69, 156);
            lblHost.Name = "lblHost";
            lblHost.Size = new Size(58, 15);
            lblHost.TabIndex = 11;
            lblHost.Text = "FTP Host:";
            // 
            // lblUser
            // 
            lblUser.AutoSize = true;
            lblUser.Location = new Point(69, 211);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(86, 15);
            lblUser.TabIndex = 12;
            lblUser.Text = "FTP Username:";
            // 
            // lblPass
            // 
            lblPass.AutoSize = true;
            lblPass.Location = new Point(69, 266);
            lblPass.Name = "lblPass";
            lblPass.Size = new Size(83, 15);
            lblPass.TabIndex = 13;
            lblPass.Text = "FTP Password:";
            // 
            // chkUseTls
            // 
            chkUseTls.AutoSize = true;
            chkUseTls.Checked = true;
            chkUseTls.CheckState = CheckState.Checked;
            chkUseTls.Location = new Point(69, 318);
            chkUseTls.Name = "chkUseTls";
            chkUseTls.Size = new Size(188, 19);
            chkUseTls.TabIndex = 4;
            chkUseTls.Text = "Use FTPS (TLS) when available";
            chkUseTls.UseVisualStyleBackColor = true;
            // 
            // chkRememberCredentials
            // 
            chkRememberCredentials.AutoSize = true;
            chkRememberCredentials.Location = new Point(69, 343);
            chkRememberCredentials.Name = "chkRememberCredentials";
            chkRememberCredentials.Size = new Size(150, 19);
            chkRememberCredentials.TabIndex = 5;
            chkRememberCredentials.Text = "Remember credentials";
            chkRememberCredentials.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            lblStatus.AutoEllipsis = true;
            lblStatus.ForeColor = Color.FromArgb(180, 180, 180);
            lblStatus.Location = new Point(69, 372);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(295, 34);
            lblStatus.TabIndex = 14;
            lblStatus.Text = "";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(272, 419);
            btnLogin.Name = "btnLogin";
            btnLogin.Padding = new Padding(5);
            btnLogin.Size = new Size(92, 30);
            btnLogin.TabIndex = 6;
            btnLogin.Text = "Connect";
            // 
            // btnCPanel
            // 
            btnCPanel.Location = new Point(69, 419);
            btnCPanel.Name = "btnCPanel";
            btnCPanel.Padding = new Padding(5);
            btnCPanel.Size = new Size(92, 30);
            btnCPanel.TabIndex = 7;
            btnCPanel.Text = "Open CPanel";
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(62, 62, 66);
            ClientSize = new Size(433, 477);
            Controls.Add(btnCPanel);
            Controls.Add(btnLogin);
            Controls.Add(lblStatus);
            Controls.Add(chkRememberCredentials);
            Controls.Add(chkUseTls);
            Controls.Add(lblPass);
            Controls.Add(lblUser);
            Controls.Add(lblHost);
            Controls.Add(lblIntro);
            Controls.Add(txtBoxPass);
            Controls.Add(txtBoxUser);
            Controls.Add(txtBoxHost);
            Controls.Add(panel1);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Connect to Great Value Archives";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private PictureBox pictureBox1;
        private ReaLTaiizor.Controls.CrownTextBox txtBoxHost;
        private ReaLTaiizor.Controls.CrownTextBox txtBoxUser;
        private ReaLTaiizor.Controls.CrownTextBox txtBoxPass;
        private Label lblIntro;
        private Label lblHost;
        private Label lblUser;
        private Label lblPass;
        private CheckBox chkUseTls;
        private CheckBox chkRememberCredentials;
        private Label lblStatus;
        private ReaLTaiizor.Controls.CrownButton btnLogin;
        private ReaLTaiizor.Controls.CrownButton btnCPanel;
    }
}
