using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using eSHOPPING.Data;
using eSHOPPING.Models;

namespace eSHOPPING.Services
{
    public class ProductService
    {
        public List<Product> GetAll()
        {
            List<Product> list = new List<Product>();

            using (SqlConnection cn = Db.GetConnection())
            {
                string sql = @"
                    SELECT ProductId,ProductCode,ProductName,
                           Manufacturer,ProductGroupId,Price,
                           StockQuantity,IsAvailable,
                           Description,Specifications,ImageUrl
                    FROM Product
                    ORDER BY ProductName";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();

                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                            list.Add(MapProduct(r));
                    }
                }
            }

            return list;
        }

        public List<Product> Search(string keyword)
        {
            List<Product> list = new List<Product>();

            using (SqlConnection cn = Db.GetConnection())
            {
                string sql = @"
                    SELECT p.ProductId,
                           p.ProductCode,
                           p.ProductName,
                           p.Manufacturer,
                           p.ProductGroupId,
                           p.Price,
                           p.StockQuantity,
                           p.IsAvailable,
                           p.Description,
                           p.Specifications,
                           p.ImageUrl
                    FROM Product p
                    WHERE
                        @Keyword = ''
                        OR p.ProductName LIKE N'%' + @Keyword + N'%'
                        OR p.ProductCode LIKE '%' + @Keyword + '%'
                        OR p.Manufacturer LIKE N'%' + @Keyword + N'%'
                    ORDER BY p.ProductName";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue(
                        "@Keyword",
                        keyword ?? "");

                    cn.Open();

                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                            list.Add(MapProduct(r));
                    }
                }
            }

            return list;
        }

        public Product GetById(int productId)
        {
            using (SqlConnection cn = Db.GetConnection())
            {
                string sql = @"
                    SELECT ProductId,ProductCode,ProductName,
                           Manufacturer,ProductGroupId,Price,
                           StockQuantity,IsAvailable,
                           Description,Specifications,ImageUrl
                    FROM Product
                    WHERE ProductId=@id";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id", productId);
                    cn.Open();

                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                            return MapProduct(r);
                    }
                }
            }

            return null;
        }

        private Product MapProduct(SqlDataReader r)
        {
            return new Product
            {
                ProductId = Convert.ToInt32(r["ProductId"]),
                ProductCode = r["ProductCode"].ToString(),
                ProductName = r["ProductName"].ToString(),
                Manufacturer = r["Manufacturer"] == DBNull.Value
                    ? ""
                    : r["Manufacturer"].ToString(),
                ProductGroupId = Convert.ToInt32(r["ProductGroupId"]),
                Price = Convert.ToDecimal(r["Price"]),
                StockQuantity = Convert.ToInt32(r["StockQuantity"]),
                IsAvailable = Convert.ToBoolean(r["IsAvailable"]),
                Description = r["Description"] == DBNull.Value
                    ? ""
                    : r["Description"].ToString(),
                Specifications = r["Specifications"] == DBNull.Value
                    ? ""
                    : r["Specifications"].ToString(),
                ImageUrl = r["ImageUrl"] == DBNull.Value
                    ? ""
                    : r["ImageUrl"].ToString()
            };
        }
    }
}