namespace eSHOPPING.Forms
{
    partial class FrmChiTietSanPham
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblMa;
        private System.Windows.Forms.Label lblTen;
        private System.Windows.Forms.Label lblHang;
        private System.Windows.Forms.Label lblGia;
        private System.Windows.Forms.Label lblTon;
        private System.Windows.Forms.Label lblSL;
        private System.Windows.Forms.NumericUpDown numSoLuong;
        private System.Windows.Forms.Label lblMoTa;
        private System.Windows.Forms.Label lblTS;
        private System.Windows.Forms.TextBox txtMoTa;
        private System.Windows.Forms.TextBox txtThongSo;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnDong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblMa = new System.Windows.Forms.Label();
            this.lblTen = new System.Windows.Forms.Label();
            this.lblHang = new System.Windows.Forms.Label();
            this.lblGia = new System.Windows.Forms.Label();
            this.lblTon = new System.Windows.Forms.Label();
            this.lblSL = new System.Windows.Forms.Label();
            this.numSoLuong = new System.Windows.Forms.NumericUpDown();
            this.lblMoTa = new System.Windows.Forms.Label();
            this.lblTS = new System.Windows.Forms.Label();
            this.txtMoTa = new System.Windows.Forms.TextBox();
            this.txtThongSo = new System.Windows.Forms.TextBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnDong = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.numSoLuong)).BeginInit();
            this.SuspendLayout();

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(30, 25);
            this.lblTitle.Text = "CHI TIẾT SẢN PHẨM";

            this.lblMa.AutoSize = true;
            this.lblMa.Location = new System.Drawing.Point(35, 85);

            this.lblTen.AutoSize = true;
            this.lblTen.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTen.Location = new System.Drawing.Point(35, 115);

            this.lblHang.AutoSize = true;
            this.lblHang.Location = new System.Drawing.Point(35, 160);

            this.lblGia.AutoSize = true;
            this.lblGia.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblGia.ForeColor = System.Drawing.Color.Red;
            this.lblGia.Location = new System.Drawing.Point(35, 195);

            this.lblTon.AutoSize = true;
            this.lblTon.Location = new System.Drawing.Point(35, 235);

            this.lblSL.AutoSize = true;
            this.lblSL.Location = new System.Drawing.Point(35, 275);
            this.lblSL.Text = "Số lượng";

            this.numSoLuong.Location = new System.Drawing.Point(110, 272);
            this.numSoLuong.Minimum = 1;
            this.numSoLuong.Maximum = 100;
            this.numSoLuong.Value = 1;

            this.lblMoTa.AutoSize = true;
            this.lblMoTa.Location = new System.Drawing.Point(35, 320);
            this.lblMoTa.Text = "Mô tả";

            this.txtMoTa.Location = new System.Drawing.Point(110, 317);
            this.txtMoTa.Multiline = true;
            this.txtMoTa.ReadOnly = true;
            this.txtMoTa.Size = new System.Drawing.Size(430, 60);

            this.lblTS.AutoSize = true;
            this.lblTS.Location = new System.Drawing.Point(35, 395);
            this.lblTS.Text = "Thông số";

            this.txtThongSo.Location = new System.Drawing.Point(110, 392);
            this.txtThongSo.Multiline = true;
            this.txtThongSo.ReadOnly = true;
            this.txtThongSo.Size = new System.Drawing.Size(430, 60);

            this.btnThem.BackColor = System.Drawing.Color.Gold;
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.Location = new System.Drawing.Point(110, 475);
            this.btnThem.Size = new System.Drawing.Size(200, 42);
            this.btnThem.Text = "Thêm vào giỏ hàng";
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);

            this.btnDong.Location = new System.Drawing.Point(325, 475);
            this.btnDong.Size = new System.Drawing.Size(215, 42);
            this.btnDong.Text = "Đóng";
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);

            this.ClientSize = new System.Drawing.Size(600, 550);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblTitle,lblMa,lblTen,lblHang,lblGia,lblTon,lblSL,numSoLuong,
                lblMoTa,txtMoTa,lblTS,txtThongSo,btnThem,btnDong
            });

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chi tiết sản phẩm";

            ((System.ComponentModel.ISupportInitialize)(this.numSoLuong)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}