using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Data;

namespace QuanLyHoSoBenhAn.Forms
{
    public sealed class TestForm : Form
    {
        private readonly ComboBox _visit = Ui.Combo(340);
        private readonly TextBox _testName = Ui.TextBox(280);
        private readonly TextBox _result = Ui.TextBox(420, true);
        private readonly ComboBox _status = Ui.Combo(190);
        private readonly DataGridView _grid = Ui.Grid();
        private int? _selectedId;

        public TestForm()
        {
            Text = "Chỉ định và kết quả xét nghiệm"; Width = 1120; Height = 720; MinimumSize = new Size(950, 600);
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(8), RowCount = 2, ColumnCount = 1 };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 230)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); Controls.Add(root);
            var box = new GroupBox { Text = "Tạo chỉ định / cập nhật kết quả", Dock = DockStyle.Fill, Padding = new Padding(8) };
            root.Controls.Add(box, 0, 0);
            var fields = new TableLayoutPanel { Dock = DockStyle.Top, ColumnCount = 4, RowCount = 3, AutoSize = true };
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140)); fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120)); fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            box.Controls.Add(fields);
            AddField(fields, 0, 0, "Phiếu khám *", _visit); AddField(fields, 0, 2, "Tên xét nghiệm *", _testName);
            AddField(fields, 1, 0, "Kết quả", _result); AddField(fields, 1, 2, "Trạng thái", _status);
            _status.Items.AddRange(new object[] { "ChoThucHien", "DangThucHien", "DaCoKetQua", "Huy" }); _status.SelectedItem = "ChoThucHien";
            var actions = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 40 };
            actions.Controls.Add(Ui.Button("Tạo chỉ định", (s, e) => CreateOrder()));
            actions.Controls.Add(Ui.Button("Lưu kết quả / trạng thái", (s, e) => SaveResult()));
            actions.Controls.Add(Ui.Button("Mới", (s, e) => ClearEditor()));
            actions.Controls.Add(Ui.Button("Nạp lại", (s, e) => { LoadVisits(); LoadTests(); })); box.Controls.Add(actions);
            root.Controls.Add(_grid, 0, 1); _grid.SelectionChanged += (s, e) => Ui.SelectRow(_grid, LoadFromRow);
            LoadVisits(); LoadTests();
        }

        private static void AddField(TableLayoutPanel table, int row, int col, string caption, Control control)
        { table.Controls.Add(Ui.Label(caption), col, row); control.Dock = DockStyle.Top; table.Controls.Add(control, col + 1, row); }

        private void LoadVisits()
        {
            try { Ui.BindCombo(_visit, Db.Query(@"SELECT pk.MaPK, CONCAT(N'PK',pk.MaPK,N' - ',bn.HoTen,N' - ',CONVERT(NVARCHAR(10),pk.NgayGioKham,103)) AS DisplayText
                         FROM dbo.PhieuKham pk JOIN dbo.DotDieuTri dt ON dt.MaDT=pk.MaDT JOIN dbo.HoSoBenhAn hs ON hs.MaHS=dt.MaHS
                         JOIN dbo.BenhNhan bn ON bn.MaBN=hs.MaBN ORDER BY pk.MaPK DESC"), "MaPK", "DisplayText"); }
            catch (Exception ex) { Ui.ShowError(ex); }
        }

        private void LoadTests()
        {
            try { _grid.DataSource = Db.Query(@"SELECT xn.MaXN, xn.MaPK, bn.HoTen AS BenhNhan, xn.TenXetNghiem, xn.NgayChiDinh,
                    xn.NgayThucHien, xn.KetQua, xn.TrangThai
                    FROM dbo.XetNghiem xn JOIN dbo.PhieuKham pk ON pk.MaPK=xn.MaPK
                    JOIN dbo.DotDieuTri dt ON dt.MaDT=pk.MaDT JOIN dbo.HoSoBenhAn hs ON hs.MaHS=dt.MaHS
                    JOIN dbo.BenhNhan bn ON bn.MaBN=hs.MaBN ORDER BY xn.MaXN DESC"); }
            catch (Exception ex) { Ui.ShowError(ex); }
        }

        private void CreateOrder()
        {
            try
            {
                int maPK = Ui.SelectedInt(_visit, "phiếu khám");
                if (string.IsNullOrWhiteSpace(_testName.Text)) throw new InvalidOperationException("Tên xét nghiệm không được để trống.");
                Db.Execute("INSERT dbo.XetNghiem(MaPK,TenXetNghiem) VALUES(@MaPK,@TenXetNghiem)",
                    Db.P("@MaPK", SqlDbType.Int, maPK), Db.P("@TenXetNghiem", SqlDbType.NVarChar, _testName.Text.Trim(), 200));
                Ui.ShowInfo("Đã tạo chỉ định xét nghiệm."); _testName.Clear(); LoadTests();
            }
            catch (Exception ex) { Ui.ShowError(ex, "Không thể tạo chỉ định"); }
        }

        private void LoadFromRow(DataGridViewRow row)
        {
            if (row.Cells["MaXN"].Value == null) return;
            _selectedId = Convert.ToInt32(row.Cells["MaXN"].Value);
            _testName.Text = Convert.ToString(row.Cells["TenXetNghiem"].Value);
            _result.Text = row.Cells["KetQua"].Value == DBNull.Value ? "" : Convert.ToString(row.Cells["KetQua"].Value);
            string state = Convert.ToString(row.Cells["TrangThai"].Value);
            if (_status.Items.Contains(state)) _status.SelectedItem = state;
            for (int i = 0; i < _visit.Items.Count; i++)
            {
                var drv = _visit.Items[i] as DataRowView;
                if (drv != null && Convert.ToInt32(drv["MaPK"]) == Convert.ToInt32(row.Cells["MaPK"].Value)) { _visit.SelectedIndex = i; break; }
            }
        }

        private void SaveResult()
        {
            try
            {
                if (!_selectedId.HasValue) throw new InvalidOperationException("Chọn một xét nghiệm trong danh sách bên dưới.");
                if (_status.SelectedItem == null) throw new InvalidOperationException("Chọn trạng thái xét nghiệm.");
                using (var conn = Db.CreateConnection())
                using (var cmd = new SqlCommand("dbo.usp_GhiNhanKetQuaXetNghiem", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@MaXN", SqlDbType.Int).Value = _selectedId.Value;
                    cmd.Parameters.Add("@KetQua", SqlDbType.NVarChar, -1).Value = Ui.NullableText(_result.Text);
                    cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 20).Value = Convert.ToString(_status.SelectedItem);
                    conn.Open(); cmd.ExecuteNonQuery();
                }
                Ui.ShowInfo("Đã cập nhật xét nghiệm."); LoadTests();
            }
            catch (Exception ex) { Ui.ShowError(ex, "Cập nhật xét nghiệm thất bại"); }
        }

        private void ClearEditor()
        {
            _selectedId = null; _testName.Clear(); _result.Clear(); _status.SelectedItem = "ChoThucHien";
        }
    }
}
