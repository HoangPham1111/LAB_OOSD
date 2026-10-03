# LAB03 - HỆ THỐNG QUẢN LÝ KHÁCH SẠN
Báo cáo bao gồm 
1 Database SQLserver
1 file word chứa hình ảnh chạy database và giao diện
1 project chạy trên visual 22

## 1. Giới thiệu

LAB03 là bài thực hành xây dựng **Hệ thống quản lý khách sạn** bằng C# Windows Forms.

Hệ thống hỗ trợ các chức năng quản lý:

- Quản lý khu vực, nhân viên, loại tiện nghi, dịch vụ và quy định đền bù.
- Quản lý phòng và tiện nghi trong phòng.
- Lập và quản lý phiếu đặt phòng.
- Quản lý khách hàng và người lưu trú.
- Quản lý sử dụng dịch vụ.
- Trả phòng, lập phiếu đền bù và hóa đơn.
- Thanh toán hóa đơn.
- Thống kê doanh thu và dịch vụ.

## 2. Công nghệ sử dụng

- Ngôn ngữ: C#
- Giao diện: Windows Forms
- Framework: .NET Framework 4.7.2
- IDE: Visual Studio 2022
- Cơ sở dữ liệu: SQL Server
- SQL Server LocalDB: `(localdb)\MSSQLLocalDB`
- Kiến trúc: UI → Services → Data → SQL Server

## 3. Cấu trúc project

```text
LAB03
│
├── 1250080062_PhamHuyHoang_lab03.docx
├── QuanLyKhachSan.sql
├── README.md
│
└── QuanLyKhachSan
    │
    ├── App.config
    ├── QuanLyKhachSan.csproj
    ├── Program.cs
    │
    ├── Data
    │   └── Db.cs
    │
    ├── Forms
    │   ├── FrmMain.cs
    │   ├── FrmDanhMuc.cs
    │   ├── FrmPhongTienNghi.cs
    │   ├── FrmDatPhong.cs
    │   ├── FrmDichVu.cs
    │   ├── FrmTraPhong.cs
    │   └── FrmThongKe.cs
    │
    ├── Models
    │   ├── DenBuItem.cs
    │   ├── KetQuaXuLy.cs
    │   └── PhongDatItem.cs
    │
    ├── Services
    │   ├── DanhMucService.cs
    │   ├── DatPhongService.cs
    │   ├── DichVuService.cs
    │   ├── PhongTienNghiService.cs
    │   ├── ThongKeService.cs
    │   └── TraPhongService.cs
    │
    └── Properties
