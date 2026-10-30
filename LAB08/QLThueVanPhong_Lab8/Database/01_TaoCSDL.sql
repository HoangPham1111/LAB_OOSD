/* QUẢN LÝ CHO THUÊ VĂN PHÒNG - SQL Server 2017+/LocalDB
   Không DROP dữ liệu khi chạy lại; chạy lần lượt 01 -> 02 -> 03.
   Mốc cuối hạn thuê là EXCLUSIVE. Ngày bắt đầu thuê bắt buộc là ngày 01
   để kỳ 7+ đến hạn ngày đầu tháng. Đơn giá tiền/tháng đóng băng ở thời điểm ký. */
IF DB_ID(N'QuanLyChoThueVanPhong') IS NULL CREATE DATABASE QuanLyChoThueVanPhong;
GO
USE QuanLyChoThueVanPhong;
GO
IF OBJECT_ID(N'dbo.Tang', 'U') IS NULL
CREATE TABLE dbo.Tang (
    MaTang int IDENTITY(1,1) CONSTRAINT PK_Tang PRIMARY KEY,
    SoTang int NOT NULL CONSTRAINT UQ_Tang_SoTang UNIQUE,
    TenTang nvarchar(100) NOT NULL
);
GO
IF OBJECT_ID(N'dbo.VanPhong', 'U') IS NULL
CREATE TABLE dbo.VanPhong (
    MaVanPhong int IDENTITY(1,1) CONSTRAINT PK_VanPhong PRIMARY KEY,
    MaTang int NOT NULL CONSTRAINT FK_VanPhong_Tang REFERENCES dbo.Tang(MaTang),
    SoPhong nvarchar(20) NOT NULL,
    DienTich decimal(10,2) NOT NULL CONSTRAINT CK_VP_DienTich CHECK(DienTich > 0),
    Huong nvarchar(40) NULL,
    DonGiaThang decimal(18,2) NOT NULL CONSTRAINT CK_VP_Gia CHECK(DonGiaThang > 0),
    TrangThai varchar(20) NOT NULL CONSTRAINT DF_VP_TrangThai DEFAULT 'SAN_SANG',
    CONSTRAINT UQ_VP_Tang_So UNIQUE (MaTang,SoPhong),
    CONSTRAINT CK_VP_TrangThai CHECK(TrangThai IN ('SAN_SANG','BAO_TRI'))
);
GO
IF OBJECT_ID(N'dbo.KhachThue', 'U') IS NULL
CREATE TABLE dbo.KhachThue (
    MaKhach int IDENTITY(1,1) CONSTRAINT PK_Khach PRIMARY KEY,
    TenCongTy nvarchar(200) NOT NULL,
    MaSoThue varchar(30) NULL,
    NguoiDaiDien nvarchar(120) NOT NULL,
    SDT varchar(25) NULL,
    Email varchar(120) NULL
);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='UX_Khach_MST' AND object_id=OBJECT_ID('dbo.KhachThue'))
    CREATE UNIQUE INDEX UX_Khach_MST ON dbo.KhachThue(MaSoThue) WHERE MaSoThue IS NOT NULL;
GO
IF OBJECT_ID(N'dbo.NhanVienQuanLy', 'U') IS NULL
CREATE TABLE dbo.NhanVienQuanLy (
    MaQL int IDENTITY(1,1) CONSTRAINT PK_QL PRIMARY KEY,
    HoTen nvarchar(120) NOT NULL,
    VaiTro varchar(30) NOT NULL,
    CONSTRAINT CK_QL_Role CHECK (VaiTro IN ('QUAN_LY','KE_TOAN','KY_THUAT'))
);
GO
IF OBJECT_ID(N'dbo.HopDong', 'U') IS NULL
CREATE TABLE dbo.HopDong (
    MaHopDong int IDENTITY(1,1) CONSTRAINT PK_HD PRIMARY KEY,
    MaKhach int NOT NULL CONSTRAINT FK_HD_Khach REFERENCES dbo.KhachThue(MaKhach),
    MaQL int NOT NULL CONSTRAINT FK_HD_QL REFERENCES dbo.NhanVienQuanLy(MaQL),
    NgayKy date NOT NULL CONSTRAINT DF_HD_NgayKy DEFAULT CONVERT(date,GETDATE()),
    NgayBatDau date NOT NULL,
    SoThangGoc int NOT NULL,
    TrangThai varchar(25) NOT NULL CONSTRAINT DF_HD_TrangThai DEFAULT 'CHO_TRA_TRUOC',
    CONSTRAINT CK_HD_ThoiHan CHECK (SoThangGoc >= 6),
    CONSTRAINT CK_HD_NgayBatDau CHECK (DAY(NgayBatDau) = 1),
    CONSTRAINT CK_HD_TrangThai CHECK (TrangThai IN ('CHO_TRA_TRUOC','HIEU_LUC','HUY','KET_THUC'))
);
GO
IF OBJECT_ID(N'dbo.ChiTietHopDong', 'U') IS NULL
CREATE TABLE dbo.ChiTietHopDong (
    MaCTHD int IDENTITY(1,1) CONSTRAINT PK_CTHD PRIMARY KEY,
    MaHopDong int NOT NULL CONSTRAINT FK_CTHD_HD REFERENCES dbo.HopDong(MaHopDong),
    MaVanPhong int NOT NULL CONSTRAINT FK_CTHD_VP REFERENCES dbo.VanPhong(MaVanPhong),
    DonGiaThangChot decimal(18,2) NOT NULL CONSTRAINT CK_CTHD_Gia CHECK (DonGiaThangChot > 0),
    NgayBatDau date NOT NULL,
    NgayKetThucGoc date NOT NULL,
    NgayKetThucHienTai date NOT NULL,
    CONSTRAINT UQ_CTHD_HD_VP UNIQUE(MaHopDong,MaVanPhong),
    CONSTRAINT CK_CTHD_Ngay CHECK(NgayBatDau < NgayKetThucGoc AND NgayKetThucHienTai >= NgayKetThucGoc)
);
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name='IX_CTHD_VP_KhoangNgay' AND object_id=OBJECT_ID('dbo.ChiTietHopDong'))
CREATE INDEX IX_CTHD_VP_KhoangNgay ON dbo.ChiTietHopDong(MaVanPhong,NgayBatDau,NgayKetThucHienTai);
GO
IF OBJECT_ID(N'dbo.DichVu', 'U') IS NULL
CREATE TABLE dbo.DichVu (
    MaDichVu int IDENTITY(1,1) CONSTRAINT PK_DV PRIMARY KEY,
    TenDichVu nvarchar(120) NOT NULL CONSTRAINT UQ_DV_Ten UNIQUE,
    DonViTinh nvarchar(30) NOT NULL,
    DonGia decimal(18,2) NOT NULL CONSTRAINT CK_DV_Gia CHECK(DonGia >= 0)
);
GO
IF OBJECT_ID(N'dbo.SuDungDichVu', 'U') IS NULL
CREATE TABLE dbo.SuDungDichVu (
    MaSuDung int IDENTITY(1,1) CONSTRAINT PK_SDDV PRIMARY KEY,
    MaCTHD int NOT NULL CONSTRAINT FK_SDDV_CTHD REFERENCES dbo.ChiTietHopDong(MaCTHD),
    MaDichVu int NOT NULL CONSTRAINT FK_SDDV_DV REFERENCES dbo.DichVu(MaDichVu),
    NgaySuDung date NOT NULL,
    SoLuong decimal(12,2) NOT NULL CONSTRAINT CK_SDDV_SL CHECK(SoLuong > 0),
    DonGiaChot decimal(18,2) NOT NULL CONSTRAINT CK_SDDV_Gia CHECK(DonGiaChot >= 0),
    ThanhTien AS (CONVERT(decimal(18,2),SoLuong * DonGiaChot)) PERSISTED,
    CONSTRAINT UQ_SDDV_Ngay UNIQUE(MaCTHD,MaDichVu,NgaySuDung)
);
GO
IF OBJECT_ID(N'dbo.HoaDon', 'U') IS NULL
CREATE TABLE dbo.HoaDon (
    MaHoaDon int IDENTITY(1,1) CONSTRAINT PK_HoaDon PRIMARY KEY,
    MaHopDong int NOT NULL CONSTRAINT FK_HoaDon_HD REFERENCES dbo.HopDong(MaHopDong),
    LoaiHoaDon varchar(20) NOT NULL,
    KyThang int NOT NULL,
    KyNam int NOT NULL,
    NgayLap date NOT NULL,
    NgayDenHan date NOT NULL,
    TongTien decimal(18,2) NOT NULL CONSTRAINT CK_HoaDon_Tien CHECK(TongTien >= 0),
    TrangThai varchar(20) NOT NULL CONSTRAINT DF_HoaDon_TrangThai DEFAULT 'CHUA_THANH_TOAN',
    CONSTRAINT UQ_HoaDon_Ky UNIQUE(MaHopDong,LoaiHoaDon,KyThang,KyNam),
    CONSTRAINT CK_HoaDon_Loai CHECK(LoaiHoaDon IN('TRA_TRUOC','TIEN_THUE','DICH_VU')),
    CONSTRAINT CK_HoaDon_Ky CHECK ((LoaiHoaDon='TRA_TRUOC' AND KyThang=0 AND KyNam=0)
                               OR (LoaiHoaDon IN('TIEN_THUE','DICH_VU') AND KyThang BETWEEN 1 AND 12 AND KyNam BETWEEN 2000 AND 2100)),
    CONSTRAINT CK_HoaDon_TrangThai CHECK(TrangThai IN('CHUA_THANH_TOAN','DA_THANH_TOAN'))
);
GO
IF OBJECT_ID(N'dbo.ChiTietHoaDon', 'U') IS NULL
CREATE TABLE dbo.ChiTietHoaDon (
    MaCTHoaDon int IDENTITY(1,1) CONSTRAINT PK_CTHoaDon PRIMARY KEY,
    MaHoaDon int NOT NULL CONSTRAINT FK_CTHoaDon_HoaDon REFERENCES dbo.HoaDon(MaHoaDon),
    MaCTHD int NULL CONSTRAINT FK_CTHoaDon_CTHD REFERENCES dbo.ChiTietHopDong(MaCTHD),
    MaSuDung int NULL CONSTRAINT FK_CTHoaDon_SDDV REFERENCES dbo.SuDungDichVu(MaSuDung),
    DienGiai nvarchar(250) NOT NULL,
    SoLuong decimal(12,2) NOT NULL CONSTRAINT CK_CTHoaDon_SL CHECK(SoLuong > 0),
    DonGia decimal(18,2) NOT NULL CONSTRAINT CK_CTHoaDon_Gia CHECK(DonGia >= 0),
    ThanhTien AS (CONVERT(decimal(18,2),SoLuong * DonGia)) PERSISTED
);
GO
IF OBJECT_ID(N'dbo.ThanhToan', 'U') IS NULL
CREATE TABLE dbo.ThanhToan (
    MaThanhToan int IDENTITY(1,1) CONSTRAINT PK_ThanhToan PRIMARY KEY,
    MaHoaDon int NOT NULL CONSTRAINT FK_TT_HoaDon REFERENCES dbo.HoaDon(MaHoaDon),
    NgayThanhToan datetime2(0) NOT NULL CONSTRAINT DF_TT_Ngay DEFAULT SYSDATETIME(),
    SoTien decimal(18,2) NOT NULL CONSTRAINT CK_TT_Tien CHECK(SoTien>0),
    PhuongThuc varchar(20) NOT NULL,
    MaGiaoDich varchar(60) NULL,
    CONSTRAINT CK_TT_PT CHECK(PhuongThuc IN('TIEN_MAT','CHUYEN_KHOAN','THE'))
);
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name='UX_TT_MaGD' AND object_id=OBJECT_ID('dbo.ThanhToan'))
CREATE UNIQUE INDEX UX_TT_MaGD ON dbo.ThanhToan(MaGiaoDich) WHERE MaGiaoDich IS NOT NULL;
GO
IF OBJECT_ID(N'dbo.PhieuGiaHan', 'U') IS NULL
CREATE TABLE dbo.PhieuGiaHan (
    MaGiaHan int IDENTITY(1,1) CONSTRAINT PK_GiaHan PRIMARY KEY,
    MaCTHD int NOT NULL CONSTRAINT FK_GiaHan_CTHD REFERENCES dbo.ChiTietHopDong(MaCTHD),
    NgayLap date NOT NULL,
    SoThangGiaHan int NOT NULL CONSTRAINT CK_GH_Thang CHECK(SoThangGiaHan>0),
    NgayKetThucCu date NOT NULL,
    NgayKetThucMoi date NOT NULL,
    CONSTRAINT CK_GH_Ngay CHECK(NgayKetThucMoi > NgayKetThucCu)
);
GO
IF OBJECT_ID(N'dbo.NhanVienLamViec', 'U') IS NULL
CREATE TABLE dbo.NhanVienLamViec (
    MaNhanVien int IDENTITY(1,1) CONSTRAINT PK_NVLV PRIMARY KEY,
    MaKhach int NOT NULL CONSTRAINT FK_NVLV_Khach REFERENCES dbo.KhachThue(MaKhach),
    MaVanPhong int NOT NULL CONSTRAINT FK_NVLV_VP REFERENCES dbo.VanPhong(MaVanPhong),
    HoTen nvarchar(120) NOT NULL,
    ChucVu nvarchar(100) NULL,
    CCCD varchar(20) NOT NULL CONSTRAINT UQ_NVLV_CCCD UNIQUE,
    DuongDanAnh nvarchar(400) NULL,
    TrangThai varchar(20) NOT NULL CONSTRAINT DF_NVLV_TrangThai DEFAULT 'DANG_LAM',
    CONSTRAINT CK_NVLV_TrangThai CHECK(TrangThai IN('DANG_LAM','DA_NGHI'))
);
GO
IF OBJECT_ID(N'dbo.TheGiuXe', 'U') IS NULL
CREATE TABLE dbo.TheGiuXe (
    MaThe int IDENTITY(1,1) CONSTRAINT PK_The PRIMARY KEY,
    MaNhanVien int NOT NULL CONSTRAINT FK_The_NV REFERENCES dbo.NhanVienLamViec(MaNhanVien),
    BienSo varchar(20) NOT NULL,
    LoaiXe nvarchar(60) NOT NULL,
    NgayCap date NOT NULL CONSTRAINT DF_The_Ngay DEFAULT CONVERT(date,GETDATE()),
    TrangThai varchar(20) NOT NULL CONSTRAINT DF_The_TrangThai DEFAULT 'HIEU_LUC',
    CONSTRAINT CK_The_TrangThai CHECK(TrangThai IN('HIEU_LUC','DA_THU_HOI'))
);
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name='UX_The_BienSoHieuLuc' AND object_id=OBJECT_ID('dbo.TheGiuXe'))
CREATE UNIQUE INDEX UX_The_BienSoHieuLuc ON dbo.TheGiuXe(BienSo) WHERE TrangThai='HIEU_LUC';
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name='UX_The_MotTheNhanVien' AND object_id=OBJECT_ID('dbo.TheGiuXe'))
CREATE UNIQUE INDEX UX_The_MotTheNhanVien ON dbo.TheGiuXe(MaNhanVien) WHERE TrangThai='HIEU_LUC';
GO
IF OBJECT_ID(N'dbo.KiemTraPhong', 'U') IS NULL
CREATE TABLE dbo.KiemTraPhong (
    MaKiemTra int IDENTITY(1,1) CONSTRAINT PK_KTP PRIMARY KEY,
    MaVanPhong int NOT NULL CONSTRAINT FK_KTP_VP REFERENCES dbo.VanPhong(MaVanPhong),
    MaQL int NOT NULL CONSTRAINT FK_KTP_QL REFERENCES dbo.NhanVienQuanLy(MaQL),
    NgayKiemTra date NOT NULL,
    TinhTrang nvarchar(200) NOT NULL,
    TuYSuaKetCau bit NOT NULL CONSTRAINT DF_KTP_TuYSua DEFAULT 0,
    TienPhat decimal(18,2) NOT NULL CONSTRAINT DF_KTP_Phat DEFAULT 0,
    GhiChu nvarchar(400) NULL,
    CONSTRAINT CK_KTP_Phat CHECK(TienPhat>=0),
    CONSTRAINT CK_KTP_HopLe CHECK (TuYSuaKetCau=1 OR TienPhat=0)
);
GO
IF TYPE_ID('dbo.DanhSachVanPhongThue') IS NULL
EXEC('CREATE TYPE dbo.DanhSachVanPhongThue AS TABLE(MaVanPhong int NOT NULL PRIMARY KEY)');
GO
PRINT N'01 - Đã tạo CSDL và các bảng (không xoá dữ liệu cũ).';
