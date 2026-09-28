namespace QuanLyKhachSan.Forms
{
    partial class FrmDanhMuc
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabKhu;
        private System.Windows.Forms.TabPage tabNV;
        private System.Windows.Forms.TabPage tabLoai;
        private System.Windows.Forms.TabPage tabDV;
        private System.Windows.Forms.TabPage tabQD;

        private System.Windows.Forms.DataGridView dgvKhu;
        private System.Windows.Forms.DataGridView dgvNV;
        private System.Windows.Forms.DataGridView dgvLoaiTN;
        private System.Windows.Forms.DataGridView dgvDV;
        private System.Windows.Forms.DataGridView dgvQD;

        private System.Windows.Forms.TextBox txtKhuMa;
        private System.Windows.Forms.TextBox txtKhuTen;
        private System.Windows.Forms.TextBox txtNVMa;
        private System.Windows.Forms.TextBox txtNVTen;
        private System.Windows.Forms.TextBox txtNVVaiTro;
        private System.Windows.Forms.TextBox txtNVSDT;
        private System.Windows.Forms.TextBox txtLoaiMa;
        private System.Windows.Forms.TextBox txtLoaiTen;
        private System.Windows.Forms.TextBox txtDVMa;
        private System.Windows.Forms.TextBox txtDVTen;
        private System.Windows.Forms.TextBox txtDVDVT;
        private System.Windows.Forms.NumericUpDown numDVGia;
        private System.Windows.Forms.TextBox txtQDMa;
        private System.Windows.Forms.ComboBox cboQDLoai;
        private System.Windows.Forms.TextBox txtQDMucDo;
        private System.Windows.Forms.NumericUpDown numQDTien;

        private System.Windows.Forms.Button btnThemKhu;
        private System.Windows.Forms.Button btnThemNV;
        private System.Windows.Forms.Button btnThemLoaiTN;
        private System.Windows.Forms.Button btnThemDV;
        private System.Windows.Forms.Button btnThemQD;
        private System.Windows.Forms.Button btnDong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Label L(string t, int x, int y)
        {
            var l = new System.Windows.Forms.Label();
            l.Text = t;
            l.Location = new System.Drawing.Point(x, y);
            l.AutoSize = true;
            return l;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabKhu = new System.Windows.Forms.TabPage();
            this.tabNV = new System.Windows.Forms.TabPage();
            this.tabLoai = new System.Windows.Forms.TabPage();
            this.tabDV = new System.Windows.Forms.TabPage();
            this.tabQD = new System.Windows.Forms.TabPage();

            this.dgvKhu = new System.Windows.Forms.DataGridView();
            this.dgvNV = new System.Windows.Forms.DataGridView();
            this.dgvLoaiTN = new System.Windows.Forms.DataGridView();
            this.dgvDV = new System.Windows.Forms.DataGridView();
            this.dgvQD = new System.Windows.Forms.DataGridView();

            this.txtKhuMa = new System.Windows.Forms.TextBox();
            this.txtKhuTen = new System.Windows.Forms.TextBox();
            this.txtNVMa = new System.Windows.Forms.TextBox();
            this.txtNVTen = new System.Windows.Forms.TextBox();
            this.txtNVVaiTro = new System.Windows.Forms.TextBox();
            this.txtNVSDT = new System.Windows.Forms.TextBox();
            this.txtLoaiMa = new System.Windows.Forms.TextBox();
            this.txtLoaiTen = new System.Windows.Forms.TextBox();
            this.txtDVMa = new System.Windows.Forms.TextBox();
            this.txtDVTen = new System.Windows.Forms.TextBox();
            this.txtDVDVT = new System.Windows.Forms.TextBox();
            this.numDVGia = new System.Windows.Forms.NumericUpDown();
            this.txtQDMa = new System.Windows.Forms.TextBox();
            this.cboQDLoai = new System.Windows.Forms.ComboBox();
            this.txtQDMucDo = new System.Windows.Forms.TextBox();
            this.numQDTien = new System.Windows.Forms.NumericUpDown();

            this.btnThemKhu = new System.Windows.Forms.Button();
            this.btnThemNV = new System.Windows.Forms.Button();
            this.btnThemLoaiTN = new System.Windows.Forms.Button();
            this.btnThemDV = new System.Windows.Forms.Button();
            this.btnThemQD = new System.Windows.Forms.Button();
            this.btnDong = new System.Windows.Forms.Button();

            this.tabMain.SuspendLayout();
            this.tabKhu.SuspendLayout();
            this.tabNV.SuspendLayout();
            this.tabLoai.SuspendLayout();
            this.tabDV.SuspendLayout();
            this.tabQD.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoaiTN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDVGia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQDTien)).BeginInit();

            this.dgvKhu.Location = new System.Drawing.Point(10, 10);
            this.dgvKhu.Size = new System.Drawing.Size(700, 330);
            this.dgvKhu.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.txtKhuMa.Location = new System.Drawing.Point(10, 360);
            this.txtKhuMa.Width = 150;
            this.txtKhuTen.Location = new System.Drawing.Point(180, 360);
            this.txtKhuTen.Width = 220;
            this.btnThemKhu.Location = new System.Drawing.Point(420, 358);
            this.btnThemKhu.Size = new System.Drawing.Size(120, 30);
            this.btnThemKhu.Text = "Thêm";
            this.btnThemKhu.Click += new System.EventHandler(this.btnThemKhu_Click);
            this.tabKhu.Controls.Add(L("Mã khu", 10, 340));
            this.tabKhu.Controls.Add(L("Tên khu", 180, 340));
            this.tabKhu.Controls.Add(this.dgvKhu);
            this.tabKhu.Controls.Add(this.txtKhuMa);
            this.tabKhu.Controls.Add(this.txtKhuTen);
            this.tabKhu.Controls.Add(this.btnThemKhu);

            this.dgvNV.Location = new System.Drawing.Point(10, 10);
            this.dgvNV.Size = new System.Drawing.Size(700, 300);
            this.dgvNV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.txtNVMa.Location = new System.Drawing.Point(10, 335); this.txtNVMa.Width = 120;
            this.txtNVTen.Location = new System.Drawing.Point(140, 335); this.txtNVTen.Width = 170;
            this.txtNVVaiTro.Location = new System.Drawing.Point(330, 335); this.txtNVVaiTro.Width = 150;
            this.txtNVSDT.Location = new System.Drawing.Point(500, 335); this.txtNVSDT.Width = 120;
            this.btnThemNV.Location = new System.Drawing.Point(620, 333); this.btnThemNV.Size = new System.Drawing.Size(90, 30);
            this.btnThemNV.Text = "Thêm";
            this.btnThemNV.Click += new System.EventHandler(this.btnThemNV_Click);
            this.tabNV.Controls.Add(this.dgvNV);
            this.tabNV.Controls.Add(this.txtNVMa); this.tabNV.Controls.Add(this.txtNVTen);
            this.tabNV.Controls.Add(this.txtNVVaiTro); this.tabNV.Controls.Add(this.txtNVSDT);
            this.tabNV.Controls.Add(this.btnThemNV);

            this.dgvLoaiTN.Location = new System.Drawing.Point(10, 10);
            this.dgvLoaiTN.Size = new System.Drawing.Size(700, 330);
            this.dgvLoaiTN.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.txtLoaiMa.Location = new System.Drawing.Point(10, 360); this.txtLoaiMa.Width = 150;
            this.txtLoaiTen.Location = new System.Drawing.Point(180, 360); this.txtLoaiTen.Width = 220;
            this.btnThemLoaiTN.Location = new System.Drawing.Point(420, 358);
            this.btnThemLoaiTN.Size = new System.Drawing.Size(120, 30);
            this.btnThemLoaiTN.Text = "Thêm";
            this.btnThemLoaiTN.Click += new System.EventHandler(this.btnThemLoaiTN_Click);
            this.tabLoai.Controls.Add(this.dgvLoaiTN);
            this.tabLoai.Controls.Add(this.txtLoaiMa); this.tabLoai.Controls.Add(this.txtLoaiTen);
            this.tabLoai.Controls.Add(this.btnThemLoaiTN);

            this.dgvDV.Location = new System.Drawing.Point(10, 10);
            this.dgvDV.Size = new System.Drawing.Size(700, 300);
            this.dgvDV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.txtDVMa.Location = new System.Drawing.Point(10, 335); this.txtDVMa.Width = 100;
            this.txtDVTen.Location = new System.Drawing.Point(120, 335); this.txtDVTen.Width = 170;
            this.txtDVDVT.Location = new System.Drawing.Point(300, 335); this.txtDVDVT.Width = 100;
            this.numDVGia.Location = new System.Drawing.Point(410, 335); this.numDVGia.Width = 120; this.numDVGia.Maximum = 1000000000;
            this.btnThemDV.Location = new System.Drawing.Point(540, 333); this.btnThemDV.Size = new System.Drawing.Size(90, 30);
            this.btnThemDV.Text = "Thêm";
            this.btnThemDV.Click += new System.EventHandler(this.btnThemDV_Click);
            this.tabDV.Controls.Add(this.dgvDV);
            this.tabDV.Controls.Add(this.txtDVMa); this.tabDV.Controls.Add(this.txtDVTen);
            this.tabDV.Controls.Add(this.txtDVDVT); this.tabDV.Controls.Add(this.numDVGia);
            this.tabDV.Controls.Add(this.btnThemDV);

            this.dgvQD.Location = new System.Drawing.Point(10, 10);
            this.dgvQD.Size = new System.Drawing.Size(700, 280);
            this.dgvQD.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.txtQDMa.Location = new System.Drawing.Point(10, 315); this.txtQDMa.Width = 100;
            this.cboQDLoai.Location = new System.Drawing.Point(120, 315); this.cboQDLoai.Width = 150;
            this.txtQDMucDo.Location = new System.Drawing.Point(280, 315); this.txtQDMucDo.Width = 150;
            this.numQDTien.Location = new System.Drawing.Point(440, 315); this.numQDTien.Width = 120; this.numQDTien.Maximum = 1000000000;
            this.btnThemQD.Location = new System.Drawing.Point(570, 313); this.btnThemQD.Size = new System.Drawing.Size(90, 30);
            this.btnThemQD.Text = "Thêm";
            this.btnThemQD.Click += new System.EventHandler(this.btnThemQD_Click);
            this.tabQD.Controls.Add(this.dgvQD);
            this.tabQD.Controls.Add(this.txtQDMa); this.tabQD.Controls.Add(this.cboQDLoai);
            this.tabQD.Controls.Add(this.txtQDMucDo); this.tabQD.Controls.Add(this.numQDTien);
            this.tabQD.Controls.Add(this.btnThemQD);

            this.tabKhu.Text = "Khu vực";
            this.tabNV.Text = "Nhân viên";
            this.tabLoai.Text = "Loại tiện nghi";
            this.tabDV.Text = "Dịch vụ";
            this.tabQD.Text = "Quy định đền bù";

            this.tabMain.Location = new System.Drawing.Point(10, 10);
            this.tabMain.Size = new System.Drawing.Size(730, 420);
            this.tabMain.TabPages.Add(this.tabKhu);
            this.tabMain.TabPages.Add(this.tabNV);
            this.tabMain.TabPages.Add(this.tabLoai);
            this.tabMain.TabPages.Add(this.tabDV);
            this.tabMain.TabPages.Add(this.tabQD);

            this.btnDong.Location = new System.Drawing.Point(640, 440);
            this.btnDong.Size = new System.Drawing.Size(100, 30);
            this.btnDong.Text = "Đóng";
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);

            this.ClientSize = new System.Drawing.Size(755, 485);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý danh mục";
            this.Load += new System.EventHandler(this.FrmDanhMuc_Load);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.btnDong);

            this.tabMain.ResumeLayout(false);
            this.tabKhu.ResumeLayout(false); this.tabKhu.PerformLayout();
            this.tabNV.ResumeLayout(false); this.tabNV.PerformLayout();
            this.tabLoai.ResumeLayout(false); this.tabLoai.PerformLayout();
            this.tabDV.ResumeLayout(false); this.tabDV.PerformLayout();
            this.tabQD.ResumeLayout(false); this.tabQD.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoaiTN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDVGia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQDTien)).EndInit();
            this.ResumeLayout(false);
        }
    }
}