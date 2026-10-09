using System.ComponentModel;
using System.Windows.Forms;
namespace QuanLyCongTyDuLich.Forms
{
    partial class FrmDangKyLe
    {
        private IContainer components = null;
        private TextBox txtSo, txtTen, txtDT;
        private ComboBox cboChuyen, cboDiemBan;
        private NumericUpDown numNguoi;
        private Label lblThanhTien;
        private Button btnDangKy, btnDong;
        private DataGridView dgv;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            DesignerUi.Form(this, "Đăng ký khách lẻ và thanh toán vé", 1120, 680);
            txtSo = DesignerUi.Text("txtSo", 95, 18, 145); cboChuyen = DesignerUi.Combo("cboChuyen", 355, 18, 300); cboDiemBan = DesignerUi.Combo("cboDiemBan", 790, 18, 260);
            txtTen = DesignerUi.Text("txtTen", 95, 62, 240); txtDT = DesignerUi.Text("txtDT", 430, 62, 180); numNguoi = DesignerUi.Number("numNguoi", 720, 62, 90, 1, 11); lblThanhTien = DesignerUi.Label("0 đ", 930, 62, 150);
            Controls.AddRange(new Control[] { DesignerUi.Label("Số đăng ký", 15, 18, 80), txtSo, DesignerUi.Label("Chuyến", 275, 18, 70), cboChuyen, DesignerUi.Label("Điểm bán vé", 690, 18, 90), cboDiemBan, DesignerUi.Label("Người đăng ký", 15, 62, 80), txtTen, DesignerUi.Label("Điện thoại", 350, 62, 75), txtDT, DesignerUi.Label("Số người", 635, 62, 80), numNguoi, DesignerUi.Label("Thành tiền", 825, 62, 95), lblThanhTien });
            btnDangKy = DesignerUi.Button("btnDangKy", "Đăng ký và thanh toán vé", 15, 105, 210); btnDong = DesignerUi.Button("btnDong", "Đóng", 970, 105, 100); Controls.AddRange(new Control[] { btnDangKy, btnDong });
            dgv = DesignerUi.Grid("dgv", 15, 155, 1075, 480); DesignerUi.GridStyle(dgv); Controls.Add(dgv);
            Load += FrmDangKyLe_Load; cboChuyen.SelectedIndexChanged += TinhTien; numNguoi.ValueChanged += TinhTien; btnDangKy.Click += btnDangKy_Click; btnDong.Click += btnDong_Click;
        }
    }
}
