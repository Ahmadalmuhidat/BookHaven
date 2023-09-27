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
using Book_Shop_Management_System.Pages;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;

namespace Book_Shop_Management_System.UserControls
{
    public class ComboBoxItem
    {
        public string DisplayText { get; set; }
        public string Value { get; set; }
    }

    public partial class BookDataEntry : UserControl
    {
        public BookDataEntry()
        {
            InitializeComponent();
            getSuppliers();
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
                                BookSupplier.Items.Add(new ComboBoxItem {
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
                MessageBox.Show(ex.ToString());
            }
        }

        public void clear()
        {
            BookID.Clear();
            BookName.Clear();
            BookAuthor.Clear();
            BookPrice.Clear();
            BookQuantity.Clear();
        }

        public void submit(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("Server=localhost;Uid=root;Pwd=root;database=book_system");
            connection.Open();

            string insertQuery = "INSERT INTO books (BookID, BookName, BookAuthor, BookPrice, BookQuantity, BookSupplier) VALUES (@param1, @param2, @param3, @param4, @param5, @param6)";

            using (MySqlCommand cmd = new MySqlCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("@param1", BookID.Text);
                cmd.Parameters.AddWithValue("@param2", BookName.Text);
                cmd.Parameters.AddWithValue("@param3", BookAuthor.Text);
                cmd.Parameters.AddWithValue("@param4", BookPrice.Text);
                cmd.Parameters.AddWithValue("@param5", BookQuantity.Text);
                cmd.Parameters.AddWithValue("@param6", BookSupplier.SelectedValue.ToString());

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
