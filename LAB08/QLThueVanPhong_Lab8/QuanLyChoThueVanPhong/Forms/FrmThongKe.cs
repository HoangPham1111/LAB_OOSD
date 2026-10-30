using System;
using System.Data;
using System.Windows.Forms;
using QuanLyChoThueVanPhong.Services;
namespace QuanLyChoThueVanPhong.Forms
{
    public class FrmThongKe:Form
    {
        public FrmThongKe()
        {
            var body=Ui.Shell(this,"Tra cứu thống kê và công nợ");
            var bar=Ui.EntryPanel(136);var grid=Ui.Grid();body.Controls.Add(grid);body.Controls.Add(bar);
            var start=Ui.Date(new DateTime(DateTime.Today.Year,1,1));
            var end=Ui.Date(new DateTime(DateTime.Today.Year+2,12,31));
            var summary=new Label{Width=850,Height=25,AutoSize=false};
            bar.Controls.Add(Ui.Field("Từ ngày đến hạn",start,205));bar.Controls.Add(Ui.Field("Đến ngày",end,205));
            Action refresh=()=>{
                DataTable dt=ReportService.Arrears(start.Value,end.Value);grid.DataSource=dt;
                decimal total=0;foreach(DataRow r in dt.Rows) total+=Convert.ToDecimal(r["ConNo"]);
                summary.Text="Tổng công nợ trong khoảng: "+total.ToString("N0")+" đ  |  Số hóa đơn: "+dt.Rows.Count;
            };
            bar.Controls.Add(Ui.Button("Thống kê công nợ",refresh,200));bar.Controls.Add(summary);
            Ui.Safe(refresh);
        }
    }
}
