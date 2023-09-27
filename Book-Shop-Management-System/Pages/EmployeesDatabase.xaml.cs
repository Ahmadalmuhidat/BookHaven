using Book_Shop_Management_System.Pages.Profiles;
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
using static Book_Shop_Management_System.Pages.MembersDatabase;

namespace Book_Shop_Management_System.Pages
{
    public partial class EmployeesDatabase : Page
    {
        public string EID;

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
            mysql();
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
                        cmd.CommandText = "select * from employees";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                this.EID = reader["EmployeeID"].ToString();
                                Employees.Items.Add(new EmployeesDataItem
                                {
                                    EmployeeID = reader["EmployeeID"].ToString(),
                                    EmployeeFullName = reader["EmployeeFullName"].ToString(),
                                    EmployeeAdressLine1 = reader["EmployeeAdressLine1"].ToString(),
                                    EmployeeAdressLine2 = reader["EmployeeAdressLine2"].ToString(),
                                    EmployeeAdressCity = reader["EmployeeAdressCity"].ToString(),
                                    EmployeeAdressState = reader["EmployeeAdressState"].ToString(),
                                    EmployeePhoneNumber = reader["EmployeePhoneNumber"].ToString(),
                                    EmployeeDateOfJoining = reader["EmployeeDateOfJoining"].ToString(),
                                    EmployeeSalary = reader["EmployeeSalary"].ToString(),
                                    EmployeeMGRStatus = reader["EmployeeMGRStatus"].ToString(),
                                    ButtonEmployeeID = reader["EmployeeID"].ToString()
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
    }
}
