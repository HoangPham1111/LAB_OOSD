namespace QuanLyKhachSan.Forms
{
    partial class FrmTraPhong
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ComboBox cboDat, cboNV, cboNV2, cboHT;
        private System.Windows.Forms.DataGridView dgvPhong, dgvTN, dgvDBChon, dgvHD;
        private System.Windows.Forms.TextBox txtPhong, txtSoDB, txtMucDo, txtSoHD, txtHDChon, txtMaTT;
        private System.Windows.Forms.NumericUpDown numDenBu, numSoNgay, numTienTT;
        private System.Windows.Forms.Button btnThemDB, btnLapDB, btnLapHD, btnThanhToan, btnTraPhong, btnDong;
        private System.Windows.Forms.Label l1, l2, l3, l4, l5;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.cboDat = new System.Windows.Forms.ComboBox(); this.cboNV = new System.Windows.Forms.ComboBox();
            this.cboNV2 = new System.Windows.Forms.ComboBox(); this.cboHT = new System.Windows.Forms.ComboBox();
            this.dgvPhong = new System.Windows.Forms.DataGridView(); this.dgvTN = new System.Windows.Forms.DataGridView();
            this.dgvDBChon = new System.Windows.Forms.DataGridView(); this.dgvHD = new System.Windows.Forms.DataGridView();
            this.txtPhong = new System.Windows.Forms.TextBox(); this.txtSoDB = new System.Windows.Forms.TextBox();
            this.txtMucDo = new System.Windows.Forms.TextBox(); this.txtSoHD = new System.Windows.Forms.TextBox();
            this.txtHDChon = new System.Windows.Forms.TextBox(); this.txtMaTT = new System.Windows.Forms.TextBox();
            this.numDenBu = new System.Windows.Forms.NumericUpDown(); this.numSoNgay = new System.Windows.Forms.NumericUpDown();
            this.numTienTT = new System.Windows.Forms.NumericUpDown();

            this.btnThemDB = new System.Windows.Forms.Button(); this.btnLapDB = new System.Windows.Forms.Button();
            this.btnLapHD = new System.Windows.Forms.Button(); this.btnThanhToan = new System.Windows.Forms.Button();
            this.btnTraPhong = new System.Windows.Forms.Button(); this.btnDong = new System.Windows.Forms.Button();

            this.l1 = new System.Windows.Forms.Label(); this.l2 = new System.Windows.Forms.Label(); this.l3 = new System.Windows.Forms.Label();
            this.l4 = new System.Windows.Forms.Label(); this.l5 = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dgvPhong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDBChon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDenBu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSoNgay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTienTT)).BeginInit();

            this.l1.Text = "Phiếu đang ở"; this.l1.Location = new System.Drawing.Point(15, 15); this.l1.AutoSize = true;
            this.l2.Text = "Đền bù"; this.l2.Location = new System.Drawing.Point(15, 235); this.l2.AutoSize = true;
            this.l3.Text = "Hóa đơn"; this.l3.Location = new System.Drawing.Point(540, 15); this.l3.AutoSize = true;
            this.l4.Text = "Thanh toán"; this.l4.Location = new System.Drawing.Point(540, 235); this.l4.AutoSize = true;
            this.l5.Text = "Phòng"; this.l5.Location = new System.Drawing.Point(250, 15); this.l5.AutoSize = true;

            this.cboDat.Location = new System.Drawing.Point(15, 35); this.cboDat.Width = 200;
            

            this.txtPhong.Location = new System.Drawing.Point(250, 35); this.txtPhong.Width = 120; this.txtPhong.ReadOnly = true;
            this.dgvPhong.Location = new System.Drawing.Point(15, 65); this.dgvPhong.Size = new System.Drawing.Size(470, 150);
            this.dgvPhong.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPhong.SelectionChanged += new System.EventHandler(this.dgvPhong_SelectionChanged);

            this.dgvTN.Location = new System.Drawing.Point(15, 265); this.dgvTN.Size = new System.Drawing.Size(470, 150);
            this.dgvTN.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.txtSoDB.Location = new System.Drawing.Point(15, 425); this.txtSoDB.Width = 100;
            this.txtMucDo.Location = new System.Drawing.Point(125, 425); this.txtMucDo.Width = 130;
            this.numDenBu.Location = new System.Drawing.Point(265, 425); this.numDenBu.Width = 120; this.numDenBu.Maximum = 1000000000;
            this.btnThemDB.Location = new System.Drawing.Point(395, 423); this.btnThemDB.Size = new System.Drawing.Size(90, 30); this.btnThemDB.Text = "Thêm";
            this.btnThemDB.Click += new System.EventHandler(this.btnThemDB_Click);

            this.dgvDBChon.Location = new System.Drawing.Point(15, 465); this.dgvDBChon.Size = new System.Drawing.Size(470, 130);
            this.dgvDBChon.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.btnLapDB.Location = new System.Drawing.Point(15, 605); this.btnLapDB.Size = new System.Drawing.Size(120, 30); this.btnLapDB.Text = "Lập đền bù";
            this.btnLapDB.Click += new System.EventHandler(this.btnLapDB_Click);

            this.cboNV.Location = new System.Drawing.Point(540, 35); this.cboNV.Width = 170;
            this.txtSoHD.Location = new System.Drawing.Point(720, 35); this.txtSoHD.Width = 110;
            this.numSoNgay.Location = new System.Drawing.Point(845, 35); this.numSoNgay.Width = 70; this.numSoNgay.Minimum = 1; this.numSoNgay.Maximum = 100;

            this.btnLapHD.Location = new System.Drawing.Point(540, 70); this.btnLapHD.Size = new System.Drawing.Size(110, 30); this.btnLapHD.Text = "Lập hóa đơn";
            this.btnLapHD.Click += new System.EventHandler(this.btnLapHD_Click);

            this.dgvHD.Location = new System.Drawing.Point(540, 110); this.dgvHD.Size = new System.Drawing.Size(430, 105);
            this.dgvHD.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHD.SelectionChanged += new System.EventHandler(this.dgvHD_SelectionChanged);

            this.txtHDChon.Location = new System.Drawing.Point(540, 265); this.txtHDChon.Width = 130; this.txtHDChon.ReadOnly = true;
            this.txtMaTT.Location = new System.Drawing.Point(680, 265); this.txtMaTT.Width = 100;
            this.cboHT.Location = new System.Drawing.Point(790, 265); this.cboHT.Width = 120;
            this.numTienTT.Location = new System.Drawing.Point(920, 265); this.numTienTT.Width = 80; this.numTienTT.Maximum = 1000000000;

            this.btnThanhToan.Location = new System.Drawing.Point(540, 305); this.btnThanhToan.Size = new System.Drawing.Size(120, 30); this.btnThanhToan.Text = "Thanh toán";
            this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click);

            this.btnTraPhong.Location = new System.Drawing.Point(540, 355); this.btnTraPhong.Size = new System.Drawing.Size(120, 35); this.btnTraPhong.Text = "Trả phòng";
            this.btnTraPhong.Click += new System.EventHandler(this.btnTraPhong_Click);

            this.btnDong.Location = new System.Drawing.Point(870, 605); this.btnDong.Size = new System.Drawing.Size(100, 30); this.btnDong.Text = "Đóng";
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);

            this.cboHT.Items.AddRange(new object[] { "Tiền mặt", "Chuyển khoản", "Thẻ", "Ví điện tử" });
            this.cboHT.SelectedIndex = 0;

            this.ClientSize = new System.Drawing.Size(1000, 650);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trả phòng - Thanh toán";
            this.Load += new System.EventHandler(this.Frm_Load);

            this.Controls.Add(this.l1); this.Controls.Add(this.l2); this.Controls.Add(this.l3); this.Controls.Add(this.l4); this.Controls.Add(this.l5);
            this.Controls.Add(this.cboDat); this.Controls.Add(this.txtPhong); this.Controls.Add(this.dgvPhong); this.Controls.Add(this.dgvTN);
            this.Controls.Add(this.txtSoDB); this.Controls.Add(this.txtMucDo); this.Controls.Add(this.numDenBu); this.Controls.Add(this.btnThemDB);
            this.Controls.Add(this.dgvDBChon); this.Controls.Add(this.btnLapDB);
            this.Controls.Add(this.cboNV); this.Controls.Add(this.txtSoHD); this.Controls.Add(this.numSoNgay); this.Controls.Add(this.btnLapHD); this.Controls.Add(this.dgvHD);
            this.Controls.Add(this.txtHDChon); this.Controls.Add(this.txtMaTT); this.Controls.Add(this.cboHT); this.Controls.Add(this.numTienTT);
            this.Controls.Add(this.btnThanhToan); this.Controls.Add(this.btnTraPhong); this.Controls.Add(this.btnDong);

            ((System.ComponentModel.ISupportInitialize)(this.dgvPhong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDBChon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDenBu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSoNgay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTienTT)).EndInit();
            this.ResumeLayout(false); this.PerformLayout();
        }
    }
}   