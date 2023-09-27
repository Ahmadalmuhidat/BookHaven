using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class PurchaseComboBoxItem
    {
        public string DisplayText { get; set; }
        public string Value { get; set; }
    }

    public partial class PurchaseDataEntry : UserControl
    {
        public PurchaseDataEntry()
        {
            InitializeComponent();
            getBooks();
            getSuppliers();
        }

        public void clear()
        {
            PurchaseID.Clear();
            PurchaseQuantity.Clear();
            PurchaseETA.Clear();
            PurchaseReceived.Clear();
            PurchaseInvoice.Clear();
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
                                PurchaseBookID.Items.Add(new PurchaseComboBoxItem
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

        public void getSuppliers()
        {
            try
            {
                var connstr = "Server=localhost;Uid=root;Pwd=root;database=book_system";
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select SupplierID, SupplierFullName from suppliers";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PurchaseSupplierID.Items.Add(new PurchaseComboBoxItem
                                {
                                    DisplayText = reader["SupplierFullName"].ToString(),
                                    Value = reader["SupplierID"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex?.ToString());
            }
        }

        public void submit(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("Server=localhost;Uid=root;Pwd=root;database=book_system");
            connection.Open();

            string insertQuery = "INSERT INTO purchases (PurchaseID, PurchaseBookID, PurchaseSupplierID, PurchaseQuantity, PurchaseDate, PurchaseETA, PurchaseReceived, PurchaseInvoice) VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8)";
            using (MySqlCommand cmd = new MySqlCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("@param1", PurchaseID.Text);
                cmd.Parameters.AddWithValue("@param2", PurchaseBookID.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@param3", PurchaseSupplierID.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@param4", PurchaseQuantity.Text);
                cmd.Parameters.AddWithValue("@param5", PurchaseDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@param6", PurchaseETA.Text);
                cmd.Parameters.AddWithValue("@param7", PurchaseReceived.Text);
                cmd.Parameters.AddWithValue("@param8", PurchaseInvoice.Text);

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
