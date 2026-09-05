namespace GreatValueArchivesManager
{
    partial class frmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            txtBoxUser = new ReaLTaiizor.Controls.CrownTextBox();
            label1 = new Label();
            txtBoxPass = new ReaLTaiizor.Controls.CrownTextBox();
            label2 = new Label();
            label3 = new Label();
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
            panel1.TabIndex = 1;
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
            // txtBoxUser
            // 
            txtBoxUser.BackColor = Color.FromArgb(69, 73, 74);
            txtBoxUser.BorderStyle = BorderStyle.FixedSingle;
            txtBoxUser.ForeColor = Color.FromArgb(220, 220, 220);
            txtBoxUser.Location = new Point(69, 174);
            txtBoxUser.Name = "txtBoxUser";
            txtBoxUser.Size = new Size(295, 23);
            txtBoxUser.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(89, 122);
            label1.Name = "label1";
            label1.Size = new Size(254, 15);
            label1.TabIndex = 3;
            label1.Text = "Login to the Great Value Archives FTP to begin.";
            // 
            // txtBoxPass
            // 
            txtBoxPass.BackColor = Color.FromArgb(69, 73, 74);
            txtBoxPass.BorderStyle = BorderStyle.FixedSingle;
            txtBoxPass.ForeColor = Color.FromArgb(220, 220, 220);
            txtBoxPass.Location = new Point(69, 229);
            txtBoxPass.Name = "txtBoxPass";
            txtBoxPass.Size = new Size(295, 23);
            txtBoxPass.TabIndex = 4;
            txtBoxPass.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(69, 156);
            label2.Name = "label2";
            label2.Size = new Size(86, 15);
            label2.TabIndex = 5;
            label2.Text = "FTP Username:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(69, 211);
            label3.Name = "label3";
            label3.Size = new Size(83, 15);
            label3.TabIndex = 6;
            label3.Text = "FTP Password:";
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(272, 277);
            btnLogin.Name = "btnLogin";
            btnLogin.Padding = new Padding(5);
            btnLogin.Size = new Size(92, 27);
            btnLogin.TabIndex = 7;
            btnLogin.Text = "Login";
            // 
            // btnCPanel
            // 
            btnCPanel.Location = new Point(69, 277);
            btnCPanel.Name = "btnCPanel";
            btnCPanel.Padding = new Padding(5);
            btnCPanel.Size = new Size(92, 27);
            btnCPanel.TabIndex = 8;
            btnCPanel.Text = "Open CPanel";
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(62, 62, 66);
            ClientSize = new Size(433, 340);
            Controls.Add(btnCPanel);
            Controls.Add(btnLogin);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txtBoxPass);
            Controls.Add(label1);
            Controls.Add(txtBoxUser);
            Controls.Add(panel1);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel panel1;
        private PictureBox pictureBox1;
        private ReaLTaiizor.Controls.CrownTextBox txtBoxUser;
        private Label label1;
        private ReaLTaiizor.Controls.CrownTextBox txtBoxPass;
        private Label label2;
        private Label label3;
        private ReaLTaiizor.Controls.CrownButton btnLogin;
        private ReaLTaiizor.Controls.CrownButton btnCPanel;
    }
}