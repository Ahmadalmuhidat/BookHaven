using Book_Shop_Management_System.DB;
using Book_Shop_Management_System.Pages.Profiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using static Book_Shop_Management_System.Pages.EmployeesDatabase;

namespace Book_Shop_Management_System.Pages
{
    public partial class MembersDatabase : Page
    {
        private MySQLConnector DB = new MySQLConnector();

        public class MemberDataItem
        {
            public string MemberID { get; set; }
            public string MemberFullName { get; set; }
            public string MemberAddressLine1 { get; set; }
            public string MemberAddressLine2 { get; set; }
            public string MemberAddressCity { get; set; }
            public string MemberAddressState { get; set; }
            public string MemberPhoneNumber { get; set; }
            public string MemberEndDate { get; set; }
            public string MemberBeginDate { get; set; }
            public string MemberValid { get; set; }
            public string ButtonMemberID { get; set; }
        }

        public MembersDatabase()
        {
            InitializeComponent();
            getMembers();
        }

        public static bool IsMembershipValid(DateTime beginDate, DateTime endDate, DateTime checkDate)
        {
            return (checkDate >= beginDate && checkDate <= endDate);
        }

        public void search(object sender, RoutedEventArgs e)
        {
            try
            {
                String searchQuery = search_input.Text;
                String query = "SELECT * FROM members WHERE MemberID > 0 AND MemberFullName LIKE '%" + searchQuery + "%'";

                using (var reader = DB.FetchData(query))
                {
                    if (reader.Rows.Count > 0)
                    {
                        Members.Items.Clear();
                        DateTime todayDate = DateTime.Now;

                        foreach (DataRow row in reader.Rows)
                        {
                            DateTime beginDate = DateTime.Parse(row["MemberBeginDate"].ToString());
                            DateTime endDate = DateTime.Parse(row["MemberEndDate"].ToString());
                            bool membership = IsMembershipValid(beginDate, endDate, todayDate);

                            Members.Items.Add(new MemberDataItem
                            {
                                MemberID = row["MemberID"].ToString(),
                                MemberFullName = row["MemberFullName"].ToString(),
                                MemberAddressLine1 = row["MemberAddressLine1"].ToString(),
                                MemberAddressLine2 = row["MemberAddressLine2"].ToString(),
                                MemberAddressCity = row["MemberAddressCity"].ToString(),
                                MemberAddressState = row["MemberAddressState"].ToString(),
                                MemberPhoneNumber = row["MemberPhoneNumber"].ToString(),
                                MemberBeginDate = row["MemberBeginDate"].ToString(),
                                MemberEndDate = row["MemberEndDate"].ToString(),
                                MemberValid = (membership == true ? "valid" : "not valid"),
                                ButtonMemberID = row["MemberID"].ToString(),
                            });
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(searchQuery))
                    {
                        Members.Items.Clear();
                        getMembers();
                    }
                    else
                    {
                        MessageBox.Show("Sorry, member has not been found!");
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
                DateTime todayDate = DateTime.Now;
                String query = "select * from members where MemberID>0";
                using (var reader = DB.FetchData(query))
                {
                    foreach (DataRow row in reader.Rows)
                    {
                        DateTime beginDate = DateTime.Parse(row["MemberBeginDate"].ToString());
                        DateTime endDate = DateTime.Parse(row["MemberEndDate"].ToString());
                        bool membership = IsMembershipValid(beginDate, endDate, todayDate);

                        Members.Items.Add(new MemberDataItem
                        {
                            MemberID = row["MemberID"].ToString(),
                            MemberFullName = row["MemberFullName"].ToString(),
                            MemberAddressLine1 = row["MemberAddressLine1"].ToString(),
                            MemberAddressLine2 = row["MemberAddressLine2"].ToString(),
                            MemberAddressCity = row["MemberAddressCity"].ToString(),
                            MemberAddressState = row["MemberAddressState"].ToString(),
                            MemberPhoneNumber = row["MemberPhoneNumber"].ToString(),
                            MemberBeginDate = row["MemberBeginDate"].ToString(),
                            MemberEndDate = row["MemberEndDate"].ToString(),
                            MemberValid = (membership == true ? "valid" : "not valid"),
                            ButtonMemberID = row["MemberID"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void goToMemberProfile(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button.CommandParameter != null)
            {
                String ButtonMemberID = (String)button.CommandParameter;
                MemberProfile profile = new MemberProfile(ButtonMemberID);
                NavigationService.Navigate(profile);
            }
        }

        private void delete(object sender, RoutedEventArgs e)
        {
            try
            {
                List<MemberDataItem> selectedMembers = Members.SelectedItems.Cast<MemberDataItem>().ToList();

                foreach (MemberDataItem classObj in selectedMembers)
                {
                    String id = classObj.MemberID;
                    String query = "DELETE FROM members WHERE MemberID=" + id;

                    if (DB.DeleteData(query))
                    {
                        string RootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                        string DistinationFolder = RootPath + "/Assets/Members Images/" + id + ".png";

                        if (File.Exists(DistinationFolder))
                        {
                            File.Delete(DistinationFolder);
                            Console.WriteLine("File deleted successfully for ID: " + id);
                        }
                        else
                        {
                            Console.WriteLine("File not found for ID: " + id);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows were deleted for ID: " + id);
                    }
                }

                MessageBox.Show("Data has been deleted successfully!");
                Members.Items.Clear();
                getMembers(); // Refresh or update your member list after deletion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
