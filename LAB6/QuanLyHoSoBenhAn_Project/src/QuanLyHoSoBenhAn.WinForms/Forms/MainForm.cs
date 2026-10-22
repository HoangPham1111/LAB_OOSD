using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyHoSoBenhAn.Forms
{
    public sealed class MainForm : Form
    {
        private readonly Dictionary<string, Form> _openForms = new Dictionary<string, Form>();

        public MainForm()
        {
            Text = "Quản lý hồ sơ bệnh án";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 1180;
            Height = 760;
            MinimumSize = new Size(1000, 650);
            BackColor = Color.FromArgb(245, 247, 250);

            var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, BackColor = BackColor };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            Controls.Add(root);

            var side = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(34, 52, 79), Padding = new Padding(12) };
            root.Controls.Add(side, 0, 0);
            var brand = new Label { Text = "HỒ SƠ\nBỆNH ÁN", ForeColor = Color.White, Font = new Font("Segoe UI", 19, FontStyle.Bold), AutoSize = false, Height = 90, Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleLeft };
            side.Controls.Add(brand);
            var nav = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, Padding = new Padding(0, 8, 0, 0) };
            side.Controls.Add(nav);
            AddNav(nav, "Bệnh nhân", () => Open("Bệnh nhân", new PatientForm()));
            AddNav(nav, "Lệ phí năm", () => Open("Lệ phí năm", new AnnualFeeForm()));
            AddNav(nav, "Cấp hồ sơ bệnh án", () => Open("Cấp hồ sơ", new RecordForm()));
            AddNav(nav, "Bác sĩ", () => Open("Bác sĩ", new MasterDataForm(MasterDataForm.Doctors())));
            AddNav(nav, "Khoa điều trị", () => Open("Khoa", new MasterDataForm(MasterDataForm.Departments())));
            AddNav(nav, "Nhân viên CSKH", () => Open("Nhân viên CSKH", new MasterDataForm(MasterDataForm.Employees())));
            AddNav(nav, "Danh mục thuốc", () => Open("Danh mục thuốc", new MasterDataForm(MasterDataForm.Medicines())));
            AddNav(nav, "Khám bệnh / Phiếu khám", () => Open("Khám bệnh", new ClinicalForm()));
            AddNav(nav, "Lập toa thuốc", () => Open("Lập toa thuốc", new PrescriptionForm()));
            AddNav(nav, "Xét nghiệm", () => Open("Xét nghiệm", new TestForm()));
            AddNav(nav, "Lịch khám định kỳ", () => Open("Lịch khám", new AppointmentForm()));
            AddNav(nav, "Viện phí / Thanh toán", () => Open("Viện phí", new HospitalFeeForm()));

            var content = new Panel { Dock = DockStyle.Fill, Padding = new Padding(32), BackColor = BackColor };
            root.Controls.Add(content, 1, 0);
            content.Controls.Add(new Label
            {
                Text = "HỆ THỐNG QUẢN LÝ HỒ SƠ BỆNH ÁN",
                Font = new Font("Segoe UI", 22, FontStyle.Bold), ForeColor = Color.FromArgb(34, 52, 79),
                AutoSize = true, Location = new Point(26, 25)
            });
            content.Controls.Add(new Label
            {
                Text = "Luồng nghiệp vụ trọng tâm:\n1. Đăng ký bệnh nhân và ghi nhận lệ phí năm.\n2. Cấp hồ sơ bệnh án khi lệ phí năm hiện tại đã được ghi nhận đã đóng.\n3. Tạo đợt điều trị và phiếu khám.\n4. Lập toa thuốc và lưu toa cùng chi tiết bằng transaction.\n5. Ghi xét nghiệm, đặt lịch khám và quản lý viện phí.",
                Font = new Font("Segoe UI", 12), ForeColor = Color.FromArgb(65, 76, 90),
                AutoSize = true, Location = new Point(30, 95)
            });
            
        }

        private void AddNav(FlowLayoutPanel panel, string text, Action action)
        {
            var b = new Button
            {
                Text = text, Width = 214, Height = 38, TextAlign = ContentAlignment.MiddleLeft,
                FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(45, 67, 98), ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5f), Margin = new Padding(0, 3, 0, 3), Padding = new Padding(8, 0, 0, 0)
            };
            b.FlatAppearance.BorderSize = 0;
            b.Click += (s, e) => action();
            panel.Controls.Add(b);
        }

        private void Open(string key, Form form)
        {
            Form existing;
            if (_openForms.TryGetValue(key, out existing) && !existing.IsDisposed)
            {
                existing.WindowState = FormWindowState.Normal;
                existing.BringToFront();
                existing.Activate();
                form.Dispose();
                return;
            }
            _openForms[key] = form;
            form.StartPosition = FormStartPosition.CenterParent;
            form.FormClosed += (s, e) => { if (_openForms.ContainsKey(key)) _openForms.Remove(key); };
            form.Show(this);
        }
    }
}
