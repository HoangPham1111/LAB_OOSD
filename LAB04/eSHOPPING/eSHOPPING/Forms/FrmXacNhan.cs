using System;
using System.Windows.Forms;

namespace eSHOPPING.Forms
{
    public partial class FrmXacNhan : Form
    {
        readonly string orderCode;
        readonly decimal subtotal;
        readonly decimal shippingFee;
        readonly decimal total;

        public FrmXacNhan(
            string orderCode,
            decimal subtotal,
            decimal shippingFee,
            decimal total)
        {
            InitializeComponent();

            this.orderCode = orderCode;
            this.subtotal = subtotal;
            this.shippingFee = shippingFee;
            this.total = total;
        }

        private void FrmXacNhan_Load(object sender, EventArgs e)
        {
            lblMaDon.Text = "Mã đơn hàng: " + orderCode;
            lblTrangThai.Text = "Trạng thái: Đã thanh toán";
            lblTamTinh.Text = "Tạm tính: " +
                subtotal.ToString("N0") + " VNĐ";
            lblPhi.Text = "Phí giao hàng: " +
                shippingFee.ToString("N0") + " VNĐ";
            lblTong.Text = "Tổng tiền: " +
                total.ToString("N0") + " VNĐ";
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}