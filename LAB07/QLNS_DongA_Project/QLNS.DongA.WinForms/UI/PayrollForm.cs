using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QLNS.DongA.Services;

namespace QLNS.DongA.UI
{
    internal sealed class PayrollForm : Form
    {
        private readonly PayrollService service = new PayrollService();
        private readonly NumericUpDown month = new NumericUpDown { Minimum = 1, Maximum = 12, Value = DateTime.Today.Month, Width = 70 };
        private readonly NumericUpDown year = new NumericUpDown { Minimum = 2000, Maximum = 2100, Value = DateTime.Today.Year, Width = 90 };
        private readonly DataGridView grid = new DataGridView();
        private readonly Label status = new Label();

        public PayrollForm()
        {
            Text = "Tạo và xem bảng lương tháng";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1200, 700);
            MinimumSize = new Size(900, 550);
            Font = new Font("Segoe UI", 9.5F);

            FlowLayoutPanel top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 48, Padding = new Padding(8), WrapContents = false };
            top.Controls.Add(new Label { Text = "Tháng:", AutoSize = true, Padding = new Padding(0, 7, 0, 0) });
            top.Controls.Add(month);
            top.Controls.Add(new Label { Text = "Năm:", AutoSize = true, Padding = new Padding(0, 7, 0, 0) });
            top.Controls.Add(year);
            Button create = new Button { Text = "Tạo / tính lại bảng lương", AutoSize = true, Height = 30 };
            create.Click += Generate;
            top.Controls.Add(create);

         
            grid.Dock = DockStyle.Fill;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            status.Dock = DockStyle.Bottom; status.Height = 28; status.Padding = new Padding(8, 5, 0, 0);
            Controls.Add(grid); Controls.Add(status); Controls.Add(note); Controls.Add(top);
        }

        private void Generate(object sender, EventArgs e)
        {
            try
            {
                DataTable result = service.Generate((int)month.Value, (int)year.Value);
                grid.DataSource = result;
                decimal totalNet = 0;
                if (result.Columns.Contains("ThucLinh"))
                    foreach (DataRow row in result.Rows) if (row["ThucLinh"] != DBNull.Value) totalNet += Convert.ToDecimal(row["ThucLinh"]);
                status.Text = "Mã bảng lương: BL-" + ((int)year.Value * 100 + (int)month.Value) + " | Số nhân viên: " + result.Rows.Count + " | Tổng thực lĩnh: " + totalNet.ToString("N0") + " VNĐ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Không thể tạo bảng lương. Kiểm tra nhân viên đang làm có quyết định lương đã ký và dữ liệu chấm công/bảo hiểm.\r\n\r\n" + ex.Message,
                    "Lỗi tính lương", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
