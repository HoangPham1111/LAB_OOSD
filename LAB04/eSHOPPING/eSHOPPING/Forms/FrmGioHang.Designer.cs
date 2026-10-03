namespace eSHOPPING.Forms
{
    partial class FrmGioHang
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvGioHang;
        private System.Windows.Forms.Label lblSoLuong;
        private System.Windows.Forms.NumericUpDown numSoLuong;
        private System.Windows.Forms.Button btnCapNhat;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Label lblTong;
        private System.Windows.Forms.Button btnThanhToan;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvGioHang = new System.Windows.Forms.DataGridView();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.numSoLuong = new System.Windows.Forms.NumericUpDown();
            this.btnCapNhat = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.lblTong = new System.Windows.Forms.Label();
            this.btnThanhToan = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvGioHang)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSoLuong)).BeginInit();
            this.SuspendLayout();

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(25, 20);
            this.lblTitle.Text = "GIỎ HÀNG";

            this.dgvGioHang.AllowUserToAddRows = false;
            this.dgvGioHang.AllowUserToDeleteRows = false;
            this.dgvGioHang.ReadOnly = true;
            this.dgvGioHang.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGioHang.Location = new System.Drawing.Point(30, 75);
            this.dgvGioHang.Size = new System.Drawing.Size(760, 300);
            this.dgvGioHang.SelectionChanged += new System.EventHandler(this.dgvGioHang_SelectionChanged);

            this.lblSoLuong.AutoSize = true;
            this.lblSoLuong.Location = new System.Drawing.Point(30, 400);
            this.lblSoLuong.Text = "Số lượng:";

            this.numSoLuong.Location = new System.Drawing.Point(95, 397);
            this.numSoLuong.Minimum = 1;
            this.numSoLuong.Maximum = 999;
            this.numSoLuong.Value = 1;

            this.btnCapNhat.Location = new System.Drawing.Point(180, 394);
            this.btnCapNhat.Size = new System.Drawing.Size(110, 32);
            this.btnCapNhat.Text = "Cập nhật";
            this.btnCapNhat.Click += new System.EventHandler(this.btnCapNhat_Click);

            this.btnXoa.Location = new System.Drawing.Point(300, 394);
            this.btnXoa.Size = new System.Drawing.Size(100, 32);
            this.btnXoa.Text = "Xóa";
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);

            this.lblTong.AutoSize = true;
            this.lblTong.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTong.ForeColor = System.Drawing.Color.Red;
            this.lblTong.Location = new System.Drawing.Point(30, 450);
            this.lblTong.Text = "Tổng tiền: 0 VNĐ";

            this.btnThanhToan.BackColor = System.Drawing.Color.Gold;
            this.btnThanhToan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThanhToan.Location = new System.Drawing.Point(610, 440);
            this.btnThanhToan.Size = new System.Drawing.Size(180, 45);
            this.btnThanhToan.Text = "THANH TOÁN";
            this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click);

            this.ClientSize = new System.Drawing.Size(830, 520);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblTitle,dgvGioHang,lblSoLuong,numSoLuong,btnCapNhat,
                btnXoa,lblTong,btnThanhToan
            });

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Giỏ hàng";

            ((System.ComponentModel.ISupportInitialize)(this.dgvGioHang)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSoLuong)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}