using System;

namespace QuanLyThuVien.Models
{
    public class KetQuaXuLy
    {
        public bool ThanhCong { get; set; }

        public string ThongBao { get; set; }

        public static KetQuaXuLy Ok(string thongBao)
        {
            return new KetQuaXuLy
            {
                ThanhCong = true,
                ThongBao = thongBao
            };
        }

        public static KetQuaXuLy Loi(string thongBao)
        {
            return new KetQuaXuLy
            {
                ThanhCong = false,
                ThongBao = thongBao
            };
        }
    }

    public class DocGia
    {
        public string MaDocGia { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
    }

    public class NhanVien
    {
        public string MaNhanVien { get; set; }
        public string HoTen { get; set; }
        public string ChucVu { get; set; }
        public string SoDienThoai { get; set; }
    }

    public class TheLoai
    {
        public string MaTheLoai { get; set; }
        public string TenTheLoai { get; set; }
    }

    public class NhaXuatBan
    {
        public string MaNhaXuatBan { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
    }

    public class DauSach
    {
        public string MaDauSach { get; set; }
        public string TenSach { get; set; }
        public int? NamXuatBan { get; set; }
        public int SoLuongHienCo { get; set; }
        public string MaTheLoai { get; set; }
        public string MaNhaXuatBan { get; set; }
    }

    public class SachDienTu
    {
        public string MaSachDienTu { get; set; }
        public string MaDauSach { get; set; }
        public string DuongDanFile { get; set; }
        public bool ChoDocTrucTuyen { get; set; }
        public bool ChoDownload { get; set; }
    }
}