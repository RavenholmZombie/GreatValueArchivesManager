namespace GreatValueArchivesManager
{
    partial class frmManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManager));
            menuStrip = new ReaLTaiizor.Controls.CrownMenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            pnlSidebar = new Panel();
            lblArchiveCategories = new Label();
            btnOverview = new ReaLTaiizor.Controls.CrownButton();
            btnFood = new ReaLTaiizor.Controls.CrownButton();
            btnBeverages = new ReaLTaiizor.Controls.CrownButton();
            btnNonFood = new ReaLTaiizor.Controls.CrownButton();
            btnDatasheets = new ReaLTaiizor.Controls.CrownButton();
            btnSpecial = new ReaLTaiizor.Controls.CrownButton();
            btnUnsorted = new ReaLTaiizor.Controls.CrownButton();
            btnConcepts = new ReaLTaiizor.Controls.CrownButton();
            btnVideos = new ReaLTaiizor.Controls.CrownButton();
            pnlSidebarDivider = new Panel();
            btnTrash = new ReaLTaiizor.Controls.CrownButton();
            pnlViewer = new Panel();
            listViewItems = new ListView();
            pnlHeader = new Panel();
            lblSubtitle = new Label();
            lblCategoryTitle = new Label();
            txtSearch = new ReaLTaiizor.Controls.CrownTextBox();
            pnlActions = new Panel();
            btnUpload = new ReaLTaiizor.Controls.CrownButton();
            btnMove = new ReaLTaiizor.Controls.CrownButton();
            btnRename = new ReaLTaiizor.Controls.CrownButton();
            btnDelete = new ReaLTaiizor.Controls.CrownButton();
            btnRefresh = new ReaLTaiizor.Controls.CrownButton();
            btnPreview = new ReaLTaiizor.Controls.CrownButton();
            statusStrip = new StatusStrip();
            lblConnectionStatus = new ToolStripStatusLabel();
            springStatus = new ToolStripStatusLabel();
            lblCategoryStatus = new ToolStripStatusLabel();
            lblItemCount = new ToolStripStatusLabel();
            menuStrip.SuspendLayout();
            pnlSidebar.SuspendLayout();
            pnlViewer.SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlActions.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.BackColor = Color.FromArgb(37, 37, 38);
            menuStrip.ForeColor = Color.WhiteSmoke;
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, viewToolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(6, 2, 0, 2);
            menuStrip.Size = new Size(1184, 24);
            menuStrip.TabIndex = 0;
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.BackColor = Color.FromArgb(37, 37, 38);
            fileToolStripMenuItem.ForeColor = Color.WhiteSmoke;
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.BackColor = Color.FromArgb(37, 37, 38);
            editToolStripMenuItem.ForeColor = Color.WhiteSmoke;
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = "Edit";
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.BackColor = Color.FromArgb(37, 37, 38);
            viewToolStripMenuItem.ForeColor = Color.WhiteSmoke;
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(44, 20);
            viewToolStripMenuItem.Text = "View";
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.BackColor = Color.FromArgb(37, 37, 38);
            toolsToolStripMenuItem.ForeColor = Color.WhiteSmoke;
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(47, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.BackColor = Color.FromArgb(37, 37, 38);
            helpToolStripMenuItem.ForeColor = Color.WhiteSmoke;
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // pnlSidebar
            // 
            pnlSidebar.BackColor = Color.FromArgb(45, 45, 48);
            pnlSidebar.Controls.Add(btnTrash);
            pnlSidebar.Controls.Add(pnlSidebarDivider);
            pnlSidebar.Controls.Add(btnVideos);
            pnlSidebar.Controls.Add(btnConcepts);
            pnlSidebar.Controls.Add(btnUnsorted);
            pnlSidebar.Controls.Add(btnSpecial);
            pnlSidebar.Controls.Add(btnDatasheets);
            pnlSidebar.Controls.Add(btnNonFood);
            pnlSidebar.Controls.Add(btnBeverages);
            pnlSidebar.Controls.Add(btnFood);
            pnlSidebar.Controls.Add(btnOverview);
            pnlSidebar.Controls.Add(lblArchiveCategories);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 24);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Padding = new Padding(12, 16, 12, 12);
            pnlSidebar.Size = new Size(240, 672);
            pnlSidebar.TabIndex = 1;
            // 
            // lblArchiveCategories
            // 
            lblArchiveCategories.AutoSize = true;
            lblArchiveCategories.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblArchiveCategories.ForeColor = Color.FromArgb(180, 180, 180);
            lblArchiveCategories.Location = new Point(16, 16);
            lblArchiveCategories.Name = "lblArchiveCategories";
            lblArchiveCategories.Size = new Size(127, 15);
            lblArchiveCategories.TabIndex = 0;
            lblArchiveCategories.Text = "ARCHIVE CATEGORIES";
            // 
            // sidebar buttons
            // 
            ConfigureSidebarButton(btnOverview, "Overview", 48);
            ConfigureSidebarButton(btnFood, "Food", 88);
            ConfigureSidebarButton(btnBeverages, "Beverages", 128);
            ConfigureSidebarButton(btnNonFood, "Non-Food Items", 168);
            ConfigureSidebarButton(btnDatasheets, "Archive Datasheets", 208);
            ConfigureSidebarButton(btnSpecial, "Special Submissions", 248);
            ConfigureSidebarButton(btnUnsorted, "Unsorted Archive Submissions", 288);
            ConfigureSidebarButton(btnConcepts, "Concepts", 328);
            ConfigureSidebarButton(btnVideos, "Videos", 368);
            // 
            // pnlSidebarDivider
            // 
            pnlSidebarDivider.BackColor = Color.FromArgb(62, 62, 66);
            pnlSidebarDivider.Location = new Point(16, 416);
            pnlSidebarDivider.Name = "pnlSidebarDivider";
            pnlSidebarDivider.Size = new Size(208, 1);
            pnlSidebarDivider.TabIndex = 10;
            // 
            // btnTrash
            // 
            ConfigureSidebarButton(btnTrash, "Trash", 432);
            // 
            // pnlViewer
            // 
            pnlViewer.BackColor = Color.FromArgb(30, 30, 30);
            pnlViewer.Controls.Add(listViewItems);
            pnlViewer.Controls.Add(pnlHeader);
            pnlViewer.Dock = DockStyle.Fill;
            pnlViewer.Location = new Point(240, 24);
            pnlViewer.Name = "pnlViewer";
            pnlViewer.Padding = new Padding(16);
            pnlViewer.Size = new Size(944, 672);
            pnlViewer.TabIndex = 2;
            // 
            // listViewItems
            // 
            listViewItems.BackColor = Color.FromArgb(30, 30, 30);
            listViewItems.BorderStyle = BorderStyle.None;
            listViewItems.Dock = DockStyle.Fill;
            listViewItems.ForeColor = Color.WhiteSmoke;
            listViewItems.HideSelection = false;
            listViewItems.Location = new Point(16, 92);
            listViewItems.Name = "listViewItems";
            listViewItems.Size = new Size(912, 564);
            listViewItems.TabIndex = 1;
            listViewItems.UseCompatibleStateImageBehavior = false;
            listViewItems.View = View.LargeIcon;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(37, 37, 38);
            pnlHeader.Controls.Add(txtSearch);
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Controls.Add(lblCategoryTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(16, 16);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(912, 76);
            pnlHeader.TabIndex = 0;
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.ForeColor = Color.FromArgb(170, 170, 170);
            lblSubtitle.Location = new Point(18, 43);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(196, 15);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Browse and manage archived media.";
            // 
            // lblCategoryTitle
            // 
            lblCategoryTitle.AutoSize = true;
            lblCategoryTitle.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold);
            lblCategoryTitle.ForeColor = Color.WhiteSmoke;
            lblCategoryTitle.Location = new Point(16, 12);
            lblCategoryTitle.Name = "lblCategoryTitle";
            lblCategoryTitle.Size = new Size(98, 28);
            lblCategoryTitle.TabIndex = 0;
            lblCategoryTitle.Text = "Overview";
            // 
            // txtSearch
            // 
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtSearch.BackColor = Color.FromArgb(45, 45, 48);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.ForeColor = Color.WhiteSmoke;
            txtSearch.Location = new Point(643, 24);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Search filenames...";
            txtSearch.Size = new Size(246, 23);
            txtSearch.TabIndex = 2;
            // 
            // pnlActions
            // 
            pnlActions.BackColor = Color.FromArgb(45, 45, 48);
            pnlActions.Controls.Add(btnPreview);
            pnlActions.Controls.Add(btnRefresh);
            pnlActions.Controls.Add(btnDelete);
            pnlActions.Controls.Add(btnRename);
            pnlActions.Controls.Add(btnMove);
            pnlActions.Controls.Add(btnUpload);
            pnlActions.Dock = DockStyle.Bottom;
            pnlActions.Location = new Point(0, 696);
            pnlActions.Name = "pnlActions";
            pnlActions.Padding = new Padding(12, 10, 12, 10);
            pnlActions.Size = new Size(1184, 54);
            pnlActions.TabIndex = 3;
            // 
            // action buttons
            // 
            ConfigureActionButton(btnUpload, "Upload", 12);
            ConfigureActionButton(btnMove, "Move", 112);
            ConfigureActionButton(btnRename, "Rename", 212);
            ConfigureActionButton(btnDelete, "Delete", 312);
            ConfigureActionButton(btnRefresh, "Refresh", 412);
            ConfigureActionButton(btnPreview, "Preview", 512);
            // 
            // statusStrip
            // 
            statusStrip.BackColor = Color.FromArgb(37, 37, 38);
            statusStrip.ForeColor = Color.Gainsboro;
            statusStrip.Items.AddRange(new ToolStripItem[] { lblConnectionStatus, springStatus, lblCategoryStatus, lblItemCount });
            statusStrip.Location = new Point(0, 750);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1184, 22);
            statusStrip.SizingGrip = false;
            statusStrip.TabIndex = 4;
            // 
            // lblConnectionStatus
            // 
            lblConnectionStatus.Name = "lblConnectionStatus";
            lblConnectionStatus.Size = new Size(101, 17);
            lblConnectionStatus.Text = "Not connected";
            // 
            // springStatus
            // 
            springStatus.Name = "springStatus";
            springStatus.Size = new Size(861, 17);
            springStatus.Spring = true;
            // 
            // lblCategoryStatus
            // 
            lblCategoryStatus.Name = "lblCategoryStatus";
            lblCategoryStatus.Size = new Size(110, 17);
            lblCategoryStatus.Text = "Category: Overview";
            // 
            // lblItemCount
            // 
            lblItemCount.Name = "lblItemCount";
            lblItemCount.Size = new Size(97, 17);
            lblItemCount.Text = "0 items";
            // 
            // frmManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(62, 62, 66);
            ClientSize = new Size(1184, 772);
            Controls.Add(pnlViewer);
            Controls.Add(pnlSidebar);
            Controls.Add(pnlActions);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            ForeColor = Color.WhiteSmoke;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            MinimumSize = new Size(980, 640);
            Name = "frmManager";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Great Value Archives Manager";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            pnlSidebar.ResumeLayout(false);
            pnlSidebar.PerformLayout();
            pnlViewer.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlActions.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private static void ConfigureSidebarButton(ReaLTaiizor.Controls.CrownButton button, string text, int top)
        {
            button.BackColor = Color.FromArgb(45, 45, 48);
            button.ForeColor = Color.WhiteSmoke;
            button.Location = new Point(16, top);
            button.Name = $"btn{text.Replace(" ", string.Empty).Replace("-", string.Empty)}";
            button.Padding = new Padding(8, 4, 8, 4);
            button.Size = new Size(208, 32);
            button.TabIndex = top;
            button.Text = text;
        }

        private static void ConfigureActionButton(ReaLTaiizor.Controls.CrownButton button, string text, int left)
        {
            button.BackColor = Color.FromArgb(62, 62, 66);
            button.ForeColor = Color.WhiteSmoke;
            button.Location = new Point(left, 11);
            button.Name = $"btn{text.Replace(" ", string.Empty)}";
            button.Padding = new Padding(5);
            button.Size = new Size(88, 30);
            button.TabIndex = left;
            button.Text = text;
        }

        #endregion

        private ReaLTaiizor.Controls.CrownMenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private Panel pnlSidebar;
        private Label lblArchiveCategories;
        private ReaLTaiizor.Controls.CrownButton btnOverview;
        private ReaLTaiizor.Controls.CrownButton btnFood;
        private ReaLTaiizor.Controls.CrownButton btnBeverages;
        private ReaLTaiizor.Controls.CrownButton btnNonFood;
        private ReaLTaiizor.Controls.CrownButton btnDatasheets;
        private ReaLTaiizor.Controls.CrownButton btnSpecial;
        private ReaLTaiizor.Controls.CrownButton btnUnsorted;
        private ReaLTaiizor.Controls.CrownButton btnConcepts;
        private ReaLTaiizor.Controls.CrownButton btnVideos;
        private Panel pnlSidebarDivider;
        private ReaLTaiizor.Controls.CrownButton btnTrash;
        private Panel pnlViewer;
        private Panel pnlHeader;
        private Label lblCategoryTitle;
        private Label lblSubtitle;
        private ReaLTaiizor.Controls.CrownTextBox txtSearch;
        private ListView listViewItems;
        private Panel pnlActions;
        private ReaLTaiizor.Controls.CrownButton btnUpload;
        private ReaLTaiizor.Controls.CrownButton btnMove;
        private ReaLTaiizor.Controls.CrownButton btnRename;
        private ReaLTaiizor.Controls.CrownButton btnDelete;
        private ReaLTaiizor.Controls.CrownButton btnRefresh;
        private ReaLTaiizor.Controls.CrownButton btnPreview;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblConnectionStatus;
        private ToolStripStatusLabel springStatus;
        private ToolStripStatusLabel lblCategoryStatus;
        private ToolStripStatusLabel lblItemCount;
    }
}
