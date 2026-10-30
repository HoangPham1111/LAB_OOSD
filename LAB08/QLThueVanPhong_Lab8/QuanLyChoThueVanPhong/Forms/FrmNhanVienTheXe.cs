using System;
using System.Windows.Forms;
using QuanLyChoThueVanPhong.Services;
namespace QuanLyChoThueVanPhong.Forms
{
    public class FrmNhanVienTheXe:Form
    {
        public FrmNhanVienTheXe()
        {
            var body=Ui.Shell(this,"Nhân viên làm việc và thẻ giữ xe");
            var tabs=new TabControl{Dock=DockStyle.Fill};body.Controls.Add(tabs);
            var ep=new TabPage("Hồ sơ nhân viên công ty thuê");tabs.TabPages.Add(ep);
            var eb=Ui.EntryPanel(166);var eg=Ui.Grid();Ui.Add(ep,eb,eg);
            var tenant=Ui.Combo();var room=Ui.Combo();var fullName=Ui.Text();var title=Ui.Text();var cccd=Ui.Text();var photo=Ui.Text();
            eb.Controls.Add(Ui.Field("Công ty",tenant,235));eb.Controls.Add(Ui.Field("Văn phòng",room,195));
            eb.Controls.Add(Ui.Field("Họ tên",fullName,195));eb.Controls.Add(Ui.Field("Chức vụ",title,160));
            eb.Controls.Add(Ui.Field("CCCD",cccd,170));eb.Controls.Add(Ui.Field("Đường dẫn ảnh",photo,220));
            Action refreshEmployees=()=>{
                Ui.Bind(tenant,CatalogService.Tenants(),"MaKhach");Ui.Bind(room,CatalogService.Rooms(),"MaVanPhong");eg.DataSource=PeopleService.Employees();};
            eb.Controls.Add(Ui.Button("Thêm nhân viên",()=>{
                PeopleService.AddEmployee(Ui.SelectedId(tenant),Ui.SelectedId(room),fullName.Text.Trim(),title.Text.Trim(),cccd.Text.Trim(),photo.Text.Trim());
                eg.DataSource=PeopleService.Employees();Ui.Success("Đã lưu hồ sơ nhân viên làm việc.");
            },190));
            var cp=new TabPage("Cấp thẻ giữ xe");tabs.TabPages.Add(cp);
            var cb=Ui.EntryPanel(130);var cg=Ui.Grid();Ui.Add(cp,cb,cg);
            var employee=Ui.Combo();var plate=Ui.Text();var vehicle=Ui.Text("Xe máy");
            cb.Controls.Add(Ui.Field("Nhân viên",employee,275));cb.Controls.Add(Ui.Field("Biển số",plate,200));
            cb.Controls.Add(Ui.Field("Loại xe",vehicle,200));
            Action refreshCards=()=>{Ui.Bind(employee,PeopleService.Employees(),"MaNhanVien");cg.DataSource=PeopleService.Cards();};
            cb.Controls.Add(Ui.Button("Cấp thẻ xe",()=>{
                PeopleService.AddCard(Ui.SelectedId(employee),plate.Text.Trim().ToUpperInvariant(),vehicle.Text.Trim());
                cg.DataSource=PeopleService.Cards();Ui.Success("Đã cấp thẻ giữ xe.");
            },175));
            tabs.SelectedIndexChanged+=(s,e)=>{if(tabs.SelectedTab==cp)Ui.Safe(refreshCards);};
            Ui.Safe(refreshEmployees);
        }
    }
}
