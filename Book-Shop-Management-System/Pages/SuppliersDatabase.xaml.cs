using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Book_Shop_Management_System.Pages.Profiles;
using Book_Shop_Management_System.DB;
using System.Data;
using System.IO;

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
        private MySQLConnector DB = new MySQLConnector();
        public SuppliersDatabase()
        {
            InitializeComponent();
            getSuppliers();
        }

        public void getSuppliers()
        {
            try
            {
                String query = "select * from suppliers";
                using (var reader = DB.FetchData(query))
                {
                    foreach (DataRow row in reader.Rows)
                    {
                        suppliers_table.Items.Add(new SuppliersDataItem
                        {
                            SupplierID = row["SupplierID"].ToString(),
                            SupplierFullName = row["SupplierFullName"].ToString(),
                            SupplierPhoneNumber = row["SupplierPhoneNumber"].ToString(),
                            SupplierAddressLine1 = row["SupplierAddressLine1"].ToString(),
                            SupplierAddressLine2 = row["SupplierAddressLine2"].ToString(),
                            SupplierCity = row["SupplierCity"].ToString(),
                            SupplierState = row["SupplierState"].ToString(),
                            SupplierCreationDate = row["SupplierCreateDate"].ToString(),
                            ButtonSupplierID = row["SupplierID"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void goToSupplierProfile(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button.CommandParameter != null)
            {
                String ButtonSupplierID = (String)button.CommandParameter;
                SupplierProfile supplierProfile = new SupplierProfile(ButtonSupplierID);
                NavigationService.Navigate(supplierProfile);

            }
        }

        private void delete(object sender, RoutedEventArgs e)
        {
            try
            {
                SuppliersDataItem classObj = suppliers_table.SelectedItem as SuppliersDataItem;
                String id = classObj.SupplierID;
                String[] values = { id };
                String query = "DELETE FROM suppliers WHERE SupplierID=" + id;

                if (DB.DeleteData(query))
                {
                    string RootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                    string DistinationFolder = RootPath + "/Assets/Suppliers Images/" + id + ".png";

                    if (File.Exists(DistinationFolder))
                    {
                        File.Delete(DistinationFolder);
                        Console.WriteLine("File deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("File not found.");
                    }

                    MessageBox.Show("Data has beem deleted successfully!");
                    suppliers_table.Items.Clear();
                    getSuppliers();
                }
                else
                {
                    MessageBox.Show("No rows were deleted. Check your data or database.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
