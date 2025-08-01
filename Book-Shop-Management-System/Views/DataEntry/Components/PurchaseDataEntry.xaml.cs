using Book_Shop_Management_System.Configrations;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Book_Shop_Management_System.UserControls
{
    public class PurchaseComboBoxItem
    {
        public string DisplayText { get; set; }
        public string Value { get; set; }
        public override string ToString() => DisplayText;
    }

    public partial class PurchaseDataEntry : UserControl
    {
        private readonly MySQLConnector _db = new MySQLConnector();

        public PurchaseDataEntry()
        {
            InitializeComponent();
            LoadBooks();
            LoadSuppliers();
        }

        public void ClearInputs()
        {
            PurchaseQuantity.Clear();
            PurchaseETA.Clear();
            PurchaseReceived.Clear();
            PurchaseDate.SelectedDate = null;
            PurchaseBookID.SelectedIndex = -1;
            PurchaseSupplierID.SelectedIndex = -1;
        }

        public void LoadBooks()
        {
            try
            {
                string query = "SELECT ID, Name FROM books";
                using DataTable dt = _db.FetchData(query);
                PurchaseBookID.Items.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    PurchaseBookID.Items.Add(new PurchaseComboBoxItem
                    {
                        DisplayText = row["Name"].ToString(),
                        Value = row["ID"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load books.\n{ex.Message}");
            }
        }

        public void LoadSuppliers()
        {
            try
            {
                string query = "SELECT ID, FullName FROM suppliers";
                using DataTable dt = _db.FetchData(query);
                PurchaseSupplierID.Items.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    PurchaseSupplierID.Items.Add(new PurchaseComboBoxItem
                    {
                        DisplayText = row["FullName"].ToString(),
                        Value = row["ID"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load suppliers.\n{ex.Message}");
            }
        }

        public bool AreInputsValid()
        {
            if (PurchaseBookID.SelectedItem == null ||
                PurchaseSupplierID.SelectedItem == null ||
                string.IsNullOrWhiteSpace(PurchaseQuantity.Text) ||
                string.IsNullOrWhiteSpace(PurchaseETA.Text) ||
                string.IsNullOrWhiteSpace(PurchaseReceived.Text) ||
                PurchaseDate.SelectedDate == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                return false;
            }

            if (!int.TryParse(PurchaseQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid positive quantity.");
                return false;
            }

            // You might want to validate ETA and Received fields as well depending on your logic

            return true;
        }

        public void Submit(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!AreInputsValid()) return;

                string purchaseId = Guid.NewGuid().ToString("N");

                string insertQuery = @"
                    INSERT INTO purchases (ID, Book, Supplier, Quantity, Date, ETA, Received) 
                    VALUES (@ID, @Book, @Supplier, @Quantity, @Date, @ETA, @Received)";

                var insertParams = new MySqlParameter[]
                {
                    new MySqlParameter("@ID", purchaseId),
                    new MySqlParameter("@Book", ((PurchaseComboBoxItem)PurchaseBookID.SelectedItem).Value),
                    new MySqlParameter("@Supplier", ((PurchaseComboBoxItem)PurchaseSupplierID.SelectedItem).Value),
                    new MySqlParameter("@Quantity", int.Parse(PurchaseQuantity.Text)),
                    new MySqlParameter("@Date", PurchaseDate.SelectedDate.Value.ToString("yyyy-MM-dd")),
                    new MySqlParameter("@ETA", PurchaseETA.Text.Trim()),
                    new MySqlParameter("@Received", PurchaseReceived.Text.Trim())
                };

                if (_db.InsertData(insertQuery, insertParams))
                {
                    string updateQuery = "UPDATE books SET Quantity = Quantity + @Quantity WHERE ID = @BookID";
                    var updateParams = new MySqlParameter[]
                    {
                        new MySqlParameter("@Quantity", int.Parse(PurchaseQuantity.Text)),
                        new MySqlParameter("@BookID", ((PurchaseComboBoxItem)PurchaseBookID.SelectedItem).Value)
                    };

                    if (_db.UpdateData(updateQuery, updateParams))
                    {
                        MessageBox.Show("Purchase recorded and book quantity updated successfully.");
                        ClearInputs();
                    }
                    else
                    {
                        MessageBox.Show("Purchase recorded but failed to update book quantity.");
                    }
                }
                else
                {
                    MessageBox.Show("Failed to record purchase. Please check your inputs or database connection.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during submission: {ex.Message}");
            }
        }
    }
}
