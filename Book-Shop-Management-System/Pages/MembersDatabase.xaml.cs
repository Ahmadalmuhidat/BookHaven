using Book_Shop_Management_System.Pages.Profiles;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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

namespace Book_Shop_Management_System.Pages
{
    public partial class MembersDatabase : Page
    {

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
            mysql();
        }

        private void mysql()
        {
            try
            {
                var connstr = "Server=localhost;Uid=root;Pwd=root;database=book_system";
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from members";
                        // cmd.Parameters.AddWithValue("@ID", "100");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Members.Items.Add(new MemberDataItem
                                {
                                    MemberID = reader["MemberID"].ToString(),
                                    MemberFullName = reader["MemberFullName"].ToString(),
                                    MemberAddressLine1 = reader["MemberAddressLine1"].ToString(),
                                    MemberAddressLine2 = reader["MemberAddressLine2"].ToString(),
                                    MemberAddressCity = reader["MemberAddressCity"].ToString(),
                                    MemberAddressState = reader["MemberAddressState"].ToString(),
                                    MemberPhoneNumber = reader["MemberPhoneNumber"].ToString(),
                                    MemberEndDate = reader["MemberEndDate"].ToString(),
                                    MemberBeginDate = reader["MemberBeginDate"].ToString(),
                                    MemberValid = reader["MemberValid"].ToString(),
                                    ButtonMemberID = reader["MemberID"].ToString(),
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
    }
}
