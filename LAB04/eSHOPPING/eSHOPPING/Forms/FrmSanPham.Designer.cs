namespace eSHOPPING.Forms
{
    partial class FrmSanPham
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Button btnTim;
        private System.Windows.Forms.Button btnChiTiet;
        private System.Windows.Forms.DataGridView dgvSanPham;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.btnTim = new System.Windows.Forms.Button();
            this.btnChiTiet = new System.Windows.Forms.Button();
            this.dgvSanPham = new System.Windows.Forms.DataGridView();

            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPham)).BeginInit();
            this.SuspendLayout();

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(25, 20);
            this.lblTitle.Text = "DANH SÁCH SẢN PHẨM";

            this.txtTimKiem.Location = new System.Drawing.Point(30, 75);
            this.txtTimKiem.Size = new System.Drawing.Size(400, 30);

            this.btnTim.BackColor = System.Drawing.Color.Gold;
            this.btnTim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTim.Location = new System.Drawing.Point(445, 74);
            this.btnTim.Size = new System.Drawing.Size(100, 32);
            this.btnTim.Text = "Tìm kiếm";
            this.btnTim.Click += new System.EventHandler(this.btnTim_Click);

            this.btnChiTiet.Location = new System.Drawing.Point(650, 74);
            this.btnChiTiet.Size = new System.Drawing.Size(130, 32);
            this.btnChiTiet.Text = "Xem chi tiết";
            this.btnChiTiet.Click += new System.EventHandler(this.btnChiTiet_Click);

            this.dgvSanPham.AllowUserToAddRows = false;
            this.dgvSanPham.AllowUserToDeleteRows = false;
            this.dgvSanPham.ReadOnly = true;
            this.dgvSanPham.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSanPham.Location = new System.Drawing.Point(30, 125);
            this.dgvSanPham.Size = new System.Drawing.Size(750, 360);
            this.dgvSanPham.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSanPham_CellDoubleClick);

            this.ClientSize = new System.Drawing.Size(820, 520);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtTimKiem);
            this.Controls.Add(this.btnTim);
            this.Controls.Add(this.btnChiTiet);
            this.Controls.Add(this.dgvSanPham);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sản phẩm";
            this.Load += new System.EventHandler(this.FrmSanPham_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPham)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}