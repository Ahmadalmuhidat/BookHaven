using Book_Shop_Management_System.Configrations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

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
        WHERE books.Name LIKE @searchTerm OR books.FullName LIKE @searchTerm";

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
