using System.ComponentModel;
using System.Windows.Forms;
namespace QuanLyCongTyDuLich.Forms
{
    partial class FrmLuongThongKe
    {
        private IContainer components = null;
        private TabControl tabTK;
        private NumericUpDown numThang, numNam;
        private DateTimePicker dtTu, dtDen;
        private Button btnLuong, btnTongHop, btnDong;
        private DataGridView dgvLuong, dgvTongHop;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            DesignerUi.Form(this, "Lương hướng dẫn viên - thống kê", 1120, 700);
            tabTK = DesignerUi.Tabs("tabTK", 10, 10, 1100, 620);
            var p1 = DesignerUi.Page("Lương hướng dẫn viên");
            numThang = DesignerUi.Number("numThang", 90, 20, 80, 1, 12); numNam = DesignerUi.Number("numNam", 250, 20, 100, 2000, 2100);
            btnLuong = DesignerUi.Button("btnLuong", "Tính lương", 390, 16, 130); p1.Controls.AddRange(new Control[] { DesignerUi.Label("Tháng", 15, 20, 70), numThang, DesignerUi.Label("Năm", 190, 20, 50), numNam, btnLuong });
            dgvLuong = DesignerUi.Grid("dgvLuong", 15, 65, 1060, 500); DesignerUi.GridStyle(dgvLuong); p1.Controls.Add(dgvLuong);
            var p2 = DesignerUi.Page("Thống kê tổng hợp");
            dtTu = DesignerUi.Date("dtTu", 90, 20, 145); dtDen = DesignerUi.Date("dtDen", 330, 20, 145); btnTongHop = DesignerUi.Button("btnTongHop", "Thống kê", 510, 16, 130);
            p2.Controls.AddRange(new Control[] { DesignerUi.Label("Từ ngày", 15, 20, 70), dtTu, DesignerUi.Label("Đến ngày", 250, 20, 75), dtDen, btnTongHop });
            dgvTongHop = DesignerUi.Grid("dgvTongHop", 15, 65, 1060, 500); DesignerUi.GridStyle(dgvTongHop); p2.Controls.Add(dgvTongHop);
            tabTK.TabPages.AddRange(new TabPage[] { p1, p2 }); btnDong = DesignerUi.Button("btnDong", "Đóng", 970, 645, 110);
            Controls.AddRange(new Control[] { tabTK, btnDong });
            Load += FrmLuongThongKe_Load; btnLuong.Click += btnLuong_Click; btnTongHop.Click += btnTongHop_Click; btnDong.Click += btnDong_Click;
        }
    }
}
