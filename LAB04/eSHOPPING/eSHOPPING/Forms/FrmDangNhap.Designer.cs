namespace eSHOPPING.Forms
{
    partial class FrmDangNhap
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnl;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkHienMatKhau;
        private System.Windows.Forms.Button btnDangNhap;
        private System.Windows.Forms.Button btnDangKy;
        private System.Windows.Forms.Button btnThoat;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnl = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkHienMatKhau = new System.Windows.Forms.CheckBox();
            this.btnDangNhap = new System.Windows.Forms.Button();
            this.btnDangKy = new System.Windows.Forms.Button();
            this.btnThoat = new System.Windows.Forms.Button();

            this.pnl.SuspendLayout();
            this.SuspendLayout();

            this.pnl.BackColor = System.Drawing.Color.White;
            this.pnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl.Location = new System.Drawing.Point(45, 35);
            this.pnl.Size = new System.Drawing.Size(420, 380);

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.lblTitle.Location = new System.Drawing.Point(125, 25);
            this.lblTitle.Text = "ĐĂNG NHẬP";

            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(45, 95);
            this.lblUsername.Text = "Tên đăng nhập";

            this.txtUsername.Location = new System.Drawing.Point(45, 120);
            this.txtUsername.Size = new System.Drawing.Size(325, 30);

            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(45, 165);
            this.lblPassword.Text = "Mật khẩu";

            this.txtPassword.Location = new System.Drawing.Point(45, 190);
            this.txtPassword.Size = new System.Drawing.Size(325, 30);
            this.txtPassword.UseSystemPasswordChar = true;

            this.chkHienMatKhau.AutoSize = true;
            this.chkHienMatKhau.Location = new System.Drawing.Point(45, 230);
            this.chkHienMatKhau.Text = "Hiện mật khẩu";
            this.chkHienMatKhau.CheckedChanged += new System.EventHandler(this.chkHienMatKhau_CheckedChanged);

            this.btnDangNhap.BackColor = System.Drawing.Color.Gold;
            this.btnDangNhap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDangNhap.Location = new System.Drawing.Point(45, 270);
            this.btnDangNhap.Size = new System.Drawing.Size(155, 40);
            this.btnDangNhap.Text = "Đăng nhập";
            this.btnDangNhap.Click += new System.EventHandler(this.btnDangNhap_Click);

            this.btnDangKy.Location = new System.Drawing.Point(215, 270);
            this.btnDangKy.Size = new System.Drawing.Size(155, 40);
            this.btnDangKy.Text = "Đăng ký";
            this.btnDangKy.Click += new System.EventHandler(this.btnDangKy_Click);

            this.btnThoat.Location = new System.Drawing.Point(45, 320);
            this.btnThoat.Size = new System.Drawing.Size(325, 35);
            this.btnThoat.Text = "Thoát";
            this.btnThoat.Click += (s, e) => Close();

            this.pnl.Controls.Add(this.lblTitle);
            this.pnl.Controls.Add(this.lblUsername);
            this.pnl.Controls.Add(this.txtUsername);
            this.pnl.Controls.Add(this.lblPassword);
            this.pnl.Controls.Add(this.txtPassword);
            this.pnl.Controls.Add(this.chkHienMatKhau);
            this.pnl.Controls.Add(this.btnDangNhap);
            this.pnl.Controls.Add(this.btnDangKy);
            this.pnl.Controls.Add(this.btnThoat);

            this.ClientSize = new System.Drawing.Size(510, 450);
            this.Controls.Add(this.pnl);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập";

            this.pnl.ResumeLayout(false);
            this.pnl.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}