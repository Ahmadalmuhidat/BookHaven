using Book_Shop_Management_System.DB;
using Book_Shop_Management_System.Pages.Profiles;
using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

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

        public void getMembers()
        {
            try
            {
                String query = "select * from members";
                using (var reader = DB.FetchData(query))
                {
                    foreach (DataRow row in reader.Rows)
                    {
                        Members.Items.Add(new MemberDataItem
                        {
                            MemberID = row["MemberID"].ToString(),
                            MemberFullName = row["MemberFullName"].ToString(),
                            MemberAddressLine1 = row["MemberAddressLine1"].ToString(),
                            MemberAddressLine2 = row["MemberAddressLine2"].ToString(),
                            MemberAddressCity = row["MemberAddressCity"].ToString(),
                            MemberAddressState = row["MemberAddressState"].ToString(),
                            MemberPhoneNumber = row["MemberPhoneNumber"].ToString(),
                            MemberEndDate = row["MemberEndDate"].ToString(),
                            MemberBeginDate = row["MemberBeginDate"].ToString(),
                            MemberValid = row["MemberValid"].ToString(),
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
                MemberDataItem classObj = Members.SelectedItem as MemberDataItem;
                String id = classObj.MemberID;
                String[] values = { id };
                String query = "DELETE FROM members WHERE MemberID=" + id;

                if (DB.DeleteData(query))
                {
                    string RootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                    string DistinationFolder = RootPath + "/Assets/Members Images/" + id + ".png";

                    if (File.Exists(DistinationFolder))
                    {
                        File.Delete(DistinationFolder);
                        Console.WriteLine("File deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("File not found.");
                    }

                    MessageBox.Show("Data has beem deleted successfully!");
                    Members.Items.Clear();
                    getMembers();
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
