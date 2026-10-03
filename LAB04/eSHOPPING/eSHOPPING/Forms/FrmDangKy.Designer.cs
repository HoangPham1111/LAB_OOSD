namespace eSHOPPING.Forms
{
    partial class FrmDangKy
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblHoTen;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblConfirm;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblDiaChi;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtConfirm;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.CheckBox chkHienMatKhau;
        private System.Windows.Forms.Button btnDangKy;
        private System.Windows.Forms.Button btnHuy;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblHoTen = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblConfirm = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblDiaChi = new System.Windows.Forms.Label();
            this.txtHoTen = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtConfirm = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtDiaChi = new System.Windows.Forms.TextBox();
            this.chkHienMatKhau = new System.Windows.Forms.CheckBox();
            this.btnDangKy = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();

            this.SuspendLayout();

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(145, 20);
            this.lblTitle.Text = "ĐĂNG KÝ";

            this.lblHoTen.AutoSize = true;
            this.lblHoTen.Location = new System.Drawing.Point(40, 80);
            this.lblHoTen.Text = "Họ và tên";

            this.txtHoTen.Location = new System.Drawing.Point(160, 77);
            this.txtHoTen.Size = new System.Drawing.Size(330, 28);

            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(40, 120);
            this.lblPhone.Text = "Điện thoại";

            this.txtPhone.Location = new System.Drawing.Point(160, 117);
            this.txtPhone.Size = new System.Drawing.Size(330, 28);

            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(40, 160);
            this.lblUsername.Text = "Tên đăng nhập";

            this.txtUsername.Location = new System.Drawing.Point(160, 157);
            this.txtUsername.Size = new System.Drawing.Size(330, 28);

            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(40, 200);
            this.lblPassword.Text = "Mật khẩu";

            this.txtPassword.Location = new System.Drawing.Point(160, 197);
            this.txtPassword.Size = new System.Drawing.Size(330, 28);
            this.txtPassword.UseSystemPasswordChar = true;

            this.lblConfirm.AutoSize = true;
            this.lblConfirm.Location = new System.Drawing.Point(40, 240);
            this.lblConfirm.Text = "Xác nhận";

            this.txtConfirm.Location = new System.Drawing.Point(160, 237);
            this.txtConfirm.Size = new System.Drawing.Size(330, 28);
            this.txtConfirm.UseSystemPasswordChar = true;

            this.chkHienMatKhau.AutoSize = true;
            this.chkHienMatKhau.Location = new System.Drawing.Point(160, 275);
            this.chkHienMatKhau.Text = "Hiện mật khẩu";
            this.chkHienMatKhau.CheckedChanged += new System.EventHandler(this.chkHienMatKhau_CheckedChanged);

            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(40, 315);
            this.lblEmail.Text = "Email";

            this.txtEmail.Location = new System.Drawing.Point(160, 312);
            this.txtEmail.Size = new System.Drawing.Size(330, 28);

            this.lblDiaChi.AutoSize = true;
            this.lblDiaChi.Location = new System.Drawing.Point(40, 355);
            this.lblDiaChi.Text = "Địa chỉ";

            this.txtDiaChi.Location = new System.Drawing.Point(160, 352);
            this.txtDiaChi.Size = new System.Drawing.Size(330, 28);

            this.btnDangKy.BackColor = System.Drawing.Color.Gold;
            this.btnDangKy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDangKy.Location = new System.Drawing.Point(160, 400);
            this.btnDangKy.Size = new System.Drawing.Size(155, 40);
            this.btnDangKy.Text = "Đăng ký";
            this.btnDangKy.Click += new System.EventHandler(this.btnDangKy_Click);

            this.btnHuy.Location = new System.Drawing.Point(335, 400);
            this.btnHuy.Size = new System.Drawing.Size(155, 40);
            this.btnHuy.Text = "Hủy";
            this.btnHuy.Click += (s, e) => Close();

            this.ClientSize = new System.Drawing.Size(550, 470);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblTitle,lblHoTen,txtHoTen,lblPhone,txtPhone,lblUsername,txtUsername,
                lblPassword,txtPassword,lblConfirm,txtConfirm,chkHienMatKhau,
                lblEmail,txtEmail,lblDiaChi,txtDiaChi,btnDangKy,btnHuy
            });

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng ký tài khoản";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}