using System;
using System.Windows.Forms;
using QuanLyChoThueVanPhong.Services;
namespace QuanLyChoThueVanPhong.Forms
{
    public class FrmVanPhong:Form
    {
        public FrmVanPhong()
        {
            var body=Ui.Shell(this,"Quản lý tầng và văn phòng");
            var bar=Ui.EntryPanel(153);var grid=Ui.Grid();body.Controls.Add(grid);body.Controls.Add(bar);
            var floor=Ui.Combo();var room=Ui.Text();var area=Ui.Number(100000,2);var dir=Ui.Text("Đông");var price=Ui.Number(100000000000,2);
            bar.Controls.Add(Ui.Field("Tầng",floor,160));bar.Controls.Add(Ui.Field("Mã / số phòng",room,160));
            bar.Controls.Add(Ui.Field("Diện tích (m²)",area,155));bar.Controls.Add(Ui.Field("Hướng phòng",dir,150));
            bar.Controls.Add(Ui.Field("Giá thuê/tháng (đ)",price,200));
            Action refresh=()=>{Ui.Bind(floor,CatalogService.Floors(),"MaTang");grid.DataSource=CatalogService.Rooms();};
            bar.Controls.Add(Ui.Button("Lưu văn phòng",()=>{CatalogService.AddRoom(Ui.SelectedId(floor),room.Text.Trim(),area.Value,dir.Text.Trim(),price.Value);refresh();Ui.Success("Đã lưu văn phòng mới.");}));
            bar.Controls.Add(Ui.Button("Làm mới",refresh,130));Ui.Safe(refresh);
        }
    }
}
