using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Book_Shop_Management_System.Configrations;
using Microsoft.Win32;
using MySql.Data.MySqlClient;

namespace Book_Shop_Management_System.UserControls
{
    public class ComboBoxItem
    {
        public string DisplayText { get; set; }
        public string Value { get; set; }
        public override string ToString() => DisplayText;  // For proper ComboBox display
    }

    public partial class BookDataEntry : UserControl
    {
        private string selectedImagePath;
        private readonly MySQLConnector _db = new MySQLConnector();

        public BookDataEntry()
        {
            InitializeComponent();
            LoadSuppliers();
        }

        private void SelectImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                selectedImagePath = openFileDialog.FileName;
                BookImage.Text = selectedImagePath;
            }
        }

        private void LoadSuppliers()
        {
            try
            {
                string query = "SELECT ID, FullName FROM suppliers";
                using DataTable dt = _db.FetchData(query);
                BookSupplier.Items.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    BookSupplier.Items.Add(new ComboBoxItem
                    {
                        DisplayText = row["FullName"].ToString(),
                        Value = row["ID"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load suppliers.\n{ex.Message}");
            }
        }

        private void ClearInputs()
        {
            BookName.Clear();
            BookAuthor.Clear();
            BookPrice.Clear();
            BookQuantity.Clear();
            BookImage.Clear();
            BookSupplier.SelectedIndex = -1;
            selectedImagePath = string.Empty;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(BookImage.Text) ||
                string.IsNullOrWhiteSpace(BookName.Text) ||
                string.IsNullOrWhiteSpace(BookAuthor.Text) ||
                string.IsNullOrWhiteSpace(BookPrice.Text) ||
                string.IsNullOrWhiteSpace(BookQuantity.Text) ||
                BookSupplier.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                return false;
            }

            if (!decimal.TryParse(BookPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Please enter a valid non-negative price.");
                return false;
            }

            if (!int.TryParse(BookQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Please enter a valid non-negative quantity.");
                return false;
            }

            if (string.IsNullOrEmpty(selectedImagePath) || !File.Exists(selectedImagePath))
            {
                MessageBox.Show("Please select a valid image file.");
                return false;
            }

            return true;
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInputs())
                    return;

                string bookId = Guid.NewGuid().ToString("N"); // Unique ID

                string rootPath = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName
                                  ?? throw new DirectoryNotFoundException("Could not resolve root path.");

                string folderPath = Path.Combine(rootPath, "Assets", "Books Images");
                string destinationPath = Path.Combine(folderPath, $"{bookId}.png");

                string query = @"
                    INSERT INTO books (ID, Name, Author, Price, Quantity, Supplier, Image)
                    VALUES (@ID, @Name, @Author, @Price, @Quantity, @Supplier, @Image)";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@ID", bookId),
                    new MySqlParameter("@Name", BookName.Text.Trim()),
                    new MySqlParameter("@Author", BookAuthor.Text.Trim()),
                    new MySqlParameter("@Price", decimal.Parse(BookPrice.Text.Trim())),
                    new MySqlParameter("@Quantity", int.Parse(BookQuantity.Text.Trim())),
                    new MySqlParameter("@Supplier", ((ComboBoxItem)BookSupplier.SelectedItem).Value),
                    new MySqlParameter("@Image", destinationPath)
                };

                if (_db.InsertData(query, parameters))
                {
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    File.Copy(selectedImagePath, destinationPath, true);
                    ClearInputs();

                    MessageBox.Show("Data inserted successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to insert data. Please check your inputs or database connection.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting data: {ex.Message}");
            }
        }
    }
}
