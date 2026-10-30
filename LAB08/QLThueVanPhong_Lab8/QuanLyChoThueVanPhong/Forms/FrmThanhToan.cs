using System;
using System.Windows.Forms;
using QuanLyChoThueVanPhong.Services;
namespace QuanLyChoThueVanPhong.Forms
{
    public class FrmThanhToan:Form
    {
        public FrmThanhToan()
        {
            var body=Ui.Shell(this,"Hóa đơn và thanh toán - công nợ");
            var grid=Ui.Grid();var tab=new TabControl{Dock=DockStyle.Top,Height=174};
            body.Controls.Add(grid);body.Controls.Add(tab);
            var invoiceTab=new TabPage("Lập hóa đơn tiền thuê tháng thứ 7 trở đi");var invoiceBar=Ui.EntryPanel(112);
            invoiceTab.Controls.Add(invoiceBar);tab.TabPages.Add(invoiceTab);
            var contract=Ui.Combo();var period=Ui.Number(240,0,7);period.Minimum=7;var asOf=Ui.Date();
            invoiceBar.Controls.Add(Ui.Field("Hợp đồng",contract,260));invoiceBar.Controls.Add(Ui.Field("Kỳ số (7, 8, 9...)",period,170));
            invoiceBar.Controls.Add(Ui.Field("Ngày tính hạn (dùng để mô phỏng)",asOf,235));
            var payTab=new TabPage("Ghi nhận thanh toán hóa đơn");var payBar=Ui.EntryPanel(112);
            payTab.Controls.Add(payBar);tab.TabPages.Add(payTab);
            var amount=Ui.Number(999999999999,2);var method=Ui.Combo();method.Items.AddRange(new object[]{"TIEN_MAT","CHUYEN_KHOAN","THE"});method.SelectedIndex=0;
            payBar.Controls.Add(Ui.Field("Số tiền thanh toán (đ)",amount,220));payBar.Controls.Add(Ui.Field("Phương thức",method,185));
            Action refresh=()=>{Ui.Bind(contract,CatalogService.Contracts(),"MaHopDong");grid.DataSource=BillingService.Invoices();};
            invoiceBar.Controls.Add(Ui.Button("Lập hóa đơn tiền thuê",()=>{
                int id=BillingService.GenerateRent(Ui.SelectedId(contract),(int)period.Value,asOf.Value);
                grid.DataSource=BillingService.Invoices();Ui.Success("Đã lập hóa đơn kỳ thuê: "+id+". Hạn trả tiền là ngày 01 tháng tương ứng.");
            },230));
            payBar.Controls.Add(Ui.Button("Thanh toán dòng đang chọn",()=>{
                int id=Ui.SelectedGridId(grid,"MaHoaDon");
                BillingService.Pay(id,amount.Value,method.Text);grid.DataSource=BillingService.Invoices();
                Ui.Success("Đã ghi nhận giao dịch vào hóa đơn "+id+".");
            },245));
            payBar.Controls.Add(Ui.Button("Tải lại",()=>grid.DataSource=BillingService.Invoices(),125));
            Ui.Safe(refresh);
        }
    }
}
