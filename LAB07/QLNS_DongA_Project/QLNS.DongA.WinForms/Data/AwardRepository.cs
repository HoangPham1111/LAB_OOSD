using System;
using System.Data;
using System.Data.SqlClient;

namespace QLNS.DongA.Data
{
    internal sealed class AwardRepository
    {
        public void CreateDraft(string id, DateTime date, string targetType, string employeeId, string departmentId,
            string reason, string content, decimal amount)
        {
            using (SqlConnection connection = ConnectionFactory.Create())
            using (SqlCommand command = new SqlCommand("dbo.sp_LapQuyetDinhKhenThuong", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@MaQDKhenThuong", SqlDbType.NVarChar, 20).Value = id;
                command.Parameters.Add("@NgayLap", SqlDbType.Date).Value = date.Date;
                command.Parameters.Add("@LoaiDoiTuongApDung", SqlDbType.NVarChar, 20).Value = targetType;
                command.Parameters.Add("@MaNhanVien", SqlDbType.NVarChar, 20).Value = String.IsNullOrWhiteSpace(employeeId) ? (object)DBNull.Value : employeeId;
                command.Parameters.Add("@MaPhongBan", SqlDbType.NVarChar, 20).Value = String.IsNullOrWhiteSpace(departmentId) ? (object)DBNull.Value : departmentId;
                command.Parameters.Add("@LyDo", SqlDbType.NVarChar, 500).Value = reason;
                command.Parameters.Add("@NoiDung", SqlDbType.NVarChar, 1000).Value = String.IsNullOrWhiteSpace(content) ? (object)DBNull.Value : content;
                SqlParameter money = command.Parameters.Add("@SoTienKhenThuong", SqlDbType.Decimal);
                money.Precision = 18; money.Scale = 2; money.Value = amount;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Sign(string id, DateTime date)
        {
            using (SqlConnection connection = ConnectionFactory.Create())
            using (SqlCommand command = new SqlCommand("dbo.sp_KyQuyetDinhKhenThuong", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@MaQDKhenThuong", SqlDbType.NVarChar, 20).Value = id;
                command.Parameters.Add("@NgayKy", SqlDbType.Date).Value = date.Date;
                connection.Open(); command.ExecuteNonQuery();
            }
        }

        public void CreatePayout(string payoutId, string awardId, DateTime date, string method)
        {
            using (SqlConnection connection = ConnectionFactory.Create())
            using (SqlCommand command = new SqlCommand("dbo.sp_LapPhieuChiKhenThuong", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@MaPhieuChi", SqlDbType.NVarChar, 20).Value = payoutId;
                command.Parameters.Add("@MaQDKhenThuong", SqlDbType.NVarChar, 20).Value = awardId;
                command.Parameters.Add("@NgayChi", SqlDbType.Date).Value = date.Date;
                command.Parameters.Add("@PhuongThucChi", SqlDbType.NVarChar, 30).Value = method;
                connection.Open(); command.ExecuteNonQuery();
            }
        }
    }
}
