using Book_Shop_Management_System.DB;
using Book_Shop_Management_System.Pages.Profiles;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Book_Shop_Management_System.Pages
{
    public partial class BooksDatabase : Page
    {
        private MySQLConnector DB = new MySQLConnector();
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
            getBooks();
        }

        public void getBooks()
        {
            try
            {
                String query = "select * from books";
                using (var reader = DB.FetchData(query))
                {
                    foreach (DataRow row in reader.Rows)
                    {
                        BID = row["BookID"].ToString();
                        books_table.Items.Add(new BooksDataItem
                        {
                            BookID = row["BookID"].ToString(),
                            BookName = row["BookName"].ToString(),
                            BookAuthor = row["BookAuthor"].ToString(),
                            BookPrice = row["BookPrice"].ToString(),
                            BookQuantity = row["BookQuantity"].ToString(),
                            ButtonBookID = row["BookID"].ToString()
                        });
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

        private void delete(object sender, RoutedEventArgs e)
        {
            try
            {
                BooksDataItem classObj = books_table.SelectedItem as BooksDataItem;
                String id = classObj.BookID;
                String[] values = { id };
                String query = "DELETE FROM books WHERE BookID=" + id;

                if (DB.DeleteData(query))
                {
                    string RootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                    string DistinationFolder = RootPath + "/Assets/Books Images/" + id + ".png";

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
                    books_table.Items.Clear();
                    getBooks();
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
