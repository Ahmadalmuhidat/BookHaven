using Book_Shop_Management_System.Configrations;
using Book_Shop_Management_System.Pages.Profiles;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Book_Shop_Management_System.Pages
{
    public class SuppliersDataItem
    {
        public string SupplierID { get; set; }
        public string SupplierFullName { get; set; }
        public string SupplierPhoneNumber { get; set; }
        public string SupplierAddressLine1 { get; set; }
        public string SupplierAddressLine2 { get; set; }
        public string SupplierCity { get; set; }
        public string SupplierState { get; set; }
        public string SupplierCreationDate { get; set; }
        public string ButtonSupplierID { get; set; }
    }

    public partial class SuppliersDatabase : Page
    {
        private readonly MySQLConnector _db = new MySQLConnector();

        public SuppliersDatabase()
        {
            InitializeComponent();
            LoadSuppliers();
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            string searchQuery = search_input.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                LoadSuppliers();
                return;
            }

            const string query = "SELECT * FROM suppliers WHERE FullName LIKE @searchQuery";

            var parameters = new Dictionary<string, object>
            {
                { "@searchQuery", $"%{searchQuery}%" }
            };

            PopulateSuppliers(query, parameters);
        }

        private void LoadSuppliers()
        {
            const string query = "SELECT * FROM suppliers";
            PopulateSuppliers(query);
        }

        private void PopulateSuppliers(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                suppliers_table.Items.Clear();

                MySqlParameter[] sqlParams = null;
                if (parameters != null)
                {
                    sqlParams = parameters.Select(kv => new MySqlParameter(kv.Key, kv.Value ?? DBNull.Value)).ToArray();
                }

                using DataTable results = sqlParams == null ? _db.FetchData(query) : _db.FetchData(query, sqlParams);

                if (results.Rows.Count == 0)
                {
                    MessageBox.Show("No suppliers found.");
                    return;
                }

                foreach (DataRow row in results.Rows)
                {
                    suppliers_table.Items.Add(new SuppliersDataItem
                    {
                        SupplierID = row["ID"].ToString(),
                        SupplierFullName = row["FullName"].ToString(),
                        SupplierPhoneNumber = row["PhoneNumber"].ToString(),
                        SupplierAddressLine1 = row["AddressLine1"].ToString(),
                        SupplierAddressLine2 = row["AddressLine2"].ToString(),
                        SupplierCity = row["City"].ToString(),
                        SupplierState = row["State"].ToString(),
                        SupplierCreationDate = row["CreateDate"].ToString(),
                        ButtonSupplierID = row["ID"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load suppliers.\n\nDetails:\n{ex.Message}");
            }
        }


        private void GoToSupplierProfile(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.CommandParameter is not string supplierId)
                return;

            var supplierProfile = new SupplierProfile(supplierId);
            NavigationService?.Navigate(supplierProfile);
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedSuppliers = suppliers_table.SelectedItems.Cast<SuppliersDataItem>().ToList();

                if (!selectedSuppliers.Any())
                {
                    MessageBox.Show("No supplier selected for deletion.");
                    return;
                }

                foreach (var supplier in selectedSuppliers)
                {
                    const string deleteQuery = "DELETE FROM suppliers WHERE ID = @id";

                    MySqlParameter[] sqlParams = {
                new MySqlParameter("@id", supplier.SupplierID)
            };

                    if (_db.DeleteData(deleteQuery, sqlParams))
                    {
                        DeleteSupplierImage(supplier.SupplierID);
                        Console.WriteLine($"Supplier deleted successfully: ID={supplier.SupplierID}");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to delete supplier: ID={supplier.SupplierID}");
                    }
                }

                MessageBox.Show("Selected suppliers deleted successfully.");
                LoadSuppliers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting suppliers.\n\nDetails:\n{ex.Message}");
            }
        }

        private void DeleteSupplierImage(string supplierId)
        {
            try
            {
                string rootPath = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
                if (string.IsNullOrEmpty(rootPath)) return;

                string imagePath = Path.Combine(rootPath, "Assets", "Suppliers Images", $"{supplierId}.png");

                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                    Console.WriteLine($"Deleted image for Supplier ID: {supplierId}");
                }
                else
                {
                    Console.WriteLine($"Image not found for Supplier ID: {supplierId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete image for Supplier ID: {supplierId}. Error: {ex.Message}");
            }
        }
    }
}
