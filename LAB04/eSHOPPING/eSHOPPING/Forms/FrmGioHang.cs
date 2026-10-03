using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using eSHOPPING.Models;
using eSHOPPING.Services;

namespace eSHOPPING.Forms
{
    public partial class FrmGioHang : Form
    {
        readonly CartService service = new CartService();
        readonly int customerId;
        List<CartItem> items = new List<CartItem>();

        public FrmGioHang(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
        }

        private void FrmGioHang_Load(object sender, EventArgs e)
        {
            LoadCart();
        }

        private void LoadCart()
        {
            try
            {
                items = service.GetCart(customerId);
                dgvGioHang.DataSource = null;
                dgvGioHang.DataSource = items;

                if (dgvGioHang.Columns.Contains("CartItemId"))
                    dgvGioHang.Columns["CartItemId"].Visible = false;

                if (dgvGioHang.Columns.Contains("CartId"))
                    dgvGioHang.Columns["CartId"].Visible = false;

                if (dgvGioHang.Columns.Contains("ProductId"))
                    dgvGioHang.Columns["ProductId"].Visible = false;

                dgvGioHang.Columns["ProductName"].HeaderText = "Sản phẩm";
                dgvGioHang.Columns["Quantity"].HeaderText = "Số lượng";
                dgvGioHang.Columns["UnitPrice"].HeaderText = "Đơn giá";
                dgvGioHang.Columns["Amount"].HeaderText = "Thành tiền";

                dgvGioHang.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                dgvGioHang.Columns["UnitPrice"].DefaultCellStyle.Format = "N0";
                dgvGioHang.Columns["Amount"].DefaultCellStyle.Format = "N0";

                lblTong.Text = "Tổng tiền: " +
                    items.Sum(x => x.Amount).ToString("N0") + " VNĐ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow == null) return;

            int cartItemId = Convert.ToInt32(
                dgvGioHang.CurrentRow.Cells["CartItemId"].Value);

            int quantity = Convert.ToInt32(numSoLuong.Value);

            try
            {
                service.UpdateItem(cartItemId, quantity);
                LoadCart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow == null) return;

            int cartItemId = Convert.ToInt32(
                dgvGioHang.CurrentRow.Cells["CartItemId"].Value);

            if (MessageBox.Show("Xóa sản phẩm khỏi giỏ hàng?",
                "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                service.DeleteItem(cartItemId);
                LoadCart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvGioHang_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow == null) return;

            if (dgvGioHang.CurrentRow.Cells["Quantity"].Value != null)
                numSoLuong.Value =
                    Convert.ToDecimal(dgvGioHang.CurrentRow.Cells["Quantity"].Value);
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (items.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống.");
                return;
            }

            FrmThanhToan f = new FrmThanhToan(customerId);
            if (f.ShowDialog() == DialogResult.OK)
                LoadCart();
        }
    }
}