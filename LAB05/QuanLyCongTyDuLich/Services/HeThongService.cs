using System;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    /// <summary>Kiểm tra kết nối để giao diện báo lỗi cấu hình thân thiện trước khi mở nghiệp vụ.</summary>
    public class HeThongService
    {
        public KetQuaXuLy KiemTraKetNoi()
        {
            try
            {
                object db = Db.Scalar("SELECT DB_NAME()");
                return KetQuaXuLy.Ok("Đã kết nối SQL Server / CSDL: " + Convert.ToString(db));
            }
            catch (Exception ex)
            {
                return KetQuaXuLy.Fail("Chưa kết nối được CSDL. Hãy kiểm tra App.config, SQL Server và script Database. Chi tiết: " + ex.Message);
            }
        }
    }
}
