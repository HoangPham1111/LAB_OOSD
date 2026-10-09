using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCongTyDuLich.Forms
{
    /// <summary>Hàm tạo control dùng trong file Designer viết tay.</summary>
    internal static class DesignerUi
    {
        public static void Form(Form f, string title, int width, int height)
        {
            f.Text = title;
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ClientSize = new Size(width, height);
            f.Font = new Font("Segoe UI", 9F);
            f.MinimumSize = new Size(width, height);
            f.AutoScaleMode = AutoScaleMode.Font;
        }
        public static Label Label(string text, int x, int y, int width = 110, int height = 24)
        {
            return new Label { Text = text, Location = new Point(x, y), Size = new Size(width, height), TextAlign = ContentAlignment.MiddleLeft };
        }
        public static TextBox Text(string name, int x, int y, int width = 160, int height = 25, bool multiline = false)
        {
            return new TextBox { Name = name, Location = new Point(x, y), Size = new Size(width, height), Multiline = multiline, ScrollBars = multiline ? ScrollBars.Vertical : ScrollBars.None };
        }
        public static ComboBox Combo(string name, int x, int y, int width = 220)
        {
            return new ComboBox { Name = name, Location = new Point(x, y), Size = new Size(width, 25), DropDownStyle = ComboBoxStyle.DropDownList };
        }
        public static NumericUpDown Number(string name, int x, int y, int width = 110, decimal min = 0, decimal max = 999999999999M, int decimals = 0)
        {
            return new NumericUpDown { Name = name, Location = new Point(x, y), Size = new Size(width, 25), Minimum = min, Maximum = max, DecimalPlaces = decimals, ThousandsSeparator = true, Increment = decimals == 0 ? 1M : 1000M };
        }
        public static DateTimePicker Date(string name, int x, int y, int width = 145)
        {
            return new DateTimePicker { Name = name, Location = new Point(x, y), Size = new Size(width, 25), Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" };
        }
        public static CheckBox Check(string name, string text, int x, int y, int width = 150)
        {
            return new CheckBox { Name = name, Text = text, Location = new Point(x, y), Size = new Size(width, 26) };
        }
        public static Button Button(string name, string text, int x, int y, int width = 130)
        {
            return new Button { Name = name, Text = text, Location = new Point(x, y), Size = new Size(width, 32), UseVisualStyleBackColor = true };
        }
        public static DataGridView Grid(string name, int x, int y, int width, int height, bool editable = false)
        {
            return new DataGridView {
                Name = name, Location = new Point(x, y), Size = new Size(width, height),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                AutoGenerateColumns = true, AllowUserToAddRows = editable, ReadOnly = !editable,
                MultiSelect = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false, BackgroundColor = Color.White
            };
        }
        public static TabControl Tabs(string name, int x, int y, int width, int height)
        {
            return new TabControl { Name = name, Location = new Point(x, y), Size = new Size(width, height), Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right };
        }
        public static TabPage Page(string title) { return new TabPage(title); }
        public static void AddField(Control parent, string label, Control control, int x, int y, int labelWidth = 95)
        {
            parent.Controls.Add(Label(label, x, y, labelWidth));
            control.Location = new Point(x + labelWidth, y);
            parent.Controls.Add(control);
        }
        public static void GridStyle(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgv.AllowUserToResizeRows = false;
            dgv.RowTemplate.Height = 25;
        }
    }
}
