
USE master;
GO

IF DB_ID(N'QuanLyKhachSan') IS NOT NULL
BEGIN
    ALTER DATABASE QuanLyKhachSan SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyKhachSan;
END
GO

CREATE DATABASE QuanLyKhachSan;
GO

USE QuanLyKhachSan;
GO

-- =============================================
-- 1. NHÂN VIÊN
-- =============================================
CREATE TABLE NhanVien(
    MaNV varchar(20) NOT NULL PRIMARY KEY,
    HoTen nvarchar(120) NOT NULL,
    VaiTro nvarchar(50) NOT NULL,
    SoDienThoai varchar(20) NULL
);
GO

-- =============================================
-- 2. KHU VỰC
-- =============================================
CREATE TABLE KhuVuc(
    MaKhuVuc varchar(20) NOT NULL PRIMARY KEY,
    TenKhuVuc nvarchar(100) NOT NULL UNIQUE
);
GO

-- =============================================
-- 3. PHÒNG
-- =============================================
CREATE TABLE Phong(
    SoPhong varchar(20) NOT NULL PRIMARY KEY,
    MaKhuVuc varchar(20) NOT NULL,
    SoNguoiToiDa int NOT NULL CHECK(SoNguoiToiDa > 0),
    DonGiaNgay decimal(18,2) NOT NULL CHECK(DonGiaNgay >= 0),
    TrangThai nvarchar(30) NOT NULL DEFAULT N'Trống',

    CONSTRAINT CK_Phong_TrangThai
        CHECK(TrangThai IN (
            N'Trống',
            N'Đã đặt',
            N'Đang ở',
            N'Bảo trì'
        )),

    CONSTRAINT FK_Phong_KhuVuc
        FOREIGN KEY(MaKhuVuc)
        REFERENCES KhuVuc(MaKhuVuc)
);
GO

-- =============================================
-- 4. LOẠI TIỆN NGHI
-- =============================================
CREATE TABLE LoaiTienNghi(
    MaLoaiTN varchar(20) NOT NULL PRIMARY KEY,
    TenLoaiTN nvarchar(100) NOT NULL UNIQUE
);
GO

-- =============================================
-- 5. TIỆN NGHI
-- =============================================
CREATE TABLE TienNghi(
    MaTienNghi varchar(30) NOT NULL PRIMARY KEY,
    MaLoaiTN varchar(20) NOT NULL,
    SoThuTu int NOT NULL,
    TinhTrangHienTai nvarchar(100) NULL,

    CONSTRAINT UQ_TienNghi_Loai_STT
        UNIQUE(MaLoaiTN, SoThuTu),

    CONSTRAINT FK_TienNghi_Loai
        FOREIGN KEY(MaLoaiTN)
        REFERENCES LoaiTienNghi(MaLoaiTN)
);
GO

-- =============================================
-- 6. PHIẾU LẮP ĐẶT
-- =============================================
CREATE TABLE PhieuLapDat(
    SoPhieuLapDat varchar(30) NOT NULL PRIMARY KEY,
    MaTienNghi varchar(30) NOT NULL,
    SoPhong varchar(20) NOT NULL,
    NgayLap date NOT NULL,
    TinhTrang nvarchar(100) NOT NULL,
    MaNV varchar(20) NOT NULL,
    GhiChu nvarchar(250) NULL,

    CONSTRAINT UQ_PhieuLapDat_ThietBi_Ngay
        UNIQUE(MaTienNghi, NgayLap),

    CONSTRAINT FK_PhieuLapDat_TienNghi
        FOREIGN KEY(MaTienNghi)
        REFERENCES TienNghi(MaTienNghi),

    CONSTRAINT FK_PhieuLapDat_Phong
        FOREIGN KEY(SoPhong)
        REFERENCES Phong(SoPhong),

    CONSTRAINT FK_PhieuLapDat_NV
        FOREIGN KEY(MaNV)
        REFERENCES NhanVien(MaNV)
);
GO

-- =============================================
-- 7. KHÁCH HÀNG
-- =============================================
CREATE TABLE KhachHang(
    MaKhach varchar(20) NOT NULL PRIMARY KEY,
    HoTen nvarchar(120) NOT NULL,
    SoCMND varchar(30) NOT NULL UNIQUE,
    QuocTich nvarchar(80) NOT NULL,
    SoDienThoai varchar(20) NULL
);
GO

-- =============================================
-- 8. PHIẾU ĐẶT PHÒNG
-- =============================================
CREATE TABLE PhieuDatPhong(
    SoPhieuDat varchar(30) NOT NULL PRIMARY KEY,
    MaKhach varchar(20) NOT NULL,
    MaNVLeTan varchar(20) NOT NULL,
    NgayLap datetime NOT NULL,
    NgayNhan date NOT NULL,
    NgayTraDuKien date NOT NULL,
    TienCoc decimal(18,2) NOT NULL DEFAULT 0 CHECK(TienCoc >= 0),
    KenhDat nvarchar(20) NOT NULL,
    TrangThai nvarchar(30) NOT NULL DEFAULT N'Đã đặt',
    NgayNhanThucTe datetime NULL,
    NgayTraThucTe datetime NULL,

    CONSTRAINT CK_PhieuDat_Ngay
        CHECK(NgayTraDuKien >= NgayNhan),

    CONSTRAINT CK_PhieuDat_Kenh
        CHECK(KenhDat IN (
            N'Điện thoại',
            N'Website',
            N'Trực tiếp'
        )),

    CONSTRAINT CK_PhieuDat_TrangThai
        CHECK(TrangThai IN (
            N'Đã đặt',
            N'Đang ở',
            N'Đã trả',
            N'No-show',
            N'Hủy'
        )),

    CONSTRAINT FK_PhieuDat_Khach
        FOREIGN KEY(MaKhach)
        REFERENCES KhachHang(MaKhach),

    CONSTRAINT FK_PhieuDat_NV
        FOREIGN KEY(MaNVLeTan)
        REFERENCES NhanVien(MaNV)
);
GO

-- =============================================
-- 9. CHI TIẾT ĐẶT PHÒNG
-- =============================================
CREATE TABLE ChiTietDatPhong(
    SoPhieuDat varchar(30) NOT NULL,
    SoPhong varchar(20) NOT NULL,
    SoNguoi int NOT NULL CHECK(SoNguoi > 0),

    PRIMARY KEY(SoPhieuDat, SoPhong),

    CONSTRAINT FK_CTDat_Phieu
        FOREIGN KEY(SoPhieuDat)
        REFERENCES PhieuDatPhong(SoPhieuDat),

    CONSTRAINT FK_CTDat_Phong
        FOREIGN KEY(SoPhong)
        REFERENCES Phong(SoPhong)
);
GO

-- =============================================
-- 10. NGƯỜI LƯU TRÚ
-- =============================================
CREATE TABLE NguoiLuuTru(
    MaNguoiLT int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    SoPhieuDat varchar(30) NOT NULL,
    SoPhong varchar(20) NOT NULL,
    HoTen nvarchar(120) NOT NULL,
    SoCMND varchar(30) NOT NULL,
    QuocTich nvarchar(80) NOT NULL,

    CONSTRAINT FK_NguoiLT_CTDat
        FOREIGN KEY(SoPhieuDat, SoPhong)
        REFERENCES ChiTietDatPhong(SoPhieuDat, SoPhong)
);
GO

-- =============================================
-- 11. DỊCH VỤ
-- =============================================
CREATE TABLE DichVu(
    MaDV varchar(20) NOT NULL PRIMARY KEY,
    TenDV nvarchar(120) NOT NULL,
    DonViTinh nvarchar(40) NOT NULL,
    DonGia decimal(18,2) NOT NULL CHECK(DonGia >= 0)
);
GO

-- =============================================
-- 12. PHIẾU SỬ DỤNG DỊCH VỤ
-- =============================================
CREATE TABLE PhieuSuDungDV(
    SoPhieuSDDV varchar(30) NOT NULL PRIMARY KEY,
    SoPhieuDat varchar(30) NOT NULL,
    SoPhong varchar(20) NOT NULL,
    NgaySuDung date NOT NULL,
    MaNV varchar(20) NOT NULL,

    CONSTRAINT UQ_PhieuSDDV_PhongNgay
        UNIQUE(SoPhieuDat, SoPhong, NgaySuDung),

    CONSTRAINT FK_PhieuSDDV_CTDat
        FOREIGN KEY(SoPhieuDat, SoPhong)
        REFERENCES ChiTietDatPhong(SoPhieuDat, SoPhong),

    CONSTRAINT FK_PhieuSDDV_NV
        FOREIGN KEY(MaNV)
        REFERENCES NhanVien(MaNV)
);
GO

-- =============================================
-- 13. CHI TIẾT PHIẾU SỬ DỤNG DỊCH VỤ
-- =============================================
CREATE TABLE ChiTietPhieuSuDungDV(
    SoPhieuSDDV varchar(30) NOT NULL,
    MaDV varchar(20) NOT NULL,
    SoLuong int NOT NULL CHECK(SoLuong > 0),
    DonGia decimal(18,2) NOT NULL CHECK(DonGia >= 0),

    ThanhTien AS (
        CONVERT(decimal(18,2), SoLuong * DonGia)
    ) PERSISTED,

    PRIMARY KEY(SoPhieuSDDV, MaDV),

    CONSTRAINT FK_CTSDDV_Phieu
        FOREIGN KEY(SoPhieuSDDV)
        REFERENCES PhieuSuDungDV(SoPhieuSDDV),

    CONSTRAINT FK_CTSDDV_DV
        FOREIGN KEY(MaDV)
        REFERENCES DichVu(MaDV)
);
GO

-- =============================================
-- 14. QUY ĐỊNH ĐỀN BÙ
-- =============================================
CREATE TABLE QuyDinhDenBu(
    MaQuyDinh varchar(30) NOT NULL PRIMARY KEY,
    MaLoaiTN varchar(20) NOT NULL,
    MucDoThietHai nvarchar(80) NOT NULL,
    MucDenBu decimal(18,2) NOT NULL CHECK(MucDenBu >= 0),

    CONSTRAINT UQ_QDDB_Loai_MucDo
        UNIQUE(MaLoaiTN, MucDoThietHai),

    CONSTRAINT FK_QDDB_Loai
        FOREIGN KEY(MaLoaiTN)
        REFERENCES LoaiTienNghi(MaLoaiTN)
);
GO

-- =============================================
-- 15. PHIẾU ĐỀN BÙ
-- =============================================
CREATE TABLE PhieuDenBu(
    SoPhieuDenBu varchar(30) NOT NULL PRIMARY KEY,
    SoPhieuDat varchar(30) NOT NULL,
    SoPhong varchar(20) NOT NULL,
    NgayLap datetime NOT NULL,
    MaNV varchar(20) NOT NULL,
    TongTien decimal(18,2) NOT NULL DEFAULT 0 CHECK(TongTien >= 0),

    CONSTRAINT FK_PhieuDB_CTDat
        FOREIGN KEY(SoPhieuDat, SoPhong)
        REFERENCES ChiTietDatPhong(SoPhieuDat, SoPhong),

    CONSTRAINT FK_PhieuDB_NV
        FOREIGN KEY(MaNV)
        REFERENCES NhanVien(MaNV)
);
GO

-- =============================================
-- 16. CHI TIẾT PHIẾU ĐỀN BÙ
-- =============================================
CREATE TABLE ChiTietPhieuDenBu(
    SoPhieuDenBu varchar(30) NOT NULL,
    MaTienNghi varchar(30) NOT NULL,
    MucDoThietHai nvarchar(80) NOT NULL,
    SoTien decimal(18,2) NOT NULL CHECK(SoTien >= 0),

    PRIMARY KEY(SoPhieuDenBu, MaTienNghi),

    CONSTRAINT FK_CTDB_Phieu
        FOREIGN KEY(SoPhieuDenBu)
        REFERENCES PhieuDenBu(SoPhieuDenBu),

    CONSTRAINT FK_CTDB_TienNghi
        FOREIGN KEY(MaTienNghi)
        REFERENCES TienNghi(MaTienNghi)
);
GO

-- =============================================
-- 17. HÓA ĐƠN
-- =============================================
CREATE TABLE HoaDon(
    SoHoaDon varchar(30) NOT NULL PRIMARY KEY,
    SoPhieuDat varchar(30) NOT NULL UNIQUE,
    NgayLap datetime NOT NULL,
    MaNV varchar(20) NOT NULL,
    SoNgayTinhTien int NOT NULL CHECK(SoNgayTinhTien > 0),
    TienPhong decimal(18,2) NOT NULL CHECK(TienPhong >= 0),
    TienDichVu decimal(18,2) NOT NULL CHECK(TienDichVu >= 0),

    TongTien AS (
        CONVERT(decimal(18,2), TienPhong + TienDichVu)
    ) PERSISTED,

    TrangThai nvarchar(30) NOT NULL DEFAULT N'Chưa thanh toán',

    CONSTRAINT CK_HoaDon_TrangThai
        CHECK(TrangThai IN (
            N'Chưa thanh toán',
            N'Đã thanh toán'
        )),

    CONSTRAINT FK_HoaDon_PhieuDat
        FOREIGN KEY(SoPhieuDat)
        REFERENCES PhieuDatPhong(SoPhieuDat),

    CONSTRAINT FK_HoaDon_NV
        FOREIGN KEY(MaNV)
        REFERENCES NhanVien(MaNV)
);
GO

-- =============================================
-- 18. THANH TOÁN
-- =============================================
CREATE TABLE ThanhToan(
    MaThanhToan varchar(30) NOT NULL PRIMARY KEY,
    SoHoaDon varchar(30) NOT NULL,
    NgayThanhToan datetime NOT NULL,
    HinhThuc nvarchar(30) NOT NULL,
    SoTien decimal(18,2) NOT NULL CHECK(SoTien > 0),

    CONSTRAINT CK_ThanhToan_HinhThuc
        CHECK(HinhThuc IN (
            N'Tiền mặt',
            N'Chuyển khoản',
            N'Thẻ',
            N'Ví điện tử'
        )),

    CONSTRAINT FK_ThanhToan_HoaDon
        FOREIGN KEY(SoHoaDon)
        REFERENCES HoaDon(SoHoaDon)
);
GO

-- =============================================
-- 19. INDEX
-- =============================================

CREATE INDEX IX_PhieuDatPhong_Ngay
ON PhieuDatPhong(
    NgayNhan,
    NgayTraDuKien,
    TrangThai
);
GO

CREATE INDEX IX_CTDat_Phong
ON ChiTietDatPhong(
    SoPhong,
    SoPhieuDat
);
GO

CREATE INDEX IX_PhieuSDDV_DatPhong
ON PhieuSuDungDV(
    SoPhieuDat,
    SoPhong,
    NgaySuDung
);
GO

-- =============================================
-- 20. KIỂM TRA DATABASE
-- =============================================

SELECT * FROM NhanVien;
SELECT * FROM KhuVuc;
SELECT * FROM Phong;
SELECT * FROM LoaiTienNghi;
SELECT * FROM TienNghi;
SELECT * FROM PhieuLapDat;
SELECT * FROM KhachHang;
SELECT * FROM PhieuDatPhong;
SELECT * FROM ChiTietDatPhong;
SELECT * FROM NguoiLuuTru;
SELECT * FROM DichVu;
SELECT * FROM PhieuSuDungDV;
SELECT * FROM ChiTietPhieuSuDungDV;
SELECT * FROM QuyDinhDenBu;
SELECT * FROM PhieuDenBu;
SELECT * FROM ChiTietPhieuDenBu;
SELECT * FROM HoaDon;
SELECT * FROM ThanhToan;
GO




USE QuanLyKhachSan;
GO

-- 1. NHÂN VIÊN
INSERT INTO NhanVien(MaNV,HoTen,VaiTro,SoDienThoai) VALUES
('NV01',N'Nguyễn Văn An',N'Lễ tân','0901234567'),('NV02',N'Trần Thị Bình',N'Quản lý','0901234568'),('NV03',N'Lê Văn Cường',N'Phục vụ phòng','0901234569'),
('NV04',N'Nguyễn Thu Hà',N'Lễ tân','0901000001'),('NV05',N'Trần Minh An',N'Phục vụ phòng','0901000002'),('NV06',N'Lê Hoàng Nam',N'Thanh toán','0901000003');
GO

-- 2. KHU VỰC
INSERT INTO KhuVuc(MaKhuVuc,TenKhuVuc) VALUES
('KV01',N'Tầng 1'),('KV02',N'Tầng 2'),('KV03',N'Tầng 3'),('KVA',N'Khu A'),('KVB',N'Khu B');
GO

-- 3. PHÒNG
INSERT INTO Phong(SoPhong,MaKhuVuc,SoNguoiToiDa,DonGiaNgay,TrangThai) VALUES
('101','KV01',2,800000,N'Trống'),('102','KV01',2,1200000,N'Trống'),('103','KV01',2,1000000,N'Trống'),
('201','KV02',4,2500000,N'Trống'),('202','KV02',4,3500000,N'Trống'),('203','KV02',3,1800000,N'Trống'),
('301','KV03',6,4500000,N'Trống'),('302','KV03',4,3000000,N'Trống'),
('A101','KVA',2,600000,N'Trống'),('A102','KVA',3,800000,N'Trống'),('B201','KVB',4,1200000,N'Trống');
GO

-- 4. LOẠI TIỆN NGHI
INSERT INTO LoaiTienNghi(MaLoaiTN,TenLoaiTN) VALUES
('TV',N'Tivi'),('AC',N'Điều hòa'),('TL',N'Tủ lạnh'),('MS',N'Máy sấy'),('DT',N'Điện thoại');
GO

-- 5. TIỆN NGHI
INSERT INTO TienNghi(MaTienNghi,MaLoaiTN,SoThuTu,TinhTrangHienTai) VALUES
('TV01','TV',1,N'Tốt'),('TV02','TV',2,N'Tốt'),('TV03','TV',3,N'Tốt'),
('AC01','AC',1,N'Tốt'),('AC02','AC',2,N'Tốt'),('AC03','AC',3,N'Tốt'),
('TL01','TL',1,N'Tốt'),('TL02','TL',2,N'Tốt'),('TL03','TL',3,N'Tốt'),
('MS01','MS',1,N'Tốt'),('MS02','MS',2,N'Tốt'),('DT01','DT',1,N'Tốt'),('DT02','DT',2,N'Tốt');
GO

-- 6. PHIẾU LẮP ĐẶT
INSERT INTO PhieuLapDat(SoPhieuLapDat,MaTienNghi,SoPhong,NgayLap,TinhTrang,MaNV,GhiChu) VALUES
('PLD001','TV01','101','2026-09-01',N'Đã lắp đặt','NV03',N'Lắp đặt tivi phòng 101'),
('PLD002','AC01','101','2026-09-01',N'Đã lắp đặt','NV03',N'Lắp đặt điều hòa phòng 101'),
('PLD003','TL01','101','2026-09-01',N'Đã lắp đặt','NV03',N'Lắp đặt tủ lạnh phòng 101'),
('PLD004','TV02','102','2026-09-02',N'Đã lắp đặt','NV03',N'Lắp đặt tivi phòng 102'),
('PLD005','AC02','102','2026-09-02',N'Đã lắp đặt','NV03',N'Lắp đặt điều hòa phòng 102'),
('PLD006','TL02','102','2026-09-02',N'Đã lắp đặt','NV03',N'Lắp đặt tủ lạnh phòng 102'),
('PLD007','TV03','201','2026-09-03',N'Đã lắp đặt','NV03',N'Lắp đặt tivi phòng 201'),
('PLD008','AC03','201','2026-09-03',N'Đã lắp đặt','NV03',N'Lắp đặt điều hòa phòng 201');
GO

-- 7. KHÁCH HÀNG
INSERT INTO KhachHang(MaKhach,HoTen,SoCMND,QuocTich,SoDienThoai) VALUES
('KH01',N'Phạm Huy Hoàng','079123456700',N'Việt Nam','0912345678'),
('KH02',N'Nguyễn Thị Lan','079123456701',N'Việt Nam','0912345679'),
('KH03',N'Trần Văn Minh','079123456702',N'Việt Nam','0912345680'),
('KH04',N'Lê Thị Hoa','079123456703',N'Việt Nam','0912345681'),
('KH05',N'John Smith','US123456789',N'Hoa Kỳ','0909876543');
GO

-- 8. DỊCH VỤ
INSERT INTO DichVu(MaDV,TenDV,DonViTinh,DonGia) VALUES
('DV01',N'Nước suối',N'Chai',15000),('DV02',N'Giặt ủi',N'Kg',50000),('DV03',N'Ăn sáng',N'Suất',100000),
('DV04',N'Massage',N'Lần',500000),('DV05',N'Đưa đón sân bay',N'Lần',300000),('DV06',N'Tắm hơi',N'Lượt',250000),('DV07',N'Karaoke',N'Giờ',300000);
GO

-- 9. QUY ĐỊNH ĐỀN BÙ
INSERT INTO QuyDinhDenBu(MaQuyDinh,MaLoaiTN,MucDoThietHai,MucDenBu) VALUES
('QD01','TV',N'Hư hỏng nhẹ',500000),('QD02','TV',N'Mất',5000000),
('QD03','AC',N'Hư hỏng nhẹ',1000000),('QD04','AC',N'Mất',12000000),
('QD05','TL',N'Hư hỏng nhẹ',400000),('QD06','TL',N'Mất',4000000),
('QD07','MS',N'Hư hỏng nhẹ',200000),('QD08','MS',N'Mất',800000),
('QD09','DT',N'Hư hỏng nhẹ',300000),('QD10','DT',N'Mất',1000000);
GO

-- 10. PHIẾU ĐẶT PHÒNG
INSERT INTO PhieuDatPhong(SoPhieuDat,MaKhach,MaNVLeTan,NgayLap,NgayNhan,NgayTraDuKien,TienCoc,KenhDat,TrangThai) VALUES
('PDP001','KH01','NV01','2026-09-20 08:30:00','2026-09-25','2026-09-28',1000000,N'Trực tiếp',N'Đã trả'),
('PDP002','KH02','NV04','2026-09-21 10:00:00','2026-09-26','2026-09-30',1500000,N'Website',N'Đang ở'),
('PDP003','KH03','NV01','2026-09-22 14:00:00','2026-10-01','2026-10-05',2000000,N'Điện thoại',N'Đã đặt');
GO

-- 11. CHI TIẾT ĐẶT PHÒNG
INSERT INTO ChiTietDatPhong(SoPhieuDat,SoPhong,SoNguoi) VALUES
('PDP001','101',2),('PDP002','201',3),('PDP003','202',4);
GO

-- 12. NGƯỜI LƯU TRÚ
INSERT INTO NguoiLuuTru(SoPhieuDat,SoPhong,HoTen,SoCMND,QuocTich) VALUES
('PDP001','101',N'Phạm Huy Hoàng','079123456700',N'Việt Nam'),
('PDP001','101',N'Nguyễn Văn Nam','079123456704',N'Việt Nam'),
('PDP002','201',N'Nguyễn Thị Lan','079123456701',N'Việt Nam'),
('PDP002','201',N'Trần Thị Mai','079123456705',N'Việt Nam'),
('PDP002','201',N'Lê Văn Bình','079123456706',N'Việt Nam'),
('PDP003','202',N'Trần Văn Minh','079123456702',N'Việt Nam'),
('PDP003','202',N'Lê Văn An','079123456707',N'Việt Nam');
GO

-- 13. PHIẾU SỬ DỤNG DỊCH VỤ
INSERT INTO PhieuSuDungDV(SoPhieuSDDV,SoPhieuDat,SoPhong,NgaySuDung,MaNV) VALUES
('SDDV001','PDP001','101','2026-09-25','NV05'),('SDDV002','PDP001','101','2026-09-26','NV05'),
('SDDV003','PDP002','201','2026-09-26','NV05'),('SDDV004','PDP002','201','2026-09-27','NV05');
GO

-- 14. CHI TIẾT SỬ DỤNG DỊCH VỤ
INSERT INTO ChiTietPhieuSuDungDV(SoPhieuSDDV,MaDV,SoLuong,DonGia) VALUES
('SDDV001','DV01',2,15000),('SDDV001','DV03',2,100000),('SDDV002','DV02',3,50000),
('SDDV003','DV01',4,15000),('SDDV003','DV03',3,100000),('SDDV003','DV04',1,500000),
('SDDV004','DV05',1,300000),('SDDV004','DV06',2,250000);
GO

-- 15. PHIẾU ĐỀN BÙ
INSERT INTO PhieuDenBu(SoPhieuDenBu,SoPhieuDat,SoPhong,NgayLap,MaNV,TongTien) VALUES
('PDB001','PDP001','101','2026-09-28 09:00:00','NV03',500000);
GO

-- 16. CHI TIẾT PHIẾU ĐỀN BÙ
INSERT INTO ChiTietPhieuDenBu(SoPhieuDenBu,MaTienNghi,MucDoThietHai,SoTien) VALUES
('PDB001','TV01',N'Hư hỏng nhẹ',500000);
GO

-- 17. HÓA ĐƠN
INSERT INTO HoaDon(SoHoaDon,SoPhieuDat,NgayLap,MaNV,SoNgayTinhTien,TienPhong,TienDichVu,TrangThai) VALUES
('HD001','PDP001','2026-09-28 10:00:00','NV06',3,2400000,330000,N'Đã thanh toán'),
('HD002','PDP002','2026-09-28 11:00:00','NV06',4,10000000,1640000,N'Chưa thanh toán');
GO

-- 18. THANH TOÁN
INSERT INTO ThanhToan(MaThanhToan,SoHoaDon,NgayThanhToan,HinhThuc,SoTien) VALUES
('TT001','HD001','2026-09-28 10:30:00',N'Tiền mặt',2730000),
('TT002','HD001','2026-09-28 10:35:00',N'Chuyển khoản',500000);
GO

-- 19. CẬP NHẬT TRẠNG THÁI PHÒNG
UPDATE Phong SET TrangThai=N'Đang ở' WHERE SoPhong='201';
UPDATE Phong SET TrangThai=N'Đã đặt' WHERE SoPhong='202';
GO

-- 20. KIỂM TRA
SELECT * FROM NhanVien;
SELECT * FROM KhuVuc;
SELECT * FROM Phong;
SELECT * FROM LoaiTienNghi;
SELECT * FROM TienNghi;
SELECT * FROM PhieuLapDat;
SELECT * FROM KhachHang;
SELECT * FROM PhieuDatPhong;
SELECT * FROM ChiTietDatPhong;
SELECT * FROM NguoiLuuTru;
SELECT * FROM DichVu;
SELECT * FROM PhieuSuDungDV;
SELECT * FROM ChiTietPhieuSuDungDV;
SELECT * FROM QuyDinhDenBu;
SELECT * FROM PhieuDenBu;
SELECT * FROM ChiTietPhieuDenBu;
SELECT * FROM HoaDon;
SELECT * FROM ThanhToan;
GO


