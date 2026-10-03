using System;
using System.Windows.Forms;
using eSHOPPING.Models;
using eSHOPPING.Services;

namespace eSHOPPING.Forms
{
    public partial class FrmChiTietSanPham : Form
    {
        readonly ProductService service = new ProductService();
        readonly CartService cartService = new CartService();
        readonly int productId;
        readonly int customerId;
        Product product;

        public FrmChiTietSanPham(int productId, int customerId)
        {
            InitializeComponent();
            this.productId = productId;
            this.customerId = customerId;
        }

        private void FrmChiTietSanPham_Load(object sender, EventArgs e)
        {
            try
            {
                product = service.GetById(productId);

                if (product == null)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm.");
                    Close();
                    return;
                }

                lblMa.Text = "Mã sản phẩm: " + product.ProductCode;
                lblTen.Text = product.ProductName;
                lblHang.Text = "Nhà sản xuất: " + product.Manufacturer;
                lblGia.Text = product.Price.ToString("N0") + " VNĐ";
                lblTon.Text = "Tồn kho: " + product.StockQuantity;
                txtMoTa.Text = product.Description;
                txtThongSo.Text = product.Specifications;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (customerId <= 0)
            {
                MessageBox.Show("Vui lòng đăng nhập trước.");
                return;
            }

            int quantity = (int)numSoLuong.Value;

            if (quantity <= 0 || quantity > product.StockQuantity)
            {
                MessageBox.Show("Số lượng không hợp lệ.");
                return;
            }

            try
            {
                cartService.AddItem(customerId, productId, quantity);
                MessageBox.Show("Đã thêm sản phẩm vào giỏ hàng.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}