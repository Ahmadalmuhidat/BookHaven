using Book_Shop_Management_System.DB;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Book_Shop_Management_System.UserControls
{
    public partial class SupplierDataEntry : UserControl
    {
        private String selectedImagePath;
        private MySQLConnector DB = new MySQLConnector();

        public SupplierDataEntry()
        {
            InitializeComponent();
        }

        private void selectImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                this.selectedImagePath = openFileDialog.FileName;
                SupplierImage.Text = selectedImagePath;
            }
        }

        private void clearInputs()
        {
            SupplierFullName.Clear();
            SupplierPhoneNumber.Clear();
            SupplierAddressLine1.Clear();
            SupplierAddressLine2.Clear();
            SupplierCity.Clear();
            SupplierState.Clear();
            SupplierCreationDate.SelectedDate = null;
            SupplierImage.Clear();
        }

        public bool areInputsNotEmpty()
        {
            if (string.IsNullOrWhiteSpace(SupplierImage.Text) ||
                string.IsNullOrWhiteSpace(SupplierFullName.Text) ||
                string.IsNullOrWhiteSpace(SupplierPhoneNumber.Text) ||
                string.IsNullOrWhiteSpace(SupplierAddressLine1.Text) ||
                string.IsNullOrWhiteSpace(SupplierAddressLine2.Text) ||
                string.IsNullOrWhiteSpace(SupplierCity.Text) ||
                string.IsNullOrWhiteSpace(SupplierState.Text) ||
                SupplierCreationDate.SelectedDate == null
                )
            {
                MessageBox.Show("Please fill in all required fields.");
                return false;
            }
            return true;
        }

        private void submit(object sender, RoutedEventArgs e)
        {
            try
            {
                if (areInputsNotEmpty())
                {
                    Random random = new Random();
                    String SupplierID = random.Next(1, 1000).ToString();
                    String RootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                    String DistinationPath = RootPath + "/Assets/Suppliers Images/" + SupplierID + ".png";
                    String query = "INSERT INTO suppliers (SupplierID, SupplierFullName, SupplierPhoneNumber, SupplierAddressLine1, SupplierAddressLine2, SupplierCity, SupplierState, SupplierCreateDate, SupplierImagePath)";
                    String[] values = {
                    SupplierID,
                    SupplierFullName.Text,
                    SupplierPhoneNumber.Text,
                    SupplierAddressLine1.Text,
                    SupplierAddressLine2.Text,
                    SupplierCity.Text,
                    SupplierState.Text,
                    SupplierCreationDate.SelectedDate.Value.ToString("yyyy-MM-dd"),
                    DistinationPath
                };
                    if (DB.InsertData(query, values))
                    {
                        System.IO.File.Copy(selectedImagePath, DistinationPath, true);
                        selectedImagePath = "";
                        MessageBox.Show("Data inserted successfully!");
                        clearInputs();
                    }
                    else
                    {
                        MessageBox.Show("No rows were inserted. Check your data or database.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}