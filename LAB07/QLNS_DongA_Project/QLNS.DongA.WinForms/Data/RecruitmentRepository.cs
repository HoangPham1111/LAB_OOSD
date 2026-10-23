using System;
using System.Data;
using System.Data.SqlClient;

namespace QLNS.DongA.Data
{
    internal sealed class RecruitmentRepository
    {
        public void CreateTrialDecision(string maQD, string maNhanVien, string maPhongBan,
            DateTime ngayKy, DateTime ngayBatDau, DateTime ngayKetThuc, decimal luongThuViec,
            string dieuKhoanNhaTuyenDung, string dieuKhoanUngVien)
        {
            if (ngayKetThuc.Date < ngayBatDau.Date) throw new InvalidOperationException("Ngày kết thúc thử việc phải bằng hoặc sau ngày bắt đầu.");
            if (ngayKy.Date > ngayBatDau.Date) throw new InvalidOperationException("Ngày ký quyết định không được sau ngày bắt đầu thử việc.");
            if (luongThuViec < 0) throw new InvalidOperationException("Lương thử việc không được âm.");

            using (SqlConnection connection = ConnectionFactory.Create())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand(@"
                            IF NOT EXISTS (SELECT 1 FROM dbo.NhanVien WHERE MaNhanVien = @MaNhanVien)
                                THROW 52001, N'Không tìm thấy nhân viên.', 1;
                            IF EXISTS (SELECT 1 FROM dbo.NhanVien WHERE MaNhanVien = @MaNhanVien AND TrangThaiNhanVien = N'DangLam')
                                THROW 52004, N'Nhân viên đang làm việc; không thể lập lại quyết định tiếp nhận thử việc trong luồng này.', 1;
                            IF EXISTS (SELECT 1 FROM dbo.HopDongLaoDong WHERE MaNhanVien = @MaNhanVien AND TrangThai = N'HieuLuc')
                                THROW 52005, N'Nhân viên đang có hợp đồng hiệu lực.', 1;
                            IF NOT EXISTS (SELECT 1 FROM dbo.PhongBan WHERE MaPhongBan = @MaPhongBan AND TrangThai = 1)
                                THROW 52002, N'Phòng ban không tồn tại hoặc đã ngừng hoạt động.', 1;
                            IF EXISTS (SELECT 1 FROM dbo.QuyetDinhTuyenDung WHERE MaQDTuyenDung = @MaQDTuyenDung)
                                THROW 52003, N'Mã quyết định tuyển dụng đã tồn tại.', 1;
                            INSERT dbo.QuyetDinhTuyenDung
                                (MaQDTuyenDung, MaNhanVien, MaPhongBan, NgayKy, NgayBatDauThuViec, NgayKetThucThuViec,
                                 LuongThuViec, DieuKhoanNhaTuyenDung, DieuKhoanUngVien, TrangThai)
                            VALUES
                                (@MaQDTuyenDung, @MaNhanVien, @MaPhongBan, @NgayKy, @NgayBatDauThuViec, @NgayKetThucThuViec,
                                 @LuongThuViec, @DieuKhoanNhaTuyenDung, @DieuKhoanUngVien, N'DangThuViec');
                            UPDATE dbo.NhanVien
                               SET MaPhongBan = @MaPhongBan, TrangThaiNhanVien = N'ThuViec',
                                   NgayTiepNhan = COALESCE(NgayTiepNhan, @NgayBatDauThuViec)
                             WHERE MaNhanVien = @MaNhanVien;", connection, transaction))
                        {
                            command.Parameters.Add("@MaQDTuyenDung", SqlDbType.NVarChar, 20).Value = maQD;
                            command.Parameters.Add("@MaNhanVien", SqlDbType.NVarChar, 20).Value = maNhanVien;
                            command.Parameters.Add("@MaPhongBan", SqlDbType.NVarChar, 20).Value = maPhongBan;
                            command.Parameters.Add("@NgayKy", SqlDbType.Date).Value = ngayKy.Date;
                            command.Parameters.Add("@NgayBatDauThuViec", SqlDbType.Date).Value = ngayBatDau.Date;
                            command.Parameters.Add("@NgayKetThucThuViec", SqlDbType.Date).Value = ngayKetThuc.Date;
                            command.Parameters.Add("@LuongThuViec", SqlDbType.Decimal).Value = luongThuViec;
                            command.Parameters["@LuongThuViec"].Precision = 18;
                            command.Parameters["@LuongThuViec"].Scale = 2;
                            command.Parameters.Add("@DieuKhoanNhaTuyenDung", SqlDbType.NVarChar, 1000).Value = dieuKhoanNhaTuyenDung;
                            command.Parameters.Add("@DieuKhoanUngVien", SqlDbType.NVarChar, 1000).Value = dieuKhoanUngVien;
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch { transaction.Rollback(); throw; }
                }
            }
        }

        public string CreateFormalContract(string maHopDong, string maQDTuyenDung, string loaiHopDong,
            DateTime ngayKy, DateTime ngayBatDau, DateTime? ngayKetThuc)
        {
            if (loaiHopDong != "DaiHan" && loaiHopDong != "NganHan") throw new InvalidOperationException("Loại hợp đồng không hợp lệ.");
            if (ngayBatDau.Date < ngayKy.Date) throw new InvalidOperationException("Ngày bắt đầu hợp đồng không được trước ngày ký.");
            if (ngayKetThuc.HasValue && ngayKetThuc.Value.Date < ngayBatDau.Date) throw new InvalidOperationException("Ngày kết thúc hợp đồng phải bằng hoặc sau ngày bắt đầu.");

            using (SqlConnection connection = ConnectionFactory.Create())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string maNhanVien;
                        DateTime ngayKetThucThuViec;
                        string trangThai;
                        using (SqlCommand check = new SqlCommand(@"
                            SELECT MaNhanVien, NgayKetThucThuViec, TrangThai
                            FROM dbo.QuyetDinhTuyenDung WITH (UPDLOCK, HOLDLOCK)
                            WHERE MaQDTuyenDung = @MaQDTuyenDung", connection, transaction))
                        {
                            check.Parameters.Add("@MaQDTuyenDung", SqlDbType.NVarChar, 20).Value = maQDTuyenDung;
                            using (SqlDataReader reader = check.ExecuteReader())
                            {
                                if (!reader.Read()) throw new InvalidOperationException("Không tìm thấy quyết định tuyển dụng.");
                                maNhanVien = reader.GetString(0);
                                ngayKetThucThuViec = reader.GetDateTime(1);
                                trangThai = reader.GetString(2);
                            }
                        }
                        if (trangThai != "DangThuViec" && trangThai != "DaKetThucThuViec")
                            throw new InvalidOperationException("Quyết định không ở trạng thái cho phép ký hợp đồng.");
                        if (ngayKy.Date < ngayKetThucThuViec.Date)
                            throw new InvalidOperationException("Ngày ký hợp đồng phải từ ngày kết thúc thử việc trở đi.");

                        using (SqlCommand insert = new SqlCommand(@"
                            INSERT dbo.HopDongLaoDong (MaHopDong, MaNhanVien, LoaiHopDong, NgayKy, NgayBatDau, NgayKetThuc, TrangThai)
                            VALUES (@MaHopDong, @MaNhanVien, @LoaiHopDong, @NgayKy, @NgayBatDau, @NgayKetThuc, N'HieuLuc');
                            UPDATE dbo.QuyetDinhTuyenDung SET TrangThai = N'DaKyHopDong' WHERE MaQDTuyenDung = @MaQDTuyenDung;
                            UPDATE dbo.NhanVien SET TrangThaiNhanVien = N'DangLam' WHERE MaNhanVien = @MaNhanVien;", connection, transaction))
                        {
                            insert.Parameters.Add("@MaHopDong", SqlDbType.NVarChar, 20).Value = maHopDong;
                            insert.Parameters.Add("@MaNhanVien", SqlDbType.NVarChar, 20).Value = maNhanVien;
                            insert.Parameters.Add("@LoaiHopDong", SqlDbType.NVarChar, 20).Value = loaiHopDong;
                            insert.Parameters.Add("@NgayKy", SqlDbType.Date).Value = ngayKy.Date;
                            insert.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = ngayBatDau.Date;
                            SqlParameter end = insert.Parameters.Add("@NgayKetThuc", SqlDbType.Date);
                            end.Value = ngayKetThuc.HasValue ? (object)ngayKetThuc.Value.Date : DBNull.Value;
                            insert.Parameters.Add("@MaQDTuyenDung", SqlDbType.NVarChar, 20).Value = maQDTuyenDung;
                            insert.ExecuteNonQuery();
                        }

                        if (loaiHopDong == "DaiHan")
                        {
                            AddInsurancePlaceholder(connection, transaction, "HSB" + Guid.NewGuid().ToString("N").Substring(0, 15), maNhanVien, maHopDong, "BHYT", ngayBatDau.Date);
                            AddInsurancePlaceholder(connection, transaction, "HSB" + Guid.NewGuid().ToString("N").Substring(0, 15), maNhanVien, maHopDong, "BHXH", ngayBatDau.Date);
                        }
                        transaction.Commit();
                        return maNhanVien;
                    }
                    catch { transaction.Rollback(); throw; }
                }
            }
        }

        private static void AddInsurancePlaceholder(SqlConnection connection, SqlTransaction transaction,
            string maHoSo, string maNhanVien, string maHopDong, string loai, DateTime ngayBatDau)
        {
            using (SqlCommand command = new SqlCommand(@"
                INSERT dbo.HoSoBaoHiem (MaHoSoBaoHiem, MaNhanVien, MaHopDong, LoaiBaoHiem, MucDong, NgayLap, NgayBatDau, TrangThai)
                VALUES (@MaHoSo, @MaNhanVien, @MaHopDong, @Loai, 0, CONVERT(date, GETDATE()), @NgayBatDau, N'ChoCapNhatMucDong')", connection, transaction))
            {
                command.Parameters.Add("@MaHoSo", SqlDbType.NVarChar, 20).Value = maHoSo;
                command.Parameters.Add("@MaNhanVien", SqlDbType.NVarChar, 20).Value = maNhanVien;
                command.Parameters.Add("@MaHopDong", SqlDbType.NVarChar, 20).Value = maHopDong;
                command.Parameters.Add("@Loai", SqlDbType.NVarChar, 10).Value = loai;
                command.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = ngayBatDau;
                command.ExecuteNonQuery();
            }
        }
    }
}
