using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using eSHOPPING.Models;
using eSHOPPING.Services;

namespace eSHOPPING.Forms
{
    public partial class FrmThanhToan : Form
    {
        readonly int customerId;
        readonly CartService cartService = new CartService();
        readonly CheckoutService checkoutService = new CheckoutService();

        List<CartItem> items = new List<CartItem>();

        public FrmThanhToan(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
        }

        private void FrmThanhToan_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                items = cartService.GetCart(customerId);

                if (items == null || items.Count == 0)
                {
                    MessageBox.Show("Giỏ hàng đang trống.");
                    Close();
                    return;
                }

                LoadDeliveryType();
                LoadDeliveryArea();
                LoadCardType();

                decimal subtotal = items.Sum(x => x.Amount);
                lblTamTinh.Text = "Tạm tính: " + subtotal.ToString("N0") + " VNĐ";
                lblPhi.Text = "Phí giao hàng: 0 VNĐ";
                lblTong.Text = "Tổng thanh toán: " + subtotal.ToString("N0") + " VNĐ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDeliveryType()
        {
            cboLoaiGiaoHang.Items.Clear();
            cboLoaiGiaoHang.Items.Add(new DeliveryType
            {
                DeliveryTypeId = 1,
                DeliveryTypeName = "Thường"
            });
            cboLoaiGiaoHang.Items.Add(new DeliveryType
            {
                DeliveryTypeId = 2,
                DeliveryTypeName = "Chuyển phát nhanh"
            });
            cboLoaiGiaoHang.Items.Add(new DeliveryType
            {
                DeliveryTypeId = 3,
                DeliveryTypeName = "Chuyển phát nhanh trong ngày"
            });

            cboLoaiGiaoHang.SelectedIndex = 0;
        }

        private void LoadDeliveryArea()
        {
            cboKhuVuc.Items.Clear();

            cboKhuVuc.Items.Add(new DeliveryArea
            {
                DeliveryAreaId = 1,
                AreaName = "Nội thành TP.HCM"
            });

            cboKhuVuc.Items.Add(new DeliveryArea
            {
                DeliveryAreaId = 2,
                AreaName = "Ngoại thành TP.HCM"
            });

            cboKhuVuc.Items.Add(new DeliveryArea
            {
                DeliveryAreaId = 3,
                AreaName = "Tỉnh thành khác"
            });

            cboKhuVuc.SelectedIndex = 0;
        }

        private void LoadCardType()
        {
            cboLoaiThe.Items.Clear();
            cboLoaiThe.Items.Add("VISA");
            cboLoaiThe.Items.Add("MasterCard");
            cboLoaiThe.Items.Add("Discover");
            cboLoaiThe.Items.Add("American Express");
            cboLoaiThe.SelectedIndex = 0;
        }

        private void cboLoaiThe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLoaiThe.Text == "American Express")
            {
                txtSoThe.MaxLength = 15;
                txtCSV.MaxLength = 4;
                lblHuongDanThe.Text = "American Express: 15 số / CSV 4 số";
            }
            else
            {
                txtSoThe.MaxLength = 16;
                txtCSV.MaxLength = 3;
                lblHuongDanThe.Text = "VISA/MasterCard/Discover: 16 số / CSV 3 số";
            }
        }

        private void btnTinhPhi_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboLoaiGiaoHang.SelectedItem == null ||
                    cboKhuVuc.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn loại giao hàng và khu vực.");
                    return;
                }

                decimal subtotal = items.Sum(x => x.Amount);

                DeliveryType type =
                    (DeliveryType)cboLoaiGiaoHang.SelectedItem;

                DeliveryArea area =
                    (DeliveryArea)cboKhuVuc.SelectedItem;

                decimal fee = CalculateShippingFee(
                    type.DeliveryTypeId,
                    area.DeliveryAreaId,
                    subtotal);

                lblTamTinh.Text =
                    "Tạm tính: " + subtotal.ToString("N0") + " VNĐ";

                lblPhi.Text =
                    "Phí giao hàng: " + fee.ToString("N0") + " VNĐ";

                lblTong.Text =
                    "Tổng thanh toán: " +
                    (subtotal + fee).ToString("N0") + " VNĐ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private decimal CalculateShippingFee(
            int deliveryTypeId,
            int deliveryAreaId,
            decimal subtotal)
        {
            if (deliveryTypeId == 2 && subtotal >= 1000000)
                return 0;

            if (deliveryTypeId == 3 && subtotal >= 5000000)
                return 0;

            if (deliveryTypeId == 1)
            {
                if (deliveryAreaId == 1) return 30000;
                if (deliveryAreaId == 2) return 50000;
                return 70000;
            }

            if (deliveryTypeId == 2)
            {
                if (deliveryAreaId == 1) return 50000;
                if (deliveryAreaId == 2) return 80000;
                return 120000;
            }

            if (deliveryAreaId == 1) return 80000;
            if (deliveryAreaId == 2) return 120000;
            return 180000;
        }

        private bool ValidateInput()
        {
            if (txtNguoiNhan.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tên người nhận.");
                txtNguoiNhan.Focus();
                return false;
            }

            if (txtDiaChi.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập địa chỉ.");
                txtDiaChi.Focus();
                return false;
            }

            if (txtDienThoai.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập số điện thoại.");
                txtDienThoai.Focus();
                return false;
            }

            if (txtSoThe.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập số thẻ.");
                txtSoThe.Focus();
                return false;
            }

            if (txtCSV.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập CSV.");
                txtCSV.Focus();
                return false;
            }

            if (txtChuThe.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tên chủ thẻ.");
                txtChuThe.Focus();
                return false;
            }

            if (!int.TryParse(txtThangHetHan.Text, out int month) ||
                month < 1 || month > 12)
            {
                MessageBox.Show("Tháng hết hạn không hợp lệ.");
                return false;
            }

            if (!int.TryParse(txtNamHetHan.Text, out int year))
            {
                MessageBox.Show("Năm hết hạn không hợp lệ.");
                return false;
            }

            return true;
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                DeliveryType type =
                    (DeliveryType)cboLoaiGiaoHang.SelectedItem;

                DeliveryArea area =
                    (DeliveryArea)cboKhuVuc.SelectedItem;

                decimal subtotal = items.Sum(x => x.Amount);

                decimal shippingFee = CalculateShippingFee(
                    type.DeliveryTypeId,
                    area.DeliveryAreaId,
                    subtotal);

                CheckoutRequest request = new CheckoutRequest
                {
                    CustomerId = customerId,
                    RecipientId = 1,
                    DeliveryTypeId = type.DeliveryTypeId,
                    DeliveryAreaId = area.DeliveryAreaId,
                    CardType = cboLoaiThe.Text,
                    CardNumber = txtSoThe.Text.Trim(),
                    CSV = txtCSV.Text.Trim(),
                    CardHolderName = txtChuThe.Text.Trim(),
                    ExpiryMonth = Convert.ToInt32(txtThangHetHan.Text),
                    ExpiryYear = Convert.ToInt32(txtNamHetHan.Text)
                };

                string orderCode = checkoutService.Checkout(request);

                FrmXacNhan f = new FrmXacNhan(
                    orderCode,
                    subtotal,
                    shippingFee,
                    subtotal + shippingFee);

                f.ShowDialog();

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Thanh toán thất bại",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}