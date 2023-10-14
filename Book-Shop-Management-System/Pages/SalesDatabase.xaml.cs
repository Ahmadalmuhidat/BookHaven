using Book_Shop_Management_System.DB;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;


namespace Book_Shop_Management_System.Pages
{
    public partial class SalesDatabase : Page
    {
        public class SalesDataItem
        {
            public string SaleID { get; set; }
            public string SaleInvoiceID { get; set; }
            public string SaleMemberID { get; set; }
            public string SaleBookID { get; set; }
            public string SaleQuantity { get; set; }
            public string SaleAmount { get; set; }
            public string SaleDate { get; set; }
            public string SaleEmployee { get; set; }
        }

        private MySQLConnector DB = new MySQLConnector();

        public SalesDatabase()
        {
            InitializeComponent();
            getSales();
        }

        public void getSales()
        {
            try
            {
                String query = "select * from sales INNER JOIN members ON members.MemberID=SaleMemberID INNER JOIN books ON books.BookID=SaleBookID INNER JOIN employees ON employees.EmployeeID=SaleEmployeeID";
                using (var reader = DB.FetchData(query))
                {
                    foreach (DataRow row in reader.Rows)
                    {
                        Sales.Items.Add(new SalesDataItem
                        {
                            SaleID = row["SaleID"].ToString(),
                            SaleInvoiceID = row["SaleInvoiceID"].ToString(),
                            SaleMemberID = row["MemberFullName"].ToString(),
                            SaleBookID = row["BookName"].ToString(),
                            SaleQuantity = row["SaleQuantity"].ToString(),
                            SaleAmount = row["SaleAmount"].ToString(),
                            SaleDate = row["SaleDate"].ToString(),
                            SaleEmployee = row["EmployeeFullName"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void delete(object sender, RoutedEventArgs e)
        {
            try
            {
                SalesDataItem classObj = Sales.SelectedItem as SalesDataItem;
                String id = classObj.SaleID;
                String[] values = { id };
                String query = "DELETE FROM sales WHERE SaleID=" + id;

                if (DB.DeleteData(query))
                {
                    MessageBox.Show("Data has beem deleted successfully!");
                    Sales.Items.Clear();
                    getSales();
                }
                else
                {
                    MessageBox.Show("No rows were deleted. Check your data or database.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}