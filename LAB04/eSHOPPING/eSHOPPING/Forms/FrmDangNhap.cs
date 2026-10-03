using System;
using System.Windows.Forms;
using eSHOPPING.Services;

namespace eSHOPPING.Forms
{
    public partial class FrmDangNhap : Form
    {
        readonly CustomerService service = new CustomerService();

        public int CustomerId { get; private set; }
        public string CustomerName { get; private set; }

        public FrmDangNhap()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (username == "" || password == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu.");
                return;
            }

            var customer = service.Login(username, password);

            if (customer == null)
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu.",
                    "Đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CustomerId = customer.CustomerId;
            CustomerName = customer.FullName;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            FrmDangKy f = new FrmDangKy();
            f.ShowDialog();
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkHienMatKhau.Checked;
        }
    }
}