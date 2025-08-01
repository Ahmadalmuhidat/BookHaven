using Book_Shop_Management_System.Configrations;
using Book_Shop_Management_System.Pages.Profiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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
            GetBooks();
        }

        public void Search(object sender, RoutedEventArgs e)
        {
            String searchQuery = search_input.Text;
            String query = "SELECT * FROM books WHERE Name LIKE '%" + searchQuery + "%'";

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
                                BookName = row["Name"].ToString(),
                                BookAuthor = row["Author"].ToString(),
                                BookPrice = row["Price"].ToString(),
                                BookQuantity = row["Quantity"].ToString(),
                                ButtonBookID = row["ID"].ToString()
                            });
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(searchQuery))
                    {
                        books_table.Items.Clear();
                        GetBooks();
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

        public void GetBooks()
        {
            try
            {
                String query = "SELECT * FROM books";
                using (var reader = DB.FetchData(query))
                {
                    foreach (DataRow row in reader.Rows)
                    {
                        BID = row["ID"].ToString();
                        books_table.Items.Add(new BooksDataItem
                        {
                            BookID = BID,
                            BookName = row["Name"].ToString(),
                            BookAuthor = row["Author"].ToString(),
                            BookPrice = row["Price"].ToString(),
                            BookQuantity = row["Quantity"].ToString(),
                            ButtonBookID = row["ID"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void GoToBookProfile(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button.CommandParameter != null)
            {
                String ButtonBookID = (String)button.CommandParameter;
                BookProfile bookProfilePage = new BookProfile(ButtonBookID);
                NavigationService.Navigate(bookProfilePage);
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                List<BooksDataItem> selectedBooks = books_table.SelectedItems.Cast<BooksDataItem>().ToList();

                foreach (BooksDataItem classObj in selectedBooks)
                {
                    String id = classObj.BookID;
                    String query = "DELETE FROM books WHERE ID=" + id;

                    if (DB.DeleteData(query))
                    {
                        string RootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                        string DistinationFolder = RootPath + "/Assets/Books Images/" + id + ".png";

                        if (File.Exists(DistinationFolder))
                        {
                            File.Delete(DistinationFolder);
                            Console.WriteLine("File deleted successfully for Book ID: " + id);
                        }
                        else
                        {
                            Console.WriteLine("File not found for Book ID: " + id);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows were deleted for Book ID: " + id);
                    }
                }

                MessageBox.Show("Data has been deleted successfully!");
                books_table.Items.Clear();
                GetBooks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
