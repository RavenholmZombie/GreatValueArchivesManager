namespace GreatValueArchivesManager;

internal static class DarkDialogs
{
    private static readonly Color Back = Color.FromArgb(45, 45, 48);
    private static readonly Color Surface = Color.FromArgb(37, 37, 38);
    private static readonly Color Accent = Color.FromArgb(0, 122, 204);

    public static string? Prompt(IWin32Window owner, string title, string label, string initialValue = "")
    {
        using Form form = CreateBaseForm(title, 440, 170);

        Label prompt = new()
        {
            AutoSize = true,
            ForeColor = Color.WhiteSmoke,
            Location = new Point(16, 18),
            Text = label
        };

        TextBox input = new()
        {
            BackColor = Back,
            BorderStyle = BorderStyle.FixedSingle,
            ForeColor = Color.WhiteSmoke,
            Location = new Point(16, 46),
            Size = new Size(390, 23),
            Text = initialValue
        };

        Button ok = CreateButton("OK", Accent, 240, 92);
        Button cancel = CreateButton("Cancel", Back, 326, 92);
        ok.DialogResult = DialogResult.OK;
        cancel.DialogResult = DialogResult.Cancel;

        form.Controls.AddRange([prompt, input, ok, cancel]);
        form.AcceptButton = ok;
        form.CancelButton = cancel;

        input.SelectAll();
        input.Focus();
        return form.ShowDialog(owner) == DialogResult.OK ? input.Text.Trim() : null;
    }

    public static string? ChooseCategory(IWin32Window owner, string title, string label, string? currentCategory = null)
    {
        using Form form = CreateBaseForm(title, 440, 180);

        Label prompt = new()
        {
            AutoSize = true,
            ForeColor = Color.WhiteSmoke,
            Location = new Point(16, 18),
            Text = label
        };

        ComboBox combo = new()
        {
            BackColor = Back,
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.WhiteSmoke,
            Location = new Point(16, 46),
            Size = new Size(390, 23)
        };

        foreach (string category in ArchiveFtpClient.CategoryFolders.Keys)
        {
            if (!category.Equals(currentCategory, StringComparison.OrdinalIgnoreCase))
            {
                combo.Items.Add(category);
            }
        }

        if (combo.Items.Count > 0)
        {
            combo.SelectedIndex = 0;
        }

        Button ok = CreateButton("Move", Accent, 240, 96);
        Button cancel = CreateButton("Cancel", Back, 326, 96);
        ok.DialogResult = DialogResult.OK;
        cancel.DialogResult = DialogResult.Cancel;

        form.Controls.AddRange([prompt, combo, ok, cancel]);
        form.AcceptButton = ok;
        form.CancelButton = cancel;

        return form.ShowDialog(owner) == DialogResult.OK ? combo.SelectedItem?.ToString() : null;
    }

    private static Form CreateBaseForm(string title, int width, int height) => new()
    {
        BackColor = Surface,
        ClientSize = new Size(width, height),
        ForeColor = Color.WhiteSmoke,
        FormBorderStyle = FormBorderStyle.FixedDialog,
        MaximizeBox = false,
        MinimizeBox = false,
        ShowInTaskbar = false,
        StartPosition = FormStartPosition.CenterParent,
        Text = title
    };

    private static Button CreateButton(string text, Color backColor, int x, int y) => new()
    {
        BackColor = backColor,
        FlatStyle = FlatStyle.Flat,
        ForeColor = Color.White,
        Location = new Point(x, y),
        Size = new Size(80, 30),
        Text = text,
        UseVisualStyleBackColor = false
    };
}
