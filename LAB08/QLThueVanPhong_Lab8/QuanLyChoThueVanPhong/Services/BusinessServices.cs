using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QuanLyChoThueVanPhong.Data;

namespace QuanLyChoThueVanPhong.Services
{
    // Mọi thao tác SQL đặt ở đây / Data, không viết SQL bên trong sự kiện Click.
    public static class CatalogService
    {
        public static DataTable Floors() { return Db.Query("SELECT MaTang,CONCAT(N'Tầng ',SoTang) AS TenHienThi,SoTang,TenTang FROM dbo.Tang ORDER BY SoTang"); }
        public static DataTable Rooms() { return Db.Query("SELECT p.MaVanPhong,CONCAT(t.TenTang,N' / ',p.SoPhong) AS TenHienThi,t.SoTang,p.SoPhong,p.DienTich,p.Huong,p.DonGiaThang,p.TrangThai FROM dbo.VanPhong p JOIN dbo.Tang t ON t.MaTang=p.MaTang ORDER BY t.SoTang,p.SoPhong"); }
        public static DataTable AvailableRooms() { return Db.Query("SELECT p.MaVanPhong,CONCAT(t.TenTang,N' / ',p.SoPhong,N' - ',FORMAT(p.DonGiaThang,'N0'),N' đ/tháng') AS TenHienThi FROM dbo.VanPhong p JOIN dbo.Tang t ON t.MaTang=p.MaTang WHERE p.TrangThai='SAN_SANG' ORDER BY t.SoTang,p.SoPhong"); }
        public static DataTable Tenants() { return Db.Query("SELECT MaKhach,TenCongTy AS TenHienThi,TenCongTy,MaSoThue,NguoiDaiDien,SDT,Email FROM dbo.KhachThue ORDER BY MaKhach"); }
        public static DataTable Services() { return Db.Query("SELECT MaDichVu,TenDichVu AS TenHienThi,TenDichVu,DonViTinh,DonGia FROM dbo.DichVu ORDER BY MaDichVu"); }
        public static DataTable Managers() { return Db.Query("SELECT MaQL,HoTen AS TenHienThi,HoTen,VaiTro FROM dbo.NhanVienQuanLy ORDER BY MaQL"); }
        public static DataTable Contracts() { return Db.Query("SELECT h.MaHopDong,CONCAT(N'HD',h.MaHopDong,N' - ',k.TenCongTy) AS TenHienThi,h.NgayBatDau,h.SoThangGoc,h.TrangThai,k.TenCongTy FROM dbo.HopDong h JOIN dbo.KhachThue k ON k.MaKhach=h.MaKhach ORDER BY h.MaHopDong DESC"); }
        public static DataTable ContractRooms() { return Db.Query("SELECT c.MaCTHD,CONCAT(N'CT',c.MaCTHD,N' / HD',c.MaHopDong,N' / ',p.SoPhong) AS TenHienThi,c.MaHopDong,p.SoPhong,c.DonGiaThangChot,c.NgayBatDau,c.NgayKetThucGoc,c.NgayKetThucHienTai FROM dbo.ChiTietHopDong c JOIN dbo.VanPhong p ON p.MaVanPhong=c.MaVanPhong JOIN dbo.HopDong h ON h.MaHopDong=c.MaHopDong WHERE h.TrangThai='HIEU_LUC' ORDER BY c.MaCTHD DESC"); }
        public static void AddFloor(int so, string ten) { Db.Execute("INSERT dbo.Tang(SoTang,TenTang) VALUES(@a,@b)",Db.P("@a",so),Db.P("@b",ten)); }
        public static void AddTenant(string ten,string mst,string daidien,string sdt,string email) { Db.Execute("INSERT dbo.KhachThue(TenCongTy,MaSoThue,NguoiDaiDien,SDT,Email) VALUES(@a,@b,@c,@d,@e)",Db.P("@a",ten),Db.P("@b",string.IsNullOrWhiteSpace(mst)?null:mst),Db.P("@c",daidien),Db.P("@d",sdt),Db.P("@e",email)); }
        public static void AddService(string ten,string donvi,decimal gia) { Db.Execute("INSERT dbo.DichVu(TenDichVu,DonViTinh,DonGia) VALUES(@a,@b,@c)",Db.P("@a",ten),Db.P("@b",donvi),Db.P("@c",gia)); }
        public static void AddManager(string ten,string role) { Db.Execute("INSERT dbo.NhanVienQuanLy(HoTen,VaiTro) VALUES(@a,@b)",Db.P("@a",ten),Db.P("@b",role)); }
        public static void AddRoom(int floor,string so,decimal area,string huong,decimal gia)
        { Db.Execute("INSERT dbo.VanPhong(MaTang,SoPhong,DienTich,Huong,DonGiaThang) VALUES(@a,@b,@c,@d,@e)",Db.P("@a",floor),Db.P("@b",so),Db.P("@c",area),Db.P("@d",huong),Db.P("@e",gia)); }
    }
    public static class ContractService
    {
        public static int Create(int tenant,int manager,DateTime start,int months,IEnumerable<int> rooms)
        {
            var tvp = new DataTable();tvp.Columns.Add("MaVanPhong",typeof(int));
            foreach (int id in rooms) tvp.Rows.Add(id);
            var list = new SqlParameter("@VanPhongs",SqlDbType.Structured) { TypeName="dbo.DanhSachVanPhongThue",Value=tvp };
            return Db.StoredWithOutput("dbo.usp_TaoHopDong","@MaHopDongMoi",
                Db.P("@MaKhach",tenant),Db.P("@MaQL",manager),Db.P("@NgayBatDau",start.Date),Db.P("@SoThangGoc",months),list);
        }
        public static DataTable Details(int contractId)
        { return Db.Query("SELECT c.MaCTHD,p.SoPhong,c.DonGiaThangChot,c.NgayBatDau,c.NgayKetThucGoc,c.NgayKetThucHienTai FROM dbo.ChiTietHopDong c JOIN dbo.VanPhong p ON p.MaVanPhong=c.MaVanPhong WHERE c.MaHopDong=@id",Db.P("@id",contractId)); }
    }
    public static class BillingService
    {
        public static DataTable Invoices()
        { return Db.Query(@"SELECT hd.MaHoaDon,hd.MaHopDong,k.TenCongTy,hd.LoaiHoaDon,hd.KyThang,hd.KyNam,hd.NgayDenHan,hd.TongTien,ISNULL(t.DaThu,0) AS DaThu,hd.TongTien-ISNULL(t.DaThu,0) AS ConNo,hd.TrangThai
                           FROM dbo.HoaDon hd JOIN dbo.HopDong h ON h.MaHopDong=hd.MaHopDong JOIN dbo.KhachThue k ON k.MaKhach=h.MaKhach
                           OUTER APPLY (SELECT SUM(SoTien) AS DaThu FROM dbo.ThanhToan WHERE MaHoaDon=hd.MaHoaDon) t ORDER BY hd.MaHoaDon DESC"); }
        public static int GenerateRent(int contract,int period,DateTime asOf)
        { return Db.StoredWithOutput("dbo.usp_LapHoaDonTienThue","@MaHoaDonMoi",Db.P("@MaHopDong",contract),Db.P("@KySo",period),Db.P("@NgayTinh",asOf.Date)); }
        public static void Pay(int invoice,decimal amount,string method)
        { Db.Stored("dbo.usp_ThanhToanHoaDon",Db.P("@MaHoaDon",invoice),Db.P("@SoTien",amount),Db.P("@PhuongThuc",method)); }
        public static void LogService(int roomDetail,int service,DateTime usage,decimal quantity)
        { Db.Stored("dbo.usp_GhiNhanDichVu",Db.P("@MaCTHD",roomDetail),Db.P("@MaDichVu",service),Db.P("@NgaySuDung",usage.Date),Db.P("@SoLuong",quantity)); }
        public static int BillServices(int contract,int month,int year,DateTime asOf)
        { return Db.StoredWithOutput("dbo.usp_LapHoaDonDichVu","@MaHoaDonMoi",Db.P("@MaHopDong",contract),Db.P("@Thang",month),Db.P("@Nam",year),Db.P("@NgayTinh",asOf.Date)); }
        public static DataTable Usage()
        { return Db.Query("SELECT TOP (200) s.MaSuDung,c.MaHopDong,p.SoPhong,d.TenDichVu,s.NgaySuDung,s.SoLuong,s.DonGiaChot,s.ThanhTien FROM dbo.SuDungDichVu s JOIN dbo.ChiTietHopDong c ON c.MaCTHD=s.MaCTHD JOIN dbo.VanPhong p ON p.MaVanPhong=c.MaVanPhong JOIN dbo.DichVu d ON d.MaDichVu=s.MaDichVu ORDER BY s.NgaySuDung DESC,s.MaSuDung DESC"); }
        public static int Extend(int roomDetail,int months,DateTime asOf)
        { return Db.StoredWithOutput("dbo.usp_LapPhieuGiaHan","@MaGiaHanMoi",Db.P("@MaCTHD",roomDetail),Db.P("@SoThangGiaHan",months),Db.P("@NgayKiemTra",asOf.Date)); }
        public static DataTable Extensions()
        { return Db.Query("SELECT g.MaGiaHan,c.MaHopDong,p.SoPhong,g.NgayLap,g.SoThangGiaHan,g.NgayKetThucCu,g.NgayKetThucMoi FROM dbo.PhieuGiaHan g JOIN dbo.ChiTietHopDong c ON c.MaCTHD=g.MaCTHD JOIN dbo.VanPhong p ON p.MaVanPhong=c.MaVanPhong ORDER BY g.MaGiaHan DESC"); }
    }
    public static class PeopleService
    {
        public static DataTable Employees()
        { return Db.Query("SELECT nv.MaNhanVien,CONCAT(nv.HoTen,N' - ',k.TenCongTy) AS TenHienThi,nv.HoTen,k.TenCongTy,p.SoPhong,nv.ChucVu,nv.CCCD,nv.DuongDanAnh,nv.TrangThai FROM dbo.NhanVienLamViec nv JOIN dbo.KhachThue k ON k.MaKhach=nv.MaKhach JOIN dbo.VanPhong p ON p.MaVanPhong=nv.MaVanPhong ORDER BY nv.MaNhanVien DESC"); }
        public static DataTable Cards()
        { return Db.Query("SELECT t.MaThe,n.HoTen,t.BienSo,t.LoaiXe,t.NgayCap,t.TrangThai FROM dbo.TheGiuXe t JOIN dbo.NhanVienLamViec n ON n.MaNhanVien=t.MaNhanVien ORDER BY t.MaThe DESC"); }
        public static void AddEmployee(int tenant,int office,string name,string title,string cccd,string photo)
        { Db.Stored("dbo.usp_ThemNhanVienLamViec",Db.P("@MaKhach",tenant),Db.P("@MaVanPhong",office),Db.P("@HoTen",name),Db.P("@ChucVu",title),Db.P("@CCCD",cccd),Db.P("@DuongDanAnh",photo)); }
        public static void AddCard(int employee,string plate,string type)
        { Db.Execute("INSERT dbo.TheGiuXe(MaNhanVien,BienSo,LoaiXe) VALUES(@a,@b,@c)",Db.P("@a",employee),Db.P("@b",plate),Db.P("@c",type)); }
    }
    public static class InspectionService
    {
        public static DataTable Inspections()
        { return Db.Query("SELECT k.MaKiemTra,p.SoPhong,n.HoTen,k.NgayKiemTra,k.TinhTrang,k.TuYSuaKetCau,k.TienPhat,k.GhiChu FROM dbo.KiemTraPhong k JOIN dbo.VanPhong p ON p.MaVanPhong=k.MaVanPhong JOIN dbo.NhanVienQuanLy n ON n.MaQL=k.MaQL ORDER BY k.NgayKiemTra DESC,k.MaKiemTra DESC"); }
        public static void Add(int room,int manager,DateTime date,string condition,bool violation,decimal penalty,string note)
        { Db.Stored("dbo.usp_GhiKiemTraPhong",Db.P("@MaVanPhong",room),Db.P("@MaQL",manager),Db.P("@NgayKiemTra",date.Date),Db.P("@TinhTrang",condition),Db.P("@TuYSuaKetCau",violation),Db.P("@TienPhat",penalty),Db.P("@GhiChu",note)); }
        public static DataTable Due()
        { return Db.Query(@"SELECT p.MaVanPhong,p.SoPhong,t.SoTang,MAX(k.NgayKiemTra) AS KiemTraGanNhat,
                           CASE WHEN MAX(k.NgayKiemTra) IS NULL THEN N'Chưa từng kiểm tra'
                                WHEN DATEADD(year,1,MAX(k.NgayKiemTra)) <= CONVERT(date,GETDATE()) THEN N'Đến hạn'
                                ELSE N'Chưa đến hạn' END AS CanhBao
                           FROM dbo.VanPhong p JOIN dbo.Tang t ON t.MaTang=p.MaTang
                           LEFT JOIN dbo.KiemTraPhong k ON k.MaVanPhong=p.MaVanPhong
                           GROUP BY p.MaVanPhong,p.SoPhong,t.SoTang ORDER BY t.SoTang,p.SoPhong"); }
    }
    public static class ReportService
    {
        public static DataTable Arrears(DateTime from,DateTime to)
        { return Db.Query(@"SELECT hd.MaHoaDon,k.TenCongTy,hd.LoaiHoaDon,hd.NgayDenHan,hd.TongTien,
                    ISNULL(t.DaThu,0) AS DaThu,hd.TongTien-ISNULL(t.DaThu,0) AS ConNo
                    FROM dbo.HoaDon hd JOIN dbo.HopDong h ON h.MaHopDong=hd.MaHopDong
                    JOIN dbo.KhachThue k ON k.MaKhach=h.MaKhach
                    OUTER APPLY(SELECT SUM(SoTien) AS DaThu FROM dbo.ThanhToan WHERE MaHoaDon=hd.MaHoaDon) t
                    WHERE hd.NgayDenHan>=@from AND hd.NgayDenHan<=@to
                    ORDER BY hd.NgayDenHan DESC",Db.P("@from",from.Date),Db.P("@to",to.Date)); }
    }
}
