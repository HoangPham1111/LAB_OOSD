# LAB05 – Quản lý công ty du lịch

## 1. Giới thiệu

LAB05 là ứng dụng desktop quản lý hoạt động của công ty du lịch, xây dựng bằng **C# Windows Forms** trên **.NET Framework 4.7.2**, sử dụng **SQL Server**. Hệ thống tổ chức theo mô hình:

```text
Forms (giao diện) → Services (nghiệp vụ) → Data/Db.cs → SQL Server
```

Ứng dụng quản lý tour, chuyến khách lẻ, khách đoàn, hướng dẫn viên, thanh toán, khảo sát, tính lương và thống kê.

## 2. Yêu cầu môi trường

- Visual Studio 2019/2022 với workload **.NET desktop development**.
- .NET Framework 4.7.2 Developer/Targeting Pack.
- SQL Server, SQL Server Express hoặc LocalDB.
- SSMS để chạy script và kiểm tra dữ liệu.

## 3. Cấu trúc project

```text
LAB05/
├── QuanLyCongTyDuLich.docx       # Tài liệu yêu cầu/thiết kế
├── QuanLyCongTyDuLich.sql        # Script tạo CSDL và dữ liệu mẫu
├── README.md
└── QuanLyCongTyDuLich/
    ├── QuanLyCongTyDuLich.sln
    ├── QuanLyCongTyDuLich.csproj
    ├── App.config
    ├── Program.cs
    ├── Data/Db.cs
    ├── Services/
    │   ├── Models.cs
    │   ├── HeThongService.cs
    │   ├── DanhMucService.cs
    │   ├── TourService.cs
    │   ├── ChuyenLeService.cs
    │   ├── DangKyLeService.cs
    │   ├── DangKyDoanService.cs
    │   ├── PhanCongService.cs
    │   ├── KetThucService.cs
    │   └── ThongKeService.cs
    ├── Forms/                     # Form chính và các màn hình nghiệp vụ
    └── Properties/AssemblyInfo.cs
```

Các thư mục `.vs`, `bin` và `obj` là dữ liệu sinh tự động của Visual Studio.

## 4. Các chức năng

- **Danh mục:** phương tiện, điểm tham quan, điểm bán vé và hướng dẫn viên.
- **Quản lý tour:** thông tin tour, điểm dừng, phương tiện theo chặng và điểm tham quan theo tour.
- **Chuyến khách lẻ:** tạo chuyến, tính ngày về, mở/đóng đăng ký.
- **Đăng ký khách lẻ:** đăng ký theo chuyến, chọn điểm bán vé, tính thành tiền.
- **Đăng ký khách đoàn:** quản lý đoàn, thành viên, bảo hiểm, tiền cọc và hủy đăng ký.
- **Phân công HDV:** phân công cho chuyến lẻ/đoàn và kiểm tra lịch trùng.
- **Kết thúc tour:** thanh toán đoàn, gửi khảo sát và ghi nhận phản hồi.
- **Thống kê:** tính lương/thù lao và tổng hợp dữ liệu theo khoảng ngày.

Các service gồm `DanhMucService`, `TourService`, `ChuyenLeService`, `DangKyLeService`, `DangKyDoanService`, `PhanCongService`, `KetThucService`, `ThongKeService` và `HeThongService`. Form chỉ lấy dữ liệu từ control và gọi Service; câu lệnh SQL được thực hiện qua `Data/Db.cs` bằng tham số.

`FrmMain` gọi `HeThongService` để kiểm tra kết nối bằng `SELECT DB_NAME()`. Khi kết nối lỗi, các nút nghiệp vụ được vô hiệu hóa.

## 5. Cơ sở dữ liệu hiện tại

Script cần chạy là:

```text
D:\2HK2_CNTT2\LAB_OOSD\LAB05\QuanLyCongTyDuLich.sql
```

Script tạo database `QuanLyCongTyDuLich` và 15 bảng:

- **Danh mục:** `Tour`, `PhuongTien`, `DiemThamQuan`, `DiemBanVe`, `HuongDanVien`.
- **Cấu hình tour:** `TourDiemDung`, `TourPhuongTien`, `TourDiemThamQuan`.
- **Đăng ký:** `ChuyenLe`, `DoanKhach`, `DangKyDoan`, `ThanhVienDoan`, `DangKyLe`.
- **Vận hành:** `PhanCongHDV`, `ThanhToanDoan`, `KhaoSat`.

Các bảng sử dụng khóa chính, khóa ngoại và `CHECK` để bảo đảm dữ liệu hợp lệ; script cũng chèn dữ liệu mẫu cho tour, điểm dừng, chuyến, đăng ký, phân công và khảo sát.

### Tạo database

1. Mở SSMS và kết nối đến SQL Server/LocalDB.
2. Mở file SQL, chọn đúng instance rồi nhấn **Execute** hoặc `F5`.
3. Kiểm tra database `QuanLyCongTyDuLich`.

```sql
USE QuanLyCongTyDuLich;
SELECT DB_NAME() AS TenCSDL;
SELECT name FROM sys.tables ORDER BY name;
SELECT * FROM Tour;
```

> **Cảnh báo:** script xóa các bảng cũ trước khi tạo lại. Chạy lại script có thể làm mất dữ liệu hiện tại; hãy sao lưu trước.

## 6. Cấu hình kết nối

Mở file:

```text
D:\2HK2_CNTT2\LAB_OOSD\LAB05\QuanLyCongTyDuLich\App.config
```

Cấu hình hiện tại:

```xml
<connectionStrings>
  <add name="QuanLyCongTyDuLichDB"
       connectionString="Data Source=DGESVRJ;Initial Catalog=QuanLyCongTyDuLich;Integrated Security=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

Thay `Data Source=DGESVRJ` bằng tên SQL Server instance trên máy. `Initial Catalog` phải trùng database và không đổi tên `QuanLyCongTyDuLichDB`, vì `Db.cs` đọc connection string này.

Nếu dùng LocalDB:

```xml
connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QuanLyCongTyDuLich;Integrated Security=True"
```

## 7. Mở và chạy chương trình

1. Mở solution bằng Visual Studio.
2. Chọn project → **Set as Startup Project**.
3. Xác nhận target framework là **.NET Framework 4.7.2**.
4. Chạy script SQL và cập nhật `App.config`.
5. Chọn **Build → Rebuild Solution**.
6. Nhấn `F5` để chạy.
7. Kiểm tra trạng thái kết nối trên `FrmMain` trước khi thao tác.

## 8. Quy tắc nghiệp vụ chính

- Khách lẻ có từ **1 đến 11 người**.
- Khách đoàn có **trên 12 người**; trường hợp đúng 12 người bị chặn theo quyết định nghiệp vụ trong database.
- Ngày về/ngày kết thúc dự kiến = ngày đi + số ngày tour - 1.
- Nếu có khách sạn, hạng sao từ 2 đến 5; nếu không có khách sạn, hạng sao phải là `NULL`.
- Chỉ phân công hướng dẫn viên đang làm việc và không bị trùng lịch.
- Thanh toán đoàn chỉ thực hiện sau ngày kết thúc dự kiến và không vượt số tiền còn lại.
- Khảo sát chỉ gửi sau khi tour kết thúc; điểm đánh giá từ 1 đến 5 và mỗi đăng ký tối đa một phiếu.

## 9. Kiểm thử đề xuất

1. Thêm dữ liệu danh mục và tour; thử trùng mã hoặc thứ tự điểm dừng.
2. Tạo chuyến lẻ; kiểm tra ngày về và trạng thái đóng/mở đăng ký.
3. Đăng ký khách lẻ; thử 12 người và chuyến đã đóng.
4. Đăng ký đoàn; kiểm tra số người, tiền cọc, bảo hiểm, thành viên và hủy đăng ký.
5. Phân công HDV; thử phân công một HDV vào lịch bị trùng.
6. Thanh toán sau tour; thử thanh toán vượt số tiền còn lại.
7. Gửi khảo sát trước khi kết thúc tour hoặc gửi trùng đăng ký.
8. Kiểm tra tính lương và thống kê theo khoảng ngày.

## 10. Ghi chú

- Đây là project WinForms .NET Framework, không chạy bằng `dotnet run`; hãy dùng Visual Studio hoặc MSBuild hỗ trợ .NET Framework.
- File Word là tài liệu tham chiếu; `QuanLyCongTyDuLich.sql` và mã nguồn là phần triển khai thực tế hiện tại.
- Không chỉnh sửa thủ công các thư mục `.vs`, `bin`, `obj`.

Bài thực hành môn Phân tích và thiết kế hướng đối tượng – LAB05.


