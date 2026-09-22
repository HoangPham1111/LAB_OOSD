using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVien.Data;

namespace QuanLyThuVien.Forms
{
    public class FrmDocTrucTuyen : Form
    {
        private TextBox txtMaDauSach, txtTenSach, txtDuongDan, txtDinhDang, txtKichThuoc;
        private CheckBox chkChoPhepDoc, chkChoPhepTai;
        private DataGridView dgvTaiLieu;
        private Button btnThem, btnCapNhat, btnXoa, btnLamMoi, btnDoc;
        public FrmDocTrucTuyen()
        {
            Text = "Quản lý tài liệu trực tuyến";
            Size = new Size(1050, 680);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            TaoGiaoDien();
            LoadData();
        }

        private void TaoGiaoDien()  
        {
            GroupBox group = new GroupBox();
            group.Text = "Quản lý tài liệu đọc trực tuyến";
            group.Location = new Point(25, 15);
            group.Size = new Size(980, 620);

            Label lbMa = TaoLabel("Mã đầu sách:", 45, 40);
            txtMaDauSach = TaoTextBox(145, 37, 220);

            Label lbDuongDan = TaoLabel("Đường dẫn file:", 45, 75);
            txtDuongDan = TaoTextBox(145, 72, 400);

            Label lbKichThuoc = TaoLabel("Kích thước KB:", 45, 110);
            txtKichThuoc = TaoTextBox(145, 107, 220);
            Label lbTen = TaoLabel("Tên sách:", 400, 40);
            txtTenSach = TaoTextBox(480, 37, 200);
            txtTenSach.ReadOnly = true;

            Label lbDinhDang = TaoLabel("Định dạng:", 400, 75);
            txtDinhDang = TaoTextBox(480, 72, 200);
            chkChoPhepDoc = new CheckBox();
            chkChoPhepDoc.Text = "Cho phép đọc";
            chkChoPhepDoc.Location = new Point(400, 110);
            chkChoPhepDoc.AutoSize = true;
            chkChoPhepDoc.Checked = true;

            chkChoPhepTai = new CheckBox();
            chkChoPhepTai.Text = "Cho phép tải";
            chkChoPhepTai.Location = new Point(520, 110);
            chkChoPhepTai.AutoSize = true;
            btnThem = TaoButton("Thêm", 720, 35);
            btnCapNhat = TaoButton("Cập nhật", 720, 70);
            btnXoa = TaoButton("Xóa", 720, 105);
            btnLamMoi = TaoButton("Làm mới", 720, 140);
            btnDoc = TaoButton("ĐỌC TÀI LIỆU", 430, 540);
            btnDoc.Size = new Size(150, 35);

            btnThem.Click += (s, e) => MessageBox.Show("Chức năng Thêm.");
            btnCapNhat.Click += (s, e) => MessageBox.Show("Chức năng Cập nhật.");
            btnXoa.Click += (s, e) => MessageBox.Show("Chức năng Xóa.");
            btnLamMoi.Click += (s, e) =>
            {
                txtMaDauSach.Clear();
                txtTenSach.Clear();
                txtDuongDan.Clear();
                txtDinhDang.Clear();
                txtKichThuoc.Clear();
                chkChoPhepDoc.Checked = true;
                chkChoPhepTai.Checked = false;
                LoadData();
            };
            btnDoc.Click += BtnDoc_Click;
            dgvTaiLieu = new DataGridView();
            dgvTaiLieu.Location = new Point(45, 175);
            dgvTaiLieu.Size = new Size(890, 340);
            dgvTaiLieu.ReadOnly = true;
            dgvTaiLieu.AllowUserToAddRows = false;
            dgvTaiLieu.AllowUserToDeleteRows = false;
            dgvTaiLieu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTaiLieu.MultiSelect = false;
            dgvTaiLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTaiLieu.BackgroundColor = Color.White;
            dgvTaiLieu.CellClick += DgvTaiLieu_CellClick;
            group.Controls.Add(lbMa);
            group.Controls.Add(txtMaDauSach);
            group.Controls.Add(lbDuongDan);
            group.Controls.Add(txtDuongDan);
            group.Controls.Add(lbKichThuoc);
            group.Controls.Add(txtKichThuoc);
            group.Controls.Add(lbTen);
            group.Controls.Add(txtTenSach);
            group.Controls.Add(lbDinhDang);
            group.Controls.Add(txtDinhDang);
            group.Controls.Add(chkChoPhepDoc);
            group.Controls.Add(chkChoPhepTai);
            group.Controls.Add(btnThem);
            group.Controls.Add(btnCapNhat);
            group.Controls.Add(btnXoa);
            group.Controls.Add(btnLamMoi);
            group.Controls.Add(dgvTaiLieu);
            group.Controls.Add(btnDoc);

            Controls.Add(group);
        }
        private void LoadData()
        {
            try
            {
                dgvTaiLieu.DataSource = Db.Query(@"
                    SELECT s.MaDauSach AS [Mã sách], d.TenSach AS [Tên sách],
                           s.DuongDanFile AS [Đường dẫn file], s.DinhDangFile AS [Định dạng],
                           s.KichThuocKB AS [Kích thước KB], s.ChoPhepDoc AS [Cho phép đọc],
                           s.ChoPhepTai AS [Cho phép tải], s.NgayCapNhat AS [Ngày cập nhật]
                    FROM SachDienTu s
                    INNER JOIN DauSach d ON s.MaDauSach = d.MaDauSach
                    ORDER BY d.TenSach");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DgvTaiLieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvTaiLieu.Rows[e.RowIndex];
            txtMaDauSach.Text = Convert.ToString(row.Cells["Mã sách"].Value);
            txtTenSach.Text = Convert.ToString(row.Cells["Tên sách"].Value);
            txtDuongDan.Text = Convert.ToString(row.Cells["Đường dẫn file"].Value);
            txtDinhDang.Text = Convert.ToString(row.Cells["Định dạng"].Value);
            txtKichThuoc.Text = Convert.ToString(row.Cells["Kích thước KB"].Value);
            chkChoPhepDoc.Checked = Convert.ToString(row.Cells["Cho phép đọc"].Value) == "True";
            chkChoPhepTai.Checked = Convert.ToString(row.Cells["Cho phép tải"].Value) == "True";
        }
        private void BtnDoc_Click(object sender, EventArgs e)
        {
            if (dgvTaiLieu.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn tài liệu.");
                return;
            }
            string path = Convert.ToString(dgvTaiLieu.CurrentRow.Cells["Đường dẫn file"].Value);
            if (string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show("Tài liệu chưa có đường dẫn.");
                return;
            }
            try{ Process.Start(path);}
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở tài liệu:\n" + ex.Message);
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