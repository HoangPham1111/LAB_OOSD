using System;
using System.Windows.Forms;
using QuanLyChoThueVanPhong.Services;
namespace QuanLyChoThueVanPhong.Forms
{
    public class FrmDichVu:Form
    {
        public FrmDichVu()
        {
            var body=Ui.Shell(this,"Ghi nhận sử dụng dịch vụ và lập hóa đơn cuối tháng");
            var tabs=new TabControl{Dock=DockStyle.Top,Height=170};var grid=Ui.Grid();body.Controls.Add(grid);body.Controls.Add(tabs);
            var usePage=new TabPage("Ghi nhận dịch vụ theo văn phòng / ngày");var useBar=Ui.EntryPanel(118);
            usePage.Controls.Add(useBar);tabs.TabPages.Add(usePage);
            var detail=Ui.Combo();var service=Ui.Combo();var date=Ui.Date();var qty=Ui.Number(999999,2,1);qty.Minimum=0.01M;
            useBar.Controls.Add(Ui.Field("Chi tiết hợp đồng / phòng",detail,250));useBar.Controls.Add(Ui.Field("Dịch vụ",service,205));
            useBar.Controls.Add(Ui.Field("Ngày sử dụng",date,185));useBar.Controls.Add(Ui.Field("Số lượng",qty,150));
            var billPage=new TabPage("Chốt hóa đơn vào cuối tháng");var billBar=Ui.EntryPanel(118);
            billPage.Controls.Add(billBar);tabs.TabPages.Add(billPage);
            var contract=Ui.Combo();var month=Ui.Number(12,0,DateTime.Today.Month);month.Minimum=1;
            var year=Ui.Number(2100,0,DateTime.Today.Year);year.Minimum=2000;var asOf=Ui.Date();
            billBar.Controls.Add(Ui.Field("Hợp đồng",contract,240));billBar.Controls.Add(Ui.Field("Tháng",month,115));
            billBar.Controls.Add(Ui.Field("Năm",year,115));billBar.Controls.Add(Ui.Field("Ngày lập (mô phỏng)",asOf,230));
            Action refresh=()=>{
                Ui.Bind(detail,CatalogService.ContractRooms(),"MaCTHD");Ui.Bind(service,CatalogService.Services(),"MaDichVu");
                Ui.Bind(contract,CatalogService.Contracts(),"MaHopDong");grid.DataSource=BillingService.Usage();
            };
            useBar.Controls.Add(Ui.Button("Ghi dịch vụ",()=>{
                BillingService.LogService(Ui.SelectedId(detail),Ui.SelectedId(service),date.Value,qty.Value);
                grid.DataSource=BillingService.Usage();Ui.Success("Đã lưu dịch vụ. Nếu trùng ngày/phòng/dịch vụ thì cộng dồn số lượng.");
            },180));
            billBar.Controls.Add(Ui.Button("Lập hóa đơn dịch vụ",()=>{
                int id=BillingService.BillServices(Ui.SelectedId(contract),(int)month.Value,(int)year.Value,asOf.Value);
                Ui.Success("Đã lập hóa đơn dịch vụ "+id+". Hạn đến cuối tháng. Vào HÓA ĐƠN - THANH TOÁN để thu phí.");
            },220));
            useBar.Controls.Add(Ui.Button("Tải lại",refresh,115));
            Ui.Safe(refresh);
        }
    }
}
