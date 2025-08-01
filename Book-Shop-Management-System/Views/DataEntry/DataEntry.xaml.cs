using System;
using System.Windows;
using System.Windows.Controls;

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
            NewPurchase.IsChecked = false;
            NewSale.IsChecked = false;
            NewSupplier.IsChecked = false;

            cb.IsChecked = true;

            BookEntryFrame.Visibility = Visibility.Collapsed;
            SaleEntryFrame.Visibility = Visibility.Collapsed;
            SupplierEntryFrame.Visibility = Visibility.Collapsed;
            PurchaseEntryFrame.Visibility = Visibility.Collapsed;

            if (cb == NewBook)
            {
                BookEntryFrame.Visibility = Visibility.Visible;
            }
            else if (cb == NewPurchase)
            {
                PurchaseEntryFrame.Visibility = Visibility.Visible;
            }
            else if (cb == NewSale)
            {
                SaleEntryFrame.Visibility = Visibility.Visible;
            }
            else if (cb == NewSupplier)
            {
                SupplierEntryFrame.Visibility = Visibility.Visible;
            }
        }
    }
}
