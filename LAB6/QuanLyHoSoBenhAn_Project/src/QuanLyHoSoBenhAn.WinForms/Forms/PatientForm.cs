using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Data;

namespace QuanLyHoSoBenhAn.Forms
{
    public sealed class PatientForm : Form
    {
        private readonly DataGridView _grid = Ui.Grid();
        private readonly TextBox _search = Ui.TextBox(280);
        private readonly TextBox _name = Ui.TextBox(250);
        private readonly TextBox _address = Ui.TextBox(250);
        private readonly DateTimePicker _birth = new DateTimePicker { Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false, Width = 250, Margin = new Padding(4) };
        private readonly TextBox _phone = Ui.TextBox(250);
        private readonly TextBox _photo = Ui.TextBox(250);
        private int? _selectedId;

        public PatientForm()
        {
            Text = "Quản lý bệnh nhân"; Width = 1050; Height = 700; MinimumSize = new Size(900, 600);
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3, ColumnCount = 1, Padding = new Padding(8) };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 58));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 42));
            Controls.Add(root);

            var searchPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
            searchPanel.Controls.Add(Ui.Label("Tìm theo mã, họ tên hoặc điện thoại:"));
            searchPanel.Controls.Add(_search);
            searchPanel.Controls.Add(Ui.Button("Tìm kiếm", (s, e) => LoadPatients()));
            searchPanel.Controls.Add(Ui.Button("Làm mới", (s, e) => { _search.Clear(); ClearEditor(); LoadPatients(); }));
            root.Controls.Add(searchPanel, 0, 0);
            root.Controls.Add(_grid, 0, 1);
            Ui.FormatGrid(_grid);
            _grid.SelectionChanged += (s, e) => Ui.SelectRow(_grid, LoadFromRow);

            var editor = new GroupBox { Text = "Thông tin bệnh nhân", Dock = DockStyle.Fill, Padding = new Padding(10) };
            root.Controls.Add(editor, 0, 2);
            var table = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 4, RowCount = 3 };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120)); table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120)); table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            editor.Controls.Add(table);
            AddField(table, 0, 0, "Họ tên *", _name);
            AddField(table, 0, 2, "Địa chỉ", _address);
            AddField(table, 1, 0, "Ngày sinh", _birth);
            AddField(table, 1, 2, "Điện thoại", _phone);
            AddField(table, 2, 0, "Ảnh 3x4 (đường dẫn)", _photo);
            var actions = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 43, FlowDirection = FlowDirection.LeftToRight };
            actions.Controls.Add(Ui.Button("Thêm mới", (s, e) => ClearEditor()));
            actions.Controls.Add(Ui.Button("Lưu bệnh nhân", (s, e) => SavePatient()));
            actions.Controls.Add(Ui.Button("Nạp lại", (s, e) => LoadPatients()));
            editor.Controls.Add(actions);
            LoadPatients();
        }

        private static void AddField(TableLayoutPanel table, int row, int col, string label, Control control)
        {
            table.Controls.Add(Ui.Label(label), col, row); control.Dock = DockStyle.Top; table.Controls.Add(control, col + 1, row);
        }

        private void LoadPatients()
        {
            try
            {
                string text = _search.Text.Trim();
                _grid.DataSource = Db.Query(
                    @"SELECT MaBN, HoTen, DiaChi, NgaySinh, DienThoai, Anh3x4, NgayTao
                      FROM dbo.BenhNhan
                      WHERE (@q = N'' OR CONVERT(NVARCHAR(20), MaBN) LIKE @like OR HoTen LIKE @like OR DienThoai LIKE @like)
                      ORDER BY MaBN DESC",
                    Db.P("@q", SqlDbType.NVarChar, text, 250), Db.P("@like", SqlDbType.NVarChar, "%" + text + "%", 260));
            }
            catch (Exception ex) { Ui.ShowError(ex); }
        }

        private void LoadFromRow(DataGridViewRow row)
        {
            if (row.Cells["MaBN"].Value == null) return;
            _selectedId = Convert.ToInt32(row.Cells["MaBN"].Value);
            _name.Text = Convert.ToString(row.Cells["HoTen"].Value);
            _address.Text = Convert.ToString(row.Cells["DiaChi"].Value);
            _phone.Text = Convert.ToString(row.Cells["DienThoai"].Value);
            _photo.Text = Convert.ToString(row.Cells["Anh3x4"].Value);
            if (row.Cells["NgaySinh"].Value == DBNull.Value || row.Cells["NgaySinh"].Value == null) _birth.Checked = false;
            else { _birth.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value); _birth.Checked = true; }
        }

        private void ClearEditor()
        {
            _selectedId = null; _name.Clear(); _address.Clear(); _phone.Clear(); _photo.Clear(); _birth.Checked = false; _grid.ClearSelection(); _name.Focus();
        }

        private void SavePatient()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_name.Text)) throw new InvalidOperationException("Họ tên bệnh nhân không được để trống.");
                if (_phone.Text.Trim().Length > 20) throw new InvalidOperationException("Điện thoại tối đa 20 ký tự.");
                if (_selectedId.HasValue)
                {
                    Db.Execute(@"UPDATE dbo.BenhNhan SET HoTen=@HoTen, DiaChi=@DiaChi, NgaySinh=@NgaySinh,
                                 DienThoai=@DienThoai, Anh3x4=@Anh3x4 WHERE MaBN=@MaBN",
                        Db.P("@HoTen", SqlDbType.NVarChar, _name.Text.Trim(), 100),
                        Db.P("@DiaChi", SqlDbType.NVarChar, Ui.NullableText(_address.Text), 250),
                        Db.P("@NgaySinh", SqlDbType.Date, _birth.Checked ? (object)_birth.Value.Date : DBNull.Value),
                        Db.P("@DienThoai", SqlDbType.VarChar, Ui.NullableText(_phone.Text), 20),
                        Db.P("@Anh3x4", SqlDbType.NVarChar, Ui.NullableText(_photo.Text), 500),
                        Db.P("@MaBN", SqlDbType.Int, _selectedId.Value));
                    Ui.ShowInfo("Đã cập nhật thông tin bệnh nhân.");
                }
                else
                {
                    object newId = Db.Scalar(@"INSERT dbo.BenhNhan(HoTen, DiaChi, NgaySinh, DienThoai, Anh3x4)
                                                VALUES(@HoTen,@DiaChi,@NgaySinh,@DienThoai,@Anh3x4);
                                                SELECT CONVERT(INT,SCOPE_IDENTITY());",
                        Db.P("@HoTen", SqlDbType.NVarChar, _name.Text.Trim(), 100),
                        Db.P("@DiaChi", SqlDbType.NVarChar, Ui.NullableText(_address.Text), 250),
                        Db.P("@NgaySinh", SqlDbType.Date, _birth.Checked ? (object)_birth.Value.Date : DBNull.Value),
                        Db.P("@DienThoai", SqlDbType.VarChar, Ui.NullableText(_phone.Text), 20),
                        Db.P("@Anh3x4", SqlDbType.NVarChar, Ui.NullableText(_photo.Text), 500));
                    Ui.ShowInfo("Đã tạo bệnh nhân. Mã bệnh nhân: " + Convert.ToInt32(newId) + ". Ghi nhận lệ phí năm trước khi cấp hồ sơ bệnh án.");
                    ClearEditor();
                }
                LoadPatients();
            }
            catch (Exception ex) { Ui.ShowError(ex, "Lưu bệnh nhân thất bại"); }
        }
    }
}
