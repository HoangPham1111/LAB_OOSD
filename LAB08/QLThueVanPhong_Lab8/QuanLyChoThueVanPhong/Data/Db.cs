using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyChoThueVanPhong.Data
{
    public static class Db
    {
        private static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["QLVanPhongDB"].ConnectionString; }
        }
        public static SqlConnection Open()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }
        public static SqlParameter P(string name, object value)
        {
            return new SqlParameter(name, value ?? DBNull.Value);
        }
        public static DataTable Query(string sql, params SqlParameter[] ps)
        {
            using (var cn = Open())
            using (var cmd = new SqlCommand(sql, cn))
            using (var da = new SqlDataAdapter(cmd))
            {
                if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
                var result = new DataTable();
                da.Fill(result);
                return result;
            }
        }
        public static int Execute(string sql, params SqlParameter[] ps)
        {
            using (var cn = Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
                return cmd.ExecuteNonQuery();
            }
        }
        public static object Scalar(string sql, params SqlParameter[] ps)
        {
            using (var cn = Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
                return cmd.ExecuteScalar();
            }
        }
        public static int StoredWithOutput(string storedProcedure, string outputName, params SqlParameter[] parameters)
        {
            using (var cn = Open())
            using (var cmd = new SqlCommand(storedProcedure, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 60;
                cmd.Parameters.AddRange(parameters);
                var output = new SqlParameter(outputName, SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(output);
                cmd.ExecuteNonQuery();
                return Convert.ToInt32(output.Value);
            }
        }
        public static void Stored(string storedProcedure, params SqlParameter[] parameters)
        {
            using (var cn = Open())
            using (var cmd = new SqlCommand(storedProcedure, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 60;
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
