using System;
using System.Data.SqlClient;
using eSHOPPING.Data;
using eSHOPPING.Models;

namespace eSHOPPING.Services
{
    public class CustomerService
    {
        public Customer Login(string username, string password)
        {
            using (SqlConnection cn = Db.GetConnection())
            {
                string sql = @"
                    SELECT CustomerId,FullName,Phone,Username,
                           PasswordHash,Email,Address
                    FROM Customer
                    WHERE Username=@u
                      AND PasswordHash=@p
                      AND IsActive=1";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", password);

                    cn.Open();

                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (!r.Read())
                            return null;

                        return new Customer
                        {
                            CustomerId = Convert.ToInt32(r["CustomerId"]),
                            FullName = r["FullName"].ToString(),
                            Phone = r["Phone"].ToString(),
                            Username = r["Username"].ToString(),
                            PasswordHash = r["PasswordHash"].ToString(),
                            Email = r["Email"] == DBNull.Value ? "" : r["Email"].ToString(),
                            Address = r["Address"] == DBNull.Value ? "" : r["Address"].ToString()
                        };
                    }
                }
            }
        }

        public bool Register(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            using (SqlConnection cn = Db.GetConnection())
            {
                string sql = @"
                    INSERT INTO Customer
                    (
                        FullName,
                        Address,
                        Phone,
                        Username,
                        PasswordHash,
                        Email
                    )
                    VALUES
                    (
                        @FullName,
                        @Address,
                        @Phone,
                        @Username,
                        @PasswordHash,
                        @Email
                    )";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@FullName", customer.FullName);
                    cmd.Parameters.AddWithValue("@Address",
                        string.IsNullOrWhiteSpace(customer.Address)
                        ? (object)DBNull.Value
                        : customer.Address);

                    cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                    cmd.Parameters.AddWithValue("@Username", customer.Username);
                    cmd.Parameters.AddWithValue("@PasswordHash", customer.PasswordHash);
                    cmd.Parameters.AddWithValue("@Email",
                        string.IsNullOrWhiteSpace(customer.Email)
                        ? (object)DBNull.Value
                        : customer.Email);

                    cn.Open();

                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627 || ex.Number == 2601)
                            throw new Exception("Tên đăng nhập đã tồn tại.");

                        throw;
                    }
                }
            }
        }
    }
}