using System;
using System.Windows.Forms;
using QuanLyChoThueVanPhong.Services;
namespace QuanLyChoThueVanPhong.Forms
{
    public class FrmGiaHan:Form
    {
        public FrmGiaHan()
        {
            var body=Ui.Shell(this,"Gia hạn thuê từng văn phòng - kiểm tra công nợ");
            var bar=Ui.EntryPanel(147);var grid=Ui.Grid();body.Controls.Add(grid);body.Controls.Add(bar);
            var detail=Ui.Combo();var months=Ui.Number(120,0,1);months.Minimum=1;var date=Ui.Date();
            bar.Controls.Add(Ui.Field("Hợp đồng / văn phòng gia hạn",detail,290));
            bar.Controls.Add(Ui.Field("Số tháng gia hạn",months,180));
            bar.Controls.Add(Ui.Field("Ngày kiểm tra công nợ (mô phỏng)",date,245));
            var note=new Label{Text="Quy tắc: tổng số tháng gia hạn ≤ 1/2 thời hạn thuê gốc; mọi hóa đơn và kỳ đến hạn phải được thanh toán.",AutoSize=false,Width=930,Height=28};
            Action refresh=()=>{Ui.Bind(detail,CatalogService.ContractRooms(),"MaCTHD");grid.DataSource=BillingService.Extensions();};
            bar.Controls.Add(Ui.Button("Kiểm tra và lập phiếu",()=>{
                int id=BillingService.Extend(Ui.SelectedId(detail),(int)months.Value,date.Value);
                refresh();Ui.Success("Đã lập phiếu gia hạn "+id+" và cập nhật hạn thuê trong cùng transaction.");
            },225));
            bar.Controls.Add(Ui.Button("Tải lại",refresh,110));
            bar.Controls.Add(note);Ui.Safe(refresh);
        }
    }
}
