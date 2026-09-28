namespace QuanLyKhachSan.Forms
{
    partial class FrmDatPhong
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabKhach;
        private System.Windows.Forms.TabPage tabDat;
        private System.Windows.Forms.TabPage tabNhan;

        private System.Windows.Forms.TextBox txtMaKH, txtTenKH, txtCMND, txtQT, txtSDT;
        private System.Windows.Forms.DataGridView dgvKhach;
        private System.Windows.Forms.Button btnThemKhach;

        private System.Windows.Forms.TextBox txtSoPhieu, txtPhieuChon;
        private System.Windows.Forms.ComboBox cboKhach, cboNV, cboKenh;
        private System.Windows.Forms.DateTimePicker dtLap, dtNhan, dtTra;
        private System.Windows.Forms.NumericUpDown numCoc, numSoNguoi;
        private System.Windows.Forms.DataGridView dgvPhong, dgvChon, dgvPhieu, dgvCT, dgvNguoi;
        private System.Windows.Forms.Button btnLapPhieu, btnThemPhong, btnBoPhong;

        private System.Windows.Forms.TextBox txtNguoiPhong, txtNguoiTen, txtNguoiCMND, txtNguoiQT;
        private System.Windows.Forms.Button btnThemNguoi, btnNhanPhong, btnNoShow, btnDong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabKhach = new System.Windows.Forms.TabPage();
            this.tabDat = new System.Windows.Forms.TabPage();
            this.tabNhan = new System.Windows.Forms.TabPage();

            this.txtMaKH = new System.Windows.Forms.TextBox(); this.txtTenKH = new System.Windows.Forms.TextBox();
            this.txtCMND = new System.Windows.Forms.TextBox(); this.txtQT = new System.Windows.Forms.TextBox();
            this.txtSDT = new System.Windows.Forms.TextBox(); this.dgvKhach = new System.Windows.Forms.DataGridView();
            this.btnThemKhach = new System.Windows.Forms.Button();

            this.txtSoPhieu = new System.Windows.Forms.TextBox();
            this.cboKhach = new System.Windows.Forms.ComboBox(); this.cboNV = new System.Windows.Forms.ComboBox(); this.cboKenh = new System.Windows.Forms.ComboBox();
            this.dtLap = new System.Windows.Forms.DateTimePicker(); this.dtNhan = new System.Windows.Forms.DateTimePicker(); this.dtTra = new System.Windows.Forms.DateTimePicker();
            this.numCoc = new System.Windows.Forms.NumericUpDown(); this.numSoNguoi = new System.Windows.Forms.NumericUpDown();
            this.dgvPhong = new System.Windows.Forms.DataGridView(); this.dgvChon = new System.Windows.Forms.DataGridView(); this.dgvPhieu = new System.Windows.Forms.DataGridView();
            this.btnLapPhieu = new System.Windows.Forms.Button(); this.btnThemPhong = new System.Windows.Forms.Button(); this.btnBoPhong = new System.Windows.Forms.Button();

            this.txtPhieuChon = new System.Windows.Forms.TextBox();
            this.txtNguoiPhong = new System.Windows.Forms.TextBox(); this.txtNguoiTen = new System.Windows.Forms.TextBox();
            this.txtNguoiCMND = new System.Windows.Forms.TextBox(); this.txtNguoiQT = new System.Windows.Forms.TextBox();
            this.dgvCT = new System.Windows.Forms.DataGridView(); this.dgvNguoi = new System.Windows.Forms.DataGridView();
            this.btnThemNguoi = new System.Windows.Forms.Button(); this.btnNhanPhong = new System.Windows.Forms.Button();
            this.btnNoShow = new System.Windows.Forms.Button(); this.btnDong = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvKhach)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhieu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNguoi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSoNguoi)).BeginInit();

            this.tabMain.Location = new System.Drawing.Point(10, 10);
            this.tabMain.Size = new System.Drawing.Size(1050, 650);
            this.tabMain.TabPages.Add(this.tabKhach); this.tabMain.TabPages.Add(this.tabDat); this.tabMain.TabPages.Add(this.tabNhan);

            this.tabKhach.Text = "Khách hàng";
            this.dgvKhach.Location = new System.Drawing.Point(10, 150); this.dgvKhach.Size = new System.Drawing.Size(1000, 430);
            this.dgvKhach.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.txtMaKH.Location = new System.Drawing.Point(10, 30); this.txtMaKH.Width = 130;
            this.txtTenKH.Location = new System.Drawing.Point(150, 30); this.txtTenKH.Width = 200;
            this.txtCMND.Location = new System.Drawing.Point(360, 30); this.txtCMND.Width = 150;
            this.txtQT.Location = new System.Drawing.Point(520, 30); this.txtQT.Width = 130;
            this.txtSDT.Location = new System.Drawing.Point(660, 30); this.txtSDT.Width = 130;
            this.btnThemKhach.Location = new System.Drawing.Point(810, 28); this.btnThemKhach.Size = new System.Drawing.Size(100, 30);
            this.btnThemKhach.Text = "Thêm khách";
            this.btnThemKhach.Click += new System.EventHandler(this.btnThemKhach_Click);
            this.tabKhach.Controls.Add(this.txtMaKH); this.tabKhach.Controls.Add(this.txtTenKH);
            this.tabKhach.Controls.Add(this.txtCMND); this.tabKhach.Controls.Add(this.txtQT); this.tabKhach.Controls.Add(this.txtSDT);
            this.tabKhach.Controls.Add(this.btnThemKhach); this.tabKhach.Controls.Add(this.dgvKhach);

            this.tabDat.Text = "Đặt phòng";
            this.txtSoPhieu.Location = new System.Drawing.Point(10, 20); this.txtSoPhieu.Width = 110;
            this.cboKhach.Location = new System.Drawing.Point(130, 20); this.cboKhach.Width = 170;
            this.cboNV.Location = new System.Drawing.Point(310, 20); this.cboNV.Width = 170;
            this.cboKenh.Location = new System.Drawing.Point(490, 20); this.cboKenh.Width = 120;
            this.dtLap.Location = new System.Drawing.Point(620, 20); this.dtLap.Width = 130;
            this.dtNhan.Location = new System.Drawing.Point(760, 20); this.dtNhan.Width = 120;
            this.dtTra.Location = new System.Drawing.Point(890, 20); this.dtTra.Width = 120;
            this.numCoc.Location = new System.Drawing.Point(10, 60); this.numCoc.Width = 140; this.numCoc.Maximum = 1000000000;

            this.dgvPhong.Location = new System.Drawing.Point(10, 110); this.dgvPhong.Size = new System.Drawing.Size(480, 220);
            this.dgvPhong.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.numSoNguoi.Location = new System.Drawing.Point(510, 140); this.numSoNguoi.Width = 80; this.numSoNguoi.Minimum = 1; this.numSoNguoi.Maximum = 100;
            this.btnThemPhong.Location = new System.Drawing.Point(600, 135); this.btnThemPhong.Size = new System.Drawing.Size(110, 30); this.btnThemPhong.Text = "Thêm phòng";
            this.btnThemPhong.Click += new System.EventHandler(this.btnThemPhong_Click);
            this.dgvChon.Location = new System.Drawing.Point(510, 185); this.dgvChon.Size = new System.Drawing.Size(470, 145);
            this.dgvChon.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.btnBoPhong.Location = new System.Drawing.Point(510, 340); this.btnBoPhong.Size = new System.Drawing.Size(110, 30); this.btnBoPhong.Text = "Bỏ phòng";
            this.btnBoPhong.Click += new System.EventHandler(this.btnBoPhong_Click);
            this.btnLapPhieu.Location = new System.Drawing.Point(650, 340); this.btnLapPhieu.Size = new System.Drawing.Size(120, 30); this.btnLapPhieu.Text = "Lập phiếu";
            this.btnLapPhieu.Click += new System.EventHandler(this.btnLapPhieu_Click);
            this.dgvPhieu.Location = new System.Drawing.Point(10, 385); this.dgvPhieu.Size = new System.Drawing.Size(970, 210);
            this.dgvPhieu.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPhieu.SelectionChanged += new System.EventHandler(this.dgvPhieu_SelectionChanged);

            this.tabDat.Controls.Add(this.txtSoPhieu); this.tabDat.Controls.Add(this.cboKhach); this.tabDat.Controls.Add(this.cboNV); this.tabDat.Controls.Add(this.cboKenh);
            this.tabDat.Controls.Add(this.dtLap); this.tabDat.Controls.Add(this.dtNhan); this.tabDat.Controls.Add(this.dtTra); this.tabDat.Controls.Add(this.numCoc);
            this.tabDat.Controls.Add(this.dgvPhong); this.tabDat.Controls.Add(this.numSoNguoi); this.tabDat.Controls.Add(this.btnThemPhong);
            this.tabDat.Controls.Add(this.dgvChon); this.tabDat.Controls.Add(this.btnBoPhong); this.tabDat.Controls.Add(this.btnLapPhieu); this.tabDat.Controls.Add(this.dgvPhieu);

            this.tabNhan.Text = "Nhận phòng";
            this.txtPhieuChon.Location = new System.Drawing.Point(10, 20); this.txtPhieuChon.Width = 120;
            this.txtNguoiPhong.Location = new System.Drawing.Point(145, 20); this.txtNguoiPhong.Width = 100;
            this.txtNguoiTen.Location = new System.Drawing.Point(255, 20); this.txtNguoiTen.Width = 170;
            this.txtNguoiCMND.Location = new System.Drawing.Point(435, 20); this.txtNguoiCMND.Width = 150;
            this.txtNguoiQT.Location = new System.Drawing.Point(595, 20); this.txtNguoiQT.Width = 130;
            this.btnThemNguoi.Location = new System.Drawing.Point(740, 18); this.btnThemNguoi.Size = new System.Drawing.Size(110, 30); this.btnThemNguoi.Text = "Thêm người";
            this.btnThemNguoi.Click += new System.EventHandler(this.btnThemNguoi_Click);

            this.dgvCT.Location = new System.Drawing.Point(10, 70); this.dgvCT.Size = new System.Drawing.Size(480, 220);
            this.dgvCT.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvNguoi.Location = new System.Drawing.Point(500, 70); this.dgvNguoi.Size = new System.Drawing.Size(480, 220);
            this.dgvNguoi.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.btnNhanPhong.Location = new System.Drawing.Point(10, 320); this.btnNhanPhong.Size = new System.Drawing.Size(120, 35); this.btnNhanPhong.Text = "Nhận phòng";
            this.btnNhanPhong.Click += new System.EventHandler(this.btnNhanPhong_Click);
            this.btnNoShow.Location = new System.Drawing.Point(145, 320); this.btnNoShow.Size = new System.Drawing.Size(120, 35); this.btnNoShow.Text = "No-show";
            this.btnNoShow.Click += new System.EventHandler(this.btnNoShow_Click);
            this.tabNhan.Controls.Add(this.txtPhieuChon); this.tabNhan.Controls.Add(this.txtNguoiPhong);
            this.tabNhan.Controls.Add(this.txtNguoiTen); this.tabNhan.Controls.Add(this.txtNguoiCMND); this.tabNhan.Controls.Add(this.txtNguoiQT);
            this.tabNhan.Controls.Add(this.btnThemNguoi); this.tabNhan.Controls.Add(this.dgvCT); this.tabNhan.Controls.Add(this.dgvNguoi);
            this.tabNhan.Controls.Add(this.btnNhanPhong); this.tabNhan.Controls.Add(this.btnNoShow);

            this.btnDong.Location = new System.Drawing.Point(960, 670); this.btnDong.Size = new System.Drawing.Size(100, 30); this.btnDong.Text = "Đóng";
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);

            this.ClientSize = new System.Drawing.Size(1080, 715);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đặt / Nhận phòng";
            this.Load += new System.EventHandler(this.Frm_Load);
            this.Controls.Add(this.tabMain); this.Controls.Add(this.btnDong);

            this.tabMain.ResumeLayout(false); this.tabKhach.ResumeLayout(false); this.tabDat.ResumeLayout(false); this.tabNhan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhach)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhieu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNguoi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSoNguoi)).EndInit();
            this.ResumeLayout(false);
        }
    }
}