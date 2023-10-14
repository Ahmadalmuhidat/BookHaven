using Book_Shop_Management_System.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace Book_Shop_Management_System
{
    public partial class MainWindow : Window
    {
        public NavigationService NavigationService { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            NavigationService = ContentFrame.NavigationService;
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
            data_entry_button.IsChecked = false;

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
            else if (menuItem == data_entry_button)
            {
                ContentFrame.Content = new DataEntry();
            }
        }
    }
}