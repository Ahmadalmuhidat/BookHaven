using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;


namespace Book_Shop_Management_System.Pages.Profiles
{
    public class PurchasesDataItem
    {
        public string PurchaseID { get; set; }
        public string PurchaseBookID { get; set; }
        public string PurchaseSupplierID { get; set; }
        public string PurchaseQuantity { get; set; }
        public string PurchaseDate { get; set; }
        public string PurchaseETA { get; set; }
        public string PurchaseReceived { get; set; }
        public string PurchaseInvoice { get; set; }
    }

    public partial class BookProfile : Page
    {
        public BookProfile(String BID)
        {
            InitializeComponent();
            getBookInfo(BID);
            getPreviousPurchases(BID);
        }

        public void getPreviousPurchases(String BID)
        {
            try
            {
                var connstr = "Server=localhost;Uid=root;Pwd=root;database=book_system";
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from purchases WHERE PurchaseBooKID=@param1";
                        cmd.Parameters.AddWithValue("@param1", BID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Purchases.Items.Add(new PurchasesDataItem
                                {
                                    PurchaseID = reader["PurchaseID"].ToString(),
                                    PurchaseBookID = reader["PurchaseBookID"].ToString(),
                                    PurchaseSupplierID = reader["PurchaseSupplierID"].ToString(),
                                    PurchaseQuantity = reader["PurchaseQuantity"].ToString(),
                                    PurchaseDate = reader["PurchaseDate"].ToString(),
                                    PurchaseETA = reader["PurchaseETA"].ToString(),
                                    PurchaseReceived = reader["PurchaseReceived"].ToString(),
                                    PurchaseInvoice = reader["PurchaseInvoice"].ToString(),
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


        public void getBookInfo(String BID)
        {
            try
            {
                var connstr = "Server=localhost;Uid=root;Pwd=root;database=book_system";
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from books WHERE BooKID=@param1";
                        cmd.Parameters.AddWithValue("@param1", BID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BookID.Text = "Book ID: " + reader["BookID"].ToString();
                                BookName.Text = "Book Name: " + reader["BookName"].ToString();
                                BookAuthor.Text = "Book Author: " + reader["BookAuthor"].ToString();
                                BookPrice.Text = "Book Price: " + reader["BookPrice"].ToString();
                                BookQuantity.Text = "Book Quantity: " + reader["BookQuantity"].ToString();
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