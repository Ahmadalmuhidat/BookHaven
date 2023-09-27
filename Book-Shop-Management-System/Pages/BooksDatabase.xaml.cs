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
using static Book_Shop_Management_System.Pages.EmployeesDatabase;

namespace Book_Shop_Management_System.Pages
{
    public partial class BooksDatabase : Page
    {
        public String BID;

        public class BooksDataItem
        {
            public string BookID { get; set; }
            public string BookName { get; set; }
            public string BookAuthor { get; set; }
            public string BookPrice { get; set; }
            public string BookQuantity { get; set; }
            public string ButtonBookID { get; set; }
        }

        public BooksDatabase()
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
                        cmd.CommandText = "select * from books";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                this.BID = reader["BookID"].ToString();
                                books_table.Items.Add(new BooksDataItem
                                {
                                    BookID = reader["BookID"].ToString(),
                                    BookName = reader["BookName"].ToString(),
                                    BookAuthor = reader["BookAuthor"].ToString(),
                                    BookPrice = reader["BookPrice"].ToString(),
                                    BookQuantity = reader["BookQuantity"].ToString(),
                                    ButtonBookID = reader["BookID"].ToString()
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

        private void goToBookProfile(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button.CommandParameter != null)
            {
                String ButtonBookID = (String)button.CommandParameter;
                BookProfile bookProfilePage = new BookProfile(ButtonBookID);
                NavigationService.Navigate(bookProfilePage);
            }
        }
    }
}
