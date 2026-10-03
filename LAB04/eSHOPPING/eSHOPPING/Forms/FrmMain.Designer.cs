namespace eSHOPPING.Forms
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblXinChao;
        private System.Windows.Forms.Button btnDangNhap;
        private System.Windows.Forms.Button btnDangXuat;
        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.Button btnSanPham;
        private System.Windows.Forms.Button btnGioHang;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblXinChao = new System.Windows.Forms.Label();
            this.btnDangNhap = new System.Windows.Forms.Button();
            this.btnDangXuat = new System.Windows.Forms.Button();
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.btnSanPham = new System.Windows.Forms.Button();
            this.btnGioHang = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubTitle = new System.Windows.Forms.Label();

            this.pnlTop.SuspendLayout();
            this.pnlMenu.SuspendLayout();
            this.SuspendLayout();

            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(25, 25, 25);
            this.pnlTop.Controls.Add(this.lblLogo);
            this.pnlTop.Controls.Add(this.lblXinChao);
            this.pnlTop.Controls.Add(this.btnDangNhap);
            this.pnlTop.Controls.Add(this.btnDangXuat);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 70;

            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.Gold;
            this.lblLogo.Location = new System.Drawing.Point(25, 17);
            this.lblLogo.Text = "e-SHOPPING";

            this.lblXinChao.AutoSize = true;
            this.lblXinChao.ForeColor = System.Drawing.Color.White;
            this.lblXinChao.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblXinChao.Location = new System.Drawing.Point(230, 25);
            this.lblXinChao.Text = "Xin chào khách hàng";

            this.btnDangNhap.BackColor = System.Drawing.Color.Gold;
            this.btnDangNhap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDangNhap.Location = new System.Drawing.Point(720, 18);
            this.btnDangNhap.Size = new System.Drawing.Size(110, 35);
            this.btnDangNhap.Text = "Đăng nhập";
            this.btnDangNhap.Click += new System.EventHandler(this.btnDangNhap_Click);

            this.btnDangXuat.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.btnDangXuat.ForeColor = System.Drawing.Color.White;
            this.btnDangXuat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDangXuat.Location = new System.Drawing.Point(720, 18);
            this.btnDangXuat.Size = new System.Drawing.Size(110, 35);
            this.btnDangXuat.Text = "Đăng xuất";
            this.btnDangXuat.Visible = false;
            this.btnDangXuat.Click += new System.EventHandler(this.btnDangXuat_Click);

            this.pnlMenu.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            this.pnlMenu.Controls.Add(this.btnSanPham);
            this.pnlMenu.Controls.Add(this.btnGioHang);
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMenu.Width = 190;

            this.btnSanPham.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            this.btnSanPham.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSanPham.ForeColor = System.Drawing.Color.White;
            this.btnSanPham.Location = new System.Drawing.Point(15, 30);
            this.btnSanPham.Size = new System.Drawing.Size(160, 45);
            this.btnSanPham.Text = "Sản phẩm";
            this.btnSanPham.Click += new System.EventHandler(this.btnSanPham_Click);

            this.btnGioHang.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            this.btnGioHang.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGioHang.ForeColor = System.Drawing.Color.White;
            this.btnGioHang.Location = new System.Drawing.Point(15, 90);
            this.btnGioHang.Size = new System.Drawing.Size(160, 45);
            this.btnGioHang.Text = "Giỏ hàng";
            this.btnGioHang.Click += new System.EventHandler(this.btnGioHang_Click);

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(250, 160);
            this.lblTitle.Text = "Chào mừng đến e-SHOPPING";

            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblSubTitle.Location = new System.Drawing.Point(255, 215);
            this.lblSubTitle.Text = "Hệ thống mua sắm trực tuyến";

            this.ClientSize = new System.Drawing.Size(850, 500);
            this.Controls.Add(this.lblSubTitle);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlMenu);
            this.Controls.Add(this.pnlTop);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "e-SHOPPING";
            this.Load += new System.EventHandler(this.FrmMain_Load);

            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}