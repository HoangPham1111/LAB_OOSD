using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Data;

namespace QuanLyHoSoBenhAn.Forms
{
    public sealed class AnnualFeeForm : Form
    {
        private readonly ComboBox _patient = Ui.Combo(360);
        private readonly NumericUpDown _year = new NumericUpDown { Minimum = 2000, Maximum = 2200, Value = DateTime.Today.Year, Width = 100, Margin = new Padding(4) };
        private readonly NumericUpDown _amount = new NumericUpDown { Minimum = 0, Maximum = 1000000000, DecimalPlaces = 2, ThousandsSeparator = true, Width = 180, Margin = new Padding(4) };
        private readonly ComboBox _status = Ui.Combo(180);
        private readonly DataGridView _grid = Ui.Grid();

        public AnnualFeeForm()
        {
            Text = "Ghi nhận lệ phí năm"; Width = 1000; Height = 650; MinimumSize = new Size(850, 550);
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), RowCount = 2, ColumnCount = 1 };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 165)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); Controls.Add(root);
            var box = new GroupBox { Text = "Lệ phí quản lý / khám định kỳ theo năm", Dock = DockStyle.Fill };
            root.Controls.Add(box, 0, 0);
            var fields = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 4, RowCount = 2, Padding = new Padding(5) };
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 135)); fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110)); fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            box.Controls.Add(fields);
            fields.Controls.Add(Ui.Label("Bệnh nhân *"), 0, 0); fields.Controls.Add(_patient, 1, 0);
            fields.Controls.Add(Ui.Label("Năm áp dụng *"), 2, 0); fields.Controls.Add(_year, 3, 0);
            fields.Controls.Add(Ui.Label("Số tiền *"), 0, 1); fields.Controls.Add(_amount, 1, 1);
            fields.Controls.Add(Ui.Label("Trạng thái"), 2, 1); fields.Controls.Add(_status, 3, 1);
            _status.Items.AddRange(new object[] { "ChuaDong", "DaDong", "Huy" }); _status.SelectedItem = "DaDong";
            var actions = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 42 };
            actions.Controls.Add(Ui.Button("Ghi nhận / cập nhật", (s, e) => Save()));
            actions.Controls.Add(Ui.Button("Nạp lại", (s, e) => { LoadPatients(); LoadRows(); })); box.Controls.Add(actions);
            root.Controls.Add(_grid, 0, 1); LoadPatients(); LoadRows();
        }

        private void LoadPatients()
        {
            try { Ui.BindCombo(_patient, Db.Query("SELECT MaBN, CONCAT(N'BN', MaBN, N' - ', HoTen) AS DisplayText FROM dbo.BenhNhan ORDER BY HoTen"), "MaBN", "DisplayText"); }
            catch (Exception ex) { Ui.ShowError(ex); }
        }

        private void LoadRows()
        {
            try { _grid.DataSource = Db.Query(@"SELECT lp.MaLePhi, lp.MaBN, bn.HoTen, lp.NamApDung, lp.NgayDong, lp.SoTien, lp.TrangThai
                  FROM dbo.LePhiNam lp INNER JOIN dbo.BenhNhan bn ON bn.MaBN=lp.MaBN ORDER BY lp.NamApDung DESC, bn.HoTen"); }
            catch (Exception ex) { Ui.ShowError(ex); }
        }

        private void Save()
        {
            try
            {
                int maBN = Ui.SelectedInt(_patient, "bệnh nhân");
                if (_status.SelectedItem == null) throw new InvalidOperationException("Vui lòng chọn trạng thái lệ phí.");
                Db.Execute("EXEC dbo.usp_GhiNhanLePhiNam @MaBN=@MaBN, @NamApDung=@NamApDung, @SoTien=@SoTien, @TrangThai=@TrangThai", new SqlParameter("@MaBN", SqlDbType.Int) { Value = maBN },
                    new SqlParameter("@NamApDung", SqlDbType.SmallInt) { Value = Convert.ToInt16(_year.Value) },
                    Db.DecimalP("@SoTien", _amount.Value, 12, 2),
                    new SqlParameter("@TrangThai", SqlDbType.NVarChar, 20) { Value = Convert.ToString(_status.SelectedItem) })
                    ;
                Ui.ShowInfo("Đã ghi nhận lệ phí năm."); LoadRows();
            }
            catch (Exception ex) { Ui.ShowError(ex, "Ghi nhận lệ phí thất bại"); }
        }
    }
}
