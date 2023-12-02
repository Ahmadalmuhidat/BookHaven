using Book_Shop_Management_System.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace Book_Shop_Management_System.Pages
{
    public partial class SalesDatabase : Page
    {
        public class SalesDataItem
        {
            public string SaleID { get; set; }
            public string SaleMemberID { get; set; }
            public string SaleBookID { get; set; }
            public string SaleQuantity { get; set; }
            public string SaleDate { get; set; }
            public string SaleEmployee { get; set; }
            public string SaleTotal { get; set; }
        }

        private MySQLConnector DB = new MySQLConnector();

        public SalesDatabase()
        {
            InitializeComponent();
            getSales();
        }

        public void search(object sender, RoutedEventArgs e)
        {
            String searchQuery = search_input.Text;
            String query = "SELECT * FROM sales INNER JOIN members ON members.MemberID=SaleMemberID INNER JOIN books ON books.BookID=SaleBookID INNER JOIN employees ON employees.EmployeeID=SaleEmployeeID WHERE BookName LIKE '%" + searchQuery + "%' OR MemberFullName LIKE'%" + searchQuery + "%' OR EmployeeFullName LIKE'%" + searchQuery + "%'";

            using (var reader = DB.FetchData(query))
            {
                try
                {
                    if (reader.Rows.Count > 0)
                    {
                        Sales.Items.Clear();

                        foreach (DataRow row in reader.Rows)
                        {
                            Sales.Items.Add(new SalesDataItem
                            {
                                SaleID = row["SaleID"].ToString(),
                                SaleMemberID = row["MemberFullName"].ToString(),
                                SaleBookID = row["BookName"].ToString(),
                                SaleQuantity = row["SaleQuantity"].ToString(),
                                SaleDate = row["SaleDate"].ToString(),
                                SaleEmployee = row["EmployeeFullName"].ToString(),
                                SaleTotal = row["SaleTotal"].ToString(),
                            });
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(searchQuery))
                    {
                        Sales.Items.Clear();
                        getSales();
                    }
                    else
                    {
                        MessageBox.Show("Sorry, book has not been found!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
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
                            SaleMemberID = row["MemberFullName"].ToString(),
                            SaleBookID = row["BookName"].ToString(),
                            SaleQuantity = row["SaleQuantity"].ToString(),
                            SaleDate = row["SaleDate"].ToString(),
                            SaleEmployee = row["EmployeeFullName"].ToString(),
                            SaleTotal = row["SaleTotal"].ToString(),
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
                List<SalesDataItem> selectedSales = Sales.SelectedItems.Cast<SalesDataItem>().ToList();

                foreach (SalesDataItem classObj in selectedSales)
                {
                    String id = classObj.SaleID;
                    String query = "DELETE FROM sales WHERE SaleID=" + id;

                    if (DB.DeleteData(query))
                    {
                        Console.WriteLine("Sale deleted successfully for SaleID: " + id);
                    }
                    else
                    {
                        Console.WriteLine("No rows were deleted for SaleID: " + id);
                    }
                }

                MessageBox.Show("Data has been deleted successfully!");
                Sales.Items.Clear();
                getSales(); // Refresh or update your sale list after deletion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}