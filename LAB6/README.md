LAB6 – Quản lý hồ sơ bệnh án của bệnh nhân
Ứng dụng quản lý hồ sơ bệnh án được xây dựng bằng C# Windows Forms trên .NET Framework 4.7.2 và Microsoft SQL Server. Đồ án mô phỏng quy trình tiếp nhận bệnh nhân, ghi nhận lệ phí năm, cấp hồ sơ bệnh án, khám bệnh, lập toa thuốc, xét nghiệm, đặt lịch khám định kỳ và thanh toán viện phí.
Phạm vi: Đây là đồ án học tập để minh họa phân tích, thiết kế và hiện thực phần mềm. Dữ liệu thuốc DEMO chỉ phục vụ kiểm thử, không dùng để đưa ra quyết định điều trị thực tế.

1. Nghiệp vụ chính
1.1. Đăng ký bệnh nhân, đóng lệ phí năm và cấp hồ sơ
1. Nhân viên mở module Bệnh nhân và nhập thông tin đăng ký: họ tên, địa chỉ, ngày sinh, điện thoại và các thông tin được hỗ trợ trên giao diện.
2. Mở module Lệ phí năm để ghi nhận khoản phí của năm áp dụng.
3. Hệ thống không cho cấp hồ sơ mới nếu lệ phí năm hiện tại chưa ở trạng thái DaDong.
4. Khi điều kiện được đáp ứng, mở module Cấp hồ sơ bệnh án để tạo hoặc lấy mã hồ sơ. Nếu hồ sơ đã tồn tại, thao tác cấp lại trả về hồ sơ hiện có thay vì tạo bản ghi trùng.
1.2. Khám bệnh và tra cứu lịch sử
1. Tìm bệnh nhân theo mã bệnh nhân, mã hồ sơ, mã phiếu khám, họ tên hoặc điện thoại trong module Tra cứu lịch sử.
2. Xem các lần khám trước, thông tin đợt điều trị, phiếu khám, xét nghiệm, toa thuốc và dữ liệu viện phí có liên quan.
3. Trong module Khám bệnh / Phiếu khám, chọn hồ sơ, tạo đợt điều trị và ghi phiếu khám với bác sĩ, khoa, ngày giờ, triệu chứng và các chỉ số sức khỏe.
1.3. Lập toa thuốc
1. Mở module Lập toa thuốc và chọn phiếu khám hợp lệ thuộc đợt điều trị chưa đóng.
2. Thêm từng thuốc, nhập liều dùng, cách dùng, số lượng và số ngày sử dụng; nhập ngày kê và lời dặn nếu có.
3. Lưu toa. Hệ thống kiểm tra phiếu khám, danh mục thuốc và điều kiện không tạo toa trùng.
4. Dữ liệu ToaThuoc và các dòng ChiTietToaThuoc được lưu trong cùng transaction SQL Server. Nếu một bước lưu thất bại, giao dịch được hoàn tác.
1.4. Xét nghiệm
1. Mở module Xét nghiệm, chọn đúng phiếu khám.
2. Tạo chỉ định xét nghiệm.
3. Nhập kết quả và trạng thái sau khi thực hiện.
4. Xét nghiệm được liên kết với phiếu khám qua MaPK, để có thể tra cứu lại trong lịch sử bệnh án.
1.5. Đặt và thực hiện khám định kỳ
1. Nhân viên chăm sóc khách hàng mở module Lịch khám định kỳ, chọn hồ sơ và ngày giờ hẹn.
2. Khi tạo đợt điều trị loại KhamDinhKy, chọn lịch đã đặt hợp lệ của đúng hồ sơ.
3. Khi lưu đợt khám thành công, lịch được liên kết với đợt khám và chuyển sang trạng thái đã thực hiện.
4. Kết quả khám định kỳ vẫn được lưu trong lịch sử kể cả khi không phát hiện bệnh.
Giả định mô hình: Hệ thống áp dụng giới hạn một lịch khám định kỳ đang có hiệu lực cho mỗi hồ sơ trong một quý; lịch đã thực hiện được tính là đã sử dụng quyền lợi quý đó. Quy tắc này cần được xác nhận với nghiệp vụ/giảng viên trước khi xem là quy định chính thức.
1.6. Viện phí và hoàn tất đợt điều trị
1. Mở module Viện phí, tạo hóa đơn cho đợt điều trị.
2. Ghi nhận một hoặc nhiều lần thanh toán. Hệ thống không cho thanh toán vượt số tiền còn nợ.
3. Với đợt DieuTri thông thường, chỉ đóng đợt sau khi hóa đơn đã thanh toán đủ.
4. Đợt KhamDinhKy miễn phí có thể hoàn tất mà không cần hóa đơn. Nếu đã lập hóa đơn thì hóa đơn phải được thanh toán đủ trước khi đóng đợt.
5. Sau khi đóng, thông tin vẫn có thể được tra cứu trong lịch sử.
2. Công nghệ và yêu cầu môi trường
- Windows.
- Visual Studio có workload .NET desktop development.
- .NET Framework 4.7.2 Developer Pack/Targeting Pack.
- Microsoft SQL Server; script sử dụng CREATE OR ALTER PROCEDURE, nên khuyến nghị SQL Server 2016 SP1 trở lên.
- SQL Server Management Studio (SSMS) để cài đặt CSDL và chạy kiểm thử.
- Quyền truy cập SQL Server bằng tài khoản Windows hiện tại hoặc tài khoản được cấu hình phù hợp.
3. Cấu trúc thư mục
LAB6/
├── README.md
├── QuanLyHoSoBenhAn.sql                 # Script CSDL để cài đặt từ thư mục LAB6
├── 1250080062_PhamHuyHoang_Lab06.docx   # Báo cáo bài thực hành
└── QuanLyHoSoBenhAn_Project/
    ├── QuanLyHoSoBenhAn.sln
    ├── README.md
    ├── Database/
    │   ├── QuanLyHoSoBenhAn.sql
    │   └── Tests/SmokeTest_QuyTacNghiepVu.sql
    ├── KiemThu/README.md
    └── src/QuanLyHoSoBenhAn.WinForms/
        ├── App.config
        ├── Program.cs
        ├── Data/Db.cs
        └── Forms/
Hai file QuanLyHoSoBenhAn.sql ở thư mục gốc và trong QuanLyHoSoBenhAn_Project/Database là bản script được đặt ở hai vị trí để tiện chạy/đóng gói. Nên chạy một bản duy nhất cho một lần cài đặt.
4. Cài đặt cơ sở dữ liệu SQL Server
1. Khởi động SQL Server và mở SQL Server Management Studio (SSMS).
2. Kết nối tới đúng instance SQL Server đang cài trên máy.
3. Chọn File → Open → File và mở LAB6/QuanLyHoSoBenhAn.sql.
4. Nhấn Execute để chạy toàn bộ script. Script tạo database QuanLyHoSoBenhAn nếu chưa có, sau đó tạo các bảng, khóa chính/ngoại, ràng buộc UNIQUE/CHECK, index, table type và stored procedure.
5. Kiểm tra Object Explorer có database QuanLyHoSoBenhAn và các bảng nghiệp vụ.
Lưu ý: Chạy trên database đồ án/phát triển. Không chạy trên CSDL bệnh viện thật hoặc dữ liệu quan trọng khi chưa sao lưu và chưa đánh giá script. Script bảo đảm/tạo các đối tượng theo cấu trúc đã thiết kế, không thay thế quy trình migration cho schema đã bị sửa thủ công.
5. Cấu hình và chạy WinForms
1. Mở LAB6/QuanLyHoSoBenhAn_Project/QuanLyHoSoBenhAn.sln bằng Visual Studio.
2. Trong Solution Explorer, nhấp phải QuanLyHoSoBenhAn.WinForms → Set as Startup Project.
3. Mở file src/QuanLyHoSoBenhAn.WinForms/App.config và sửa Data Source theo instance SQL Server trên máy. Ví dụ:
   SQL Server Express:
   Data Source=.\SQLEXPRESS
   LocalDB:
   Data Source=(localdb)\MSSQLLocalDB
   Default instance:
   Data Source=.
4. Chuỗi kết nối hiện dùng Integrated Security=True; tài khoản Windows chạy ứng dụng cần có quyền truy cập database QuanLyHoSoBenhAn.
5. Nếu project báo thiếu reference assemblies, cài .NET Framework 4.7.2 Developer Pack/Targeting Pack trong Visual Studio Installer.
6. Chọn Build → Rebuild Solution. Chỉ chạy tiếp khi Error List không còn lỗi biên dịch.
7. Nhấn F5 để chạy có debugger hoặc Ctrl+F5 để chạy không debugger.
Nếu Visual Studio báo không tìm thấy QuanLyHoSoBenhAn.WinForms.exe, hãy mở View → Error List và sửa lỗi build đầu tiên. File .exe chỉ được tạo khi biên dịch thành công.
6. Kịch bản chạy thử từ đầu đến cuối
Nên dùng dữ liệu mới phục vụ đồ án, không dùng thông tin bệnh nhân thật.
Kịch bản A – Đăng ký và cấp hồ sơ
1. Vào Bệnh nhân và thêm một bệnh nhân thử nghiệm.
2. Vào Lệ phí năm, ghi lệ phí cho năm hiện tại ở trạng thái ChuaDong.
3. Vào Cấp hồ sơ bệnh án và thử cấp hồ sơ. Kết quả mong đợi: hệ thống từ chối vì lệ phí chưa đóng.
4. Quay lại Lệ phí năm, cập nhật trạng thái thành DaDong.
5. Cấp hồ sơ lại. Kết quả mong đợi: tạo/nhận được MaHS.
6. Thực hiện thao tác cấp/lấy hồ sơ lần nữa. Kết quả mong đợi: cùng một MaHS, không phát sinh hồ sơ chính thứ hai.
Kịch bản B – Khám bệnh, toa thuốc và xét nghiệm
1. Vào Khám bệnh / Phiếu khám, tạo một đợt DieuTri cho hồ sơ vừa cấp và ghi phiếu khám.
2. Mở Lập toa thuốc, chọn MaPK, thêm ít nhất một thuốc cùng thông tin liều dùng/cách dùng/số lượng/số ngày, rồi lưu.
3. Kiểm tra kết quả trả về và tra cứu lại phiếu khám. Kết quả mong đợi: toa và chi tiết thuốc được liên kết đúng.
4. Thử lưu toa thứ hai cho cùng MaPK. Kết quả mong đợi: bị chặn theo giả định một phiếu khám tối đa một toa.
5. Mở Xét nghiệm, tạo chỉ định theo cùng MaPK, nhập kết quả.
6. Mở Tra cứu lịch sử, tìm bệnh nhân bằng mã bệnh nhân/mã hồ sơ/họ tên/điện thoại hoặc mã phiếu khám. Kết quả mong đợi: phiếu khám có thể truy vết được toa thuốc và xét nghiệm liên quan.
Kịch bản C – Lịch khám định kỳ
1. Mở Lịch khám định kỳ, chọn hồ sơ hợp lệ và đặt lịch trong tương lai.
2. Thử tạo lịch thứ hai cho cùng hồ sơ trong cùng quý. Kết quả mong đợi: hệ thống từ chối theo giả định giới hạn lịch/quý.
3. Nếu có chức năng hủy lịch, hủy lịch chưa thực hiện rồi thử đặt lại trong cùng quý; lịch đã hủy không được tính là đã sử dụng quyền lợi.
4. Mở Khám bệnh / Phiếu khám, tạo đợt loại KhamDinhKy và chọn lịch hợp lệ của đúng hồ sơ.
5. Hoàn tất lưu. Kết quả mong đợi: lịch được liên kết với MaDT và chuyển thành DaThucHien.
6. Ghi nhận kết quả khám kể cả trường hợp không phát hiện bệnh; tra cứu lại lịch sử để xác minh dữ liệu được lưu.
Kịch bản D – Viện phí và đóng đợt
1. Với đợt DieuTri, mở Viện phí và tạo hóa đơn.
2. Thử đóng đợt khi chưa thanh toán. Kết quả mong đợi: hệ thống từ chối.
3. Ghi nhận thanh toán một phần và thử đóng đợt lần nữa. Kết quả mong đợi: vẫn bị từ chối.
4. Thanh toán phần còn nợ và thực hiện đóng đợt. Kết quả mong đợi: hóa đơn ở trạng thái đã thanh toán, đợt chuyển DaDong.
5. Với đợt KhamDinhKy miễn phí không có hóa đơn, kiểm tra rằng có thể đóng đợt theo quy tắc thiết kế.
7. Chạy kiểm thử quy tắc nghiệp vụ bằng SQL
1. Trong SSMS, mở QuanLyHoSoBenhAn_Project/Database/Tests/SmokeTest_QuyTacNghiepVu.sql.
2. Xác nhận cửa sổ query đang kết nối tới đúng SQL Server và database theo script.
3. Chạy script trên database đồ án/phát triển.
4. Kiểm tra lưới kết quả cuối: cột KetQua cho biết PASS/FAIL; đọc thêm cột chi tiết khi có lỗi.
5. Script sử dụng dữ liệu được đánh dấu kiểm thử và có phần dọn dữ liệu. Đọc script trước khi chạy; không chạy trên dữ liệu bệnh nhân thật.
Các nhóm quy tắc được kiểm tra gồm đăng ký/lệ phí/cấp hồ sơ, giới hạn lịch định kỳ, liên kết lịch với đợt khám, liên kết toa và xét nghiệm với phiếu khám, tra cứu lịch sử, thanh toán đủ và điều kiện đóng đợt. Xem QuanLyHoSoBenhAn_Project/KiemThu/README.md để biết danh sách ca kiểm thử và kết quả mong đợi.
8. Ràng buộc và giả định mô hình
- Một bệnh nhân có một hồ sơ bệnh án chính.
- Một phiếu khám có tối đa một toa thuốc.
- Một đợt điều trị có tối đa một hóa đơn viện phí.
- Một thuốc xuất hiện tối đa một dòng trong cùng một toa.
- Mỗi hồ sơ tối đa một lịch khám định kỳ có hiệu lực trong một quý là giả định thiết kế cần được xác nhận với giảng viên/nghiệp vụ.
- Lệ phí năm và viện phí là hai khoản tách biệt.
- Đợt khám định kỳ có thể không cần hóa đơn; đợt điều trị thông thường cần thanh toán đủ trước khi đóng.
- Các vai trò/phân quyền chi tiết và quy trình bảo vệ dữ liệu y tế cần được bổ sung nếu phát triển ngoài phạm vi bài thực hành.
9. Tài liệu liên quan
- QuanLyHoSoBenhAn_Project/README.md: hướng dẫn kỹ thuật và chi tiết chức năng của project.
- QuanLyHoSoBenhAn_Project/KiemThu/README.md: ma trận ca kiểm thử và kết quả mong đợi.
- QuanLyHoSoBenhAn_Project/Database/Tests/SmokeTest_QuyTacNghiepVu.sql: script kiểm thử SQL.
- 1250080062_PhamHuyHoang_Lab06.docx: báo cáo bài thực hành.
