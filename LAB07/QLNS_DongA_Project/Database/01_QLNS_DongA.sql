/*
  CSDL: QLNS_DongA
  SQL Server (2016+ recommended)
  Nguon nghiep vu: Bai thuc hanh BAI 7 - QUAN LY NHAN SU.
  Luu y: Cong thuc prorate luong va GioChuanThang trong sp_TaoBangLuongThang la
  chinh sach MINH HOA de kiem thu; de bai chua cung cap cong thuc/tieu chuan gio chinh thuc.
*/
IF DB_ID(N'QLNS_DongA') IS NULL
BEGIN
    CREATE DATABASE QLNS_DongA;
END;
GO
USE QLNS_DongA;
GO

/* 1. Danh muc phong ban */
IF OBJECT_ID(N'dbo.PhongBan', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PhongBan
    (
        MaPhongBan NVARCHAR(20) NOT NULL CONSTRAINT PK_PhongBan PRIMARY KEY,
        TenPhongBan NVARCHAR(150) NOT NULL,
        LoaiDonVi NVARCHAR(40) NULL,
        NhuCauTuyenDung BIT NOT NULL CONSTRAINT DF_PhongBan_NhuCau DEFAULT (0),
        TrangThai BIT NOT NULL CONSTRAINT DF_PhongBan_TrangThai DEFAULT (1),
        CONSTRAINT UQ_PhongBan_Ten UNIQUE (TenPhongBan)
    );
END;
GO

/* 2. Ho so nhan vien */
IF OBJECT_ID(N'dbo.NhanVien', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.NhanVien
    (
        MaNhanVien NVARCHAR(20) NOT NULL CONSTRAINT PK_NhanVien PRIMARY KEY,
        HoTen NVARCHAR(150) NOT NULL,
        NgayTiepNhan DATE NULL,
        TrangThaiNhanVien NVARCHAR(30) NOT NULL CONSTRAINT DF_NhanVien_TrangThai DEFAULT (N'ThuViec'),
        MaPhongBan NVARCHAR(20) NULL,
        Email NVARCHAR(150) NULL,
        DienThoai NVARCHAR(30) NULL,
        CONSTRAINT FK_NhanVien_PhongBan FOREIGN KEY (MaPhongBan) REFERENCES dbo.PhongBan(MaPhongBan),
        CONSTRAINT CK_NhanVien_TrangThai CHECK (TrangThaiNhanVien IN (N'ThuViec', N'DangLam', N'NgungLam'))
    );
END;
GO

/* 3. Quyet dinh tuyen dung/thu viec */
IF OBJECT_ID(N'dbo.QuyetDinhTuyenDung', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.QuyetDinhTuyenDung
    (
        MaQDTuyenDung NVARCHAR(20) NOT NULL CONSTRAINT PK_QuyetDinhTuyenDung PRIMARY KEY,
        MaNhanVien NVARCHAR(20) NOT NULL,
        MaPhongBan NVARCHAR(20) NOT NULL,
        NgayKy DATE NOT NULL,
        NgayBatDauThuViec DATE NOT NULL,
        NgayKetThucThuViec DATE NOT NULL,
        LuongThuViec DECIMAL(18,2) NOT NULL,
        DieuKhoanNhaTuyenDung NVARCHAR(1000) NOT NULL,
        DieuKhoanUngVien NVARCHAR(1000) NOT NULL,
        TrangThai NVARCHAR(30) NOT NULL CONSTRAINT DF_QDTuyenDung_TrangThai DEFAULT (N'DangThuViec'),
        CONSTRAINT FK_QDTuyenDung_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien(MaNhanVien),
        CONSTRAINT FK_QDTuyenDung_PhongBan FOREIGN KEY (MaPhongBan) REFERENCES dbo.PhongBan(MaPhongBan),
        CONSTRAINT CK_QDTuyenDung_Ngay CHECK (NgayKetThucThuViec >= NgayBatDauThuViec AND NgayKy <= NgayBatDauThuViec),
        CONSTRAINT CK_QDTuyenDung_Luong CHECK (LuongThuViec >= 0),
        CONSTRAINT CK_QDTuyenDung_TrangThai CHECK (TrangThai IN (N'DangThuViec', N'DaKetThucThuViec', N'DaKyHopDong', N'Huy'))
    );
END;
GO

/* 4. Hop dong lao dong */
IF OBJECT_ID(N'dbo.HopDongLaoDong', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.HopDongLaoDong
    (
        MaHopDong NVARCHAR(20) NOT NULL CONSTRAINT PK_HopDongLaoDong PRIMARY KEY,
        MaNhanVien NVARCHAR(20) NOT NULL,
        LoaiHopDong NVARCHAR(20) NOT NULL,
        NgayKy DATE NOT NULL,
        NgayBatDau DATE NOT NULL,
        NgayKetThuc DATE NULL,
        TrangThai NVARCHAR(30) NOT NULL CONSTRAINT DF_HopDong_TrangThai DEFAULT (N'HieuLuc'),
        CONSTRAINT FK_HopDong_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien(MaNhanVien),
        CONSTRAINT CK_HopDong_Loai CHECK (LoaiHopDong IN (N'DaiHan', N'NganHan')),
        CONSTRAINT CK_HopDong_Ngay CHECK (NgayKetThuc IS NULL OR NgayKetThuc >= NgayBatDau),
        CONSTRAINT CK_HopDong_TrangThai CHECK (TrangThai IN (N'HieuLuc', N'HetHieuLuc', N'Huy'))
    );
END;
GO

/* 5. Quyet dinh chuc vu */
IF OBJECT_ID(N'dbo.QuyetDinhChucVu', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.QuyetDinhChucVu
    (
        MaQDChucVu NVARCHAR(20) NOT NULL CONSTRAINT PK_QuyetDinhChucVu PRIMARY KEY,
        MaNhanVien NVARCHAR(20) NOT NULL,
        NgayKy DATE NOT NULL,
        TenChucVu NVARCHAR(120) NOT NULL,
        PhuCapChucVu DECIMAL(18,2) NOT NULL CONSTRAINT DF_QDChucVu_PhuCap DEFAULT (0),
        NgayHieuLuc DATE NOT NULL,
        NoiDung NVARCHAR(1000) NULL,
        TrangThai NVARCHAR(20) NOT NULL CONSTRAINT DF_QDChucVu_TrangThai DEFAULT (N'DaKy'),
        CONSTRAINT FK_QDChucVu_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien(MaNhanVien),
        CONSTRAINT CK_QDChucVu_PhuCap CHECK (PhuCapChucVu >= 0),
        CONSTRAINT CK_QDChucVu_TrangThai CHECK (TrangThai IN (N'ChoKy', N'DaKy', N'Huy'))
    );
END;
GO

/* 6. Quyet dinh luong */
IF OBJECT_ID(N'dbo.QuyetDinhLuong', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.QuyetDinhLuong
    (
        MaQDLuong NVARCHAR(20) NOT NULL CONSTRAINT PK_QuyetDinhLuong PRIMARY KEY,
        MaNhanVien NVARCHAR(20) NOT NULL,
        NgayKy DATE NOT NULL,
        LuongCoBan DECIMAL(18,2) NOT NULL,
        PhuCapLuong DECIMAL(18,2) NOT NULL CONSTRAINT DF_QDLuong_PhuCap DEFAULT (0),
        PhuCapDocHai DECIMAL(18,2) NOT NULL CONSTRAINT DF_QDLuong_DocHai DEFAULT (0),
        NgayHieuLuc DATE NOT NULL,
        NoiDung NVARCHAR(1000) NULL,
        TrangThai NVARCHAR(20) NOT NULL CONSTRAINT DF_QDLuong_TrangThai DEFAULT (N'DaKy'),
        CONSTRAINT FK_QDLuong_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien(MaNhanVien),
        CONSTRAINT CK_QDLuong_Tien CHECK (LuongCoBan >= 0 AND PhuCapLuong >= 0 AND PhuCapDocHai >= 0),
        CONSTRAINT CK_QDLuong_TrangThai CHECK (TrangThai IN (N'ChoKy', N'DaKy', N'Huy'))
    );
END;
GO

/* 7. Ho so bao hiem; MucDong la so tien khau tru/thang da duoc xac dinh boi don vi co tham quyen. */
IF OBJECT_ID(N'dbo.HoSoBaoHiem', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.HoSoBaoHiem
    (
        MaHoSoBaoHiem NVARCHAR(20) NOT NULL CONSTRAINT PK_HoSoBaoHiem PRIMARY KEY,
        MaNhanVien NVARCHAR(20) NOT NULL,
        MaHopDong NVARCHAR(20) NOT NULL,
        LoaiBaoHiem NVARCHAR(10) NOT NULL,
        MucDong DECIMAL(18,2) NOT NULL CONSTRAINT DF_HoSoBH_MucDong DEFAULT (0),
        NgayLap DATE NOT NULL,
        NgayBatDau DATE NOT NULL,
        NgayKetThuc DATE NULL,
        TrangThai NVARCHAR(30) NOT NULL CONSTRAINT DF_HoSoBH_TrangThai DEFAULT (N'ChoCapNhatMucDong'),
        CONSTRAINT FK_HoSoBH_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien(MaNhanVien),
        CONSTRAINT FK_HoSoBH_HopDong FOREIGN KEY (MaHopDong) REFERENCES dbo.HopDongLaoDong(MaHopDong),
        CONSTRAINT UQ_HoSoBH_HopDong_Loai UNIQUE (MaHopDong, LoaiBaoHiem),
        CONSTRAINT CK_HoSoBH_Loai CHECK (LoaiBaoHiem IN (N'BHYT', N'BHXH')),
        CONSTRAINT CK_HoSoBH_MucDong CHECK (MucDong >= 0),
        CONSTRAINT CK_HoSoBH_Ngay CHECK (NgayKetThuc IS NULL OR NgayKetThuc >= NgayBatDau),
        CONSTRAINT CK_HoSoBH_TrangThai CHECK (TrangThai IN (N'ChoCapNhatMucDong', N'DangThamGia', N'NgungThamGia'))
    );
END;
GO

/* 8. Ke hoach phan cong theo tuan */
IF OBJECT_ID(N'dbo.KeHoachPhanCong', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.KeHoachPhanCong
    (
        MaKeHoach NVARCHAR(20) NOT NULL CONSTRAINT PK_KeHoachPhanCong PRIMARY KEY,
        MaPhongBan NVARCHAR(20) NOT NULL,
        TuanBatDau DATE NOT NULL,
        TuanKetThuc DATE NOT NULL,
        NgayLap DATE NOT NULL,
        TrangThai NVARCHAR(20) NOT NULL CONSTRAINT DF_KeHoach_TrangThai DEFAULT (N'Nhap'),
        CONSTRAINT FK_KeHoach_PhongBan FOREIGN KEY (MaPhongBan) REFERENCES dbo.PhongBan(MaPhongBan),
        CONSTRAINT CK_KeHoach_Ngay CHECK (TuanKetThuc >= TuanBatDau),
        CONSTRAINT CK_KeHoach_TrangThai CHECK (TrangThai IN (N'Nhap', N'DaGuiChamCong', N'DaDong'))
    );
END;
GO

/* 9. Chi tiet phan cong ca */
IF OBJECT_ID(N'dbo.PhanCongCa', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PhanCongCa
    (
        MaPhanCong NVARCHAR(20) NOT NULL CONSTRAINT PK_PhanCongCa PRIMARY KEY,
        MaKeHoach NVARCHAR(20) NOT NULL,
        MaNhanVien NVARCHAR(20) NOT NULL,
        NgayLam DATE NOT NULL,
        CaLamViec NVARCHAR(50) NOT NULL,
        LoaiPhanCong NVARCHAR(20) NOT NULL CONSTRAINT DF_PhanCong_Loai DEFAULT (N'Thuong'),
        SoGioDuKien DECIMAL(5,2) NOT NULL CONSTRAINT DF_PhanCong_Gio DEFAULT (8),
        GhiChu NVARCHAR(500) NULL,
        CONSTRAINT FK_PhanCong_KeHoach FOREIGN KEY (MaKeHoach) REFERENCES dbo.KeHoachPhanCong(MaKeHoach),
        CONSTRAINT FK_PhanCong_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien(MaNhanVien),
        CONSTRAINT CK_PhanCong_Loai CHECK (LoaiPhanCong IN (N'Thuong', N'PhuTroi')),
        CONSTRAINT CK_PhanCong_Gio CHECK (SoGioDuKien > 0 AND SoGioDuKien <= 24),
        CONSTRAINT UQ_PhanCong UNIQUE (MaNhanVien, NgayLam, CaLamViec)
    );
END;
GO

/* 10. Bang cham cong thang */
IF OBJECT_ID(N'dbo.BangChamCong', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.BangChamCong
    (
        MaBangChamCong NVARCHAR(20) NOT NULL CONSTRAINT PK_BangChamCong PRIMARY KEY,
        Thang TINYINT NOT NULL,
        Nam SMALLINT NOT NULL,
        NgayLap DATE NOT NULL CONSTRAINT DF_BangChamCong_NgayLap DEFAULT (CONVERT(date, GETDATE())),
        TrangThai NVARCHAR(20) NOT NULL CONSTRAINT DF_BangChamCong_TrangThai DEFAULT (N'Nhap'),
        CONSTRAINT UQ_BangChamCong_ThangNam UNIQUE (Thang, Nam),
        CONSTRAINT CK_BangChamCong_Thang CHECK (Thang BETWEEN 1 AND 12),
        CONSTRAINT CK_BangChamCong_Nam CHECK (Nam BETWEEN 2000 AND 2100),
        CONSTRAINT CK_BangChamCong_TrangThai CHECK (TrangThai IN (N'Nhap', N'DaXacNhan', N'DaDong'))
    );
END;
GO

/* 11. Chi tiet cham cong theo nhan vien/ngay */
IF OBJECT_ID(N'dbo.ChiTietChamCong', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ChiTietChamCong
    (
        MaCTChamCong NVARCHAR(20) NOT NULL CONSTRAINT PK_ChiTietChamCong PRIMARY KEY,
        MaBangChamCong NVARCHAR(20) NOT NULL,
        MaNhanVien NVARCHAR(20) NOT NULL,
        Ngay DATE NOT NULL,
        SoGioCong DECIMAL(5,2) NOT NULL,
        TrangThaiCong NVARCHAR(30) NOT NULL CONSTRAINT DF_CTChamCong_TrangThai DEFAULT (N'DiLam'),
        GhiChu NVARCHAR(500) NULL,
        CONSTRAINT FK_CTChamCong_Bang FOREIGN KEY (MaBangChamCong) REFERENCES dbo.BangChamCong(MaBangChamCong),
        CONSTRAINT FK_CTChamCong_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien(MaNhanVien),
        CONSTRAINT CK_CTChamCong_Gio CHECK (SoGioCong >= 0 AND SoGioCong <= 24),
        CONSTRAINT CK_CTChamCong_TrangThai CHECK (TrangThaiCong IN (N'DiLam', N'NghiPhep', N'NghiKhongLuong', N'NghiLe')),
        CONSTRAINT UQ_CTChamCong_Ngay UNIQUE (MaBangChamCong, MaNhanVien, Ngay)
    );
END;
GO

/* 12. Phieu phu troi */
IF OBJECT_ID(N'dbo.PhieuPhuTroi', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PhieuPhuTroi
    (
        SoPhieuPhuTroi NVARCHAR(20) NOT NULL CONSTRAINT PK_PhieuPhuTroi PRIMARY KEY,
        NgayPhuTroi DATE NOT NULL,
        HinhThucPhuTroi NVARCHAR(20) NOT NULL,
        NgayLap DATE NOT NULL CONSTRAINT DF_PhieuPhuTroi_NgayLap DEFAULT (CONVERT(date, GETDATE())),
        TrangThai NVARCHAR(20) NOT NULL CONSTRAINT DF_PhieuPhuTroi_TrangThai DEFAULT (N'ChoXacNhan'),
        CONSTRAINT CK_PhieuPhuTroi_HinhThuc CHECK (HinhThucPhuTroi IN (N'TangCa', N'NgayLe', N'NgayNghi')),
        CONSTRAINT CK_PhieuPhuTroi_TrangThai CHECK (TrangThai IN (N'ChoXacNhan', N'DaXacNhan', N'Huy'))
    );
END;
GO

/* 13. Chi tiet nhan vien tham gia phu troi */
IF OBJECT_ID(N'dbo.ChiTietPhuTroi', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ChiTietPhuTroi
    (
        MaCTPhuTroi NVARCHAR(20) NOT NULL CONSTRAINT PK_ChiTietPhuTroi PRIMARY KEY,
        SoPhieuPhuTroi NVARCHAR(20) NOT NULL,
        MaNhanVien NVARCHAR(20) NOT NULL,
        SoGioPhuTroi DECIMAL(5,2) NOT NULL,
        HeSoTinhGio DECIMAL(4,2) NOT NULL CONSTRAINT DF_CTPhuTroi_HeSo DEFAULT (1.5),
        TrangThaiXacNhan NVARCHAR(20) NOT NULL CONSTRAINT DF_CTPhuTroi_TrangThai DEFAULT (N'ChoXacNhan'),
        CONSTRAINT FK_CTPhuTroi_Phieu FOREIGN KEY (SoPhieuPhuTroi) REFERENCES dbo.PhieuPhuTroi(SoPhieuPhuTroi),
        CONSTRAINT FK_CTPhuTroi_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien(MaNhanVien),
        CONSTRAINT UQ_CTPhuTroi_NhanVien UNIQUE (SoPhieuPhuTroi, MaNhanVien),
        CONSTRAINT CK_CTPhuTroi_Gio CHECK (SoGioPhuTroi > 0 AND SoGioPhuTroi <= 24),
        CONSTRAINT CK_CTPhuTroi_HeSo CHECK (HeSoTinhGio = 1.5),
        CONSTRAINT CK_CTPhuTroi_TrangThai CHECK (TrangThaiXacNhan IN (N'ChoXacNhan', N'DaXacNhan'))
    );
END;
GO

/* 14. Bang luong */
IF OBJECT_ID(N'dbo.BangLuong', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.BangLuong
    (
        MaBangLuong NVARCHAR(20) NOT NULL CONSTRAINT PK_BangLuong PRIMARY KEY,
        Thang TINYINT NOT NULL,
        Nam SMALLINT NOT NULL,
        NgayLap DATE NOT NULL CONSTRAINT DF_BangLuong_NgayLap DEFAULT (CONVERT(date, GETDATE())),
        TrangThai NVARCHAR(20) NOT NULL CONSTRAINT DF_BangLuong_TrangThai DEFAULT (N'Nhap'),
        CONSTRAINT UQ_BangLuong_ThangNam UNIQUE (Thang, Nam),
        CONSTRAINT CK_BangLuong_Thang CHECK (Thang BETWEEN 1 AND 12),
        CONSTRAINT CK_BangLuong_Nam CHECK (Nam BETWEEN 2000 AND 2100),
        CONSTRAINT CK_BangLuong_TrangThai CHECK (TrangThai IN (N'Nhap', N'DaChot'))
    );
END;
GO

/* 15. Chi tiet luong */
IF OBJECT_ID(N'dbo.ChiTietBangLuong', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ChiTietBangLuong
    (
        MaCTBangLuong NVARCHAR(30) NOT NULL CONSTRAINT PK_ChiTietBangLuong PRIMARY KEY,
        MaBangLuong NVARCHAR(20) NOT NULL,
        MaNhanVien NVARCHAR(20) NOT NULL,
        MaQDLuong NVARCHAR(20) NOT NULL,
        MaQDChucVu NVARCHAR(20) NULL,
        SoGioCong DECIMAL(7,2) NOT NULL CONSTRAINT DF_CTBL_GioCong DEFAULT (0),
        SoGioPhuTroiQuyDoi DECIMAL(7,2) NOT NULL CONSTRAINT DF_CTBL_GioOT DEFAULT (0),
        LuongCoBanApDung DECIMAL(18,2) NOT NULL,
        PhuCapLuong DECIMAL(18,2) NOT NULL CONSTRAINT DF_CTBL_PCLuong DEFAULT (0),
        PhuCapDocHai DECIMAL(18,2) NOT NULL CONSTRAINT DF_CTBL_PCDocHai DEFAULT (0),
        PhuCapChucVu DECIMAL(18,2) NOT NULL CONSTRAINT DF_CTBL_PCChucVu DEFAULT (0),
        TienPhuTroi DECIMAL(18,2) NOT NULL CONSTRAINT DF_CTBL_TienOT DEFAULT (0),
        KhauTruBHYT DECIMAL(18,2) NOT NULL CONSTRAINT DF_CTBL_BHYT DEFAULT (0),
        KhauTruBHXH DECIMAL(18,2) NOT NULL CONSTRAINT DF_CTBL_BHXH DEFAULT (0),
        TienThuong DECIMAL(18,2) NOT NULL CONSTRAINT DF_CTBL_Thuong DEFAULT (0),
        TienPhat DECIMAL(18,2) NOT NULL CONSTRAINT DF_CTBL_Phat DEFAULT (0),
        TongThuNhap DECIMAL(18,2) NOT NULL CONSTRAINT DF_CTBL_TongThuNhap DEFAULT (0),
        ThucLinh DECIMAL(18,2) NOT NULL CONSTRAINT DF_CTBL_ThucLinh DEFAULT (0),
        CONSTRAINT FK_CTBL_BangLuong FOREIGN KEY (MaBangLuong) REFERENCES dbo.BangLuong(MaBangLuong),
        CONSTRAINT FK_CTBL_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien(MaNhanVien),
        CONSTRAINT FK_CTBL_QDLuong FOREIGN KEY (MaQDLuong) REFERENCES dbo.QuyetDinhLuong(MaQDLuong),
        CONSTRAINT FK_CTBL_QDChucVu FOREIGN KEY (MaQDChucVu) REFERENCES dbo.QuyetDinhChucVu(MaQDChucVu),
        CONSTRAINT UQ_CTBL_NhanVien UNIQUE (MaBangLuong, MaNhanVien),
        CONSTRAINT CK_CTBL_Tien CHECK (LuongCoBanApDung >= 0 AND PhuCapLuong >= 0 AND PhuCapDocHai >= 0 AND PhuCapChucVu >= 0 AND TienPhuTroi >= 0 AND KhauTruBHYT >= 0 AND KhauTruBHXH >= 0 AND TienThuong >= 0 AND TienPhat >= 0 AND TongThuNhap >= 0 AND ThucLinh >= 0)
    );
END;
GO

/* 16. Quyet dinh khen thuong; doi tuong lien ket qua bang N-N o phia duoi. */
IF OBJECT_ID(N'dbo.QuyetDinhKhenThuong', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.QuyetDinhKhenThuong
    (
        MaQDKhenThuong NVARCHAR(20) NOT NULL CONSTRAINT PK_QuyetDinhKhenThuong PRIMARY KEY,
        NgayLap DATE NOT NULL,
        NgayKy DATE NULL,
        LoaiDoiTuongApDung NVARCHAR(20) NOT NULL,
        LyDo NVARCHAR(500) NOT NULL,
        NoiDung NVARCHAR(1000) NULL,
        SoTienKhenThuong DECIMAL(18,2) NOT NULL,
        TrangThai NVARCHAR(20) NOT NULL CONSTRAINT DF_QDKT_TrangThai DEFAULT (N'ChoKy'),
        CONSTRAINT CK_QDKT_Loai CHECK (LoaiDoiTuongApDung IN (N'CaNhan', N'TapThe')),
        CONSTRAINT CK_QDKT_Tien CHECK (SoTienKhenThuong >= 0),
        CONSTRAINT CK_QDKT_TrangThai CHECK (TrangThai IN (N'ChoKy', N'DaKy', N'Huy'))
    );
END;
GO

/* 17. Phieu chi khen thuong */
IF OBJECT_ID(N'dbo.PhieuChiKhenThuong', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PhieuChiKhenThuong
    (
        MaPhieuChi NVARCHAR(20) NOT NULL CONSTRAINT PK_PhieuChiKhenThuong PRIMARY KEY,
        MaQDKhenThuong NVARCHAR(20) NOT NULL,
        NgayChi DATE NOT NULL,
        SoTienChi DECIMAL(18,2) NOT NULL,
        PhuongThucChi NVARCHAR(30) NOT NULL,
        TrangThai NVARCHAR(20) NOT NULL CONSTRAINT DF_PhieuChiKT_TrangThai DEFAULT (N'DaLap'),
        CONSTRAINT FK_PhieuChiKT_QDKT FOREIGN KEY (MaQDKhenThuong) REFERENCES dbo.QuyetDinhKhenThuong(MaQDKhenThuong),
        CONSTRAINT UQ_PhieuChiKT_QDKT UNIQUE (MaQDKhenThuong),
        CONSTRAINT CK_PhieuChiKT_Tien CHECK (SoTienChi >= 0),
        CONSTRAINT CK_PhieuChiKT_PhuongThuc CHECK (PhuongThucChi IN (N'TienMat', N'ChuyenKhoan')),
        CONSTRAINT CK_PhieuChiKT_TrangThai CHECK (TrangThai IN (N'DaLap', N'DaChi', N'Huy'))
    );
END;
GO

/* 18. Quyet dinh ky luat */
IF OBJECT_ID(N'dbo.QuyetDinhKyLuat', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.QuyetDinhKyLuat
    (
        MaQDKyLuat NVARCHAR(20) NOT NULL CONSTRAINT PK_QuyetDinhKyLuat PRIMARY KEY,
        NgayLap DATE NOT NULL,
        NgayKy DATE NULL,
        LoaiDoiTuongApDung NVARCHAR(20) NOT NULL,
        LyDo NVARCHAR(500) NOT NULL,
        NoiDung NVARCHAR(1000) NULL,
        SoTienPhat DECIMAL(18,2) NOT NULL,
        HinhThucXuLy NVARCHAR(20) NOT NULL,
        TrangThai NVARCHAR(20) NOT NULL CONSTRAINT DF_QDKL_TrangThai DEFAULT (N'ChoKy'),
        CONSTRAINT CK_QDKL_Loai CHECK (LoaiDoiTuongApDung IN (N'CaNhan', N'TapThe')),
        CONSTRAINT CK_QDKL_Tien CHECK (SoTienPhat >= 0),
        CONSTRAINT CK_QDKL_HinhThuc CHECK (HinhThucXuLy IN (N'NopPhat', N'TruLuong')),
        CONSTRAINT CK_QDKL_TrangThai CHECK (TrangThai IN (N'ChoKy', N'DaKy', N'Huy'))
    );
END;
GO

/* 19. Phieu thu nop phat */
IF OBJECT_ID(N'dbo.PhieuThuNopPhat', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PhieuThuNopPhat
    (
        MaPhieuThu NVARCHAR(20) NOT NULL CONSTRAINT PK_PhieuThuNopPhat PRIMARY KEY,
        MaQDKyLuat NVARCHAR(20) NOT NULL,
        NgayThu DATE NOT NULL,
        SoTienThu DECIMAL(18,2) NOT NULL,
        TrangThai NVARCHAR(20) NOT NULL CONSTRAINT DF_PhieuThuPhat_TrangThai DEFAULT (N'DaLap'),
        CONSTRAINT FK_PhieuThuPhat_QDKL FOREIGN KEY (MaQDKyLuat) REFERENCES dbo.QuyetDinhKyLuat(MaQDKyLuat),
        CONSTRAINT UQ_PhieuThuPhat_QDKL UNIQUE (MaQDKyLuat),
        CONSTRAINT CK_PhieuThuPhat_Tien CHECK (SoTienThu >= 0),
        CONSTRAINT CK_PhieuThuPhat_TrangThai CHECK (TrangThai IN (N'DaLap', N'DaThu', N'Huy'))
    );
END;
GO

/* 20-23. Bang lien ket de mo hinh hoa quan he N-N trong Class Diagram. */
IF OBJECT_ID(N'dbo.QDKhenThuong_NhanVien', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.QDKhenThuong_NhanVien
    (
        MaQDKhenThuong NVARCHAR(20) NOT NULL,
        MaNhanVien NVARCHAR(20) NOT NULL,
        CONSTRAINT PK_QDKhenThuong_NhanVien PRIMARY KEY (MaQDKhenThuong, MaNhanVien),
        CONSTRAINT FK_QDKTNV_QDKT FOREIGN KEY (MaQDKhenThuong) REFERENCES dbo.QuyetDinhKhenThuong(MaQDKhenThuong),
        CONSTRAINT FK_QDKTNV_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien(MaNhanVien)
    );
END;
GO
IF OBJECT_ID(N'dbo.QDKhenThuong_PhongBan', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.QDKhenThuong_PhongBan
    (
        MaQDKhenThuong NVARCHAR(20) NOT NULL,
        MaPhongBan NVARCHAR(20) NOT NULL,
        CONSTRAINT PK_QDKhenThuong_PhongBan PRIMARY KEY (MaQDKhenThuong, MaPhongBan),
        CONSTRAINT FK_QDKTPB_QDKT FOREIGN KEY (MaQDKhenThuong) REFERENCES dbo.QuyetDinhKhenThuong(MaQDKhenThuong),
        CONSTRAINT FK_QDKTPB_PhongBan FOREIGN KEY (MaPhongBan) REFERENCES dbo.PhongBan(MaPhongBan)
    );
END;
GO
IF OBJECT_ID(N'dbo.QDKyLuat_NhanVien', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.QDKyLuat_NhanVien
    (
        MaQDKyLuat NVARCHAR(20) NOT NULL,
        MaNhanVien NVARCHAR(20) NOT NULL,
        CONSTRAINT PK_QDKyLuat_NhanVien PRIMARY KEY (MaQDKyLuat, MaNhanVien),
        CONSTRAINT FK_QDKLNV_QDKL FOREIGN KEY (MaQDKyLuat) REFERENCES dbo.QuyetDinhKyLuat(MaQDKyLuat),
        CONSTRAINT FK_QDKLNV_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien(MaNhanVien)
    );
END;
GO
IF OBJECT_ID(N'dbo.QDKyLuat_PhongBan', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.QDKyLuat_PhongBan
    (
        MaQDKyLuat NVARCHAR(20) NOT NULL,
        MaPhongBan NVARCHAR(20) NOT NULL,
        CONSTRAINT PK_QDKyLuat_PhongBan PRIMARY KEY (MaQDKyLuat, MaPhongBan),
        CONSTRAINT FK_QDKLPB_QDKL FOREIGN KEY (MaQDKyLuat) REFERENCES dbo.QuyetDinhKyLuat(MaQDKyLuat),
        CONSTRAINT FK_QDKLPB_PhongBan FOREIGN KEY (MaPhongBan) REFERENCES dbo.PhongBan(MaPhongBan)
    );
END;
GO

/* Trigger: loai doi tuong trong quyet dinh phai khop bang lien ket. */
CREATE OR ALTER TRIGGER dbo.TR_QDKhenThuong_NhanVien_Loai
ON dbo.QDKhenThuong_NhanVien
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM inserted i JOIN dbo.QuyetDinhKhenThuong q ON q.MaQDKhenThuong = i.MaQDKhenThuong WHERE q.LoaiDoiTuongApDung <> N'CaNhan')
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51004, N'Quyet dinh dang TapThe khong the lien ket voi nhan vien qua bang lien ket ca nhan.', 1;
    END;
END;
GO
CREATE OR ALTER TRIGGER dbo.TR_QDKhenThuong_PhongBan_Loai
ON dbo.QDKhenThuong_PhongBan
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM inserted i JOIN dbo.QuyetDinhKhenThuong q ON q.MaQDKhenThuong = i.MaQDKhenThuong WHERE q.LoaiDoiTuongApDung <> N'TapThe')
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51005, N'Quyet dinh dang CaNhan khong the lien ket voi phong ban qua bang lien ket tap the.', 1;
    END;
END;
GO
CREATE OR ALTER TRIGGER dbo.TR_QDKyLuat_NhanVien_Loai
ON dbo.QDKyLuat_NhanVien
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM inserted i JOIN dbo.QuyetDinhKyLuat q ON q.MaQDKyLuat = i.MaQDKyLuat WHERE q.LoaiDoiTuongApDung <> N'CaNhan')
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51006, N'Quyet dinh ky luat dang TapThe khong the lien ket vao bang ca nhan.', 1;
    END;
END;
GO
CREATE OR ALTER TRIGGER dbo.TR_QDKyLuat_PhongBan_Loai
ON dbo.QDKyLuat_PhongBan
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM inserted i JOIN dbo.QuyetDinhKyLuat q ON q.MaQDKyLuat = i.MaQDKyLuat WHERE q.LoaiDoiTuongApDung <> N'TapThe')
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51007, N'Quyet dinh ky luat dang CaNhan khong the lien ket vao bang tap the.', 1;
    END;
END;
GO
/* Bao ve chieu nguoc: khong cho doi loai doi tuong neu da co lien ket khong phu hop. */
CREATE OR ALTER TRIGGER dbo.TR_QDKhenThuong_KhongDoiLoaiSai
ON dbo.QuyetDinhKhenThuong
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(LoaiDoiTuongApDung) AND EXISTS
    (
        SELECT 1
        FROM inserted i
        WHERE (i.LoaiDoiTuongApDung = N'CaNhan' AND EXISTS (SELECT 1 FROM dbo.QDKhenThuong_PhongBan x WHERE x.MaQDKhenThuong = i.MaQDKhenThuong))
           OR (i.LoaiDoiTuongApDung = N'TapThe' AND EXISTS (SELECT 1 FROM dbo.QDKhenThuong_NhanVien x WHERE x.MaQDKhenThuong = i.MaQDKhenThuong))
    )
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51025, N'Khong the doi loai doi tuong khen thuong khi dang co lien ket khong tuong ung.', 1;
    END;
END;
GO

CREATE OR ALTER TRIGGER dbo.TR_QDKyLuat_KhongDoiLoaiSai
ON dbo.QuyetDinhKyLuat
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(LoaiDoiTuongApDung) AND EXISTS
    (
        SELECT 1
        FROM inserted i
        WHERE (i.LoaiDoiTuongApDung = N'CaNhan' AND EXISTS (SELECT 1 FROM dbo.QDKyLuat_PhongBan x WHERE x.MaQDKyLuat = i.MaQDKyLuat))
           OR (i.LoaiDoiTuongApDung = N'TapThe' AND EXISTS (SELECT 1 FROM dbo.QDKyLuat_NhanVien x WHERE x.MaQDKyLuat = i.MaQDKyLuat))
    )
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51026, N'Khong the doi loai doi tuong ky luat khi dang co lien ket khong tuong ung.', 1;
    END;
END;
GO

/* Trigger: chi tiet cham cong phai nam trong thang/nam cua bang cham cong. */
CREATE OR ALTER TRIGGER dbo.TR_ChiTietChamCong_KhopKy
ON dbo.ChiTietChamCong
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS
    (
        SELECT 1 FROM inserted i
        INNER JOIN dbo.BangChamCong b ON b.MaBangChamCong = i.MaBangChamCong
        WHERE MONTH(i.Ngay) <> b.Thang OR YEAR(i.Ngay) <> b.Nam
    )
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51008, N'Ngay cham cong phai nam trong thang/nam cua bang cham cong.', 1;
    END;
END;
GO

/* Trigger: quyet dinh luong/chuc vu trong chi tiet luong phai thuoc cung nhan vien. */
CREATE OR ALTER TRIGGER dbo.TR_ChiTietBangLuong_DecisionEmployee
ON dbo.ChiTietBangLuong
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        INNER JOIN dbo.QuyetDinhLuong ql ON ql.MaQDLuong = i.MaQDLuong
        WHERE ql.MaNhanVien <> i.MaNhanVien
    )
    OR EXISTS
    (
        SELECT 1
        FROM inserted i
        INNER JOIN dbo.QuyetDinhChucVu qcv ON qcv.MaQDChucVu = i.MaQDChucVu
        WHERE qcv.MaNhanVien <> i.MaNhanVien
    )
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51009, N'Quyet dinh luong/chuc vu phai thuoc cung nhan vien voi dong bang luong.', 1;
    END;
END;
GO

/* Trigger: chi lap ho so BHYT/BHXH cho hop dong dai han. */
CREATE OR ALTER TRIGGER dbo.TR_HoSoBaoHiem_ChiHopDongDaiHan
ON dbo.HoSoBaoHiem
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        INNER JOIN dbo.HopDongLaoDong h ON h.MaHopDong = i.MaHopDong
        WHERE h.LoaiHopDong <> N'DaiHan' OR h.MaNhanVien <> i.MaNhanVien
    )
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51001, N'Chi lap ho so BHYT/BHXH cho hop dong dai han.', 1;
    END;
END;
GO

/* Trigger: chi duoc lap phieu chi khi quyet dinh khen thuong da ky. */
CREATE OR ALTER TRIGGER dbo.TR_PhieuChiKT_PhaiCoQuyetDinhDaKy
ON dbo.PhieuChiKhenThuong
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        INNER JOIN dbo.QuyetDinhKhenThuong q ON q.MaQDKhenThuong = i.MaQDKhenThuong
        WHERE q.TrangThai <> N'DaKy' OR i.SoTienChi <> q.SoTienKhenThuong OR i.NgayChi < q.NgayKy
    )
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51002, N'Quyet dinh phai da ky va so tien chi phai khop so tien khen thuong.', 1;
    END;
END;
GO

/* Trigger: phieu thu nop phat chi duoc lap cho quyet dinh da ky, hinh thuc nop phat. */
CREATE OR ALTER TRIGGER dbo.TR_PhieuThuPhat_PhaiCoQuyetDinhDaKy
ON dbo.PhieuThuNopPhat
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        INNER JOIN dbo.QuyetDinhKyLuat q ON q.MaQDKyLuat = i.MaQDKyLuat
        WHERE q.TrangThai <> N'DaKy' OR q.HinhThucXuLy <> N'NopPhat' OR i.SoTienThu <> q.SoTienPhat OR i.NgayThu < q.NgayKy
    )
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51003, N'Phieu thu phai thuoc quyet dinh da ky co hinh thuc nop phat va dung so tien.', 1;
    END;
END;
GO

/* Lap quyet dinh khen thuong o trang thai cho ky. */
CREATE OR ALTER PROCEDURE dbo.sp_LapQuyetDinhKhenThuong
    @MaQDKhenThuong NVARCHAR(20),
    @NgayLap DATE,
    @LoaiDoiTuongApDung NVARCHAR(20),
    @MaNhanVien NVARCHAR(20) = NULL,
    @MaPhongBan NVARCHAR(20) = NULL,
    @LyDo NVARCHAR(500),
    @NoiDung NVARCHAR(1000) = NULL,
    @SoTienKhenThuong DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    IF @LoaiDoiTuongApDung NOT IN (N'CaNhan', N'TapThe')
        THROW 51010, N'Loai doi tuong khen thuong khong hop le.', 1;
    IF @LoaiDoiTuongApDung = N'CaNhan' AND (@MaNhanVien IS NULL OR @MaPhongBan IS NOT NULL)
        THROW 51011, N'Khen thuong ca nhan phai co MaNhanVien va khong co MaPhongBan.', 1;
    IF @LoaiDoiTuongApDung = N'TapThe' AND (@MaPhongBan IS NULL OR @MaNhanVien IS NOT NULL)
        THROW 51012, N'Khen thuong tap the phai co MaPhongBan va khong co MaNhanVien.', 1;
    IF @LoaiDoiTuongApDung = N'CaNhan' AND NOT EXISTS (SELECT 1 FROM dbo.NhanVien WHERE MaNhanVien = @MaNhanVien)
        THROW 51016, N'Nhan vien duoc khen thuong khong ton tai.', 1;
    IF @LoaiDoiTuongApDung = N'TapThe' AND NOT EXISTS (SELECT 1 FROM dbo.PhongBan WHERE MaPhongBan = @MaPhongBan)
        THROW 51017, N'Phong ban/tap the duoc khen thuong khong ton tai.', 1;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
    INSERT dbo.QuyetDinhKhenThuong
        (MaQDKhenThuong, NgayLap, LoaiDoiTuongApDung, LyDo, NoiDung, SoTienKhenThuong, TrangThai)
    VALUES
        (@MaQDKhenThuong, @NgayLap, @LoaiDoiTuongApDung, @LyDo, @NoiDung, @SoTienKhenThuong, N'ChoKy');
    IF @LoaiDoiTuongApDung = N'CaNhan'
        INSERT dbo.QDKhenThuong_NhanVien (MaQDKhenThuong, MaNhanVien) VALUES (@MaQDKhenThuong, @MaNhanVien);
    ELSE
        INSERT dbo.QDKhenThuong_PhongBan (MaQDKhenThuong, MaPhongBan) VALUES (@MaQDKhenThuong, @MaPhongBan);
    COMMIT TRANSACTION;
END;
GO

/* Ky quyet dinh khen thuong. */
CREATE OR ALTER PROCEDURE dbo.sp_KyQuyetDinhKhenThuong
    @MaQDKhenThuong NVARCHAR(20),
    @NgayKy DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET @NgayKy = ISNULL(@NgayKy, CONVERT(date, GETDATE()));
    IF EXISTS (SELECT 1 FROM dbo.QuyetDinhKhenThuong WHERE MaQDKhenThuong = @MaQDKhenThuong AND @NgayKy < NgayLap)
        THROW 51018, N'Ngay ky khong duoc truoc ngay lap quyet dinh.', 1;
    UPDATE dbo.QuyetDinhKhenThuong
       SET TrangThai = N'DaKy', NgayKy = @NgayKy
     WHERE MaQDKhenThuong = @MaQDKhenThuong AND TrangThai = N'ChoKy';
    IF @@ROWCOUNT = 0
        THROW 51013, N'Khong tim thay quyet dinh dang cho ky hoac quyet dinh khong o trang thai hop le.', 1;
END;
GO

/* Lap phieu chi tu quyet dinh da ky; so tien lay tu quyet dinh, khong tu nhap lai. */
CREATE OR ALTER PROCEDURE dbo.sp_LapPhieuChiKhenThuong
    @MaPhieuChi NVARCHAR(20),
    @MaQDKhenThuong NVARCHAR(20),
    @NgayChi DATE,
    @PhuongThucChi NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
    IF NOT EXISTS (SELECT 1 FROM dbo.QuyetDinhKhenThuong WHERE MaQDKhenThuong = @MaQDKhenThuong AND TrangThai = N'DaKy')
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51014, N'Chi lap phieu chi khi quyet dinh khen thuong da ky.', 1;
    END;
    IF EXISTS (SELECT 1 FROM dbo.PhieuChiKhenThuong WHERE MaQDKhenThuong = @MaQDKhenThuong)
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51015, N'Quyet dinh nay da co phieu chi.', 1;
    END;
    INSERT dbo.PhieuChiKhenThuong (MaPhieuChi, MaQDKhenThuong, NgayChi, SoTienChi, PhuongThucChi, TrangThai)
    SELECT @MaPhieuChi, MaQDKhenThuong, @NgayChi, SoTienKhenThuong, @PhuongThucChi, N'DaLap'
    FROM dbo.QuyetDinhKhenThuong
    WHERE MaQDKhenThuong = @MaQDKhenThuong;
    COMMIT TRANSACTION;
END;
GO

/*
  Tao bang luong minh hoa. Chinh sach mau:
  - Gio chuan thang truyen vao tu App.config, mac dinh 208 gio (KHONG phai quy dinh trong de bai).
  - Thu nhap co dinh = (luong co ban + cac phu cap) * MIN(gio cong/gio chuan, 1).
  - Tien phu troi = luong co ban/gio chuan * gio phu troi thuc te * he so 1.5.
  - Bao hiem lay theo MucDong da duoc nhap/xac nhan trong HoSoBaoHiem.
  - TienThuong trong chi tiet luong de 0 theo mac dinh; quyet dinh khen thuong duoc chi qua PhieuChiKhenThuong theo mo ta bai.
  - Chi khau tru khoan phat ca nhan co HinhThucXuLy = TruLuong duoc ky trong thang; khong tu phan bo phat tap the.
  Can duoc thay bang quy tac da duoc cong ty phe duyet truoc khi su dung thuc te.
*/
CREATE OR ALTER PROCEDURE dbo.sp_TaoBangLuongThang
    @Thang TINYINT,
    @Nam SMALLINT,
    @GioChuanThang DECIMAL(7,2) = 208
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF @Thang NOT BETWEEN 1 AND 12 OR @Nam NOT BETWEEN 2000 AND 2100 OR @GioChuanThang <= 0
        THROW 51020, N'Thang, nam hoac gio chuan thang khong hop le.', 1;

    DECLARE @NgayDau DATE = DATEFROMPARTS(@Nam, @Thang, 1);
    DECLARE @NgayCuoi DATE = EOMONTH(@NgayDau);
    DECLARE @MaBangLuong NVARCHAR(20) = N'BL-' + CONVERT(NVARCHAR(6), @Nam * 100 + @Thang);

    IF EXISTS (SELECT 1 FROM dbo.BangLuong WHERE Thang = @Thang AND Nam = @Nam AND TrangThai = N'DaChot')
        THROW 51021, N'Bang luong da chot, khong the tao lai.', 1;

    IF EXISTS
    (
        SELECT 1 FROM dbo.NhanVien n
        WHERE n.TrangThaiNhanVien = N'DangLam'
          AND NOT EXISTS
          (
              SELECT 1 FROM dbo.QuyetDinhLuong q
              WHERE q.MaNhanVien = n.MaNhanVien AND q.TrangThai = N'DaKy' AND q.NgayHieuLuc <= @NgayCuoi
          )
    )
        THROW 51022, N'Co nhan vien dang lam chua co quyet dinh luong da ky co hieu luc trong ky.', 1;

    /* Khong tu chia mot so tien quyet dinh phat cho nhieu nhan vien khi de bai chua quy dinh cach phan bo. */
    IF EXISTS
    (
        SELECT q.MaQDKyLuat
        FROM dbo.QuyetDinhKyLuat q
        INNER JOIN dbo.QDKyLuat_NhanVien t ON t.MaQDKyLuat = q.MaQDKyLuat
        WHERE q.TrangThai = N'DaKy' AND q.HinhThucXuLy = N'TruLuong'
          AND q.NgayKy >= @NgayDau AND q.NgayKy <= @NgayCuoi
        GROUP BY q.MaQDKyLuat
        HAVING COUNT(*) > 1
    )
        THROW 51024, N'Co quyet dinh phat lien ket nhieu nhan vien; can quy dinh cach phan bo truoc khi tinh luong.', 1;

    BEGIN TRANSACTION;

    IF EXISTS (SELECT 1 FROM dbo.BangLuong WHERE MaBangLuong = @MaBangLuong)
    BEGIN
        DELETE FROM dbo.ChiTietBangLuong WHERE MaBangLuong = @MaBangLuong;
        UPDATE dbo.BangLuong SET NgayLap = CONVERT(date, GETDATE()), TrangThai = N'Nhap' WHERE MaBangLuong = @MaBangLuong;
    END
    ELSE
    BEGIN
        INSERT dbo.BangLuong (MaBangLuong, Thang, Nam, NgayLap, TrangThai)
        VALUES (@MaBangLuong, @Thang, @Nam, CONVERT(date, GETDATE()), N'Nhap');
    END;

    ;WITH Nguon AS
    (
        SELECT
            n.MaNhanVien,
            ql.MaQDLuong,
            qcv.MaQDChucVu,
            ISNULL(ql.LuongCoBan, 0) AS LuongCoBan,
            ISNULL(ql.PhuCapLuong, 0) AS PhuCapLuong,
            ISNULL(ql.PhuCapDocHai, 0) AS PhuCapDocHai,
            ISNULL(qcv.PhuCapChucVu, 0) AS PhuCapChucVu,
            ISNULL(cc.SoGioCong, 0) AS SoGioCong,
            ISNULL(ot.SoGioPhuTroi, 0) AS SoGioPhuTroi,
            ISNULL(bh.KhauTruBHYT, 0) AS KhauTruBHYT,
            ISNULL(bh.KhauTruBHXH, 0) AS KhauTruBHXH,
            ISNULL(kl.TienPhat, 0) AS TienPhat,
            CAST(0 AS DECIMAL(18,2)) AS TienThuong
        FROM dbo.NhanVien n
        CROSS APPLY
        (
            SELECT TOP (1) q.*
            FROM dbo.QuyetDinhLuong q
            WHERE q.MaNhanVien = n.MaNhanVien AND q.TrangThai = N'DaKy' AND q.NgayHieuLuc <= @NgayCuoi
            ORDER BY q.NgayHieuLuc DESC, q.NgayKy DESC, q.MaQDLuong DESC
        ) ql
        OUTER APPLY
        (
            SELECT TOP (1) q.*
            FROM dbo.QuyetDinhChucVu q
            WHERE q.MaNhanVien = n.MaNhanVien AND q.TrangThai = N'DaKy' AND q.NgayHieuLuc <= @NgayCuoi
            ORDER BY q.NgayHieuLuc DESC, q.NgayKy DESC, q.MaQDChucVu DESC
        ) qcv
        OUTER APPLY
        (
            SELECT SUM(c.SoGioCong) AS SoGioCong
            FROM dbo.ChiTietChamCong c
            INNER JOIN dbo.BangChamCong b ON b.MaBangChamCong = c.MaBangChamCong
            WHERE c.MaNhanVien = n.MaNhanVien AND c.Ngay >= @NgayDau AND c.Ngay <= @NgayCuoi
              AND b.TrangThai IN (N'DaXacNhan', N'DaDong')
        ) cc
        OUTER APPLY
        (
            SELECT SUM(c.SoGioPhuTroi) AS SoGioPhuTroi
            FROM dbo.ChiTietPhuTroi c
            INNER JOIN dbo.PhieuPhuTroi p ON p.SoPhieuPhuTroi = c.SoPhieuPhuTroi
            WHERE c.MaNhanVien = n.MaNhanVien
              AND p.NgayPhuTroi >= @NgayDau AND p.NgayPhuTroi <= @NgayCuoi
              AND p.TrangThai = N'DaXacNhan' AND c.TrangThaiXacNhan = N'DaXacNhan'
        ) ot
        OUTER APPLY
        (
            SELECT
                SUM(CASE WHEN h.LoaiBaoHiem = N'BHYT' THEN h.MucDong ELSE 0 END) AS KhauTruBHYT,
                SUM(CASE WHEN h.LoaiBaoHiem = N'BHXH' THEN h.MucDong ELSE 0 END) AS KhauTruBHXH
            FROM dbo.HoSoBaoHiem h
            WHERE h.MaNhanVien = n.MaNhanVien AND h.TrangThai = N'DangThamGia'
              AND h.NgayBatDau <= @NgayCuoi AND (h.NgayKetThuc IS NULL OR h.NgayKetThuc >= @NgayDau)
        ) bh
        OUTER APPLY
        (
            SELECT SUM(q.SoTienPhat) AS TienPhat
            FROM dbo.QuyetDinhKyLuat q
            INNER JOIN dbo.QDKyLuat_NhanVien target ON target.MaQDKyLuat = q.MaQDKyLuat
            WHERE target.MaNhanVien = n.MaNhanVien AND q.TrangThai = N'DaKy'
              AND q.HinhThucXuLy = N'TruLuong'
              AND q.NgayKy >= @NgayDau AND q.NgayKy <= @NgayCuoi
        ) kl
        WHERE n.TrangThaiNhanVien = N'DangLam'
    ),
    Tinh AS
    (
        SELECT *,
            CAST(ROUND((LuongCoBan + PhuCapLuong + PhuCapDocHai + PhuCapChucVu)
                * CASE WHEN SoGioCong >= @GioChuanThang THEN 1.0 ELSE SoGioCong / @GioChuanThang END, 2) AS DECIMAL(18,2)) AS ThuNhapTheoCong,
            CAST(ROUND((LuongCoBan / @GioChuanThang) * SoGioPhuTroi * 1.5, 2) AS DECIMAL(18,2)) AS TienPhuTroiTinh
        FROM Nguon
    )
    INSERT dbo.ChiTietBangLuong
    (
        MaCTBangLuong, MaBangLuong, MaNhanVien, MaQDLuong, MaQDChucVu,
        SoGioCong, SoGioPhuTroiQuyDoi, LuongCoBanApDung, PhuCapLuong,
        PhuCapDocHai, PhuCapChucVu, TienPhuTroi, KhauTruBHYT, KhauTruBHXH,
        TienThuong, TienPhat, TongThuNhap, ThucLinh
    )
    SELECT
        N'CT-' + CONVERT(NVARCHAR(6), @Nam * 100 + @Thang) + N'-' + RIGHT(N'000' + CONVERT(NVARCHAR(10), ROW_NUMBER() OVER (ORDER BY MaNhanVien)), 3),
        @MaBangLuong, MaNhanVien, MaQDLuong, MaQDChucVu,
        SoGioCong, CAST(ROUND(SoGioPhuTroi * 1.5, 2) AS DECIMAL(7,2)),
        LuongCoBan, PhuCapLuong, PhuCapDocHai, PhuCapChucVu, TienPhuTroiTinh,
        KhauTruBHYT, KhauTruBHXH, TienThuong, TienPhat,
        CAST(ROUND(ThuNhapTheoCong + TienPhuTroiTinh + TienThuong, 2) AS DECIMAL(18,2)),
        CAST(ROUND(ThuNhapTheoCong + TienPhuTroiTinh + TienThuong - KhauTruBHYT - KhauTruBHXH - TienPhat, 2) AS DECIMAL(18,2))
    FROM Tinh;

    COMMIT TRANSACTION;
    SELECT MaBangLuong, COUNT(*) AS SoNhanVien FROM dbo.ChiTietBangLuong WHERE MaBangLuong = @MaBangLuong GROUP BY MaBangLuong;
END;
GO

/* View cho giao dien xem bang luong */
CREATE OR ALTER VIEW dbo.vw_BangLuongChiTiet
AS
SELECT bl.MaBangLuong, bl.Thang, bl.Nam, bl.TrangThai AS TrangThaiBangLuong,
       nv.MaNhanVien, nv.HoTen, ct.SoGioCong, ct.SoGioPhuTroiQuyDoi,
       ct.LuongCoBanApDung, ct.PhuCapLuong, ct.PhuCapDocHai, ct.PhuCapChucVu,
       ct.TienPhuTroi, ct.KhauTruBHYT, ct.KhauTruBHXH, ct.TienThuong, ct.TienPhat,
       ct.TongThuNhap, ct.ThucLinh
FROM dbo.BangLuong bl
JOIN dbo.ChiTietBangLuong ct ON ct.MaBangLuong = bl.MaBangLuong
JOIN dbo.NhanVien nv ON nv.MaNhanVien = ct.MaNhanVien;
GO

/* Index ho tro truy van theo nhan vien va ngay hieu luc */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_QDLuong_NhanVien_HieuLuc' AND object_id = OBJECT_ID(N'dbo.QuyetDinhLuong'))
    CREATE INDEX IX_QDLuong_NhanVien_HieuLuc ON dbo.QuyetDinhLuong(MaNhanVien, NgayHieuLuc DESC, NgayKy DESC);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_CTChamCong_NhanVien_Ngay' AND object_id = OBJECT_ID(N'dbo.ChiTietChamCong'))
    CREATE INDEX IX_CTChamCong_NhanVien_Ngay ON dbo.ChiTietChamCong(MaNhanVien, Ngay);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_PhieuPhuTroi_Ngay_TrangThai' AND object_id = OBJECT_ID(N'dbo.PhieuPhuTroi'))
    CREATE INDEX IX_PhieuPhuTroi_Ngay_TrangThai ON dbo.PhieuPhuTroi(NgayPhuTroi, TrangThai);
GO

PRINT N'Da tao/cap nhat schema QLNS_DongA.';
GO
