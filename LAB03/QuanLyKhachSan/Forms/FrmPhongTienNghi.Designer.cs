namespace QuanLyKhachSan.Forms
{
    partial class FrmPhongTienNghi
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPhong;
        private System.Windows.Forms.TabPage tabTN;
        private System.Windows.Forms.TabPage tabLD;

        private System.Windows.Forms.TextBox txtPhong;
        private System.Windows.Forms.ComboBox cboKhu;
        private System.Windows.Forms.NumericUpDown numMax;
        private System.Windows.Forms.NumericUpDown numGia;
        private System.Windows.Forms.DataGridView dgvPhong;

        private System.Windows.Forms.TextBox txtMaTN;
        private System.Windows.Forms.ComboBox cboLoai;
        private System.Windows.Forms.NumericUpDown numSTT;
        private System.Windows.Forms.TextBox txtTinhTrang;
        private System.Windows.Forms.DataGridView dgvTN;

        private System.Windows.Forms.TextBox txtSoLD;
        private System.Windows.Forms.ComboBox cboTN;
        private System.Windows.Forms.ComboBox cboPhong;
        private System.Windows.Forms.DateTimePicker dtNgay;
        private System.Windows.Forms.TextBox txtTTLD;
        private System.Windows.Forms.ComboBox cboNV;
        private System.Windows.Forms.TextBox txtGhiChu;
        private System.Windows.Forms.DataGridView dgvLD;

        private System.Windows.Forms.Button btnThemPhong;
        private System.Windows.Forms.Button btnThemTN;
        private System.Windows.Forms.Button btnLapDat;
        private System.Windows.Forms.Button btnDong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPhong = new System.Windows.Forms.TabPage();
            this.tabTN = new System.Windows.Forms.TabPage();
            this.tabLD = new System.Windows.Forms.TabPage();

            this.txtPhong = new System.Windows.Forms.TextBox();
            this.cboKhu = new System.Windows.Forms.ComboBox();
            this.numMax = new System.Windows.Forms.NumericUpDown();
            this.numGia = new System.Windows.Forms.NumericUpDown();
            this.dgvPhong = new System.Windows.Forms.DataGridView();

            this.txtMaTN = new System.Windows.Forms.TextBox();
            this.cboLoai = new System.Windows.Forms.ComboBox();
            this.numSTT = new System.Windows.Forms.NumericUpDown();
            this.txtTinhTrang = new System.Windows.Forms.TextBox();
            this.dgvTN = new System.Windows.Forms.DataGridView();

            this.txtSoLD = new System.Windows.Forms.TextBox();
            this.cboTN = new System.Windows.Forms.ComboBox();
            this.cboPhong = new System.Windows.Forms.ComboBox();
            this.dtNgay = new System.Windows.Forms.DateTimePicker();
            this.txtTTLD = new System.Windows.Forms.TextBox();
            this.cboNV = new System.Windows.Forms.ComboBox();
            this.txtGhiChu = new System.Windows.Forms.TextBox();
            this.dgvLD = new System.Windows.Forms.DataGridView();

            this.btnThemPhong = new System.Windows.Forms.Button();
            this.btnThemTN = new System.Windows.Forms.Button();
            this.btnLapDat = new System.Windows.Forms.Button();
            this.btnDong = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.numMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSTT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLD)).BeginInit();

            this.tabMain.Location = new System.Drawing.Point(10, 10);
            this.tabMain.Size = new System.Drawing.Size(820, 560);
            this.tabMain.TabPages.Add(this.tabPhong);
            this.tabMain.TabPages.Add(this.tabTN);
            this.tabMain.TabPages.Add(this.tabLD);

            this.tabPhong.Text = "Phòng";
            this.tabTN.Text = "Tiện nghi";
            this.tabLD.Text = "Lắp đặt / luân chuyển";

            this.dgvPhong.Location = new System.Drawing.Point(10, 10);
            this.dgvPhong.Size = new System.Drawing.Size(790, 360);
            this.dgvPhong.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.txtPhong.Location = new System.Drawing.Point(10, 400); this.txtPhong.Width = 120;
            this.cboKhu.Location = new System.Drawing.Point(145, 400); this.cboKhu.Width = 160;
            this.numMax.Location = new System.Drawing.Point(320, 400); this.numMax.Width = 80; this.numMax.Minimum = 1; this.numMax.Maximum = 100;
            this.numGia.Location = new System.Drawing.Point(415, 400); this.numGia.Width = 120; this.numGia.Maximum = 1000000000;
            this.btnThemPhong.Location = new System.Drawing.Point(550, 398); this.btnThemPhong.Size = new System.Drawing.Size(100, 30);
            this.btnThemPhong.Text = "Thêm phòng";
            this.btnThemPhong.Click += new System.EventHandler(this.btnThemPhong_Click);
            this.tabPhong.Controls.Add(this.dgvPhong);
            this.tabPhong.Controls.Add(this.txtPhong); this.tabPhong.Controls.Add(this.cboKhu);
            this.tabPhong.Controls.Add(this.numMax); this.tabPhong.Controls.Add(this.numGia);
            this.tabPhong.Controls.Add(this.btnThemPhong);

            this.dgvTN.Location = new System.Drawing.Point(10, 10);
            this.dgvTN.Size = new System.Drawing.Size(790, 360);
            this.dgvTN.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.txtMaTN.Location = new System.Drawing.Point(10, 400); this.txtMaTN.Width = 120;
            this.cboLoai.Location = new System.Drawing.Point(145, 400); this.cboLoai.Width = 150;
            this.numSTT.Location = new System.Drawing.Point(310, 400); this.numSTT.Width = 80; this.numSTT.Minimum = 1; this.numSTT.Maximum = 1000;
            this.txtTinhTrang.Location = new System.Drawing.Point(405, 400); this.txtTinhTrang.Width = 150;
            this.btnThemTN.Location = new System.Drawing.Point(570, 398); this.btnThemTN.Size = new System.Drawing.Size(110, 30);
            this.btnThemTN.Text = "Thêm tiện nghi";
            this.btnThemTN.Click += new System.EventHandler(this.btnThemTN_Click);
            this.tabTN.Controls.Add(this.dgvTN);
            this.tabTN.Controls.Add(this.txtMaTN); this.tabTN.Controls.Add(this.cboLoai);
            this.tabTN.Controls.Add(this.numSTT); this.tabTN.Controls.Add(this.txtTinhTrang);
            this.tabTN.Controls.Add(this.btnThemTN);

            this.dgvLD.Location = new System.Drawing.Point(10, 180);
            this.dgvLD.Size = new System.Drawing.Size(790, 300);
            this.dgvLD.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.txtSoLD.Location = new System.Drawing.Point(10, 20); this.txtSoLD.Width = 110;
            this.cboTN.Location = new System.Drawing.Point(130, 20); this.cboTN.Width = 130;
            this.cboPhong.Location = new System.Drawing.Point(270, 20); this.cboPhong.Width = 110;
            this.dtNgay.Location = new System.Drawing.Point(390, 20); this.dtNgay.Width = 130;
            this.txtTTLD.Location = new System.Drawing.Point(530, 20); this.txtTTLD.Width = 120;
            this.cboNV.Location = new System.Drawing.Point(10, 70); this.cboNV.Width = 150;
            this.txtGhiChu.Location = new System.Drawing.Point(175, 70); this.txtGhiChu.Width = 300;
            this.btnLapDat.Location = new System.Drawing.Point(500, 68); this.btnLapDat.Size = new System.Drawing.Size(120, 30);
            this.btnLapDat.Text = "Lập phiếu";
            this.btnLapDat.Click += new System.EventHandler(this.btnLapDat_Click);

            this.tabLD.Controls.Add(this.dgvLD);
            this.tabLD.Controls.Add(this.txtSoLD); this.tabLD.Controls.Add(this.cboTN);
            this.tabLD.Controls.Add(this.cboPhong); this.tabLD.Controls.Add(this.dtNgay);
            this.tabLD.Controls.Add(this.txtTTLD); this.tabLD.Controls.Add(this.cboNV);
            this.tabLD.Controls.Add(this.txtGhiChu); this.tabLD.Controls.Add(this.btnLapDat);

            this.btnDong.Location = new System.Drawing.Point(730, 580);
            this.btnDong.Size = new System.Drawing.Size(100, 30);
            this.btnDong.Text = "Đóng";
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);

            this.ClientSize = new System.Drawing.Size(850, 625);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Phòng - Tiện nghi";
            this.Load += new System.EventHandler(this.Frm_Load);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.btnDong);

            this.tabMain.ResumeLayout(false);
            this.tabPhong.ResumeLayout(false); this.tabTN.ResumeLayout(false); this.tabLD.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSTT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLD)).EndInit();
            this.ResumeLayout(false);
        }
    }
}