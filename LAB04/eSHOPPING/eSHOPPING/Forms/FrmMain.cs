using System;
using System.Windows.Forms;

namespace eSHOPPING.Forms
{
    public partial class FrmMain : Form
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            lblXinChao.Text = string.IsNullOrWhiteSpace(CustomerName)
                ? "Xin chào khách hàng"
                : "Xin chào, " + CustomerName;
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            FrmSanPham f = new FrmSanPham(CustomerId);
            f.ShowDialog();
        }

        private void btnGioHang_Click(object sender, EventArgs e)
        {
            if (CustomerId <= 0)
            {
                MessageBox.Show(
                    "Vui lòng đăng nhập trước.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            FrmGioHang f = new FrmGioHang(CustomerId);
            f.ShowDialog();
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            FrmDangNhap f = new FrmDangNhap();

            if (f.ShowDialog() == DialogResult.OK)
            {
                CustomerId = f.CustomerId;
                CustomerName = f.CustomerName;

                lblXinChao.Text = "Xin chào, " + CustomerName;
                btnDangNhap.Visible = false;
                btnDangXuat.Visible = true;
            }
        }
    }
}