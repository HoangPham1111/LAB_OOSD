using System;
using System.Drawing;
using System.Windows.Forms;
namespace QuanLyChoThueVanPhong.Forms
{
    public class FrmMain:Form
    {
        public FrmMain()
        {
            var body=Ui.Shell(this,"Hệ thống quản lý cho thuê văn phòng cao ốc");
            var table=new TableLayoutPanel { Dock=DockStyle.Fill,ColumnCount=3,RowCount=3,
                Padding=new Padding(40,45,40,35),BackColor=Color.White };
            for(int i=0;i<3;i++) { table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100F/3));table.RowStyles.Add(new RowStyle(SizeType.Percent,100F/3)); }
            AddMenu(table,"1. DANH MỤC",0,0,()=>new FrmDanhMuc());
            AddMenu(table,"2. TẦNG - VĂN PHÒNG",1,0,()=>new FrmVanPhong());
            AddMenu(table,"3. HỢP ĐỒNG THUÊ",2,0,()=>new FrmHopDong());
            AddMenu(table,"4. DỊCH VỤ",0,1,()=>new FrmDichVu());
            AddMenu(table,"5. HÓA ĐƠN - THANH TOÁN",1,1,()=>new FrmThanhToan());
            AddMenu(table,"6. PHIẾU GIA HẠN",2,1,()=>new FrmGiaHan());
            AddMenu(table,"7. NHÂN VIÊN - THẺ XE",0,2,()=>new FrmNhanVienTheXe());
            AddMenu(table,"8. KIỂM TRA VĂN PHÒNG",1,2,()=>new FrmKiemTraPhong());
            AddMenu(table,"9. THỐNG KÊ - CÔNG NỢ",2,2,()=>new FrmThongKe());
            body.Controls.Add(table);
        }
        private void AddMenu(TableLayoutPanel table,string label,int col,int row,Func<Form> factory)
        {
            var button=new Button { Text=label,Dock=DockStyle.Fill,Margin=new Padding(13,10,13,10),
                BackColor=Color.FromArgb(236,244,251),ForeColor=Ui.Accent,FlatStyle=FlatStyle.Flat,
                Font=new Font("Segoe UI Semibold",12F,FontStyle.Bold),Cursor=Cursors.Hand };
            button.FlatAppearance.BorderColor=Color.FromArgb(161,190,215);
            button.Click+=(s,e)=>Ui.Safe(()=>{using(var form=factory())form.ShowDialog(this);});
            table.Controls.Add(button,col,row);
        }
    }
}
