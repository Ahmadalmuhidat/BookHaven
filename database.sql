-- Create the database
CREATE DATABASE IF NOT EXISTS BookHevean;

-- Use the database
USE BookHevean;

-- Create the books table
CREATE TABLE books (
    ID VARCHAR(200) PRIMARY KEY,
    Name VARCHAR(200),
    Author VARCHAR(200),
    Price VARCHAR(200),
    Quantity INT,
    Supplier VARCHAR(200),
    Image VARCHAR(200)
);

-- Create the suppliers table
CREATE TABLE suppliers (
    ID VARCHAR(200) PRIMARY KEY,
    FullName VARCHAR(200),
    PhoneNumber VARCHAR(200),
    AddressLine1 VARCHAR(200),
    AddressLine2 VARCHAR(200),
    City VARCHAR(200),
    State VARCHAR(200),
    CreateDate DATE,
    ImagePath VARCHAR(200)
);

-- Create the purchases table
CREATE TABLE purchases (
    ID VARCHAR(200) PRIMARY KEY,
    Book VARCHAR(200),
    Supplier VARCHAR(200),
    Quantity VARCHAR(200),
    ETA VARCHAR(200),
    Received VARCHAR(200),
    Date DATE,
    FOREIGN KEY (Book) REFERENCES books(ID),
    FOREIGN KEY (Supplier) REFERENCES suppliers(ID)
);

-- Create the sales table
CREATE TABLE sales (
    ID VARCHAR(200) PRIMARY KEY,
    Date DATE,
    Total INT
);

-- Create the items table
CREATE TABLE sale_items (
  ID INT AUTO_INCREMENT PRIMARY KEY,
  Sale VARCHAR(36),
  Book VARCHAR(200),
  Quantity INT,
  UnitPrice DECIMAL(10, 2),
  FOREIGN KEY (Sale) REFERENCES sales(ID),
  FOREIGN KEY (Book) REFERENCES books(ID)
);