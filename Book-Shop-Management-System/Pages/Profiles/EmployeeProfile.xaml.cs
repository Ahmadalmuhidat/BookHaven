using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace Book_Shop_Management_System.Pages.Profiles
{
    public class SalesDataItem
    {
        public string SaleID { get; set; }
        public string SaleMemberID { get; set; }
        public string SaleBookID { get; set; }
        public string SaleQuantity { get; set; }
        public string SaleDate { get; set; }
    }

    public partial class EmployeeProfile : Page
    {
        public EmployeeProfile(String EID)
        {
            InitializeComponent();
            getInfo(EID);
            getSales(EID);
        }

        public void load_image(String ID)
        {
            String RootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            String AssetsPath = RootPath + "/Assets/Employees Images/" + ID + ".png";
            EmployeeImage.Source = new BitmapImage(new Uri(AssetsPath));
        }

        public void getInfo(string EID)
        {
            try
            {
                var connstr = "Server=localhost;Uid=root;Pwd=root;database=book_system";
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from employees WHERE EmployeeID=@param1";
                        cmd.Parameters.AddWithValue("@param1", EID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                load_image(reader["EmployeeID"].ToString());

                                EmployeeID.Inlines.Add(new Run("Employee ID: ") { FontWeight = FontWeights.Bold });
                                EmployeeID.Inlines.Add(new Run(reader["EmployeeID"].ToString()) { FontWeight = FontWeights.Regular });

                                EmployeeFullName.Inlines.Add(new Run("Full Name: ") { FontWeight = FontWeights.Bold });
                                EmployeeFullName.Inlines.Add(new Run(reader["EmployeeFullName"].ToString()) { FontWeight = FontWeights.Regular });

                                EmployeeAddressLine1.Inlines.Add(new Run("Address Line 1: ") { FontWeight = FontWeights.Bold });
                                EmployeeAddressLine1.Inlines.Add(new Run(reader["EmployeeAdressLine1"].ToString()) { FontWeight = FontWeights.Regular });

                                EmployeeAddressLine2.Inlines.Add(new Run("Address Line 2: ") { FontWeight = FontWeights.Bold });
                                EmployeeAddressLine2.Inlines.Add(new Run(reader["EmployeeAdressLine2"].ToString()) { FontWeight = FontWeights.Regular });

                                EmployeeAddressCity.Inlines.Add(new Run("Address City: ") { FontWeight = FontWeights.Bold });
                                EmployeeAddressCity.Inlines.Add(new Run(reader["EmployeeAdressCity"].ToString()) { FontWeight = FontWeights.Regular });

                                EmployeeAdressState.Inlines.Add(new Run("Addres State: ") { FontWeight = FontWeights.Bold });
                                EmployeeAdressState.Inlines.Add(new Run(reader["EmployeeAdressState"].ToString()) { FontWeight = FontWeights.Regular });

                                EmployeePhoneNumber.Inlines.Add(new Run("Phone Number: ") { FontWeight = FontWeights.Bold });
                                EmployeePhoneNumber.Inlines.Add(new Run(reader["EmployeePhoneNumber"].ToString()) { FontWeight = FontWeights.Regular });

                                EmployeeDateOfJoining.Inlines.Add(new Run("Phone Number: ") { FontWeight = FontWeights.Bold });
                                EmployeeDateOfJoining.Inlines.Add(new Run(reader["EmployeeDateOfJoining"].ToString()) { FontWeight = FontWeights.Regular });

                                EmployeeSalary.Inlines.Add(new Run("Salary: ") { FontWeight = FontWeights.Bold });
                                EmployeeSalary.Inlines.Add(new Run(reader["EmployeeSalary"].ToString()) { FontWeight = FontWeights.Regular });

                                EmployeeMGRStatus.Inlines.Add(new Run("MGR Status: ") { FontWeight = FontWeights.Bold });
                                EmployeeMGRStatus.Inlines.Add(new Run(reader["EmployeeMGRStatus"].ToString()) { FontWeight = FontWeights.Regular });
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

        public void getSales(string EID)
        {
            try
            {
                var connstr = "Server=localhost;Uid=root;Pwd=root;database=book_system";
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from sales WHERE SaleEmployeeID=@param1";
                        cmd.Parameters.AddWithValue("@param1", EID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Sales.Items.Add(new SalesDataItem
                                {
                                    SaleID = reader["SaleID"].ToString(),
                                    SaleMemberID = reader["SaleMemberID"].ToString(),
                                    SaleBookID = reader["SaleBookID"].ToString(),
                                    SaleQuantity = reader["SaleQuantity"].ToString(),
                                    SaleDate = reader["SaleDate"].ToString(),
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
    }
}
