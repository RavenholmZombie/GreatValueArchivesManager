namespace GreatValueArchivesManager
{
    public partial class frmManager : Form
    {
        private readonly Dictionary<Control, string> _categoryNames = new();

        public frmManager()
        {
            InitializeComponent();
            WireCategoryButtons();
            ApplyPalette();
        }

        private void WireCategoryButtons()
        {
            _categoryNames[btnOverview] = "Overview";
            _categoryNames[btnFood] = "Food";
            _categoryNames[btnBeverages] = "Beverages";
            _categoryNames[btnNonFood] = "Non-Food Items";
            _categoryNames[btnDatasheets] = "Archive Datasheets";
            _categoryNames[btnSpecial] = "Special Submissions";
            _categoryNames[btnUnsorted] = "Unsorted Archive Submissions";
            _categoryNames[btnConcepts] = "Concepts";
            _categoryNames[btnVideos] = "Videos";
            _categoryNames[btnTrash] = "Trash";

            foreach (Control button in _categoryNames.Keys)
            {
                button.Click += CategoryButton_Click;
            }

            SelectCategory(btnOverview, "Overview");
        }

        private void CategoryButton_Click(object? sender, EventArgs e)
        {
            if (sender is Control button && _categoryNames.TryGetValue(button, out string? category))
            {
                SelectCategory(button, category);
            }
        }

        private void SelectCategory(Control selectedButton, string category)
        {
            foreach (Control button in _categoryNames.Keys)
            {
                button.BackColor = Color.FromArgb(45, 45, 48);
                button.ForeColor = Color.WhiteSmoke;
            }

            selectedButton.BackColor = Color.FromArgb(0, 122, 204);
            selectedButton.ForeColor = Color.White;

            lblCategoryTitle.Text = category;
            lblCategoryStatus.Text = $"Category: {category}";
            lblSubtitle.Text = category switch
            {
                "Overview" => "Browse and manage archived media.",
                "Trash" => "Review items removed from public archive categories.",
                _ => $"Manage files in {category}."
            };
        }

        private void ApplyPalette()
        {
            // VS dark theme palette: #007acc, #3e3e42, #2d2d30, #252526, #1e1e1e
            BackColor = Color.FromArgb(62, 62, 66);
            pnlSidebar.BackColor = Color.FromArgb(45, 45, 48);
            pnlHeader.BackColor = Color.FromArgb(37, 37, 38);
            pnlViewer.BackColor = Color.FromArgb(30, 30, 30);
            pnlActions.BackColor = Color.FromArgb(45, 45, 48);
            listViewItems.BackColor = Color.FromArgb(30, 30, 30);
            listViewItems.ForeColor = Color.WhiteSmoke;
            statusStrip.BackColor = Color.FromArgb(37, 37, 38);
            menuStrip.BackColor = Color.FromArgb(37, 37, 38);
        }
    }
}
