namespace eSHOPPING.Forms
{
    partial class FrmThanhToan
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpNguoiNhan;
        private System.Windows.Forms.Label lblNguoiNhan;
        private System.Windows.Forms.Label lblDiaChi;
        private System.Windows.Forms.Label lblDienThoai;
        private System.Windows.Forms.TextBox txtNguoiNhan;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.TextBox txtDienThoai;

        private System.Windows.Forms.GroupBox grpGiaoHang;
        private System.Windows.Forms.Label lblLoaiGiaoHang;
        private System.Windows.Forms.Label lblKhuVuc;
        private System.Windows.Forms.ComboBox cboLoaiGiaoHang;
        private System.Windows.Forms.ComboBox cboKhuVuc;
        private System.Windows.Forms.Button btnTinhPhi;

        private System.Windows.Forms.GroupBox grpThanhToan;
        private System.Windows.Forms.Label lblLoaiThe;
        private System.Windows.Forms.Label lblSoThe;
        private System.Windows.Forms.Label lblCSV;
        private System.Windows.Forms.Label lblChuThe;
        private System.Windows.Forms.Label lblThang;
        private System.Windows.Forms.Label lblNam;
        private System.Windows.Forms.ComboBox cboLoaiThe;
        private System.Windows.Forms.TextBox txtSoThe;
        private System.Windows.Forms.TextBox txtCSV;
        private System.Windows.Forms.TextBox txtChuThe;
        private System.Windows.Forms.TextBox txtThangHetHan;
        private System.Windows.Forms.TextBox txtNamHetHan;
        private System.Windows.Forms.Label lblHuongDanThe;

        private System.Windows.Forms.Label lblTamTinh;
        private System.Windows.Forms.Label lblPhi;
        private System.Windows.Forms.Label lblTong;
        private System.Windows.Forms.Button btnThanhToan;
        private System.Windows.Forms.Button btnQuayLai;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpNguoiNhan = new System.Windows.Forms.GroupBox();
            this.lblNguoiNhan = new System.Windows.Forms.Label();
            this.lblDiaChi = new System.Windows.Forms.Label();
            this.lblDienThoai = new System.Windows.Forms.Label();
            this.txtNguoiNhan = new System.Windows.Forms.TextBox();
            this.txtDiaChi = new System.Windows.Forms.TextBox();
            this.txtDienThoai = new System.Windows.Forms.TextBox();

            this.grpGiaoHang = new System.Windows.Forms.GroupBox();
            this.lblLoaiGiaoHang = new System.Windows.Forms.Label();
            this.lblKhuVuc = new System.Windows.Forms.Label();
            this.cboLoaiGiaoHang = new System.Windows.Forms.ComboBox();
            this.cboKhuVuc = new System.Windows.Forms.ComboBox();
            this.btnTinhPhi = new System.Windows.Forms.Button();

            this.grpThanhToan = new System.Windows.Forms.GroupBox();
            this.lblLoaiThe = new System.Windows.Forms.Label();
            this.lblSoThe = new System.Windows.Forms.Label();
            this.lblCSV = new System.Windows.Forms.Label();
            this.lblChuThe = new System.Windows.Forms.Label();
            this.lblThang = new System.Windows.Forms.Label();
            this.lblNam = new System.Windows.Forms.Label();
            this.cboLoaiThe = new System.Windows.Forms.ComboBox();
            this.txtSoThe = new System.Windows.Forms.TextBox();
            this.txtCSV = new System.Windows.Forms.TextBox();
            this.txtChuThe = new System.Windows.Forms.TextBox();
            this.txtThangHetHan = new System.Windows.Forms.TextBox();
            this.txtNamHetHan = new System.Windows.Forms.TextBox();
            this.lblHuongDanThe = new System.Windows.Forms.Label();

            this.lblTamTinh = new System.Windows.Forms.Label();
            this.lblPhi = new System.Windows.Forms.Label();
            this.lblTong = new System.Windows.Forms.Label();
            this.btnThanhToan = new System.Windows.Forms.Button();
            this.btnQuayLai = new System.Windows.Forms.Button();

            this.SuspendLayout();

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font(
                "Segoe UI", 22F,
                System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(25, 20);
            this.lblTitle.Text = "THANH TOÁN ĐƠN HÀNG";

            this.grpNguoiNhan.Text = "1. Thông tin người nhận";
            this.grpNguoiNhan.Location = new System.Drawing.Point(25, 70);
            this.grpNguoiNhan.Size = new System.Drawing.Size(390, 190);

            this.lblNguoiNhan.AutoSize = true;
            this.lblNguoiNhan.Location = new System.Drawing.Point(20, 35);
            this.lblNguoiNhan.Text = "Họ tên:";

            this.txtNguoiNhan.Location = new System.Drawing.Point(110, 32);
            this.txtNguoiNhan.Size = new System.Drawing.Size(250, 27);

            this.lblDiaChi.AutoSize = true;
            this.lblDiaChi.Location = new System.Drawing.Point(20, 75);
            this.lblDiaChi.Text = "Địa chỉ:";

            this.txtDiaChi.Location = new System.Drawing.Point(110, 72);
            this.txtDiaChi.Size = new System.Drawing.Size(250, 27);

            this.lblDienThoai.AutoSize = true;
            this.lblDienThoai.Location = new System.Drawing.Point(20, 115);
            this.lblDienThoai.Text = "Điện thoại:";

            this.txtDienThoai.Location = new System.Drawing.Point(110, 112);
            this.txtDienThoai.Size = new System.Drawing.Size(250, 27);

            this.grpNguoiNhan.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblNguoiNhan,txtNguoiNhan,
                lblDiaChi,txtDiaChi,
                lblDienThoai,txtDienThoai
            });

            this.grpGiaoHang.Text = "2. Hình thức giao hàng";
            this.grpGiaoHang.Location = new System.Drawing.Point(430, 70);
            this.grpGiaoHang.Size = new System.Drawing.Size(390, 190);

            this.lblLoaiGiaoHang.AutoSize = true;
            this.lblLoaiGiaoHang.Location = new System.Drawing.Point(20, 40);
            this.lblLoaiGiaoHang.Text = "Loại giao hàng:";

            this.cboLoaiGiaoHang.DropDownStyle =
                System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoaiGiaoHang.Location = new System.Drawing.Point(125, 37);
            this.cboLoaiGiaoHang.Size = new System.Drawing.Size(235, 28);

            this.lblKhuVuc.AutoSize = true;
            this.lblKhuVuc.Location = new System.Drawing.Point(20, 85);
            this.lblKhuVuc.Text = "Khu vực:";

            this.cboKhuVuc.DropDownStyle =
                System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboKhuVuc.Location = new System.Drawing.Point(125, 82);
            this.cboKhuVuc.Size = new System.Drawing.Size(235, 28);

            this.btnTinhPhi.BackColor = System.Drawing.Color.Gold;
            this.btnTinhPhi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTinhPhi.Location = new System.Drawing.Point(125, 125);
            this.btnTinhPhi.Size = new System.Drawing.Size(235, 35);
            this.btnTinhPhi.Text = "Tính phí giao hàng";
            this.btnTinhPhi.Click += new System.EventHandler(this.btnTinhPhi_Click);

            this.grpGiaoHang.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblLoaiGiaoHang,cboLoaiGiaoHang,
                lblKhuVuc,cboKhuVuc,btnTinhPhi
            });

            this.grpThanhToan.Text = "3. Thông tin thanh toán";
            this.grpThanhToan.Location = new System.Drawing.Point(25, 280);
            this.grpThanhToan.Size = new System.Drawing.Size(795, 190);

            this.lblLoaiThe.AutoSize = true;
            this.lblLoaiThe.Location = new System.Drawing.Point(20, 35);
            this.lblLoaiThe.Text = "Loại thẻ:";

            this.cboLoaiThe.DropDownStyle =
                System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoaiThe.Location = new System.Drawing.Point(110, 32);
            this.cboLoaiThe.Size = new System.Drawing.Size(210, 28);
            this.cboLoaiThe.SelectedIndexChanged +=
                new System.EventHandler(this.cboLoaiThe_SelectedIndexChanged);

            this.lblSoThe.AutoSize = true;
            this.lblSoThe.Location = new System.Drawing.Point(20, 75);
            this.lblSoThe.Text = "Số thẻ:";

            this.txtSoThe.Location = new System.Drawing.Point(110, 72);
            this.txtSoThe.Size = new System.Drawing.Size(210, 27);

            this.lblCSV.AutoSize = true;
            this.lblCSV.Location = new System.Drawing.Point(20, 115);
            this.lblCSV.Text = "CSV:";

            this.txtCSV.Location = new System.Drawing.Point(110, 112);
            this.txtCSV.Size = new System.Drawing.Size(100, 27);
            this.txtCSV.UseSystemPasswordChar = true;

            this.lblChuThe.AutoSize = true;
            this.lblChuThe.Location = new System.Drawing.Point(390, 35);
            this.lblChuThe.Text = "Chủ thẻ:";

            this.txtChuThe.Location = new System.Drawing.Point(475, 32);
            this.txtChuThe.Size = new System.Drawing.Size(280, 27);

            this.lblThang.AutoSize = true;
            this.lblThang.Location = new System.Drawing.Point(390, 75);
            this.lblThang.Text = "Tháng:";

            this.txtThangHetHan.Location = new System.Drawing.Point(475, 72);
            this.txtThangHetHan.Size = new System.Drawing.Size(80, 27);
            this.txtThangHetHan.MaxLength = 2;

            this.lblNam.AutoSize = true;
            this.lblNam.Location = new System.Drawing.Point(580, 75);
            this.lblNam.Text = "Năm:";

            this.txtNamHetHan.Location = new System.Drawing.Point(625, 72);
            this.txtNamHetHan.Size = new System.Drawing.Size(130, 27);
            this.txtNamHetHan.MaxLength = 4;

            this.lblHuongDanThe.AutoSize = true;
            this.lblHuongDanThe.ForeColor = System.Drawing.Color.Gray;
            this.lblHuongDanThe.Location = new System.Drawing.Point(390, 115);
            this.lblHuongDanThe.Text = "Thông tin thẻ";

            this.grpThanhToan.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblLoaiThe,cboLoaiThe,
                lblSoThe,txtSoThe,
                lblCSV,txtCSV,
                lblChuThe,txtChuThe,
                lblThang,txtThangHetHan,
                lblNam,txtNamHetHan,
                lblHuongDanThe
            });

            this.lblTamTinh.AutoSize = true;
            this.lblTamTinh.Location = new System.Drawing.Point(500, 490);
            this.lblTamTinh.Text = "Tạm tính: 0 VNĐ";

            this.lblPhi.AutoSize = true;
            this.lblPhi.Location = new System.Drawing.Point(500, 520);
            this.lblPhi.Text = "Phí giao hàng: 0 VNĐ";

            this.lblTong.AutoSize = true;
            this.lblTong.Font = new System.Drawing.Font(
                "Segoe UI", 15F,
                System.Drawing.FontStyle.Bold);
            this.lblTong.ForeColor = System.Drawing.Color.Red;
            this.lblTong.Location = new System.Drawing.Point(500, 550);
            this.lblTong.Text = "Tổng thanh toán: 0 VNĐ";

            this.btnThanhToan.BackColor = System.Drawing.Color.Gold;
            this.btnThanhToan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThanhToan.Location = new System.Drawing.Point(500, 590);
            this.btnThanhToan.Size = new System.Drawing.Size(180, 45);
            this.btnThanhToan.Text = "THANH TOÁN";
            this.btnThanhToan.Click +=
                new System.EventHandler(this.btnThanhToan_Click);

            this.btnQuayLai.Location = new System.Drawing.Point(695, 590);
            this.btnQuayLai.Size = new System.Drawing.Size(125, 45);
            this.btnQuayLai.Text = "Quay lại";
            this.btnQuayLai.Click +=
                new System.EventHandler(this.btnQuayLai_Click);

            this.ClientSize = new System.Drawing.Size(850, 660);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblTitle,
                grpNguoiNhan,
                grpGiaoHang,
                grpThanhToan,
                lblTamTinh,
                lblPhi,
                lblTong,
                btnThanhToan,
                btnQuayLai
            });

            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thanh toán";
            this.Load +=
                new System.EventHandler(this.FrmThanhToan_Load);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}