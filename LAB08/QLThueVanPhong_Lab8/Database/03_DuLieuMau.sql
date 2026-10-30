USE QuanLyChoThueVanPhong;
GO
/* Không ghi đè thông tin đã có; mỗi lần chạy giữ nguyên dữ liệu người dùng. */
IF NOT EXISTS(SELECT 1 FROM dbo.Tang WHERE SoTang=3) INSERT dbo.Tang(SoTang,TenTang) VALUES(3,N'Tầng 3');
IF NOT EXISTS(SELECT 1 FROM dbo.Tang WHERE SoTang=4) INSERT dbo.Tang(SoTang,TenTang) VALUES(4,N'Tầng 4');
IF NOT EXISTS(SELECT 1 FROM dbo.VanPhong WHERE SoPhong=N'P301' AND MaTang=(SELECT MaTang FROM dbo.Tang WHERE SoTang=3))
 INSERT dbo.VanPhong(MaTang,SoPhong,DienTich,Huong,DonGiaThang) SELECT MaTang,N'P301',75,N'Đông',12000000 FROM dbo.Tang WHERE SoTang=3;
IF NOT EXISTS(SELECT 1 FROM dbo.VanPhong WHERE SoPhong=N'P302' AND MaTang=(SELECT MaTang FROM dbo.Tang WHERE SoTang=3))
 INSERT dbo.VanPhong(MaTang,SoPhong,DienTich,Huong,DonGiaThang) SELECT MaTang,N'P302',110,N'Tây',18000000 FROM dbo.Tang WHERE SoTang=3;
IF NOT EXISTS(SELECT 1 FROM dbo.VanPhong WHERE SoPhong=N'P401' AND MaTang=(SELECT MaTang FROM dbo.Tang WHERE SoTang=4))
 INSERT dbo.VanPhong(MaTang,SoPhong,DienTich,Huong,DonGiaThang) SELECT MaTang,N'P401',85,N'Nam',14000000 FROM dbo.Tang WHERE SoTang=4;
IF NOT EXISTS(SELECT 1 FROM dbo.KhachThue WHERE MaSoThue='0312345678')
 INSERT dbo.KhachThue(TenCongTy,MaSoThue,NguoiDaiDien,SDT,Email)
 VALUES(N'Công ty Minh Họa A','0312345678',N'Nguyễn Văn An','0901000001','demo-a@example.test');
IF NOT EXISTS(SELECT 1 FROM dbo.KhachThue WHERE MaSoThue='0312345679')
 INSERT dbo.KhachThue(TenCongTy,MaSoThue,NguoiDaiDien,SDT,Email)
 VALUES(N'Công ty Minh Họa B','0312345679',N'Trần Thị Bình','0901000002','demo-b@example.test');
IF NOT EXISTS(SELECT 1 FROM dbo.NhanVienQuanLy WHERE HoTen=N'Quản lý demo')
 INSERT dbo.NhanVienQuanLy(HoTen,VaiTro) VALUES(N'Quản lý demo','QUAN_LY');
IF NOT EXISTS(SELECT 1 FROM dbo.NhanVienQuanLy WHERE HoTen=N'Kế toán demo')
 INSERT dbo.NhanVienQuanLy(HoTen,VaiTro) VALUES(N'Kế toán demo','KE_TOAN');
IF NOT EXISTS(SELECT 1 FROM dbo.NhanVienQuanLy WHERE HoTen=N'Kỹ thuật demo')
 INSERT dbo.NhanVienQuanLy(HoTen,VaiTro) VALUES(N'Kỹ thuật demo','KY_THUAT');
IF NOT EXISTS(SELECT 1 FROM dbo.DichVu WHERE TenDichVu=N'Điện') INSERT dbo.DichVu(TenDichVu,DonViTinh,DonGia) VALUES(N'Điện',N'kWh',3500);
IF NOT EXISTS(SELECT 1 FROM dbo.DichVu WHERE TenDichVu=N'Nước') INSERT dbo.DichVu(TenDichVu,DonViTinh,DonGia) VALUES(N'Nước',N'm³',18000);
IF NOT EXISTS(SELECT 1 FROM dbo.DichVu WHERE TenDichVu=N'Internet') INSERT dbo.DichVu(TenDichVu,DonViTinh,DonGia) VALUES(N'Internet',N'tháng',350000);
GO
PRINT N'03 - Đã có tầng, văn phòng, khách thuê, người quản lý và dịch vụ mẫu.';
