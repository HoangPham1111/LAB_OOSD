
GO
CREATE DATABASE eSHOPPINGDB;
GO
USE eSHOPPINGDB;
GO

CREATE TABLE Customer(
 CustomerId INT IDENTITY PRIMARY KEY,
 FullName NVARCHAR(100) NOT NULL,
 BirthDate DATE NULL,
 IdentityNo VARCHAR(30) NULL,
 Address NVARCHAR(255) NULL,
 Phone VARCHAR(20) NOT NULL,
 Username VARCHAR(50) NOT NULL UNIQUE,
 PasswordHash VARCHAR(255) NOT NULL,
 Email VARCHAR(100) NULL,
 CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
 IsActive BIT NOT NULL DEFAULT 1,
 CONSTRAINT CK_Customer_Phone CHECK(LEN(Phone)>=9)
);
GO
CREATE TABLE ProductGroup(
 ProductGroupId INT IDENTITY PRIMARY KEY,
 GroupName NVARCHAR(100) NOT NULL UNIQUE,
 Description NVARCHAR(500) NULL
);
GO
CREATE TABLE Product(
 ProductId INT IDENTITY PRIMARY KEY,
 ProductCode VARCHAR(30) NOT NULL UNIQUE,
 ProductName NVARCHAR(200) NOT NULL,
 Manufacturer NVARCHAR(100) NULL,
 ProductGroupId INT NOT NULL,
 ImageUrl VARCHAR(500) NULL,
 Description NVARCHAR(1000) NULL,
 Specifications NVARCHAR(1000) NULL,
 Price DECIMAL(18,2) NOT NULL,
 StockQuantity INT NOT NULL DEFAULT 0,
 IsAvailable BIT NOT NULL DEFAULT 1,
 CONSTRAINT FK_Product_ProductGroup FOREIGN KEY(ProductGroupId) REFERENCES ProductGroup(ProductGroupId),
 CONSTRAINT CK_Product_Price CHECK(Price>=0),
 CONSTRAINT CK_Product_Stock CHECK(StockQuantity>=0)
);
GO
CREATE TABLE ShoppingCart(
 CartId INT IDENTITY PRIMARY KEY,
 CustomerId INT NOT NULL UNIQUE,
 CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
 UpdatedAt DATETIME NULL,
 CONSTRAINT FK_ShoppingCart_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(CustomerId)
);
GO
CREATE TABLE CartItem(
 CartItemId INT IDENTITY PRIMARY KEY,
 CartId INT NOT NULL,
 ProductId INT NOT NULL,
 Quantity INT NOT NULL,
 UnitPrice DECIMAL(18,2) NOT NULL,
 CONSTRAINT FK_CartItem_Cart FOREIGN KEY(CartId) REFERENCES ShoppingCart(CartId),
 CONSTRAINT FK_CartItem_Product FOREIGN KEY(ProductId) REFERENCES Product(ProductId),
 CONSTRAINT UQ_CartItem UNIQUE(CartId,ProductId),
 CONSTRAINT CK_CartItem_Quantity CHECK(Quantity>0),
 CONSTRAINT CK_CartItem_UnitPrice CHECK(UnitPrice>=0)
);
GO
CREATE TABLE DeliveryType(
 DeliveryTypeId INT IDENTITY PRIMARY KEY,
 DeliveryTypeName NVARCHAR(100) NOT NULL UNIQUE,
 Description NVARCHAR(500) NULL
);
GO
CREATE TABLE DeliveryArea(
 DeliveryAreaId INT IDENTITY PRIMARY KEY,
 AreaName NVARCHAR(100) NOT NULL UNIQUE,
 Description NVARCHAR(500) NULL
);
GO
CREATE TABLE DeliveryRate(
 DeliveryRateId INT IDENTITY PRIMARY KEY,
 DeliveryTypeId INT NOT NULL,
 DeliveryAreaId INT NOT NULL,
 Fee DECIMAL(18,2) NOT NULL,
 CONSTRAINT FK_DeliveryRate_Type FOREIGN KEY(DeliveryTypeId) REFERENCES DeliveryType(DeliveryTypeId),
 CONSTRAINT FK_DeliveryRate_Area FOREIGN KEY(DeliveryAreaId) REFERENCES DeliveryArea(DeliveryAreaId),
 CONSTRAINT UQ_DeliveryRate UNIQUE(DeliveryTypeId,DeliveryAreaId),
 CONSTRAINT CK_DeliveryRate_Fee CHECK(Fee>=0)
);
GO
CREATE TABLE Recipient(
 RecipientId INT IDENTITY PRIMARY KEY,
 FullName NVARCHAR(100) NOT NULL,
 Address NVARCHAR(255) NOT NULL,
 Phone VARCHAR(20) NOT NULL,
 CONSTRAINT CK_Recipient_Phone CHECK(LEN(Phone)>=9)
);
GO
CREATE TABLE Orders(
 OrderId INT IDENTITY PRIMARY KEY,
 OrderCode VARCHAR(30) NOT NULL UNIQUE,
 CustomerId INT NOT NULL,
 RecipientId INT NOT NULL,
 DeliveryTypeId INT NOT NULL,
 DeliveryAreaId INT NOT NULL,
 OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
 SubTotal DECIMAL(18,2) NOT NULL,
 ShippingFee DECIMAL(18,2) NOT NULL DEFAULT 0,
 TotalAmount DECIMAL(18,2) NOT NULL,
 OrderStatus NVARCHAR(50) NOT NULL DEFAULT N'Chờ xác nhận',
 CONSTRAINT FK_Orders_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(CustomerId),
 CONSTRAINT FK_Orders_Recipient FOREIGN KEY(RecipientId) REFERENCES Recipient(RecipientId),
 CONSTRAINT FK_Orders_DeliveryType FOREIGN KEY(DeliveryTypeId) REFERENCES DeliveryType(DeliveryTypeId),
 CONSTRAINT FK_Orders_DeliveryArea FOREIGN KEY(DeliveryAreaId) REFERENCES DeliveryArea(DeliveryAreaId),
 CONSTRAINT CK_Orders_SubTotal CHECK(SubTotal>=0),
 CONSTRAINT CK_Orders_ShippingFee CHECK(ShippingFee>=0),
 CONSTRAINT CK_Orders_Total CHECK(TotalAmount>=0),
 CONSTRAINT CK_Orders_Status CHECK(OrderStatus IN(N'Chờ xác nhận',N'Chờ thanh toán',N'Đang xử lý thanh toán',N'Đã thanh toán',N'Đang giao hàng',N'Đã giao hàng',N'Đã hủy'))
);
GO
CREATE TABLE OrderItem(
 OrderItemId INT IDENTITY PRIMARY KEY,
 OrderId INT NOT NULL,
 ProductId INT NOT NULL,
 Quantity INT NOT NULL,
 UnitPrice DECIMAL(18,2) NOT NULL,
 LineTotal AS(Quantity*UnitPrice) PERSISTED,
 CONSTRAINT FK_OrderItem_Order FOREIGN KEY(OrderId) REFERENCES Orders(OrderId),
 CONSTRAINT FK_OrderItem_Product FOREIGN KEY(ProductId) REFERENCES Product(ProductId),
 CONSTRAINT UQ_OrderItem UNIQUE(OrderId,ProductId),
 CONSTRAINT CK_OrderItem_Quantity CHECK(Quantity>0),
 CONSTRAINT CK_OrderItem_UnitPrice CHECK(UnitPrice>=0)
);
GO
CREATE TABLE Payment(
 PaymentId INT IDENTITY PRIMARY KEY,
 OrderId INT NOT NULL UNIQUE,
 CardType VARCHAR(20) NOT NULL,
 CardLast4 VARCHAR(4) NOT NULL,
 CardHolderName NVARCHAR(100) NOT NULL,
 ExpiryMonth INT NOT NULL,
 ExpiryYear INT NOT NULL,
 Amount DECIMAL(18,2) NOT NULL,
 PaymentStatus NVARCHAR(50) NOT NULL DEFAULT N'Chờ thanh toán',
 TransactionCode VARCHAR(100) NULL,
 PaymentDate DATETIME NULL,
 CONSTRAINT FK_Payment_Order FOREIGN KEY(OrderId) REFERENCES Orders(OrderId),
 CONSTRAINT CK_Payment_CardType CHECK(CardType IN('VISA','MasterCard','Discover','American Express')),
 CONSTRAINT CK_Payment_Last4 CHECK(LEN(CardLast4)=4),
 CONSTRAINT CK_Payment_Month CHECK(ExpiryMonth BETWEEN 1 AND 12),
 CONSTRAINT CK_Payment_Amount CHECK(Amount>=0),
 CONSTRAINT CK_Payment_Status CHECK(PaymentStatus IN(N'Chờ thanh toán',N'Đang xử lý',N'Thành công',N'Thất bại'))
);
GO

INSERT Customer(FullName,BirthDate,IdentityNo,Address,Phone,Username,PasswordHash,Email) VALUES
(N'Nguyễn Văn An','2002-05-10','079202000001',N'Quận 1, TP.HCM','0901234567','nguyenvanan','123456','an@gmail.com'),
(N'Trần Thị Bình','2001-08-20','079202000002',N'Quận 3, TP.HCM','0912345678','tranthibinh','123456','binh@gmail.com'),
(N'Lê Minh Hoàng','2003-02-15','079202000003',N'Thủ Đức, TP.HCM','0923456789','leminhhoang','123456',NULL),
(N'Phạm Huy Hoàng','2005-11-11','079202000004',N'Quận 10, TP.HCM','0934567890','hoangpham','123456','hoang@gmail.com');
GO
INSERT ProductGroup(GroupName,Description) VALUES
(N'Laptop',N'Laptop học tập và làm việc'),(N'Điện thoại',N'Điện thoại thông minh'),(N'Máy ảnh',N'Máy ảnh kỹ thuật số'),(N'Thiết bị máy tính',N'Chuột, bàn phím, màn hình'),(N'Đồ gia dụng',N'Thiết bị gia dụng');
GO
INSERT Product(ProductCode,ProductName,Manufacturer,ProductGroupId,ImageUrl,Description,Specifications,Price,StockQuantity,IsAvailable) VALUES
('LT001',N'Laptop ASUS Vivobook 15',N'ASUS',1,'asus-vivobook.jpg',N'Laptop học tập và làm việc',N'Intel Core i5, RAM 16GB, SSD 512GB',15990000,20,1),
('LT002',N'Laptop Dell Inspiron 15',N'Dell',1,'dell-inspiron.jpg',N'Laptop Dell phổ thông',N'Intel Core i5, RAM 16GB, SSD 512GB',17990000,15,1),
('DT001',N'iPhone 15',N'Apple',2,'iphone15.jpg',N'Điện thoại Apple iPhone 15',N'128GB, màn hình 6.1 inch',18990000,30,1),
('DT002',N'Samsung Galaxy S24',N'Samsung',2,'galaxy-s24.jpg',N'Điện thoại Samsung Galaxy',N'256GB, RAM 8GB',16990000,25,1),
('CAM001',N'Canon EOS R50',N'Canon',3,'canon-r50.jpg',N'Máy ảnh Canon EOS R50',N'APS-C, 24MP',19990000,10,1),
('PC001',N'Logitech MX Master 3S',N'Logitech',4,'mx-master-3s.jpg',N'Chuột không dây Logitech',N'Wireless, USB-C',1990000,50,1),
('PC002',N'Bàn phím Logitech MX Keys',N'Logitech',4,'mx-keys.jpg',N'Bàn phím không dây',N'Wireless, Bluetooth',2290000,40,1),
('GD001',N'Nồi chiên không dầu Philips',N'Philips',5,'philips-airfryer.jpg',N'Nồi chiên không dầu',N'Dung tích 5.5L',3290000,12,1);
GO
INSERT DeliveryType(DeliveryTypeName,Description) VALUES
(N'Thường',N'Giao hàng tiêu chuẩn'),(N'Chuyển phát nhanh',N'Giao hàng nhanh'),(N'Chuyển phát nhanh trong ngày',N'Giao hàng trong ngày');
GO
INSERT DeliveryArea(AreaName,Description) VALUES
(N'Nội thành TP.HCM',N'Các quận nội thành'),(N'Ngoại thành TP.HCM',N'Các khu vực ngoại thành'),(N'Tỉnh thành khác',N'Các tỉnh thành ngoài TP.HCM');
GO
INSERT DeliveryRate(DeliveryTypeId,DeliveryAreaId,Fee) VALUES
(1,1,30000),(2,1,50000),(3,1,80000),(1,2,50000),(2,2,80000),(3,2,120000),(1,3,70000),(2,3,120000),(3,3,180000);
GO
INSERT ShoppingCart(CustomerId) VALUES(1),(2),(3),(4);
GO
INSERT CartItem(CartId,ProductId,Quantity,UnitPrice) VALUES
(1,1,1,15990000),(1,6,2,1990000),(2,3,1,18990000),(2,7,1,2290000),(3,4,1,16990000),(4,5,1,19990000),(4,8,1,3290000);
GO
INSERT Recipient(FullName,Address,Phone) VALUES
(N'Nguyễn Văn An',N'Quận 1, TP.HCM','0901234567'),(N'Trần Minh Khang',N'Quận 3, TP.HCM','0911111111'),(N'Lê Minh Hoàng',N'Thủ Đức, TP.HCM','0922222222'),(N'Nguyễn Thị Lan',N'Quận 10, TP.HCM','0933333333');
GO
INSERT Orders(OrderCode,CustomerId,RecipientId,DeliveryTypeId,DeliveryAreaId,SubTotal,ShippingFee,TotalAmount,OrderStatus) VALUES
('ORD0001',1,1,2,1,19970000,0,19970000,N'Đã thanh toán'),
('ORD0002',2,2,1,1,21280000,30000,21310000,N'Chờ thanh toán'),
('ORD0003',3,3,3,1,16990000,0,16990000,N'Đã thanh toán');
GO
INSERT OrderItem(OrderId,ProductId,Quantity,UnitPrice) VALUES
(1,1,1,15990000),(1,6,2,1990000),(2,3,1,18990000),(2,7,1,2290000),(3,4,1,16990000);
GO
INSERT Payment(OrderId,CardType,CardLast4,CardHolderName,ExpiryMonth,ExpiryYear,Amount,PaymentStatus,TransactionCode,PaymentDate) VALUES
(1,'VISA','4242',N'NGUYEN VAN AN',12,2028,19970000,N'Thành công','TXN000001',GETDATE()),
(3,'MasterCard','5555',N'LE MINH HOANG',10,2029,16990000,N'Thành công','TXN000002',GETDATE());
GO

CREATE VIEW vw_OrderSummary AS
SELECT o.OrderId,o.OrderCode,c.FullName AS CustomerName,r.FullName AS RecipientName,r.Address AS RecipientAddress,r.Phone AS RecipientPhone,dt.DeliveryTypeName,da.AreaName,o.OrderDate,o.SubTotal,o.ShippingFee,o.TotalAmount,o.OrderStatus
FROM Orders o JOIN Customer c ON o.CustomerId=c.CustomerId JOIN Recipient r ON o.RecipientId=r.RecipientId JOIN DeliveryType dt ON o.DeliveryTypeId=dt.DeliveryTypeId JOIN DeliveryArea da ON o.DeliveryAreaId=da.DeliveryAreaId;
GO
CREATE VIEW vw_CartDetail AS
SELECT sc.CartId,c.CustomerId,c.FullName AS CustomerName,p.ProductId,p.ProductCode,p.ProductName,ci.Quantity,ci.UnitPrice,ci.Quantity*ci.UnitPrice AS Amount
FROM ShoppingCart sc JOIN Customer c ON sc.CustomerId=c.CustomerId JOIN CartItem ci ON sc.CartId=ci.CartId JOIN Product p ON ci.ProductId=p.ProductId;
GO
CREATE VIEW vw_OrderDetail AS
SELECT o.OrderId,o.OrderCode,c.FullName AS CustomerName,p.ProductCode,p.ProductName,oi.Quantity,oi.UnitPrice,oi.LineTotal,o.SubTotal,o.ShippingFee,o.TotalAmount,o.OrderStatus
FROM Orders o JOIN Customer c ON o.CustomerId=c.CustomerId JOIN OrderItem oi ON o.OrderId=oi.OrderId JOIN Product p ON oi.ProductId=p.ProductId;
GO

CREATE PROCEDURE sp_CalculateShippingFee
 @SubTotal DECIMAL(18,2), @DeliveryTypeId INT, @DeliveryAreaId INT, @ShippingFee DECIMAL(18,2) OUTPUT
AS
BEGIN
 SET NOCOUNT ON;
 IF @SubTotal<0 THROW 50001,N'SubTotal không hợp lệ.',1;
 IF @DeliveryTypeId=2 AND @SubTotal>=1000000 BEGIN SET @ShippingFee=0; RETURN; END;
 IF @DeliveryTypeId=3 AND @SubTotal>=5000000 BEGIN SET @ShippingFee=0; RETURN; END;
 SELECT @ShippingFee=Fee FROM DeliveryRate WHERE DeliveryTypeId=@DeliveryTypeId AND DeliveryAreaId=@DeliveryAreaId;
 IF @ShippingFee IS NULL THROW 50002,N'Không tìm thấy phí giao hàng.',1;
END;
GO
CREATE PROCEDURE sp_SearchProduct
 @Keyword NVARCHAR(200)=NULL, @ProductGroupId INT=NULL
AS
BEGIN
 SET NOCOUNT ON;
 SELECT p.ProductId,p.ProductCode,p.ProductName,p.Manufacturer,pg.GroupName,p.Price,p.StockQuantity,p.IsAvailable
 FROM Product p JOIN ProductGroup pg ON p.ProductGroupId=pg.ProductGroupId
 WHERE (@Keyword IS NULL OR p.ProductName LIKE N'%'+@Keyword+N'%' OR p.ProductCode LIKE '%'+@Keyword+'%' OR p.Manufacturer LIKE N'%'+@Keyword+N'%')
 AND (@ProductGroupId IS NULL OR p.ProductGroupId=@ProductGroupId)
 ORDER BY p.ProductName;
END;
GO


SELECT 'Customer' AS TableName,COUNT(*) AS TotalRows FROM Customer UNION ALL
SELECT 'ProductGroup',COUNT(*) FROM ProductGroup UNION ALL
SELECT 'Product',COUNT(*) FROM Product UNION ALL
SELECT 'ShoppingCart',COUNT(*) FROM ShoppingCart UNION ALL
SELECT 'CartItem',COUNT(*) FROM CartItem UNION ALL
SELECT 'DeliveryType',COUNT(*) FROM DeliveryType UNION ALL
SELECT 'DeliveryArea',COUNT(*) FROM DeliveryArea UNION ALL
SELECT 'DeliveryRate',COUNT(*) FROM DeliveryRate UNION ALL
SELECT 'Recipient',COUNT(*) FROM Recipient UNION ALL
SELECT 'Orders',COUNT(*) FROM Orders UNION ALL
SELECT 'OrderItem',COUNT(*) FROM OrderItem UNION ALL
SELECT 'Payment',COUNT(*) FROM Payment;
GO
SELECT * FROM vw_OrderSummary;
GO

