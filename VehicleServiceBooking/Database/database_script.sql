/* ============================================================
   Vehicle Maintenance Service Booking System
   Database Script - SQL Server
   Run this entire script in SQL Server Management Studio (SSMS).
   It creates the database, tables, constraints, sample data,
   and all stored procedures used by the ASP.NET website.
   ============================================================ */

IF DB_ID('VehicleServiceDB') IS NOT NULL
BEGIN
    ALTER DATABASE VehicleServiceDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE VehicleServiceDB;
END
GO

CREATE DATABASE VehicleServiceDB;
GO

USE VehicleServiceDB;
GO

/* ============================================================
   TABLES
   ============================================================ */

-- 1) Customers
CREATE TABLE Customers (
    CustomerID   INT IDENTITY(1,1) NOT NULL,
    FullName     NVARCHAR(100) NOT NULL,
    Email        NVARCHAR(150) NOT NULL,
    Phone        NVARCHAR(20)  NOT NULL,
    CreatedDate  DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Customers PRIMARY KEY (CustomerID),
    CONSTRAINT UQ_Customers_Email UNIQUE (Email)
);
GO

-- 2) Vehicles  (One customer -> many vehicles)
CREATE TABLE Vehicles (
    VehicleID    INT IDENTITY(1,1) NOT NULL,
    CustomerID   INT NOT NULL,
    PlateNumber  NVARCHAR(20) NOT NULL,
    Brand        NVARCHAR(50) NOT NULL,
    Model        NVARCHAR(50) NOT NULL,
    Year         INT NOT NULL,
    CreatedDate  DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Vehicles PRIMARY KEY (VehicleID),
    CONSTRAINT FK_Vehicles_Customers FOREIGN KEY (CustomerID)
        REFERENCES Customers(CustomerID) ON DELETE CASCADE,
    CONSTRAINT CK_Vehicles_Year CHECK (Year BETWEEN 1980 AND 2100)
);
GO

-- 3) ServiceBookings (One vehicle -> many bookings)
CREATE TABLE ServiceBookings (
    BookingID     INT IDENTITY(1,1) NOT NULL,
    VehicleID     INT NOT NULL,
    ServiceType   NVARCHAR(50) NOT NULL,
    BookingDate   DATE NOT NULL,
    Status        NVARCHAR(20) NOT NULL DEFAULT 'Pending',
    Notes         NVARCHAR(500) NULL,
    CreatedDate   DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_ServiceBookings PRIMARY KEY (BookingID),
    CONSTRAINT FK_ServiceBookings_Vehicles FOREIGN KEY (VehicleID)
        REFERENCES Vehicles(VehicleID) ON DELETE CASCADE,
    CONSTRAINT CK_ServiceBookings_Status CHECK (Status IN ('Pending','Confirmed','In Progress','Completed','Cancelled'))
);
GO

/* ============================================================
   SAMPLE DATA
   ============================================================ */

INSERT INTO Customers (FullName, Email, Phone) VALUES
('Ahmed Al Mazrouei', 'ahmed.mazrouei@example.com', '0501234567'),
('Sara Al Nuaimi', 'sara.nuaimi@example.com', '0559876543'),
('John Peterson', 'john.peterson@example.com', '0521122334');
GO

INSERT INTO Vehicles (CustomerID, PlateNumber, Brand, Model, Year) VALUES
(1, 'A12345', 'Toyota', 'Camry', 2019),
(1, 'B98765', 'Nissan', 'Patrol', 2021),
(2, 'D55211', 'Honda', 'Civic', 2020),
(3, 'E30044', 'Ford', 'F-150', 2018);
GO

INSERT INTO ServiceBookings (VehicleID, ServiceType, BookingDate, Status, Notes) VALUES
(1, 'Oil Change', '2026-07-05', 'Pending', 'Customer requested synthetic oil.'),
(1, 'Tire Rotation', '2026-06-20', 'Completed', 'All tires rotated and balanced.'),
(2, 'Full Inspection', '2026-07-10', 'Confirmed', 'Pre-road-trip inspection.'),
(3, 'Brake Service', '2026-06-25', 'In Progress', 'Front brake pads replaced.'),
(4, 'Battery Replacement', '2026-07-02', 'Pending', NULL);
GO

/* ============================================================
   STORED PROCEDURES - Customers
   ============================================================ */

CREATE PROCEDURE sp_Customer_Insert
    @FullName NVARCHAR(100),
    @Email NVARCHAR(150),
    @Phone NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Customers (FullName, Email, Phone)
    VALUES (@FullName, @Email, @Phone);
    SELECT SCOPE_IDENTITY() AS NewCustomerID;
END
GO

CREATE PROCEDURE sp_Customer_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CustomerID, FullName, Email, Phone, CreatedDate
    FROM Customers
    ORDER BY FullName;
END
GO

CREATE PROCEDURE sp_Customer_GetById
    @CustomerID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CustomerID, FullName, Email, Phone, CreatedDate
    FROM Customers
    WHERE CustomerID = @CustomerID;
END
GO

CREATE PROCEDURE sp_Customer_Update
    @CustomerID INT,
    @FullName NVARCHAR(100),
    @Email NVARCHAR(150),
    @Phone NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Customers
    SET FullName = @FullName,
        Email = @Email,
        Phone = @Phone
    WHERE CustomerID = @CustomerID;
END
GO

CREATE PROCEDURE sp_Customer_Delete
    @CustomerID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Customers WHERE CustomerID = @CustomerID;
END
GO

/* ============================================================
   STORED PROCEDURES - Vehicles
   ============================================================ */

CREATE PROCEDURE sp_Vehicle_Insert
    @CustomerID INT,
    @PlateNumber NVARCHAR(20),
    @Brand NVARCHAR(50),
    @Model NVARCHAR(50),
    @Year INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Vehicles (CustomerID, PlateNumber, Brand, Model, Year)
    VALUES (@CustomerID, @PlateNumber, @Brand, @Model, @Year);
    SELECT SCOPE_IDENTITY() AS NewVehicleID;
END
GO

CREATE PROCEDURE sp_Vehicle_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT v.VehicleID, v.CustomerID, c.FullName AS OwnerName,
           v.PlateNumber, v.Brand, v.Model, v.Year, v.CreatedDate
    FROM Vehicles v
    INNER JOIN Customers c ON v.CustomerID = c.CustomerID
    ORDER BY v.CreatedDate DESC;
END
GO

CREATE PROCEDURE sp_Vehicle_GetByCustomer
    @CustomerID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT VehicleID, CustomerID, PlateNumber, Brand, Model, Year, CreatedDate
    FROM Vehicles
    WHERE CustomerID = @CustomerID
    ORDER BY CreatedDate DESC;
END
GO

CREATE PROCEDURE sp_Vehicle_GetById
    @VehicleID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT VehicleID, CustomerID, PlateNumber, Brand, Model, Year, CreatedDate
    FROM Vehicles
    WHERE VehicleID = @VehicleID;
END
GO

CREATE PROCEDURE sp_Vehicle_Update
    @VehicleID INT,
    @PlateNumber NVARCHAR(20),
    @Brand NVARCHAR(50),
    @Model NVARCHAR(50),
    @Year INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Vehicles
    SET PlateNumber = @PlateNumber,
        Brand = @Brand,
        Model = @Model,
        Year = @Year
    WHERE VehicleID = @VehicleID;
END
GO

CREATE PROCEDURE sp_Vehicle_Delete
    @VehicleID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Vehicles WHERE VehicleID = @VehicleID;
END
GO

/* ============================================================
   STORED PROCEDURES - ServiceBookings
   ============================================================ */

CREATE PROCEDURE sp_Booking_Insert
    @VehicleID INT,
    @ServiceType NVARCHAR(50),
    @BookingDate DATE,
    @Status NVARCHAR(20),
    @Notes NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ServiceBookings (VehicleID, ServiceType, BookingDate, Status, Notes)
    VALUES (@VehicleID, @ServiceType, @BookingDate, @Status, @Notes);
    SELECT SCOPE_IDENTITY() AS NewBookingID;
END
GO

CREATE PROCEDURE sp_Booking_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT b.BookingID, b.VehicleID, v.PlateNumber, v.Brand, v.Model,
           c.FullName AS OwnerName, b.ServiceType, b.BookingDate, b.Status, b.Notes
    FROM ServiceBookings b
    INNER JOIN Vehicles v ON b.VehicleID = v.VehicleID
    INNER JOIN Customers c ON v.CustomerID = c.CustomerID
    ORDER BY b.BookingDate DESC;
END
GO

CREATE PROCEDURE sp_Booking_GetByVehicle
    @VehicleID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT BookingID, VehicleID, ServiceType, BookingDate, Status, Notes
    FROM ServiceBookings
    WHERE VehicleID = @VehicleID
    ORDER BY BookingDate DESC;
END
GO

CREATE PROCEDURE sp_Booking_GetById
    @BookingID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT BookingID, VehicleID, ServiceType, BookingDate, Status, Notes
    FROM ServiceBookings
    WHERE BookingID = @BookingID;
END
GO

CREATE PROCEDURE sp_Booking_UpdateStatus
    @BookingID INT,
    @Status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ServiceBookings
    SET Status = @Status
    WHERE BookingID = @BookingID;
END
GO

CREATE PROCEDURE sp_Booking_Update
    @BookingID INT,
    @ServiceType NVARCHAR(50),
    @BookingDate DATE,
    @Status NVARCHAR(20),
    @Notes NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ServiceBookings
    SET ServiceType = @ServiceType,
        BookingDate = @BookingDate,
        Status = @Status,
        Notes = @Notes
    WHERE BookingID = @BookingID;
END
GO

CREATE PROCEDURE sp_Booking_Delete
    @BookingID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ServiceBookings WHERE BookingID = @BookingID;
END
GO

CREATE PROCEDURE sp_Booking_SearchFilter
    @Status NVARCHAR(20) = NULL,
    @BookingDate DATE = NULL,
    @PlateNumber NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT b.BookingID, b.VehicleID, v.PlateNumber, v.Brand, v.Model,
           c.FullName AS OwnerName, b.ServiceType, b.BookingDate, b.Status, b.Notes
    FROM ServiceBookings b
    INNER JOIN Vehicles v ON b.VehicleID = v.VehicleID
    INNER JOIN Customers c ON v.CustomerID = c.CustomerID
    WHERE (@Status IS NULL OR b.Status = @Status)
      AND (@BookingDate IS NULL OR b.BookingDate = @BookingDate)
      AND (@PlateNumber IS NULL OR v.PlateNumber LIKE '%' + @PlateNumber + '%')
    ORDER BY b.BookingDate DESC;
END
GO

/* ============================================================
   REPORT PROCEDURES
   ============================================================ */

-- Service bookings by date (all, ordered)
CREATE PROCEDURE sp_Report_BookingsByDate
AS
BEGIN
    SET NOCOUNT ON;
    SELECT b.BookingID, v.PlateNumber, c.FullName AS OwnerName,
           b.ServiceType, b.BookingDate, b.Status
    FROM ServiceBookings b
    INNER JOIN Vehicles v ON b.VehicleID = v.VehicleID
    INNER JOIN Customers c ON v.CustomerID = c.CustomerID
    ORDER BY b.BookingDate ASC;
END
GO

-- Service bookings grouped/filtered by status
CREATE PROCEDURE sp_Report_BookingsByStatus
    @Status NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT b.BookingID, v.PlateNumber, c.FullName AS OwnerName,
           b.ServiceType, b.BookingDate, b.Status
    FROM ServiceBookings b
    INNER JOIN Vehicles v ON b.VehicleID = v.VehicleID
    INNER JOIN Customers c ON v.CustomerID = c.CustomerID
    WHERE (@Status IS NULL OR b.Status = @Status)
    ORDER BY b.Status, b.BookingDate DESC;
END
GO

-- Vehicle service history for one vehicle
CREATE PROCEDURE sp_Report_VehicleServiceHistory
    @VehicleID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT b.BookingID, b.ServiceType, b.BookingDate, b.Status, b.Notes
    FROM ServiceBookings b
    WHERE b.VehicleID = @VehicleID
    ORDER BY b.BookingDate DESC;
END
GO

-- Customer vehicle list (all customers with their vehicles)
CREATE PROCEDURE sp_Report_CustomerVehicleList
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.CustomerID, c.FullName, c.Email, c.Phone,
           v.VehicleID, v.PlateNumber, v.Brand, v.Model, v.Year
    FROM Customers c
    LEFT JOIN Vehicles v ON c.CustomerID = v.CustomerID
    ORDER BY c.FullName, v.Brand;
END
GO

PRINT 'VehicleServiceDB created successfully with tables, sample data, and stored procedures.';
GO
