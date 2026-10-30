using System;
using System.Windows.Forms;
using QuanLyChoThueVanPhong.Services;
namespace QuanLyChoThueVanPhong.Forms
{
    public class FrmDanhMuc:Form
    {
        public FrmDanhMuc()
        {
            var body=Ui.Shell(this,"Danh mục dữ liệu nền");
            var tabs=new TabControl{Dock=DockStyle.Fill};body.Controls.Add(tabs);
            Floors(tabs);Tenants(tabs);Services(tabs);Managers(tabs);
        }
        private void Floors(TabControl tabs)
        {
            var tab=new TabPage("Tầng cao ốc");var bar=Ui.EntryPanel(92);var grid=Ui.Grid();
            var so=Ui.Number(500);var ten=Ui.Text();
            bar.Controls.Add(Ui.Field("Số tầng",so,140));bar.Controls.Add(Ui.Field("Tên tầng",ten,240));
            Action refresh=()=>grid.DataSource=CatalogService.Floors();
            bar.Controls.Add(Ui.Button("Thêm tầng",()=>{CatalogService.AddFloor((int)so.Value,ten.Text.Trim());refresh();Ui.Success("Đã thêm tầng.");}));
            Ui.Add(tab,bar,grid);tabs.TabPages.Add(tab);Ui.Safe(refresh);
        }
        private void Tenants(TabControl tabs)
        {
            var tab=new TabPage("Khách thuê / công ty");var bar=Ui.EntryPanel(160);var grid=Ui.Grid();
            var ten=Ui.Text();var mst=Ui.Text();var dai=Ui.Text();var sdt=Ui.Text();var email=Ui.Text();
            bar.Controls.Add(Ui.Field("Tên công ty",ten,250));bar.Controls.Add(Ui.Field("Mã số thuế",mst,160));
            bar.Controls.Add(Ui.Field("Người đại diện",dai,220));bar.Controls.Add(Ui.Field("Điện thoại",sdt,175));
            bar.Controls.Add(Ui.Field("Email",email,235));
            Action refresh=()=>grid.DataSource=CatalogService.Tenants();
            bar.Controls.Add(Ui.Button("Thêm khách thuê",()=>{CatalogService.AddTenant(ten.Text.Trim(),mst.Text.Trim(),dai.Text.Trim(),sdt.Text.Trim(),email.Text.Trim());refresh();Ui.Success("Đã thêm khách thuê.");}));
            Ui.Add(tab,bar,grid);tabs.TabPages.Add(tab);Ui.Safe(refresh);
        }
        private void Services(TabControl tabs)
        {
            var tab=new TabPage("Dịch vụ");var bar=Ui.EntryPanel(92);var grid=Ui.Grid();
            var ten=Ui.Text();var donVi=Ui.Text();var gia=Ui.Number(1000000000,2);
            bar.Controls.Add(Ui.Field("Tên dịch vụ",ten,220));bar.Controls.Add(Ui.Field("Đơn vị tính",donVi,150));bar.Controls.Add(Ui.Field("Đơn giá",gia,195));
            Action refresh=()=>grid.DataSource=CatalogService.Services();
            bar.Controls.Add(Ui.Button("Thêm dịch vụ",()=>{CatalogService.AddService(ten.Text.Trim(),donVi.Text.Trim(),gia.Value);refresh();Ui.Success("Đã thêm dịch vụ.");}));
            Ui.Add(tab,bar,grid);tabs.TabPages.Add(tab);Ui.Safe(refresh);
        }
        private void Managers(TabControl tabs)
        {
            var tab=new TabPage("Nhân viên quản lý cao ốc");var bar=Ui.EntryPanel(92);var grid=Ui.Grid();
            var ten=Ui.Text();var role=Ui.Combo();role.Items.AddRange(new object[]{"QUAN_LY","KE_TOAN","KY_THUAT"});role.SelectedIndex=0;
            bar.Controls.Add(Ui.Field("Họ tên",ten,260));bar.Controls.Add(Ui.Field("Vai trò",role,190));
            Action refresh=()=>grid.DataSource=CatalogService.Managers();
            bar.Controls.Add(Ui.Button("Thêm nhân viên",()=>{CatalogService.AddManager(ten.Text.Trim(),role.Text);refresh();Ui.Success("Đã thêm nhân viên quản lý.");}));
            Ui.Add(tab,bar,grid);tabs.TabPages.Add(tab);Ui.Safe(refresh);
        }
    }
}
