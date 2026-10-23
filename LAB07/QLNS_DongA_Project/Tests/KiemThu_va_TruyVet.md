# Kiểm thử theo kịch bản và truy vết yêu cầu – UML – Form/Service – CSDL – Test case

## 1. Bộ dữ liệu kiểm thử

Chạy `Database/01_QLNS_DongA.sql`, sau đó `Database/02_DuLieuMau_KiemThu.sql`.

| Dữ liệu | Giá trị mẫu | Mục đích |
|---|---|---|
| Phòng ban | `PB-TCKT`, `PB-KT`, `PB-SX` | Kiểm thử FK phòng ban và đối tượng khen thưởng tập thể |
| Nhân viên | `NV001` Nguyễn Văn An; `NV002` Trần Thị Bình; `NV003` Lê Văn Cường | NV001/NV002 đang làm; NV003 đã kết thúc thử việc |
| Hợp đồng | `HD-SEED01` dài hạn của NV001; `HD-SEED02` ngắn hạn của NV002 | Kiểm thử ràng buộc BHYT/BHXH |
| Quyết định lương | `QDL-SEED01`: 15.000.000 + 500.000 + 0; `QDL-SEED02`: 12.000.000 + 300.000 + 200.000 | Dữ liệu lương giả lập cho tháng 09/2026 |
| Quyết định chức vụ | `QDCV-SEED01`: phụ cấp 1.000.000 cho NV001; `QDCV-SEED02`: phụ cấp 0 cho NV002 | Kiểm thử lịch sử chức vụ |
| Giờ công tháng 09/2026 | NV001 = 176 giờ; NV002 = 160 giờ | Kiểm thử tổng giờ công từ chi tiết chấm công |
| Giờ phụ trội tháng 09/2026 | NV001 = 10 giờ; NV002 = 4 giờ, hệ số 1,5 | Kiểm thử quy đổi giờ và tiền phụ trội |
| Bảo hiểm NV001 | BHYT = 150.000; BHXH = 1.200.000 mỗi tháng | Số tiền minh họa đã được xác nhận, không phải tỷ lệ chính thức |
| Thưởng/phạt NV001 | Thưởng cá nhân 500.000 ngày 25/09/2026; phạt trừ lương 100.000 ngày 20/09/2026 | Kiểm thử phiếu chi thưởng riêng và trừ phạt cá nhân trong lương |

## 2. Công thức tham chiếu cho test tính lương

**Các giá trị sau chỉ đúng theo quy tắc minh họa trong stored procedure, không phải quy định chính thức của đề bài.** `GioChuanThang = 208`.

### NV001 – Nguyễn Văn An

- Lương cơ bản: 15.000.000 VNĐ.
- Phụ cấp lương: 500.000 VNĐ; phụ cấp độc hại: 0 VNĐ; phụ cấp chức vụ: 1.000.000 VNĐ.
- Giờ công: 176; giờ phụ trội thực tế: 10; giờ phụ trội quy đổi: 10 × 1,5 = 15.
- Thu nhập cố định theo công: (15.000.000 + 500.000 + 0 + 1.000.000) × 176 / 208 = **13.961.538,46 VNĐ**.
- Tiền phụ trội: 15.000.000 / 208 × 10 × 1,5 = **1.081.730,77 VNĐ**.
- Quyết định thưởng 500.000 VNĐ được xử lý bằng phiếu chi riêng, không tự cộng vào bảng lương; BHYT: 150.000 VNĐ; BHXH: 1.200.000 VNĐ; phạt trừ lương: 100.000 VNĐ.
- Tổng thu nhập = 13.961.538,46 + 1.081.730,77 = **15.043.269,23 VNĐ**.
- Thực lĩnh = 15.043.269,23 − 150.000 − 1.200.000 − 100.000 = **13.593.269,23 VNĐ**.

### NV002 – Trần Thị Bình

- Lương cơ bản: 12.000.000 VNĐ; phụ cấp lương: 300.000 VNĐ; phụ cấp độc hại: 200.000 VNĐ; phụ cấp chức vụ: 0 VNĐ.
- Giờ công: 160; giờ phụ trội thực tế: 4; giờ phụ trội quy đổi: 6.
- Thu nhập cố định theo công: 12.500.000 × 160 / 208 = **9.615.384,62 VNĐ**.
- Tiền phụ trội: 12.000.000 / 208 × 4 × 1,5 = **346.153,85 VNĐ**.
- Không có hồ sơ bảo hiểm đang tham gia, thưởng cá nhân hay phạt trừ lương trong dữ liệu mẫu.
- Tổng thu nhập và thực lĩnh = **9.961.538,47 VNĐ**.

## 3. Cách chạy bộ test để kết quả không bị ảnh hưởng chéo

- Luôn bắt đầu trên CSDL mới, lần lượt chạy `01_QLNS_DongA.sql` rồi `02_DuLieuMau_KiemThu.sql`. Script seed chỉ thêm dữ liệu còn thiếu, không tự xóa dữ liệu phát sinh từ lần chạy trước.
- Để giữ nguyên kết quả bảng lương chuẩn tháng 09/2026, thực hiện TC12 và TC14 trước TC15; thực hiện TC13 sau các test bảng lương, hoặc dùng CSDL test riêng. TC15 chốt `BL-202609`, nên không thể chạy lại tính lương tháng 09/2026 trên cùng trạng thái dữ liệu.
- TC07 ký hợp đồng cho `NV003`, làm nhân viên này chuyển sang `DangLam`; vì NV003 không có quyết định lương mẫu nên TC07 phải chạy sau nhóm test bảng lương trên CSDL gốc, hoặc trên CSDL riêng. Sau TC07 không dùng lại dữ liệu đó để xác nhận kết quả lương mẫu hai nhân viên.
- TC04 nên dùng nhân viên thử việc mới, ví dụ tạo `NV004` (Họ tên `Phạm Minh Dũng`, trạng thái `ThuViec`, phòng ban `PB-SX`) trước khi lập quyết định. Không dùng NV003 để tránh làm lẫn với `QDT-SEED03` dành cho TC06–TC07.
- Với TC13, dùng mã `NV-TEST-NOSALARY`, trạng thái `DangLam`, phòng ban `PB-KT` nhưng không có quyết định lương. Sau khi xác nhận thủ tục từ chối tính lương, đổi trạng thái thử nghiệm sang `NgungLam` để không ảnh hưởng các kỳ test tiếp theo:

```sql
UPDATE dbo.NhanVien
SET TrangThaiNhanVien = N'NgungLam'
WHERE MaNhanVien = N'NV-TEST-NOSALARY';
```

- TC17 nên dùng ngày hiệu lực của quyết định mới từ `2026-10-01` trở đi và xem kỳ lương khác (ví dụ tháng 10/2026), không chỉnh quyết định mẫu đang dùng để xác nhận TC12.

## 4. Kịch bản kiểm thử chức năng

| TC | Tiền điều kiện / dữ liệu | Thao tác | Kết quả mong đợi |
|---|---|---|---|
| TC01 – Tải và sửa bảng danh mục | CSDL đã được tạo; bảng `PhongBan` có `PB-TCKT` | Mở Quản lý dữ liệu, chọn `PhongBan`, sửa mô tả hoặc thêm một phòng ban với mã mới, lưu | Dữ liệu được lưu; tải lại vẫn thấy thay đổi |
| TC02 – Khóa chính trùng | Bảng `NhanVien` đã có `NV001` | Thêm dòng khác dùng mã `NV001`, lưu | SQL Server từ chối do PK; giao diện hiện thông báo lỗi, dữ liệu không được ghi thành dòng trùng |
| TC03 – FK không hợp lệ | `MaPhongBan = PB-NOT-EXIST` không tồn tại | Thêm nhân viên mới với mã phòng ban không tồn tại | SQL Server từ chối do FK; không tạo nhân viên mồ côi |
| TC04 – Lập quyết định thử việc hợp lệ | Nhân viên chưa có hợp đồng hiệu lực, phòng ban hoạt động | Trong form tuyển dụng, chọn nhân viên/phòng ban; ngày ký không sau ngày bắt đầu; ngày kết thúc sau ngày bắt đầu; nhập lương và đủ hai điều khoản | Tạo `QuyetDinhTuyenDung` trạng thái `DangThuViec`; trạng thái nhân viên là `ThuViec`; các thao tác cập nhật nằm trong một transaction |
| TC05 – Thử việc có ngày sai | Bất kỳ nhân viên phù hợp | Đặt ngày kết thúc thử việc trước ngày bắt đầu hoặc ngày ký sau ngày bắt đầu, rồi lập quyết định | Service/CSDL từ chối; không phát sinh quyết định mới |
| TC06 – Không ký HĐ trước khi hết thử việc | `QDT-SEED03` có ngày kết thúc thử việc 31/08/2026 | Thử ký hợp đồng với ngày ký 15/08/2026 | Bị từ chối vì ngày ký trước ngày kết thúc thử việc |
| TC07 – Ký hợp đồng dài hạn | `QDT-SEED03` còn ở trạng thái được ký hợp đồng; ngày ký từ 31/08/2026 trở đi | Chọn dài hạn, ký hợp đồng; sau đó mở `HoSoBaoHiem` | Hợp đồng được tạo; nhân viên chuyển `DangLam`; hai hồ sơ BHYT/BHXH được tạo ở trạng thái `ChoCapNhatMucDong` |
| TC08 – Chỉ hợp đồng dài hạn có hồ sơ BH | `HD-SEED02` là hợp đồng ngắn hạn | Thử thêm dòng `HoSoBaoHiem` gắn `HD-SEED02` | Trigger SQL từ chối; thông báo lỗi nghiệp vụ |
| TC09 – Lập quyết định khen thưởng cá nhân | `NV001` tồn tại | Chọn Cá nhân, NV001, lý do và số tiền 500.000; lập quyết định | Có `QuyetDinhKhenThuong` trạng thái `ChoKy` và một dòng trong `QDKhenThuong_NhanVien` |
| TC10 – Ký và lập phiếu chi | Đã có quyết định chờ ký | Thử tạo phiếu chi trước khi ký; sau đó ký và tạo lại phiếu chi | Trước khi ký: bị từ chối. Sau khi ký: tạo phiếu chi, số tiền lấy từ quyết định; chỉ cho một phiếu chi mỗi quyết định |
| TC11 – Không cho lệch số tiền chi | TC10 đã tạo phiếu chi hợp lệ cho quyết định thưởng 500.000 | Sửa phiếu chi đang có thành số tiền 450.000 rồi lưu | Trigger từ chối vì số tiền không khớp quyết định; giá trị hợp lệ trước đó vẫn được giữ sau rollback |
| TC12 – Tính bảng lương tháng 09/2026 | Đã nạp dữ liệu mẫu; `GioChuanThang=208` | Mở Tạo bảng lương, chọn tháng 9 năm 2026, tính | Có 2 dòng nhân viên; NV001 thực lĩnh xấp xỉ 13.593.269,23 VNĐ; NV002 xấp xỉ 9.961.538,47 VNĐ (theo chính sách demo; quyết định khen thưởng chi qua phiếu chi riêng nên không cộng vào lương) |
| TC13 – Tính lương khi thiếu QĐ lương | Tạo một nhân viên `DangLam` không có QĐ lương đã ký có hiệu lực | Tạo bảng lương cho kỳ bất kỳ | Stored procedure từ chối và nêu yêu cầu bổ sung QĐ lương; không tạo bảng lương không đầy đủ |
| TC14 – Tính lại bảng lương đang nhập | Bảng lương đã tồn tại trạng thái `Nhap` | Chạy tạo/tính lại cùng tháng | Chi tiết cũ được thay bằng chi tiết mới trong transaction; không phát sinh trùng dòng nhân viên |
| TC15 – Không tính lại bảng lương đã chốt | Đổi `BangLuong.TrangThai` của `BL-202609` thành `DaChot` | Chạy lại tính lương tháng 9/2026 | Stored procedure từ chối; dữ liệu đã chốt không bị ghi đè |
| TC16 – Kỷ luật: nộp phạt hoặc trừ lương | Chuẩn bị hai quyết định kỷ luật đã ký: một quyết định `NopPhat` và một quyết định `TruLuong`, mỗi quyết định có đối tượng cá nhân phù hợp | Lập phiếu thu đúng số tiền cho quyết định `NopPhat`; thử tạo phiếu thu cho quyết định `TruLuong`; thử cập nhật phiếu thu thành số tiền sai | Phiếu thu đúng cho `NopPhat` được chấp nhận; phiếu thu cho `TruLuong` và số tiền không khớp bị trigger từ chối |
| TC17 – Truy vấn lịch sử quyết định | Một nhân viên có nhiều quyết định lương/chức vụ với ngày hiệu lực khác nhau | Tạo bảng lương tại hai kỳ khác nhau, với QĐ mới có hiệu lực giữa hai kỳ | Kỳ trước dùng quyết định cũ; kỳ sau dùng quyết định mới nhất có hiệu lực trước hoặc trong kỳ |
| TC18 – Bảo toàn dữ liệu khi lưu thất bại | Thử thao tác lưu nhiều dòng có một FK sai | Nhấn lưu | Giao dịch cập nhật toàn lưới phải rollback toàn bộ nếu một lệnh thất bại; kiểm tra dữ liệu trong SQL Server không có các dòng đã cập nhật dở dang và lưới vẫn giữ trạng thái chỉnh sửa để người dùng sửa lỗi rồi lưu lại |

| TC19 – Không cho đổi loại quyết định trái với liên kết | Có quyết định khen thưởng cá nhân đã liên kết NV001, ví dụ `QDKT-SEED01` | Thử đổi `LoaiDoiTuongApDung` của quyết định sang `TapThe` trong màn hình dữ liệu | Trigger từ chối; quyết định vẫn là `CaNhan`, liên kết nhân viên còn nguyên |
| TC20 – Khen thưởng tập thể | Phòng ban `PB-KT` tồn tại | Lập quyết định chọn `Tập thể phòng ban/đơn vị`, sau đó kiểm tra bảng liên kết | Quyết định ở `ChoKy`, có đúng dòng trong `QDKhenThuong_PhongBan`; không có dòng ở `QDKhenThuong_NhanVien` |

## 5. Truy vết yêu cầu → UML → Form/Service → bảng CSDL → test case

| ID yêu cầu | Yêu cầu gốc/diễn giải | UML liên quan | Form / Service | Bảng / thủ tục SQL | Test |
|---|---|---|---|---|---|
| REQ-01 | Nhân viên được tiếp nhận thử việc phải có quyết định tuyển dụng, nêu phòng ban, thời gian, mức lương và điều khoản | Câu 1–2: `NhanVien`, `PhongBan`, `QuyetDinhTuyenDung`; Câu 3–5: UC tiếp nhận, Use Case phân rã, Activity Diagram | `RecruitmentWorkflowForm` → `RecruitmentService` → `RecruitmentRepository` | `NhanVien`, `PhongBan`, `QuyetDinhTuyenDung` | TC04, TC05 |
| REQ-02 | Sau thử việc ký hợp đồng; hợp đồng dài hạn/ngắn hạn; hợp đồng dài hạn có BHYT/BHXH | `HopDongLaoDong`, `HoSoBaoHiem`; Activity Diagram nhánh hợp đồng | `RecruitmentWorkflowForm` → `RecruitmentService` | `HopDongLaoDong`, `HoSoBaoHiem`, trigger `TR_HoSoBaoHiem_ChiHopDongDaiHan` | TC06, TC07, TC08 |
| REQ-03 | Lưu lịch sử quyết định chức vụ và quyết định lương | Class Diagram: `QuyetDinhChucVu`, `QuyetDinhLuong` liên kết `NhanVien` | `GenericTableForm` → `GenericTableService` | `QuyetDinhChucVu`, `QuyetDinhLuong` | TC01, TC17 |
| REQ-04 | Lập kế hoạch phân công theo tuần/ca; chấm công và ghi nhận phụ trội | Class Diagram: `KeHoachPhanCong`, `PhanCongCa`, `BangChamCong`, `ChiTietChamCong`, `PhieuPhuTroi`, `ChiTietPhuTroi` | `GenericTableForm` → `GenericTableService` | Sáu bảng lịch/phân công/chấm công/phụ trội | TC01, TC12, TC14 |
| REQ-05 | Lương tháng dựa trên QĐ lương, QĐ chức vụ, chấm công, phụ trội và bảo hiểm | Use Case phân rã tính lương; Class Diagram: `BangLuong`, `ChiTietBangLuong` | `PayrollForm` → `PayrollService` → `PayrollRepository` | `QuyetDinhLuong`, `QuyetDinhChucVu`, `ChiTietChamCong`, `ChiTietPhuTroi`, `HoSoBaoHiem`, `BangLuong`, `ChiTietBangLuong`, `sp_TaoBangLuongThang`, `vw_BangLuongChiTiet` | TC12–TC15, TC17 |
| REQ-06 | Khen thưởng cá nhân/tập thể; quyết định phải được Giám đốc ký; chi tiền bằng phiếu chi | Class Diagram: `QuyetDinhKhenThuong` ↔ `NhanVien`/`PhongBan`; Sequence Diagram lập quyết định khen thưởng | `AwardForm` → `AwardService` → `AwardRepository` | `QuyetDinhKhenThuong`, `QDKhenThuong_NhanVien`, `QDKhenThuong_PhongBan`, `PhieuChiKhenThuong`, `sp_LapQuyetDinhKhenThuong`, `sp_KyQuyetDinhKhenThuong`, `sp_LapPhieuChiKhenThuong` | TC09–TC11 |
| REQ-07 | Kỷ luật cho cá nhân/tập thể; ghi nhận phiếu thu nộp phạt hoặc trừ lương | Class Diagram: `QuyetDinhKyLuat` ↔ `NhanVien`/`PhongBan`; `ChiTietBangLuong` | `GenericTableForm` → `GenericTableService`; bảng lương dùng `PayrollService` | `QuyetDinhKyLuat`, `QDKyLuat_NhanVien`, `QDKyLuat_PhongBan`, `PhieuThuNopPhat`, `ChiTietBangLuong` | TC12, TC16 |
| REQ-08 | Dữ liệu quan hệ toàn vẹn, không dùng quyết định chưa ký cho khoản chi | Constraint/FK/trigger trong thiết kế CSDL; Sequence Diagram nhánh lỗi trước khi ký | `AwardForm` → `AwardService` | FK, CHECK, `TR_PhieuChiKT_PhaiCoQuyetDinhDaKy`, `TR_PhieuThuPhat_PhaiCoQuyetDinhDaKy`, `TR_QDKhenThuong_KhongDoiLoaiSai`, `TR_QDKyLuat_KhongDoiLoaiSai` | TC02, TC03, TC10, TC11, TC18–TC20 |

## 6. Tiêu chí chấp nhận

- [ ] Chạy được script tạo schema trên SQL Server hỗ trợ cú pháp `CREATE OR ALTER` (SQL Server 2016 SP1 trở lên khuyến nghị).
- [ ] Nạp được dữ liệu mẫu, không có lỗi FK/unique/check.
- [ ] Mở được solution với target .NET Framework 4.7.2 sau khi cấu hình đúng connection string.
- [ ] Tạo quyết định thử việc và hợp đồng theo đúng thứ tự ngày tháng.
- [ ] Không tạo được hồ sơ bảo hiểm cho hợp đồng ngắn hạn.
- [ ] Không tạo được phiếu chi khi quyết định chưa ký hoặc số tiền không khớp.
- [ ] Tạo được bảng lương mẫu tháng 09/2026 với hai dòng kết quả phù hợp công thức demo nêu tại Mục 2.
- [ ] Ghi nhận rõ: công thức/tỷ lệ chính thức phải được người phụ trách nghiệp vụ xác nhận trước khi dùng dữ liệu lương thực tế.
