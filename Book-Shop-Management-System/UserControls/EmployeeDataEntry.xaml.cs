using Book_Shop_Management_System.DB;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Book_Shop_Management_System.UserControls
{
    public partial class EmployeeDataEntry : UserControl
    {
        private String selectedImagePath;
        private MySQLConnector DB = new MySQLConnector();

        public EmployeeDataEntry()
        {
            InitializeComponent();
        }

        private void SelectImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                selectedImagePath = openFileDialog.FileName;
                EmployeeImage.Text = selectedImagePath;
            }
        }

        public void clearInputs()
        {
            EmployeeFullName.Clear();
            EmployeeAddressLine1.Clear();
            EmployeeAddressLine2.Clear();
            EmployeeAddressCity.Clear();
            EmployeeAddressState.Clear();
            EmployeePhoneNumber.Clear();
            EmployeeSalary.Clear();
            EmployeeMGRStatus.Clear();
            EmployeeDateOfJoining.SelectedDate = null;
            EmployeeImage.Clear();
        }

        public bool areInputsNotEmpty()
        {
            if (string.IsNullOrWhiteSpace(EmployeeImage.Text) ||
                string.IsNullOrWhiteSpace(EmployeeFullName.Text) ||
                string.IsNullOrWhiteSpace(EmployeeAddressLine1.Text) ||
                string.IsNullOrWhiteSpace(EmployeeAddressLine2.Text) ||
                string.IsNullOrWhiteSpace(EmployeeAddressCity.Text) ||
                string.IsNullOrWhiteSpace(EmployeeAddressState.Text) ||
                string.IsNullOrWhiteSpace(EmployeePhoneNumber.Text) ||
                string.IsNullOrWhiteSpace(EmployeeSalary.Text) ||
                EmployeeDateOfJoining.SelectedDate == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                return false;
            }
            return true;
        }

        public void submit(object sender, RoutedEventArgs e)
        {
            try
            {
                if (areInputsNotEmpty())
                {
                    Random random = new Random();
                    String EmployeeID = random.Next(1, 1000).ToString();
                    String RootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                    String DistinationFolder = RootPath + "/Assets/Employees Images/" + EmployeeID + ".png";
                    String query = "INSERT INTO employees (EmployeeID, EmployeeFullName, EmployeeAdressLine1, EmployeeAdressLine2, EmployeeAdressCity, EmployeeAdressState, EmployeePhoneNumber, EmployeeDateOfJoining, EmployeeSalary, EmployeeMGRStatus, EmployeeImagePath)";
                    String[] values = {
                        EmployeeID,
                        EmployeeFullName.Text,
                        EmployeeAddressLine1.Text,
                        EmployeeAddressLine2.Text,
                        EmployeeAddressCity.Text,
                        EmployeeAddressState.Text,
                        EmployeePhoneNumber.Text,
                        EmployeeDateOfJoining.SelectedDate.Value.ToString("yyyy-MM-dd"),
                        EmployeeSalary.Text,
                        EmployeeMGRStatus.Text,
                        DistinationFolder,
                    };

                    if (DB.InsertData(query, values))
                    {
                        System.IO.File.Copy(selectedImagePath, DistinationFolder, true);
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