USE master;
GO

IF DB_ID(N'QuanLyThuVienDB') IS NOT NULL
BEGIN
    ALTER DATABASE QuanLyThuVienDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyThuVienDB;
END
GO

CREATE DATABASE QuanLyThuVienDB;
GO

USE QuanLyThuVienDB;
GO
CREATE TABLE dbo.DocGia (
    MaDocGia       NVARCHAR(20)  NOT NULL,
    Ho             NVARCHAR(50)  NOT NULL,
    Ten            NVARCHAR(50)  NOT NULL,
    Email          NVARCHAR(150) NOT NULL,
    SoDienThoai    NVARCHAR(20)  NULL,
    DonVi          NVARCHAR(150) NULL,
    NgaySinh       DATE          NULL,
    NgayTao       DATETIME2      NOT NULL
        CONSTRAINT DF_DocGia_NgayTao DEFAULT SYSDATETIME(),
    TrangThai      BIT           NOT NULL
        CONSTRAINT DF_DocGia_TrangThai DEFAULT 1,

    CONSTRAINT PK_DocGia PRIMARY KEY (MaDocGia),
    CONSTRAINT UQ_DocGia_Email UNIQUE (Email)
);
GO

CREATE TABLE dbo.TaiKhoan (
    MaTaiKhoan     INT IDENTITY(1,1) NOT NULL,
    MaDocGia       NVARCHAR(20) NULL,
    MaNhanVien     NVARCHAR(20) NULL,
    TenDangNhap    NVARCHAR(50) NOT NULL,
    MatKhauHash    NVARCHAR(255) NOT NULL,
    VaiTro         NVARCHAR(20) NOT NULL,
    NgayTao        DATETIME2 NOT NULL
        CONSTRAINT DF_TaiKhoan_NgayTao DEFAULT SYSDATETIME(),
    DangHoatDong   BIT NOT NULL
        CONSTRAINT DF_TaiKhoan_DangHoatDong DEFAULT 1,

    CONSTRAINT PK_TaiKhoan PRIMARY KEY (MaTaiKhoan),
    CONSTRAINT UQ_TaiKhoan_TenDangNhap UNIQUE (TenDangNhap),
    CONSTRAINT CK_TaiKhoan_VaiTro
        CHECK (VaiTro IN (N'DocGia', N'ThuThu', N'Admin')),
    CONSTRAINT CK_TaiKhoan_ChuSoHuu
        CHECK (
            (MaDocGia IS NOT NULL AND MaNhanVien IS NULL)
            OR
            (MaDocGia IS NULL AND MaNhanVien IS NOT NULL)
        )
);
GO

CREATE INDEX IX_TaiKhoan_MaDocGia
ON dbo.TaiKhoan(MaDocGia);
GO

CREATE TABLE dbo.NhanVien (
    MaNhanVien    NVARCHAR(20)  NOT NULL,
    Ho            NVARCHAR(50)  NOT NULL,
    Ten           NVARCHAR(50)  NOT NULL,
    Phai          NVARCHAR(10)  NOT NULL,
    NgaySinh      DATE          NOT NULL,
    ChucVu        NVARCHAR(80)  NOT NULL,
    SoDienThoai   NVARCHAR(20)  NULL,
    Email         NVARCHAR(150) NULL,
    TrangThai     BIT NOT NULL
        CONSTRAINT DF_NhanVien_TrangThai DEFAULT 1,

    CONSTRAINT PK_NhanVien PRIMARY KEY (MaNhanVien)
);
GO

CREATE UNIQUE INDEX UX_NhanVien_Email
ON dbo.NhanVien(Email)
WHERE Email IS NOT NULL;
GO

ALTER TABLE dbo.TaiKhoan
ADD CONSTRAINT FK_TaiKhoan_NhanVien
FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien(MaNhanVien);
GO

CREATE TABLE dbo.TheDocGia (
    MaThe        NVARCHAR(20) NOT NULL,
    MaDocGia     NVARCHAR(20) NOT NULL,
    NgayCap      DATE NOT NULL,
    HanSuDung    DATE NOT NULL,
    TrangThai    BIT NOT NULL
        CONSTRAINT DF_TheDocGia_TrangThai DEFAULT 1,

    CONSTRAINT PK_TheDocGia PRIMARY KEY (MaThe),
    CONSTRAINT FK_TheDocGia_DocGia
        FOREIGN KEY (MaDocGia) REFERENCES dbo.DocGia(MaDocGia),
    CONSTRAINT CK_TheDocGia_Ngay
        CHECK (HanSuDung >= NgayCap)
);
GO

CREATE UNIQUE INDEX UX_TheDocGia_MaDocGia_DangHoatDong
ON dbo.TheDocGia(MaDocGia)
WHERE TrangThai = 1;
GO

CREATE TABLE dbo.TheLoai (
    MaTheLoai     NVARCHAR(20)  NOT NULL,
    TenTheLoai    NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_TheLoai PRIMARY KEY (MaTheLoai),
    CONSTRAINT UQ_TheLoai_Ten UNIQUE (TenTheLoai)
);
GO

CREATE TABLE dbo.NhaXuatBan (
    MaNhaXuatBan  NVARCHAR(20)  NOT NULL,
    TenNhaXuatBan NVARCHAR(150) NOT NULL,
    DiaChi        NVARCHAR(250) NULL,
    SoDienThoai   NVARCHAR(20)  NULL,
    Email         NVARCHAR(150) NULL,

    CONSTRAINT PK_NhaXuatBan PRIMARY KEY (MaNhaXuatBan),
    CONSTRAINT UQ_NhaXuatBan_Ten UNIQUE (TenNhaXuatBan)
);
GO

CREATE TABLE dbo.TacGia (
    MaTacGia      NVARCHAR(20)  NOT NULL,
    TenTacGia     NVARCHAR(150) NOT NULL,

    CONSTRAINT PK_TacGia PRIMARY KEY (MaTacGia)
);
GO

CREATE TABLE dbo.DauSach (
    MaDauSach       NVARCHAR(20)  NOT NULL,
    TenSach         NVARCHAR(250) NOT NULL,
    MaTheLoai       NVARCHAR(20)  NOT NULL,
    MaNhaXuatBan    NVARCHAR(20)  NULL,
    NamXuatBan      INT           NULL,
    ISBN            NVARCHAR(30)  NULL,
    MoTa            NVARCHAR(MAX) NULL,
    LoaiTaiLieu     NVARCHAR(20)  NOT NULL,
    SoLuongTong     INT           NOT NULL
        CONSTRAINT DF_DauSach_SoLuongTong DEFAULT 0,
    SoLuongHienCo   INT           NOT NULL
        CONSTRAINT DF_DauSach_SoLuongHienCo DEFAULT 0,
    TrangThai       BIT           NOT NULL
        CONSTRAINT DF_DauSach_TrangThai DEFAULT 1,
    NgayCapNhat     DATETIME2     NOT NULL
        CONSTRAINT DF_DauSach_NgayCapNhat DEFAULT SYSDATETIME(),

    CONSTRAINT PK_DauSach PRIMARY KEY (MaDauSach),
    CONSTRAINT FK_DauSach_TheLoai
        FOREIGN KEY (MaTheLoai) REFERENCES dbo.TheLoai(MaTheLoai),
    CONSTRAINT FK_DauSach_NhaXuatBan
        FOREIGN KEY (MaNhaXuatBan) REFERENCES dbo.NhaXuatBan(MaNhaXuatBan),
    CONSTRAINT UQ_DauSach_ISBN UNIQUE (ISBN),
    CONSTRAINT CK_DauSach_Loai
        CHECK (LoaiTaiLieu IN (N'SachThuong', N'SachDienTu')),
    CONSTRAINT CK_DauSach_SoLuongTong
        CHECK (SoLuongTong >= 0),
    CONSTRAINT CK_DauSach_SoLuongHienCo
        CHECK (SoLuongHienCo >= 0 AND SoLuongHienCo <= SoLuongTong),
    CONSTRAINT CK_DauSach_NamXuatBan
        CHECK (NamXuatBan IS NULL OR NamXuatBan BETWEEN 1000 AND YEAR(GETDATE()))
);
GO

CREATE TABLE dbo.DauSach_TacGia (
    MaDauSach    NVARCHAR(20) NOT NULL,
    MaTacGia     NVARCHAR(20) NOT NULL,

    CONSTRAINT PK_DauSach_TacGia
        PRIMARY KEY (MaDauSach, MaTacGia),
    CONSTRAINT FK_DSTG_DauSach
        FOREIGN KEY (MaDauSach) REFERENCES dbo.DauSach(MaDauSach),
    CONSTRAINT FK_DSTG_TacGia
        FOREIGN KEY (MaTacGia) REFERENCES dbo.TacGia(MaTacGia)
);
GO


CREATE TABLE dbo.SachDienTu (
    MaDauSach       NVARCHAR(20) NOT NULL,
    DuongDanFile    NVARCHAR(500) NOT NULL,
    DinhDangFile    NVARCHAR(20)  NOT NULL,
    KichThuocKB     BIGINT        NULL,
    ChoPhepDoc      BIT           NOT NULL
        CONSTRAINT DF_SachDienTu_ChoPhepDoc DEFAULT 1,
    ChoPhepTai      BIT           NOT NULL
        CONSTRAINT DF_SachDienTu_ChoPhepTai DEFAULT 1,
    NgayCapNhat     DATETIME2     NOT NULL
        CONSTRAINT DF_SachDienTu_NgayCapNhat DEFAULT SYSDATETIME(),

    CONSTRAINT PK_SachDienTu PRIMARY KEY (MaDauSach),
    CONSTRAINT FK_SachDienTu_DauSach
        FOREIGN KEY (MaDauSach) REFERENCES dbo.DauSach(MaDauSach),
    CONSTRAINT CK_SachDienTu_KichThuoc
        CHECK (KichThuocKB IS NULL OR KichThuocKB >= 0)
);
GO


CREATE TABLE dbo.PhieuMuon (
    MaPhieuMuon     INT IDENTITY(1,1) NOT NULL,
    MaDocGia        NVARCHAR(20) NOT NULL,
    MaThe           NVARCHAR(20) NOT NULL,
    MaNhanVien      NVARCHAR(20) NULL,
    NgayMuon        DATETIME2 NOT NULL
        CONSTRAINT DF_PhieuMuon_NgayMuon DEFAULT SYSDATETIME(),
    HanTra          DATE NOT NULL,
    NgayTra         DATETIME2 NULL,
    TrangThai       NVARCHAR(30) NOT NULL
        CONSTRAINT DF_PhieuMuon_TrangThai DEFAULT N'DangMuon',
    GhiChu          NVARCHAR(500) NULL,

    CONSTRAINT PK_PhieuMuon PRIMARY KEY (MaPhieuMuon),
    CONSTRAINT FK_PhieuMuon_DocGia
        FOREIGN KEY (MaDocGia) REFERENCES dbo.DocGia(MaDocGia),
    CONSTRAINT FK_PhieuMuon_The
        FOREIGN KEY (MaThe) REFERENCES dbo.TheDocGia(MaThe),
    CONSTRAINT FK_PhieuMuon_NhanVien
        FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien(MaNhanVien),
    CONSTRAINT CK_PhieuMuon_TrangThai
        CHECK (TrangThai IN
            (N'DangMuon', N'DaTra', N'QuaHan', N'Huy')),
    CONSTRAINT CK_PhieuMuon_HanTra
        CHECK (HanTra >= CAST(NgayMuon AS DATE)),
    CONSTRAINT CK_PhieuMuon_NgayTra
        CHECK (NgayTra IS NULL OR NgayTra >= NgayMuon)
);
GO

CREATE TABLE dbo.ChiTietPhieuMuon (
    MaPhieuMuon     INT NOT NULL,
    MaDauSach       NVARCHAR(20) NOT NULL,
    SoLuong         INT NOT NULL
        CONSTRAINT DF_ChiTietPhieuMuon_SoLuong DEFAULT 1,
    HanSuDung       DATE NULL,

    CONSTRAINT PK_ChiTietPhieuMuon
        PRIMARY KEY (MaPhieuMuon, MaDauSach),
    CONSTRAINT FK_CTPM_PhieuMuon
        FOREIGN KEY (MaPhieuMuon) REFERENCES dbo.PhieuMuon(MaPhieuMuon),
    CONSTRAINT FK_CTPM_DauSach
        FOREIGN KEY (MaDauSach) REFERENCES dbo.DauSach(MaDauSach),
    CONSTRAINT CK_CTPM_SoLuong
        CHECK (SoLuong > 0)
);
GO


CREATE TABLE dbo.PhieuPhat (
    MaPhieuPhat    INT IDENTITY(1,1) NOT NULL,
    MaPhieuMuon    INT NOT NULL,
    MaDocGia       NVARCHAR(20) NOT NULL,
    SoTien         DECIMAL(18,2) NOT NULL,
    LyDo           NVARCHAR(250) NOT NULL,
    NgayLap        DATETIME2 NOT NULL
        CONSTRAINT DF_PhieuPhat_NgayLap DEFAULT SYSDATETIME(),
    DaThanhToan    BIT NOT NULL
        CONSTRAINT DF_PhieuPhat_DaThanhToan DEFAULT 0,

    CONSTRAINT PK_PhieuPhat PRIMARY KEY (MaPhieuPhat),
    CONSTRAINT FK_PhieuPhat_PhieuMuon
        FOREIGN KEY (MaPhieuMuon) REFERENCES dbo.PhieuMuon(MaPhieuMuon),
    CONSTRAINT FK_PhieuPhat_DocGia
        FOREIGN KEY (MaDocGia) REFERENCES dbo.DocGia(MaDocGia),
    CONSTRAINT CK_PhieuPhat_SoTien
        CHECK (SoTien >= 0)
);
GO


CREATE TABLE dbo.YeuCauDatMua (
    MaYeuCau       INT IDENTITY(1,1) NOT NULL,
    MaDocGia       NVARCHAR(20) NOT NULL,
    MaDauSach      NVARCHAR(20) NULL,
    TenSachYeuCau  NVARCHAR(250) NOT NULL,
    TenTacGia      NVARCHAR(150) NULL,
    NamXuatBan     INT NULL,
    NgayYeuCau     DATETIME2 NOT NULL
        CONSTRAINT DF_YCDM_NgayYeuCau DEFAULT SYSDATETIME(),
    TrangThai      NVARCHAR(30) NOT NULL
        CONSTRAINT DF_YCDM_TrangThai DEFAULT N'ChoXuLy',
    PhanHoi        NVARCHAR(500) NULL,
    MaNhanVienXuLy NVARCHAR(20) NULL,
    NgayXuLy       DATETIME2 NULL,

    CONSTRAINT PK_YeuCauDatMua PRIMARY KEY (MaYeuCau),
    CONSTRAINT FK_YCDM_DocGia
        FOREIGN KEY (MaDocGia) REFERENCES dbo.DocGia(MaDocGia),
    CONSTRAINT FK_YCDM_DauSach
        FOREIGN KEY (MaDauSach) REFERENCES dbo.DauSach(MaDauSach),
    CONSTRAINT FK_YCDM_NhanVien
        FOREIGN KEY (MaNhanVienXuLy) REFERENCES dbo.NhanVien(MaNhanVien),
    CONSTRAINT CK_YCDM_TrangThai
        CHECK (TrangThai IN
            (N'ChoXuLy', N'ChapNhan', N'TuChoi', N'HoanTat')),
    CONSTRAINT CK_YCDM_NamXuatBan
        CHECK (NamXuatBan IS NULL OR NamXuatBan BETWEEN 1000 AND YEAR(GETDATE()))
);
GO

CREATE TABLE dbo.ThanhToan (
    MaThanhToan     INT IDENTITY(1,1) NOT NULL,
    MaYeuCau        INT NOT NULL,
    SoTien          DECIMAL(18,2) NOT NULL,
    PhuongThuc      NVARCHAR(30) NOT NULL,
    MaGiaoDich      NVARCHAR(100) NULL,
    TrangThai       NVARCHAR(30) NOT NULL
        CONSTRAINT DF_ThanhToan_TrangThai DEFAULT N'ChoThanhToan',
    NgayThanhToan   DATETIME2 NULL,

    CONSTRAINT PK_ThanhToan PRIMARY KEY (MaThanhToan),
    CONSTRAINT FK_ThanhToan_YeuCau
        FOREIGN KEY (MaYeuCau) REFERENCES dbo.YeuCauDatMua(MaYeuCau),
    CONSTRAINT CK_ThanhToan_SoTien
        CHECK (SoTien >= 0),
    CONSTRAINT CK_ThanhToan_TrangThai
        CHECK (TrangThai IN
            (N'ChoThanhToan', N'ThanhCong', N'ThatBai', N'HoanTien'))
);
GO

CREATE UNIQUE INDEX UX_ThanhToan_MaGiaoDich
ON dbo.ThanhToan(MaGiaoDich)
WHERE MaGiaoDich IS NOT NULL;
GO


CREATE TABLE dbo.LichSuTruyCapTaiLieu (
    MaLichSu       BIGINT IDENTITY(1,1) NOT NULL,
    MaDocGia       NVARCHAR(20) NOT NULL,
    MaDauSach      NVARCHAR(20) NOT NULL,
    LoaiTruyCap    NVARCHAR(20) NOT NULL,
    ThoiGian       DATETIME2 NOT NULL
        CONSTRAINT DF_LSTL_ThoiGian DEFAULT SYSDATETIME(),
    DiaChiIP       NVARCHAR(50) NULL,

    CONSTRAINT PK_LichSuTruyCapTaiLieu PRIMARY KEY (MaLichSu),
    CONSTRAINT FK_LSTL_DocGia
        FOREIGN KEY (MaDocGia) REFERENCES dbo.DocGia(MaDocGia),
    CONSTRAINT FK_LSTL_DauSach
        FOREIGN KEY (MaDauSach) REFERENCES dbo.DauSach(MaDauSach),
    CONSTRAINT CK_LSTL_LoaiTruyCap
        CHECK (LoaiTruyCap IN (N'DocTrucTuyen', N'TaiTaiLieu'))
);
GO

CREATE TABLE dbo.ThongBaoEmail (
    MaThongBao      BIGINT IDENTITY(1,1) NOT NULL,
    MaPhieuMuon     INT NOT NULL,
    MaDocGia        NVARCHAR(20) NOT NULL,
    EmailNhan       NVARCHAR(150) NOT NULL,
    LoaiThongBao    NVARCHAR(30) NOT NULL,
    NgayDuKienGui   DATE NOT NULL,
    NgayGui         DATETIME2 NULL,
    TrangThai       NVARCHAR(20) NOT NULL
        CONSTRAINT DF_ThongBaoEmail_TrangThai DEFAULT N'ChoGui',
    NoiDung         NVARCHAR(MAX) NULL,

    CONSTRAINT PK_ThongBaoEmail PRIMARY KEY (MaThongBao),
    CONSTRAINT FK_ThongBaoEmail_PhieuMuon
        FOREIGN KEY (MaPhieuMuon) REFERENCES dbo.PhieuMuon(MaPhieuMuon),
    CONSTRAINT FK_ThongBaoEmail_DocGia
        FOREIGN KEY (MaDocGia) REFERENCES dbo.DocGia(MaDocGia),
    CONSTRAINT CK_ThongBaoEmail_Loai
        CHECK (LoaiThongBao IN (N'NhacHanTra')),
    CONSTRAINT CK_ThongBaoEmail_TrangThai
        CHECK (TrangThai IN (N'ChoGui', N'DaGui', N'Loi'))
);
GO

CREATE UNIQUE INDEX UX_ThongBaoEmail_PhieuMuon_Loai
ON dbo.ThongBaoEmail(MaPhieuMuon, LoaiThongBao);
GO

CREATE INDEX IX_DauSach_TenSach
ON dbo.DauSach(TenSach);
GO

CREATE INDEX IX_DauSach_TheLoai
ON dbo.DauSach(MaTheLoai);
GO

CREATE INDEX IX_DauSach_NamXuatBan
ON dbo.DauSach(NamXuatBan);
GO

CREATE INDEX IX_DauSach_TrangThai_Loai
ON dbo.DauSach(TrangThai, LoaiTaiLieu);
GO

CREATE INDEX IX_DauSach_TacGia
ON dbo.DauSach_TacGia(MaTacGia, MaDauSach);
GO

CREATE INDEX IX_PhieuMuon_DocGia_TrangThai
ON dbo.PhieuMuon(MaDocGia, TrangThai);
GO

CREATE INDEX IX_PhieuMuon_HanTra
ON dbo.PhieuMuon(HanTra, TrangThai);
GO

CREATE INDEX IX_YeuCauDatMua_TrangThai
ON dbo.YeuCauDatMua(TrangThai, NgayYeuCau);
GO

CREATE VIEW dbo.vw_TimKiemTaiLieu
AS
SELECT
    ds.MaDauSach,
    ds.TenSach,
    tl.TenTheLoai,
    nxb.TenNhaXuatBan,
    ds.NamXuatBan,
    ds.ISBN,
    ds.LoaiTaiLieu,
    ds.SoLuongTong,
    ds.SoLuongHienCo,
    ds.TrangThai,
    STUFF((
        SELECT N', ' + tg.TenTacGia
        FROM dbo.DauSach_TacGia dstg
        INNER JOIN dbo.TacGia tg
            ON tg.MaTacGia = dstg.MaTacGia
        WHERE dstg.MaDauSach = ds.MaDauSach
        FOR XML PATH(N''), TYPE
    ).value(N'.', N'nvarchar(max)'), 1, 2, N'') AS TacGia
FROM dbo.DauSach ds
INNER JOIN dbo.TheLoai tl
    ON tl.MaTheLoai = ds.MaTheLoai
LEFT JOIN dbo.NhaXuatBan nxb
    ON nxb.MaNhaXuatBan = ds.MaNhaXuatBan
WHERE ds.TrangThai = 1;
GO

CREATE VIEW dbo.vw_SachQuaHan
AS
SELECT
    pm.MaPhieuMuon,
    pm.MaDocGia,
    dg.Ho + N' ' + dg.Ten AS HoTenDocGia,
    dg.Email,
    pm.NgayMuon,
    pm.HanTra,
    DATEDIFF(DAY, pm.HanTra, CAST(GETDATE() AS DATE)) AS SoNgayQuaHan,
    ctp.MaDauSach,
    ds.TenSach,
    ctp.SoLuong
FROM dbo.PhieuMuon pm
INNER JOIN dbo.DocGia dg
    ON dg.MaDocGia = pm.MaDocGia
INNER JOIN dbo.ChiTietPhieuMuon ctp
    ON ctp.MaPhieuMuon = pm.MaPhieuMuon
INNER JOIN dbo.DauSach ds
    ON ds.MaDauSach = ctp.MaDauSach
WHERE pm.NgayTra IS NULL
  AND pm.HanTra < CAST(GETDATE() AS DATE);
GO


CREATE PROCEDURE dbo.sp_DangKyMuonSach
    @MaDocGia       NVARCHAR(20),
    @MaThe          NVARCHAR(20),
    @MaDauSach      NVARCHAR(20),
    @SoLuong        INT = 1,
    @SoNgayMuon     INT = 14,
    @MaNhanVien     NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF @SoLuong <= 0
            THROW 50001, N'So luong muon phai lon hon 0.', 1;

        IF NOT EXISTS (
            SELECT 1
            FROM dbo.DocGia
            WHERE MaDocGia = @MaDocGia
              AND TrangThai = 1
        )
            THROW 50002, N'Doc gia khong ton tai hoac da bi khoa.', 1;

        IF NOT EXISTS (
            SELECT 1
            FROM dbo.TheDocGia
            WHERE MaThe = @MaThe
              AND MaDocGia = @MaDocGia
              AND TrangThai = 1
              AND HanSuDung >= CAST(GETDATE() AS DATE)
        )
            THROW 50003, N'Ma the khong hop le hoac da het han.', 1;

        DECLARE @SoLuongHienCo INT;

        SELECT @SoLuongHienCo = SoLuongHienCo
        FROM dbo.DauSach WITH (UPDLOCK, HOLDLOCK)
        WHERE MaDauSach = @MaDauSach
          AND LoaiTaiLieu = N'SachThuong'
          AND TrangThai = 1;

        IF @SoLuongHienCo IS NULL
            THROW 50004, N'Dau sach khong ton tai hoac khong phai sach trong thu vien.', 1;

        IF @SoLuongHienCo < @SoLuong
            THROW 50005, N'So luong sach hien co khong du.', 1;

        DECLARE @MaPhieuMuon INT;
        DECLARE @NgayMuon DATETIME2 = SYSDATETIME();
        DECLARE @HanTra DATE = DATEADD(DAY, @SoNgayMuon, CAST(@NgayMuon AS DATE));

        INSERT INTO dbo.PhieuMuon
        (
            MaDocGia, MaThe, MaNhanVien, NgayMuon, HanTra, TrangThai
        )
        VALUES
        (
            @MaDocGia, @MaThe, @MaNhanVien, @NgayMuon, @HanTra, N'DangMuon'
        );

        SET @MaPhieuMuon = SCOPE_IDENTITY();

        INSERT INTO dbo.ChiTietPhieuMuon
        (
            MaPhieuMuon, MaDauSach, SoLuong, HanSuDung
        )
        VALUES
        (
            @MaPhieuMuon, @MaDauSach, @SoLuong, @HanTra
        );

        UPDATE dbo.DauSach
        SET SoLuongHienCo = SoLuongHienCo - @SoLuong,
            NgayCapNhat = SYSDATETIME()
        WHERE MaDauSach = @MaDauSach;

        COMMIT TRANSACTION;

        SELECT
            @MaPhieuMuon AS MaPhieuMuon,
            @HanTra AS HanTra,
            N'Dang ky muon sach thanh cong.' AS ThongBao;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO


CREATE PROCEDURE dbo.sp_TraSach
    @MaPhieuMuon INT,
    @MaNhanVien  NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @NgayTra DATETIME2 = SYSDATETIME();

        IF NOT EXISTS (
            SELECT 1
            FROM dbo.PhieuMuon WITH (UPDLOCK, HOLDLOCK)
            WHERE MaPhieuMuon = @MaPhieuMuon
              AND NgayTra IS NULL
              AND TrangThai IN (N'DangMuon', N'QuaHan')
        )
            THROW 50006, N'Phieu muon khong ton tai hoac da duoc tra.', 1;

        UPDATE dbo.PhieuMuon
        SET NgayTra = @NgayTra,
            TrangThai = N'DaTra',
            MaNhanVien = COALESCE(@MaNhanVien, MaNhanVien)
        WHERE MaPhieuMuon = @MaPhieuMuon;

        UPDATE ds
        SET ds.SoLuongHienCo = ds.SoLuongHienCo + ctp.SoLuong,
            ds.NgayCapNhat = SYSDATETIME()
        FROM dbo.DauSach ds
        INNER JOIN dbo.ChiTietPhieuMuon ctp
            ON ctp.MaDauSach = ds.MaDauSach
        WHERE ctp.MaPhieuMuon = @MaPhieuMuon;

        COMMIT TRANSACTION;

        SELECT N'Tra sach thanh cong.' AS ThongBao;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO


CREATE PROCEDURE dbo.sp_TaoThongBaoHanTra
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.ThongBaoEmail
    (
        MaPhieuMuon,
        MaDocGia,
        EmailNhan,
        LoaiThongBao,
        NgayDuKienGui,
        TrangThai,
        NoiDung
    )
    SELECT
        pm.MaPhieuMuon,
        pm.MaDocGia,
        dg.Email,
        N'NhacHanTra',
        CAST(GETDATE() AS DATE),
        N'ChoGui',
        N'Thong bao: Sach co ma phieu muon '
        + CONVERT(NVARCHAR(20), pm.MaPhieuMuon)
        + N' se den han tra vao ngay '
        + CONVERT(NVARCHAR(10), pm.HanTra, 103)
        + N'. Vui long tra sach dung han.'
    FROM dbo.PhieuMuon pm
    INNER JOIN dbo.DocGia dg
        ON dg.MaDocGia = pm.MaDocGia
    WHERE pm.NgayTra IS NULL
      AND pm.HanTra = DATEADD(DAY, 3, CAST(GETDATE() AS DATE))
      AND NOT EXISTS (
          SELECT 1
          FROM dbo.ThongBaoEmail tb
          WHERE tb.MaPhieuMuon = pm.MaPhieuMuon
            AND tb.LoaiThongBao = N'NhacHanTra'
      );

    SELECT @@ROWCOUNT AS SoThongBaoDuocTao;
END;
GO

CREATE PROCEDURE dbo.sp_CapNhatSachQuaHan
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.PhieuMuon
    SET TrangThai = N'QuaHan'
    WHERE NgayTra IS NULL
      AND HanTra < CAST(GETDATE() AS DATE)
      AND TrangThai = N'DangMuon';

    SELECT @@ROWCOUNT AS SoPhieuQuaHan;
END;
GO

CREATE PROCEDURE dbo.sp_TimKiemTaiLieu
    @TuKhoa        NVARCHAR(250) = NULL,
    @MaTheLoai     NVARCHAR(20) = NULL,
    @MaTacGia      NVARCHAR(20) = NULL,
    @NamXuatBan    INT = NULL,
    @LoaiTaiLieu   NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT
        v.MaDauSach,
        v.TenSach,
        v.TacGia,
        v.TenTheLoai,
        v.TenNhaXuatBan,
        v.NamXuatBan,
        v.ISBN,
        v.LoaiTaiLieu,
        v.SoLuongTong,
        v.SoLuongHienCo
    FROM dbo.vw_TimKiemTaiLieu v
    INNER JOIN dbo.DauSach_TacGia dstg
        ON dstg.MaDauSach = v.MaDauSach
    WHERE
        (
            @TuKhoa IS NULL
            OR v.TenSach LIKE N'%' + @TuKhoa + N'%'
            OR v.TacGia LIKE N'%' + @TuKhoa + N'%'
            OR v.TenTheLoai LIKE N'%' + @TuKhoa + N'%'
            OR v.TenNhaXuatBan LIKE N'%' + @TuKhoa + N'%'
        )
        AND (@MaTheLoai IS NULL OR v.MaDauSach IN
            (SELECT MaDauSach
             FROM dbo.DauSach
             WHERE MaTheLoai = @MaTheLoai))
        AND (@MaTacGia IS NULL OR dstg.MaTacGia = @MaTacGia)
        AND (@NamXuatBan IS NULL OR v.NamXuatBan = @NamXuatBan)
        AND (@LoaiTaiLieu IS NULL OR v.LoaiTaiLieu = @LoaiTaiLieu)
    ORDER BY v.TenSach;
END;
GO


CREATE PROCEDURE dbo.sp_TraLoiYeuCauDatMua
    @MaYeuCau       INT,
    @MaNhanVien     NVARCHAR(20),
    @TrangThai      NVARCHAR(20),
    @PhanHoi        NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    IF @TrangThai NOT IN (N'ChapNhan', N'TuChoi')
        THROW 50007, N'Trang thai phan hoi khong hop le.', 1;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.YeuCauDatMua
        WHERE MaYeuCau = @MaYeuCau
    )
        THROW 50008, N'Khong tim thay yeu cau dat mua.', 1;

    UPDATE dbo.YeuCauDatMua
    SET TrangThai = @TrangThai,
        PhanHoi = @PhanHoi,
        MaNhanVienXuLy = @MaNhanVien,
        NgayXuLy = SYSDATETIME()
    WHERE MaYeuCau = @MaYeuCau;

    SELECT N'Cap nhat yeu cau dat mua thanh cong.' AS ThongBao;
END;
GO



INSERT INTO dbo.NhanVien
(
    MaNhanVien, Ho, Ten, Phai, NgaySinh, ChucVu, SoDienThoai, Email
)
VALUES
(N'NV001', N'Nguyen', N'An', N'Nam', '1995-05-10',
 N'Thu thu', N'0900000001', N'thuthu@thuvien.edu.vn');
GO

INSERT INTO dbo.TheLoai (MaTheLoai, TenTheLoai)
VALUES
(N'TL001', N'Cong nghe thong tin'),
(N'TL002', N'Kinh te'),
(N'TL003', N'Van hoc'),
(N'TL004', N'Khoa hoc');
GO

INSERT INTO dbo.NhaXuatBan
(
    MaNhaXuatBan, TenNhaXuatBan, DiaChi, SoDienThoai
)
VALUES
(N'NXB001', N'Nha xuat ban Giao duc', N'TP. Ho Chi Minh', N'0280000001'),
(N'NXB002', N'Nha xuat ban Tre', N'TP. Ho Chi Minh', N'0280000002');
GO

INSERT INTO dbo.TacGia (MaTacGia, TenTacGia)
VALUES
(N'TG001', N'Nguyen Van A'),
(N'TG002', N'Tran Thi B'),
(N'TG003', N'Le Van C');
GO

INSERT INTO dbo.DocGia
(
    MaDocGia, Ho, Ten, Email, SoDienThoai, DonVi
)
VALUES
(N'DG001', N'Pham', N'Huy Hoang',
 N'hoang@example.com', N'0900000003', N'Khoa Phat trien phan mem');
GO

INSERT INTO dbo.TheDocGia
(
    MaThe, MaDocGia, NgayCap, HanSuDung, TrangThai
)
VALUES
(N'THE001', N'DG001', CAST(GETDATE() AS DATE),
 DATEADD(YEAR, 1, CAST(GETDATE() AS DATE)), 1);
GO


INSERT INTO dbo.TaiKhoan
(
    MaDocGia, TenDangNhap, MatKhauHash, VaiTro
)
VALUES
(N'DG001', N'hoang', N'$DEMO_HASH_REPLACE_IN_APPLICATION$', N'DocGia');
GO

INSERT INTO dbo.TaiKhoan
(
    MaNhanVien, TenDangNhap, MatKhauHash, VaiTro
)
VALUES
(N'NV001', N'thuthu', N'$DEMO_HASH_REPLACE_IN_APPLICATION$', N'ThuThu');
GO

INSERT INTO dbo.DauSach
(
    MaDauSach, TenSach, MaTheLoai, MaNhaXuatBan,
    NamXuatBan, ISBN, MoTa, LoaiTaiLieu,
    SoLuongTong, SoLuongHienCo
)
VALUES
(
 N'DS001', N'Lap trinh C# co ban', N'TL001', N'NXB001',
 2025, N'978000000001', N'Tai lieu lap trinh C#',
 N'SachThuong', 10, 10
),
(
 N'DS002', N'Co so du lieu SQL Server', N'TL001', N'NXB001',
 2025, N'978000000002', N'Tai lieu SQL Server',
 N'SachThuong', 8, 8
),
(
 N'DS003', N'Nhap mon cong nghe thong tin', N'TL001', N'NXB001',
 2024, N'978000000003', N'Tai lieu CNTT',
 N'SachDienTu', 0, 0
);
GO

INSERT INTO dbo.DauSach_TacGia (MaDauSach, MaTacGia)
VALUES
(N'DS001', N'TG001'),
(N'DS002', N'TG002'),
(N'DS003', N'TG003');
GO

INSERT INTO dbo.SachDienTu
(
    MaDauSach, DuongDanFile, DinhDangFile,
    KichThuocKB, ChoPhepDoc, ChoPhepTai
)
VALUES
(
 N'DS003',
 N'/documents/nhap-mon-cntt.pdf',
 N'PDF',
 5120, 1, 1
);

GO
USE QuanLyThuVienDB;
GO

SELECT * FROM DocGia;
SELECT * FROM TaiKhoan;
SELECT * FROM NhanVien;
SELECT * FROM TheDocGia;
SELECT * FROM TheLoai;
SELECT * FROM NhaXuatBan;
SELECT * FROM TacGia;
SELECT * FROM DauSach;
SELECT * FROM DauSach_TacGia;
SELECT * FROM SachDienTu;
SELECT * FROM PhieuMuon;
SELECT * FROM ChiTietPhieuMuon;
SELECT * FROM PhieuPhat;
SELECT * FROM YeuCauDatMua;
SELECT * FROM ThanhToan;
SELECT * FROM LichSuTruyCapTaiLieu;
SELECT * FROM ThongBaoEmail;
GO
