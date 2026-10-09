# HƯỚNG DẪN CHẠY ĐỒ ÁN QUẢN LÝ CÔNG TY DU LỊCH

Bộ project này được dựng theo tài liệu `Bai_6_Quan_ly_cong_ty_du_lich.docx`: 9 Form, 8 Service nghiệp vụ, lớp `Db` truy cập SQL Server, các bảng/khóa/ràng buộc của script trong tài liệu. Các file `*.Designer.cs` được tạo bổ sung dựa trên danh sách control và sự kiện đã nêu ở phần thiết kế giao diện để không cần kéo từng control bằng tay.

## 1. Yêu cầu

- Visual Studio 2019/2022 có workload **.NET desktop development**.
- .NET Framework 4.7.2 Developer Pack/Targeting Pack.
- SQL Server LocalDB (có sẵn trong nhiều bản cài Visual Studio) hoặc một SQL Server instance.
- SSMS (khuyến nghị) để chạy và kiểm tra script SQL.

## 2. Cấu trúc thư mục

- `Database/QuanLyCongTyDuLich.sql`: tạo database, bảng, ràng buộc, index và dữ liệu mẫu lấy từ file Word.
- `Data/Db.cs`: mở kết nối, Query/Execute/Scalar và tạo tham số SQL.
- `Services/Models.cs`: `KetQuaXuLy`, `ThanhVienDoanItem`, `QuyDinh`.
- `Services/*Service.cs`: 8 service nghiệp vụ theo tài liệu và `HeThongService.cs` kiểm tra kết nối ban đầu.
- `Forms/*.cs`: xử lý sự kiện UI, gọi Service, hiển thị kết quả.
- `Forms/*.Designer.cs`: khai báo control, bố cục và nối sự kiện cho cả 9 Form.
- `App.config`: chuỗi kết nối SQL Server.

## 3. Tạo database

1. Mở SSMS và kết nối tới `(localdb)\MSSQLLocalDB` (hoặc instance bạn đang dùng).
2. Mở `Database/QuanLyCongTyDuLich.sql`.
3. Nhấn **Execute** (F5).
4. Làm mới Databases và xác nhận có `QuanLyCongTyDuLich`.
5. Có thể xác nhận nhanh bằng:

```sql
USE QuanLyCongTyDuLich;
SELECT DB_NAME() AS TenCSDL;
SELECT name FROM sys.tables ORDER BY name;
SELECT * FROM Tour;
SELECT * FROM DangKyDoan;
```

**Cảnh báo dữ liệu:** script trong tài liệu có lệnh `DROP TABLE` rồi tạo lại các bảng. Chạy lại sẽ xóa dữ liệu hiện tại trong các bảng đó. Sao lưu trước khi chạy trên dữ liệu cần giữ.

## 4. Cấu hình kết nối

Mở `App.config`. Mặc định là LocalDB:

```xml
<connectionStrings>
  <add name="QuanLyCongTyDuLichDB"
       connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QuanLyCongTyDuLich;Integrated Security=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

Nếu dùng SQL Server Express, thay Data Source ví dụ thành `Data Source=.\SQLEXPRESS`. Nếu dùng server khác, thay bằng đúng tên instance trong SSMS. Không thay tên `Initial Catalog` nếu database vẫn là `QuanLyCongTyDuLich`.

Đảm bảo đã có reference `System.Configuration` (project đã khai báo). Tầng dữ liệu lấy connection string theo tên `QuanLyCongTyDuLichDB`, nên tên này phải khớp chính xác.

## 5. Mở và chạy project

1. Giải nén ZIP ra một thư mục không bị Windows chặn quyền ghi.
2. Mở `QuanLyCongTyDuLich.sln`.
3. Trong Solution Explorer, nhấp phải project → **Set as Startup Project**.
4. Mở project Properties → Application và xác nhận Target framework là **.NET Framework 4.7.2**.
5. Chọn **Build → Rebuild Solution**.
6. Nhấn **F5**. Form chính kiểm tra kết nối lúc khởi động. Nếu kết nối lỗi, các nút nghiệp vụ bị vô hiệu hóa và thông báo hiện chi tiết lỗi cấu hình.

## 6. Luồng kiến trúc

`Form (UI) → Service (quy tắc nghiệp vụ) → Data/Db.cs → SQL Server`

Không viết SQL trong sự kiện Button của Form. Form chỉ đọc giá trị control, gọi Service và dùng `FormHelper.Bao(...)` để hiển thị kết quả. Các truy vấn trong Service dùng tham số SQL `@...` thay vì nối chuỗi dữ liệu người dùng.

## 7. Các Form

1. `FrmMain`: điều hướng và báo trạng thái database.
2. `FrmDanhMuc`: phương tiện, điểm bán vé, hướng dẫn viên, điểm tham quan.
3. `FrmTour`: tour, điểm dừng, phương tiện theo chặng, điểm tham quan theo tour.
4. `FrmChuyenLe`: tạo chuyến khách lẻ, ngày về tự tính theo `Ngày đi + Số ngày - 1`, đóng đăng ký.
5. `FrmDangKyLe`: đăng ký khách lẻ 1–11 người; tính và ghi nhận tiền vé.
6. `FrmDangKyDoan`: đăng ký đoàn trên 12 người, danh sách người đi khi mua bảo hiểm, hủy phiếu mất cọc.
7. `FrmPhanCongHDV`: phân công HDV, kiểm tra HDV đang hoạt động, kiểm tra chồng chéo lịch và một HDV/chuyến lẻ.
8. `FrmKetThucKhaoSat`: thanh toán đoàn sau ngày kết thúc tour, gửi khảo sát, ghi góp ý.
9. `FrmLuongThongKe`: tính lương tháng và thống kê khoảng ngày.

## 8. Kiểm thử theo tài liệu

Bảng test case trong file Word có TC01–TC24. Một số kịch bản trọng tâm:

- **TC01:** thêm hướng dẫn viên mới trong Danh mục.
- **TC02–TC03:** thêm tour / thử trùng thứ tự điểm dừng.
- **TC04:** tạo chuyến và kiểm tra ngày về tự tính.
- **TC05–TC07:** đăng ký khách lẻ, kiểm tra không nhận 12 người hoặc chuyến đã đóng.
- **TC08–TC12:** đăng ký đoàn, kiểm tra số người > 12, danh sách đủ người khi mua bảo hiểm, tiền cọc > 0 và hủy phiếu.
- **TC13–TC16:** phân công HDV, kiểm tra không phân công HDV trùng lịch và mỗi chuyến lẻ chỉ có một HDV.
- **TC17–TC22:** thanh toán sau khi kết thúc tour, không thu vượt số còn lại, khảo sát chỉ gửi sau tour và mỗi đăng ký tối đa một khảo sát.
- **TC23–TC24:** tính lương và thống kê tổng hợp.

Ngày dữ liệu mẫu trong tài liệu là dữ liệu cố định năm 2026. Nếu kiểm thử sau các mốc ngày đó, một số danh sách “chưa kết thúc tour” sẽ thay đổi theo ngày hệ thống; hãy cập nhật ngày thử cho phù hợp.

## 9. Quy tắc nghiệp vụ cần giữ

- Khách lẻ: số người từ 1 đến 11.
- Khách đoàn: số người lớn hơn 12; trường hợp đúng 12 người bị từ chối theo QD01 vì đề chưa định nghĩa.
- Ngày về / ngày kết thúc dự kiến = ngày đi + số ngày của tour - 1.
- Nếu có khách sạn, hạng sao 2–5; nếu không có khách sạn, hạng sao phải NULL.
- Không phân công hướng dẫn viên có lịch giao nhau.
- Thanh toán kinh phí đoàn chỉ sau ngày kết thúc dự kiến; số thu không được vượt số còn lại.
- Phiếu khảo sát chỉ gửi sau khi kết thúc tour; điểm 1–5 và tối đa một phiếu mỗi đăng ký.

## 10. Ghi chú phạm vi và chất lượng

- Logic nghiệp vụ và cấu trúc CSDL bám theo tài liệu Word; các file Designer là phần hiện thực hóa control list trong tài liệu.
- `HeThongService.cs` và phần kiểm tra kết nối trong `FrmMain` được bổ sung để người dùng dễ xác định lỗi cấu hình trước khi vào các màn hình dữ liệu.
- Tôi không thể kiểm chứng kết nối đến SQL Server đang cài trên máy bạn từ đây. Việc kiểm tra kết nối được thực hiện khi bạn chạy ứng dụng trong Visual Studio; cần dùng đúng instance mà bạn đã chạy script SQL.
