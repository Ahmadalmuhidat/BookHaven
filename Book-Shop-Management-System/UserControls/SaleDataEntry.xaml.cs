using Book_Shop_Management_System.DB;
using System;
using System.Data;
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

    public partial class SaleDataEntry : UserControl
    {
        private MySQLConnector DB = new MySQLConnector();

        public SaleDataEntry()
        {
            InitializeComponent();
            getEmployees();
            getMembers();
            getBooks();
        }

        public void getEmployees()
        {
            try
            {
                String query = "select EmployeeID, EmployeeFullName from employees";
                using (var reader = DB.FetchData(query))
                {
                    foreach (DataRow row in reader.Rows)
                    {
                        SaleEmployee.Items.Add(new SaleComboBoxItem
                        {
                            DisplayText = row["EmployeeFullName"].ToString(),
                            Value = row["EmployeeID"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void getMembers()
        {
            try
            {
                String query = "select MemberID, MemberFullName from members";
                using (var reader = DB.FetchData(query))
                {
                    foreach (DataRow row in reader.Rows)
                    {
                        SaleMember.Items.Add(new SaleComboBoxItem
                        {
                            DisplayText = row["MemberFullName"].ToString(),
                            Value = row["MemberID"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void getBooks()
        {
            try
            {
                String query = "select BookID, BookName, BookPrice from books";
                using (var reader = DB.FetchData(query))
                {
                    foreach (DataRow row in reader.Rows)
                    {
                        SaleBook.Items.Add(new SaleComboBoxItem
                        {
                            DisplayText = row["BookName"].ToString(),
                            Value = row["BookID"].ToString(),
                            Price = row["BookPrice"].ToString()
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
            SaleQuantity.Clear();
            SaleMember.SelectedValue = null;
            SaleBook.SelectedValue = null;
            SaleEmployee.SelectedValue = null;
            SaleDate.SelectedDate = null;
        }

        public bool areInputsNotEmpty()
        {
            if (string.IsNullOrWhiteSpace(SaleBook.SelectedValue.ToString()) ||
                string.IsNullOrWhiteSpace(SaleEmployee.SelectedValue.ToString()) ||
                string.IsNullOrWhiteSpace(SaleQuantity.Text) ||
                SaleDate.SelectedDate == null
                )
            {
                MessageBox.Show("Please fill in all required fields.");
                return false;
            }
            return true;
        }

        private String CalculateTotal(SaleComboBoxItem selectedItem, String quantity)
        {
            if (int.TryParse(quantity, out int saleQuantity) && decimal.TryParse(selectedItem.Price, out decimal price))
            {
                decimal totalPrice = price * saleQuantity;
                return totalPrice.ToString();
            }
            return null;
        }


        public void submit(object sender, RoutedEventArgs e)
        {
            try
            {
                if(areInputsNotEmpty())
                {
                    Random random = new Random();
                    String SaleID = random.Next(1, 1000).ToString();
                    SaleComboBoxItem selectedItem = (SaleComboBoxItem)SaleBook.SelectedItem;
                    String Total = CalculateTotal(selectedItem, SaleQuantity.Text);
                    String query = "INSERT INTO sales (SaleID, SaleMemberID, SaleBookID, SaleEmployeeID, SaleQuantity, SaleDate, SaleTotal)";
                    String[] values = {
                        SaleID,
                        SaleMember.SelectedValue.ToString(),
                        SaleBook.SelectedValue.ToString(),
                        SaleEmployee.SelectedValue.ToString(),
                        SaleQuantity.Text,
                        SaleDate.SelectedDate.Value.ToString("yyyy-MM-dd"),
                        Total
                    };
                    if (DB.InsertData(query, values))
                    {
                        query = "UPDATE books SET BookQuantity=BookQuantity - " + SaleQuantity.Text + " WHERE BOOKID=" + SaleBook.SelectedValue.ToString();
                        if (DB.UpdateData(query))
                        {
                            MessageBox.Show("Total: " + Total);
                            MessageBox.Show("Data inserted successfully!");
                            clearInputs();
                        }
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