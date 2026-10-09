using System.ComponentModel;
using System.Windows.Forms;

namespace QuanLyCongTyDuLich.Forms
{
    partial class FrmDanhMuc
    {
        private IContainer components = null;
        private TabControl tabDanhMuc;
        private DataGridView dgvPT, dgvDB, dgvHDV, dgvDTQ;
        private TextBox txtPTMa, txtPTTen, txtPTGhiChu, txtDBMa, txtDBTen, txtDBDiaChi, txtDBDT, txtHDVMa, txtHDVTen, txtHDVDT, txtDTQMa, txtDTQTen, txtDTQDiaDiem, txtDTQNoiDung, txtDTQYNghia;
        private NumericUpDown numLuong;
        private Button btnThemPT, btnThemDB, btnThemHDV, btnThemDTQ, btnDong;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            DesignerUi.Form(this, "Danh mục hệ thống", 1160, 730);
            tabDanhMuc = DesignerUi.Tabs("tabDanhMuc", 10, 10, 1140, 660);
            var p1 = DesignerUi.Page("Phương tiện");
            txtPTMa = DesignerUi.Text("txtPTMa", 95, 15, 150); txtPTTen = DesignerUi.Text("txtPTTen", 355, 15, 220); txtPTGhiChu = DesignerUi.Text("txtPTGhiChu", 685, 15, 250);
            p1.Controls.AddRange(new Control[] { DesignerUi.Label("Mã PT", 15, 15), txtPTMa, DesignerUi.Label("Tên PT", 270, 15), txtPTTen, DesignerUi.Label("Ghi chú", 600, 15), txtPTGhiChu });
            btnThemPT = DesignerUi.Button("btnThemPT", "Thêm phương tiện", 950, 12, 160); p1.Controls.Add(btnThemPT);
            dgvPT = DesignerUi.Grid("dgvPT", 15, 60, 1095, 545); DesignerUi.GridStyle(dgvPT); p1.Controls.Add(dgvPT);

            var p2 = DesignerUi.Page("Điểm bán vé");
            txtDBMa = DesignerUi.Text("txtDBMa", 100, 15, 145); txtDBTen = DesignerUi.Text("txtDBTen", 350, 15, 220); txtDBDiaChi = DesignerUi.Text("txtDBDiaChi", 665, 15, 245); txtDBDT = DesignerUi.Text("txtDBDT", 100, 55, 160);
            p2.Controls.AddRange(new Control[] { DesignerUi.Label("Mã điểm bán", 15, 15, 85), txtDBMa, DesignerUi.Label("Tên điểm bán", 260, 15, 90), txtDBTen, DesignerUi.Label("Địa chỉ", 590, 15, 70), txtDBDiaChi, DesignerUi.Label("Điện thoại", 15, 55, 85), txtDBDT });
            btnThemDB = DesignerUi.Button("btnThemDB", "Thêm điểm bán", 300, 52, 150); p2.Controls.Add(btnThemDB);
            dgvDB = DesignerUi.Grid("dgvDB", 15, 100, 1095, 505); DesignerUi.GridStyle(dgvDB); p2.Controls.Add(dgvDB);

            var p3 = DesignerUi.Page("Hướng dẫn viên");
            txtHDVMa = DesignerUi.Text("txtHDVMa", 95, 15, 130); txtHDVTen = DesignerUi.Text("txtHDVTen", 310, 15, 205); txtHDVDT = DesignerUi.Text("txtHDVDT", 600, 15, 150); numLuong = DesignerUi.Number("numLuong", 850, 15, 150, 0, 999999999999M, 0);
            p3.Controls.AddRange(new Control[] { DesignerUi.Label("Mã HDV", 15, 15), txtHDVMa, DesignerUi.Label("Họ tên", 240, 15, 65), txtHDVTen, DesignerUi.Label("Điện thoại", 525, 15, 75), txtHDVDT, DesignerUi.Label("Lương cơ bản", 755, 15, 95), numLuong });
            btnThemHDV = DesignerUi.Button("btnThemHDV", "Thêm HDV", 15, 55, 130); p3.Controls.Add(btnThemHDV);
            dgvHDV = DesignerUi.Grid("dgvHDV", 15, 100, 1095, 505); DesignerUi.GridStyle(dgvHDV); p3.Controls.Add(dgvHDV);

            var p4 = DesignerUi.Page("Điểm tham quan");
            txtDTQMa = DesignerUi.Text("txtDTQMa", 105, 12, 130); txtDTQTen = DesignerUi.Text("txtDTQTen", 330, 12, 235); txtDTQDiaDiem = DesignerUi.Text("txtDTQDiaDiem", 670, 12, 260);
            txtDTQNoiDung = DesignerUi.Text("txtDTQNoiDung", 105, 50, 420); txtDTQYNghia = DesignerUi.Text("txtDTQYNghia", 620, 50, 420);
            p4.Controls.AddRange(new Control[] { DesignerUi.Label("Mã điểm TQ", 15, 12, 90), txtDTQMa, DesignerUi.Label("Tên điểm TQ", 245, 12, 85), txtDTQTen, DesignerUi.Label("Địa điểm", 590, 12, 75), txtDTQDiaDiem, DesignerUi.Label("Nội dung", 15, 50, 90), txtDTQNoiDung, DesignerUi.Label("Ý nghĩa", 535, 50, 80), txtDTQYNghia });
            btnThemDTQ = DesignerUi.Button("btnThemDTQ", "Thêm điểm tham quan", 15, 88, 185); p4.Controls.Add(btnThemDTQ);
            dgvDTQ = DesignerUi.Grid("dgvDTQ", 15, 130, 1095, 475); DesignerUi.GridStyle(dgvDTQ); p4.Controls.Add(dgvDTQ);

            tabDanhMuc.TabPages.AddRange(new TabPage[] { p1, p2, p3, p4 });
            btnDong = DesignerUi.Button("btnDong", "Đóng", 1010, 680, 120);
            Controls.Add(tabDanhMuc); Controls.Add(btnDong);
            Load += FrmDanhMuc_Load; btnThemPT.Click += btnThemPT_Click; btnThemDB.Click += btnThemDB_Click; btnThemHDV.Click += btnThemHDV_Click; btnThemDTQ.Click += btnThemDTQ_Click; btnDong.Click += btnDong_Click;
        }
    }
}
