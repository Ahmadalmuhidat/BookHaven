using Book_Shop_Management_System.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using static Book_Shop_Management_System.Pages.BooksDatabase;

namespace Book_Shop_Management_System.Pages
{
    public class PurchasesDataItem
    {
        public string PurchaseID { get; set; }
        public string PurchaseBookID { get; set; }
        public string PurchaseSupplierID { get; set; }
        public string PurchaseQuantity { get; set; }
        public string PurchaseDate { get; set; }
        public string PurchaseETA { get; set; }
        public string PurchaseReceived { get; set; }
    }

    public partial class PurchasesDatabase : Page
    {
        private MySQLConnector DB = new MySQLConnector();
        public PurchasesDatabase()
        {
            InitializeComponent();
            getPurchases();
        }

        public void search(object sender, RoutedEventArgs e)
        {
            String searchQuery = search_input.Text;
            String query = "SELECT * FROM purchases INNER JOIN books ON books.BookID=purchases.PurchaseBookID INNER JOIN suppliers ON SupplierID=purchases.PurchaseSupplierID WHERE BookName LIKE '%" + searchQuery + "%' OR SupplierFullName LIKE'%" + searchQuery + "%'";

            using (var reader = DB.FetchData(query))
            {
                try
                {
                    if (reader.Rows.Count > 0)
                    {
                        Purchases.Items.Clear();

                        foreach (DataRow row in reader.Rows)
                        {
                            Purchases.Items.Add(new PurchasesDataItem
                            {
                                PurchaseID = row["PurchaseID"].ToString(),
                                PurchaseBookID = row["BookName"].ToString(),
                                PurchaseSupplierID = row["SupplierFullName"].ToString(),
                                PurchaseQuantity = row["PurchaseQuantity"].ToString(),
                                PurchaseDate = row["PurchaseDate"].ToString(),
                                PurchaseETA = row["PurchaseETA"].ToString(),
                                PurchaseReceived = row["PurchaseReceived"].ToString(),
                            });
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(searchQuery))
                    {
                        Purchases.Items.Clear();
                        getPurchases();
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

        public void getPurchases()
        {
            try
            {
                String query = "SELECT * FROM purchases INNER JOIN books ON books.BookID=purchases.PurchaseBookID INNER JOIN suppliers ON SupplierID=purchases.PurchaseSupplierID";
                using (var reader = DB.FetchData(query))
                {
                    foreach (DataRow row in reader.Rows)
                    {
                        Purchases.Items.Add(new PurchasesDataItem
                        {
                            PurchaseID = row["PurchaseID"].ToString(),
                            PurchaseBookID = row["BookName"].ToString(),
                            PurchaseSupplierID = row["SupplierFullName"].ToString(),
                            PurchaseQuantity = row["PurchaseQuantity"].ToString(),
                            PurchaseDate = row["PurchaseDate"].ToString(),
                            PurchaseETA = row["PurchaseETA"].ToString(),
                            PurchaseReceived = row["PurchaseReceived"].ToString(),
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
                List<PurchasesDataItem> selectedPurchases = Purchases.SelectedItems.Cast<PurchasesDataItem>().ToList();

                foreach (PurchasesDataItem classObj in selectedPurchases)
                {
                    String id = classObj.PurchaseID;
                    String query = "DELETE FROM purchases WHERE PurchaseID=" + id;

                    if (DB.DeleteData(query))
                    {
                        Console.WriteLine("Purchase deleted successfully for PurchaseID: " + id);
                    }
                    else
                    {
                        Console.WriteLine("No rows were deleted for PurchaseID: " + id);
                    }
                }

                MessageBox.Show("Data has been deleted successfully!");
                Purchases.Items.Clear();
                getPurchases(); // Refresh or update your purchase list after deletion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
