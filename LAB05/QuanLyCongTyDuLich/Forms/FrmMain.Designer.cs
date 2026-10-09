using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCongTyDuLich.Forms
{
    partial class FrmMain
    {
        private IContainer components = null;
        private Button btnDanhMuc, btnTour, btnChuyenLe, btnDangKyLe, btnDangKyDoan, btnPhanCong, btnKetThuc, btnThongKe, btnThoat;
        private Label lblKetNoi;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            DesignerUi.Form(this, "QUẢN LÝ CÔNG TY DU LỊCH VĂN HÓA VIỆT", 820, 610);
            var title = DesignerUi.Label("QUẢN LÝ CÔNG TY DU LỊCH VĂN HÓA VIỆT", 25, 20, 770, 42);
            title.Font = new Font("Segoe UI", 17F, FontStyle.Bold); title.TextAlign = ContentAlignment.MiddleCenter;
            var sub = DesignerUi.Label("Chọn nghiệp vụ cần thực hiện", 25, 68, 770, 26); sub.TextAlign = ContentAlignment.MiddleCenter;
            btnDanhMuc = DesignerUi.Button("btnDanhMuc", "Danh mục", 90, 125, 285);
            btnTour = DesignerUi.Button("btnTour", "Tour - hành trình", 445, 125, 285);
            btnChuyenLe = DesignerUi.Button("btnChuyenLe", "Lịch chuyến khách lẻ", 90, 185, 285);
            btnDangKyLe = DesignerUi.Button("btnDangKyLe", "Đăng ký khách lẻ", 445, 185, 285);
            btnDangKyDoan = DesignerUi.Button("btnDangKyDoan", "Đăng ký theo đoàn", 90, 245, 285);
            btnPhanCong = DesignerUi.Button("btnPhanCong", "Phân công hướng dẫn viên", 445, 245, 285);
            btnKetThuc = DesignerUi.Button("btnKetThuc", "Kết thúc tour - khảo sát", 90, 305, 285);
            btnThongKe = DesignerUi.Button("btnThongKe", "Lương - thống kê", 445, 305, 285);
            btnThoat = DesignerUi.Button("btnThoat", "Thoát chương trình", 292, 385, 235);
            lblKetNoi = DesignerUi.Label("Đang kiểm tra kết nối SQL Server...", 25, 470, 770, 32);
            lblKetNoi.TextAlign = ContentAlignment.MiddleCenter;
            Controls.AddRange(new Control[] { title, sub, btnDanhMuc, btnTour, btnChuyenLe, btnDangKyLe, btnDangKyDoan, btnPhanCong, btnKetThuc, btnThongKe, btnThoat, lblKetNoi });
            Load += FrmMain_Load;
            btnDanhMuc.Click += btnDanhMuc_Click; btnTour.Click += btnTour_Click; btnChuyenLe.Click += btnChuyenLe_Click;
            btnDangKyLe.Click += btnDangKyLe_Click; btnDangKyDoan.Click += btnDangKyDoan_Click; btnPhanCong.Click += btnPhanCong_Click;
            btnKetThuc.Click += btnKetThuc_Click; btnThongKe.Click += btnThongKe_Click; btnThoat.Click += btnThoat_Click;
        }
    }
}
