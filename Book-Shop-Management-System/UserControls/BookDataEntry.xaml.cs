using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Book_Shop_Management_System.DB;
using Microsoft.Win32;
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
        private String selectedImagePath;
        private MySQLConnector DB = new MySQLConnector();

        public BookDataEntry()
        {
            InitializeComponent();
            getSuppliers();
        }

        private void SelectImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                selectedImagePath = openFileDialog.FileName;
                BookImage.Text = selectedImagePath;
            }
        }

        public void getSuppliers()
        {
            try
            {
                String query = "select SupplierID, SupplierFullName from suppliers";
                using (var reader = DB.FetchData(query))
                {
                    foreach (DataRow row in reader.Rows)
                    {
                        BookSupplier.Items.Add(new ComboBoxItem
                        {
                            DisplayText = row["SupplierFullName"].ToString(),
                            Value = row["SupplierID"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void clearInputs()
        {
            BookID.Clear();
            BookName.Clear();
            BookAuthor.Clear();
            BookPrice.Clear();
            BookQuantity.Clear();
        }

        public bool areInputsNotEmpty()
        {
            if (string.IsNullOrWhiteSpace(BookImage.Text) ||
                string.IsNullOrWhiteSpace(BookID.Text) ||
                string.IsNullOrWhiteSpace(BookName.Text) ||
                string.IsNullOrWhiteSpace(BookAuthor.Text) ||
                string.IsNullOrWhiteSpace(BookPrice.Text) ||
                string.IsNullOrWhiteSpace(BookQuantity.Text) ||
                BookSupplier.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                return false;
            }
            return true;
        }

        public void submit(object sender, RoutedEventArgs e)
        {
            try
            {
                if (areInputsNotEmpty())
                {
                    String RootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                    String DistinationFolder = RootPath + "/Assets/Books Images/" + BookID.Text + ".png";

                    String query = "INSERT INTO books (BookID, BookName, BookAuthor, BookPrice, BookQuantity, BookSupplier, BookImage)";
                    String[] values = {
                        BookID.Text,
                        BookName.Text,
                        BookAuthor.Text,
                        BookPrice.Text,
                        BookQuantity.Text,
                        BookSupplier.SelectedValue.ToString(),
                        DistinationFolder
                    };

                    if (DB.InsertData(query, values))
                    {
                        System.IO.File.Copy(selectedImagePath, DistinationFolder, true);
                        selectedImagePath = "";
                        MessageBox.Show("Data inserted successfully!");
                        clearInputs();
                    }
                    else
                    {
                        MessageBox.Show("No rows were inserted. Check your data or database.");
                    }
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}