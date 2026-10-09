using System.ComponentModel;
using System.Windows.Forms;
namespace QuanLyCongTyDuLich.Forms
{
    partial class FrmChuyenLe
    {
        private IContainer components = null;
        private TextBox txtMa, txtDon;
        private ComboBox cboTour;
        private DateTimePicker dtDi;
        private Label lblNgayVe;
        private Button btnThem, btnDongDK, btnDong;
        private DataGridView dgv;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            DesignerUi.Form(this, "Lịch chuyến khách lẻ", 1080, 680);
            txtMa = DesignerUi.Text("txtMa", 90, 20, 130); cboTour = DesignerUi.Combo("cboTour", 315, 20, 280); dtDi = DesignerUi.Date("dtDi", 700, 20, 140);
            lblNgayVe = DesignerUi.Label("-", 920, 20, 130); txtDon = DesignerUi.Text("txtDon", 130, 65, 430);
            Controls.AddRange(new Control[] { DesignerUi.Label("Mã chuyến", 15, 20), txtMa, DesignerUi.Label("Tour", 245, 20, 65), cboTour, DesignerUi.Label("Ngày đi", 625, 20, 70), dtDi, DesignerUi.Label("Ngày về dự kiến", 820, 20, 100), lblNgayVe, DesignerUi.Label("Địa điểm đón", 15, 65, 110), txtDon });
            btnThem = DesignerUi.Button("btnThem", "Tạo chuyến", 585, 62, 130); btnDongDK = DesignerUi.Button("btnDongDK", "Đóng đăng ký", 730, 62, 145); btnDong = DesignerUi.Button("btnDong", "Đóng", 890, 62, 100);
            Controls.AddRange(new Control[] { btnThem, btnDongDK, btnDong });
            dgv = DesignerUi.Grid("dgv", 15, 115, 1035, 520); DesignerUi.GridStyle(dgv); Controls.Add(dgv);
            Load += FrmChuyenLe_Load; cboTour.SelectedIndexChanged += TinhNgayVe; dtDi.ValueChanged += TinhNgayVe; btnThem.Click += btnThem_Click; btnDongDK.Click += btnDongDK_Click; btnDong.Click += btnDong_Click;
        }
    }
}
