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
    public class SaleComboBoxItem
    {
        public string DisplayText { get; set; }
        public string Value { get; set; }
    }

    public partial class SaleDataEntry : UserControl
    {
        public SaleDataEntry()
        {
            InitializeComponent();
            getEmployees();
            getMembers();
            getBooks();
        }

        public void getEmployees()
        {
            try
            {
                var connstr = "Server=localhost;Uid=root;Pwd=root;database=book_system";
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select EmployeeID, EmployeeFullName from employees";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SaleEmployee.Items.Add(new SaleComboBoxItem
                                {
                                    DisplayText = reader["EmployeeFullName"].ToString(),
                                    Value = reader["EmployeeID"].ToString()
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

        public void getMembers()
        {
            try
            {
                var connstr = "Server=localhost;Uid=root;Pwd=root;database=book_system";
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select MemberID, MemberFullName from members";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SaleMember.Items.Add(new SaleComboBoxItem
                                {
                                    DisplayText = reader["MemberFullName"].ToString(),
                                    Value = reader["MemberID"].ToString()
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

        public void getBooks()
        {
            try
            {
                var connstr = "Server=localhost;Uid=root;Pwd=root;database=book_system";
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select BookID, BookName from books";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SaleBook.Items.Add(new SaleComboBoxItem
                                {
                                    DisplayText = reader["BookName"].ToString(),
                                    Value = reader["BookID"].ToString()
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

        public void clear()
        {
            SaleID.Clear();
            SaleInvoiceID.Clear();
            SaleQuantity.Clear();
            SaleAmount.Clear();
        }

        public void submit(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("Server=localhost;Uid=root;Pwd=root;database=book_system");
            connection.Open();

            string insertQuery = "INSERT INTO sales (SaleID, SaleInvoiceID, SaleMemberID, SaleBookID, SaleEmployeeID, SaleQuantity, SaleAmount, SaleDate) VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8)";

            using (MySqlCommand cmd = new MySqlCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("@param1", SaleID.Text);
                cmd.Parameters.AddWithValue("@param2", SaleInvoiceID.Text);
                cmd.Parameters.AddWithValue("@param3", SaleMember.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@param4", SaleBook.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@param5", SaleEmployee.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@param6", SaleQuantity.Text);
                cmd.Parameters.AddWithValue("@param7", SaleAmount.Text);
                cmd.Parameters.AddWithValue("@param8", SaleDate.SelectedDate.Value.ToString("yyyy-MM-dd"));

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
