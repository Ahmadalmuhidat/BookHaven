Create a MySQL Database

DataBase Name : book_system

And Tables :

books

1. BookID varchar(200) 
2. BookName varchar(200) 
3. BookAuthor varchar(200) 
4. BookPrice varchar(200) 
5. BookQuantity int 
6. BookSupplier varchar(200) 
7. BookImage varchar(200)


suppliers

1. SupplierID varchar(200) 
2. SupplierFullName varchar(200) 
3. SupplierPhoneNumber varchar(200) 
4. SupplierAddressLine1 varchar(200) 
5. SupplierAddressLine2 varchar(200) 
6. SupplierCity varchar(200) 
7. SupplierState varchar(200) 
8. SupplierCreateDate date 
9. supplierImagePath varchar(200)

purchases

1. PurchaseID varchar(200) 
2. PurchaseBookID varchar(200) 
3. PurchaseSupplierID varchar(200) 
4. PurchaseQuantity varchar(200) 
5. PurchaseETA varchar(200) 
6. PurchaseReceived varchar(200) 
7. PurchaseDate date

employees

1. BookID varchar(200) 
2. BookName varchar(200) 
3. BookAuthor varchar(200) 
4. BookPrice varchar(200) 
5. BookQuantity int 
6. BookSupplier varchar(200) 
7. BookImage varchar(200)

members

1. MemberID varchar(200) 
2. MemberFullName varchar(200) 
3. MemberAddressLine1 varchar(200) 
4. MemberAddressLine2 varchar(200) 
5. MemberAddressCity varchar(200) 
6. MemberAddressState varchar(200) 
7. MemberPhoneNumber varchar(200) 
8. MemberEndDate date 
9. MemberBeginDate date 
10. MemberValid varchar(200) 
11. MemberImagePath varchar(200)

sales

1. SaleID varchar(200) 
2. SaleMemberID varchar(200) 
3. SaleBookID varchar(200) 
4. SaleEmployeeID varchar(200) 
5. SaleQuantity int 
6. SaleDate date 
7. SaleTotal int


Note:
you muts ad these rows to the database after creating it:
1. in members table: '0', 'Unknown', '--', '--', '--', '--', '--', '2023-10-16', '2023-10-16', '--', '----'
2. in employees table: '0', 'Unkown', '----', '----', '----', '----', '----', '2023-10-16', '0', '----', '----'
