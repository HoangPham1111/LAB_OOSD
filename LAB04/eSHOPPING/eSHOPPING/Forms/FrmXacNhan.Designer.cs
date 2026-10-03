namespace eSHOPPING.Forms
{
    partial class FrmXacNhan
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblIcon;
        private System.Windows.Forms.Label lblMaDon;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.Label lblTamTinh;
        private System.Windows.Forms.Label lblPhi;
        private System.Windows.Forms.Label lblTong;
        private System.Windows.Forms.Label lblThongBao;
        private System.Windows.Forms.Button btnDong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblIcon = new System.Windows.Forms.Label();
            this.lblMaDon = new System.Windows.Forms.Label();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.lblTamTinh = new System.Windows.Forms.Label();
            this.lblPhi = new System.Windows.Forms.Label();
            this.lblTong = new System.Windows.Forms.Label();
            this.lblThongBao = new System.Windows.Forms.Label();
            this.btnDong = new System.Windows.Forms.Button();

            this.SuspendLayout();

            this.lblIcon.AutoSize = true;
            this.lblIcon.Font = new System.Drawing.Font(
                "Segoe UI", 35F,
                System.Drawing.FontStyle.Bold);
            this.lblIcon.ForeColor = System.Drawing.Color.Green;
            this.lblIcon.Location = new System.Drawing.Point(175, 20);
            this.lblIcon.Text = "✓";

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font(
                "Segoe UI", 22F,
                System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(220, 35);
            this.lblTitle.Text = "ĐẶT HÀNG THÀNH CÔNG";

            this.lblThongBao.AutoSize = true;
            this.lblThongBao.Font = new System.Drawing.Font(
                "Segoe UI", 11F);
            this.lblThongBao.Location = new System.Drawing.Point(100, 100);
            this.lblThongBao.Text =
                "Cảm ơn bạn đã mua hàng tại e-SHOPPING.";

            this.lblMaDon.AutoSize = true;
            this.lblMaDon.Font = new System.Drawing.Font(
                "Segoe UI", 12F,
                System.Drawing.FontStyle.Bold);
            this.lblMaDon.Location = new System.Drawing.Point(100, 145);

            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.ForeColor = System.Drawing.Color.Green;
            this.lblTrangThai.Location = new System.Drawing.Point(100, 180);

            this.lblTamTinh.AutoSize = true;
            this.lblTamTinh.Location = new System.Drawing.Point(100, 225);

            this.lblPhi.AutoSize = true;
            this.lblPhi.Location = new System.Drawing.Point(100, 260);

            this.lblTong.AutoSize = true;
            this.lblTong.Font = new System.Drawing.Font(
                "Segoe UI", 15F,
                System.Drawing.FontStyle.Bold);
            this.lblTong.ForeColor = System.Drawing.Color.Red;
            this.lblTong.Location = new System.Drawing.Point(100, 300);

            this.btnDong.BackColor = System.Drawing.Color.Gold;
            this.btnDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDong.Location = new System.Drawing.Point(170, 360);
            this.btnDong.Size = new System.Drawing.Size(250, 45);
            this.btnDong.Text = "Hoàn tất";
            this.btnDong.Click +=
                new System.EventHandler(this.btnDong_Click);

            this.ClientSize = new System.Drawing.Size(590, 440);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblIcon,
                lblTitle,
                lblThongBao,
                lblMaDon,
                lblTrangThai,
                lblTamTinh,
                lblPhi,
                lblTong,
                btnDong
            });

            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Xác nhận đơn hàng";
            this.Load +=
                new System.EventHandler(this.FrmXacNhan_Load);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}