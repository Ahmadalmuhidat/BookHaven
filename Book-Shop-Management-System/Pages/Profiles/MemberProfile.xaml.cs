using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Book_Shop_Management_System.Pages.Profiles
{
    public class dSalesDataItem
    {
        public string SaleID { get; set; }
        public string SaleMemberID { get; set; }
        public string SaleBookID { get; set; }
        public string SaleQuantity { get; set; }
        public string SaleDate { get; set; }
    }

    public partial class MemberProfile : Page
    {
        public MemberProfile(string EID)
        {
            InitializeComponent();
            getInfo(EID);
            getSales(EID);
        }

        public void load_image(String ID)
        {
            String RootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            String AssetsPath = RootPath + "/Assets/Members Images/" + ID + ".png";
            MemberImage.Source = new BitmapImage(new Uri(AssetsPath));
        }

        public void getInfo(string EID)
        {
            try
            {
                var connstr = "Server=localhost;Uid=root;Pwd=root;database=book_system";
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from members WHERE MemberID=@param1";
                        cmd.Parameters.AddWithValue("@param1", EID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                load_image(reader["MemberID"].ToString());

                                MemberID.Inlines.Add(new Run("Member ID: ") { FontWeight = FontWeights.Bold });
                                MemberID.Inlines.Add(new Run(reader["MemberID"].ToString()) { FontWeight = FontWeights.Regular });

                                MemberFullName.Inlines.Add(new Run("Full Name: ") { FontWeight = FontWeights.Bold });
                                MemberFullName.Inlines.Add(new Run(reader["MemberFullName"].ToString()) { FontWeight = FontWeights.Regular });

                                MemberAddressLine1.Inlines.Add(new Run("Address Line 1: ") { FontWeight = FontWeights.Bold });
                                MemberAddressLine1.Inlines.Add(new Run(reader["MemberAddressLine1"].ToString()) { FontWeight = FontWeights.Regular });

                                MemberAddressLine2.Inlines.Add(new Run("Address Line 2: ") { FontWeight = FontWeights.Bold });
                                MemberAddressLine2.Inlines.Add(new Run(reader["MemberAddressLine2"].ToString()) { FontWeight = FontWeights.Regular });

                                MemberAddressCity.Inlines.Add(new Run("Address City: ") { FontWeight = FontWeights.Bold });
                                MemberAddressCity.Inlines.Add(new Run(reader["MemberAddressCity"].ToString()) { FontWeight = FontWeights.Regular });

                                MemberPhoneNumber.Inlines.Add(new Run("Phone Number: ") { FontWeight = FontWeights.Bold });
                                MemberPhoneNumber.Inlines.Add(new Run(reader["MemberPhoneNumber"].ToString()) { FontWeight = FontWeights.Regular });

                                MembershipBeginDate.Inlines.Add(new Run("Membership Begin Date: ") { FontWeight = FontWeights.Bold });
                                MembershipBeginDate.Inlines.Add(new Run(reader["MemberBeginDate"].ToString()) { FontWeight = FontWeights.Regular });

                                MembershipEndDate.Inlines.Add(new Run("Membership End Date: ") { FontWeight = FontWeights.Bold });
                                MembershipEndDate.Inlines.Add(new Run(reader["MemberEndDate"].ToString()) { FontWeight = FontWeights.Regular });

                                AddressState.Inlines.Add(new Run("Addres State: ") { FontWeight = FontWeights.Bold });
                                AddressState.Inlines.Add(new Run(reader["MemberAddressState"].ToString()) { FontWeight = FontWeights.Regular });

                                MemberValid.Inlines.Add(new Run("Membership Valid: ") { FontWeight = FontWeights.Bold });
                                MemberValid.Inlines.Add(new Run(reader["MemberValid"].ToString()) { FontWeight = FontWeights.Regular });
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

        public void getSales(string EID)
        {
            try
            {
                var connstr = "Server=localhost;Uid=root;Pwd=root;database=book_system";
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from sales WHERE SaleMemberID=@param1";
                        cmd.Parameters.AddWithValue("@param1", EID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Sales.Items.Add(new dSalesDataItem
                                {
                                    SaleID = reader["SaleID"].ToString(),
                                    SaleMemberID = reader["SaleMemberID"].ToString(),
                                    SaleBookID = reader["SaleBookID"].ToString(),
                                    SaleQuantity = reader["SaleQuantity"].ToString(),
                                    SaleDate = reader["SaleDate"].ToString(),
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
    }
}
