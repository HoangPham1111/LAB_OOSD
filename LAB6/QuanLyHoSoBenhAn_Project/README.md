# Quản lý hồ sơ bệnh án – C# WinForms (.NET Framework 4.7.2)

## 1. Công nghệ

- C# WinForms, .NET Framework 4.7.2
- SQL Server (khuyến nghị SQL Server 2016 SP1 trở lên vì script dùng `CREATE OR ALTER PROCEDURE`)
- ADO.NET (`System.Data.SqlClient`), truy vấn có tham số và stored procedure
- Giao dịch SQL Server cho các thao tác nhiều bảng

## 2. Chức năng đã hiện thực

1. **Bệnh nhân:** tìm kiếm, tạo mới, cập nhật hồ sơ đăng ký.
2. **Lệ phí năm:** ghi nhận/cập nhật lệ phí theo bệnh nhân và năm, phân biệt `ChuaDong`, `DaDong`, `Huy`.
3. **Cấp hồ sơ bệnh án:** chỉ cấp hồ sơ mới khi đã ghi nhận lệ phí năm hiện tại là `DaDong`; nếu hồ sơ đã tồn tại, trả lại mã hồ sơ đó.
4. **Danh mục:** quản lý bác sĩ, khoa điều trị, nhân viên chăm sóc khách hàng và thuốc; không xóa bản ghi có thể liên quan lịch sử y tế, thay vào đó đánh dấu ngừng hoạt động.
5. **Khám bệnh:** tạo đợt điều trị và phiếu khám trong cùng transaction.
6. **Lập toa thuốc:** kiểm tra phiếu khám/đợt điều trị, chống tạo toa trùng, kiểm tra danh mục thuốc; lưu `ToaThuoc` và nhiều `ChiTietToaThuoc` bằng một transaction và table-valued parameter.
7. **Xét nghiệm:** tạo chỉ định, ghi kết quả và trạng thái.
8. **Lịch khám định kỳ:** đặt lịch và cập nhật trạng thái.
9. **Viện phí:** tạo hóa đơn, ghi nhận thanh toán một phần/nhiều lần, chặn thanh toán vượt số tiền còn nợ.
10. **Hoàn tất đợt điều trị:** yêu cầu hóa đơn đã thanh toán đầy đủ trước khi đóng đợt.

## 3. Thiết lập cơ sở dữ liệu

1. Mở SQL Server Management Studio (SSMS), kết nối tới SQL Server.
2. Mở `Database/QuanLyHoSoBenhAn.sql`.
3. Chạy toàn bộ script. Script tạo database `QuanLyHoSoBenhAn`, các bảng, PK/FK/UNIQUE/CHECK, index, table type và stored procedure. Một số danh mục DEMO sẽ được thêm nếu bảng danh mục tương ứng còn rỗng.
4. Không chạy script trên một database sản xuất chưa sao lưu. Script tạo/đảm bảo các đối tượng, không phải công cụ migrate các schema đã thay đổi thủ công.

## 4. Mở và chạy WinForms

1. Mở `QuanLyHoSoBenhAn.sln` bằng Visual Studio trên Windows có workload **.NET desktop development**.
2. Trong `src/QuanLyHoSoBenhAn.WinForms/App.config`, sửa `Data Source` cho đúng máy SQL Server:
   - SQL Server Express: `Data Source=.\SQLEXPRESS`
   - LocalDB: `Data Source=(localdb)\MSSQLLocalDB`
   - Default instance: `Data Source=.` hoặc tên máy/instance thực tế.
3. Đảm bảo tài khoản Windows chạy ứng dụng có quyền truy cập database `QuanLyHoSoBenhAn`.
4. Chọn `QuanLyHoSoBenhAn.WinForms` làm Startup Project, build và chạy.

Ứng dụng dùng `Integrated Security=True`; không có thông tin đăng nhập SQL dạng mật khẩu được nhúng trong project.

## 5. Luồng kiểm thử gợi ý

1. Tạo bệnh nhân ở module **Bệnh nhân**.
2. Vào **Lệ phí năm**, chọn bệnh nhân, chọn năm hiện tại, số tiền và trạng thái `DaDong`.
3. Vào **Cấp hồ sơ bệnh án**, chọn bệnh nhân và cấp/lấy mã hồ sơ.
4. Vào **Khám bệnh / Phiếu khám**, chọn hồ sơ, bác sĩ, khoa, nhập triệu chứng và tạo phiếu khám.
5. Vào **Lập toa thuốc**, chọn phiếu khám chưa có toa, thêm ít nhất một thuốc và lưu.
6. Kiểm tra các bảng `ToaThuoc` và `ChiTietToaThuoc`; thử lưu lại cùng `MaPK` để xác nhận bị chặn tạo toa trùng.
7. Tạo xét nghiệm nếu cần; tạo hóa đơn cho đợt điều trị, ghi nhận thanh toán đủ; quay lại **Khám bệnh / Phiếu khám** chọn đợt và hoàn tất.

## 6. Giả định mô hình cần biết

- Một bệnh nhân có một hồ sơ bệnh án chính (`UNIQUE HoSoBenhAn.MaBN`).
- Một phiếu khám có tối đa một toa thuốc (`UNIQUE ToaThuoc.MaPK`).
- Một đợt điều trị có tối đa một hóa đơn viện phí (`UNIQUE HoaDonVienPhi.MaDT`).
- Một thuốc xuất hiện tối đa một dòng trong cùng một toa (`PRIMARY KEY ChiTietToaThuoc(MaToa, MaThuoc)`).
- Thuật ngữ trạng thái trong CSDL lưu không dấu để đơn giản việc kiểm tra ràng buộc; giao diện hiển thị tiếng Việt.
- Đây là đồ án học tập, không phải phần mềm y tế đạt chứng nhận hoặc đã đánh giá an toàn lâm sàng. Danh mục thuốc DEMO chỉ để kiểm thử, không dùng làm hướng dẫn điều trị.

## 7. Cấu trúc project

```text
QuanLyHoSoBenhAn.sln
├── Database/QuanLyHoSoBenhAn.sql
└── src/QuanLyHoSoBenhAn.WinForms/
    ├── Program.cs
    ├── App.config
    ├── Data/Db.cs
    ├── Forms/
    │   ├── MainForm.cs
    │   ├── PatientForm.cs
    │   ├── MasterDataForm.cs
    │   ├── AnnualFeeForm.cs
    │   ├── RecordForm.cs
    │   ├── ClinicalForm.cs
    │   ├── PrescriptionForm.cs
    │   ├── TestForm.cs
    │   ├── AppointmentForm.cs
    │   ├── HospitalFeeForm.cs
    │   └── Ui.cs
    └── Properties/AssemblyInfo.cs
```
