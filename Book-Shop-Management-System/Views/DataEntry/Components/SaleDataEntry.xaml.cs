using Book_Shop_Management_System.Configrations;
using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Book_Shop_Management_System.UserControls
{
    public class SaleComboBoxItem
    {
        public string DisplayText { get; set; }
        public string Value { get; set; }
        public string Price { get; set; }
    }

    public class SaleEntryRow : DependencyObject
    {
        // Use DependencyProperties for proper binding updates
        public SaleComboBoxItem Book
        {
            get { return (SaleComboBoxItem)GetValue(BookProperty); }
            set { SetValue(BookProperty, value); }
        }
        public static readonly DependencyProperty BookProperty = DependencyProperty.Register("Book", typeof(SaleComboBoxItem), typeof(SaleEntryRow), new PropertyMetadata(null));

        public string Quantity
        {
            get { return (string)GetValue(QuantityProperty); }
            set { SetValue(QuantityProperty, value); }
        }
        public static readonly DependencyProperty QuantityProperty = DependencyProperty.Register("Quantity", typeof(string), typeof(SaleEntryRow), new PropertyMetadata(string.Empty));

        public SaleComboBoxItem Employee
        {
            get { return (SaleComboBoxItem)GetValue(EmployeeProperty); }
            set { SetValue(EmployeeProperty, value); }
        }
        public static readonly DependencyProperty EmployeeProperty = DependencyProperty.Register("Employee", typeof(SaleComboBoxItem), typeof(SaleEntryRow), new PropertyMetadata(null));
    }

    public partial class SaleDataEntry : UserControl
    {
        private readonly MySQLConnector DB = new MySQLConnector();

        public ObservableCollection<SaleComboBoxItem> Books { get; set; } = new ObservableCollection<SaleComboBoxItem>();
        public ObservableCollection<SaleComboBoxItem> Members { get; set; } = new ObservableCollection<SaleComboBoxItem>();

        public ObservableCollection<SaleEntryRow> SaleEntries { get; set; } = new ObservableCollection<SaleEntryRow>();

        public SaleDataEntry()
        {
            InitializeComponent();
            this.DataContext = this;

            LoadBooks();

            // Add placeholder sale entry row
            SaleEntries.Add(new SaleEntryRow
            {
                Book = Books.FirstOrDefault(),
                Quantity = ""
            });
        }

        private void LoadBooks()
        {
            Books.Clear();
            Books.Add(new SaleComboBoxItem { DisplayText = "Select Book", Value = "", Price = "0" });

            try
            {
                var rows = DB.FetchData("SELECT ID, Name, Price FROM books");
                foreach (DataRow row in rows.Rows)
                {
                    Books.Add(new SaleComboBoxItem
                    {
                        DisplayText = row["Name"].ToString(),
                        Value = row["ID"].ToString(),
                        Price = row["Price"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading books: " + ex.Message);
            }
        }

        private void AddRow(object sender, RoutedEventArgs e)
        {
            SaleEntries.Add(new SaleEntryRow
            {
                Book = Books.FirstOrDefault(),
                Quantity = ""
            });
        }

        private void SubmitSale(object sender, RoutedEventArgs e)
        {
            if (!SaleEntries.Any())
            {
                MessageBox.Show("No sale entries to submit.");
                return;
            }

            foreach (var entry in SaleEntries)
            {
                if (entry.Book == null || string.IsNullOrEmpty(entry.Book.Value) ||
                    string.IsNullOrWhiteSpace(entry.Quantity) ||
                    !int.TryParse(entry.Quantity, out int qty) || qty <= 0)
                {
                    MessageBox.Show("Please fill all sale entry rows correctly.");
                    return;
                }
            }

            try
            {
                string saleID = Guid.NewGuid().ToString("N");  // use full GUID
                string saleDate = DateTime.Now.ToString("yyyy-MM-dd");

                decimal grandTotal = 0m;

                foreach (var entry in SaleEntries)
                {
                    decimal price = decimal.Parse(entry.Book.Price);
                    int quantity = int.Parse(entry.Quantity);
                    grandTotal += price * quantity;
                }

                // Insert into `sales` table
                string insertSale = "INSERT INTO sales (ID, Date, Total) VALUES (?, ?, ?)";
                MySqlParameter[] saleParams = {
                    new MySqlParameter(null, saleID),
                    new MySqlParameter(null, saleDate),
                    new MySqlParameter(null, grandTotal.ToString("0.00"))
                };

                if (!DB.InsertData(insertSale, saleParams))
                {
                    MessageBox.Show("Failed to insert sale.");
                    return;
                }

                // Insert into `sale_items` table
                foreach (var entry in SaleEntries)
                {
                    decimal price = decimal.Parse(entry.Book.Price);
                    int quantity = int.Parse(entry.Quantity);

                    string insertItem = "INSERT INTO sale_items (Sale, Book, Quantity, UnitPrice) VALUES (?, ?, ?, ?)";
                    MySqlParameter[] itemParams = {
                        new MySqlParameter(null, saleID),
                        new MySqlParameter(null, entry.Book.Value),
                        new MySqlParameter(null, quantity),
                        new MySqlParameter(null, price)
                    };

                    if (!DB.InsertData(insertItem, itemParams))
                    {
                        MessageBox.Show("Failed to insert sale item.");
                        return;
                    }

                    // Update book quantity
                    DB.UpdateData($"UPDATE books SET Quantity = Quantity - {quantity} WHERE ID = '{entry.Book.Value}'");
                }

                MessageBox.Show("Sale completed successfully!");
                SaleEntries.Clear();
                SaleEntries.Add(new SaleEntryRow
                {
                    Book = Books.FirstOrDefault(),
                    Quantity = ""
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during sale: " + ex.Message);
            }
        }

        private void RemoveRow(object sender, RoutedEventArgs e)
        {
            // Get the Button that was clicked
            var button = sender as Button;
            if (button == null) return;

            // Get the SaleEntryRow associated with this button's DataContext
            var row = button.DataContext as SaleEntryRow;
            if (row == null) return;

            // Remove the row from the observable collection
            if (SaleEntries.Contains(row))
            {
                SaleEntries.Remove(row);
            }
        }

        private string CalculateTotal(SaleComboBoxItem item, string quantity)
        {
            if (int.TryParse(quantity, out int qty) && decimal.TryParse(item.Price, out decimal price))
            {
                return (qty * price).ToString("0.00");
            }
            return "0.00";
        }
    }
}
