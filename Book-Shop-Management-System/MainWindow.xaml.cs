using Book_Shop_Management_System.Pages;
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

namespace Book_Shop_Management_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            books_database_button.IsChecked = true;
            ContentFrame.Content = new BooksDatabase();
        }

        private void switch_page(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;

            if(menuItem.IsChecked)
            {
                return;
            }

            books_database_button.IsChecked = false;
            suppliers_database_button.IsChecked = false;
            purchases_database_button.IsChecked = false;
            employees_database_button.IsChecked = false;
            members_database_button.IsChecked = false;
            sales_database_button.IsChecked = false;

            menuItem.IsChecked = true;

            if (menuItem == books_database_button)
            {
                ContentFrame.Content = new BooksDatabase();
            }
            else if (menuItem == suppliers_database_button)
            {
                ContentFrame.Content = new SuppliersDatabase();
            }
            else if (menuItem == purchases_database_button)
            {
                ContentFrame.Content = new PurchasesDatabase();
            }
            else if (menuItem == employees_database_button)
            {
                ContentFrame.Content = new EmployeesDatabase();
            }
            else if (menuItem == members_database_button)
            {
                ContentFrame.Content = new MembersDatabase();
            }
            else if (menuItem == sales_database_button)
            {
                ContentFrame.Content = new SalesDatabase();
            }
        }
    }
}
