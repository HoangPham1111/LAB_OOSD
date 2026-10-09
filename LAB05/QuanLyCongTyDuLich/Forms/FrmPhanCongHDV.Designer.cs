using System.ComponentModel;
using System.Windows.Forms;
namespace QuanLyCongTyDuLich.Forms
{
    partial class FrmPhanCongHDV
    {
        private IContainer components = null;
        private TextBox txtMaPC;
        private ComboBox cboHDV, cboLoai, cboDoiTuong;
        private NumericUpDown numThuLao;
        private Button btnPhanCong, btnDong;
        private DataGridView dgv;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            DesignerUi.Form(this, "Phân công hướng dẫn viên", 1120, 680);
            txtMaPC = DesignerUi.Text("txtMaPC", 100, 20, 130); cboHDV = DesignerUi.Combo("cboHDV", 330, 20, 250); cboLoai = DesignerUi.Combo("cboLoai", 675, 20, 100); cboDoiTuong = DesignerUi.Combo("cboDoiTuong", 870, 20, 210); numThuLao = DesignerUi.Number("numThuLao", 115, 65, 155, 0, 999999999999M);
            Controls.AddRange(new Control[] { DesignerUi.Label("Mã phân công", 15, 20, 85), txtMaPC, DesignerUi.Label("Hướng dẫn viên", 235, 20, 90), cboHDV, DesignerUi.Label("Loại", 625, 20, 45), cboLoai, DesignerUi.Label("Chuyến / đoàn", 780, 20, 90), cboDoiTuong, DesignerUi.Label("Thù lao tour", 15, 65, 95), numThuLao });
            btnPhanCong = DesignerUi.Button("btnPhanCong", "Phân công", 300, 62, 130); btnDong = DesignerUi.Button("btnDong", "Đóng", 960, 62, 100); Controls.AddRange(new Control[] { btnPhanCong, btnDong });
            dgv = DesignerUi.Grid("dgv", 15, 115, 1070, 510); DesignerUi.GridStyle(dgv); Controls.Add(dgv);
            Load += FrmPhanCongHDV_Load; cboLoai.SelectedIndexChanged += cboLoai_SelectedIndexChanged; btnPhanCong.Click += btnPhanCong_Click; btnDong.Click += btnDong_Click;
        }
    }
}
