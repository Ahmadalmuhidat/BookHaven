Create a MySQL Database

DataBase Name : book_system

And Tables :

books

BookID varchar(200) 
BookName varchar(200) 
BookAuthor varchar(200) 
BookPrice varchar(200) 
BookQuantity int 
BookSupplier varchar(200) 
BookImage varchar(200)


suppliers

SupplierID varchar(200) 
SupplierFullName varchar(200) 
SupplierPhoneNumber varchar(200) 
SupplierAddressLine1 varchar(200) 
SupplierAddressLine2 varchar(200) 
SupplierCity varchar(200) 
SupplierState varchar(200) 
SupplierCreateDate date 
supplierImagePath varchar(200)

purchases

PurchaseID varchar(200) 
PurchaseBookID varchar(200) 
PurchaseSupplierID varchar(200) 
PurchaseQuantity varchar(200) 
PurchaseETA varchar(200) 
PurchaseReceived varchar(200) 
PurchaseDate date

employees

BookID varchar(200) 
BookName varchar(200) 
BookAuthor varchar(200) 
BookPrice varchar(200) 
BookQuantity int 
BookSupplier varchar(200) 
BookImage varchar(200)

members

MemberID varchar(200) 
MemberFullName varchar(200) 
MemberAddressLine1 varchar(200) 
MemberAddressLine2 varchar(200) 
MemberAddressCity varchar(200) 
MemberAddressState varchar(200) 
MemberPhoneNumber varchar(200) 
MemberEndDate date 
MemberBeginDate date 
MemberValid varchar(200) 
MemberImagePath varchar(200)

sales

SaleID varchar(200) 
SaleMemberID varchar(200) 
SaleBookID varchar(200) 
SaleEmployeeID varchar(200) 
SaleQuantity int 
SaleDate date 
SaleTotal int
