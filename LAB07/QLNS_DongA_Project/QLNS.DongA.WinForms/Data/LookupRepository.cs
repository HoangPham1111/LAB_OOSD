using System.Data;
using System.Data.SqlClient;

namespace QLNS.DongA.Data
{
    internal sealed class LookupRepository
    {
        public DataTable GetEmployees(bool trialOnly = false)
        {
            string sql = "SELECT MaNhanVien, MaNhanVien + N' - ' + HoTen AS HienThi FROM dbo.NhanVien" +
                         (trialOnly ? " WHERE TrangThaiNhanVien = N'ThuViec'" : " WHERE TrangThaiNhanVien <> N'NgungLam'") +
                         " ORDER BY HoTen";
            return new GenericDataRepository().Query(sql);
        }

        public DataTable GetDepartments()
        {
            return new GenericDataRepository().Query("SELECT MaPhongBan, MaPhongBan + N' - ' + TenPhongBan AS HienThi FROM dbo.PhongBan WHERE TrangThai = 1 ORDER BY TenPhongBan");
        }

        public DataTable GetTrialDecisions()
        {
            return new GenericDataRepository().Query(@"
                SELECT q.MaQDTuyenDung,
                       q.MaQDTuyenDung + N' - ' + n.HoTen + N' (ket thuc thu viec: ' + CONVERT(nvarchar(10), q.NgayKetThucThuViec, 120) + N')' AS HienThi
                FROM dbo.QuyetDinhTuyenDung q
                INNER JOIN dbo.NhanVien n ON n.MaNhanVien = q.MaNhanVien
                WHERE q.TrangThai IN (N'DangThuViec', N'DaKetThucThuViec')
                ORDER BY q.NgayKetThucThuViec");
        }

        public DataTable GetAwardDecisions()
        {
            return new GenericDataRepository().Query(@"
                SELECT q.MaQDKhenThuong,
                       q.MaQDKhenThuong + N' - ' + q.TrangThai + N' - ' + CONVERT(nvarchar(30), q.SoTienKhenThuong) AS HienThi
                FROM dbo.QuyetDinhKhenThuong q
                ORDER BY q.NgayLap DESC");
        }
    }
}
