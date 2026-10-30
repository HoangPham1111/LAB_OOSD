using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyChoThueVanPhong.Forms
{
    public static class Ui
    {
        public static readonly Color Accent=Color.FromArgb(20,91,153);
        public static readonly Color Back=Color.FromArgb(245,248,251);
        public static Panel Shell(Form form,string caption)
        {
            form.Text=caption+" | Hệ thống quản lý cho thuê văn phòng";
            form.StartPosition=FormStartPosition.CenterScreen;
            form.Size=new Size(1140,760);
            form.MinimumSize=new Size(920,620);
            form.Font=new Font("Segoe UI",10F);
            form.BackColor=Back;
            var body=new Panel { Dock=DockStyle.Fill,Padding=new Padding(16) };
            var banner=new Panel { Dock=DockStyle.Top,Height=80,BackColor=Color.White,Padding=new Padding(24,13,20,8) };
            var lbl=new Label { Text=caption.ToUpperInvariant(),AutoSize=true,Font=new Font("Segoe UI Semibold",18F,FontStyle.Bold),ForeColor=Accent,Location=new Point(20,10) };
            var sub=new Label { Text="QUẢN LÝ CAO ỐC  •  BÀI 4",AutoSize=true,Font=new Font("Segoe UI",8.7F),ForeColor=Color.DimGray,Location=new Point(22,47) };
            banner.Controls.Add(lbl);banner.Controls.Add(sub);
            var footer=new Label { Text="",Dock=DockStyle.Bottom,Height=26,TextAlign=ContentAlignment.MiddleCenter,ForeColor=Color.DimGray,BackColor=Color.White,Font=new Font("Segoe UI",8.5F) };
            form.Controls.Add(body);form.Controls.Add(footer);form.Controls.Add(banner);
            return body;
        }
        public static FlowLayoutPanel EntryPanel(int height=135)
        {
            return new FlowLayoutPanel { Dock=DockStyle.Top,Height=height,AutoScroll=true,WrapContents=true,
                Padding=new Padding(8),BackColor=Color.White,FlowDirection=FlowDirection.LeftToRight };
        }
        public static Control Field(string label,Control control,int width=185)
        {
            var host=new Panel { Width=width,Height=63,Margin=new Padding(7,4,7,3) };
            var lbl=new Label { Text=label,Left=3,Top=1,Width=width-8,Height=23,ForeColor=Color.FromArgb(53,69,83),Font=new Font("Segoe UI Semibold",9F) };
            control.Left=3;control.Top=25;control.Width=width-8;control.Height=29;
            control.Anchor=AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Right;
            host.Controls.Add(lbl);host.Controls.Add(control);return host;
        }
        public static TextBox Text(string value="") { return new TextBox { Text=value,BorderStyle=BorderStyle.FixedSingle }; }
        public static NumericUpDown Number(decimal max=1000000000M,int decimals=0,decimal start=0)
        { return new NumericUpDown { Minimum=0,Maximum=max,DecimalPlaces=decimals,Value=start,ThousandsSeparator=true }; }
        public static DateTimePicker Date(DateTime? value=null)
        { return new DateTimePicker { Format=DateTimePickerFormat.Custom,CustomFormat="dd/MM/yyyy",Value=value??DateTime.Today }; }
        public static ComboBox Combo() { return new ComboBox { DropDownStyle=ComboBoxStyle.DropDownList }; }
        public static void Bind(ComboBox cbo,DataTable dt,string valueColumn)
        { cbo.DataSource=dt;cbo.DisplayMember="TenHienThi";cbo.ValueMember=valueColumn; }
        public static int SelectedId(ComboBox cbo)
        {
            if(cbo.SelectedValue==null || cbo.SelectedValue is DataRowView)
                throw new InvalidOperationException("Hãy chọn dữ liệu trong danh sách.");
            return Convert.ToInt32(cbo.SelectedValue);
        }
        public static Button Button(string text,Action click,int width=195)
        {
            var b=new Button { Text=text,Width=width,Height=37,Margin=new Padding(8,16,8,4),BackColor=Accent,
                ForeColor=Color.White,FlatStyle=FlatStyle.Flat,Cursor=Cursors.Hand,Font=new Font("Segoe UI Semibold",9.2F) };
            b.FlatAppearance.BorderSize=0;
            b.Click+=(s,e)=>Safe(click);
            return b;
        }
        public static DataGridView Grid()
        {
            return new DataGridView { Dock=DockStyle.Fill,ReadOnly=true,AllowUserToAddRows=false,
                AllowUserToDeleteRows=false,AllowUserToResizeRows=false,SelectionMode=DataGridViewSelectionMode.FullRowSelect,
                MultiSelect=false,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor=Color.White,BorderStyle=BorderStyle.None,
                RowHeadersVisible=false,RowTemplate={Height=29},ColumnHeadersHeight=36,
                EnableHeadersVisualStyles=false,ColumnHeadersDefaultCellStyle={BackColor=Accent,ForeColor=Color.White,
                    Font=new Font("Segoe UI Semibold",9.2F)} };
        }
        public static void Safe(Action action)
        {
            try { action(); }
            catch (Exception ex) { MessageBox.Show(ex.Message,"Thông báo nghiệp vụ",MessageBoxButtons.OK,MessageBoxIcon.Warning); }
        }
        public static void Success(string detail)
        { MessageBox.Show(detail,"Thành công",MessageBoxButtons.OK,MessageBoxIcon.Information); }
        public static Control Add(TabPage page,Control input,DataGridView grid)
        { page.Controls.Add(grid);page.Controls.Add(input);return page; }
        public static int SelectedGridId(DataGridView grid,string col)
        {
            if(grid.CurrentRow==null) throw new InvalidOperationException("Hãy chọn một dòng dữ liệu.");
            return Convert.ToInt32(grid.CurrentRow.Cells[col].Value);
        }
    }
}
