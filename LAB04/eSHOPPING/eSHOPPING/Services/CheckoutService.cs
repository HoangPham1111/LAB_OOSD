using System;
using System.Data;
using System.Data.SqlClient;
using eSHOPPING.Data;
using eSHOPPING.Models;

namespace eSHOPPING.Services
{
    public class CheckoutResult
    {
        public string OrderCode { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }

    public class CheckoutService
    {
        public CheckoutResult Checkout(CheckoutRequest request)
        {
            if (request == null)
                throw new Exception("Thông tin đặt hàng không hợp lệ.");

            if (request.CustomerId <= 0)
                throw new Exception("Khách hàng chưa đăng nhập.");

            if (request.RecipientId <= 0)
                throw new Exception("Chưa chọn người nhận.");

            if (request.DeliveryTypeId <= 0)
                throw new Exception("Chưa chọn loại giao hàng.");

            if (request.DeliveryAreaId <= 0)
                throw new Exception("Chưa chọn khu vực giao hàng.");

            ValidateCard(request);

            using (SqlConnection cn = Db.GetConnection())
            {
                cn.Open();

                SqlTransaction tx = cn.BeginTransaction(
                    IsolationLevel.Serializable);

                try
                {
                    decimal subTotal = 0;

                    DataTable cart = new DataTable();

                    string cartSql = @"
                        SELECT
                            ci.CartItemId,
                            ci.ProductId,
                            ci.Quantity,
                            ci.UnitPrice,
                            ci.Quantity * ci.UnitPrice AS Amount
                        FROM ShoppingCart sc
                        INNER JOIN CartItem ci
                            ON sc.CartId=ci.CartId
                        WHERE sc.CustomerId=@CustomerId";

                    using (SqlCommand cmd = new SqlCommand(cartSql, cn, tx))
                    {
                        cmd.Parameters.AddWithValue(
                            "@CustomerId",
                            request.CustomerId);

                        using (SqlDataAdapter da =
                            new SqlDataAdapter(cmd))
                        {
                            da.Fill(cart);
                        }
                    }

                    if (cart.Rows.Count == 0)
                        throw new Exception("Giỏ hàng đang trống.");

                    foreach (DataRow row in cart.Rows)
                    {
                        int productId = Convert.ToInt32(row["ProductId"]);
                        int quantity = Convert.ToInt32(row["Quantity"]);

                        decimal price = GetCurrentPrice(
                            cn,
                            tx,
                            productId);

                        int stock = GetStock(
                            cn,
                            tx,
                            productId);

                        if (quantity > stock)
                            throw new Exception(
                                "Sản phẩm không đủ số lượng tồn kho.");

                        subTotal += price * quantity;
                    }

                    decimal shippingFee = CalculateShippingFee(
                        cn,
                        tx,
                        subTotal,
                        request.DeliveryTypeId,
                        request.DeliveryAreaId);

                    decimal total = subTotal + shippingFee;

                    string orderCode =
                        "ORD" +
                        DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    string insertOrder = @"
                        INSERT INTO Orders
                        (
                            OrderCode,
                            CustomerId,
                            RecipientId,
                            DeliveryTypeId,
                            DeliveryAreaId,
                            SubTotal,
                            ShippingFee,
                            TotalAmount,
                            OrderStatus
                        )
                        VALUES
                        (
                            @OrderCode,
                            @CustomerId,
                            @RecipientId,
                            @DeliveryTypeId,
                            @DeliveryAreaId,
                            @SubTotal,
                            @ShippingFee,
                            @TotalAmount,
                            N'Đã thanh toán'
                        );

                        SELECT SCOPE_IDENTITY();";

                    int orderId;

                    using (SqlCommand cmd =
                        new SqlCommand(insertOrder, cn, tx))
                    {
                        cmd.Parameters.AddWithValue(
                            "@OrderCode", orderCode);
                        cmd.Parameters.AddWithValue(
                            "@CustomerId", request.CustomerId);
                        cmd.Parameters.AddWithValue(
                            "@RecipientId", request.RecipientId);
                        cmd.Parameters.AddWithValue(
                            "@DeliveryTypeId", request.DeliveryTypeId);
                        cmd.Parameters.AddWithValue(
                            "@DeliveryAreaId", request.DeliveryAreaId);
                        cmd.Parameters.AddWithValue(
                            "@SubTotal", subTotal);
                        cmd.Parameters.AddWithValue(
                            "@ShippingFee", shippingFee);
                        cmd.Parameters.AddWithValue(
                            "@TotalAmount", total);

                        orderId = Convert.ToInt32(
                            cmd.ExecuteScalar());
                    }

                    foreach (DataRow row in cart.Rows)
                    {
                        int productId =
                            Convert.ToInt32(row["ProductId"]);

                        int quantity =
                            Convert.ToInt32(row["Quantity"]);

                        decimal price =
                            GetCurrentPrice(
                                cn,
                                tx,
                                productId);

                        string itemSql = @"
                            INSERT INTO OrderItem
                            (
                                OrderId,
                                ProductId,
                                Quantity,
                                UnitPrice
                            )
                            VALUES
                            (
                                @OrderId,
                                @ProductId,
                                @Quantity,
                                @UnitPrice
                            )";

                        using (SqlCommand cmd =
                            new SqlCommand(itemSql, cn, tx))
                        {
                            cmd.Parameters.AddWithValue(
                                "@OrderId", orderId);
                            cmd.Parameters.AddWithValue(
                                "@ProductId", productId);
                            cmd.Parameters.AddWithValue(
                                "@Quantity", quantity);
                            cmd.Parameters.AddWithValue(
                                "@UnitPrice", price);

                            cmd.ExecuteNonQuery();
                        }

                        string updateStock = @"
                            UPDATE Product
                            SET StockQuantity =
                                StockQuantity - @Quantity
                            WHERE ProductId=@ProductId";

                        using (SqlCommand cmd =
                            new SqlCommand(updateStock, cn, tx))
                        {
                            cmd.Parameters.AddWithValue(
                                "@Quantity", quantity);
                            cmd.Parameters.AddWithValue(
                                "@ProductId", productId);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    string cardLast4 =
                        request.CardNumber.Substring(
                            request.CardNumber.Length - 4);

                    string paymentSql = @"
                        INSERT INTO Payment
                        (
                            OrderId,
                            CardType,
                            CardLast4,
                            CardHolderName,
                            ExpiryMonth,
                            ExpiryYear,
                            Amount,
                            PaymentStatus,
                            TransactionCode,
                            PaymentDate
                        )
                        VALUES
                        (
                            @OrderId,
                            @CardType,
                            @CardLast4,
                            @CardHolderName,
                            @ExpiryMonth,
                            @ExpiryYear,
                            @Amount,
                            N'Thành công',
                            @TransactionCode,
                            GETDATE()
                        )";

                    using (SqlCommand cmd =
                        new SqlCommand(paymentSql, cn, tx))
                    {
                        cmd.Parameters.AddWithValue(
                            "@OrderId", orderId);
                        cmd.Parameters.AddWithValue(
                            "@CardType", request.CardType);
                        cmd.Parameters.AddWithValue(
                            "@CardLast4", cardLast4);
                        cmd.Parameters.AddWithValue(
                            "@CardHolderName",
                            request.CardHolderName);
                        cmd.Parameters.AddWithValue(
                            "@ExpiryMonth",
                            request.ExpiryMonth);
                        cmd.Parameters.AddWithValue(
                            "@ExpiryYear",
                            request.ExpiryYear);
                        cmd.Parameters.AddWithValue(
                            "@Amount", total);
                        cmd.Parameters.AddWithValue(
                            "@TransactionCode",
                            "TXN" +
                            DateTime.Now.ToString("yyyyMMddHHmmssfff"));

                        cmd.ExecuteNonQuery();
                    }

                    string clearCart = @"
                        DELETE ci
                        FROM CartItem ci
                        INNER JOIN ShoppingCart sc
                            ON ci.CartId=sc.CartId
                        WHERE sc.CustomerId=@CustomerId";

                    using (SqlCommand cmd =
                        new SqlCommand(clearCart, cn, tx))
                    {
                        cmd.Parameters.AddWithValue(
                            "@CustomerId",
                            request.CustomerId);

                        cmd.ExecuteNonQuery();
                    }

                    tx.Commit();

                    return new CheckoutResult
                    {
                        OrderCode = orderCode,
                        SubTotal = subTotal,
                        ShippingFee = shippingFee,
                        TotalAmount = total,
                        Status = "Đã thanh toán"
                    };
                }
                catch
                {
                    try
                    {
                        tx.Rollback();
                    }
                    catch
                    {
                    }

                    throw;
                }
            }
        }

        private decimal CalculateShippingFee(
            SqlConnection cn,
            SqlTransaction tx,
            decimal subTotal,
            int deliveryTypeId,
            int deliveryAreaId)
        {
            using (SqlCommand cmd =
                new SqlCommand(
                    "sp_CalculateShippingFee",
                    cn,
                    tx))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@SubTotal",
                    subTotal);

                cmd.Parameters.AddWithValue(
                    "@DeliveryTypeId",
                    deliveryTypeId);

                cmd.Parameters.AddWithValue(
                    "@DeliveryAreaId",
                    deliveryAreaId);

                SqlParameter output =
                    new SqlParameter(
                        "@ShippingFee",
                        SqlDbType.Decimal);

                output.Precision = 18;
                output.Scale = 2;
                output.Direction =
                    ParameterDirection.Output;

                cmd.Parameters.Add(output);

                cmd.ExecuteNonQuery();

                return Convert.ToDecimal(output.Value);
            }
        }

        private decimal GetCurrentPrice(
            SqlConnection cn,
            SqlTransaction tx,
            int productId)
        {
            string sql = @"
                SELECT Price
                FROM Product
                WHERE ProductId=@ProductId";

            using (SqlCommand cmd =
                new SqlCommand(sql, cn, tx))
            {
                cmd.Parameters.AddWithValue(
                    "@ProductId",
                    productId);

                object result = cmd.ExecuteScalar();

                if (result == null)
                    throw new Exception(
                        "Không tìm thấy sản phẩm.");

                return Convert.ToDecimal(result);
            }
        }

        private int GetStock(
            SqlConnection cn,
            SqlTransaction tx,
            int productId)
        {
            string sql = @"
                SELECT StockQuantity
                FROM Product
                WHERE ProductId=@ProductId";

            using (SqlCommand cmd =
                new SqlCommand(sql, cn, tx))
            {
                cmd.Parameters.AddWithValue(
                    "@ProductId",
                    productId);

                object result = cmd.ExecuteScalar();

                if (result == null)
                    throw new Exception(
                        "Không tìm thấy sản phẩm.");

                return Convert.ToInt32(result);
            }
        }

        private void ValidateCard(CheckoutRequest request)
        {
            string number =
                (request.CardNumber ?? "").Trim()
                .Replace(" ", "");

            string csv =
                (request.CSV ?? "").Trim();

            if (request.CardType == "American Express")
            {
                if (number.Length != 15 ||
                    csv.Length != 4)
                {
                    throw new Exception(
                        "American Express phải có 15 số và CSV 4 số.");
                }
            }
            else
            {
                if (number.Length != 16 ||
                    csv.Length != 3)
                {
                    throw new Exception(
                        "VISA/MasterCard/Discover phải có 16 số và CSV 3 số.");
                }
            }

            if (!long.TryParse(number, out _))
                throw new Exception(
                    "Số thẻ chỉ được chứa chữ số.");

            if (!int.TryParse(csv, out _))
                throw new Exception(
                    "CSV chỉ được chứa chữ số.");

            if (request.ExpiryMonth < 1 ||
                request.ExpiryMonth > 12)
                throw new Exception(
                    "Tháng hết hạn không hợp lệ.");

            if (request.ExpiryYear < DateTime.Now.Year)
                throw new Exception(
                    "Thẻ đã hết hạn.");
        }
    }
}