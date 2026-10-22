using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Data;

namespace QuanLyHoSoBenhAn.Forms
{
    internal sealed class MasterField
    {
        public string Name;
        public string Caption;
        public int Size;
        public bool Required;
        public bool IsBit;
        public bool Multiline;
        public MasterField(string name, string caption, int size, bool required = false, bool isBit = false, bool multiline = false)
        { Name = name; Caption = caption; Size = size; Required = required; IsBit = isBit; Multiline = multiline; }
    }

    internal sealed class MasterDefinition
    {
        public string Title, Table, PrimaryKey, SearchColumn;
        public string[] Columns;
        public List<MasterField> Fields;
        public MasterDefinition(string title, string table, string pk, string search, string[] columns, List<MasterField> fields)
        { Title = title; Table = table; PrimaryKey = pk; SearchColumn = search; Columns = columns; Fields = fields; }
    }

    public sealed class MasterDataForm : Form
    {
        private readonly MasterDefinition _definition;
        private readonly DataGridView _grid = Ui.Grid();
        private readonly TextBox _search = Ui.TextBox(260);
        private readonly Dictionary<string, Control> _inputs = new Dictionary<string, Control>();
        private readonly Label _keyLabel = new Label { AutoSize = true, Text = "Bản ghi mới", Margin = new Padding(8) };
        private int? _selectedId;

        internal MasterDataForm(MasterDefinition definition)
        {
            _definition = definition;
            Text = "Danh mục - " + definition.Title; Width = 1000; Height = 680; MinimumSize = new Size(850, 560);
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(8), RowCount = 3, ColumnCount = 1 };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 42)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 58)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 42));
            Controls.Add(root);
            var search = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
            search.Controls.Add(Ui.Label("Tìm kiếm:")); search.Controls.Add(_search);
            search.Controls.Add(Ui.Button("Tìm", (s, e) => LoadRows())); search.Controls.Add(Ui.Button("Làm mới", (s, e) => { _search.Clear(); ClearEditor(); LoadRows(); }));
            root.Controls.Add(search, 0, 0); root.Controls.Add(_grid, 0, 1); Ui.FormatGrid(_grid);
            _grid.SelectionChanged += (s, e) => Ui.SelectRow(_grid, LoadFromRow);

            var editor = new GroupBox { Text = "Thông tin " + definition.Title, Dock = DockStyle.Fill, Padding = new Padding(8) };
            root.Controls.Add(editor, 0, 2);
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, AutoScroll = true, AutoSize = false };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140)); layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140)); layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            editor.Controls.Add(layout);
            layout.Controls.Add(Ui.Label("Mã bản ghi:"), 0, 0); layout.Controls.Add(_keyLabel, 1, 0);
            int row = 1, col = 0;
            foreach (var field in _definition.Fields)
            {
                Control input;
                if (field.IsBit) input = new CheckBox { Text = "Đang hoạt động", AutoSize = true, Checked = true, Margin = new Padding(4) };
                else input = Ui.TextBox(240, field.Multiline);
                _inputs[field.Name] = input;
                layout.Controls.Add(Ui.Label(field.Caption + (field.Required ? " *" : "")), col, row);
                layout.Controls.Add(input, col + 1, row);
                if (col == 0) col = 2;
                else { col = 0; row++; }
            }
            var actions = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 42, FlowDirection = FlowDirection.LeftToRight };
            actions.Controls.Add(Ui.Button("Thêm mới", (s, e) => ClearEditor()));
            actions.Controls.Add(Ui.Button("Lưu", (s, e) => Save()));
            actions.Controls.Add(Ui.Button("Nạp lại", (s, e) => LoadRows()));
            editor.Controls.Add(actions);
            LoadRows();
        }

        internal static MasterDefinition Doctors()
        {
            return new MasterDefinition("Bác sĩ", "BacSi", "MaBS", "HoTen",
                new[] { "MaBS", "HoTen", "ChuyenKhoa", "DienThoai", "ConHanhNghe" },
                new List<MasterField> { new MasterField("HoTen", "Họ tên", 100, true), new MasterField("ChuyenKhoa", "Chuyên khoa", 100), new MasterField("DienThoai", "Điện thoại", 20), new MasterField("ConHanhNghe", "Trạng thái", 1, false, true) });
        }

        internal static MasterDefinition Departments()
        {
            return new MasterDefinition("Khoa điều trị", "KhoaDieuTri", "MaKhoa", "TenKhoa",
                new[] { "MaKhoa", "TenKhoa" }, new List<MasterField> { new MasterField("TenKhoa", "Tên khoa", 100, true) });
        }

        internal static MasterDefinition Employees()
        {
            return new MasterDefinition("Nhân viên CSKH", "NhanVienCSKH", "MaNV", "HoTen",
                new[] { "MaNV", "HoTen", "DienThoai", "Email", "ConHoatDong" },
                new List<MasterField> { new MasterField("HoTen", "Họ tên", 100, true), new MasterField("DienThoai", "Điện thoại", 20), new MasterField("Email", "Email", 254), new MasterField("ConHoatDong", "Trạng thái", 1, false, true) });
        }

        internal static MasterDefinition Medicines()
        {
            return new MasterDefinition("Danh mục thuốc", "Thuoc", "MaThuoc", "TenThuoc",
                new[] { "MaThuoc", "TenThuoc", "DonVi", "HoatChat", "HamLuong", "ConSuDung" },
                new List<MasterField> { new MasterField("TenThuoc", "Tên thuốc", 200, true), new MasterField("DonVi", "Đơn vị", 50, true), new MasterField("HoatChat", "Hoạt chất", 200), new MasterField("HamLuong", "Hàm lượng", 100), new MasterField("ConSuDung", "Còn sử dụng", 1, false, true) });
        }

        private void LoadRows()
        {
            try
            {
                string cols = string.Join(", ", _definition.Columns.Select(c => "[" + c + "]"));
                string sql = "SELECT " + cols + " FROM dbo.[" + _definition.Table + "] WHERE (@q=N'' OR CONVERT(NVARCHAR(30),[" + _definition.PrimaryKey + "]) LIKE @like OR [" + _definition.SearchColumn + "] LIKE @like) ORDER BY [" + _definition.PrimaryKey + "] DESC";
                _grid.DataSource = Db.Query(sql, Db.P("@q", SqlDbType.NVarChar, _search.Text.Trim(), 260), Db.P("@like", SqlDbType.NVarChar, "%" + _search.Text.Trim() + "%", 280));
            }
            catch (Exception ex) { Ui.ShowError(ex); }
        }

        private void LoadFromRow(DataGridViewRow row)
        {
            var pk = row.Cells[_definition.PrimaryKey].Value;
            if (pk == null || pk == DBNull.Value) return;
            _selectedId = Convert.ToInt32(pk); _keyLabel.Text = _selectedId.Value.ToString();
            foreach (var field in _definition.Fields)
            {
                var value = row.Cells[field.Name].Value;
                if (field.IsBit) ((CheckBox)_inputs[field.Name]).Checked = value != DBNull.Value && Convert.ToBoolean(value);
                else ((TextBox)_inputs[field.Name]).Text = value == DBNull.Value ? "" : Convert.ToString(value);
            }
        }

        private void ClearEditor()
        {
            _selectedId = null; _keyLabel.Text = "Bản ghi mới";
            foreach (var field in _definition.Fields)
            {
                if (field.IsBit) ((CheckBox)_inputs[field.Name]).Checked = true;
                else ((TextBox)_inputs[field.Name]).Clear();
            }
            _grid.ClearSelection();
        }

        private void Save()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                foreach (var field in _definition.Fields)
                {
                    var control = _inputs[field.Name];
                    object value;
                    if (field.IsBit) value = ((CheckBox)control).Checked;
                    else
                    {
                        string text = ((TextBox)control).Text.Trim();
                        if (field.Required && text.Length == 0) throw new InvalidOperationException(field.Caption + " không được để trống.");
                        if (text.Length > field.Size) throw new InvalidOperationException(field.Caption + " vượt quá " + field.Size + " ký tự.");
                        value = text.Length == 0 ? (object)DBNull.Value : text;
                    }
                    parameters.Add(Db.P("@" + field.Name, field.IsBit ? SqlDbType.Bit : SqlDbType.NVarChar, value, field.IsBit ? 0 : field.Size));
                }
                if (_selectedId.HasValue)
                {
                    var setters = _definition.Fields.Select(f => "[" + f.Name + "]=@" + f.Name);
                    parameters.Add(Db.P("@pk", SqlDbType.Int, _selectedId.Value));
                    Db.Execute("UPDATE dbo.[" + _definition.Table + "] SET " + string.Join(",", setters) + " WHERE [" + _definition.PrimaryKey + "]=@pk", parameters.ToArray());
                }
                else
                {
                    var cols = _definition.Fields.Select(f => "[" + f.Name + "]");
                    var vals = _definition.Fields.Select(f => "@" + f.Name);
                    Db.Execute("INSERT dbo.[" + _definition.Table + "] (" + string.Join(",", cols) + ") VALUES (" + string.Join(",", vals) + ")", parameters.ToArray());
                }
                Ui.ShowInfo("Đã lưu " + _definition.Title + "."); ClearEditor(); LoadRows();
            }
            catch (Exception ex) { Ui.ShowError(ex, "Lưu danh mục thất bại"); }
        }
    }
}
