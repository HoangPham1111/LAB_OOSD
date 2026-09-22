using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVien.Data;

namespace QuanLyThuVien.Forms
{
    public class FrmDocGia : Form
    {
        private TextBox txtMaDocGia, txtHo, txtTen, txtNgaySinh;
        private ComboBox cboPhai;
        private TextBox txtDienThoai, txtDiaChi, txtEmail, txtAnh, txtNgayCap, txtHanSuDung;
        private CheckBox chkDaDongLePhi;
        private DataGridView dgvDocGia;
        private Button btnThem, btnCapNhat, btnXoa, btnCapThe, btnGiaHan;
        public FrmDocGia()
        {
            Text = "Độc giả và thẻ thư viện";
            Size = new Size(1050, 680);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            TaoGiaoDien();
            LoadData();
        }
        private void TaoGiaoDien()
        {
            GroupBox group = new GroupBox();
            group.Text = "Độc giả và thẻ thư viện";
            group.Location = new Point(25, 15);
            group.Size = new Size(980, 620);
            Label lbMa = TaoLabel("Mã độc giả:", 45, 40);
            txtMaDocGia = TaoTextBox(140, 37, 190);
           Label lbHo = TaoLabel("Họ:", 45, 75);
            txtHo = TaoTextBox(140, 72, 190);
            Label lbTen = TaoLabel("Tên:", 45, 110);
            txtTen = TaoTextBox(140, 107, 190);
            Label lbNgaySinh = TaoLabel("Ngày sinh:", 45, 145);
            txtNgaySinh = TaoTextBox(140, 142, 190);
            Label lbPhai = TaoLabel("Phái:", 45, 180);
            cboPhai = new ComboBox();
            cboPhai.Location = new Point(140, 177);
            cboPhai.Size = new Size(190, 25);
            cboPhai.Items.Add("Nam");
            cboPhai.Items.Add("Nữ");
            Label lbDienThoai = TaoLabel("Điện thoại:", 370, 40);
            txtDienThoai = TaoTextBox(470, 37, 190);
            Label lbDiaChi = TaoLabel("Địa chỉ:", 370, 75);
            txtDiaChi = TaoTextBox(470, 72, 190);
            Label lbEmail = TaoLabel("Email:", 370, 110);
            txtEmail = TaoTextBox(470, 107, 190);
            Label lbAnh = TaoLabel("Ảnh 3x4:", 370, 145);
            txtAnh = TaoTextBox(470, 142, 190);
            Label lbNgayCap = TaoLabel("Ngày cấp:", 370, 180);
            txtNgayCap = TaoTextBox(470, 177, 120);
            Label lbHan = TaoLabel("Hạn sử dụng:", 370, 215);
            txtHanSuDung = TaoTextBox(470, 212, 120);
            btnThem = TaoButton("Thêm", 700, 35);
            btnCapNhat = TaoButton("Cập nhật", 700, 70);
            btnXoa = TaoButton("Xóa", 700, 105);
            btnCapThe = TaoButton("Cấp thẻ", 600, 210);
            btnGiaHan = TaoButton("Gia hạn", 710, 210);
            btnThem.Click += (s, e) => MessageBox.Show("Chức năng Thêm.");
            btnCapNhat.Click += (s, e) => MessageBox.Show("Chức năng Cập nhật.");
            btnXoa.Click += (s, e) => MessageBox.Show("Chức năng Xóa.");
            btnCapThe.Click += (s, e) => MessageBox.Show("Chức năng Cấp thẻ.");
            btnGiaHan.Click += (s, e) => MessageBox.Show("Chức năng Gia hạn.");
            chkDaDongLePhi = new CheckBox();
            chkDaDongLePhi.Text = "Đã đóng lệ phí";
            chkDaDongLePhi.Location = new Point(610, 180);
            chkDaDongLePhi.AutoSize = true;
            dgvDocGia = new DataGridView();
            dgvDocGia.Location = new Point(45, 270);
            dgvDocGia.Size = new Size(890, 300);
            dgvDocGia.ReadOnly = true;
            dgvDocGia.AllowUserToAddRows = false;
            dgvDocGia.AllowUserToDeleteRows = false;
            dgvDocGia.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDocGia.MultiSelect = false;
            dgvDocGia.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDocGia.BackgroundColor = Color.White;
            group.Controls.Add(lbMa);
            group.Controls.Add(txtMaDocGia);
            group.Controls.Add(lbHo);
            group.Controls.Add(txtHo);
            group.Controls.Add(lbTen);
            group.Controls.Add(txtTen);
            group.Controls.Add(lbNgaySinh);
            group.Controls.Add(txtNgaySinh);
            group.Controls.Add(lbPhai);
            group.Controls.Add(cboPhai);
            group.Controls.Add(lbDienThoai);
            group.Controls.Add(txtDienThoai);
            group.Controls.Add(lbDiaChi);
            group.Controls.Add(txtDiaChi);
            group.Controls.Add(lbEmail);
            group.Controls.Add(txtEmail);
            group.Controls.Add(lbAnh);
            group.Controls.Add(txtAnh);
            group.Controls.Add(lbNgayCap);
            group.Controls.Add(txtNgayCap);
            group.Controls.Add(lbHan);
            group.Controls.Add(txtHanSuDung);
            group.Controls.Add(chkDaDongLePhi);
            group.Controls.Add(btnThem);
            group.Controls.Add(btnCapNhat);
            group.Controls.Add(btnXoa);
            group.Controls.Add(btnCapThe);
            group.Controls.Add(btnGiaHan);
            group.Controls.Add(dgvDocGia);
            Controls.Add(group);
        }
        private void LoadData()
        {
            try
            {
                dgvDocGia.DataSource = Db.Query("SELECT * FROM DocGia");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
    }
}