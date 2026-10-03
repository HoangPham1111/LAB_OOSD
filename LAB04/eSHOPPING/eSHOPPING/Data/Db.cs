using System.Configuration;
using System.Data.SqlClient;

namespace eSHOPPING.Data
{
    public static class Db
    {
        private static readonly string cs =
            ConfigurationManager.ConnectionStrings["eShoppingDb"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(cs);
        }
    }
}