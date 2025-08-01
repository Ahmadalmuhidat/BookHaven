using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Book_Shop_Management_System.Configrations;
using MySql.Data.MySqlClient;

namespace Book_Shop_Management_System.Pages.Profiles
{
    public class PurchaseDataItem
    {
        public string ID { get; set; }
        public string Book { get; set; }
        public string Quantity { get; set; }
        public string Date { get; set; }
        public string ETA { get; set; }
        public string Received { get; set; }
    }

    public partial class SupplierProfile : Page
    {
        private readonly MySQLConnector _db = new MySQLConnector();

        public SupplierProfile(string supplierId)
        {
            InitializeComponent();
            LoadSupplierInfo(supplierId);
            LoadPreviousPurchases(supplierId);
        }

        private void LoadImage(string supplierId)
        {
            try
            {
                string rootPath = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
                if (string.IsNullOrEmpty(rootPath)) return;

                string imagePath = Path.Combine(rootPath, "Assets", "Suppliers Images", $"{supplierId}.png");
                if (File.Exists(imagePath))
                {
                    Image.Source = new BitmapImage(new Uri(imagePath));
                }
                else
                {
                    Image.Source = null; // Optionally set to a placeholder image here
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load supplier image: {ex.Message}", "Image Load Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadPreviousPurchases(string supplierId)
        {
            const string query = @"
                SELECT purchases.ID, books.Name, purchases.Quantity, purchases.Date, purchases.ETA, purchases.Received
                FROM purchases
                INNER JOIN books ON books.ID = purchases.Book
                WHERE purchases.Supplier = @SupplierId";

            try
            {
                using DataTable results = _db.FetchData(query, new MySqlParameter("@SupplierId", supplierId));
                Purchases.Items.Clear();

                foreach (DataRow row in results.Rows)
                {
                    Purchases.Items.Add(new PurchaseDataItem
                    {
                        ID = row["ID"].ToString(),
                        Book = row["Name"].ToString(),
                        Quantity = row["Quantity"].ToString(),
                        Date = row["Date"].ToString(),
                        ETA = row["ETA"].ToString(),
                        Received = row["Received"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading purchases: {ex.Message}", "Data Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadSupplierInfo(string supplierId)
        {
            const string query = "SELECT * FROM suppliers WHERE ID = @SupplierId";

            try
            {
                using DataTable results = _db.FetchData(query, new MySqlParameter("@SupplierId", supplierId));

                if (results.Rows.Count == 0) return;

                var row = results.Rows[0];
                LoadImage(row["ID"].ToString());

                SetTextWithLabel(ID, "Supplier ID", row["ID"]);
                SetTextWithLabel(FullName, "Full Name", row["FullName"]);
                SetTextWithLabel(PhoneNumber, "Phone Number", row["PhoneNumber"]);
                SetTextWithLabel(AddressLine1, "Address Line 1", row["AddressLine1"]);
                SetTextWithLabel(AddressLine2, "Address Line 2", row["AddressLine2"]);
                SetTextWithLabel(City, "City", row["City"]);
                SetTextWithLabel(State, "State", row["State"]);
                SetTextWithLabel(CreateDate, "Create Date", row["CreateDate"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading supplier info: {ex.Message}", "Data Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetTextWithLabel(TextBlock textBlock, string label, object value)
        {
            textBlock.Text = $"{label}: {value?.ToString() ?? "N/A"}";
        }
    }
}
