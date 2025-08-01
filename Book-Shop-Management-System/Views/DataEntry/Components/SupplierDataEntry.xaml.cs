using Book_Shop_Management_System.Configrations;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Book_Shop_Management_System.UserControls
{
    public partial class SupplierDataEntry : UserControl
    {
        private string _selectedImagePath = string.Empty;
        private readonly MySQLConnector _db = new MySQLConnector();

        public SupplierDataEntry()
        {
            InitializeComponent();
        }

        private void SelectImage(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedImagePath = openFileDialog.FileName;
                SupplierImage.Text = _selectedImagePath; // Assuming SupplierImage is a TextBox or similar
            }
        }

        private void ClearInputs()
        {
            SupplierFullName.Clear();
            SupplierPhoneNumber.Clear();
            SupplierAddressLine1.Clear();
            SupplierAddressLine2.Clear();
            SupplierCity.Clear();
            SupplierState.Clear();
            SupplierCreationDate.SelectedDate = null;
            SupplierImage.Clear();
            _selectedImagePath = string.Empty;
        }

        public bool AreInputsNotEmpty()
        {
            if (string.IsNullOrWhiteSpace(SupplierImage.Text) ||
                string.IsNullOrWhiteSpace(SupplierFullName.Text) ||
                string.IsNullOrWhiteSpace(SupplierPhoneNumber.Text) ||
                string.IsNullOrWhiteSpace(SupplierAddressLine1.Text) ||
                string.IsNullOrWhiteSpace(SupplierAddressLine2.Text) ||
                string.IsNullOrWhiteSpace(SupplierCity.Text) ||
                string.IsNullOrWhiteSpace(SupplierState.Text) ||
                SupplierCreationDate.SelectedDate == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                return false;
            }
            return true;
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!AreInputsNotEmpty()) return;

                string supplierId = Guid.NewGuid().ToString("N");
                string rootPath = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName ?? string.Empty;
                string folderPath = Path.Combine(rootPath, "Assets", "Suppliers Images");
                string destinationPath = Path.Combine(folderPath, $"{supplierId}.png");

                string query = @"
                    INSERT INTO suppliers (
                        ID,
                        FullName,
                        PhoneNumber,
                        AddressLine1,
                        AddressLine2,
                        City,
                        State,
                        CreateDate,
                        ImagePath
                    ) VALUES (
                        @ID,
                        @FullName,
                        @PhoneNumber,
                        @AddressLine1,
                        @AddressLine2,
                        @City,
                        @State,
                        @CreateDate,
                        @ImagePath
                    )";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@ID", supplierId),
                    new MySqlParameter("@FullName", SupplierFullName.Text.Trim()),
                    new MySqlParameter("@PhoneNumber", SupplierPhoneNumber.Text.Trim()),
                    new MySqlParameter("@AddressLine1", SupplierAddressLine1.Text.Trim()),
                    new MySqlParameter("@AddressLine2", SupplierAddressLine2.Text.Trim()),
                    new MySqlParameter("@City", SupplierCity.Text.Trim()),
                    new MySqlParameter("@State", SupplierState.Text.Trim()),
                    new MySqlParameter("@CreateDate", SupplierCreationDate.SelectedDate.Value.ToString("yyyy-MM-dd")),
                    new MySqlParameter("@ImagePath", destinationPath)
                };

                if (_db.InsertData(query, parameters))
                {
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    if (!string.IsNullOrEmpty(_selectedImagePath) && File.Exists(_selectedImagePath))
                    {
                        File.Copy(_selectedImagePath, destinationPath, overwrite: true);
                        _selectedImagePath = string.Empty;
                    }

                    MessageBox.Show("Data inserted successfully!");
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("No rows were inserted. Check your data or database connection.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to insert supplier data.\n\nDetails:\n{ex.Message}");
            }
        }
    }
}
