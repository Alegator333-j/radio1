using Microsoft.Extensions.Configuration;
using radio.Pages;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace radio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var connectionString = App.Configuration.GetConnectionString("RadioDB");
            NavButton_Click(btnCatalog, null);
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            ResetNavButtonsStyle();

            // Если кликнули по логотипу
            if (button.Content == null && button.Parent is StackPanel == false)
            {
                MainContentFrame.Navigate(new CatalogPage());
                return;
            }

            if (button != null)
            {
                button.Style = (Style)FindResource("SelectedNavButtonStyle");
                NavigateToPage(button.Name);
            }
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            // Обработка клика по кнопке корзины
            MainContentFrame.Navigate(new CartPage());
        }

        private void ResetNavButtonsStyle()
        {
            btnCatalog.Style = (Style)FindResource("NavButtonStyle");
            btnOrder.Style = (Style)FindResource("NavButtonStyle");
            btnDelivery.Style = (Style)FindResource("NavButtonStyle");
            btnContacts.Style = (Style)FindResource("NavButtonStyle");
        }


        private void NavigateToPage(string buttonName)
        {
            switch (buttonName)
            {
                case "btnCatalog":
                    MainContentFrame.Navigate(new CatalogPage());
                    break;
                case "btnOrder":
                    MainContentFrame.Navigate(new OrderPage());
                    break;
                case "btnDelivery":
                    MainContentFrame.Navigate(new DeliveryPage());
                    break;
                case "btnContacts":
                    MainContentFrame.Navigate(new ContactsPage());
                    break;
                default:
                    MainContentFrame.Navigate(new CatalogPage());
                    break;
            }
        }
    }
}