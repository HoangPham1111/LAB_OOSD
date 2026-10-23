using System;
using System.Configuration;
using System.Data.SqlClient;

namespace QLNS.DongA.Data
{
    internal static class ConnectionFactory
    {
        public static SqlConnection Create()
        {
            ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings["QLNS_DongA"];
            if (setting == null || String.IsNullOrWhiteSpace(setting.ConnectionString))
                throw new InvalidOperationException("Chua cau hinh connection string QLNS_DongA trong App.config.");
            return new SqlConnection(setting.ConnectionString);
        }

        public static decimal StandardMonthlyHours
        {
            get
            {
                string raw = ConfigurationManager.AppSettings["GioChuanThang"];
                decimal value;
                if (!Decimal.TryParse(raw, out value) || value <= 0) return 208m;
                return value;
            }
        }
    }
}
