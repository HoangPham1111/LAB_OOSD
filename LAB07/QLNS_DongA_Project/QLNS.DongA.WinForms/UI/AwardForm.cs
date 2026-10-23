using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QLNS.DongA.Services;

namespace QLNS.DongA.UI
{
    internal sealed class AwardForm : Form
    {
        private readonly AwardService service = new AwardService();
        private readonly ComboBox targetType = new ComboBox();
        private readonly ComboBox employeeCombo = new ComboBox();
        private readonly ComboBox departmentCombo = new ComboBox();
        private readonly DateTimePicker dateCreated = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" };
        private readonly TextBox reason = new TextBox();
        private readonly TextBox content = new TextBox();
        private readonly NumericUpDown amount = new NumericUpDown { Minimum = 0, Maximum = 1000000000000m, DecimalPlaces = 2, ThousandsSeparator = true, Value = 500000 };
        private readonly ComboBox decisions = new ComboBox();
        private readonly DateTimePicker signDate = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" };
        private readonly DateTimePicker payoutDate = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" };
        private readonly ComboBox payoutMethod = new ComboBox();

        public AwardForm()
        {
            Text = "Khen thưởng - ký duyệt - phiếu chi";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(900, 700);
            MinimumSize = new Size(760, 600);
            Font = new Font("Segoe UI", 9.5F);

            TableLayoutPanel root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(10) };
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 48));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 18));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 34));
            root.Controls.Add(BuildCreateGroup(), 0, 0);
            root.Controls.Add(BuildDecisionGroup(), 0, 1);
            root.Controls.Add(BuildPayoutGroup(), 0, 2);
            Controls.Add(root);

            Shown += delegate { LoadLookups(); };
            targetType.SelectedIndexChanged += delegate { UpdateTargetControls(); };
        }

        private Control BuildCreateGroup()
        {
            GroupBox box = new GroupBox { Text = "1. Lập quyết định khen thưởng", Dock = DockStyle.Fill, Padding = new Padding(8) };
            TableLayoutPanel table = NewTable(6);
            targetType.DropDownStyle = ComboBoxStyle.DropDownList;
            targetType.Items.Add(new Choice("CaNhan", "Cá nhân"));
            targetType.Items.Add(new Choice("TapThe", "Tập thể phòng ban/đơn vị"));
            targetType.DisplayMember = "Text"; targetType.ValueMember = "Value"; targetType.SelectedIndex = 0;
            Configure(employeeCombo); Configure(departmentCombo);
            reason.Dock = DockStyle.Fill;
            content.Dock = DockStyle.Fill; content.Multiline = true; content.Height = 48; content.ScrollBars = ScrollBars.Vertical;
            amount.Dock = DockStyle.Fill;
            dateCreated.Dock = DockStyle.Fill;
            AddRow(table, 0, "Đối tượng", targetType);
            AddRow(table, 1, "Nhân viên", employeeCombo);
            AddRow(table, 2, "Phòng ban/tập thể", departmentCombo);
            AddRow(table, 3, "Ngày lập", dateCreated);
            AddRow(table, 4, "Lý do", reason);
            AddRow(table, 5, "Nội dung / số tiền", BuildContentAndAmount());
            Button create = new Button { Text = "Lập quyết định (chờ ký)", Dock = DockStyle.Bottom, Height = 36 };
            create.Click += CreateDraft;
            box.Controls.Add(table);
            box.Controls.Add(create);
            return box;
        }

        private Control BuildContentAndAmount()
        {
            TableLayoutPanel panel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            panel.Controls.Add(content, 0, 0);
            panel.Controls.Add(amount, 1, 0);
            return panel;
        }

        private Control BuildDecisionGroup()
        {
            GroupBox box = new GroupBox { Text = "2. Chọn quyết định và ghi nhận Giám đốc ký", Dock = DockStyle.Fill, Padding = new Padding(8) };
            FlowLayoutPanel flow = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = true, Padding = new Padding(4) };
            decisions.DropDownStyle = ComboBoxStyle.DropDownList; decisions.Width = 340;
            signDate.Width = 130;
            Button refresh = new Button { Text = "Tải quyết định", AutoSize = true };
            Button sign = new Button { Text = "Giám đốc ký", AutoSize = true };
            refresh.Click += delegate { RefreshDecisions(); };
            sign.Click += SignDecision;
            flow.Controls.Add(new Label { Text = "Quyết định:", AutoSize = true, Padding = new Padding(0, 7, 0, 0) });
            flow.Controls.Add(decisions);
            flow.Controls.Add(new Label { Text = "Ngày ký:", AutoSize = true, Padding = new Padding(0, 7, 0, 0) });
            flow.Controls.Add(signDate);
            flow.Controls.Add(refresh);
            flow.Controls.Add(sign);
            box.Controls.Add(flow);
            return box;
        }

        private Control BuildPayoutGroup()
        {
            GroupBox box = new GroupBox { Text = "3. Lập phiếu chi sau khi quyết định đã ký", Dock = DockStyle.Fill, Padding = new Padding(8) };
            TableLayoutPanel table = NewTable(2);
            payoutDate.Dock = DockStyle.Fill;
            DataTable methods = new DataTable(); methods.Columns.Add("Value"); methods.Columns.Add("Text");
            methods.Rows.Add("TienMat", "Tiền mặt"); methods.Rows.Add("ChuyenKhoan", "Chuyển khoản");
            payoutMethod.DropDownStyle = ComboBoxStyle.DropDownList; payoutMethod.DisplayMember = "Text"; payoutMethod.ValueMember = "Value"; payoutMethod.DataSource = methods; payoutMethod.Dock = DockStyle.Fill;
            AddRow(table, 0, "Ngày chi", payoutDate);
            AddRow(table, 1, "Phương thức chi", payoutMethod);
            Button payout = new Button { Text = "Tạo phiếu chi", Dock = DockStyle.Bottom, Height = 36 };
            payout.Click += CreatePayout;
            box.Controls.Add(table);
            box.Controls.Add(payout);
            return box;
        }

        private void LoadLookups()
        {
            try
            {
                Bind(employeeCombo, service.GetEmployees(), "MaNhanVien", "HienThi");
                Bind(departmentCombo, service.GetDepartments(), "MaPhongBan", "HienThi");
                RefreshDecisions(); UpdateTargetControls();
            }
            catch (Exception ex) { MessageBox.Show(this, "Không tải được dữ liệu danh mục. Kiểm tra SQL Server/App.config.\r\n" + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void CreateDraft(object sender, EventArgs e)
        {
            try
            {
                string type = SelectedTargetType;
                string id = service.CreateDraft(type,
                    type == "CaNhan" ? Convert.ToString(employeeCombo.SelectedValue) : null,
                    type == "TapThe" ? Convert.ToString(departmentCombo.SelectedValue) : null,
                    dateCreated.Value.Date, reason.Text, content.Text, amount.Value);
                MessageBox.Show(this, "Đã tạo quyết định " + id + " ở trạng thái Chờ ký.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                reason.Clear(); content.Clear(); RefreshDecisions();
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void SignDecision(object sender, EventArgs e)
        {
            try
            {
                if (decisions.SelectedValue == null) throw new InvalidOperationException("Chọn một quyết định.");
                string id = Convert.ToString(decisions.SelectedValue);
                service.Sign(id, signDate.Value.Date);
                MessageBox.Show(this, "Đã ghi nhận ký quyết định: " + id, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshDecisions();
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void CreatePayout(object sender, EventArgs e)
        {
            try
            {
                if (decisions.SelectedValue == null) throw new InvalidOperationException("Chọn quyết định khen thưởng.");
                string id = service.CreatePayout(Convert.ToString(decisions.SelectedValue), payoutDate.Value.Date, Convert.ToString(payoutMethod.SelectedValue));
                MessageBox.Show(this, "Đã tạo phiếu chi: " + id, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void RefreshDecisions()
        {
            Bind(decisions, service.GetAwardDecisions(), "MaQDKhenThuong", "HienThi");
        }

        private void UpdateTargetControls()
        {
            bool individual = SelectedTargetType == "CaNhan";
            employeeCombo.Enabled = individual;
            departmentCombo.Enabled = !individual;
        }

        private string SelectedTargetType
        {
            get { Choice item = targetType.SelectedItem as Choice; return item == null ? "CaNhan" : item.Value; }
        }

        private void ShowError(Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Không thể thực hiện nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private static TableLayoutPanel NewTable(int rows)
        {
            TableLayoutPanel table = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = rows, Padding = new Padding(4) };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72));
            for (int i = 0; i < rows; i++) table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            return table;
        }
        private static void AddRow(TableLayoutPanel table, int row, string label, Control control)
        {
            table.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left, Padding = new Padding(0, 5, 0, 0) }, 0, row);
            control.Margin = new Padding(3, 4, 3, 4); control.Dock = DockStyle.Fill;
            table.Controls.Add(control, 1, row);
        }
        private static void Configure(ComboBox combo) { combo.DropDownStyle = ComboBoxStyle.DropDownList; combo.Dock = DockStyle.Fill; }
        private static void Bind(ComboBox combo, DataTable table, string valueMember, string displayMember)
        {
            combo.DataSource = null; combo.DisplayMember = displayMember; combo.ValueMember = valueMember; combo.DataSource = table;
        }

        private sealed class Choice
        {
            public string Value { get; private set; }
            public string Text { get; private set; }
            public Choice(string value, string text) { Value = value; Text = text; }
        }
    }
}
