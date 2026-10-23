# QLNS_DongA - Bài thực hành 7: Quản lý nhân sự

## 1. Thành phần bàn giao

- `Database/01_QLNS_DongA.sql`: tạo CSDL `QLNS_DongA`, 19 bảng nghiệp vụ tương ứng 19 lớp chính, 4 bảng liên kết cho các quan hệ N-N khen thưởng/kỷ luật, ràng buộc, trigger, stored procedure và view.
- `Database/02_DuLieuMau_KiemThu.sql`: dữ liệu giả lập phục vụ chạy thử và kiểm thử tháng 09/2026.
- `Database/03_TruyVanKiemTra.sql`: truy vấn kiểm tra quan hệ, gọi thủ tục tính lương và xem kết quả sau khi nạp dữ liệu mẫu.
- `QLNS.DongA.sln`: solution Visual Studio.
- `QLNS.DongA.WinForms/`: mã nguồn C# WinForms, target `.NET Framework 4.7.2`, không cần package NuGet bên ngoài.
- `Tests/KiemThu_va_TruyVet.md`: kịch bản test, dữ liệu cụ thể, kết quả mong đợi và traceability yêu cầu → UML → Form/Service → SQL → test case.

## 2. Công nghệ và kiến trúc

- C# Windows Forms, `.NET Framework 4.7.2`.
- SQL Server / SQL Server Express / LocalDB.
- ADO.NET (`System.Data.SqlClient`).
- Phân lớp: **UI → Service → Data → SQL Server**.

Các thư mục chính:

- `UI`: `MainForm`, `GenericTableForm`, `RecruitmentWorkflowForm`, `AwardForm`, `PayrollForm`.
- `Services`: kiểm tra/điều phối nghiệp vụ tuyển dụng, khen thưởng, tính lương và truy cập bảng dữ liệu.
- `Data`: connection factory, repository SQL, tra cứu danh mục và danh sách bảng cho phép truy cập.

## 3. Cài đặt cơ sở dữ liệu

1. Mở SQL Server Management Studio (SSMS) và kết nối đến instance SQL Server/LocalDB của máy.
2. Mở, chạy toàn bộ `Database/01_QLNS_DongA.sql`.
3. Chạy tiếp `Database/02_DuLieuMau_KiemThu.sql` để nạp bộ dữ liệu test. Nếu không muốn dữ liệu mẫu, có thể bỏ qua bước này.
4. (Tùy chọn) Chạy `Database/03_TruyVanKiemTra.sql` để đối chiếu dữ liệu, tính lương thử và xem kết quả.
5. Mở `QLNS.DongA.sln` bằng Visual Studio 2022 hoặc Visual Studio có hỗ trợ .NET Framework 4.7.2.
6. Mở `QLNS.DongA.WinForms/App.config`, chỉnh `Data Source` cho đúng SQL Server đang dùng nếu không có `(localdb)\MSSQLLocalDB`.
7. Đặt `QLNS.DongA.WinForms` làm Startup Project, Build Solution rồi chạy.

Nếu SQL Server sử dụng SQL Login, thay `Integrated Security=True` bằng `User ID=...;Password=...;` (không lưu mật khẩu thật trong source code khi nộp/chia sẻ).

## 4. Hướng dẫn chạy các nghiệp vụ

### 4.1. Quản lý dữ liệu

1. Vào **Quản lý dữ liệu nghiệp vụ**.
2. Chọn bảng trong ComboBox và nhấn **Tải dữ liệu**.
3. Sửa trực tiếp trên lưới; dùng **Thêm dòng** để tạo dòng mới; chọn dòng và dùng **Xóa dòng chọn** để đánh dấu xóa.
4. Nhấn **Lưu thay đổi**. SQL Server kiểm tra khóa ngoại, khóa chính, CHECK constraint và trigger.
5. Các mã như `MaNhanVien`, `MaPhongBan`, `MaQDLuong` là khóa chính dạng chuỗi do người dùng nhập. Phải tạo dữ liệu theo thứ tự bảng cha trước bảng con. Không thể xóa bản ghi còn được bảng khác tham chiếu.

Màn hình quản lý dữ liệu phục vụ CRUD và kiểm thử. Trong triển khai thực tế nên bổ sung form nhập riêng theo từng bảng, phân quyền, nhật ký thao tác và khóa sửa các chứng từ đã ký/chốt.

### 4.2. Tuyển dụng → thử việc → hợp đồng chính thức

1. Đảm bảo nhân viên và phòng ban đã tồn tại trong màn hình dữ liệu.
2. Trong màn hình nghiệp vụ, chọn nhân viên, phòng ban, ngày ký quyết định, giai đoạn thử việc, lương thử việc và điều khoản hai bên.
3. Nhấn **Lập quyết định thử việc**. Hệ thống lưu quyết định ở trạng thái `DangThuViec` và cập nhật trạng thái nhân viên.
4. Chọn quyết định thử việc ở phần bên phải, chọn loại hợp đồng và ngày ký/ngày bắt đầu.
5. Ngày ký hợp đồng không được trước ngày kết thúc thử việc; ngày bắt đầu hợp đồng không được trước ngày ký.
6. Với hợp đồng dài hạn, hệ thống tạo 2 hồ sơ BHYT/BHXH ở trạng thái `ChoCapNhatMucDong`. Vào bảng `HoSoBaoHiem`, nhập `MucDong` đã được xác nhận và chuyển `TrangThai` thành `DangThamGia` trước khi tính lương. Không dùng số tiền mẫu nếu chưa có quy định chính thức.

### 4.3. Khen thưởng → ký → phiếu chi

1. Chọn đối tượng `Cá nhân` hoặc `Tập thể phòng ban/đơn vị`.
2. Nhập lý do và số tiền; nhấn lập quyết định. Hệ thống tạo quyết định `ChoKy` và một bản ghi trong bảng liên kết tương ứng.
3. Chọn quyết định, nhập ngày ký và nhấn **Giám đốc ký**.
4. Sau khi ký, chọn phương thức chi và nhấn **Tạo phiếu chi**. Stored procedure chỉ cho phép tạo phiếu chi cho quyết định đã ký, chỉ cho tạo một phiếu chi/ quyết định và lấy số tiền từ quyết định để tránh chênh lệch.

Ứng dụng hiện mô phỏng thao tác ký bằng cách cập nhật trạng thái; đây chưa phải chữ ký số và chưa có đăng nhập/phân quyền tài khoản.

### 4.4. Tạo bảng lương

1. Dùng dữ liệu chấm công, phụ trội, quyết định lương/chức vụ và hồ sơ bảo hiểm đã nhập.
2. Chọn tháng/năm rồi nhấn **Tạo / tính lại bảng lương**.
3. Xem kết quả trong lưới. Bảng lương có thể tính lại khi đang ở trạng thái `Nhap`; không thể tính lại khi đã `DaChot`.
4. Bảng lương mẫu trong dữ liệu kiểm thử là tháng 09/2026, mã `BL-202609`.

## 5. Giả định triển khai cần đọc trước khi dùng

Mô tả Bài 7 không cung cấp công thức lương thực nhận chi tiết, giờ chuẩn trong tháng, tỷ lệ BHYT/BHXH, cách phân bổ tiền thưởng tập thể hoặc phân bổ tiền phạt tập thể. Vì vậy, stored procedure sử dụng **quy tắc minh họa**:

- `GioChuanThang = 208` cấu hình trong `App.config` (chỉ để demo; phải thay bằng chính sách được phê duyệt).
- Thu nhập theo công = (lương cơ bản + phụ cấp lương + phụ cấp độc hại + phụ cấp chức vụ) × MIN(giờ công / giờ chuẩn, 1).
- Tiền phụ trội = lương cơ bản / giờ chuẩn × giờ phụ trội thực tế × 1,5.
- BHYT/BHXH được khấu trừ theo **số tiền MucDong đã nhập**, không tự suy đoán tỷ lệ.
- Quyết định khen thưởng được thực hiện qua `PhieuChiKhenThuong` theo mô tả nên không tự cộng vào lương; chỉ khấu trừ khoản phạt cá nhân có hình thức `TruLuong` được ký trong tháng. Không tự chia tiền phạt tập thể cho nhân viên.

Các công thức trên giúp chạy và kiểm thử được dự án, nhưng không được xem là công thức chính thức của Công ty Sơn Đông Á.

## 6. Lưu ý về tính đầy đủ

Đây là project thực hành có CRUD tổng quát cho 23 bảng vật lý và các luồng chính (thử việc/hợp đồng, khen thưởng/phiếu chi, tính lương). Nó chưa phải hệ thống HRM production: chưa có xác thực đăng nhập, phân quyền theo vai trò, chữ ký số, báo cáo in, sao lưu/khôi phục, nhật ký kiểm toán đầy đủ, hoặc quy trình phê duyệt đa cấp.
