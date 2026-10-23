/* Du lieu mau de chay demo va test. Co the chay lai; cac khoa mau duoc kiem tra truoc khi them. */
USE QLNS_DongA;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.PhongBan WHERE MaPhongBan = N'PB-TCKT')
    INSERT dbo.PhongBan (MaPhongBan, TenPhongBan, LoaiDonVi, NhuCauTuyenDung, TrangThai) VALUES (N'PB-TCKT', N'Phòng Tài chính - Kế toán', N'PhongBan', 1, 1);
IF NOT EXISTS (SELECT 1 FROM dbo.PhongBan WHERE MaPhongBan = N'PB-KT')
    INSERT dbo.PhongBan (MaPhongBan, TenPhongBan, LoaiDonVi, NhuCauTuyenDung, TrangThai) VALUES (N'PB-KT', N'Phòng Kỹ thuật', N'PhongBan', 0, 1);
IF NOT EXISTS (SELECT 1 FROM dbo.PhongBan WHERE MaPhongBan = N'PB-SX')
    INSERT dbo.PhongBan (MaPhongBan, TenPhongBan, LoaiDonVi, NhuCauTuyenDung, TrangThai) VALUES (N'PB-SX', N'Phòng Sản xuất', N'PhongBan', 1, 1);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.NhanVien WHERE MaNhanVien = N'NV001')
    INSERT dbo.NhanVien (MaNhanVien, HoTen, NgayTiepNhan, TrangThaiNhanVien, MaPhongBan, Email, DienThoai)
    VALUES (N'NV001', N'Nguyễn Văn An', '2024-01-15', N'DangLam', N'PB-TCKT', N'an.demo@example.local', N'0900000001');
IF NOT EXISTS (SELECT 1 FROM dbo.NhanVien WHERE MaNhanVien = N'NV002')
    INSERT dbo.NhanVien (MaNhanVien, HoTen, NgayTiepNhan, TrangThaiNhanVien, MaPhongBan, Email, DienThoai)
    VALUES (N'NV002', N'Trần Thị Bình', '2023-06-01', N'DangLam', N'PB-KT', N'binh.demo@example.local', N'0900000002');
IF NOT EXISTS (SELECT 1 FROM dbo.NhanVien WHERE MaNhanVien = N'NV003')
    INSERT dbo.NhanVien (MaNhanVien, HoTen, NgayTiepNhan, TrangThaiNhanVien, MaPhongBan, Email, DienThoai)
    VALUES (N'NV003', N'Lê Văn Cường', '2026-08-01', N'ThuViec', N'PB-SX', N'cuong.demo@example.local', N'0900000003');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.QuyetDinhTuyenDung WHERE MaQDTuyenDung = N'QDT-SEED03')
    INSERT dbo.QuyetDinhTuyenDung
    (MaQDTuyenDung, MaNhanVien, MaPhongBan, NgayKy, NgayBatDauThuViec, NgayKetThucThuViec, LuongThuViec, DieuKhoanNhaTuyenDung, DieuKhoanUngVien, TrangThai)
    VALUES (N'QDT-SEED03', N'NV003', N'PB-SX', '2026-08-01', '2026-08-01', '2026-08-31', 6000000,
            N'Thử việc theo nội quy công ty.', N'Tuân thủ nội quy và bảo mật thông tin.', N'DaKetThucThuViec');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.HopDongLaoDong WHERE MaHopDong = N'HD-SEED01')
    INSERT dbo.HopDongLaoDong (MaHopDong, MaNhanVien, LoaiHopDong, NgayKy, NgayBatDau, NgayKetThuc, TrangThai)
    VALUES (N'HD-SEED01', N'NV001', N'DaiHan', '2024-01-15', '2024-01-15', NULL, N'HieuLuc');
IF NOT EXISTS (SELECT 1 FROM dbo.HopDongLaoDong WHERE MaHopDong = N'HD-SEED02')
    INSERT dbo.HopDongLaoDong (MaHopDong, MaNhanVien, LoaiHopDong, NgayKy, NgayBatDau, NgayKetThuc, TrangThai)
    VALUES (N'HD-SEED02', N'NV002', N'NganHan', '2025-01-01', '2025-01-01', '2027-01-01', N'HieuLuc');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.QuyetDinhChucVu WHERE MaQDChucVu = N'QDCV-SEED01')
    INSERT dbo.QuyetDinhChucVu (MaQDChucVu, MaNhanVien, NgayKy, TenChucVu, PhuCapChucVu, NgayHieuLuc, NoiDung, TrangThai)
    VALUES (N'QDCV-SEED01', N'NV001', '2026-01-01', N'Chuyên viên', 1000000, '2026-01-01', N'Quyết định chức vụ mẫu.', N'DaKy');
IF NOT EXISTS (SELECT 1 FROM dbo.QuyetDinhChucVu WHERE MaQDChucVu = N'QDCV-SEED02')
    INSERT dbo.QuyetDinhChucVu (MaQDChucVu, MaNhanVien, NgayKy, TenChucVu, PhuCapChucVu, NgayHieuLuc, NoiDung, TrangThai)
    VALUES (N'QDCV-SEED02', N'NV002', '2026-01-01', N'Kỹ thuật viên', 0, '2026-01-01', N'Quyết định chức vụ mẫu.', N'DaKy');
IF NOT EXISTS (SELECT 1 FROM dbo.QuyetDinhLuong WHERE MaQDLuong = N'QDL-SEED01')
    INSERT dbo.QuyetDinhLuong (MaQDLuong, MaNhanVien, NgayKy, LuongCoBan, PhuCapLuong, PhuCapDocHai, NgayHieuLuc, NoiDung, TrangThai)
    VALUES (N'QDL-SEED01', N'NV001', '2026-01-01', 15000000, 500000, 0, '2026-01-01', N'Lương minh họa cho kiểm thử.', N'DaKy');
IF NOT EXISTS (SELECT 1 FROM dbo.QuyetDinhLuong WHERE MaQDLuong = N'QDL-SEED02')
    INSERT dbo.QuyetDinhLuong (MaQDLuong, MaNhanVien, NgayKy, LuongCoBan, PhuCapLuong, PhuCapDocHai, NgayHieuLuc, NoiDung, TrangThai)
    VALUES (N'QDL-SEED02', N'NV002', '2026-01-01', 12000000, 300000, 200000, '2026-01-01', N'Lương minh họa cho kiểm thử.', N'DaKy');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.HoSoBaoHiem WHERE MaHoSoBaoHiem = N'HSB-SEED-BHYT')
    INSERT dbo.HoSoBaoHiem (MaHoSoBaoHiem, MaNhanVien, MaHopDong, LoaiBaoHiem, MucDong, NgayLap, NgayBatDau, TrangThai)
    VALUES (N'HSB-SEED-BHYT', N'NV001', N'HD-SEED01', N'BHYT', 150000, '2026-01-01', '2026-01-01', N'DangThamGia');
IF NOT EXISTS (SELECT 1 FROM dbo.HoSoBaoHiem WHERE MaHoSoBaoHiem = N'HSB-SEED-BHXH')
    INSERT dbo.HoSoBaoHiem (MaHoSoBaoHiem, MaNhanVien, MaHopDong, LoaiBaoHiem, MucDong, NgayLap, NgayBatDau, TrangThai)
    VALUES (N'HSB-SEED-BHXH', N'NV001', N'HD-SEED01', N'BHXH', 1200000, '2026-01-01', '2026-01-01', N'DangThamGia');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.BangChamCong WHERE MaBangChamCong = N'BCC-202609')
    INSERT dbo.BangChamCong (MaBangChamCong, Thang, Nam, NgayLap, TrangThai) VALUES (N'BCC-202609', 9, 2026, '2026-10-01', N'DaXacNhan');
GO

/* Tao 22 ban ghi x 8 gio = 176 gio cho NV001, va 20 ban ghi x 8 gio = 160 gio cho NV002. */
;WITH N AS
(
    SELECT 1 AS n UNION ALL SELECT n + 1 FROM N WHERE n < 22
)
INSERT dbo.ChiTietChamCong (MaCTChamCong, MaBangChamCong, MaNhanVien, Ngay, SoGioCong, TrangThaiCong, GhiChu)
SELECT N'CC1-' + RIGHT(N'00' + CONVERT(NVARCHAR(2), N.n), 2), N'BCC-202609', N'NV001', DATEADD(day, N.n - 1, CONVERT(date, '2026-09-01')), 8, N'DiLam', N'Dữ liệu test minh họa'
FROM N
WHERE NOT EXISTS (SELECT 1 FROM dbo.ChiTietChamCong c WHERE c.MaCTChamCong = N'CC1-' + RIGHT(N'00' + CONVERT(NVARCHAR(2), N.n), 2))
OPTION (MAXRECURSION 30);
GO
;WITH N AS
(
    SELECT 1 AS n UNION ALL SELECT n + 1 FROM N WHERE n < 20
)
INSERT dbo.ChiTietChamCong (MaCTChamCong, MaBangChamCong, MaNhanVien, Ngay, SoGioCong, TrangThaiCong, GhiChu)
SELECT N'CC2-' + RIGHT(N'00' + CONVERT(NVARCHAR(2), N.n), 2), N'BCC-202609', N'NV002', DATEADD(day, N.n - 1, CONVERT(date, '2026-09-01')), 8, N'DiLam', N'Dữ liệu test minh họa'
FROM N
WHERE NOT EXISTS (SELECT 1 FROM dbo.ChiTietChamCong c WHERE c.MaCTChamCong = N'CC2-' + RIGHT(N'00' + CONVERT(NVARCHAR(2), N.n), 2))
OPTION (MAXRECURSION 30);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.PhieuPhuTroi WHERE SoPhieuPhuTroi = N'PPT-SEED01')
    INSERT dbo.PhieuPhuTroi (SoPhieuPhuTroi, NgayPhuTroi, HinhThucPhuTroi, NgayLap, TrangThai)
    VALUES (N'PPT-SEED01', '2026-09-15', N'TangCa', '2026-09-15', N'DaXacNhan');
IF NOT EXISTS (SELECT 1 FROM dbo.ChiTietPhuTroi WHERE MaCTPhuTroi = N'CTPT-SEED01')
    INSERT dbo.ChiTietPhuTroi (MaCTPhuTroi, SoPhieuPhuTroi, MaNhanVien, SoGioPhuTroi, HeSoTinhGio, TrangThaiXacNhan)
    VALUES (N'CTPT-SEED01', N'PPT-SEED01', N'NV001', 10, 1.5, N'DaXacNhan');
IF NOT EXISTS (SELECT 1 FROM dbo.PhieuPhuTroi WHERE SoPhieuPhuTroi = N'PPT-SEED02')
    INSERT dbo.PhieuPhuTroi (SoPhieuPhuTroi, NgayPhuTroi, HinhThucPhuTroi, NgayLap, TrangThai)
    VALUES (N'PPT-SEED02', '2026-09-18', N'NgayLe', '2026-09-18', N'DaXacNhan');
IF NOT EXISTS (SELECT 1 FROM dbo.ChiTietPhuTroi WHERE MaCTPhuTroi = N'CTPT-SEED02')
    INSERT dbo.ChiTietPhuTroi (MaCTPhuTroi, SoPhieuPhuTroi, MaNhanVien, SoGioPhuTroi, HeSoTinhGio, TrangThaiXacNhan)
    VALUES (N'CTPT-SEED02', N'PPT-SEED02', N'NV002', 4, 1.5, N'DaXacNhan');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.QuyetDinhKhenThuong WHERE MaQDKhenThuong = N'QDKT-SEED01')
    INSERT dbo.QuyetDinhKhenThuong (MaQDKhenThuong, NgayLap, NgayKy, LoaiDoiTuongApDung, LyDo, NoiDung, SoTienKhenThuong, TrangThai)
    VALUES (N'QDKT-SEED01', '2026-09-24', '2026-09-25', N'CaNhan', N'Hoàn thành công việc tốt', N'Thưởng minh họa tháng 9', 500000, N'DaKy');
IF NOT EXISTS (SELECT 1 FROM dbo.QDKhenThuong_NhanVien WHERE MaQDKhenThuong = N'QDKT-SEED01' AND MaNhanVien = N'NV001')
    INSERT dbo.QDKhenThuong_NhanVien (MaQDKhenThuong, MaNhanVien) VALUES (N'QDKT-SEED01', N'NV001');
IF NOT EXISTS (SELECT 1 FROM dbo.QuyetDinhKyLuat WHERE MaQDKyLuat = N'QDKL-SEED01')
    INSERT dbo.QuyetDinhKyLuat (MaQDKyLuat, NgayLap, NgayKy, LoaiDoiTuongApDung, LyDo, NoiDung, SoTienPhat, HinhThucXuLy, TrangThai)
    VALUES (N'QDKL-SEED01', '2026-09-19', '2026-09-20', N'CaNhan', N'Vi phạm quy trình nội bộ (test)', N'Khoản phạt minh họa để test khấu trừ', 100000, N'TruLuong', N'DaKy');
IF NOT EXISTS (SELECT 1 FROM dbo.QDKyLuat_NhanVien WHERE MaQDKyLuat = N'QDKL-SEED01' AND MaNhanVien = N'NV001')
    INSERT dbo.QDKyLuat_NhanVien (MaQDKyLuat, MaNhanVien) VALUES (N'QDKL-SEED01', N'NV001');
GO

PRINT N'Da them du lieu mau. Du lieu chi phuc vu demo/kiem thu, khong phai du lieu that cua cong ty.';
GO
