using Book_Shop_Management_System.DB;
using Book_Shop_Management_System.Pages.Profiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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

        public void search(object sender, RoutedEventArgs e)
        {
            String searchQuery = search_input.Text;
            String query = "SELECT * FROM books WHERE BookName LIKE '%" + searchQuery + "%'";

            using (var reader = DB.FetchData(query))
            {
                try
                {
                    if (reader.Rows.Count > 0)
                    {
                        books_table.Items.Clear();

                        foreach (DataRow row in reader.Rows)
                        {
                            BID = row["BookID"].ToString();
                            books_table.Items.Add(new BooksDataItem
                            {
                                BookID = BID,
                                BookName = row["BookName"].ToString(),
                                BookAuthor = row["BookAuthor"].ToString(),
                                BookPrice = row["BookPrice"].ToString(),
                                BookQuantity = row["BookQuantity"].ToString(),
                                ButtonBookID = row["BookID"].ToString()
                            });
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(searchQuery))
                    {
                        books_table.Items.Clear();
                        getBooks();
                    }
                    else
                    {
                        MessageBox.Show("Sorry, book has not been found!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        public void getBooks()
        {
            try
            {
                String query = "SELECT * FROM books";
                using (var reader = DB.FetchData(query))
                {
                    foreach (DataRow row in reader.Rows)
                    {
                        BID = row["BookID"].ToString();
                        books_table.Items.Add(new BooksDataItem
                        {
                            BookID = BID,
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
                List<BooksDataItem> selectedBooks = books_table.SelectedItems.Cast<BooksDataItem>().ToList();

                foreach (BooksDataItem classObj in selectedBooks)
                {
                    String id = classObj.BookID;
                    String query = "DELETE FROM books WHERE BookID=" + id;

                    if (DB.DeleteData(query))
                    {
                        string RootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                        string DistinationFolder = RootPath + "/Assets/Books Images/" + id + ".png";

                        if (File.Exists(DistinationFolder))
                        {
                            File.Delete(DistinationFolder);
                            Console.WriteLine("File deleted successfully for BookID: " + id);
                        }
                        else
                        {
                            Console.WriteLine("File not found for BookID: " + id);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows were deleted for BookID: " + id);
                    }
                }

                MessageBox.Show("Data has been deleted successfully!");
                books_table.Items.Clear();
                getBooks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
