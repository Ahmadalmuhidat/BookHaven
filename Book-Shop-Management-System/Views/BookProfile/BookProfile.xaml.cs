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
    public class PurchasesDataItem
    {
        public string ID { get; set; }
        public string Supplier { get; set; }
        public string Quantity { get; set; }
        public string Date { get; set; }
        public string ETA { get; set; }
        public string Received { get; set; }
    }

    public partial class BookProfile : Page
    {
        private readonly MySQLConnector _db = new MySQLConnector();
        private const string ImageDirectory = @"Assets\Books Images";

        public BookProfile(string bookId)
        {
            InitializeComponent();
            LoadBookInfo(bookId);
            LoadPreviousPurchases(bookId);
        }

        private void LoadImage(string bookId)
        {
            try
            {
                string rootPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
                string imagePath = Path.Combine(rootPath, ImageDirectory, $"{bookId}.png");

                Image.Source = File.Exists(imagePath)
                  ? new BitmapImage(new Uri(imagePath))
                  : null; // Optional: load placeholder here
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load image for book '{bookId}': {ex.Message}");
            }
        }

        private void LoadPreviousPurchases(string bookId)
        {
            try
            {
                const string query = @"
          SELECT 
            purchases.ID, 
            suppliers.FullName, 
            purchases.Quantity, 
            purchases.Date, 
            purchases.ETA, 
            purchases.Received 
          FROM purchases
          INNER JOIN books ON books.ID = purchases.Book
          INNER JOIN suppliers ON suppliers.ID = purchases.Supplier
          WHERE purchases.Book = @param1;
        ";

                using var results = _db.FetchData(query, new MySqlParameter("@param1", bookId));
                Purchases.Items.Clear();

                foreach (DataRow row in results.Rows)
                {
                    Purchases.Items.Add(new PurchasesDataItem
                    {
                        ID = GetSafeString(row["ID"]),
                        Supplier = GetSafeString(row["FullName"]),
                        Quantity = GetSafeString(row["Quantity"]),
                        Date = GetSafeString(row["Date"]),
                        ETA = GetSafeString(row["ETA"]),
                        Received = GetSafeString(row["Received"])
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading purchase history for book '{bookId}': {ex.Message}");
            }
        }

        private void LoadBookInfo(string bookId)
        {
            try
            {
                const string query = "SELECT * FROM books WHERE ID = @param1";
                using var result = _db.FetchData(query, new MySqlParameter("@param1", bookId));

                if (result.Rows.Count == 0)
                {
                    MessageBox.Show("Book not found.");
                    return;
                }

                var row = result.Rows[0];
                LoadImage(bookId);

                SetTextWithLabel(ID, "Book ID", row["ID"]);
                SetTextWithLabel(Name, "Book Name", row["Name"]);
                SetTextWithLabel(Author, "Book Author", row["Author"]);
                SetTextWithLabel(Price, "Book Price", row["Price"]);
                SetTextWithLabel(Quantity, "Book Quantity", row["Quantity"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading book information for ID '{bookId}': {ex.Message}");
            }
        }

        private void SetTextWithLabel(TextBlock textBlock, string label, object value)
        {
            textBlock.Text = $"{label}: {GetSafeString(value)}";
        }

        private string GetSafeString(object value)
        {
            return value?.ToString() ?? "N/A";
        }
    }
}
