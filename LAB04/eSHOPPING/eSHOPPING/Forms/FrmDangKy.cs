using System;
using System.Windows.Forms;
using eSHOPPING.Models;
using eSHOPPING.Services;

namespace eSHOPPING.Forms
{
    public partial class FrmDangKy : Form
    {
        readonly CustomerService service = new CustomerService();

        public FrmDangKy()
        {
            InitializeComponent();
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            if (txtHoTen.Text.Trim() == "" ||
                txtPhone.Text.Trim() == "" ||
                txtUsername.Text.Trim() == "" ||
                txtPassword.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            if (txtPassword.Text != txtConfirm.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp.");
                return;
            }

            Customer c = new Customer
            {
                FullName = txtHoTen.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Username = txtUsername.Text.Trim(),
                PasswordHash = txtPassword.Text,
                Email = txtEmail.Text.Trim(),
                Address = txtDiaChi.Text.Trim()
            };

            try
            {
                if (service.Register(c))
                {
                    MessageBox.Show("Đăng ký thành công.");
                    Close();
                }
                else
                    MessageBox.Show("Đăng ký thất bại.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            bool show = chkHienMatKhau.Checked;
            txtPassword.UseSystemPasswordChar = !show;
            txtConfirm.UseSystemPasswordChar = !show;
        }
    }
}