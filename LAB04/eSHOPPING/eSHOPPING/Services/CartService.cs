using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using eSHOPPING.Data;
using eSHOPPING.Models;

namespace eSHOPPING.Services
{
    public class CartService
    {
        public List<CartItem> GetCart(int customerId)
        {
            List<CartItem> list = new List<CartItem>();

            using (SqlConnection cn = Db.GetConnection())
            {
                string sql = @"
                    SELECT CartItemId,
                           CartId,
                           ProductId,
                           ProductName,
                           Quantity,
                           UnitPrice,
                           Amount
                    FROM vw_CartDetail
                    WHERE CustomerId=@CustomerId
                    ORDER BY ProductName";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    cn.Open();

                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new CartItem
                            {
                                CartItemId = Convert.ToInt32(r["CartItemId"]),
                                CartId = Convert.ToInt32(r["CartId"]),
                                ProductId = Convert.ToInt32(r["ProductId"]),
                                ProductName = r["ProductName"].ToString(),
                                Quantity = Convert.ToInt32(r["Quantity"]),
                                UnitPrice = Convert.ToDecimal(r["UnitPrice"])
                            });
                        }
                    }
                }
            }

            return list;
        }

        public void AddItem(int customerId, int productId, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("Số lượng phải lớn hơn 0.");

            using (SqlConnection cn = Db.GetConnection())
            {
                cn.Open();

                int cartId;

                string cartSql = @"
                    SELECT CartId
                    FROM ShoppingCart
                    WHERE CustomerId=@CustomerId";

                using (SqlCommand cmd = new SqlCommand(cartSql, cn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);

                    object result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        string insertCart = @"
                            INSERT INTO ShoppingCart(CustomerId)
                            VALUES(@CustomerId);
                            SELECT SCOPE_IDENTITY();";

                        using (SqlCommand c = new SqlCommand(insertCart, cn))
                        {
                            c.Parameters.AddWithValue("@CustomerId", customerId);
                            cartId = Convert.ToInt32(c.ExecuteScalar());
                        }
                    }
                    else
                    {
                        cartId = Convert.ToInt32(result);
                    }
                }

                decimal price;
                int stock;
                bool available;

                string productSql = @"
                    SELECT Price,StockQuantity,IsAvailable
                    FROM Product
                    WHERE ProductId=@ProductId";

                using (SqlCommand cmd = new SqlCommand(productSql, cn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);

                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (!r.Read())
                            throw new Exception("Không tìm thấy sản phẩm.");

                        price = Convert.ToDecimal(r["Price"]);
                        stock = Convert.ToInt32(r["StockQuantity"]);
                        available = Convert.ToBoolean(r["IsAvailable"]);
                    }
                }

                if (!available)
                    throw new Exception("Sản phẩm hiện không còn bán.");

                if (quantity > stock)
                    throw new Exception("Số lượng sản phẩm vượt quá tồn kho.");

                int oldQuantity = 0;

                string checkSql = @"
                    SELECT Quantity
                    FROM CartItem
                    WHERE CartId=@CartId
                      AND ProductId=@ProductId";

                using (SqlCommand cmd = new SqlCommand(checkSql, cn))
                {
                    cmd.Parameters.AddWithValue("@CartId", cartId);
                    cmd.Parameters.AddWithValue("@ProductId", productId);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                        oldQuantity = Convert.ToInt32(result);
                }

                int newQuantity = oldQuantity + quantity;

                if (newQuantity > stock)
                    throw new Exception("Tổng số lượng trong giỏ vượt quá tồn kho.");

                if (oldQuantity > 0)
                {
                    string sql = @"
                        UPDATE CartItem
                        SET Quantity=@Quantity,
                            UnitPrice=@UnitPrice
                        WHERE CartId=@CartId
                          AND ProductId=@ProductId";

                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@Quantity", newQuantity);
                        cmd.Parameters.AddWithValue("@UnitPrice", price);
                        cmd.Parameters.AddWithValue("@CartId", cartId);
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string sql = @"
                        INSERT INTO CartItem
                        (
                            CartId,
                            ProductId,
                            Quantity,
                            UnitPrice
                        )
                        VALUES
                        (
                            @CartId,
                            @ProductId,
                            @Quantity,
                            @UnitPrice
                        )";

                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@CartId", cartId);
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@UnitPrice", price);
                        cmd.ExecuteNonQuery();
                    }
                }

                UpdateCartTime(cn, cartId);
            }
        }

        public void UpdateItem(int cartItemId, int quantity)
        {
            if (quantity <= 0)
            {
                DeleteItem(cartItemId);
                return;
            }

            using (SqlConnection cn = Db.GetConnection())
            {
                cn.Open();

                int cartId;
                int productId;

                string findSql = @"
                    SELECT CartId,ProductId
                    FROM CartItem
                    WHERE CartItemId=@CartItemId";

                using (SqlCommand cmd = new SqlCommand(findSql, cn))
                {
                    cmd.Parameters.AddWithValue("@CartItemId", cartItemId);

                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (!r.Read())
                            throw new Exception("Không tìm thấy sản phẩm trong giỏ.");

                        cartId = Convert.ToInt32(r["CartId"]);
                        productId = Convert.ToInt32(r["ProductId"]);
                    }
                }

                int stock;

                using (SqlCommand cmd = new SqlCommand(
                    "SELECT StockQuantity FROM Product WHERE ProductId=@ProductId", cn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    stock = Convert.ToInt32(cmd.ExecuteScalar());
                }

                if (quantity > stock)
                    throw new Exception("Số lượng vượt quá tồn kho.");

                string sql = @"
                    UPDATE CartItem
                    SET Quantity=@Quantity
                    WHERE CartItemId=@CartItemId";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@CartItemId", cartItemId);
                    cmd.ExecuteNonQuery();
                }

                UpdateCartTime(cn, cartId);
            }
        }

        public void DeleteItem(int cartItemId)
        {
            using (SqlConnection cn = Db.GetConnection())
            {
                cn.Open();

                int cartId;

                using (SqlCommand cmd = new SqlCommand(
                    "SELECT CartId FROM CartItem WHERE CartItemId=@id", cn))
                {
                    cmd.Parameters.AddWithValue("@id", cartItemId);

                    object result = cmd.ExecuteScalar();

                    if (result == null)
                        return;

                    cartId = Convert.ToInt32(result);
                }

                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM CartItem WHERE CartItemId=@id", cn))
                {
                    cmd.Parameters.AddWithValue("@id", cartItemId);
                    cmd.ExecuteNonQuery();
                }

                UpdateCartTime(cn, cartId);
            }
        }

        private void UpdateCartTime(SqlConnection cn, int cartId)
        {
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE ShoppingCart SET UpdatedAt=GETDATE() WHERE CartId=@id", cn))
            {
                cmd.Parameters.AddWithValue("@id", cartId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}