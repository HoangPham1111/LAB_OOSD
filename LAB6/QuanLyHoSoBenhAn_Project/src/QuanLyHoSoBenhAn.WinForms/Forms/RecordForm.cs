using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Data;

namespace QuanLyHoSoBenhAn.Forms
{
    public sealed class RecordForm : Form
    {
        private readonly ComboBox _patient = Ui.Combo(400);
        private readonly DataGridView _grid = Ui.Grid();

        public RecordForm()
        {
            Text = "Cấp và tra cứu hồ sơ bệnh án"; Width = 980; Height = 650; MinimumSize = new Size(820, 530);
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), RowCount = 2, ColumnCount = 1 };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 85)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); Controls.Add(root);
            var top = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
            top.Controls.Add(Ui.Label("Bệnh nhân:")); top.Controls.Add(_patient);
            top.Controls.Add(Ui.Button("Cấp / lấy hồ sơ", (s, e) => IssueRecord()));
            top.Controls.Add(Ui.Button("Làm mới", (s, e) => { LoadPatients(); LoadRows(); }));
            root.Controls.Add(top, 0, 0); root.Controls.Add(_grid, 0, 1); LoadPatients(); LoadRows();
        }

        private void LoadPatients()
        {
            try { Ui.BindCombo(_patient, Db.Query("SELECT MaBN, CONCAT(N'BN', MaBN, N' - ', HoTen) AS DisplayText FROM dbo.BenhNhan ORDER BY HoTen"), "MaBN", "DisplayText"); }
            catch (Exception ex) { Ui.ShowError(ex); }
        }

        private void LoadRows()
        {
            try { _grid.DataSource = Db.Query(@"SELECT hs.MaHS, hs.MaBN, bn.HoTen, hs.NgayLap, hs.TrangThai
                     FROM dbo.HoSoBenhAn hs JOIN dbo.BenhNhan bn ON bn.MaBN=hs.MaBN ORDER BY hs.MaHS DESC"); }
            catch (Exception ex) { Ui.ShowError(ex); }
        }

        private void IssueRecord()
        {
            try
            {
                int maBN = Ui.SelectedInt(_patient, "bệnh nhân");
                using (var conn = Db.CreateConnection())
                using (var cmd = new SqlCommand("dbo.usp_CapHoSoBenhAn", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@MaBN", SqlDbType.Int).Value = maBN;
                    var p = cmd.Parameters.Add("@MaHS", SqlDbType.Int); p.Direction = ParameterDirection.Output;
                    conn.Open(); cmd.ExecuteNonQuery();
                    Ui.ShowInfo("Mã hồ sơ bệnh án: " + Convert.ToInt32(p.Value) + ".");
                }
                LoadRows();
            }
            catch (Exception ex) { Ui.ShowError(ex, "Không thể cấp hồ sơ"); }
        }
    }
}
