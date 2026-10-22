using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyHoSoBenhAn.Forms
{
    internal static class Ui
    {
        public static DataGridView Grid()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoGenerateColumns = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                BackgroundColor = Color.White,
                RowHeadersVisible = false,
                Margin = new Padding(4)
            };
        }

        public static Button Button(string text, EventHandler onClick)
        {
            var button = new Button { Text = text, AutoSize = true, Height = 32, Padding = new Padding(10, 3, 10, 3), Margin = new Padding(4) };
            if (onClick != null) button.Click += onClick;
            return button;
        }

        public static Label Label(string text)
        {
            return new Label { Text = text, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(4, 8, 4, 4) };
        }

        public static TextBox TextBox(int width = 220, bool multiline = false)
        {
            return new TextBox { Width = width, Multiline = multiline, Height = multiline ? 56 : 25, Margin = new Padding(4), ScrollBars = multiline ? ScrollBars.Vertical : ScrollBars.None };
        }

        public static ComboBox Combo(int width = 240)
        {
            return new ComboBox { Width = width, DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(4) };
        }

        public static void ShowError(Exception ex, string title = "Không thể thực hiện")
        {
            MessageBox.Show(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowInfo(string text)
        {
            MessageBox.Show(text, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void BindCombo(ComboBox combo, DataTable table, string valueMember, string displayMember)
        {
            combo.DataSource = null;
            combo.ValueMember = valueMember;
            combo.DisplayMember = displayMember;
            combo.DataSource = table;
            if (table.Rows.Count == 0) combo.SelectedIndex = -1;
        }

        public static int SelectedInt(ComboBox combo, string prompt)
        {
            if (combo.SelectedValue == null || combo.SelectedValue is DataRowView)
                throw new InvalidOperationException("Vui lòng chọn " + prompt + ".");
            return Convert.ToInt32(combo.SelectedValue);
        }

        public static object NullableText(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (object)DBNull.Value : value.Trim();
        }

        public static void SelectRow(DataGridView grid, Action<DataGridViewRow> action)
        {
            if (grid.CurrentRow != null && grid.CurrentRow.DataBoundItem != null) action(grid.CurrentRow);
        }

        public static void FormatGrid(DataGridView grid)
        {
            grid.DataBindingComplete += (s, e) =>
            {
                foreach (DataGridViewColumn col in grid.Columns)
                {
                    if (col.Name.StartsWith("Ma", StringComparison.OrdinalIgnoreCase)) col.HeaderText = col.HeaderText.Replace("Ma", "Mã ");
                    col.SortMode = DataGridViewColumnSortMode.Automatic;
                }
            };
        }
    }
}
