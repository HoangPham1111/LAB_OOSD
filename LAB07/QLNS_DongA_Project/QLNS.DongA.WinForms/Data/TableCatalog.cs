using System;
using System.Collections.Generic;
using System.Linq;

namespace QLNS.DongA.Data
{
    internal sealed class TableDefinition
    {
        public string Name { get; private set; }
        public string Label { get; private set; }
        public TableDefinition(string name, string label) { Name = name; Label = label; }
        public override string ToString() { return Label + " (" + Name + ")"; }
    }

    internal static class TableCatalog
    {
        private static readonly List<TableDefinition> Items = new List<TableDefinition>
        {
            new TableDefinition("PhongBan", "Phòng ban"),
            new TableDefinition("NhanVien", "Nhân viên"),
            new TableDefinition("QuyetDinhTuyenDung", "Quyết định tuyển dụng"),
            new TableDefinition("HopDongLaoDong", "Hợp đồng lao động"),
            new TableDefinition("QuyetDinhChucVu", "Quyết định chức vụ"),
            new TableDefinition("QuyetDinhLuong", "Quyết định lương"),
            new TableDefinition("HoSoBaoHiem", "Hồ sơ bảo hiểm"),
            new TableDefinition("KeHoachPhanCong", "Kế hoạch phân công"),
            new TableDefinition("PhanCongCa", "Phân công ca"),
            new TableDefinition("BangChamCong", "Bảng chấm công"),
            new TableDefinition("ChiTietChamCong", "Chi tiết chấm công"),
            new TableDefinition("PhieuPhuTroi", "Phiếu phụ trội"),
            new TableDefinition("ChiTietPhuTroi", "Chi tiết phụ trội"),
            new TableDefinition("BangLuong", "Bảng lương"),
            new TableDefinition("ChiTietBangLuong", "Chi tiết bảng lương"),
            new TableDefinition("QuyetDinhKhenThuong", "Quyết định khen thưởng"),
            new TableDefinition("PhieuChiKhenThuong", "Phiếu chi khen thưởng"),
            new TableDefinition("QuyetDinhKyLuat", "Quyết định kỷ luật"),
            new TableDefinition("PhieuThuNopPhat", "Phiếu thu nộp phạt"),
            new TableDefinition("QDKhenThuong_NhanVien", "Liên kết quyết định thưởng - nhân viên"),
            new TableDefinition("QDKhenThuong_PhongBan", "Liên kết quyết định thưởng - phòng ban"),
            new TableDefinition("QDKyLuat_NhanVien", "Liên kết quyết định kỷ luật - nhân viên"),
            new TableDefinition("QDKyLuat_PhongBan", "Liên kết quyết định kỷ luật - phòng ban")
        };

        public static IList<TableDefinition> All { get { return Items.AsReadOnly(); } }
        public static bool IsAllowed(string tableName) { return Items.Any(x => String.Equals(x.Name, tableName, StringComparison.OrdinalIgnoreCase)); }
        public static string Validate(string tableName)
        {
            TableDefinition item = Items.FirstOrDefault(x => String.Equals(x.Name, tableName, StringComparison.OrdinalIgnoreCase));
            if (item == null) throw new ArgumentException("Bảng không nằm trong danh mục được phép truy cập.", "tableName");
            return item.Name;
        }
    }
}
