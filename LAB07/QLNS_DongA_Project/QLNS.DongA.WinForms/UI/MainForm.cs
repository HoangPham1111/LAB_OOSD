using System;
using System.Drawing;
using System.Windows.Forms;
using QLNS.DongA.Data;

namespace QLNS.DongA.UI
{
    internal sealed class MainForm : Form
    {
        public MainForm()
        {
            Text = "Hệ thống quản lý nhân sự - Công ty Sơn Đông Á";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(780, 480);
            Size = new Size(900, 560);
            Font = new Font("Segoe UI", 10F);

            Label heading = new Label
            {
                Text = "HỆ THỐNG QUẢN LÝ NHÂN SỰ\r\nCÔNG TY SƠN ĐÔNG Á",
                Dock = DockStyle.Top,
                Height = 90,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold)
            };

            Label intro = new Label
            {
                Text = "",
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };

            TableLayoutPanel buttons = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                Padding = new Padding(35),
                BackColor = Color.WhiteSmoke
            };
            buttons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            buttons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            buttons.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            buttons.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

            buttons.Controls.Add(CreateButton("1. Quản lý dữ liệu nghiệp vụ (23 bảng)", delegate { Open(new GenericTableForm()); }), 0, 0);
            buttons.Controls.Add(CreateButton("2. Tuyển dụng → thử việc → hợp đồng", delegate { Open(new RecruitmentWorkflowForm()); }), 1, 0);
            buttons.Controls.Add(CreateButton("3. Quyết định khen thưởng → ký → phiếu chi", delegate { Open(new AwardForm()); }), 0, 1);
            buttons.Controls.Add(CreateButton("4. Tạo bảng lương tháng", delegate { Open(new PayrollForm()); }), 1, 1);

            Label footer = new Label
            {
                Text = "",
                Dock = DockStyle.Bottom,
                Height = 35,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.DarkSlateGray
            };
            Controls.Add(buttons);
            Controls.Add(intro);
            Controls.Add(heading);
            Controls.Add(footer);
        }

        private static Button CreateButton(string text, EventHandler click)
        {
            Button button = new Button
            {
                Text = text,
                Dock = DockStyle.Fill,
                Margin = new Padding(12),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                FlatStyle = FlatStyle.Standard
            };
            button.Click += click;
            return button;
        }

        private void Open(Form form)
        {
            using (form) form.ShowDialog(this);
        }
    }
}
