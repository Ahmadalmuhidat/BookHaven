using Book_Shop_Management_System.Configrations;
using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Net;

namespace Book_Shop_Management_System.Pages
{
    public partial class SalesDatabase : Page
    {
        public class SalesDataItem
        {
            public string ID { get; set; }
            public string Date { get; set; }
            public string Total { get; set; }
        }

        private readonly MySQLConnector db = new MySQLConnector();

        public SalesDatabase()
        {
            InitializeComponent();
            GetSales();
        }

        public void Search(object sender, RoutedEventArgs e)
        {
            string searchTerm = search_input.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Sales.Items.Clear();
                GetSales();
                return;
            }

            string query = @"
                SELECT sales.ID, sales.Date, sales.Total 
                FROM sales 
                INNER JOIN books ON books.ID = sales.Book 
                WHERE books.Name LIKE @searchTerm OR books.FullName LIKE @searchTerm
            ";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@searchTerm", "%" + searchTerm + "%")
            };

            try
            {
                using (var table = db.FetchData(query, parameters))
                {
                    if (table.Rows.Count > 0)
                    {
                        LoadSalesFromTable(table);
                    }
                    else
                    {
                        MessageBox.Show("No matching records found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while searching sales:\n" + ex.Message);
            }
        }

        public void GetSales()
        {
            try
            {
                string query = "SELECT ID, Date, Total FROM sales";

                using (var table = db.FetchData(query))
                {
                    LoadSalesFromTable(table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to fetch sales:\n" + ex.Message);
            }
        }

        private void LoadSalesFromTable(DataTable table)
        {
            Sales.Items.Clear();

            foreach (DataRow row in table.Rows)
            {
                Sales.Items.Add(new SalesDataItem
                {
                    ID = row["ID"].ToString(),
                    Date = row["Date"].ToString(),
                    Total = row["Total"].ToString()
                });
            }
        }

        private void GenerateInvoice(object sender, RoutedEventArgs e)
        {
            if (Sales.SelectedItem == null)
            {
                MessageBox.Show("Please select a sale to generate the invoice.");
                return;
            }

            var selectedSale = (SalesDataItem)Sales.SelectedItem;

            string saleInfoQuery = "SELECT ID, Date, Total FROM sales WHERE ID = @saleID";
            var saleInfoParams = new MySqlParameter[]
            {
                new MySqlParameter("@saleID", selectedSale.ID)
            };

            try
            {
                using (var saleTable = db.FetchData(saleInfoQuery, saleInfoParams))
                {
                    if (saleTable.Rows.Count == 0)
                    {
                        MessageBox.Show("Sale not found.");
                        return;
                    }

                    var saleRow = saleTable.Rows[0];
                    string saleId = saleRow["ID"].ToString();
                    string saleDate = Convert.ToDateTime(saleRow["Date"]).ToShortDateString();
                    string saleTotal = saleRow["Total"].ToString();

                    // Now get all sale items for that sale
                    string itemQuery = @"
                        SELECT 
                          b.Name AS BookName,
                          b.Author,
                          si.Quantity,
                          si.UnitPrice
                        FROM sale_items si
                        INNER JOIN books b ON b.ID = si.Book
                        WHERE si.Sale = @saleID
                    ";

                    using (var itemTable = db.FetchData(itemQuery, saleInfoParams))
                    {
                        if (itemTable.Rows.Count == 0)
                        {
                            MessageBox.Show("No items found for this sale.");
                            return;
                        }

                        string invoiceText = $"===== INVOICE =====\n" +
                                             $"Sale ID: {saleId}\n" +
                                             $"Date: {saleDate}\n" +
                                             $"===============================\n";

                        foreach (DataRow item in itemTable.Rows)
                        {
                            string book = item["BookName"].ToString();
                            int qty = Convert.ToInt32(item["Quantity"]);

                            invoiceText += $"{qty,5}  {book,-40}\n";
                        }

                        invoiceText += "-------------------------------\n" +
                                       $"Total Amount: {saleTotal} USD\n" +
                                       "===============================\n" +
                                       "Thank you for your purchase!";

                        MessageBox.Show(invoiceText, "Invoice");

                        string fileName = $"Invoice_{saleId}.txt";
                        string rootPath = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName
                                          ?? throw new DirectoryNotFoundException("Could not resolve root path.");

                        string invoiceDir = Path.Combine(rootPath, "Invoices");

                        if (!Directory.Exists(invoiceDir))
                        {
                            Directory.CreateDirectory(invoiceDir);
                        }

                        string destinationPath = Path.Combine(invoiceDir, fileName);
                        File.WriteAllText(destinationPath, invoiceText);
                        MessageBox.Show($"Invoice saved to:\n{destinationPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to generate invoice:\n" + ex.Message);
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedSales = Sales.SelectedItems.Cast<SalesDataItem>().ToList();

                if (!selectedSales.Any())
                {
                    MessageBox.Show("No sales selected for deletion.");
                    return;
                }

                foreach (var sale in selectedSales)
                {
                    string query = "DELETE FROM sales WHERE ID = @id";
                    var parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@id", sale.ID)
                    };

                    if (db.DeleteData(query, parameters))
                    {
                        Console.WriteLine("Deleted sale ID: " + sale.ID);
                    }
                    else
                    {
                        Console.WriteLine("No rows deleted for sale ID: " + sale.ID);
                    }
                }
                MessageBox.Show("Selected sales deleted successfully.");
                GetSales();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete sales:\n" + ex.Message);
            }
        }
    }
}
