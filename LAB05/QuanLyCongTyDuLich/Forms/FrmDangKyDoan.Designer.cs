using System.ComponentModel;
using System.Windows.Forms;
namespace QuanLyCongTyDuLich.Forms
{
    partial class FrmDangKyDoan
    {
        private IContainer components = null;
        private GroupBox grpDoan, grpDangKy;
        private TextBox txtMaDoan, txtTenCQ, txtDiaChi, txtDT, txtDaiDien, txtSo, txtDon;
        private ComboBox cboTour;
        private DateTimePicker dtDi;
        private NumericUpDown numNguoi, numCoc;
        private CheckBox chkBH;
        private Label lblKetThuc, lblTong;
        private DataGridView dgvThanhVien, dgv;
        private Button btnDangKy, btnHuy, btnDong;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            DesignerUi.Form(this, "Đăng ký theo đoàn", 1200, 820);
            grpDoan = new GroupBox { Text = "Thông tin đoàn khách", Location = new System.Drawing.Point(12, 10), Size = new System.Drawing.Size(560, 155) };
            txtMaDoan = DesignerUi.Text("txtMaDoan", 105, 25, 145); txtTenCQ = DesignerUi.Text("txtTenCQ", 105, 60, 420); txtDiaChi = DesignerUi.Text("txtDiaChi", 105, 95, 420); txtDT = DesignerUi.Text("txtDT", 105, 125, 150); txtDaiDien = DesignerUi.Text("txtDaiDien", 350, 125, 175);
            grpDoan.Controls.AddRange(new Control[] { DesignerUi.Label("Mã đoàn", 12, 25, 90), txtMaDoan, DesignerUi.Label("Cơ quan/gia đình", 12, 60, 90), txtTenCQ, DesignerUi.Label("Địa chỉ", 12, 95, 90), txtDiaChi, DesignerUi.Label("Điện thoại", 12, 125, 90), txtDT, DesignerUi.Label("Người đại diện", 260, 125, 90), txtDaiDien });
            grpDangKy = new GroupBox { Text = "Đăng ký tour", Location = new System.Drawing.Point(585, 10), Size = new System.Drawing.Size(600, 155) };
            txtSo = DesignerUi.Text("txtSo", 80, 25, 145); cboTour = DesignerUi.Combo("cboTour", 320, 25, 260); dtDi = DesignerUi.Date("dtDi", 80, 60, 145); numNguoi = DesignerUi.Number("numNguoi", 320, 60, 90, 13, 1000); txtDon = DesignerUi.Text("txtDon", 80, 95, 250); numCoc = DesignerUi.Number("numCoc", 420, 95, 150, 0, 999999999999M); chkBH = DesignerUi.Check("chkBH", "Mua bảo hiểm", 10, 125, 130); lblKetThuc = DesignerUi.Label("-", 225, 125, 100); lblTong = DesignerUi.Label("0 đ", 430, 125, 150);
            grpDangKy.Controls.AddRange(new Control[] { DesignerUi.Label("Số phiếu", 10, 25, 70), txtSo, DesignerUi.Label("Tour", 270, 25, 50), cboTour, DesignerUi.Label("Ngày đi", 10, 60, 70), dtDi, DesignerUi.Label("Số người", 240, 60, 75), numNguoi, DesignerUi.Label("Địa điểm đón", 10, 95, 70), txtDon, DesignerUi.Label("Tiền cọc", 345, 95, 70), numCoc, chkBH, DesignerUi.Label("Kết thúc DK", 130, 125, 90), lblKetThuc, DesignerUi.Label("Tổng dự kiến", 335, 125, 90), lblTong });
            dgvThanhVien = DesignerUi.Grid("dgvThanhVien", 12, 175, 1173, 190, true); DesignerUi.GridStyle(dgvThanhVien); dgvThanhVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            btnDangKy = DesignerUi.Button("btnDangKy", "Lập phiếu đăng ký", 12, 375, 175); btnHuy = DesignerUi.Button("btnHuy", "Hủy phiếu (mất cọc)", 200, 375, 190); btnDong = DesignerUi.Button("btnDong", "Đóng", 1065, 375, 120);
            dgv = DesignerUi.Grid("dgv", 12, 420, 1173, 350); DesignerUi.GridStyle(dgv);
            Controls.AddRange(new Control[] { grpDoan, grpDangKy, dgvThanhVien, btnDangKy, btnHuy, btnDong, dgv });
            Load += FrmDangKyDoan_Load; cboTour.SelectedIndexChanged += TinhTong; dtDi.ValueChanged += TinhTong; numNguoi.ValueChanged += TinhTong; chkBH.CheckedChanged += chkBH_CheckedChanged;
            btnDangKy.Click += btnDangKy_Click; btnHuy.Click += btnHuy_Click; btnDong.Click += btnDong_Click;
        }
    }
}
