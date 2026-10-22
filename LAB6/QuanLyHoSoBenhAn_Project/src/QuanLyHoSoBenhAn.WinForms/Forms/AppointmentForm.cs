using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Data;

namespace QuanLyHoSoBenhAn.Forms
{
    public sealed class AppointmentForm : Form
    {
        private readonly ComboBox _record = Ui.Combo(360);
        private readonly ComboBox _employee = Ui.Combo(300);
        private readonly DateTimePicker _appointment = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm", Width = 180, ShowUpDown = true, Margin = new Padding(4), Value = DateTime.Now.AddDays(1) };
        private readonly ComboBox _status = Ui.Combo(180);
        private readonly DataGridView _grid = Ui.Grid();
        private int? _selectedId;

        public AppointmentForm()
        {
            Text = "Đặt lịch khám định kỳ"; Width = 1120; Height = 700; MinimumSize = new Size(950, 580);
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(8), RowCount = 2, ColumnCount = 1 };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 155)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); Controls.Add(root);
            var box = new GroupBox { Text = "Đặt mới / cập nhật trạng thái lịch", Dock = DockStyle.Fill };
            root.Controls.Add(box, 0, 0);
            var fields = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 4, RowCount = 2 };
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130)); fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120)); fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            box.Controls.Add(fields);
            AddField(fields, 0, 0, "Hồ sơ bệnh án *", _record); AddField(fields, 0, 2, "Nhân viên CSKH *", _employee);
            AddField(fields, 1, 0, "Ngày giờ hẹn *", _appointment); AddField(fields, 1, 2, "Trạng thái", _status);
            _status.Items.AddRange(new object[] { "DaDat", "DaXacNhan", "DaThucHien", "DaHuy", "KhongDen" }); _status.SelectedItem = "DaDat";
            var actions = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 40 };
            actions.Controls.Add(Ui.Button("Đặt lịch", (s, e) => SaveNew())); actions.Controls.Add(Ui.Button("Cập nhật trạng thái", (s, e) => UpdateStatus()));
            actions.Controls.Add(Ui.Button("Mới", (s, e) => { _selectedId = null; _status.SelectedItem = "DaDat"; }));
            actions.Controls.Add(Ui.Button("Nạp lại", (s, e) => { LoadCombos(); LoadRows(); })); box.Controls.Add(actions);
            root.Controls.Add(_grid, 0, 1); _grid.SelectionChanged += (s, e) => Ui.SelectRow(_grid, LoadFromRow);
            LoadCombos(); LoadRows();
        }

        private static void AddField(TableLayoutPanel table, int row, int col, string caption, Control control)
        { table.Controls.Add(Ui.Label(caption), col, row); control.Dock = DockStyle.Top; table.Controls.Add(control, col + 1, row); }

        private void LoadCombos()
        {
            try
            {
                Ui.BindCombo(_record, Db.Query(@"SELECT hs.MaHS, CONCAT(N'HS',hs.MaHS,N' - ',bn.HoTen) AS DisplayText
                        FROM dbo.HoSoBenhAn hs JOIN dbo.BenhNhan bn ON bn.MaBN=hs.MaBN WHERE hs.TrangThai=N'DangHoatDong' ORDER BY hs.MaHS DESC"), "MaHS", "DisplayText");
                Ui.BindCombo(_employee, Db.Query("SELECT MaNV, CONCAT(N'NV',MaNV,N' - ',HoTen) AS DisplayText FROM dbo.NhanVienCSKH WHERE ConHoatDong=1 ORDER BY HoTen"), "MaNV", "DisplayText");
            }
            catch (Exception ex) { Ui.ShowError(ex); }
        }

        private void LoadRows()
        {
            try { _grid.DataSource = Db.Query(@"SELECT l.MaLich, l.MaHS, bn.HoTen AS BenhNhan, l.MaNVDatLich, nv.HoTen AS NhanVienCSKH,
                     l.NgayDat, l.NgayGioHen, l.TrangThai FROM dbo.LichKhamDinhKy l
                     JOIN dbo.HoSoBenhAn hs ON hs.MaHS=l.MaHS JOIN dbo.BenhNhan bn ON bn.MaBN=hs.MaBN
                     JOIN dbo.NhanVienCSKH nv ON nv.MaNV=l.MaNVDatLich ORDER BY l.NgayGioHen DESC"); }
            catch (Exception ex) { Ui.ShowError(ex); }
        }

        private void SaveNew()
        {
            try
            {
                int maHS = Ui.SelectedInt(_record, "hồ sơ bệnh án"); int maNV = Ui.SelectedInt(_employee, "nhân viên CSKH");
                if (_appointment.Value < DateTime.Now.AddMinutes(-1)) throw new InvalidOperationException("Ngày giờ hẹn không được ở trong quá khứ.");
                Db.Execute("INSERT dbo.LichKhamDinhKy(MaHS,MaNVDatLich,NgayGioHen,TrangThai) VALUES(@MaHS,@MaNV,@NgayGioHen,N'DaDat')",
                    Db.P("@MaHS", SqlDbType.Int, maHS), Db.P("@MaNV", SqlDbType.Int, maNV), Db.P("@NgayGioHen", SqlDbType.DateTime2, _appointment.Value));
                Ui.ShowInfo("Đã đặt lịch khám định kỳ."); LoadRows();
            }
            catch (Exception ex) { Ui.ShowError(ex, "Đặt lịch thất bại"); }
        }

        private void LoadFromRow(DataGridViewRow row)
        {
            if (row.Cells["MaLich"].Value == null) return;
            _selectedId = Convert.ToInt32(row.Cells["MaLich"].Value);
            string state = Convert.ToString(row.Cells["TrangThai"].Value);
            if (_status.Items.Contains(state)) _status.SelectedItem = state;
            _appointment.Value = Convert.ToDateTime(row.Cells["NgayGioHen"].Value);
        }

        private void UpdateStatus()
        {
            try
            {
                if (!_selectedId.HasValue) throw new InvalidOperationException("Chọn một lịch khám trong danh sách.");
                if (_status.SelectedItem == null) throw new InvalidOperationException("Chọn trạng thái lịch.");
                Db.Execute("UPDATE dbo.LichKhamDinhKy SET TrangThai=@TrangThai WHERE MaLich=@MaLich",
                    Db.P("@TrangThai", SqlDbType.NVarChar, Convert.ToString(_status.SelectedItem), 20), Db.P("@MaLich", SqlDbType.Int, _selectedId.Value));
                Ui.ShowInfo("Đã cập nhật trạng thái lịch."); LoadRows();
            }
            catch (Exception ex) { Ui.ShowError(ex, "Cập nhật lịch thất bại"); }
        }
    }
}
