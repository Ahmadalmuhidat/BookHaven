using Book_Shop_Management_System.Configrations;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Book_Shop_Management_System.Pages
{
    public class PurchaseDataItem
    {
        public string ID { get; set; }
        public string Book { get; set; }
        public string Supplier { get; set; }
        public string Quantity { get; set; }
        public string Date { get; set; }
        public string ETA { get; set; }
        public string Received { get; set; }
    }

    public partial class PurchasesDatabase : Page
    {
        private readonly MySQLConnector DB = new MySQLConnector();

        public PurchasesDatabase()
        {
            InitializeComponent();
            LoadAllPurchases();
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            string searchTerm = search_input.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                LoadAllPurchases();
                return;
            }

            string query = @"
                SELECT * FROM purchases
                INNER JOIN books ON books.ID = purchases.Book
                INNER JOIN suppliers ON suppliers.ID = purchases.Supplier
                WHERE books.Name LIKE @searchTerm 
                OR suppliers.FullName LIKE @searchTerm;
            ";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@searchTerm", "%" + searchTerm + "%")
            };

            PopulatePurchaseList(query, parameters);
        }

        private void LoadAllPurchases()
        {
            string query = @"
                SELECT * FROM purchases
                INNER JOIN books ON books.ID = purchases.Book
                INNER JOIN suppliers ON suppliers.ID = purchases.Supplier;
            ";
            PopulatePurchaseList(query);
        }

        private void PopulatePurchaseList(string query, MySqlParameter[] parameters = null)
        {
            try
            {
                Purchases.Items.Clear();

                using (var table = parameters == null ? DB.FetchData(query) : DB.FetchData(query, parameters))
                {
                    if (table.Rows.Count == 0)
                    {
                        MessageBox.Show("No purchases found.");
                        return;
                    }

                    foreach (DataRow row in table.Rows)
                    {
                        Purchases.Items.Add(new PurchaseDataItem
                        {
                            ID = row["ID"].ToString(),
                            Book = row["Name"].ToString(),
                            Supplier = row["FullName"].ToString(),
                            Quantity = row["Quantity"].ToString(),
                            Date = row["Date"].ToString(),
                            ETA = row["ETA"].ToString(),
                            Received = row["Received"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to fetch purchase data.\n\nDetails:\n" + ex.Message);
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedPurchases = Purchases.SelectedItems.Cast<PurchaseDataItem>().ToList();

                if (!selectedPurchases.Any())
                {
                    MessageBox.Show("No purchase selected.");
                    return;
                }

                foreach (var item in selectedPurchases)
                {
                    string query = "DELETE FROM purchases WHERE ID = @id";

                    var parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@id", item.ID)
                    };

                    if (DB.DeleteData(query, parameters))
                    {
                        Console.WriteLine($"Deleted purchase ID: {item.ID}");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to delete purchase ID: {item.ID}");
                    }
                }

                MessageBox.Show("Selected purchases deleted successfully.");
                LoadAllPurchases();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting purchases.\n\nDetails:\n" + ex.Message);
            }
        }
    }
}
