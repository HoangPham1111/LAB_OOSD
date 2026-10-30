using System;
using System.Windows.Forms;
using QuanLyChoThueVanPhong.Services;
namespace QuanLyChoThueVanPhong.Forms
{
    public class FrmKiemTraPhong:Form
    {
        public FrmKiemTraPhong()
        {
            var body=Ui.Shell(this,"Kiểm tra phòng định kỳ và ghi nhận vi phạm");
            var tab=new TabControl{Dock=DockStyle.Fill};body.Controls.Add(tab);
            var historyPage=new TabPage("Ghi nhận lần kiểm tra");tab.TabPages.Add(historyPage);
            var bar=Ui.EntryPanel(174);var grid=Ui.Grid();Ui.Add(historyPage,bar,grid);
            var room=Ui.Combo();var manager=Ui.Combo();var date=Ui.Date();var state=Ui.Text("Bình thường");var violation=new CheckBox{Text="Có tự ý sửa kết cấu"};
            var penalty=Ui.Number(1000000000,2);var note=Ui.Text();
            bar.Controls.Add(Ui.Field("Văn phòng",room,195));bar.Controls.Add(Ui.Field("Người kiểm tra",manager,190));
            bar.Controls.Add(Ui.Field("Ngày kiểm tra",date,165));bar.Controls.Add(Ui.Field("Tình trạng",state,215));
            bar.Controls.Add(Ui.Field("Vi phạm kết cấu",violation,185));bar.Controls.Add(Ui.Field("Tiền phạt xác nhận",penalty,190));
            bar.Controls.Add(Ui.Field("Ghi chú",note,220));
            Action refresh=()=>{Ui.Bind(room,CatalogService.Rooms(),"MaVanPhong");Ui.Bind(manager,CatalogService.Managers(),"MaQL");grid.DataSource=InspectionService.Inspections();};
            bar.Controls.Add(Ui.Button("Lưu biên bản kiểm tra",()=>{
                InspectionService.Add(Ui.SelectedId(room),Ui.SelectedId(manager),date.Value,state.Text.Trim(),violation.Checked,penalty.Value,note.Text.Trim());
                grid.DataSource=InspectionService.Inspections();Ui.Success("Đã ghi nhận kiểm tra văn phòng.");
            },245));
            var duePage=new TabPage("Theo dõi phòng đến hạn kiểm tra 1 năm");tab.TabPages.Add(duePage);
            var dueGrid=Ui.Grid();duePage.Controls.Add(dueGrid);
            tab.SelectedIndexChanged+=(s,e)=>{if(tab.SelectedTab==duePage)Ui.Safe(()=>dueGrid.DataSource=InspectionService.Due());};
            Ui.Safe(refresh);
        }
    }
}
