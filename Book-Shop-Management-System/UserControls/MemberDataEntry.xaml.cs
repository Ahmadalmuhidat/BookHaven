using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Book_Shop_Management_System.UserControls
{
    public partial class MemberDataEntry : UserControl
    {
        private string selectedImagePath;

        public MemberDataEntry()
        {
            InitializeComponent();
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                selectedImagePath = openFileDialog.FileName;
                DisplaySelectedImage(selectedImagePath);
            }
        }

        private void DisplaySelectedImage(string imagePath)
        {
            imagePreview.Source = new BitmapImage(new Uri(imagePath));
        }

        private void ClearFields()
        {
            MemberID.Clear();
            MemberFullName.Clear();
            MemberAddressLine1.Clear();
            MemberAddressLine2.Clear();
            MemberAddressCity.Clear();
            MemberAddressState.Clear();
            MemberPhoneNumber.Clear();
            MemberValid.Clear();
            imagePreview.Source = null;
        }

        private void submit(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Uid=root;Pwd=root;database=book_system"))
            {
                connection.Open();
                string insertQuery = "INSERT INTO members (MemberID, MemberFullName, MemberAddressLine1, MemberAddressLine2, MemberAddressCity, MemberAddressState, MemberPhoneNumber, MemberBeginDate, MemberEndDate, MemberValid, MemberImagePath) VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8, @param9, @param10, @param11)";

                using (MySqlCommand cmd = new MySqlCommand(insertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@param1", MemberID.Text);
                    cmd.Parameters.AddWithValue("@param2", MemberFullName.Text);
                    cmd.Parameters.AddWithValue("@param3", MemberAddressLine1.Text);
                    cmd.Parameters.AddWithValue("@param4", MemberAddressLine2.Text);
                    cmd.Parameters.AddWithValue("@param5", MemberAddressCity.Text);
                    cmd.Parameters.AddWithValue("@param6", MemberAddressState.Text);
                    cmd.Parameters.AddWithValue("@param7", MemberPhoneNumber.Text);
                    cmd.Parameters.AddWithValue("@param8", MemberBeginDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@param9", MemberEndDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@param10", MemberValid.Text);
                    cmd.Parameters.AddWithValue("@param11", selectedImagePath);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data inserted successfully!");
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("No rows were inserted. Check your data or database.");
                    }
                }
            }
        }
    }
}
