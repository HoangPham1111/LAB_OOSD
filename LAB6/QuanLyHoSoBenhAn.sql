/* ================================================================
   HE THONG QUAN LY HO SO BENH AN - SQL SERVER
   Pham vi: benh nhan, ho so, dot dieu tri, phieu kham, xet nghiem,
            toa thuoc/chi tiet toa, lich kham dinh ky, vien phi.
   Nghiep vu mau co transaction: dbo.usp_LapToaThuoc
   SQL Server 2016 SP1+ (CREATE OR ALTER PROCEDURE).
   ================================================================ */

IF DB_ID(N'QuanLyHoSoBenhAn') IS NULL
BEGIN
    EXEC(N'CREATE DATABASE QuanLyHoSoBenhAn');
END;
GO

USE QuanLyHoSoBenhAn;
GO

/* 1. Danh muc nhan vien CSKH */
IF OBJECT_ID(N'dbo.NhanVienCSKH', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.NhanVienCSKH
    (
        MaNV       INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_NhanVienCSKH PRIMARY KEY,
        HoTen      NVARCHAR(100) NOT NULL,
        DienThoai  VARCHAR(20) NULL,
        Email      VARCHAR(254) NULL,
        ConHoatDong BIT NOT NULL CONSTRAINT DF_NhanVienCSKH_ConHoatDong DEFAULT (1)
    );
END;
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_NhanVienCSKH_Email' AND object_id = OBJECT_ID(N'dbo.NhanVienCSKH'))
BEGIN
    CREATE UNIQUE INDEX UX_NhanVienCSKH_Email ON dbo.NhanVienCSKH(Email) WHERE Email IS NOT NULL;
END;
GO

/* 2. Benh nhan */
IF OBJECT_ID(N'dbo.BenhNhan', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.BenhNhan
    (
        MaBN       INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_BenhNhan PRIMARY KEY,
        HoTen      NVARCHAR(100) NOT NULL,
        DiaChi     NVARCHAR(250) NULL,
        NgaySinh   DATE NULL,
        DienThoai  VARCHAR(20) NULL,
        Anh3x4     NVARCHAR(500) NULL,
        NgayTao    DATETIME2(0) NOT NULL CONSTRAINT DF_BenhNhan_NgayTao DEFAULT (SYSDATETIME())
    );
END;
GO

/* 3. Le phi quan ly/khám dinh ky theo nam - tach rieng vien phi */
IF OBJECT_ID(N'dbo.LePhiNam', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.LePhiNam
    (
        MaLePhi    INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_LePhiNam PRIMARY KEY,
        MaBN       INT NOT NULL,
        NamApDung  SMALLINT NOT NULL,
        NgayDong   DATE NULL,
        SoTien     DECIMAL(12,2) NOT NULL,
        TrangThai  NVARCHAR(20) NOT NULL CONSTRAINT DF_LePhiNam_TrangThai DEFAULT (N'ChuaDong'),
        CONSTRAINT FK_LePhiNam_BenhNhan FOREIGN KEY (MaBN) REFERENCES dbo.BenhNhan(MaBN),
        CONSTRAINT UQ_LePhiNam_BenhNhan_Nam UNIQUE (MaBN, NamApDung),
        CONSTRAINT CK_LePhiNam_SoTien CHECK (SoTien >= 0),
        CONSTRAINT CK_LePhiNam_TrangThai CHECK (TrangThai IN (N'ChuaDong', N'DaDong', N'Huy'))
    );
END;
GO

/* 4. Mot benh nhan co mot ho so benh an chinh theo gia dinh thiet ke */
IF OBJECT_ID(N'dbo.HoSoBenhAn', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.HoSoBenhAn
    (
        MaHS       INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_HoSoBenhAn PRIMARY KEY,
        MaBN       INT NOT NULL,
        NgayLap    DATETIME2(0) NOT NULL CONSTRAINT DF_HoSoBenhAn_NgayLap DEFAULT (SYSDATETIME()),
        TrangThai  NVARCHAR(20) NOT NULL CONSTRAINT DF_HoSoBenhAn_TrangThai DEFAULT (N'DangHoatDong'),
        CONSTRAINT FK_HoSoBenhAn_BenhNhan FOREIGN KEY (MaBN) REFERENCES dbo.BenhNhan(MaBN),
        CONSTRAINT UQ_HoSoBenhAn_MaBN UNIQUE (MaBN),
        CONSTRAINT CK_HoSoBenhAn_TrangThai CHECK (TrangThai IN (N'DangHoatDong', N'TamKhoa', N'LuuTru'))
    );
END;
GO

/* 5. Bac si va khoa dieu tri */
IF OBJECT_ID(N'dbo.BacSi', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.BacSi
    (
        MaBS          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_BacSi PRIMARY KEY,
        HoTen         NVARCHAR(100) NOT NULL,
        ChuyenKhoa    NVARCHAR(100) NULL,
        DienThoai     VARCHAR(20) NULL,
        ConHanhNghe   BIT NOT NULL CONSTRAINT DF_BacSi_ConHanhNghe DEFAULT (1)
    );
END;
GO

IF OBJECT_ID(N'dbo.KhoaDieuTri', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.KhoaDieuTri
    (
        MaKhoa   INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_KhoaDieuTri PRIMARY KEY,
        TenKhoa  NVARCHAR(100) NOT NULL,
        CONSTRAINT UQ_KhoaDieuTri_TenKhoa UNIQUE (TenKhoa)
    );
END;
GO

/* 6. Dot dieu tri / dot kham suc khoe dinh ky */
IF OBJECT_ID(N'dbo.DotDieuTri', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.DotDieuTri
    (
        MaDT          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_DotDieuTri PRIMARY KEY,
        MaHS          INT NOT NULL,
        MaNVNhap      INT NULL,
        LoaiDot       NVARCHAR(20) NOT NULL,
        NgayBatDau    DATETIME2(0) NOT NULL CONSTRAINT DF_DotDieuTri_NgayBatDau DEFAULT (SYSDATETIME()),
        NgayKetThuc   DATETIME2(0) NULL,
        NgayXuatVien  DATETIME2(0) NULL,
        KetQuaTongKet NVARCHAR(1000) NULL,
        TrangThai     NVARCHAR(20) NOT NULL CONSTRAINT DF_DotDieuTri_TrangThai DEFAULT (N'DangXuLy'),
        CONSTRAINT FK_DotDieuTri_HoSo FOREIGN KEY (MaHS) REFERENCES dbo.HoSoBenhAn(MaHS),
        CONSTRAINT FK_DotDieuTri_NhanVien FOREIGN KEY (MaNVNhap) REFERENCES dbo.NhanVienCSKH(MaNV),
        CONSTRAINT CK_DotDieuTri_LoaiDot CHECK (LoaiDot IN (N'DieuTri', N'KhamDinhKy')),
        CONSTRAINT CK_DotDieuTri_TrangThai CHECK (TrangThai IN (N'DangXuLy', N'ChoHoanTat', N'DaDong')),
        CONSTRAINT CK_DotDieuTri_Ngay CHECK (NgayKetThuc IS NULL OR NgayKetThuc >= NgayBatDau)
    );
END;
GO

/* 7. Phieu kham */
IF OBJECT_ID(N'dbo.PhieuKham', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PhieuKham
    (
        MaPK         INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_PhieuKham PRIMARY KEY,
        MaDT         INT NOT NULL,
        MaBS         INT NOT NULL,
        MaKhoa       INT NOT NULL,
        NgayGioKham  DATETIME2(0) NOT NULL CONSTRAINT DF_PhieuKham_NgayGioKham DEFAULT (SYSDATETIME()),
        TrieuChung   NVARCHAR(1000) NULL,
        ThanNhiet    DECIMAL(4,1) NULL,
        HuyetAp      NVARCHAR(30) NULL,
        GhiChu       NVARCHAR(1000) NULL,
        CONSTRAINT FK_PhieuKham_DotDieuTri FOREIGN KEY (MaDT) REFERENCES dbo.DotDieuTri(MaDT),
        CONSTRAINT FK_PhieuKham_BacSi FOREIGN KEY (MaBS) REFERENCES dbo.BacSi(MaBS),
        CONSTRAINT FK_PhieuKham_Khoa FOREIGN KEY (MaKhoa) REFERENCES dbo.KhoaDieuTri(MaKhoa),
        CONSTRAINT CK_PhieuKham_ThanNhiet CHECK (ThanNhiet IS NULL OR (ThanNhiet BETWEEN 25 AND 45))
    );
END;
GO

/* 8. Danh muc thuoc */
IF OBJECT_ID(N'dbo.Thuoc', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Thuoc
    (
        MaThuoc    INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Thuoc PRIMARY KEY,
        TenThuoc   NVARCHAR(200) NOT NULL,
        DonVi      NVARCHAR(50) NOT NULL,
        HoatChat   NVARCHAR(200) NULL,
        HamLuong   NVARCHAR(100) NULL,
        ConSuDung  BIT NOT NULL CONSTRAINT DF_Thuoc_ConSuDung DEFAULT (1)
    );
END;
GO

/* 9. Toa thuoc: UNIQUE(MaPK) dam bao toi da mot toa/phieu kham */
IF OBJECT_ID(N'dbo.ToaThuoc', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ToaThuoc
    (
        MaToa   INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ToaThuoc PRIMARY KEY,
        MaPK    INT NOT NULL,
        NgayKe  DATE NOT NULL CONSTRAINT DF_ToaThuoc_NgayKe DEFAULT (CONVERT(date, SYSDATETIME())),
        LoiDan  NVARCHAR(1000) NULL,
        CONSTRAINT FK_ToaThuoc_PhieuKham FOREIGN KEY (MaPK) REFERENCES dbo.PhieuKham(MaPK),
        CONSTRAINT UQ_ToaThuoc_MaPK UNIQUE (MaPK)
    );
END;
GO

/* 10. Chi tiet toa thuoc: mot thuoc xuat hien toi da mot dong/toa */
IF OBJECT_ID(N'dbo.ChiTietToaThuoc', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ChiTietToaThuoc
    (
        MaToa     INT NOT NULL,
        MaThuoc   INT NOT NULL,
        LieuDung  NVARCHAR(200) NOT NULL,
        CachDung  NVARCHAR(300) NOT NULL,
        SoLuong   DECIMAL(10,2) NOT NULL,
        SoNgay    INT NOT NULL,
        CONSTRAINT PK_ChiTietToaThuoc PRIMARY KEY (MaToa, MaThuoc),
        CONSTRAINT FK_ChiTietToaThuoc_ToaThuoc FOREIGN KEY (MaToa) REFERENCES dbo.ToaThuoc(MaToa),
        CONSTRAINT FK_ChiTietToaThuoc_Thuoc FOREIGN KEY (MaThuoc) REFERENCES dbo.Thuoc(MaThuoc),
        CONSTRAINT CK_ChiTietToaThuoc_SoLuong CHECK (SoLuong > 0),
        CONSTRAINT CK_ChiTietToaThuoc_SoNgay CHECK (SoNgay > 0)
    );
END;
GO

/* 11. Xet nghiem theo phieu kham */
IF OBJECT_ID(N'dbo.XetNghiem', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.XetNghiem
    (
        MaXN          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_XetNghiem PRIMARY KEY,
        MaPK          INT NOT NULL,
        TenXetNghiem  NVARCHAR(200) NOT NULL,
        NgayChiDinh   DATETIME2(0) NOT NULL CONSTRAINT DF_XetNghiem_NgayChiDinh DEFAULT (SYSDATETIME()),
        NgayThucHien  DATETIME2(0) NULL,
        KetQua        NVARCHAR(MAX) NULL,
        TrangThai     NVARCHAR(20) NOT NULL CONSTRAINT DF_XetNghiem_TrangThai DEFAULT (N'ChoThucHien'),
        CONSTRAINT FK_XetNghiem_PhieuKham FOREIGN KEY (MaPK) REFERENCES dbo.PhieuKham(MaPK),
        CONSTRAINT CK_XetNghiem_TrangThai CHECK (TrangThai IN (N'ChoThucHien', N'DangThucHien', N'DaCoKetQua', N'Huy'))
    );
END;
GO

/* 12. Lich kham dinh ky */
IF OBJECT_ID(N'dbo.LichKhamDinhKy', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.LichKhamDinhKy
    (
        MaLich       INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_LichKhamDinhKy PRIMARY KEY,
        MaHS         INT NOT NULL,
        MaNVDatLich  INT NOT NULL,
        NgayDat      DATETIME2(0) NOT NULL CONSTRAINT DF_LichKham_NgayDat DEFAULT (SYSDATETIME()),
        NgayGioHen   DATETIME2(0) NOT NULL,
        TrangThai    NVARCHAR(20) NOT NULL CONSTRAINT DF_LichKham_TrangThai DEFAULT (N'DaDat'),
        CONSTRAINT FK_LichKham_HoSo FOREIGN KEY (MaHS) REFERENCES dbo.HoSoBenhAn(MaHS),
        CONSTRAINT FK_LichKham_NhanVien FOREIGN KEY (MaNVDatLich) REFERENCES dbo.NhanVienCSKH(MaNV),
        CONSTRAINT CK_LichKham_TrangThai CHECK (TrangThai IN (N'DaDat', N'DaXacNhan', N'DaThucHien', N'DaHuy', N'KhongDen'))
    );
END;
GO

/* 13. Hoa don vien phi: gia dinh mot hoa don/dot dieu tri */
IF OBJECT_ID(N'dbo.HoaDonVienPhi', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.HoaDonVienPhi
    (
        MaHD      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_HoaDonVienPhi PRIMARY KEY,
        MaDT      INT NOT NULL,
        NgayLap   DATETIME2(0) NOT NULL CONSTRAINT DF_HoaDon_NgayLap DEFAULT (SYSDATETIME()),
        TongTien  DECIMAL(12,2) NOT NULL,
        TrangThai NVARCHAR(20) NOT NULL CONSTRAINT DF_HoaDon_TrangThai DEFAULT (N'ChuaThanhToan'),
        CONSTRAINT FK_HoaDon_DotDieuTri FOREIGN KEY (MaDT) REFERENCES dbo.DotDieuTri(MaDT),
        CONSTRAINT UQ_HoaDon_MaDT UNIQUE (MaDT),
        CONSTRAINT CK_HoaDon_TongTien CHECK (TongTien >= 0),
        CONSTRAINT CK_HoaDon_TrangThai CHECK (TrangThai IN (N'ChuaThanhToan', N'ThanhToanMotPhan', N'DaThanhToan', N'Huy'))
    );
END;
GO

/* 14. Giao dich thanh toan: cho phep thanh toan nhieu lan/hoa don */
IF OBJECT_ID(N'dbo.ThanhToanVienPhi', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ThanhToanVienPhi
    (
        MaTT          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ThanhToanVienPhi PRIMARY KEY,
        MaHD          INT NOT NULL,
        NgayThanhToan DATETIME2(0) NOT NULL CONSTRAINT DF_ThanhToan_Ngay DEFAULT (SYSDATETIME()),
        SoTien        DECIMAL(12,2) NOT NULL,
        PhuongThuc    NVARCHAR(30) NOT NULL,
        TrangThai     NVARCHAR(20) NOT NULL CONSTRAINT DF_ThanhToan_TrangThai DEFAULT (N'ThanhCong'),
        CONSTRAINT FK_ThanhToan_HoaDon FOREIGN KEY (MaHD) REFERENCES dbo.HoaDonVienPhi(MaHD),
        CONSTRAINT CK_ThanhToan_SoTien CHECK (SoTien > 0),
        CONSTRAINT CK_ThanhToan_PhuongThuc CHECK (PhuongThuc IN (N'TienMat', N'ChuyenKhoan', N'The', N'Khac')),
        CONSTRAINT CK_ThanhToan_TrangThai CHECK (TrangThai IN (N'ThanhCong', N'ThatBai', N'HoanTien'))
    );
END;
GO

/* Index phuc vu cac truy van thuong dung */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_DotDieuTri_MaHS' AND object_id = OBJECT_ID(N'dbo.DotDieuTri'))
    CREATE INDEX IX_DotDieuTri_MaHS ON dbo.DotDieuTri(MaHS);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_PhieuKham_MaDT' AND object_id = OBJECT_ID(N'dbo.PhieuKham'))
    CREATE INDEX IX_PhieuKham_MaDT ON dbo.PhieuKham(MaDT);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_XetNghiem_MaPK' AND object_id = OBJECT_ID(N'dbo.XetNghiem'))
    CREATE INDEX IX_XetNghiem_MaPK ON dbo.XetNghiem(MaPK);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_LichKham_MaHS_NgayGioHen' AND object_id = OBJECT_ID(N'dbo.LichKhamDinhKy'))
    CREATE INDEX IX_LichKham_MaHS_NgayGioHen ON dbo.LichKhamDinhKy(MaHS, NgayGioHen);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_ThanhToan_MaHD' AND object_id = OBJECT_ID(N'dbo.ThanhToanVienPhi'))
    CREATE INDEX IX_ThanhToan_MaHD ON dbo.ThanhToanVienPhi(MaHD);
GO

/* ================================================================
   TABLE TYPE cho danh sach chi tiet toa gui tu WinForms.
   Moi MaThuoc chi xuat hien toi da mot lan trong mot toa.
   ================================================================ */
IF TYPE_ID(N'dbo.ToaThuocChiTietType') IS NULL
BEGIN
    EXEC(N'CREATE TYPE dbo.ToaThuocChiTietType AS TABLE
    (
        MaThuoc   INT NOT NULL PRIMARY KEY,
        LieuDung  NVARCHAR(200) NOT NULL,
        CachDung  NVARCHAR(300) NOT NULL,
        SoLuong   DECIMAL(10,2) NOT NULL CHECK (SoLuong > 0),
        SoNgay    INT NOT NULL CHECK (SoNgay > 0)
    );');
END;
GO

/* ================================================================
   TRANSACTION NGHIEP VU LAP TOA THUOC
   - Kiem tra phieu kham va dot dieu tri chua dong.
   - Chan toa trung theo MaPK.
   - Kiem tra danh muc thuoc.
   - Insert ToaThuoc va tat ca ChiTietToaThuoc trong mot transaction.
   - Neu bat ky buoc nao loi: rollback toan bo.
   ================================================================ */
CREATE OR ALTER PROCEDURE dbo.usp_LapToaThuoc
    @MaPK     INT,
    @NgayKe   DATE,
    @LoiDan   NVARCHAR(1000) = NULL,
    @ChiTiet  dbo.ToaThuocChiTietType READONLY
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @NgayKe IS NULL
    BEGIN
        THROW 50001, N'Ngay ke toa khong duoc de trong.', 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM @ChiTiet)
    BEGIN
        THROW 50002, N'Toa thuoc phai co it nhat mot dong thuoc.', 1;
    END;

    IF EXISTS
    (
        SELECT 1 FROM @ChiTiet
        WHERE LEN(LTRIM(RTRIM(LieuDung))) = 0
           OR LEN(LTRIM(RTRIM(CachDung))) = 0
           OR SoLuong <= 0 OR SoNgay <= 0
    )
    BEGIN
        THROW 50007, N'Lieu dung, cach dung, so luong va so ngay phai hop le.', 1;
    END;

    DECLARE @TrangThaiDot NVARCHAR(20);
    DECLARE @MaToa INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        /* Khoa dong duoc doc trong khi kiem tra dieu kien lap toa. */
        SELECT @TrangThaiDot = d.TrangThai
        FROM dbo.PhieuKham AS pk WITH (UPDLOCK, HOLDLOCK)
        INNER JOIN dbo.DotDieuTri AS d WITH (UPDLOCK, HOLDLOCK)
            ON d.MaDT = pk.MaDT
        WHERE pk.MaPK = @MaPK;

        IF @TrangThaiDot IS NULL
        BEGIN
            THROW 50003, N'Khong tim thay phieu kham hop le.', 1;
        END;

        IF @TrangThaiDot = N'DaDong'
        BEGIN
            THROW 50004, N'Dot dieu tri da dong, khong the lap toa.', 1;
        END;

        /* UNIQUE(MaPK) trong ToaThuoc la lop bao ve cuoi cung chong toa trung. */
        IF EXISTS
        (
            SELECT 1
            FROM dbo.ToaThuoc WITH (UPDLOCK, HOLDLOCK)
            WHERE MaPK = @MaPK
        )
        BEGIN
            THROW 50005, N'Phieu kham da co toa thuoc.', 1;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM @ChiTiet AS ct
            LEFT JOIN dbo.Thuoc AS t WITH (HOLDLOCK)
                ON t.MaThuoc = ct.MaThuoc
            WHERE t.MaThuoc IS NULL OR t.ConSuDung = 0
        )
        BEGIN
            THROW 50006, N'Co thuoc khong ton tai hoac da ngung su dung.', 1;
        END;

        INSERT INTO dbo.ToaThuoc (MaPK, NgayKe, LoiDan)
        VALUES (@MaPK, @NgayKe, @LoiDan);

        SET @MaToa = CONVERT(INT, SCOPE_IDENTITY());

        INSERT INTO dbo.ChiTietToaThuoc
            (MaToa, MaThuoc, LieuDung, CachDung, SoLuong, SoNgay)
        SELECT
            @MaToa, ct.MaThuoc, ct.LieuDung, ct.CachDung, ct.SoLuong, ct.SoNgay
        FROM @ChiTiet AS ct;

        COMMIT TRANSACTION;

        SELECT @MaToa AS MaToa, @MaPK AS MaPK, @NgayKe AS NgayKe,
               N'ThanhCong' AS KetQua;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

/* GHI CHU THIET KE:
   1. MaBN, MaHS, MaPK, MaToa... la khoa chinh identity.
   2. FK khong dung ON DELETE CASCADE de tranh xoa nham lich su y te.
   3. UNIQUE(MaBN, NamApDung): mot benh nhan khong co hai le phi cung nam.
   4. UNIQUE(HoSoBenhAn.MaBN): mot ho so chinh/benh nhan theo gia dinh bai.
   5. UNIQUE(ToaThuoc.MaPK): toi da mot toa/ phieu kham theo gia dinh bai.
   6. UNIQUE(HoaDonVienPhi.MaDT): mot hoa don/dot dieu tri theo gia dinh bai.
   7. Can xac nhan lai cac gia dinh 4-6 voi nghiep vu benh vien thuc te.
*/

/* ================================================================
   CAC THU TUC NGHIEP VU PHUC VU UNG DUNG WINFORMS
   ================================================================ */
CREATE OR ALTER PROCEDURE dbo.usp_GhiNhanLePhiNam
    @MaBN INT,
    @NamApDung SMALLINT,
    @SoTien DECIMAL(12,2),
    @TrangThai NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF @SoTien < 0 THROW 50101, N'So tien le phi khong duoc am.', 1;
    IF @TrangThai NOT IN (N'ChuaDong', N'DaDong', N'Huy')
        THROW 50102, N'Trang thai le phi khong hop le.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.BenhNhan WHERE MaBN = @MaBN)
        THROW 50103, N'Khong tim thay benh nhan.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;
        IF EXISTS (SELECT 1 FROM dbo.LePhiNam WITH (UPDLOCK, HOLDLOCK)
                   WHERE MaBN = @MaBN AND NamApDung = @NamApDung)
        BEGIN
            UPDATE dbo.LePhiNam
            SET SoTien = @SoTien,
                TrangThai = @TrangThai,
                NgayDong = CASE WHEN @TrangThai = N'DaDong'
                                THEN COALESCE(NgayDong, CONVERT(date, SYSDATETIME()))
                                ELSE NULL END
            WHERE MaBN = @MaBN AND NamApDung = @NamApDung;
        END
        ELSE
        BEGIN
            INSERT dbo.LePhiNam(MaBN, NamApDung, NgayDong, SoTien, TrangThai)
            VALUES(@MaBN, @NamApDung,
                   CASE WHEN @TrangThai = N'DaDong' THEN CONVERT(date, SYSDATETIME()) ELSE NULL END,
                   @SoTien, @TrangThai);
        END;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_CapHoSoBenhAn
    @MaBN INT,
    @MaHS INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET @MaHS = NULL;
    BEGIN TRY
        BEGIN TRANSACTION;
        SELECT @MaHS = MaHS
        FROM dbo.HoSoBenhAn WITH (UPDLOCK, HOLDLOCK)
        WHERE MaBN = @MaBN;

        IF @MaHS IS NULL
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM dbo.BenhNhan WHERE MaBN = @MaBN)
                THROW 50110, N'Khong tim thay benh nhan.', 1;
            IF NOT EXISTS
            (
                SELECT 1 FROM dbo.LePhiNam
                WHERE MaBN = @MaBN AND NamApDung = YEAR(GETDATE()) AND TrangThai = N'DaDong'
            )
                THROW 50111, N'Benh nhan chua dong le phi nam hien tai; khong the cap ho so.', 1;

            INSERT dbo.HoSoBenhAn(MaBN, TrangThai)
            VALUES(@MaBN, N'DangHoatDong');
            SET @MaHS = CONVERT(INT, SCOPE_IDENTITY());
        END;
        COMMIT TRANSACTION;
        SELECT @MaHS AS MaHS;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_TaoDotVaPhieuKham
    @MaHS INT,
    @MaBS INT,
    @MaKhoa INT,
    @TrieuChung NVARCHAR(1000) = NULL,
    @ThanNhiet DECIMAL(4,1) = NULL,
    @HuyetAp NVARCHAR(30) = NULL,
    @GhiChu NVARCHAR(1000) = NULL,
    @LoaiDot NVARCHAR(20) = N'DieuTri'
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF @LoaiDot NOT IN (N'DieuTri', N'KhamDinhKy')
        THROW 50120, N'Loai dot dieu tri khong hop le.', 1;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM dbo.HoSoBenhAn WITH (UPDLOCK, HOLDLOCK)
                       WHERE MaHS = @MaHS AND TrangThai = N'DangHoatDong')
            THROW 50121, N'Ho so khong ton tai hoac dang tam khoa/luu tru.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.BacSi WHERE MaBS = @MaBS AND ConHanhNghe = 1)
            THROW 50122, N'Bac si khong ton tai hoac khong con hanh nghe.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.KhoaDieuTri WHERE MaKhoa = @MaKhoa)
            THROW 50123, N'Khong tim thay khoa dieu tri.', 1;

        INSERT dbo.DotDieuTri(MaHS, LoaiDot, TrangThai)
        VALUES(@MaHS, @LoaiDot, N'DangXuLy');
        DECLARE @MaDT INT = CONVERT(INT, SCOPE_IDENTITY());

        INSERT dbo.PhieuKham(MaDT, MaBS, MaKhoa, TrieuChung, ThanNhiet, HuyetAp, GhiChu)
        VALUES(@MaDT, @MaBS, @MaKhoa, @TrieuChung, @ThanNhiet, @HuyetAp, @GhiChu);
        DECLARE @MaPK INT = CONVERT(INT, SCOPE_IDENTITY());

        COMMIT TRANSACTION;
        SELECT @MaDT AS MaDT, @MaPK AS MaPK;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_GhiNhanKetQuaXetNghiem
    @MaXN INT,
    @KetQua NVARCHAR(MAX),
    @TrangThai NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    IF @TrangThai NOT IN (N'ChoThucHien', N'DangThucHien', N'DaCoKetQua', N'Huy')
        THROW 50130, N'Trang thai xet nghiem khong hop le.', 1;
    IF @TrangThai = N'DaCoKetQua' AND NULLIF(LTRIM(RTRIM(@KetQua)), N'') IS NULL
        THROW 50131, N'Can nhap ket qua truoc khi danh dau da co ket qua.', 1;
    UPDATE dbo.XetNghiem
    SET KetQua = @KetQua,
        TrangThai = @TrangThai,
        NgayThucHien = CASE WHEN @TrangThai = N'DaCoKetQua'
                            THEN COALESCE(NgayThucHien, SYSDATETIME())
                            ELSE NgayThucHien END
    WHERE MaXN = @MaXN;
    IF @@ROWCOUNT = 0 THROW 50132, N'Khong tim thay phieu xet nghiem.', 1;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_TaoHoaDonVienPhi
    @MaDT INT,
    @TongTien DECIMAL(12,2)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF @TongTien < 0 THROW 50140, N'Tong tien khong duoc am.', 1;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM dbo.DotDieuTri WITH (UPDLOCK, HOLDLOCK) WHERE MaDT = @MaDT)
            THROW 50141, N'Khong tim thay dot dieu tri.', 1;
        IF EXISTS (SELECT 1 FROM dbo.HoaDonVienPhi WITH (UPDLOCK, HOLDLOCK) WHERE MaDT = @MaDT)
            THROW 50142, N'Dot dieu tri da co hoa don.', 1;
        INSERT dbo.HoaDonVienPhi(MaDT, TongTien, TrangThai)
        VALUES(@MaDT, @TongTien, CASE WHEN @TongTien = 0 THEN N'DaThanhToan' ELSE N'ChuaThanhToan' END);
        DECLARE @MaHD INT = CONVERT(INT, SCOPE_IDENTITY());
        COMMIT TRANSACTION;
        SELECT @MaHD AS MaHD;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_ThanhToanVienPhi
    @MaHD INT,
    @SoTien DECIMAL(12,2),
    @PhuongThuc NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF @SoTien <= 0 THROW 50150, N'So tien thanh toan phai lon hon 0.', 1;
    IF @PhuongThuc NOT IN (N'TienMat', N'ChuyenKhoan', N'The', N'Khac')
        THROW 50151, N'Phuong thuc thanh toan khong hop le.', 1;
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @TongTien DECIMAL(12,2), @TrangThaiHD NVARCHAR(20), @DaTra DECIMAL(12,2);
        SELECT @TongTien = TongTien, @TrangThaiHD = TrangThai
        FROM dbo.HoaDonVienPhi WITH (UPDLOCK, HOLDLOCK)
        WHERE MaHD = @MaHD;
        IF @TongTien IS NULL THROW 50152, N'Khong tim thay hoa don.', 1;
        IF @TrangThaiHD = N'Huy' THROW 50153, N'Hoa don da huy, khong the thanh toan.', 1;

        SELECT @DaTra = COALESCE(SUM(SoTien), 0)
        FROM dbo.ThanhToanVienPhi WITH (UPDLOCK, HOLDLOCK)
        WHERE MaHD = @MaHD AND TrangThai = N'ThanhCong';
        IF @SoTien > @TongTien - @DaTra
            THROW 50154, N'So tien thanh toan vuot qua so tien con no.', 1;

        INSERT dbo.ThanhToanVienPhi(MaHD, SoTien, PhuongThuc, TrangThai)
        VALUES(@MaHD, @SoTien, @PhuongThuc, N'ThanhCong');
        SET @DaTra = @DaTra + @SoTien;
        UPDATE dbo.HoaDonVienPhi
        SET TrangThai = CASE WHEN @DaTra >= TongTien THEN N'DaThanhToan'
                             WHEN @DaTra > 0 THEN N'ThanhToanMotPhan'
                             ELSE N'ChuaThanhToan' END
        WHERE MaHD = @MaHD;
        COMMIT TRANSACTION;
        SELECT @MaHD AS MaHD, @DaTra AS DaThanhToan, (@TongTien - @DaTra) AS ConNo;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

/* Du lieu danh muc DEMO de co the mo form va thu nghiem ngay sau khi chay script. */
IF NOT EXISTS (SELECT 1 FROM dbo.BacSi)
    INSERT dbo.BacSi(HoTen, ChuyenKhoa, DienThoai, ConHanhNghe)
    VALUES (N'Bác sĩ Demo', N'Nội tổng quát', '0900000001', 1);
GO
IF NOT EXISTS (SELECT 1 FROM dbo.KhoaDieuTri)
    INSERT dbo.KhoaDieuTri(TenKhoa) VALUES (N'Nội tổng quát'), (N'Nhi'), (N'Xét nghiệm');
GO
IF NOT EXISTS (SELECT 1 FROM dbo.NhanVienCSKH)
    INSERT dbo.NhanVienCSKH(HoTen, DienThoai, Email, ConHoatDong)
    VALUES (N'Nhân viên CSKH Demo', '0900000002', 'cskh.demo@example.local', 1);
GO
IF NOT EXISTS (SELECT 1 FROM dbo.Thuoc)
    INSERT dbo.Thuoc(TenThuoc, DonVi, HoatChat, HamLuong, ConSuDung)
    VALUES
      (N'Paracetamol (DEMO)', N'Viên', N'Paracetamol', N'500 mg', 1),
      (N'Nước muối sinh lý (DEMO)', N'Chai', N'Natri clorid', N'0,9%', 1),
      (N'Vitamin C (DEMO)', N'Viên', N'Ascorbic acid', N'500 mg', 1);
GO

CREATE OR ALTER PROCEDURE dbo.usp_DongDotDieuTri
    @MaDT INT,
    @KetQuaTongKet NVARCHAR(1000) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @TrangThai NVARCHAR(20);
        SELECT @TrangThai = TrangThai FROM dbo.DotDieuTri WITH (UPDLOCK, HOLDLOCK) WHERE MaDT = @MaDT;
        IF @TrangThai IS NULL THROW 50160, N'Khong tim thay dot dieu tri.', 1;
        IF @TrangThai = N'DaDong' THROW 50161, N'Dot dieu tri da dong truoc do.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.HoaDonVienPhi WHERE MaDT = @MaDT AND TrangThai = N'DaThanhToan')
            THROW 50162, N'Can co hoa don da thanh toan day du truoc khi dong dot dieu tri.', 1;
        UPDATE dbo.DotDieuTri
        SET TrangThai = N'DaDong',
            NgayKetThuc = COALESCE(NgayKetThuc, SYSDATETIME()),
            NgayXuatVien = COALESCE(NgayXuatVien, SYSDATETIME()),
            KetQuaTongKet = @KetQuaTongKet
        WHERE MaDT = @MaDT;
        COMMIT TRANSACTION;
        SELECT @MaDT AS MaDT, N'DaDong' AS TrangThai;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO
