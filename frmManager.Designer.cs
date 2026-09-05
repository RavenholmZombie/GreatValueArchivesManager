namespace GreatValueArchivesManager
{
    partial class frmManager
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManager));
            menuStrip = new ReaLTaiizor.Controls.CrownMenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            pnlActions = new Panel();
            pnlSidebar = new Panel();
            pnlViewer = new Panel();
            listView1 = new ListView();
            menuStrip.SuspendLayout();
            pnlViewer.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.BackColor = Color.FromArgb(60, 63, 65);
            menuStrip.ForeColor = Color.FromArgb(220, 220, 220);
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, viewToolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(3, 2, 0, 2);
            menuStrip.Size = new Size(800, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "crownMenuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            fileToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            editToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = "Edit";
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            viewToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(44, 20);
            viewToolStripMenuItem.Text = "View";
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            toolsToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(47, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            helpToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // pnlActions
            // 
            pnlActions.BorderStyle = BorderStyle.FixedSingle;
            pnlActions.Dock = DockStyle.Bottom;
            pnlActions.Location = new Point(0, 440);
            pnlActions.Name = "pnlActions";
            pnlActions.Size = new Size(800, 58);
            pnlActions.TabIndex = 1;
            // 
            // pnlSidebar
            // 
            pnlSidebar.BackColor = Color.FromArgb(45, 45, 48);
            pnlSidebar.BorderStyle = BorderStyle.FixedSingle;
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 24);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new Size(200, 416);
            pnlSidebar.TabIndex = 2;
            // 
            // pnlViewer
            // 
            pnlViewer.BorderStyle = BorderStyle.FixedSingle;
            pnlViewer.Controls.Add(listView1);
            pnlViewer.Dock = DockStyle.Fill;
            pnlViewer.Location = new Point(200, 24);
            pnlViewer.Name = "pnlViewer";
            pnlViewer.Size = new Size(600, 416);
            pnlViewer.TabIndex = 3;
            // 
            // listView1
            // 
            listView1.BackColor = Color.FromArgb(30, 30, 30);
            listView1.BorderStyle = BorderStyle.FixedSingle;
            listView1.Dock = DockStyle.Fill;
            listView1.ForeColor = Color.White;
            listView1.Location = new Point(0, 0);
            listView1.Name = "listView1";
            listView1.Size = new Size(598, 414);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            // 
            // frmManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(62, 62, 66);
            ClientSize = new Size(800, 498);
            Controls.Add(pnlViewer);
            Controls.Add(pnlSidebar);
            Controls.Add(pnlActions);
            Controls.Add(menuStrip);
            ForeColor = Color.White;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            Name = "frmManager";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Great Value Archives Manager";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            pnlViewer.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ReaLTaiizor.Controls.CrownMenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private Panel pnlActions;
        private Panel pnlSidebar;
        private Panel pnlViewer;
        private ListView listView1;
    }
}
