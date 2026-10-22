using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Data;

namespace QuanLyHoSoBenhAn.Forms
{
    public sealed class PrescriptionForm : Form
    {
        private readonly DataGridView _encounters = Ui.Grid();
        private readonly DataGridView _detailsGrid = Ui.Grid();
        private readonly DataTable _details = CreateDetailTable();
        private readonly TextBox _visitId = Ui.TextBox(100);
        private readonly DateTimePicker _date = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 140, Margin = new Padding(4) };
        private readonly TextBox _advice = Ui.TextBox(320);
        private readonly ComboBox _drug = Ui.Combo(280);
        private readonly TextBox _dose = Ui.TextBox(180);
        private readonly TextBox _method = Ui.TextBox(240);
        private readonly NumericUpDown _quantity = new NumericUpDown { Minimum = 0.01M, Maximum = 100000M, DecimalPlaces = 2, Value = 1, Width = 100, Margin = new Padding(4) };
        private readonly NumericUpDown _days = new NumericUpDown { Minimum = 1, Maximum = 365, Value = 1, Width = 80, Margin = new Padding(4) };

        public PrescriptionForm()
        {
            Text = "Lập toa thuốc"; Width = 1350; Height = 820; MinimumSize = new Size(1100, 700);
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(8), RowCount = 2, ColumnCount = 1 };
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 42)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 58)); Controls.Add(root);
            var encounterBox = new GroupBox { Text = "Phiếu khám chưa có toa thuốc - chọn một dòng để lập toa", Dock = DockStyle.Fill };
            encounterBox.Controls.Add(_encounters); root.Controls.Add(encounterBox, 0, 0); Ui.FormatGrid(_encounters);
            _encounters.SelectionChanged += (s, e) =>
            {
                if (_encounters.CurrentRow != null && _encounters.CurrentRow.Cells["MaPK"].Value != null)
                    _visitId.Text = Convert.ToString(_encounters.CurrentRow.Cells["MaPK"].Value);
            };

            var lower = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            lower.RowStyles.Add(new RowStyle(SizeType.Absolute, 62)); lower.RowStyles.Add(new RowStyle(SizeType.Absolute, 110)); lower.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.Controls.Add(lower, 0, 1);
            var header = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, AutoScroll = true };
            header.Controls.Add(Ui.Label("Mã phiếu khám *")); _visitId.ReadOnly = true; header.Controls.Add(_visitId);
            header.Controls.Add(Ui.Label("Ngày kê")); header.Controls.Add(_date);
            header.Controls.Add(Ui.Label("Lời dặn")); header.Controls.Add(_advice);
            header.Controls.Add(Ui.Button("Lưu toa thuốc", (s, e) => SavePrescription()));
            header.Controls.Add(Ui.Button("Làm mới", (s, e) => { ClearPrescription(); LoadEncounters(); }));
            lower.Controls.Add(header, 0, 0);

            var lineBox = new GroupBox { Text = "Thêm thuốc vào toa", Dock = DockStyle.Fill };
            lower.Controls.Add(lineBox, 0, 1);
            var line = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = true, AutoScroll = true, Padding = new Padding(4) };
            lineBox.Controls.Add(line);
            line.Controls.Add(Ui.Label("Thuốc *")); line.Controls.Add(_drug);
            line.Controls.Add(Ui.Label("Liều dùng *")); line.Controls.Add(_dose);
            line.Controls.Add(Ui.Label("Cách dùng *")); line.Controls.Add(_method);
            line.Controls.Add(Ui.Label("Số lượng *")); line.Controls.Add(_quantity);
            line.Controls.Add(Ui.Label("Số ngày *")); line.Controls.Add(_days);
            line.Controls.Add(Ui.Button("Thêm dòng thuốc", (s, e) => AddDetail()));
            line.Controls.Add(Ui.Button("Xóa dòng chọn", (s, e) => RemoveSelectedDetail()));

            var detailsBox = new GroupBox { Text = "Chi tiết toa thuốc", Dock = DockStyle.Fill };
            detailsBox.Controls.Add(_detailsGrid); lower.Controls.Add(detailsBox, 0, 2);
            _detailsGrid.DataSource = _details; Ui.FormatGrid(_detailsGrid);
            LoadDrugs(); LoadEncounters();
        }

        private static DataTable CreateDetailTable()
        {
            var table = new DataTable();
            table.Columns.Add("MaThuoc", typeof(int));
            table.Columns.Add("TenThuoc", typeof(string));
            table.Columns.Add("LieuDung", typeof(string));
            table.Columns.Add("CachDung", typeof(string));
            table.Columns.Add("SoLuong", typeof(decimal));
            table.Columns.Add("SoNgay", typeof(int));
            return table;
        }

        private void LoadDrugs()
        {
            try
            {
                Ui.BindCombo(_drug, Db.Query("SELECT MaThuoc, CONCAT(TenThuoc, N' (', DonVi, N')') AS DisplayText FROM dbo.Thuoc WHERE ConSuDung=1 ORDER BY TenThuoc"), "MaThuoc", "DisplayText");
            }
            catch (Exception ex) { Ui.ShowError(ex); }
        }

        private void LoadEncounters()
        {
            try
            {
                _encounters.DataSource = Db.Query(@"SELECT pk.MaPK, dt.MaDT, hs.MaHS, bn.HoTen AS BenhNhan, bs.HoTen AS BacSi,
                      k.TenKhoa, pk.NgayGioKham, dt.TrangThai AS TrangThaiDot
                      FROM dbo.PhieuKham pk
                      JOIN dbo.DotDieuTri dt ON dt.MaDT=pk.MaDT
                      JOIN dbo.HoSoBenhAn hs ON hs.MaHS=dt.MaHS
                      JOIN dbo.BenhNhan bn ON bn.MaBN=hs.MaBN
                      JOIN dbo.BacSi bs ON bs.MaBS=pk.MaBS
                      JOIN dbo.KhoaDieuTri k ON k.MaKhoa=pk.MaKhoa
                      LEFT JOIN dbo.ToaThuoc toa ON toa.MaPK=pk.MaPK
                      WHERE toa.MaToa IS NULL AND dt.TrangThai<>N'DaDong'
                      ORDER BY pk.MaPK DESC");
            }
            catch (Exception ex) { Ui.ShowError(ex); }
        }

        private void AddDetail()
        {
            try
            {
                int maThuoc = Ui.SelectedInt(_drug, "thuốc");
                string label = Convert.ToString(((DataRowView)_drug.SelectedItem)["DisplayText"]);
                if (string.IsNullOrWhiteSpace(_dose.Text)) throw new InvalidOperationException("Liều dùng không được để trống.");
                if (string.IsNullOrWhiteSpace(_method.Text)) throw new InvalidOperationException("Cách dùng không được để trống.");
                if (_details.AsEnumerable().Any(r => r.Field<int>("MaThuoc") == maThuoc))
                    throw new InvalidOperationException("Thuốc này đã có trong danh sách toa. Mỗi thuốc chỉ được có một dòng trong toa theo mô hình hiện tại.");
                _details.Rows.Add(maThuoc, label, _dose.Text.Trim(), _method.Text.Trim(), _quantity.Value, Convert.ToInt32(_days.Value));
                _dose.Clear(); _method.Clear(); _quantity.Value = 1; _days.Value = 1;
            }
            catch (Exception ex) { Ui.ShowError(ex, "Không thể thêm thuốc"); }
        }

        private void RemoveSelectedDetail()
        {
            if (_detailsGrid.CurrentRow == null || _detailsGrid.CurrentRow.Index < 0) return;
            int index = _detailsGrid.CurrentRow.Index;
            if (index >= 0 && index < _details.Rows.Count) _details.Rows.RemoveAt(index);
        }

        private void SavePrescription()
        {
            try
            {
                int maPK;
                if (!int.TryParse(_visitId.Text, out maPK) || maPK <= 0) throw new InvalidOperationException("Chọn một phiếu khám chưa có toa thuốc ở danh sách phía trên.");
                if (_details.Rows.Count == 0) throw new InvalidOperationException("Toa thuốc cần ít nhất một dòng thuốc.");

                var tvp = new DataTable();
                tvp.Columns.Add("MaThuoc", typeof(int));
                tvp.Columns.Add("LieuDung", typeof(string));
                tvp.Columns.Add("CachDung", typeof(string));
                tvp.Columns.Add("SoLuong", typeof(decimal));
                tvp.Columns.Add("SoNgay", typeof(int));
                foreach (DataRow row in _details.Rows)
                    tvp.Rows.Add(row.Field<int>("MaThuoc"), row.Field<string>("LieuDung"), row.Field<string>("CachDung"), row.Field<decimal>("SoLuong"), row.Field<int>("SoNgay"));

                using (var conn = Db.CreateConnection())
                using (var cmd = new SqlCommand("dbo.usp_LapToaThuoc", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@MaPK", SqlDbType.Int).Value = maPK;
                    cmd.Parameters.Add("@NgayKe", SqlDbType.Date).Value = _date.Value.Date;
                    cmd.Parameters.Add("@LoiDan", SqlDbType.NVarChar, 1000).Value = Ui.NullableText(_advice.Text);
                    var pDetails = cmd.Parameters.Add("@ChiTiet", SqlDbType.Structured);
                    pDetails.TypeName = "dbo.ToaThuocChiTietType";
                    pDetails.Value = tvp;
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    Ui.ShowInfo("Đã lưu toa thuốc. Mã toa: " + Convert.ToInt32(result) + ".");
                }
                ClearPrescription(); LoadEncounters();
            }
            catch (Exception ex) { Ui.ShowError(ex, "Lập toa thuốc thất bại"); }
        }

        private void ClearPrescription()
        {
            _visitId.Clear(); _advice.Clear(); _details.Clear(); _date.Value = DateTime.Today;
        }
    }
}
