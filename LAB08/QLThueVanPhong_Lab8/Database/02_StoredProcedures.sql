USE QuanLyChoThueVanPhong;
GO
/* Giao dịch 1: HD + các văn phòng + hóa đơn trả trước trong MỘT transaction. */
CREATE OR ALTER PROCEDURE dbo.usp_TaoHopDong
 @MaKhach int, @MaQL int, @NgayBatDau date, @SoThangGoc int,
 @VanPhongs dbo.DanhSachVanPhongThue READONLY, @MaHopDongMoi int OUTPUT
AS
BEGIN
 SET NOCOUNT ON; SET XACT_ABORT ON;
 IF @SoThangGoc < 6 OR @SoThangGoc IS NULL THROW 51001,N'Thời gian thuê tối thiểu 6 tháng.',1;
 IF DAY(@NgayBatDau) <> 1 THROW 51002,N'Bản triển khai yêu cầu bắt đầu thuê ngày 01 để thống nhất hạn đầu tháng.',1;
 IF NOT EXISTS(SELECT 1 FROM @VanPhongs) THROW 51003,N'Phải chọn ít nhất một văn phòng.',1;
 DECLARE @NgayKT date=DATEADD(month,@SoThangGoc,@NgayBatDau), @SoPhong int;
 BEGIN TRY
  SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
  BEGIN TRAN;
  SELECT @SoPhong=COUNT(*) FROM dbo.VanPhong WITH(UPDLOCK,HOLDLOCK)
  WHERE MaVanPhong IN (SELECT MaVanPhong FROM @VanPhongs) AND TrangThai='SAN_SANG';
  IF @SoPhong <> (SELECT COUNT(*) FROM @VanPhongs)
    THROW 51004,N'Văn phòng không tồn tại hoặc đang bảo trì.',1;
  IF EXISTS (
    SELECT 1 FROM dbo.ChiTietHopDong c WITH(UPDLOCK,HOLDLOCK)
    INNER JOIN dbo.HopDong h ON h.MaHopDong=c.MaHopDong
    INNER JOIN @VanPhongs v ON v.MaVanPhong=c.MaVanPhong
    WHERE h.TrangThai IN('CHO_TRA_TRUOC','HIEU_LUC')
      AND c.NgayBatDau < @NgayKT AND c.NgayKetThucHienTai > @NgayBatDau)
    THROW 51005,N'Văn phòng đã có hợp đồng trùng thời gian thuê.',1;
  INSERT dbo.HopDong(MaKhach,MaQL,NgayBatDau,SoThangGoc)
  VALUES(@MaKhach,@MaQL,@NgayBatDau,@SoThangGoc);
  SET @MaHopDongMoi=CONVERT(int,SCOPE_IDENTITY());
  INSERT dbo.ChiTietHopDong(MaHopDong,MaVanPhong,DonGiaThangChot,NgayBatDau,NgayKetThucGoc,NgayKetThucHienTai)
  SELECT @MaHopDongMoi,p.MaVanPhong,p.DonGiaThang,@NgayBatDau,@NgayKT,@NgayKT
  FROM dbo.VanPhong p JOIN @VanPhongs v ON v.MaVanPhong=p.MaVanPhong;
  DECLARE @SoHoaDon int;
  INSERT dbo.HoaDon(MaHopDong,LoaiHoaDon,KyThang,KyNam,NgayLap,NgayDenHan,TongTien)
  SELECT @MaHopDongMoi,'TRA_TRUOC',0,0,CONVERT(date,GETDATE()),@NgayBatDau,
         CONVERT(decimal(18,2),6 * SUM(DonGiaThangChot))
  FROM dbo.ChiTietHopDong WHERE MaHopDong=@MaHopDongMoi;
  SET @SoHoaDon=CONVERT(int,SCOPE_IDENTITY());
  INSERT dbo.ChiTietHoaDon(MaHoaDon,MaCTHD,DienGiai,SoLuong,DonGia)
  SELECT @SoHoaDon,MaCTHD,N'Trả trước 6 tháng đầu - VP '+CAST(MaVanPhong AS nvarchar(20)),6,DonGiaThangChot
  FROM dbo.ChiTietHopDong WHERE MaHopDong=@MaHopDongMoi;
  COMMIT;
 END TRY
 BEGIN CATCH
  IF @@TRANCOUNT>0 ROLLBACK;
  THROW;
 END CATCH
END;
GO
/* Giao dịch 2: nhiều khoản trả vào một hóa đơn, có khóa tránh thanh toán vượt.
   Thời điểm trả trước cho phép chậm nhưng hệ thống CHƯA cho hợp đồng hiệu lực khi chưa trả đủ. */
CREATE OR ALTER PROCEDURE dbo.usp_ThanhToanHoaDon
 @MaHoaDon int,@SoTien decimal(18,2),@PhuongThuc varchar(20),
 @MaGiaoDich varchar(60)=NULL,@NgayThanhToan datetime2(0)=NULL
AS
BEGIN
 SET NOCOUNT ON;SET XACT_ABORT ON;
 IF @SoTien<=0 OR @SoTien IS NULL THROW 51010,N'Số tiền thanh toán phải lớn hơn 0.',1;
 IF @PhuongThuc NOT IN ('TIEN_MAT','CHUYEN_KHOAN','THE') THROW 51011,N'Phương thức thanh toán không hợp lệ.',1;
 BEGIN TRY
  BEGIN TRAN;
  DECLARE @Tong decimal(18,2),@DaThu decimal(18,2),@Loai varchar(20),@MaHD int,@TinhTrang varchar(20);
  SELECT @Tong=TongTien,@Loai=LoaiHoaDon,@MaHD=MaHopDong,@TinhTrang=TrangThai
  FROM dbo.HoaDon WITH(UPDLOCK,HOLDLOCK) WHERE MaHoaDon=@MaHoaDon;
  IF @MaHD IS NULL THROW 51012,N'Hóa đơn không tồn tại.',1;
  SELECT @DaThu=ISNULL(SUM(SoTien),0) FROM dbo.ThanhToan WITH(UPDLOCK,HOLDLOCK) WHERE MaHoaDon=@MaHoaDon;
  IF @TinhTrang='DA_THANH_TOAN' OR @SoTien > @Tong-@DaThu
    THROW 51013,N'Hóa đơn đã trả đủ hoặc số tiền vượt công nợ còn lại.',1;
  INSERT dbo.ThanhToan(MaHoaDon,SoTien,PhuongThuc,MaGiaoDich,NgayThanhToan)
  VALUES(@MaHoaDon,@SoTien,@PhuongThuc,@MaGiaoDich,COALESCE(@NgayThanhToan,SYSDATETIME()));
  IF @DaThu+@SoTien=@Tong
  BEGIN
   UPDATE dbo.HoaDon SET TrangThai='DA_THANH_TOAN' WHERE MaHoaDon=@MaHoaDon;
   IF @Loai='TRA_TRUOC'
      UPDATE dbo.HopDong SET TrangThai='HIEU_LUC' WHERE MaHopDong=@MaHD AND TrangThai='CHO_TRA_TRUOC';
  END
  COMMIT;
 END TRY
 BEGIN CATCH IF @@TRANCOUNT>0 ROLLBACK; THROW; END CATCH
END;
GO
/* Kỳ 7 trở đi: do ngày bắt đầu là 01, NgayDenHan = ngày 01 của tháng tương ứng.
   Chỉ lập kỳ đã tới hạn, không lập trùng hoặc tính phòng hết hạn. */
CREATE OR ALTER PROCEDURE dbo.usp_LapHoaDonTienThue
 @MaHopDong int,@KySo int,@NgayTinh date=NULL,@MaHoaDonMoi int OUTPUT
AS
BEGIN
 SET NOCOUNT ON;SET XACT_ABORT ON;
 IF @KySo<7 THROW 51020,N'Kỳ 1-6 đã được thanh toán trước; kỳ định kỳ phải từ kỳ 7.',1;
 DECLARE @BatDau date, @DenHan date;
 SELECT @BatDau=NgayBatDau FROM dbo.HopDong WHERE MaHopDong=@MaHopDong AND TrangThai='HIEU_LUC';
 IF @BatDau IS NULL THROW 51021,N'Hợp đồng chưa có hiệu lực.',1;
 SET @DenHan=DATEADD(month,@KySo-1,@BatDau);
 IF @DenHan>COALESCE(@NgayTinh,CONVERT(date,GETDATE())) THROW 51022,N'Chưa đến ngày 01 của kỳ thuê.',1;
 BEGIN TRY
  BEGIN TRAN;
  IF EXISTS(SELECT 1 FROM dbo.HoaDon WITH(UPDLOCK,HOLDLOCK)
    WHERE MaHopDong=@MaHopDong AND LoaiHoaDon='TIEN_THUE'
          AND KyThang=MONTH(@DenHan) AND KyNam=YEAR(@DenHan))
     THROW 51023,N'Hóa đơn tiền thuê kỳ này đã được lập.',1;
  IF NOT EXISTS(SELECT 1 FROM dbo.ChiTietHopDong WHERE MaHopDong=@MaHopDong
       AND NgayBatDau<=@DenHan AND NgayKetThucHienTai>@DenHan)
    THROW 51024,N'Không có văn phòng nào còn hiệu lực trong kỳ.',1;
  INSERT dbo.HoaDon(MaHopDong,LoaiHoaDon,KyThang,KyNam,NgayLap,NgayDenHan,TongTien)
  SELECT @MaHopDong,'TIEN_THUE',MONTH(@DenHan),YEAR(@DenHan),
     COALESCE(@NgayTinh,CONVERT(date,GETDATE())),@DenHan,SUM(DonGiaThangChot)
  FROM dbo.ChiTietHopDong WHERE MaHopDong=@MaHopDong
       AND NgayBatDau<=@DenHan AND NgayKetThucHienTai>@DenHan;
  SET @MaHoaDonMoi=CONVERT(int,SCOPE_IDENTITY());
  INSERT dbo.ChiTietHoaDon(MaHoaDon,MaCTHD,DienGiai,SoLuong,DonGia)
  SELECT @MaHoaDonMoi,MaCTHD,N'Tiền thuê VP '+CAST(MaVanPhong AS nvarchar(20)) +N' - kỳ '+CAST(@KySo AS nvarchar(10)),1,DonGiaThangChot
  FROM dbo.ChiTietHopDong WHERE MaHopDong=@MaHopDong AND NgayBatDau<=@DenHan AND NgayKetThucHienTai>@DenHan;
  COMMIT;
 END TRY BEGIN CATCH IF @@TRANCOUNT>0 ROLLBACK; THROW; END CATCH
END;
GO
/* Sử dụng dịch vụ - cộng dồn theo VP, DV, ngày; không cho sửa dữ liệu đã chốt hóa đơn. */
CREATE OR ALTER PROCEDURE dbo.usp_GhiNhanDichVu
 @MaCTHD int,@MaDichVu int,@NgaySuDung date,@SoLuong decimal(12,2)
AS
BEGIN
 SET NOCOUNT ON;SET XACT_ABORT ON;
 IF @SoLuong<=0 THROW 51030,N'Số lượng dịch vụ phải dương.',1;
 BEGIN TRY
  BEGIN TRAN;
  DECLARE @MaHopDong int,@BatDau date,@KetThuc date,@Gia decimal(18,2),@GiaCu decimal(18,2);
  SELECT @MaHopDong=c.MaHopDong,@BatDau=c.NgayBatDau,@KetThuc=c.NgayKetThucHienTai
  FROM dbo.ChiTietHopDong c WITH(UPDLOCK,HOLDLOCK)
  JOIN dbo.HopDong h ON h.MaHopDong=c.MaHopDong AND h.TrangThai='HIEU_LUC'
  WHERE c.MaCTHD=@MaCTHD;
  IF @MaHopDong IS NULL OR @NgaySuDung<@BatDau OR @NgaySuDung>=@KetThuc
    THROW 51031,N'Ngày sử dụng nằm ngoài thời gian thuê có hiệu lực.',1;
  IF EXISTS (SELECT 1 FROM dbo.HoaDon WITH(UPDLOCK,HOLDLOCK)
    WHERE MaHopDong=@MaHopDong AND LoaiHoaDon='DICH_VU'
      AND KyThang=MONTH(@NgaySuDung) AND KyNam=YEAR(@NgaySuDung))
    THROW 51032,N'Hóa đơn dịch vụ tháng này đã chốt, không thể thêm phát sinh.',1;
  SELECT @Gia=DonGia FROM dbo.DichVu WHERE MaDichVu=@MaDichVu;
  IF @Gia IS NULL THROW 51033,N'Dịch vụ không tồn tại.',1;
  SELECT @GiaCu=DonGiaChot FROM dbo.SuDungDichVu WITH(UPDLOCK,HOLDLOCK)
  WHERE MaCTHD=@MaCTHD AND MaDichVu=@MaDichVu AND NgaySuDung=@NgaySuDung;
  IF @GiaCu IS NULL
   INSERT dbo.SuDungDichVu(MaCTHD,MaDichVu,NgaySuDung,SoLuong,DonGiaChot)
   VALUES(@MaCTHD,@MaDichVu,@NgaySuDung,@SoLuong,@Gia);
  ELSE
   UPDATE dbo.SuDungDichVu SET SoLuong=SoLuong+@SoLuong
   WHERE MaCTHD=@MaCTHD AND MaDichVu=@MaDichVu AND NgaySuDung=@NgaySuDung;
  COMMIT;
 END TRY BEGIN CATCH IF @@TRANCOUNT>0 ROLLBACK; THROW; END CATCH
END;
GO
/* Chốt dịch vụ vào cuối tháng, theo EOMONTH. Không phát sinh => không cần hóa đơn. */
CREATE OR ALTER PROCEDURE dbo.usp_LapHoaDonDichVu
 @MaHopDong int,@Thang int,@Nam int,@NgayTinh date=NULL,@MaHoaDonMoi int OUTPUT
AS
BEGIN
 SET NOCOUNT ON; SET XACT_ABORT ON;
 IF @Thang NOT BETWEEN 1 AND 12 OR @Nam NOT BETWEEN 2000 AND 2100
    THROW 51040,N'Tháng/năm lập hóa đơn không hợp lệ.',1;
 DECLARE @Dau date=DATEFROMPARTS(@Nam,@Thang,1),@Cuoi date=EOMONTH(DATEFROMPARTS(@Nam,@Thang,1));
 IF COALESCE(@NgayTinh,CONVERT(date,GETDATE()))<@Cuoi
    THROW 51041,N'Dịch vụ được lập hóa đơn vào cuối tháng.',1;
 BEGIN TRY
  BEGIN TRAN;
  IF EXISTS(SELECT 1 FROM dbo.HoaDon WITH(UPDLOCK,HOLDLOCK) WHERE MaHopDong=@MaHopDong AND LoaiHoaDon='DICH_VU' AND KyThang=@Thang AND KyNam=@Nam)
   THROW 51042,N'Hóa đơn dịch vụ tháng này đã có.',1;
  IF NOT EXISTS(SELECT 1 FROM dbo.SuDungDichVu s JOIN dbo.ChiTietHopDong c ON c.MaCTHD=s.MaCTHD
    WHERE c.MaHopDong=@MaHopDong AND s.NgaySuDung>=@Dau AND s.NgaySuDung<DATEADD(day,1,@Cuoi))
   THROW 51043,N'Tháng này không phát sinh dịch vụ.',1;
  INSERT dbo.HoaDon(MaHopDong,LoaiHoaDon,KyThang,KyNam,NgayLap,NgayDenHan,TongTien)
  SELECT @MaHopDong,'DICH_VU',@Thang,@Nam,COALESCE(@NgayTinh,CONVERT(date,GETDATE())),@Cuoi,SUM(s.ThanhTien)
  FROM dbo.SuDungDichVu s JOIN dbo.ChiTietHopDong c ON c.MaCTHD=s.MaCTHD
  WHERE c.MaHopDong=@MaHopDong AND s.NgaySuDung>=@Dau AND s.NgaySuDung<DATEADD(day,1,@Cuoi);
  SET @MaHoaDonMoi=CONVERT(int,SCOPE_IDENTITY());
  INSERT dbo.ChiTietHoaDon(MaHoaDon,MaCTHD,MaSuDung,DienGiai,SoLuong,DonGia)
  SELECT @MaHoaDonMoi,s.MaCTHD,s.MaSuDung,N'Dịch vụ '+dv.TenDichVu+N' - VP '+CONVERT(nvarchar(20),c.MaVanPhong),s.SoLuong,s.DonGiaChot
  FROM dbo.SuDungDichVu s JOIN dbo.ChiTietHopDong c ON c.MaCTHD=s.MaCTHD
  JOIN dbo.DichVu dv ON dv.MaDichVu=s.MaDichVu
  WHERE c.MaHopDong=@MaHopDong AND s.NgaySuDung>=@Dau AND s.NgaySuDung<DATEADD(day,1,@Cuoi);
  COMMIT;
 END TRY BEGIN CATCH IF @@TRANCOUNT>0 ROLLBACK; THROW; END CATCH
END;
GO
/* Transaction gia hạn: một văn phòng + phiếu + cập nhật hạn.
   Tổng số tháng gia hạn MỌI LẦN tối đa FLOOR(SoThangGoc/2),
   và phải hết nợ cả tiền thuê lẫn DV; còn kỳ đã tới hạn chưa lập HĐ cũng không được bỏ qua. */
CREATE OR ALTER PROCEDURE dbo.usp_LapPhieuGiaHan
 @MaCTHD int,@SoThangGiaHan int,@NgayKiemTra date=NULL,@MaGiaHanMoi int OUTPUT
AS
BEGIN
 SET NOCOUNT ON; SET XACT_ABORT ON;
 IF @SoThangGiaHan<=0 THROW 51050,N'Số tháng gia hạn phải lớn hơn 0.',1;
 SET @NgayKiemTra=COALESCE(@NgayKiemTra,CONVERT(date,GETDATE()));
 BEGIN TRY
  SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
  BEGIN TRAN;
  DECLARE @MaHD int,@MaVP int,@NgayBatDau date,@NgayCu date,@NgayMoi date,@SoThangGoc int,
          @DaGiaHan int,@TrangThai varchar(25),@Ky int,@KyDate date;
  SELECT @MaHD=c.MaHopDong,@MaVP=c.MaVanPhong,@NgayBatDau=c.NgayBatDau,
         @NgayCu=c.NgayKetThucHienTai,@SoThangGoc=h.SoThangGoc,@TrangThai=h.TrangThai
  FROM dbo.ChiTietHopDong c WITH(UPDLOCK,HOLDLOCK)
  JOIN dbo.HopDong h WITH(UPDLOCK,HOLDLOCK) ON h.MaHopDong=c.MaHopDong
  WHERE c.MaCTHD=@MaCTHD;
  IF @MaHD IS NULL OR @TrangThai<>'HIEU_LUC' THROW 51051,N'Hợp đồng chưa có hiệu lực hoặc không tồn tại.',1;
  SELECT @DaGiaHan=ISNULL(SUM(SoThangGiaHan),0) FROM dbo.PhieuGiaHan WITH(UPDLOCK,HOLDLOCK) WHERE MaCTHD=@MaCTHD;
  IF @DaGiaHan + @SoThangGiaHan > @SoThangGoc/2
   THROW 51052,N'Tổng thời gian gia hạn vượt một nửa thời gian thuê gốc.',1;
  IF EXISTS (SELECT 1 FROM dbo.HoaDon hd WITH(UPDLOCK,HOLDLOCK)
    WHERE hd.MaHopDong=@MaHD AND (hd.TongTien >
       (SELECT ISNULL(SUM(t.SoTien),0) FROM dbo.ThanhToan t WHERE t.MaHoaDon=hd.MaHoaDon)))
   THROW 51053,N'Còn nợ tiền thuê hoặc dịch vụ, không được gia hạn.',1;
  /* Ngăn bỏ qua khoản thuê đã tới hạn nhưng chưa phát hành hóa đơn */
  SET @Ky=7;
  WHILE @Ky<=DATEDIFF(month,@NgayBatDau,@NgayKiemTra)+1
  BEGIN
    SET @KyDate=DATEADD(month,@Ky-1,@NgayBatDau);
    IF @KyDate<=@NgayKiemTra AND EXISTS(SELECT 1 FROM dbo.ChiTietHopDong WHERE MaHopDong=@MaHD
         AND NgayBatDau<=@KyDate AND NgayKetThucHienTai>@KyDate)
      AND NOT EXISTS(SELECT 1 FROM dbo.HoaDon WHERE MaHopDong=@MaHD
       AND LoaiHoaDon='TIEN_THUE' AND KyThang=MONTH(@KyDate) AND KyNam=YEAR(@KyDate))
       THROW 51054,N'Có kỳ tiền thuê đã tới hạn nhưng chưa lập hóa đơn.',1;
    SET @Ky=@Ky+1;
  END;
  /* Dịch vụ ở các tháng đã kết thúc phải được lập HĐ và trả đủ */
  IF EXISTS (SELECT 1 FROM dbo.SuDungDichVu s
    JOIN dbo.ChiTietHopDong c ON c.MaCTHD=s.MaCTHD
    WHERE c.MaHopDong=@MaHD AND EOMONTH(s.NgaySuDung)<=@NgayKiemTra
      AND NOT EXISTS (SELECT 1 FROM dbo.ChiTietHoaDon ct JOIN dbo.HoaDon hd ON hd.MaHoaDon=ct.MaHoaDon
         WHERE ct.MaSuDung=s.MaSuDung AND hd.LoaiHoaDon='DICH_VU'))
   THROW 51055,N'Có dịch vụ đến hạn chưa xuất hóa đơn.',1;
  SET @NgayMoi=DATEADD(month,@SoThangGiaHan,@NgayCu);
  /* Khóa phòng để bảo vệ phần gia hạn trước giao dịch lập hợp đồng khác */
  SELECT @MaVP=MaVanPhong FROM dbo.VanPhong WITH(UPDLOCK,HOLDLOCK) WHERE MaVanPhong=@MaVP;
  IF EXISTS(SELECT 1 FROM dbo.ChiTietHopDong c WITH(UPDLOCK,HOLDLOCK)
    JOIN dbo.HopDong h ON c.MaHopDong=h.MaHopDong
    WHERE c.MaVanPhong=@MaVP AND c.MaCTHD<>@MaCTHD AND h.TrangThai IN('CHO_TRA_TRUOC','HIEU_LUC')
      AND c.NgayBatDau<@NgayMoi AND c.NgayKetThucHienTai>@NgayCu)
   THROW 51056,N'Gia hạn bị trùng lịch với hợp đồng của khách khác.',1;
  INSERT dbo.PhieuGiaHan(MaCTHD,NgayLap,SoThangGiaHan,NgayKetThucCu,NgayKetThucMoi)
  VALUES(@MaCTHD,@NgayKiemTra,@SoThangGiaHan,@NgayCu,@NgayMoi);
  SET @MaGiaHanMoi=CONVERT(int,SCOPE_IDENTITY());
  UPDATE dbo.ChiTietHopDong SET NgayKetThucHienTai=@NgayMoi WHERE MaCTHD=@MaCTHD;
  COMMIT;
 END TRY BEGIN CATCH IF @@TRANCOUNT>0 ROLLBACK; THROW; END CATCH
END;
GO
/* Ghi kiểm tra thường niên: được ghi nhiều lần để bảo toàn lịch sử; cảnh báo nếu > 1 năm.
   Tiền phạt là số đã xác nhận theo quy định riêng, không suy ra tỷ lệ tùy tiện. */
CREATE OR ALTER PROCEDURE dbo.usp_GhiKiemTraPhong
 @MaVanPhong int,@MaQL int,@NgayKiemTra date,@TinhTrang nvarchar(200),
 @TuYSuaKetCau bit,@TienPhat decimal(18,2),@GhiChu nvarchar(400)=NULL
AS
BEGIN
 SET NOCOUNT ON;SET XACT_ABORT ON;
 IF @TuYSuaKetCau=0 AND @TienPhat<>0 THROW 51060,N'Không có vi phạm thì tiền phạt phải bằng 0.',1;
 IF @TienPhat<0 THROW 51061,N'Tiền phạt không được âm.',1;
 BEGIN TRY
  BEGIN TRAN;
  INSERT dbo.KiemTraPhong(MaVanPhong,MaQL,NgayKiemTra,TinhTrang,TuYSuaKetCau,TienPhat,GhiChu)
  VALUES(@MaVanPhong,@MaQL,@NgayKiemTra,@TinhTrang,@TuYSuaKetCau,@TienPhat,@GhiChu);
  IF @TinhTrang LIKE N'%bảo trì%'
      UPDATE dbo.VanPhong SET TrangThai='BAO_TRI' WHERE MaVanPhong=@MaVanPhong;
  COMMIT;
 END TRY BEGIN CATCH IF @@TRANCOUNT>0 ROLLBACK; THROW; END CATCH
END;
GO
/* Bảo đảm nhân viên được ghi nhận đúng đơn vị thuê và đúng văn phòng đang thuê. */
CREATE OR ALTER PROCEDURE dbo.usp_ThemNhanVienLamViec
 @MaKhach int,@MaVanPhong int,@HoTen nvarchar(120),@ChucVu nvarchar(100),
 @CCCD varchar(20),@DuongDanAnh nvarchar(400)=NULL
AS
BEGIN
 SET NOCOUNT ON;SET XACT_ABORT ON;
 IF NULLIF(LTRIM(RTRIM(@HoTen)),N'') IS NULL OR NULLIF(LTRIM(RTRIM(@CCCD)),'') IS NULL
  THROW 51070,N'Tên và CCCD không được để trống.',1;
 BEGIN TRY
  BEGIN TRAN;
  IF NOT EXISTS(SELECT 1 FROM dbo.ChiTietHopDong ct
    JOIN dbo.HopDong h ON h.MaHopDong=ct.MaHopDong
    WHERE h.MaKhach=@MaKhach AND ct.MaVanPhong=@MaVanPhong AND h.TrangThai='HIEU_LUC'
      AND ct.NgayBatDau<=CONVERT(date,GETDATE()) AND ct.NgayKetThucHienTai>CONVERT(date,GETDATE()))
   THROW 51071,N'Công ty này không có hợp đồng thuê hiệu lực ở văn phòng đã chọn.',1;
  INSERT dbo.NhanVienLamViec(MaKhach,MaVanPhong,HoTen,ChucVu,CCCD,DuongDanAnh)
  VALUES(@MaKhach,@MaVanPhong,@HoTen,@ChucVu,@CCCD,@DuongDanAnh);
  COMMIT;
 END TRY BEGIN CATCH IF @@TRANCOUNT>0 ROLLBACK; THROW; END CATCH
END;
GO
PRINT N'02 - Đã tạo các stored procedures giao dịch nghiệp vụ.';
