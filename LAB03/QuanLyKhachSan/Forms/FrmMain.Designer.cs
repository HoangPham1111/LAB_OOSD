namespace QuanLyKhachSan.Forms
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnDanhMuc;
        private System.Windows.Forms.Button btnPhong;
        private System.Windows.Forms.Button btnDatPhong;
        private System.Windows.Forms.Button btnDichVu;
        private System.Windows.Forms.Button btnTraPhong;
        private System.Windows.Forms.Button btnThongKe;
        private System.Windows.Forms.Button btnThoat;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnDanhMuc = new System.Windows.Forms.Button();
            this.btnPhong = new System.Windows.Forms.Button();
            this.btnDatPhong = new System.Windows.Forms.Button();
            this.btnDichVu = new System.Windows.Forms.Button();
            this.btnTraPhong = new System.Windows.Forms.Button();
            this.btnThongKe = new System.Windows.Forms.Button();
            this.btnThoat = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(120, 30);
            this.lblTitle.Text = "HỆ THỐNG QUẢN LÝ KHÁCH SẠN";

            this.btnDanhMuc.Location = new System.Drawing.Point(95, 90);
            this.btnDanhMuc.Size = new System.Drawing.Size(280, 45);
            this.btnDanhMuc.Text = "Danh mục";
            this.btnDanhMuc.Click += new System.EventHandler(this.btnDanhMuc_Click);

            this.btnPhong.Location = new System.Drawing.Point(95, 145);
            this.btnPhong.Size = new System.Drawing.Size(280, 45);
            this.btnPhong.Text = "Phòng - Tiện nghi";
            this.btnPhong.Click += new System.EventHandler(this.btnPhong_Click);

            this.btnDatPhong.Location = new System.Drawing.Point(95, 200);
            this.btnDatPhong.Size = new System.Drawing.Size(280, 45);
            this.btnDatPhong.Text = "Đặt / Nhận phòng";
            this.btnDatPhong.Click += new System.EventHandler(this.btnDatPhong_Click);

            this.btnDichVu.Location = new System.Drawing.Point(95, 255);
            this.btnDichVu.Size = new System.Drawing.Size(280, 45);
            this.btnDichVu.Text = "Sử dụng dịch vụ";
            this.btnDichVu.Click += new System.EventHandler(this.btnDichVu_Click);

            this.btnTraPhong.Location = new System.Drawing.Point(95, 310);
            this.btnTraPhong.Size = new System.Drawing.Size(280, 45);
            this.btnTraPhong.Text = "Trả phòng - Thanh toán";
            this.btnTraPhong.Click += new System.EventHandler(this.btnTraPhong_Click);

            this.btnThongKe.Location = new System.Drawing.Point(95, 365);
            this.btnThongKe.Size = new System.Drawing.Size(280, 45);
            this.btnThongKe.Text = "Thống kê";
            this.btnThongKe.Click += new System.EventHandler(this.btnThongKe_Click);

            this.btnThoat.Location = new System.Drawing.Point(95, 420);
            this.btnThoat.Size = new System.Drawing.Size(280, 45);
            this.btnThoat.Text = "Thoát";
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);

            this.ClientSize = new System.Drawing.Size(470, 510);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý khách sạn";
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnDanhMuc);
            this.Controls.Add(this.btnPhong);
            this.Controls.Add(this.btnDatPhong);
            this.Controls.Add(this.btnDichVu);
            this.Controls.Add(this.btnTraPhong);
            this.Controls.Add(this.btnThongKe);
            this.Controls.Add(this.btnThoat);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}