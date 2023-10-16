using Book_Shop_Management_System.DB;
using Book_Shop_Management_System.Pages.Profiles;
using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Book_Shop_Management_System.Pages
{
    public partial class EmployeesDatabase : Page
    {
        public string EID;
        private MySQLConnector DB = new MySQLConnector();

        public class EmployeesDataItem
        {
            public string EmployeeID { get; set; }
            public string EmployeeFullName { get; set; }
            public string EmployeeAdressLine1 { get; set; }
            public string EmployeeAdressLine2 { get; set; }
            public string EmployeeAdressCity { get; set; }
            public string EmployeeAdressState { get; set; }
            public string EmployeePhoneNumber { get; set; }
            public string EmployeeDateOfJoining { get; set; }
            public string EmployeeSalary { get; set; }
            public string EmployeeMGRStatus { get; set; }
            public string ButtonEmployeeID { get; set; }
        }

        public EmployeesDatabase()
        {
            InitializeComponent();
            getEmployees();
        }

        public void getEmployees()
        {
            try
            {
                String query = "select * from employees where EmployeeID>0";
                using (var reader = DB.FetchData(query))
                {
                    foreach (DataRow row in reader.Rows)
                    {
                        Employees.Items.Add(new EmployeesDataItem
                        {
                            EmployeeID = row["EmployeeID"].ToString(),
                            EmployeeFullName = row["EmployeeFullName"].ToString(),
                            EmployeeAdressLine1 = row["EmployeeAdressLine1"].ToString(),
                            EmployeeAdressLine2 = row["EmployeeAdressLine2"].ToString(),
                            EmployeeAdressCity = row["EmployeeAdressCity"].ToString(),
                            EmployeeAdressState = row["EmployeeAdressState"].ToString(),
                            EmployeePhoneNumber = row["EmployeePhoneNumber"].ToString(),
                            EmployeeDateOfJoining = row["EmployeeDateOfJoining"].ToString(),
                            EmployeeSalary = row["EmployeeSalary"].ToString(),
                            EmployeeMGRStatus = row["EmployeeMGRStatus"].ToString(),
                            ButtonEmployeeID = row["EmployeeID"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void goToEmployeeProfile(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button.CommandParameter != null)
            {
                String ButtonEmployeeID = (String)button.CommandParameter;
                EmployeeProfile employeeProfile = new EmployeeProfile(ButtonEmployeeID);
                NavigationService.Navigate(employeeProfile);
            }
        }

        private void delete(object sender, RoutedEventArgs e)
        {
            try
            {
                EmployeesDataItem classObj = Employees.SelectedItem as EmployeesDataItem;
                String id = classObj.EmployeeID;
                String[] values = { id };
                String query = "DELETE FROM employees WHERE EmployeeID=" + id;

                if (DB.DeleteData(query))
                {
                    string RootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                    string DistinationFolder = RootPath + "/Assets/Employees Images/" + id + ".png";

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
                    Employees.Items.Clear();
                    getEmployees();
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
