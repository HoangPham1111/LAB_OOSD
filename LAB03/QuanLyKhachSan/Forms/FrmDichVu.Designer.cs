namespace QuanLyKhachSan.Forms
{
    partial class FrmDichVu
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lbl1, lbl2, lbl3, lbl4, lbl5;
        private System.Windows.Forms.ComboBox cboLuot, cboDV, cboNV;
        private System.Windows.Forms.TextBox txtPhong;
        private System.Windows.Forms.DateTimePicker dtNgay;
        private System.Windows.Forms.NumericUpDown numSL;
        private System.Windows.Forms.DataGridView dgvLichSu;
        private System.Windows.Forms.Button btnGhi, btnDong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lbl1 = new System.Windows.Forms.Label(); this.lbl2 = new System.Windows.Forms.Label();
            this.lbl3 = new System.Windows.Forms.Label(); this.lbl4 = new System.Windows.Forms.Label(); this.lbl5 = new System.Windows.Forms.Label();
            this.cboLuot = new System.Windows.Forms.ComboBox(); this.cboDV = new System.Windows.Forms.ComboBox(); this.cboNV = new System.Windows.Forms.ComboBox();
            this.txtPhong = new System.Windows.Forms.TextBox(); this.dtNgay = new System.Windows.Forms.DateTimePicker();
            this.numSL = new System.Windows.Forms.NumericUpDown(); this.dgvLichSu = new System.Windows.Forms.DataGridView();
            this.btnGhi = new System.Windows.Forms.Button(); this.btnDong = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.numSL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichSu)).BeginInit();

            this.lbl1.Text = "Phiếu đang ở"; this.lbl1.Location = new System.Drawing.Point(20, 25); this.lbl1.AutoSize = true;
            this.lbl2.Text = "Phòng"; this.lbl2.Location = new System.Drawing.Point(250, 25); this.lbl2.AutoSize = true;
            this.lbl3.Text = "Dịch vụ"; this.lbl3.Location = new System.Drawing.Point(430, 25); this.lbl3.AutoSize = true;
            this.lbl4.Text = "Ngày"; this.lbl4.Location = new System.Drawing.Point(20, 75); this.lbl4.AutoSize = true;
            this.lbl5.Text = "Số lượng"; this.lbl5.Location = new System.Drawing.Point(250, 75); this.lbl5.AutoSize = true;

            this.cboLuot.Location = new System.Drawing.Point(20, 45); this.cboLuot.Width = 200;
            this.cboLuot.SelectedIndexChanged += new System.EventHandler(this.cboLuot_SelectedIndexChanged);
            this.txtPhong.Location = new System.Drawing.Point(250, 45); this.txtPhong.Width = 150; this.txtPhong.ReadOnly = true;
            this.cboDV.Location = new System.Drawing.Point(430, 45); this.cboDV.Width = 200;
            this.cboNV.Location = new System.Drawing.Point(650, 45); this.cboNV.Width = 170;
            this.dtNgay.Location = new System.Drawing.Point(20, 95); this.dtNgay.Width = 200;
            this.numSL.Location = new System.Drawing.Point(250, 95); this.numSL.Width = 100; this.numSL.Minimum = 1; this.numSL.Maximum = 1000;

            this.btnGhi.Location = new System.Drawing.Point(380, 92); this.btnGhi.Size = new System.Drawing.Size(100, 30);
            this.btnGhi.Text = "Ghi dịch vụ"; this.btnGhi.Click += new System.EventHandler(this.btnGhi_Click);

            this.dgvLichSu.Location = new System.Drawing.Point(20, 145); this.dgvLichSu.Size = new System.Drawing.Size(800, 350);
            this.dgvLichSu.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.btnDong.Location = new System.Drawing.Point(720, 510); this.btnDong.Size = new System.Drawing.Size(100, 30);
            this.btnDong.Text = "Đóng"; this.btnDong.Click += new System.EventHandler(this.btnDong_Click);

            this.ClientSize = new System.Drawing.Size(850, 560);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ghi nhận dịch vụ";
            this.Load += new System.EventHandler(this.Frm_Load);

            this.Controls.Add(this.lbl1); this.Controls.Add(this.lbl2); this.Controls.Add(this.lbl3); this.Controls.Add(this.lbl4); this.Controls.Add(this.lbl5);
            this.Controls.Add(this.cboLuot); this.Controls.Add(this.txtPhong); this.Controls.Add(this.cboDV); this.Controls.Add(this.cboNV);
            this.Controls.Add(this.dtNgay); this.Controls.Add(this.numSL); this.Controls.Add(this.btnGhi); this.Controls.Add(this.dgvLichSu); this.Controls.Add(this.btnDong);

            ((System.ComponentModel.ISupportInitialize)(this.numSL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichSu)).EndInit();
            this.ResumeLayout(false); this.PerformLayout();
        }
    }
}