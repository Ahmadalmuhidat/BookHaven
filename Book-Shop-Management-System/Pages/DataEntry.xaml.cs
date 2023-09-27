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
    public partial class DataEntry : Page
    {
        public DataEntry()
        {
            InitializeComponent();
            NewBook.IsChecked = true;
            BookEntryFrame.Visibility = Visibility.Visible;
        }

        public void switch_frame(object sender, RoutedEventArgs e)
        {
            var cb = (CheckBox)sender;

            NewBook.IsChecked = false;
            NewEmployee.IsChecked = false;
            NewMember.IsChecked = false;
            NewPurchase.IsChecked = false;
            NewSale.IsChecked = false;
            NewSupplier.IsChecked = false;

            cb.IsChecked = true;

            BookEntryFrame.Visibility=Visibility.Collapsed;
            EmployeeEntryFrame.Visibility=Visibility.Collapsed;
            MemberEntryFrame.Visibility=Visibility.Collapsed;
            SaleEntryFrame.Visibility=Visibility.Collapsed;
            SupplierEntryFrame.Visibility=Visibility.Collapsed;
            PurchaseEntryFrame.Visibility = Visibility.Collapsed;

            if (cb == NewBook)
            {
                BookEntryFrame.Visibility = Visibility.Visible;
            }
            else if (cb == NewEmployee)
            {
                EmployeeEntryFrame.Visibility = Visibility.Visible;
            }
            else if (cb == NewMember)
            {
                MemberEntryFrame.Visibility = Visibility.Visible;
            }
            else if ( cb == NewPurchase)
            {
                PurchaseEntryFrame.Visibility = Visibility.Visible;
            }
            else if ( cb == NewSale)
            {
                SaleEntryFrame.Visibility = Visibility.Visible;
            }
            else if ( cb == NewSupplier)
            {
                SupplierEntryFrame.Visibility = Visibility.Visible;
            }
        }
    }
}
