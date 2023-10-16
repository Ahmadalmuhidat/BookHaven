using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using MySql.Data.MySqlClient;


namespace Book_Shop_Management_System.Pages.Profiles
{
    public class cPurchasesDataItem
    {
        public string PurchaseID { get; set; }
        public string PurchaseBookID { get; set; }
        public string PurchaseSupplierID { get; set; }
        public string PurchaseQuantity { get; set; }
        public string PurchaseDate { get; set; }
        public string PurchaseETA { get; set; }
        public string PurchaseReceived { get; set; }
    }

    public partial class SupplierProfile : Page
    {
        public SupplierProfile(String SupplierID)
        {
            InitializeComponent();
            getSupplierInfo(SupplierID);
            getPreviousPurchases(SupplierID);
        }

        public void load_image(String ID)
        {
            String RootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            String AssetsPath = RootPath + "/Assets/Suppliers Images/" + ID + ".png";
            SupplierImage.Source = new BitmapImage(new Uri(AssetsPath));
        }

        public void getPreviousPurchases(String BID)
        {
            try
            {
                var connstr = "Server=localhost;Uid=root;Pwd=root;database=book_system";
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from purchases WHERE PurchaseSupplierID=@param1";
                        cmd.Parameters.AddWithValue("@param1", BID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Purchases.Items.Add(new cPurchasesDataItem
                                {
                                    PurchaseID = reader["PurchaseID"].ToString(),
                                    PurchaseBookID = reader["PurchaseBookID"].ToString(),
                                    PurchaseSupplierID = reader["PurchaseSupplierID"].ToString(),
                                    PurchaseQuantity = reader["PurchaseQuantity"].ToString(),
                                    PurchaseDate = reader["PurchaseDate"].ToString(),
                                    PurchaseETA = reader["PurchaseETA"].ToString(),
                                    PurchaseReceived = reader["PurchaseReceived"].ToString(),
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

    public void getSupplierInfo(String BID)
        {
            try
            {
                var connstr = "Server=localhost;Uid=root;Pwd=root;database=book_system";
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from suppliers WHERE SupplierID=@param1";
                        cmd.Parameters.AddWithValue("@param1", BID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                load_image(reader["SupplierID"].ToString());

                                SupplierID.Inlines.Add(new Run("Supplier ID: ") { FontWeight = FontWeights.Bold });
                                SupplierID.Inlines.Add(new Run(reader["SupplierID"].ToString()) { FontWeight = FontWeights.Regular });

                                SupplierFullName.Inlines.Add(new Run("Full Name: ") { FontWeight = FontWeights.Bold });
                                SupplierFullName.Inlines.Add(new Run(reader["SupplierFullName"].ToString()) { FontWeight = FontWeights.Regular });

                                SupplierPhoneNumber.Inlines.Add(new Run("Phone Number: ") { FontWeight = FontWeights.Bold });
                                SupplierPhoneNumber.Inlines.Add(new Run(reader["SupplierID"].ToString()) { FontWeight = FontWeights.Regular });

                                SupplierAddressLine1.Inlines.Add(new Run("Address Line 1: ") { FontWeight = FontWeights.Bold });
                                SupplierAddressLine1.Inlines.Add(new Run(reader["SupplierAddressLine1"].ToString()) { FontWeight = FontWeights.Regular });

                                SupplierAddressLine2.Inlines.Add(new Run("Address Line 2: ") { FontWeight = FontWeights.Bold });
                                SupplierAddressLine2.Inlines.Add(new Run(reader["SupplierAddressLine2"].ToString()) { FontWeight = FontWeights.Regular });

                                SupplierCity.Inlines.Add(new Run("City: ") { FontWeight = FontWeights.Bold });
                                SupplierCity.Inlines.Add(new Run(reader["SupplierCity"].ToString()) { FontWeight = FontWeights.Regular });

                                SupplierState.Inlines.Add(new Run("State: ") { FontWeight = FontWeights.Bold });
                                SupplierState.Inlines.Add(new Run(reader["SupplierState"].ToString()) { FontWeight = FontWeights.Regular });

                                SupplierCreateDate.Inlines.Add(new Run("Create Date: ") { FontWeight = FontWeights.Bold });
                                SupplierCreateDate.Inlines.Add(new Run(reader["SupplierCreateDate"].ToString()) { FontWeight = FontWeights.Regular });
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
    }
}
