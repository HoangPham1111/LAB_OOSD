using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Data;

namespace QuanLyHoSoBenhAn.Forms
{
    public sealed class HospitalFeeForm : Form
    {
        private readonly ComboBox _episode = Ui.Combo(360);
        private readonly NumericUpDown _total = new NumericUpDown { Minimum = 0, Maximum = 1000000000, DecimalPlaces = 2, ThousandsSeparator = true, Width = 160, Margin = new Padding(4) };
        private readonly ComboBox _invoice = Ui.Combo(420);
        private readonly NumericUpDown _payment = new NumericUpDown { Minimum = 0.01M, Maximum = 1000000000, DecimalPlaces = 2, ThousandsSeparator = true, Width = 160, Margin = new Padding(4), Value = 1 };
        private readonly ComboBox _method = Ui.Combo(180);
        private readonly DataGridView _grid = Ui.Grid();

        public HospitalFeeForm()
        {
            Text = "Hóa đơn viện phí và thanh toán"; Width = 1180; Height = 730; MinimumSize = new Size(1000, 610);
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(8), RowCount = 2, ColumnCount = 1 };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 230)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); Controls.Add(root);
            var box = new GroupBox { Text = "Lập hóa đơn / ghi nhận thanh toán", Dock = DockStyle.Fill, Padding = new Padding(8) }; root.Controls.Add(box, 0, 0);
            var fields = new TableLayoutPanel { Dock = DockStyle.Top, ColumnCount = 4, RowCount = 3, AutoSize = true };
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 135)); fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120)); fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); box.Controls.Add(fields);
            AddField(fields, 0, 0, "Đợt chưa có HĐ", _episode); AddField(fields, 0, 2, "Tổng tiền hóa đơn", _total);
            AddField(fields, 1, 0, "Hóa đơn thanh toán", _invoice); AddField(fields, 1, 2, "Số tiền trả", _payment);
            AddField(fields, 2, 0, "Phương thức", _method);
            _method.Items.AddRange(new object[] { "TienMat", "ChuyenKhoan", "The", "Khac" }); _method.SelectedItem = "TienMat";
            var actions = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 42 };
            actions.Controls.Add(Ui.Button("Tạo hóa đơn", (s, e) => CreateInvoice()));
            actions.Controls.Add(Ui.Button("Ghi nhận thanh toán", (s, e) => PayInvoice()));
            actions.Controls.Add(Ui.Button("Nạp lại", (s, e) => ReloadAll())); box.Controls.Add(actions);
            root.Controls.Add(_grid, 0, 1); ReloadAll();
        }

        private static void AddField(TableLayoutPanel table, int row, int col, string caption, Control control)
        { table.Controls.Add(Ui.Label(caption), col, row); control.Dock = DockStyle.Top; table.Controls.Add(control, col + 1, row); }

        private void ReloadAll()
        {
            try
            {
                Ui.BindCombo(_episode, Db.Query(@"SELECT dt.MaDT, CONCAT(N'ĐT',dt.MaDT,N' - ',bn.HoTen,N' - ',CONVERT(NVARCHAR(10),dt.NgayBatDau,103)) AS DisplayText
                    FROM dbo.DotDieuTri dt JOIN dbo.HoSoBenhAn hs ON hs.MaHS=dt.MaHS JOIN dbo.BenhNhan bn ON bn.MaBN=hs.MaBN
                    LEFT JOIN dbo.HoaDonVienPhi hd ON hd.MaDT=dt.MaDT WHERE hd.MaHD IS NULL AND dt.TrangThai<>N'DaDong' ORDER BY dt.MaDT DESC"), "MaDT", "DisplayText");
                Ui.BindCombo(_invoice, Db.Query(@"SELECT hd.MaHD, CONCAT(N'HĐ',hd.MaHD,N' - ',bn.HoTen,N' - còn trạng thái ',hd.TrangThai,N' - ',CONVERT(NVARCHAR(30),hd.TongTien)) AS DisplayText
                    FROM dbo.HoaDonVienPhi hd JOIN dbo.DotDieuTri dt ON dt.MaDT=hd.MaDT JOIN dbo.HoSoBenhAn hs ON hs.MaHS=dt.MaHS
                    JOIN dbo.BenhNhan bn ON bn.MaBN=hs.MaBN WHERE hd.TrangThai<>N'Huy' ORDER BY hd.MaHD DESC"), "MaHD", "DisplayText");
                _grid.DataSource = Db.Query(@"SELECT hd.MaHD, hd.MaDT, bn.HoTen AS BenhNhan, hd.NgayLap, hd.TongTien, hd.TrangThai,
                      COALESCE(SUM(CASE WHEN tt.TrangThai=N'ThanhCong' THEN tt.SoTien ELSE 0 END),0) AS DaThanhToan,
                      hd.TongTien-COALESCE(SUM(CASE WHEN tt.TrangThai=N'ThanhCong' THEN tt.SoTien ELSE 0 END),0) AS ConNo
                    FROM dbo.HoaDonVienPhi hd JOIN dbo.DotDieuTri dt ON dt.MaDT=hd.MaDT JOIN dbo.HoSoBenhAn hs ON hs.MaHS=dt.MaHS
                    JOIN dbo.BenhNhan bn ON bn.MaBN=hs.MaBN LEFT JOIN dbo.ThanhToanVienPhi tt ON tt.MaHD=hd.MaHD
                    GROUP BY hd.MaHD,hd.MaDT,bn.HoTen,hd.NgayLap,hd.TongTien,hd.TrangThai ORDER BY hd.MaHD DESC");
            }
            catch (Exception ex) { Ui.ShowError(ex); }
        }

        private void CreateInvoice()
        {
            try
            {
                int maDT = Ui.SelectedInt(_episode, "đợt điều trị");
                object result = Db.Scalar("EXEC dbo.usp_TaoHoaDonVienPhi @MaDT, @TongTien",
                    Db.P("@MaDT", SqlDbType.Int, maDT), Db.DecimalP("@TongTien", _total.Value, 12, 2));
                Ui.ShowInfo("Đã tạo hóa đơn viện phí. Mã hóa đơn: " + Convert.ToInt32(result) + "."); ReloadAll();
            }
            catch (Exception ex) { Ui.ShowError(ex, "Tạo hóa đơn thất bại"); }
        }

        private void PayInvoice()
        {
            try
            {
                int maHD = Ui.SelectedInt(_invoice, "hóa đơn");
                if (_method.SelectedItem == null) throw new InvalidOperationException("Chọn phương thức thanh toán.");
                Db.Scalar("EXEC dbo.usp_ThanhToanVienPhi @MaHD, @SoTien, @PhuongThuc",
                    Db.P("@MaHD", SqlDbType.Int, maHD), Db.DecimalP("@SoTien", _payment.Value, 12, 2),
                    Db.P("@PhuongThuc", SqlDbType.NVarChar, Convert.ToString(_method.SelectedItem), 30));
                Ui.ShowInfo("Đã ghi nhận thanh toán. Hệ thống đã cập nhật trạng thái hóa đơn theo số tiền đã thu."); ReloadAll();
            }
            catch (Exception ex) { Ui.ShowError(ex, "Thanh toán thất bại"); }
        }
    }
}
