using System;
using System.Data;
using System.Data.SqlClient;

namespace QLNS.DongA.Data
{
    internal sealed class PayrollRepository
    {
        public string Generate(int month, int year, decimal standardHours)
        {
            using (SqlConnection connection = ConnectionFactory.Create())
            using (SqlCommand command = new SqlCommand("dbo.sp_TaoBangLuongThang", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Thang", SqlDbType.TinyInt).Value = month;
                command.Parameters.Add("@Nam", SqlDbType.SmallInt).Value = year;
                SqlParameter hours = command.Parameters.Add("@GioChuanThang", SqlDbType.Decimal);
                hours.Precision = 7; hours.Scale = 2; hours.Value = standardHours;
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) return Convert.ToString(reader["MaBangLuong"]);
                }
            }
            return "BL-" + (year * 100 + month).ToString();
        }

        public DataTable GetDetails(string payrollId)
        {
            using (SqlConnection connection = ConnectionFactory.Create())
            using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.vw_BangLuongChiTiet WHERE MaBangLuong = @id ORDER BY MaNhanVien", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.Add("@id", SqlDbType.NVarChar, 20).Value = payrollId;
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }
    }
}
