using System;
using System.Collections.Generic;
using System.Windows.Forms;
using eSHOPPING.Models;
using eSHOPPING.Services;

namespace eSHOPPING.Forms
{
    public partial class FrmSanPham : Form
    {
        readonly ProductService service = new ProductService();
        readonly int customerId;
        List<Product> products = new List<Product>();

        public FrmSanPham(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
        }

        private void FrmSanPham_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                products = service.GetAll();
                dgvSanPham.DataSource = products;
                FormatGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string key = txtTimKiem.Text.Trim();

            try
            {
                products = service.Search(key);
                dgvSanPham.DataSource = products;
                FormatGrid();
            }
            catch
            {
                LoadProducts();
            }
        }

        private void FormatGrid()
        {
            if (dgvSanPham.Columns.Count == 0) return;

            dgvSanPham.Columns["ProductId"].Visible = false;
            dgvSanPham.Columns["ProductCode"].HeaderText = "Mã SP";
            dgvSanPham.Columns["ProductName"].HeaderText = "Tên sản phẩm";
            dgvSanPham.Columns["Manufacturer"].HeaderText = "Nhà sản xuất";
            dgvSanPham.Columns["Price"].HeaderText = "Giá";
            dgvSanPham.Columns["StockQuantity"].HeaderText = "Tồn kho";
            dgvSanPham.Columns["IsAvailable"].HeaderText = "Đang bán";

            dgvSanPham.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvSanPham.Columns["Price"].DefaultCellStyle.Format = "N0";
        }

        private void dgvSanPham_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int id = Convert.ToInt32(
                dgvSanPham.Rows[e.RowIndex].Cells["ProductId"].Value);

            FrmChiTietSanPham f = new FrmChiTietSanPham(id, customerId);
            f.ShowDialog();
        }

        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            if (dgvSanPham.CurrentRow == null) return;

            int id = Convert.ToInt32(
                dgvSanPham.CurrentRow.Cells["ProductId"].Value);

            FrmChiTietSanPham f = new FrmChiTietSanPham(id, customerId);
            f.ShowDialog();
        }
    }
}