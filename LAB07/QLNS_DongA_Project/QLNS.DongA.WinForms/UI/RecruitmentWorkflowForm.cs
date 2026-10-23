using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QLNS.DongA.Services;

namespace QLNS.DongA.UI
{
    internal sealed class RecruitmentWorkflowForm : Form
    {
        private readonly RecruitmentService service = new RecruitmentService();
        private readonly ComboBox employeeCombo = new ComboBox();
        private readonly ComboBox departmentCombo = new ComboBox();
        private readonly DateTimePicker signTrial = DatePicker();
        private readonly DateTimePicker startTrial = DatePicker();
        private readonly DateTimePicker endTrial = DatePicker();
        private readonly NumericUpDown trialSalary = MoneyControl();
        private readonly TextBox employerTerms = new TextBox();
        private readonly TextBox candidateTerms = new TextBox();
        private readonly ComboBox trialDecisionCombo = new ComboBox();
        private readonly ComboBox contractTypeCombo = new ComboBox();
        private readonly DateTimePicker signContract = DatePicker();
        private readonly DateTimePicker startContract = DatePicker();
        private readonly DateTimePicker endContract = DatePicker();
        private readonly CheckBox noEndDate = new CheckBox { Text = "Không xác định ngày kết thúc (thường dùng với dài hạn)", AutoSize = true };

        public RecruitmentWorkflowForm()
        {
            Text = "Quy trình tuyển dụng - thử việc - hợp đồng";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1180, 760);
            MinimumSize = new Size(1050, 680);
            Font = new Font("Segoe UI", 9.5F);

            SplitContainer split = new SplitContainer { Size = new Size(1140, 680), Orientation = Orientation.Vertical, Padding = new Padding(8) };
            split.SplitterDistance = 555;
            split.Dock = DockStyle.Fill;
            split.Panel1.Controls.Add(BuildTrialPanel());
            split.Panel2.Controls.Add(BuildContractPanel());
            Label footer = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 52,
                Text = "Với hợp đồng dài hạn, hệ thống tạo hồ sơ BHYT/BHXH ở trạng thái 'Chờ cập nhật mức đóng'. Hãy nhập mức đóng đã được xác nhận và chuyển trạng thái sang 'Đang tham gia' trước khi tính lương.",
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.DarkSlateGray,
                Padding = new Padding(8)
            };
            Controls.Add(split);
            Controls.Add(footer);
            Shown += delegate { LoadLookups(); };
            contractTypeCombo.SelectedIndexChanged += delegate { UpdateEndDateState(); };
            noEndDate.CheckedChanged += delegate { endContract.Enabled = !noEndDate.Checked; };
            UpdateEndDateState();
        }

        private Control BuildTrialPanel()
        {
            GroupBox group = new GroupBox { Text = "A. Tiếp nhận vào thử việc", Dock = DockStyle.Fill, Padding = new Padding(10) };
            TableLayoutPanel table = NewTable(8);
            ConfigureCombo(employeeCombo); ConfigureCombo(departmentCombo);
            employerTerms.Multiline = true; employerTerms.Height = 65; employerTerms.ScrollBars = ScrollBars.Vertical;
            candidateTerms.Multiline = true; candidateTerms.Height = 65; candidateTerms.ScrollBars = ScrollBars.Vertical;
            AddRow(table, 0, "Nhân viên", employeeCombo);
            AddRow(table, 1, "Phòng ban", departmentCombo);
            AddRow(table, 2, "Ngày ký QĐ", signTrial);
            AddRow(table, 3, "Bắt đầu thử việc", startTrial);
            AddRow(table, 4, "Kết thúc thử việc", endTrial);
            AddRow(table, 5, "Lương thử việc (VNĐ)", trialSalary);
            AddRow(table, 6, "Điều khoản nhà tuyển dụng", employerTerms);
            AddRow(table, 7, "Điều khoản ứng viên", candidateTerms);
            Button create = new Button { Text = "Lập quyết định thử việc", Height = 38, Dock = DockStyle.Bottom };
            create.Click += CreateTrialDecision;
            group.Controls.Add(table);
            group.Controls.Add(create);
            return group;
        }

        private Control BuildContractPanel()
        {
            GroupBox group = new GroupBox { Text = "B. Ký hợp đồng chính thức sau thử việc", Dock = DockStyle.Fill, Padding = new Padding(10) };
            TableLayoutPanel table = NewTable(6);
            ConfigureCombo(trialDecisionCombo);
            DataTable contractTypes = new DataTable();
            contractTypes.Columns.Add("Loai", typeof(string)); contractTypes.Columns.Add("HienThi", typeof(string));
            contractTypes.Rows.Add("DaiHan", "Hợp đồng dài hạn");
            contractTypes.Rows.Add("NganHan", "Hợp đồng ngắn hạn");
            contractTypeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            contractTypeCombo.DisplayMember = "HienThi"; contractTypeCombo.ValueMember = "Loai"; contractTypeCombo.DataSource = contractTypes;
            AddRow(table, 0, "Quyết định thử việc", trialDecisionCombo);
            AddRow(table, 1, "Loại hợp đồng", contractTypeCombo);
            AddRow(table, 2, "Ngày ký hợp đồng", signContract);
            AddRow(table, 3, "Ngày bắt đầu", startContract);
            AddRow(table, 4, "Ngày kết thúc", endContract);
            AddRow(table, 5, "Tùy chọn", noEndDate);
            Button create = new Button { Text = "Ký hợp đồng chính thức", Height = 38, Dock = DockStyle.Bottom };
            create.Click += CreateFormalContract;
            group.Controls.Add(table);
            group.Controls.Add(create);
            return group;
        }

        private void LoadLookups()
        {
            try
            {
                Bind(employeeCombo, service.GetEmployees(), "MaNhanVien", "HienThi");
                Bind(departmentCombo, service.GetDepartments(), "MaPhongBan", "HienThi");
                RefreshTrialDecisions();
            }
            catch (Exception ex) { MessageBox.Show(this, "Không tải được danh mục. Hãy chạy script SQL và kiểm tra App.config.\r\n" + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void CreateTrialDecision(object sender, EventArgs e)
        {
            try
            {
                if (employeeCombo.SelectedValue == null || departmentCombo.SelectedValue == null) throw new InvalidOperationException("Cần có nhân viên và phòng ban hợp lệ.");
                if (String.IsNullOrWhiteSpace(employerTerms.Text) || String.IsNullOrWhiteSpace(candidateTerms.Text)) throw new InvalidOperationException("Cần nhập điều khoản của nhà tuyển dụng và ứng viên.");
                string id = service.CreateTrialDecision(Convert.ToString(employeeCombo.SelectedValue), Convert.ToString(departmentCombo.SelectedValue),
                    signTrial.Value.Date, startTrial.Value.Date, endTrial.Value.Date, trialSalary.Value,
                    employerTerms.Text.Trim(), candidateTerms.Text.Trim());
                MessageBox.Show(this, "Đã lập quyết định thử việc: " + id, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshTrialDecisions();
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void CreateFormalContract(object sender, EventArgs e)
        {
            try
            {
                if (trialDecisionCombo.SelectedValue == null) throw new InvalidOperationException("Chưa có quyết định thử việc đang chờ ký hợp đồng.");
                string type = Convert.ToString(contractTypeCombo.SelectedValue);
                DateTime? end = noEndDate.Checked ? (DateTime?)null : endContract.Value.Date;
                string id = service.CreateFormalContract(Convert.ToString(trialDecisionCombo.SelectedValue), type,
                    signContract.Value.Date, startContract.Value.Date, end);
                MessageBox.Show(this, "Đã tạo hợp đồng chính thức: " + id + (type == "DaiHan" ? "\r\nĐã tạo hồ sơ BHYT/BHXH chờ cập nhật mức đóng." : ""),
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshTrialDecisions();
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void RefreshTrialDecisions()
        {
            Bind(trialDecisionCombo, service.GetTrialDecisions(), "MaQDTuyenDung", "HienThi");
        }

        private void UpdateEndDateState()
        {
            if (Convert.ToString(contractTypeCombo.SelectedValue) == "DaiHan") noEndDate.Checked = true;
            else if (Convert.ToString(contractTypeCombo.SelectedValue) == "NganHan") noEndDate.Checked = false;
            endContract.Enabled = !noEndDate.Checked;
        }

        private void ShowError(Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Không thể thực hiện nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private static DateTimePicker DatePicker()
        {
            return new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy", Dock = DockStyle.Fill };
        }

        private static NumericUpDown MoneyControl()
        {
            return new NumericUpDown { Minimum = 0, Maximum = 1000000000, DecimalPlaces = 2, ThousandsSeparator = true, Dock = DockStyle.Fill, Value = 5000000 };
        }

        private static void ConfigureCombo(ComboBox combo)
        {
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.Dock = DockStyle.Fill;
        }

        private static void Bind(ComboBox combo, DataTable table, string valueMember, string displayMember)
        {
            combo.DataSource = null;
            combo.DisplayMember = displayMember;
            combo.ValueMember = valueMember;
            combo.DataSource = table;
        }

        private static TableLayoutPanel NewTable(int rows)
        {
            TableLayoutPanel table = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = rows, AutoScroll = true, Padding = new Padding(4) };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
            for (int i = 0; i < rows; i++) table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            return table;
        }

        private static void AddRow(TableLayoutPanel table, int row, string label, Control control)
        {
            table.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left, Padding = new Padding(0, 5, 0, 0) }, 0, row);
            control.Margin = new Padding(3, 4, 3, 4);
            control.Dock = DockStyle.Fill;
            table.Controls.Add(control, 1, row);
        }
    }
}
