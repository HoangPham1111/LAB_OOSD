using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVien.Data;

namespace QuanLyThuVien.Forms
{
    public class FrmSach : Form
    {
        private TextBox txtMaDauSach, txtTenSach, txtNamXuatBan, txtSoLuong, txtTimKiem;
        private ComboBox cboTheLoai, cboNhaXuatBan;
        private DataGridView dgvSach;
        private Button btnThem, btnCapNhat, btnXoa, btnLamMoi, btnTimKiem;

        public FrmSach()
        {
            Text = "Quản lý đầu sách";
            Size = new Size(1050, 680);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            TaoGiaoDien();
            LoadComboBox();
            LoadData();
        }

        private void TaoGiaoDien()
        {
            GroupBox group = new GroupBox();
            group.Text = "Quản lý đầu sách";
            group.Font = new Font("Arial", 9);
            group.Location = new Point(25, 15);
            group.Size = new Size(980, 620);

            Label lbMa = TaoLabel("Mã đầu sách:", 45, 40);
            txtMaDauSach = TaoTextBox(145, 37, 190);
            Label lbTheLoai = TaoLabel("Thể loại:", 370, 40);

            cboTheLoai = new ComboBox();
            cboTheLoai.Location = new Point(455, 37);
            cboTheLoai.Size = new Size(170, 25);
            cboTheLoai.DropDownStyle = ComboBoxStyle.DropDownList;

            btnThem = TaoButton("Thêm", 670, 35);
            btnThem.Click += BtnThem_Click;

            Label lbTen = TaoLabel("Tên sách:", 45, 75);
            txtTenSach = TaoTextBox(145, 72, 190);
            Label lbNXB = TaoLabel("Nhà xuất bản:", 370, 75);

            cboNhaXuatBan = new ComboBox();
            cboNhaXuatBan.Location = new Point(455, 72);
            cboNhaXuatBan.Size = new Size(170, 25);
            cboNhaXuatBan.DropDownStyle = ComboBoxStyle.DropDownList;

            btnCapNhat = TaoButton("Cập nhật", 670, 70);
            btnCapNhat.Click += BtnCapNhat_Click;

            Label lbNam = TaoLabel("Năm xuất bản:", 45, 110);
            txtNamXuatBan = TaoTextBox(145, 107, 190);

            btnXoa = TaoButton("Xóa", 670, 105);
            btnXoa.Click += BtnXoa_Click;

            Label lbSoLuong = TaoLabel("Số lượng hiện có:", 45, 145);
            txtSoLuong = TaoTextBox(145, 142, 190);

            btnLamMoi = TaoButton("Làm mới", 670, 140);
            btnLamMoi.Click += BtnLamMoi_Click;

            Label lbTimKiem = TaoLabel("Tìm kiếm:", 45, 190);
            txtTimKiem = TaoTextBox(145, 187, 350);

            btnTimKiem = TaoButton("Tìm kiếm", 510, 185);
            btnTimKiem.Size = new Size(115, 28);
            btnTimKiem.Click += (s, e) => LoadData();

            dgvSach = new DataGridView();
            dgvSach.Location = new Point(45, 235);
            dgvSach.Size = new Size(890, 340);
            dgvSach.ReadOnly = true;
            dgvSach.AllowUserToAddRows = false;
            dgvSach.AllowUserToDeleteRows = false;
            dgvSach.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSach.MultiSelect = false;
            dgvSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSach.BackgroundColor = Color.White;
            dgvSach.BorderStyle = BorderStyle.FixedSingle;
            dgvSach.CellClick += DgvSach_CellClick;

            group.Controls.Add(lbMa);
            group.Controls.Add(txtMaDauSach);
            group.Controls.Add(lbTheLoai);
            group.Controls.Add(cboTheLoai);
            group.Controls.Add(btnThem);
            group.Controls.Add(lbTen);
            group.Controls.Add(txtTenSach);
            group.Controls.Add(lbNXB);
            group.Controls.Add(cboNhaXuatBan);
            group.Controls.Add(btnCapNhat);
            group.Controls.Add(lbNam);
            group.Controls.Add(txtNamXuatBan);
            group.Controls.Add(btnXoa);
            group.Controls.Add(lbSoLuong);
            group.Controls.Add(txtSoLuong);
            group.Controls.Add(btnLamMoi);
            group.Controls.Add(lbTimKiem);
            group.Controls.Add(txtTimKiem);
            group.Controls.Add(btnTimKiem);
            group.Controls.Add(dgvSach);

            Controls.Add(group);
        }
        private Label TaoLabel(string text, int x, int y)
        {
            Label lb = new Label();
            lb.Text = text;
            lb.Location = new Point(x, y);
            lb.AutoSize = true;
            return lb;
        }
        private TextBox TaoTextBox(int x, int y, int width)
        {
            TextBox txt = new TextBox();
            txt.Location = new Point(x, y);
            txt.Size = new Size(width, 25);
            return txt;
        }
        private Button TaoButton(string text, int x, int y)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new Point(x, y);
            btn.Size = new Size(100, 28);
            return btn;
        }
        private void LoadComboBox()
        {
            try
            {
                DataTable dtTheLoai = Db.Query(@"SELECT MaTheLoai, TenTheLoai FROM TheLoai ORDER BY TenTheLoai");
                cboTheLoai.DataSource = dtTheLoai;
                cboTheLoai.DisplayMember = "TenTheLoai";
                cboTheLoai.ValueMember = "MaTheLoai";

                DataTable dtNXB = Db.Query(@"SELECT MaNhaXuatBan, TenNhaXuatBan FROM NhaXuatBan ORDER BY TenNhaXuatBan");
                cboNhaXuatBan.DataSource = dtNXB;
                cboNhaXuatBan.DisplayMember = "TenNhaXuatBan";
                cboNhaXuatBan.ValueMember = "MaNhaXuatBan";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải thể loại / nhà xuất bản:\n" + ex.Message);
            }
        }
        private void LoadData()
        {
            try
            {
                string key = txtTimKiem == null ? "" : txtTimKiem.Text.Trim();

                dgvSach.DataSource = Db.Query(@"
                    SELECT d.MaDauSach AS [Mã sách], d.TenSach AS [Tên sách],
                           d.NamXuatBan AS [Năm XB], d.SoLuongHienCo AS [Số lượng],
                           tl.TenTheLoai AS [Thể loại], nxb.MaNhaXuatBan AS [Mã NXB]
                    FROM DauSach d
                    LEFT JOIN TheLoai tl ON d.MaTheLoai = tl.MaTheLoai
                    LEFT JOIN NhaXuatBan nxb ON d.MaNhaXuatBan = nxb.MaNhaXuatBan
                    WHERE d.MaDauSach LIKE '%' + @Key + '%'
                       OR d.TenSach LIKE '%' + @Key + '%'
                    ORDER BY d.MaDauSach",
                    new SqlParameter("@Key", key));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DgvSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvSach.Rows[e.RowIndex];
            txtMaDauSach.Text = Convert.ToString(row.Cells["Mã sách"].Value);
            txtTenSach.Text = Convert.ToString(row.Cells["Tên sách"].Value);
            txtNamXuatBan.Text = Convert.ToString(row.Cells["Năm XB"].Value);
            txtSoLuong.Text = Convert.ToString(row.Cells["Số lượng"].Value);
        }
        private void BtnThem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng Thêm sẽ sử dụng các thông tin đang nhập.", "Thông báo");
        }
        private void BtnCapNhat_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng Cập nhật đã sẵn sàng cho phần xử lý SQL.", "Thông báo");
        }
        private void BtnXoa_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng Xóa đã sẵn sàng.", "Thông báo");
        }
        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaDauSach.Clear();
            txtTenSach.Clear();
            txtNamXuatBan.Clear();
            txtSoLuong.Clear();
            LoadData();
        }
    }
}