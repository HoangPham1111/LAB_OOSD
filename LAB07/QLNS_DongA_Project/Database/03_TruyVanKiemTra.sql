USE QLNS_DongA;
GO

/* 1. Kiem tra cac bang/chung tu cot loi */
SELECT NhanVien.MaNhanVien, NhanVien.HoTen, NhanVien.TrangThaiNhanVien, PhongBan.TenPhongBan
FROM dbo.NhanVien LEFT JOIN dbo.PhongBan ON PhongBan.MaPhongBan = NhanVien.MaPhongBan
ORDER BY NhanVien.MaNhanVien;
GO

/* 2. Kiem tra tong gio cong va gio phu troi cho thang 09/2026 */
SELECT c.MaNhanVien, SUM(c.SoGioCong) AS TongGioCong
FROM dbo.ChiTietChamCong c
WHERE c.Ngay >= '2026-09-01' AND c.Ngay < '2026-10-01'
GROUP BY c.MaNhanVien
ORDER BY c.MaNhanVien;
GO
SELECT c.MaNhanVien, SUM(c.SoGioPhuTroi) AS TongGioPhuTroiThucTe,
       SUM(c.SoGioPhuTroi * c.HeSoTinhGio) AS TongGioPhuTroiQuyDoi
FROM dbo.ChiTietPhuTroi c
JOIN dbo.PhieuPhuTroi p ON p.SoPhieuPhuTroi = c.SoPhieuPhuTroi
WHERE p.NgayPhuTroi >= '2026-09-01' AND p.NgayPhuTroi < '2026-10-01'
  AND p.TrangThai = N'DaXacNhan' AND c.TrangThaiXacNhan = N'DaXacNhan'
GROUP BY c.MaNhanVien
ORDER BY c.MaNhanVien;
GO

/* 3. Tao/tinh lai bang luong mau (chi sau khi da nap script du lieu mau). */
EXEC dbo.sp_TaoBangLuongThang @Thang = 9, @Nam = 2026, @GioChuanThang = 208;
GO
SELECT * FROM dbo.vw_BangLuongChiTiet WHERE MaBangLuong = N'BL-202609' ORDER BY MaNhanVien;
GO

/* 4. Kiem tra quyet dinh khen thuong, nguoi/tap the nhan va phieu chi */
SELECT q.MaQDKhenThuong, q.LoaiDoiTuongApDung, q.TrangThai, q.SoTienKhenThuong,
       nv.MaNhanVien, nv.HoTen, pb.MaPhongBan, pb.TenPhongBan,
       pc.MaPhieuChi, pc.SoTienChi, pc.TrangThai AS TrangThaiPhieuChi
FROM dbo.QuyetDinhKhenThuong q
LEFT JOIN dbo.QDKhenThuong_NhanVien m ON m.MaQDKhenThuong = q.MaQDKhenThuong
LEFT JOIN dbo.NhanVien nv ON nv.MaNhanVien = m.MaNhanVien
LEFT JOIN dbo.QDKhenThuong_PhongBan g ON g.MaQDKhenThuong = q.MaQDKhenThuong
LEFT JOIN dbo.PhongBan pb ON pb.MaPhongBan = g.MaPhongBan
LEFT JOIN dbo.PhieuChiKhenThuong pc ON pc.MaQDKhenThuong = q.MaQDKhenThuong
ORDER BY q.NgayLap DESC;
GO

/* 5. Cac truy van sau giup quan sat FK/constraint; khong tu dong tao du lieu loi. */
SELECT OBJECT_NAME(f.parent_object_id) AS BangCon, f.name AS TenFK,
       OBJECT_NAME(f.referenced_object_id) AS BangCha
FROM sys.foreign_keys f
WHERE f.is_disabled = 0
ORDER BY BangCon, TenFK;
GO
