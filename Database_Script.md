-- Create the database
CREATE DATABASE book_system;

-- Use the database
USE book_system;

-- Create the tables
CREATE TABLE books (
    BookID VARCHAR(200),
    BookName VARCHAR(200),
    BookAuthor VARCHAR(200),
    BookPrice VARCHAR(200),
    BookQuantity INT,
    BookSupplier VARCHAR(200),
    BookImage VARCHAR(200)
);

CREATE TABLE suppliers (
    SupplierID VARCHAR(200),
    SupplierFullName VARCHAR(200),
    SupplierPhoneNumber VARCHAR(200),
    SupplierAddressLine1 VARCHAR(200),
    SupplierAddressLine2 VARCHAR(200),
    SupplierCity VARCHAR(200),
    SupplierState VARCHAR(200),
    SupplierCreateDate DATE,
    SupplierImagePath VARCHAR(200)
);

CREATE TABLE purchases (
    PurchaseID VARCHAR(200),
    PurchaseBookID VARCHAR(200),
    PurchaseSupplierID VARCHAR(200),
    PurchaseQuantity VARCHAR(200),
    PurchaseETA VARCHAR(200),
    PurchaseReceived VARCHAR(200),
    PurchaseDate DATE
);

CREATE TABLE employees (
    BookID VARCHAR(200),
    BookName VARCHAR(200),
    BookAuthor VARCHAR(200),
    BookPrice VARCHAR(200),
    BookQuantity INT,
    BookSupplier VARCHAR(200),
    BookImage VARCHAR(200)
);

CREATE TABLE members (
    MemberID VARCHAR(200),
    MemberFullName VARCHAR(200),
    MemberAddressLine1 VARCHAR(200),
    MemberAddressLine2 VARCHAR(200),
    MemberAddressCity VARCHAR(200),
    MemberAddressState VARCHAR(200),
    MemberPhoneNumber VARCHAR(200),
    MemberEndDate DATE,
    MemberBeginDate DATE,
    MemberValid VARCHAR(200),
    MemberImagePath VARCHAR(200)
);

CREATE TABLE sales (
    SaleID VARCHAR(200),
    SaleMemberID VARCHAR(200),
    SaleBookID VARCHAR(200),
    SaleEmployeeID VARCHAR(200),
    SaleQuantity INT,
    SaleDate DATE,
    SaleTotal INT
);

-- Insert initial rows into members and employees tables
INSERT INTO members (MemberID, MemberFullName, MemberAddressLine1, MemberAddressLine2, MemberAddressCity, MemberAddressState, MemberPhoneNumber, MemberEndDate, MemberBeginDate, MemberValid, MemberImagePath)
VALUES ('0', 'Unknown', '--', '--', '--', '--', '--', '2023-10-16', '2023-10-16', '--', '----');

INSERT INTO employees (BookID, BookName, BookAuthor, BookPrice, BookQuantity, BookSupplier, BookImage)
VALUES ('0', 'Unknown', '----', '----', '----', '----', '----', '2023-10-16', '0', '----', '----');
