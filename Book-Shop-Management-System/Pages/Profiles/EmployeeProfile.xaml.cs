using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace Book_Shop_Management_System.Pages.Profiles
{
    public class SalesDataItem
    {
        public string SaleID { get; set; }
        public string SaleInvoiceID { get; set; }
        public string SaleMemberID { get; set; }
        public string SaleBookID { get; set; }
        public string SaleQuantity { get; set; }
        public string SaleAmount { get; set; }
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
                                    SaleInvoiceID = reader["SaleInvoiceID"].ToString(),
                                    SaleMemberID = reader["SaleMemberID"].ToString(),
                                    SaleBookID = reader["SaleBookID"].ToString(),
                                    SaleQuantity = reader["SaleQuantity"].ToString(),
                                    SaleAmount = reader["SaleAmount"].ToString(),
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
