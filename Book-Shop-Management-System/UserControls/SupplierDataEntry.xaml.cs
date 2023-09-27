using Microsoft.Win32;
using MySql.Data.MySqlClient;
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

namespace Book_Shop_Management_System.UserControls
{
    public partial class SupplierDataEntry : UserControl
    {
        public string selectedImagePath;
        public SupplierDataEntry()
        {
            InitializeComponent();
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // Load and display the selected image
                this.selectedImagePath = openFileDialog.FileName;
                imagePreview.Source = new BitmapImage(new Uri(selectedImagePath));
            }
        }

        public void clear()
        {
            SupplierID.Clear();
            SupplierFullName.Clear();
            SupplierPhoneNumber.Clear();
            SupplierAddressLine1.Clear();
            SupplierAddressLine2.Clear();
            SupplierCity.Clear();
            SupplierState.Clear();
            imagePreview.Source = null;
        }

        public void submit(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("Server=localhost;Uid=root;Pwd=root;database=book_system");
            connection.Open();

            string insertQuery = "INSERT INTO suppliers (SupplierID, SupplierFullName, SupplierPhoneNumber, SupplierAddressLine1, SupplierAddressLine2, SupplierCity, SupplierState, SupplierCreateDate, SupplierImagePath) VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8, @param9)";

            using (MySqlCommand cmd = new MySqlCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("@param1", SupplierID.Text);
                cmd.Parameters.AddWithValue("@param2", SupplierFullName.Text);
                cmd.Parameters.AddWithValue("@param3", SupplierPhoneNumber.Text);
                cmd.Parameters.AddWithValue("@param4", SupplierAddressLine1.Text);
                cmd.Parameters.AddWithValue("@param5", SupplierAddressLine2.Text);
                cmd.Parameters.AddWithValue("@param6", SupplierCity.Text);
                cmd.Parameters.AddWithValue("@param7", SupplierState.Text);
                cmd.Parameters.AddWithValue("@param8", SupplierCreationDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@param9", this.selectedImagePath);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Data inserted successfully!");
                    clear();
                }
                else
                {
                    MessageBox.Show("No rows were inserted. Check your data or database.");
                }
            }
            connection.Close();
        }
    }
}
