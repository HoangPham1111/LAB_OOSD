using System.ComponentModel;
using System.Windows.Forms;
namespace QuanLyCongTyDuLich.Forms
{
    partial class FrmKetThucKhaoSat
    {
        private IContainer components = null;
        private TabControl tabKT;
        private DataGridView dgvDoan, dgvKS;
        private TextBox txtSoTT, txtSoDK, txtGhiChu, txtMaKS, txtKSChon, txtGopY;
        private DateTimePicker dtTT, dtGui, dtPH;
        private NumericUpDown numTien, numDiem;
        private ComboBox cboLoaiKS, cboDangKy;
        private Button btnThanhToan, btnGui, btnGhiPH, btnDong;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            DesignerUi.Form(this, "Kết thúc tour - khảo sát khách hàng", 1180, 760);
            tabKT = DesignerUi.Tabs("tabKT", 10, 10, 1160, 680);
            var p1 = DesignerUi.Page("Thanh toán sau tour (đoàn)");
            dgvDoan = DesignerUi.Grid("dgvDoan", 10, 10, 1125, 350); DesignerUi.GridStyle(dgvDoan); p1.Controls.Add(dgvDoan);
            txtSoTT = DesignerUi.Text("txtSoTT", 100, 385, 130); txtSoDK = DesignerUi.Text("txtSoDK", 320, 385, 140); dtTT = DesignerUi.Date("dtTT", 570, 385, 140); numTien = DesignerUi.Number("numTien", 820, 385, 170, 0, 999999999999M); txtGhiChu = DesignerUi.Text("txtGhiChu", 100, 430, 650);
            p1.Controls.AddRange(new Control[] { DesignerUi.Label("Số thanh toán", 10, 385, 90), txtSoTT, DesignerUi.Label("Phiếu đoàn", 245, 385, 75), txtSoDK, DesignerUi.Label("Ngày thanh toán", 465, 385, 100), dtTT, DesignerUi.Label("Số tiền", 735, 385, 75), numTien, DesignerUi.Label("Ghi chú", 10, 430, 90), txtGhiChu });
            btnThanhToan = DesignerUi.Button("btnThanhToan", "Ghi nhận thanh toán", 10, 475, 180); p1.Controls.Add(btnThanhToan);

            var p2 = DesignerUi.Page("Khảo sát khách hàng");
            cboLoaiKS = DesignerUi.Combo("cboLoaiKS", 110, 15, 100); cboDangKy = DesignerUi.Combo("cboDangKy", 330, 15, 350); txtMaKS = DesignerUi.Text("txtMaKS", 790, 15, 130); dtGui = DesignerUi.Date("dtGui", 1000, 15, 125);
            p2.Controls.AddRange(new Control[] { DesignerUi.Label("Loại khách", 10, 15, 90), cboLoaiKS, DesignerUi.Label("Đăng ký đã kết thúc", 220, 15, 105), cboDangKy, DesignerUi.Label("Mã KS", 710, 15, 70), txtMaKS, DesignerUi.Label("Ngày gửi", 925, 15, 70), dtGui });
            btnGui = DesignerUi.Button("btnGui", "Gửi phiếu khảo sát", 10, 55, 165); p2.Controls.Add(btnGui);
            dgvKS = DesignerUi.Grid("dgvKS", 10, 100, 1125, 300); DesignerUi.GridStyle(dgvKS); p2.Controls.Add(dgvKS);
            txtKSChon = DesignerUi.Text("txtKSChon", 100, 420, 130); dtPH = DesignerUi.Date("dtPH", 330, 420, 140); numDiem = DesignerUi.Number("numDiem", 570, 420, 70, 1, 5); txtGopY = DesignerUi.Text("txtGopY", 100, 465, 800, 35, true);
            p2.Controls.AddRange(new Control[] { DesignerUi.Label("Phiếu chọn", 10, 420, 85), txtKSChon, DesignerUi.Label("Ngày phản hồi", 235, 420, 90), dtPH, DesignerUi.Label("Điểm (1-5)", 480, 420, 85), numDiem, DesignerUi.Label("Góp ý", 10, 465, 85), txtGopY });
            btnGhiPH = DesignerUi.Button("btnGhiPH", "Ghi nhận góp ý", 10, 515, 150); p2.Controls.Add(btnGhiPH);
            tabKT.TabPages.AddRange(new TabPage[] { p1, p2 }); btnDong = DesignerUi.Button("btnDong", "Đóng", 1030, 700, 110);
            Controls.AddRange(new Control[] { tabKT, btnDong });
            Load += FrmKetThucKhaoSat_Load; dgvDoan.SelectionChanged += dgvDoan_SelectionChanged; btnThanhToan.Click += btnThanhToan_Click;
            cboLoaiKS.SelectedIndexChanged += cboLoaiKS_SelectedIndexChanged; btnGui.Click += btnGui_Click; dgvKS.SelectionChanged += dgvKS_SelectionChanged; btnGhiPH.Click += btnGhiPH_Click; btnDong.Click += btnDong_Click;
        }
    }
}
