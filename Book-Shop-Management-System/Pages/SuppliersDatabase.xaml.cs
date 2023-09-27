using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Book_Shop_Management_System.Pages.BooksDatabase;
using MySql.Data.MySqlClient;
using Book_Shop_Management_System.Pages.Profiles;

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
        public SuppliersDatabase()
        {
            InitializeComponent();
            mysql();

            // suppliers_table.Items.Add(new SuppliersDataItem { SupplierID = "111", SupplierFullName = "Done", SupplierPhoneNumber = "22", SupplierAddressLine1 = "hello there", SupplierAddressLine2 = "11-11-11", SupplierCity = "11-11-11", SupplierState = "11-11-11" });
        }

        private void mysql()
        {
            try
            {
                var connstr = "Server=localhost;Uid=root;Pwd=root;database=book_system";
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from suppliers";
                        // cmd.Parameters.AddWithValue("@ID", "100");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                suppliers_table.Items.Add(new SuppliersDataItem {
                                SupplierID = reader["SupplierID"].ToString(),
                                SupplierFullName = reader["SupplierFullName"].ToString(),
                                SupplierPhoneNumber = reader["SupplierPhoneNumber"].ToString(),
                                SupplierAddressLine1 = reader["SupplierAddressLine1"].ToString(),
                                SupplierAddressLine2 = reader["SupplierAddressLine2"].ToString(),
                                SupplierCity = reader["SupplierCity"].ToString(),
                                SupplierState = reader["SupplierState"].ToString(),
                                SupplierCreationDate = reader["SupplierCreateDate"].ToString(),
                                ButtonSupplierID = reader["SupplierID"].ToString()
                                });
                            }
                        }
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
    }
}
