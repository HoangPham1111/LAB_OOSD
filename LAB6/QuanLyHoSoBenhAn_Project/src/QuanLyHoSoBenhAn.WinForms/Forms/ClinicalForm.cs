using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Data;

namespace QuanLyHoSoBenhAn.Forms
{
    public sealed class ClinicalForm : Form
    {
        private readonly ComboBox _record = Ui.Combo(330);
        private readonly ComboBox _doctor = Ui.Combo(250);
        private readonly ComboBox _department = Ui.Combo(230);
        private readonly TextBox _symptoms = Ui.TextBox(300, true);
        private readonly TextBox _bloodPressure = Ui.TextBox(160);
        private readonly TextBox _notes = Ui.TextBox(260);
        private readonly CheckBox _hasTemp = new CheckBox { Text = "Nhập thân nhiệt", AutoSize = true, Checked = false, Margin = new Padding(4, 8, 4, 4) };
        private readonly NumericUpDown _temperature = new NumericUpDown { Minimum = 25, Maximum = 45, DecimalPlaces = 1, Value = 36.5M, Increment = 0.1M, Width = 100, Enabled = false, Margin = new Padding(4) };
        private readonly ComboBox _episodeType = Ui.Combo(180);
        private readonly DataGridView _grid = Ui.Grid();

        public ClinicalForm()
        {
            Text = "Khám bệnh và lập phiếu khám"; Width = 1200; Height = 760; MinimumSize = new Size(1000, 650);
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(8), RowCount = 2, ColumnCount = 1 };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 290)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); Controls.Add(root);
            var group = new GroupBox { Text = "Tạo đợt điều trị và phiếu khám", Dock = DockStyle.Fill, Padding = new Padding(8) }; root.Controls.Add(group, 0, 0);
            var body = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 4, RowCount = 5, Padding = new Padding(4) };
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120)); body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120)); body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); group.Controls.Add(body);
            AddField(body, 0, 0, "Hồ sơ bệnh án *", _record); AddField(body, 0, 2, "Bác sĩ *", _doctor);
            AddField(body, 1, 0, "Khoa điều trị *", _department); AddField(body, 1, 2, "Loại đợt", _episodeType);
            AddField(body, 2, 0, "Triệu chứng", _symptoms); AddField(body, 2, 2, "Huyết áp", _bloodPressure);
            var tempPanel = new FlowLayoutPanel { AutoSize = true, WrapContents = false };
            tempPanel.Controls.Add(_hasTemp); tempPanel.Controls.Add(_temperature);
            AddField(body, 3, 0, "Thân nhiệt", tempPanel); AddField(body, 3, 2, "Ghi chú", _notes);
            _episodeType.Items.AddRange(new object[] { "DieuTri", "KhamDinhKy" }); _episodeType.SelectedItem = "DieuTri";
            _hasTemp.CheckedChanged += (s, e) => _temperature.Enabled = _hasTemp.Checked;
            var actions = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 42 };
            actions.Controls.Add(Ui.Button("Tạo đợt điều trị + phiếu khám", (s, e) => CreateVisit()));
            actions.Controls.Add(Ui.Button("Hoàn tất đợt (cần đã thanh toán)", (s, e) => CloseSelectedEpisode()));
            actions.Controls.Add(Ui.Button("Nạp lại", (s, e) => { LoadCombos(); LoadVisits(); })); group.Controls.Add(actions);
            root.Controls.Add(_grid, 0, 1); Ui.FormatGrid(_grid); LoadCombos(); LoadVisits();
        }

        private static void AddField(TableLayoutPanel table, int row, int col, string caption, Control control)
        {
            table.Controls.Add(Ui.Label(caption), col, row); control.Dock = DockStyle.Top; table.Controls.Add(control, col + 1, row);
        }

        private void LoadCombos()
        {
            try
            {
                Ui.BindCombo(_record, Db.Query(@"SELECT hs.MaHS, CONCAT(N'HS',hs.MaHS,N' - ',bn.HoTen) AS DisplayText
                      FROM dbo.HoSoBenhAn hs JOIN dbo.BenhNhan bn ON bn.MaBN=hs.MaBN
                      WHERE hs.TrangThai=N'DangHoatDong' ORDER BY hs.MaHS DESC"), "MaHS", "DisplayText");
                Ui.BindCombo(_doctor, Db.Query("SELECT MaBS, CONCAT(N'BS',MaBS,N' - ',HoTen) AS DisplayText FROM dbo.BacSi WHERE ConHanhNghe=1 ORDER BY HoTen"), "MaBS", "DisplayText");
                Ui.BindCombo(_department, Db.Query("SELECT MaKhoa, TenKhoa AS DisplayText FROM dbo.KhoaDieuTri ORDER BY TenKhoa"), "MaKhoa", "DisplayText");
            }
            catch (Exception ex) { Ui.ShowError(ex); }
        }

        private void LoadVisits()
        {
            try
            {
                _grid.DataSource = Db.Query(@"SELECT TOP (500) pk.MaPK, dt.MaDT, hs.MaHS, bn.HoTen AS BenhNhan,
                     bs.HoTen AS BacSi, k.TenKhoa, pk.NgayGioKham, pk.TrieuChung, dt.TrangThai AS TrangThaiDot
                     FROM dbo.PhieuKham pk JOIN dbo.DotDieuTri dt ON dt.MaDT=pk.MaDT
                     JOIN dbo.HoSoBenhAn hs ON hs.MaHS=dt.MaHS JOIN dbo.BenhNhan bn ON bn.MaBN=hs.MaBN
                     JOIN dbo.BacSi bs ON bs.MaBS=pk.MaBS JOIN dbo.KhoaDieuTri k ON k.MaKhoa=pk.MaKhoa
                     ORDER BY pk.MaPK DESC");
            }
            catch (Exception ex) { Ui.ShowError(ex); }
        }

        private void CreateVisit()
        {
            try
            {
                int maHS = Ui.SelectedInt(_record, "hồ sơ bệnh án");
                int maBS = Ui.SelectedInt(_doctor, "bác sĩ");
                int maKhoa = Ui.SelectedInt(_department, "khoa điều trị");
                using (var conn = Db.CreateConnection())
                using (var cmd = new SqlCommand("dbo.usp_TaoDotVaPhieuKham", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@MaHS", SqlDbType.Int).Value = maHS;
                    cmd.Parameters.Add("@MaBS", SqlDbType.Int).Value = maBS;
                    cmd.Parameters.Add("@MaKhoa", SqlDbType.Int).Value = maKhoa;
                    cmd.Parameters.Add("@TrieuChung", SqlDbType.NVarChar, 1000).Value = Ui.NullableText(_symptoms.Text);
                    var temp = cmd.Parameters.Add("@ThanNhiet", SqlDbType.Decimal); temp.Precision = 4; temp.Scale = 1; temp.Value = _hasTemp.Checked ? (object)_temperature.Value : DBNull.Value;
                    cmd.Parameters.Add("@HuyetAp", SqlDbType.NVarChar, 30).Value = Ui.NullableText(_bloodPressure.Text);
                    cmd.Parameters.Add("@GhiChu", SqlDbType.NVarChar, 1000).Value = Ui.NullableText(_notes.Text);
                    cmd.Parameters.Add("@LoaiDot", SqlDbType.NVarChar, 20).Value = Convert.ToString(_episodeType.SelectedItem ?? "DieuTri");
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) Ui.ShowInfo("Đã tạo đợt điều trị mã " + reader["MaDT"] + " và phiếu khám mã " + reader["MaPK"] + ".");
                    }
                }
                LoadVisits();
            }
            catch (Exception ex) { Ui.ShowError(ex, "Không thể tạo phiếu khám"); }
        }

        private void CloseSelectedEpisode()
        {
            try
            {
                if (_grid.CurrentRow == null || _grid.CurrentRow.Cells["MaDT"].Value == null)
                    throw new InvalidOperationException("Chọn đợt điều trị trong danh sách bên dưới.");
                int maDT = Convert.ToInt32(_grid.CurrentRow.Cells["MaDT"].Value);
                var confirm = MessageBox.Show("Chỉ đóng đợt khi hóa đơn viện phí đã thanh toán đầy đủ. Tiếp tục?", "Xác nhận hoàn tất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;
                object result = Db.Scalar("EXEC dbo.usp_DongDotDieuTri @MaDT, @KetQuaTongKet",
                    Db.P("@MaDT", SqlDbType.Int, maDT), Db.P("@KetQuaTongKet", SqlDbType.NVarChar, Ui.NullableText(_notes.Text), 1000));
                Ui.ShowInfo("Đã đóng đợt điều trị " + maDT + "."); LoadVisits();
            }
            catch (Exception ex) { Ui.ShowError(ex, "Chưa thể đóng đợt điều trị"); }
        }
    }
}
