using Book_Shop_Management_System.DB;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Book_Shop_Management_System.Pages
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
    }

    public partial class PurchasesDatabase : Page
    {
        private MySQLConnector DB = new MySQLConnector();
        public PurchasesDatabase()
        {
            InitializeComponent();
            getPurchases();
        }

        public void getPurchases()
        {
            try
            {
                String query = "SELECT * FROM purchases INNER JOIN books ON books.BookID=purchases.PurchaseBookID INNER JOIN suppliers ON SupplierID=purchases.PurchaseSupplierID";
                using (var reader = DB.FetchData(query))
                {
                    foreach (DataRow row in reader.Rows)
                    {
                        Purchases.Items.Add(new PurchasesDataItem
                        {
                            PurchaseID = row["PurchaseID"].ToString(),
                            PurchaseBookID = row["BookName"].ToString(),
                            PurchaseSupplierID = row["SupplierFullName"].ToString(),
                            PurchaseQuantity = row["PurchaseQuantity"].ToString(),
                            PurchaseDate = row["PurchaseDate"].ToString(),
                            PurchaseETA = row["PurchaseETA"].ToString(),
                            PurchaseReceived = row["PurchaseReceived"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void delete(object sender, RoutedEventArgs e)
        {
            try
            {
                PurchasesDataItem classObj = Purchases.SelectedItem as PurchasesDataItem;
                String id = classObj.PurchaseID;
                String[] values = { id };
                String query = "DELETE FROM purchases WHERE PurchaseID=" + id;

                if (DB.DeleteData(query))
                {
                    MessageBox.Show("Data has beem deleted successfully!");
                    Purchases.Items.Clear();
                    getPurchases();
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
