using System.Diagnostics;

namespace GreatValueArchivesManager
{
    public partial class frmManager : Form
    {
        private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(20) };

        private readonly Dictionary<Control, string> _categoryNames = new();
        private readonly ArchiveFtpClient _client;
        private readonly ImageList _thumbnailImages = new();
        private readonly SemaphoreSlim _thumbnailGate = new(6);
        private readonly List<ArchiveItem> _currentItems = [];
        private CancellationTokenSource? _loadCts;
        private string _currentCategory = "Overview";

        public frmManager(ArchiveFtpClient client)
        {
            _client = client;
            InitializeComponent();
            ConfigureListView();
            WireCategoryButtons();
            WireActions();
            BuildMenus();
            ApplyPalette();

            lblConnectionStatus.Text = $"Connected to {_client.Host}";
            Shown += async (_, _) => await SelectCategoryAsync(btnOverview, "Overview");
            FormClosing += (_, _) => _loadCts?.Cancel();
        }

        private void ConfigureListView()
        {
            _thumbnailImages.ColorDepth = ColorDepth.Depth32Bit;
            _thumbnailImages.ImageSize = new Size(160, 120);
            _thumbnailImages.TransparentColor = Color.Transparent;
            _thumbnailImages.Images.Add("image", CreatePlaceholder("IMAGE"));
            _thumbnailImages.Images.Add("video", CreatePlaceholder("VIDEO"));
            _thumbnailImages.Images.Add("trash", CreatePlaceholder("TRASH"));

            listViewItems.LargeImageList = _thumbnailImages;
            listViewItems.MultiSelect = true;
            listViewItems.LabelWrap = true;
            listViewItems.ShowItemToolTips = true;
            listViewItems.TextChanged += (_, _) => { };
            txtSearch.TextChanged += (_, _) => ApplySearchFilter();
            listViewItems.DoubleClick += async (_, _) => await PreviewSelectedAsync();
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
        }

        private void WireActions()
        {
            btnUpload.Click += async (_, _) => await UploadAsync();
            btnMove.Click += async (_, _) => await MoveSelectedAsync();
            btnRename.Click += async (_, _) => await RenameSelectedAsync();
            btnDelete.Click += async (_, _) => await DeleteSelectedAsync();
            btnRefresh.Click += async (_, _) => await ReloadCurrentCategoryAsync();
            btnPreview.Click += async (_, _) => await PreviewSelectedAsync();
        }

        private void BuildMenus()
        {
            fileToolStripMenuItem.DropDownItems.Clear();
            fileToolStripMenuItem.DropDownItems.Add(CreateMenuItem("Upload...", async () => await UploadAsync(), Keys.Control | Keys.U));
            fileToolStripMenuItem.DropDownItems.Add(CreateMenuItem("Refresh", async () => await ReloadCurrentCategoryAsync(), Keys.F5));
            fileToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
            fileToolStripMenuItem.DropDownItems.Add(CreateMenuItem("Log out", () => { Close(); return Task.CompletedTask; }));
            fileToolStripMenuItem.DropDownItems.Add(CreateMenuItem("Exit", () => { Application.Exit(); return Task.CompletedTask; }));

            editToolStripMenuItem.DropDownItems.Clear();
            editToolStripMenuItem.DropDownItems.Add(CreateMenuItem("Rename...", async () => await RenameSelectedAsync(), Keys.F2));
            editToolStripMenuItem.DropDownItems.Add(CreateMenuItem("Move to...", async () => await MoveSelectedAsync(), Keys.Control | Keys.M));
            editToolStripMenuItem.DropDownItems.Add(CreateMenuItem("Delete", async () => await DeleteSelectedAsync(), Keys.Delete));

            viewToolStripMenuItem.DropDownItems.Clear();
            viewToolStripMenuItem.DropDownItems.Add(CreateMenuItem("Preview selected", async () => await PreviewSelectedAsync(), Keys.Enter));
            viewToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
            foreach ((Control button, string category) in _categoryNames)
            {
                viewToolStripMenuItem.DropDownItems.Add(CreateMenuItem(category, async () => await SelectCategoryAsync(button, category)));
            }

            toolsToolStripMenuItem.DropDownItems.Clear();
            toolsToolStripMenuItem.DropDownItems.Add(CreateMenuItem("Open Archive Viewer", () =>
            {
                OpenArchiveViewer();
                return Task.CompletedTask;
            }));
            toolsToolStripMenuItem.DropDownItems.Add(CreateMenuItem("Open Namecheap / cPanel", () =>
            {
                OpenUrl("https://www.namecheap.com/myaccount/login/");
                return Task.CompletedTask;
            }));

            helpToolStripMenuItem.DropDownItems.Clear();
            helpToolStripMenuItem.DropDownItems.Add(CreateMenuItem("About", () =>
            {
                MessageBox.Show(
                    this,
                    $"Great Value Archives Manager\n\nConnected host: {_client.Host}\nArchive root: {_client.MediaRoot}",
                    "About",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return Task.CompletedTask;
            }));
        }

        private ToolStripMenuItem CreateMenuItem(string text, Func<Task> action, Keys shortcutKeys = Keys.None)
        {
            ToolStripMenuItem item = new(text)
            {
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.WhiteSmoke,
                ShortcutKeys = shortcutKeys
            };
            item.Click += async (_, _) => await RunUiActionAsync(action);
            return item;
        }

        private async void CategoryButton_Click(object? sender, EventArgs e)
        {
            if (sender is Control button && _categoryNames.TryGetValue(button, out string? category))
            {
                await SelectCategoryAsync(button, category);
            }
        }

        private async Task SelectCategoryAsync(Control selectedButton, string category)
        {
            _currentCategory = category;

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
                "Overview" => "Browse all archived media across every public category.",
                "Trash" => "Review removed items or permanently delete them.",
                _ => $"Manage files in {category}."
            };

            btnUpload.Enabled = category != "Trash";
            btnMove.Enabled = category != "Overview";
            btnRename.Enabled = category != "Overview";
            btnDelete.Enabled = category != "Overview";
            txtSearch.Clear();

            await ReloadCurrentCategoryAsync();
        }

        private async Task ReloadCurrentCategoryAsync()
        {
            _loadCts?.Cancel();
            _loadCts?.Dispose();
            _loadCts = new CancellationTokenSource();
            CancellationToken token = _loadCts.Token;

            SetBusy(true, "Loading archive items...");
            try
            {
                IReadOnlyList<ArchiveItem> items = await _client.ListCategoryAsync(_currentCategory, token);
                token.ThrowIfCancellationRequested();

                _currentItems.Clear();
                _currentItems.AddRange(items);
                ApplySearchFilter();
                lblItemCount.Text = $"{_currentItems.Count:N0} items";

                foreach (ArchiveItem item in items.Where(i => !i.IsVideo && i.PublicUrl is not null))
                {
                    _ = LoadThumbnailAsync(item, token);
                }
            }
            catch (OperationCanceledException)
            {
                // A newer category/refresh request replaced this one.
            }
            catch (Exception ex)
            {
                ShowOperationError("Could not load this archive category.", ex);
            }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    SetBusy(false, "Ready");
                }
            }
        }

        private void ApplySearchFilter()
        {
            string filter = txtSearch.Text.Trim();
            IEnumerable<ArchiveItem> filtered = string.IsNullOrWhiteSpace(filter)
                ? _currentItems
                : _currentItems.Where(i => i.FileName.Contains(filter, StringComparison.OrdinalIgnoreCase));

            listViewItems.BeginUpdate();
            try
            {
                listViewItems.Items.Clear();
                foreach (ArchiveItem item in filtered)
                {
                    string imageKey = GetImageKey(item);
                    ListViewItem listItem = new(item.FileName, imageKey)
                    {
                        Tag = item,
                        ToolTipText = item.Category == _currentCategory
                            ? item.FileName
                            : $"{item.FileName}\n{item.Category}"
                    };
                    listViewItems.Items.Add(listItem);
                }
            }
            finally
            {
                listViewItems.EndUpdate();
            }

            lblItemCount.Text = $"{listViewItems.Items.Count:N0} of {_currentItems.Count:N0} items";
        }

        private async Task LoadThumbnailAsync(ArchiveItem item, CancellationToken token)
        {
            string key = item.RemotePath;
            if (_thumbnailImages.Images.ContainsKey(key) || item.PublicUrl is null)
            {
                return;
            }

            await _thumbnailGate.WaitAsync(token);
            try
            {
                if (_thumbnailImages.Images.ContainsKey(key))
                {
                    return;
                }

                byte[] bytes = await Http.GetByteArrayAsync(item.PublicUrl, token);
                using MemoryStream stream = new(bytes);
                using Image source = Image.FromStream(stream);
                using Bitmap thumbnail = new(source);
                _thumbnailImages.Images.Add(key, new Bitmap(thumbnail));

                foreach (ListViewItem visibleItem in listViewItems.Items)
                {
                    if (visibleItem.Tag is ArchiveItem archiveItem && archiveItem.RemotePath == item.RemotePath)
                    {
                        visibleItem.ImageKey = key;
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch
            {
                // The generic image placeholder remains when an individual thumbnail cannot be downloaded.
            }
            finally
            {
                _thumbnailGate.Release();
            }
        }

        private async Task UploadAsync()
        {
            string? targetCategory = _currentCategory;
            if (targetCategory is "Overview" or "Trash")
            {
                targetCategory = DarkDialogs.ChooseCategory(this, "Upload", "Choose the category for the uploaded files:");
                if (targetCategory is null)
                {
                    return;
                }
            }

            using OpenFileDialog dialog = new()
            {
                Multiselect = true,
                Title = $"Upload to {targetCategory}",
                Filter = "Archive media|*.jpg;*.jpeg;*.png;*.gif;*.webp;*.avif;*.mp4;*.webm;*.ogg;*.mov;*.m4v|All files|*.*"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            await RunUiActionAsync(async () =>
            {
                SetBusy(true, $"Uploading {dialog.FileNames.Length} file(s)...");
                foreach (string file in dialog.FileNames)
                {
                    await _client.UploadFileAsync(file, targetCategory);
                }
                await ReloadCurrentCategoryAsync();
            });
        }

        private async Task RenameSelectedAsync()
        {
            ArchiveItem? item = GetSingleSelectedItem("Select one item to rename.");
            if (item is null)
            {
                return;
            }

            string? newName = DarkDialogs.Prompt(this, "Rename", "New file name:", item.FileName);
            if (string.IsNullOrWhiteSpace(newName) || newName.Equals(item.FileName, StringComparison.Ordinal))
            {
                return;
            }

            await RunUiActionAsync(async () =>
            {
                SetBusy(true, $"Renaming {item.FileName}...");
                await _client.RenameAsync(item, newName);
                await ReloadCurrentCategoryAsync();
            });
        }

        private async Task MoveSelectedAsync()
        {
            IReadOnlyList<ArchiveItem> selected = GetSelectedItems();
            if (selected.Count == 0)
            {
                ShowSelectionMessage("Select one or more items to move.");
                return;
            }

            string? target = DarkDialogs.ChooseCategory(this, "Move Items", "Move selected items to:", _currentCategory);
            if (target is null)
            {
                return;
            }

            await RunUiActionAsync(async () =>
            {
                SetBusy(true, $"Moving {selected.Count} item(s) to {target}...");
                foreach (ArchiveItem item in selected)
                {
                    await _client.MoveAsync(item, target);
                }
                await ReloadCurrentCategoryAsync();
            });
        }

        private async Task DeleteSelectedAsync()
        {
            IReadOnlyList<ArchiveItem> selected = GetSelectedItems();
            if (selected.Count == 0)
            {
                ShowSelectionMessage("Select one or more items to delete.");
                return;
            }

            bool permanent = _currentCategory.Equals("Trash", StringComparison.OrdinalIgnoreCase);
            string prompt = permanent
                ? $"Permanently delete {selected.Count} selected item(s)? This cannot be undone."
                : $"Move {selected.Count} selected item(s) to Trash?";

            DialogResult answer = MessageBox.Show(
                this,
                prompt,
                permanent ? "Permanently Delete" : "Move to Trash",
                MessageBoxButtons.YesNo,
                permanent ? MessageBoxIcon.Warning : MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (answer != DialogResult.Yes)
            {
                return;
            }

            await RunUiActionAsync(async () =>
            {
                SetBusy(true, permanent ? "Deleting items..." : "Moving items to Trash...");
                foreach (ArchiveItem item in selected)
                {
                    if (permanent)
                    {
                        await _client.PermanentlyDeleteAsync(item);
                    }
                    else
                    {
                        await _client.MoveToTrashAsync(item);
                    }
                }
                await ReloadCurrentCategoryAsync();
            });
        }

        private async Task PreviewSelectedAsync()
        {
            ArchiveItem? item = GetSingleSelectedItem("Select one item to preview.", showMessage: false);
            if (item is null)
            {
                return;
            }

            try
            {
                if (item.PublicUrl is not null)
                {
                    OpenUrl(item.PublicUrl);
                    return;
                }

                SetBusy(true, $"Downloading {item.FileName} for preview...");
                byte[] bytes = await _client.DownloadFileAsync(item.RemotePath);
                string tempFolder = Path.Combine(Path.GetTempPath(), "GreatValueArchivesManager");
                Directory.CreateDirectory(tempFolder);
                string tempPath = Path.Combine(tempFolder, item.FileName);
                await File.WriteAllBytesAsync(tempPath, bytes);
                Process.Start(new ProcessStartInfo(tempPath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                ShowOperationError("Could not preview the selected item.", ex);
            }
            finally
            {
                SetBusy(false, "Ready");
            }
        }

        private IReadOnlyList<ArchiveItem> GetSelectedItems() =>
            listViewItems.SelectedItems.Cast<ListViewItem>()
                .Select(i => i.Tag)
                .OfType<ArchiveItem>()
                .ToArray();

        private ArchiveItem? GetSingleSelectedItem(string message, bool showMessage = true)
        {
            IReadOnlyList<ArchiveItem> selected = GetSelectedItems();
            if (selected.Count == 1)
            {
                return selected[0];
            }

            if (showMessage)
            {
                ShowSelectionMessage(message);
            }
            return null;
        }

        private async Task RunUiActionAsync(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                ShowOperationError("The FTP operation failed.", ex);
                SetBusy(false, "Ready");
            }
        }

        private void SetBusy(bool busy, string status)
        {
            UseWaitCursor = busy;
            lblConnectionStatus.Text = busy ? status : $"Connected to {_client.Host}";
            btnUpload.Enabled = !busy && _currentCategory != "Trash";
            btnMove.Enabled = !busy && _currentCategory != "Overview";
            btnRename.Enabled = !busy && _currentCategory != "Overview";
            btnDelete.Enabled = !busy && _currentCategory != "Overview";
            btnRefresh.Enabled = !busy;
            btnPreview.Enabled = !busy;
        }

        private string GetImageKey(ArchiveItem item)
        {
            if (_thumbnailImages.Images.ContainsKey(item.RemotePath))
            {
                return item.RemotePath;
            }
            if (_currentCategory == "Trash")
            {
                return "trash";
            }
            return item.IsVideo ? "video" : "image";
        }

        private void OpenArchiveViewer()
        {
            string url = _currentCategory is "Overview" or "Trash"
                ? "https://gvarchive.com/viewer/"
                : $"https://gvarchive.com/viewer/?category={Uri.EscapeDataString(_currentCategory)}";
            OpenUrl(url);
        }

        private static void OpenUrl(string url) =>
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

        private void ShowSelectionMessage(string message) =>
            MessageBox.Show(this, message, "Select Archive Item", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private void ShowOperationError(string heading, Exception ex) =>
            MessageBox.Show(this, $"{heading}\n\n{ex.Message}", "Great Value Archives Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);

        private static Bitmap CreatePlaceholder(string text)
        {
            Bitmap bitmap = new(160, 120);
            using Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.FromArgb(37, 37, 38));
            using Pen border = new(Color.FromArgb(62, 62, 66));
            graphics.DrawRectangle(border, 0, 0, 159, 119);
            using Font font = new("Segoe UI Semibold", 11F, FontStyle.Bold);
            SizeF size = graphics.MeasureString(text, font);
            graphics.DrawString(text, font, Brushes.Gainsboro, (160 - size.Width) / 2, (120 - size.Height) / 2);
            return bitmap;
        }

        private void ApplyPalette()
        {
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
