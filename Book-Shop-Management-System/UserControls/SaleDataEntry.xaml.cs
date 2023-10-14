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
                String query = "select BookID, BookName from books";
                using (var reader = DB.FetchData(query))
                {
                    foreach (DataRow row in reader.Rows)
                    {
                        SaleBook.Items.Add(new SaleComboBoxItem
                        {
                            DisplayText = row["BookName"].ToString(),
                            Value = row["BookID"].ToString()
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
            SaleID.Clear();
            SaleInvoiceID.Clear();
            SaleQuantity.Clear();
            SaleAmount.Clear();
            SaleMember.SelectedValue = null;
            SaleBook.SelectedValue = null;
            SaleEmployee.SelectedValue = null;
            SaleDate.SelectedDate = null;
        }

        public bool areInputsNotEmpty()
        {
            if (string.IsNullOrWhiteSpace(SaleID.Text) ||
                string.IsNullOrWhiteSpace(SaleBook.SelectedValue.ToString()) ||
                string.IsNullOrWhiteSpace(SaleMember.SelectedValue.ToString()) ||
                string.IsNullOrWhiteSpace(SaleEmployee.SelectedValue.ToString()) ||
                string.IsNullOrWhiteSpace(SaleQuantity.Text) ||
                string.IsNullOrWhiteSpace(SaleInvoiceID.Text) ||
                string.IsNullOrWhiteSpace(SaleAmount.Text) ||
                SaleDate.SelectedDate == null
                )
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
                if(areInputsNotEmpty())
                {
                    String query = "INSERT INTO sales (SaleID, SaleInvoiceID, SaleMemberID, SaleBookID, SaleEmployeeID, SaleQuantity, SaleAmount, SaleDate)";
                    String[] values = {
                    SaleID.Text,
                    SaleInvoiceID.Text,
                    SaleMember.SelectedValue.ToString(),
                    SaleBook.SelectedValue.ToString(),
                    SaleEmployee.SelectedValue.ToString(),
                    SaleQuantity.Text,
                    SaleAmount.Text,
                    SaleDate.SelectedDate.Value.ToString("yyyy-MM-dd"),
                };

                    if (DB.InsertData(query, values))
                    {
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