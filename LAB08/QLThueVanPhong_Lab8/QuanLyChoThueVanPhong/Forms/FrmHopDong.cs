using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using QuanLyChoThueVanPhong.Services;
namespace QuanLyChoThueVanPhong.Forms
{
    public class FrmHopDong:Form
    {
        public FrmHopDong()
        {
            var body=Ui.Shell(this,"Lập hợp đồng thuê nhiều văn phòng");
            var top=Ui.EntryPanel(155);var mid=new SplitContainer { Dock=DockStyle.Fill,Orientation=Orientation.Vertical,SplitterDistance=410 };
            body.Controls.Add(mid);body.Controls.Add(top);
            var tenant=Ui.Combo();var manager=Ui.Combo();var date=Ui.Date(new DateTime(DateTime.Today.Year,DateTime.Today.Month,1).AddMonths(1));
            var months=Ui.Number(120,0,6);months.Minimum=6;
            var list=new CheckedListBox { Dock=DockStyle.Fill,CheckOnClick=true,IntegralHeight=false };
            var grid=Ui.Grid();
            mid.Panel1.Controls.Add(list);mid.Panel1.Controls.Add(new Label{Text="Chọn một hoặc nhiều văn phòng (giữ Ctrl không cần thiết, tích ô để chọn)",Height=34,Dock=DockStyle.Top});
            mid.Panel2.Controls.Add(grid);
            top.Controls.Add(Ui.Field("Công ty thuê",tenant,255));top.Controls.Add(Ui.Field("Nhân viên lập HĐ",manager,220));
            top.Controls.Add(Ui.Field("Ngày bắt đầu (ngày 01)",date,200));top.Controls.Add(Ui.Field("Thời hạn gốc (tháng)",months,180));
            Action refresh=()=>{
                Ui.Bind(tenant,CatalogService.Tenants(),"MaKhach");Ui.Bind(manager,CatalogService.Managers(),"MaQL");
                var rooms=CatalogService.AvailableRooms();list.Items.Clear();
                foreach(DataRow row in rooms.Rows)list.Items.Add(new RoomChoice(Convert.ToInt32(row["MaVanPhong"]),Convert.ToString(row["TenHienThi"])));
                grid.DataSource=CatalogService.Contracts();
            };
            top.Controls.Add(Ui.Button("Lập HĐ + 6 tháng trả trước",()=>{
                var ids=new List<int>();foreach(var item in list.CheckedItems)ids.Add(((RoomChoice)item).Id);
                int id=ContractService.Create(Ui.SelectedId(tenant),Ui.SelectedId(manager),date.Value,(int)months.Value,ids);
                grid.DataSource=CatalogService.Contracts();
                Ui.Success("Đã lập HĐ"+id+" và tạo hóa đơn trả trước đúng 6 tháng. Vào HÓA ĐƠN - THANH TOÁN để thu tiền, khi trả đủ hợp đồng mới có hiệu lực.");
            },260));
            top.Controls.Add(Ui.Button("Tải lại danh sách",refresh,155));
            Ui.Safe(refresh);
        }
        private class RoomChoice
        {
            public readonly int Id;private readonly string caption;
            public RoomChoice(int id,string text){Id=id;caption=text;}
            public override string ToString(){return caption;}
        }
    }
}
