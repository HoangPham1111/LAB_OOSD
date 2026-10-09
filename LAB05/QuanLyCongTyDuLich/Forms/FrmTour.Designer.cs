using System.ComponentModel;
using System.Windows.Forms;

namespace QuanLyCongTyDuLich.Forms
{
    partial class FrmTour
    {
        private IContainer components = null;
        private ComboBox cboTour, cboPT, cboDTQ;
        private TabControl tabTour;
        private DataGridView dgvTour, dgvDiemDung, dgvChang, dgvTQ;
        private TextBox txtMa, txtTen, txtMoTa, txtDiemDung, txtGhiChuDD, txtGhiChuPT;
        private NumericUpDown numNgay, numDem, numGia, numThuTu, numSao, numChang, numThuTuTQ;
        private CheckBox chkDoiPT, chkAn, chkKS;
        private Button btnThemTour, btnThemDD, btnThemChang, btnThemTQ, btnDong;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            DesignerUi.Form(this, "Quản lý tour - hành trình", 1180, 770);
            Controls.Add(DesignerUi.Label("Tour đang chọn:", 15, 15, 105));
            cboTour = DesignerUi.Combo("cboTour", 120, 15, 450); Controls.Add(cboTour);
            tabTour = DesignerUi.Tabs("tabTour", 10, 50, 1160, 650);

            var p1 = DesignerUi.Page("Tour");
            dgvTour = DesignerUi.Grid("dgvTour", 10, 10, 1125, 300); DesignerUi.GridStyle(dgvTour); p1.Controls.Add(dgvTour);
            txtMa = DesignerUi.Text("txtMa", 75, 330, 130); txtTen = DesignerUi.Text("txtTen", 285, 330, 280); numNgay = DesignerUi.Number("numNgay", 680, 330, 80, 1, 100, 0); numDem = DesignerUi.Number("numDem", 870, 330, 80, 0, 100, 0);
            numGia = DesignerUi.Number("numGia", 75, 375, 160, 0, 999999999999M, 0); txtMoTa = DesignerUi.Text("txtMoTa", 310, 375, 500, 32, true);
            p1.Controls.AddRange(new Control[] { DesignerUi.Label("Mã tour", 10, 330, 65), txtMa, DesignerUi.Label("Tên tour", 220, 330, 65), txtTen, DesignerUi.Label("Số ngày", 610, 330, 65), numNgay, DesignerUi.Label("Số đêm", 800, 330, 65), numDem, DesignerUi.Label("Đơn giá/khách", 10, 375, 65), numGia, DesignerUi.Label("Mô tả", 250, 375, 60), txtMoTa });
            btnThemTour = DesignerUi.Button("btnThemTour", "Thêm tour", 845, 375, 140); p1.Controls.Add(btnThemTour);

            var p2 = DesignerUi.Page("Điểm dừng");
            dgvDiemDung = DesignerUi.Grid("dgvDiemDung", 10, 10, 1125, 300); DesignerUi.GridStyle(dgvDiemDung); p2.Controls.Add(dgvDiemDung);
            numThuTu = DesignerUi.Number("numThuTu", 90, 330, 70, 1, 500); txtDiemDung = DesignerUi.Text("txtDiemDung", 260, 330, 230); chkDoiPT = DesignerUi.Check("chkDoiPT", "Đổi phương tiện", 510, 330, 140); chkAn = DesignerUi.Check("chkAn", "Có nơi ăn", 660, 330, 100); chkKS = DesignerUi.Check("chkKS", "Có khách sạn", 770, 330, 115); numSao = DesignerUi.Number("numSao", 990, 330, 70, 2, 5); numSao.Enabled = false; txtGhiChuDD = DesignerUi.Text("txtGhiChuDD", 90, 375, 450);
            p2.Controls.AddRange(new Control[] { DesignerUi.Label("Thứ tự", 10, 330, 75), numThuTu, DesignerUi.Label("Tên điểm dừng", 165, 330, 95), txtDiemDung, chkDoiPT, chkAn, chkKS, DesignerUi.Label("Hạng sao", 895, 330, 90), numSao, DesignerUi.Label("Ghi chú", 10, 375, 75), txtGhiChuDD });
            btnThemDD = DesignerUi.Button("btnThemDD", "Thêm điểm dừng", 575, 372, 160); p2.Controls.Add(btnThemDD);

            var p3 = DesignerUi.Page("Phương tiện theo chặng");
            dgvChang = DesignerUi.Grid("dgvChang", 10, 10, 1125, 300); DesignerUi.GridStyle(dgvChang); p3.Controls.Add(dgvChang);
            numChang = DesignerUi.Number("numChang", 105, 330, 90, 1, 500); cboPT = DesignerUi.Combo("cboPT", 300, 330, 260); txtGhiChuPT = DesignerUi.Text("txtGhiChuPT", 660, 330, 300);
            p3.Controls.AddRange(new Control[] { DesignerUi.Label("Chặng thứ", 10, 330, 90), numChang, DesignerUi.Label("Phương tiện", 210, 330, 85), cboPT, DesignerUi.Label("Ghi chú", 575, 330, 80), txtGhiChuPT });
            btnThemChang = DesignerUi.Button("btnThemChang", "Gắn phương tiện", 10, 375, 160); p3.Controls.Add(btnThemChang);

            var p4 = DesignerUi.Page("Điểm tham quan");
            dgvTQ = DesignerUi.Grid("dgvTQ", 10, 10, 1125, 300); DesignerUi.GridStyle(dgvTQ); p4.Controls.Add(dgvTQ);
            cboDTQ = DesignerUi.Combo("cboDTQ", 125, 330, 390); numThuTuTQ = DesignerUi.Number("numThuTuTQ", 600, 330, 90, 1, 500);
            p4.Controls.AddRange(new Control[] { DesignerUi.Label("Điểm tham quan", 10, 330, 110), cboDTQ, DesignerUi.Label("Thứ tự", 530, 330, 65), numThuTuTQ });
            btnThemTQ = DesignerUi.Button("btnThemTQ", "Gắn điểm tham quan", 10, 375, 175); p4.Controls.Add(btnThemTQ);

            tabTour.TabPages.AddRange(new TabPage[] { p1, p2, p3, p4 });
            btnDong = DesignerUi.Button("btnDong", "Đóng", 1025, 710, 120);
            Controls.Add(tabTour); Controls.Add(btnDong);
            Load += FrmTour_Load; cboTour.SelectedIndexChanged += cboTour_SelectedIndexChanged; chkKS.CheckedChanged += chkKS_CheckedChanged;
            btnThemTour.Click += btnThemTour_Click; btnThemDD.Click += btnThemDD_Click; btnThemChang.Click += btnThemChang_Click; btnThemTQ.Click += btnThemTQ_Click; btnDong.Click += btnDong_Click;
        }
    }
}
