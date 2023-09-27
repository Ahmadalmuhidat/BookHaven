using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.IO;

namespace Book_Shop_Management_System.UserControls
{
    public partial class EmployeeDataEntry : UserControl
    {
        public string selectedImagePath;

        public EmployeeDataEntry()
        {
            InitializeComponent();
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            openFileDialog.Filter = "Image files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                this.selectedImagePath = openFileDialog.FileName;
                imagePreview.Source = new BitmapImage(new Uri(selectedImagePath));
            }
        }

        public void clear()
        {
            EmployeeID.Clear();
            EmployeeFullName.Clear();
            EmployeeAdressLine1.Clear();
            EmployeeAdressLine2.Clear();
            EmployeeAdressCity.Clear();
            EmployeeAdressState.Clear();
            EmployeePhoneNumber.Clear();
            EmployeeSalary.Clear();
            EmployeeMGRStatus.Clear();
            imagePreview.Source = null;
        }

        public void submit(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("Server=localhost;Uid=root;Pwd=root;database=book_system");
            connection.Open();

            string insertQuery = "INSERT INTO employees (EmployeeID, EmployeeFullName, EmployeeAdressLine1, EmployeeAdressLine2, EmployeeAdressCity, EmployeeAdressState, EmployeePhoneNumber, EmployeeDateOfJoining, EmployeeSalary, EmployeeMGRStatus, EmployeeImagePath) VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8, @param9, @param10, @param11)";
            using (MySqlCommand cmd = new MySqlCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("@param1", EmployeeID.Text);
                cmd.Parameters.AddWithValue("@param2", EmployeeFullName.Text);
                cmd.Parameters.AddWithValue("@param3", EmployeeAdressLine1.Text);
                cmd.Parameters.AddWithValue("@param4", EmployeeAdressLine2.Text);
                cmd.Parameters.AddWithValue("@param5", EmployeeAdressCity.Text);
                cmd.Parameters.AddWithValue("@param6", EmployeeAdressState.Text);
                cmd.Parameters.AddWithValue("@param7", EmployeePhoneNumber.Text);
                cmd.Parameters.AddWithValue("@param8", EmployeeDateOfJoining.SelectedDate.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@param9", EmployeeSalary.Text);
                cmd.Parameters.AddWithValue("@param10", EmployeeMGRStatus.Text);
                cmd.Parameters.AddWithValue("@param11", this.selectedImagePath);

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
