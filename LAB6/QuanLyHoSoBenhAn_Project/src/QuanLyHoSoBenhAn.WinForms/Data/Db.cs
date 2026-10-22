using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyHoSoBenhAn.Data
{
    internal static class Db
    {
        private static string ConnectionString
        {
            get
            {
                var item = ConfigurationManager.ConnectionStrings["QuanLyHoSoBenhAn"];
                if (item == null || string.IsNullOrWhiteSpace(item.ConnectionString))
                    throw new InvalidOperationException("Chưa cấu hình connection string QuanLyHoSoBenhAn trong App.config.");
                return item.ConnectionString;
            }
        }

        public static SqlConnection CreateConnection() { return new SqlConnection(ConnectionString); }

        public static DataTable Query(string sql, params SqlParameter[] parameters)
        {
            using (var conn = CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                var table = new DataTable();
                da.Fill(table);
                return table;
            }
        }

        public static int Execute(string sql, params SqlParameter[] parameters)
        {
            using (var conn = CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static object Scalar(string sql, params SqlParameter[] parameters)
        {
            using (var conn = CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }

        public static SqlParameter P(string name, SqlDbType type, object value, int size = 0)
        {
            var p = size > 0 ? new SqlParameter(name, type, size) : new SqlParameter(name, type);
            p.Value = value ?? DBNull.Value;
            return p;
        }

        public static SqlParameter DecimalP(string name, object value, byte precision, byte scale)
        {
            var p = new SqlParameter(name, SqlDbType.Decimal) { Precision = precision, Scale = scale, Value = value ?? DBNull.Value };
            return p;
        }

        public static DataTable ComboQuery(string sql)
        {
            var table = Query(sql);
            if (!table.Columns.Contains("DisplayText"))
                throw new InvalidOperationException("Truy vấn ComboBox cần trả về cột DisplayText.");
            return table;
        }
    }
}
