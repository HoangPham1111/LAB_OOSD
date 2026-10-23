using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLNS.DongA.Data;
using QLNS.DongA.Services;

namespace QLNS.DongA.UI
{
    internal sealed class GenericTableForm : Form
    {
        private readonly GenericTableService service = new GenericTableService();
        private readonly ComboBox tableCombo = new ComboBox();
        private readonly DataGridView grid = new DataGridView();
        private readonly Label status = new Label();
        private DataTable currentTable;

        public GenericTableForm()
        {
            Text = "Quản lý dữ liệu nghiệp vụ";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1200, 720);
            MinimumSize = new Size(900, 550);
            Font = new Font("Segoe UI", 9.5F);

            Panel top = new Panel { Dock = DockStyle.Top, Height = 48, Padding = new Padding(8) };
            tableCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            tableCombo.Width = 330;
            tableCombo.DataSource = TableCatalog.All.ToList();
            FlowLayoutPanel toolbar = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
            toolbar.Controls.Add(new Label { Text = "Bảng:", AutoSize = true, Padding = new Padding(0, 7, 0, 0) });
            toolbar.Controls.Add(tableCombo);
            toolbar.Controls.Add(MakeButton("Tải dữ liệu", LoadSelected));
            toolbar.Controls.Add(MakeButton("Thêm dòng", AddRow));
            toolbar.Controls.Add(MakeButton("Xóa dòng chọn", DeleteRow));
            toolbar.Controls.Add(MakeButton("Lưu thay đổi", SaveChanges));
            top.Controls.Add(toolbar);

            grid.Dock = DockStyle.Fill;
            grid.AllowUserToAddRows = true;
            grid.AllowUserToDeleteRows = false;
            grid.AutoGenerateColumns = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            grid.DataError += delegate(object sender, DataGridViewDataErrorEventArgs e)
            {
                e.ThrowException = false;
                MessageBox.Show(this, "Giá trị nhập không đúng kiểu dữ liệu của cột.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            };
            status.Dock = DockStyle.Bottom;
            status.Height = 28;
            status.Padding = new Padding(8, 5, 0, 0);
            Controls.Add(grid);
            Controls.Add(status);
            Controls.Add(top);
            Shown += delegate { LoadSelected(null, EventArgs.Empty); };
        }

        private static Button MakeButton(string text, EventHandler click)
        {
            Button b = new Button { Text = text, AutoSize = true, Height = 29, Margin = new Padding(5, 1, 5, 1) };
            b.Click += click;
            return b;
        }

        private string SelectedTableName
        {
            get { return ((TableDefinition)tableCombo.SelectedItem).Name; }
        }

        private void LoadSelected(object sender, EventArgs e)
        {
            try
            {
                if (grid.IsCurrentCellInEditMode) grid.EndEdit();
                currentTable = service.Load(SelectedTableName);
                grid.DataSource = currentTable;
                status.Text = "Bảng " + SelectedTableName + ": " + currentTable.Rows.Count + " dòng (màn hình tải tối đa 1.000 dòng).";
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void AddRow(object sender, EventArgs e)
        {
            if (currentTable == null) return;
            DataRow row = currentTable.NewRow();
            currentTable.Rows.Add(row);
            int index = currentTable.Rows.Count - 1;
            if (index >= 0 && index < grid.Rows.Count) grid.CurrentCell = grid.Rows[index].Cells[0];
            status.Text = "Nhập dữ liệu cho dòng mới; điền khóa chính và các khóa ngoại bắt buộc trước khi lưu.";
        }

        private void DeleteRow(object sender, EventArgs e)
        {
            if (grid.CurrentRow == null || !(grid.CurrentRow.DataBoundItem is DataRowView view)) return;
            if (MessageBox.Show(this, "Xóa dòng đang chọn? Nếu dữ liệu đã được nghiệp vụ khác tham chiếu, SQL Server sẽ từ chối.", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            view.Row.Delete();
        }

        private void SaveChanges(object sender, EventArgs e)
        {
            try
            {
                grid.EndEdit();
                BindingContext[currentTable].EndCurrentEdit();
                int affected = service.Save(SelectedTableName, currentTable);
                status.Text = "Đã lưu. Số dòng được thêm/cập nhật/xóa: " + affected + ".";
                MessageBox.Show(this, "Lưu dữ liệu thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSelected(null, EventArgs.Empty);
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void ShowError(Exception ex)
        {
            status.Text = "Có lỗi khi thao tác dữ liệu.";
            MessageBox.Show(this, "Không thể hoàn thành thao tác. Kiểm tra khóa chính, khóa ngoại, trường bắt buộc và ràng buộc nghiệp vụ.\r\n\r\nChi tiết: " + ex.Message,
                "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
