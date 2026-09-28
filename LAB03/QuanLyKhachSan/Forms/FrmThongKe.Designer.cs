namespace QuanLyKhachSan.Forms
{
    partial class FrmThongKe
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTu, lblDen;
        private System.Windows.Forms.DateTimePicker dtTu, dtDen;
        private System.Windows.Forms.DataGridView dgvTongHop, dgvDV;
        private System.Windows.Forms.Button btnTK, btnDong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblTu = new System.Windows.Forms.Label();
            this.lblDen = new System.Windows.Forms.Label();
            this.dtTu = new System.Windows.Forms.DateTimePicker();
            this.dtDen = new System.Windows.Forms.DateTimePicker();
            this.dgvTongHop = new System.Windows.Forms.DataGridView();
            this.dgvDV = new System.Windows.Forms.DataGridView();
            this.btnTK = new System.Windows.Forms.Button();
            this.btnDong = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvTongHop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDV)).BeginInit();

            this.lblTu.Text = "Từ ngày";
            this.lblTu.Location = new System.Drawing.Point(20, 25);
            this.lblTu.AutoSize = true;

            this.dtTu.Location = new System.Drawing.Point(85, 20);
            this.dtTu.Width = 140;

            this.lblDen.Text = "Đến ngày";
            this.lblDen.Location = new System.Drawing.Point(250, 25);
            this.lblDen.AutoSize = true;

            this.dtDen.Location = new System.Drawing.Point(320, 20);
            this.dtDen.Width = 140;

            this.btnTK.Location = new System.Drawing.Point(480, 18);
            this.btnTK.Size = new System.Drawing.Size(110, 30);
            this.btnTK.Text = "Thống kê";
            this.btnTK.Click += new System.EventHandler(this.btnTK_Click);

            this.dgvTongHop.Location = new System.Drawing.Point(20, 75);
            this.dgvTongHop.Size = new System.Drawing.Size(760, 180);
            this.dgvTongHop.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.dgvDV.Location = new System.Drawing.Point(20, 280);
            this.dgvDV.Size = new System.Drawing.Size(760, 220);
            this.dgvDV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.btnDong.Location = new System.Drawing.Point(680, 520);
            this.btnDong.Size = new System.Drawing.Size(100, 30);
            this.btnDong.Text = "Đóng";
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);

            this.ClientSize = new System.Drawing.Size(810, 570);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thống kê";

            this.Controls.Add(this.lblTu);
            this.Controls.Add(this.lblDen);
            this.Controls.Add(this.dtTu);
            this.Controls.Add(this.dtDen);
            this.Controls.Add(this.btnTK);
            this.Controls.Add(this.dgvTongHop);
            this.Controls.Add(this.dgvDV);
            this.Controls.Add(this.btnDong);

            ((System.ComponentModel.ISupportInitialize)(this.dgvTongHop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}