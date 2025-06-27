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

            // Инициализация подключения к БД
            var connectionString = App.Configuration?.GetConnectionString("RadioDB");

            // Установка начальной страницы
            NavButton_Click(btnCatalog, null);

            // Установка контекста данных
            DataContext = MainWindowViewModel.Instance;
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button)) return;

            ResetNavButtonsStyle();

            // Обработка клика по логотипу (если кнопка без контента и не в StackPanel)
            if (button.Content == null && !(button.Parent is StackPanel))
            {
                MainContentFrame.Navigate(new CatalogPage());
                return;
            }

            // Установка стиля для активной кнопки
            button.Style = (Style)FindResource("SelectedNavButtonStyle");

            // Навигация на соответствующую страницу
            NavigateToPage(button.Name);
        }

        private void ResetNavButtonsStyle()
        {
            if (btnCatalog != null)
                btnCatalog.Style = (Style)FindResource("NavButtonStyle");
            if (btnOrder != null)
                btnOrder.Style = (Style)FindResource("NavButtonStyle");
            if (btnDelivery != null)
                btnDelivery.Style = (Style)FindResource("NavButtonStyle");
            if (btnContacts != null)
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

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            // Навигация на страницу корзины
            MainContentFrame.Navigate(new CartPage());

            // Сброс стилей навигационных кнопок
            ResetNavButtonsStyle();

            // Установка стиля для кнопки корзины, если нужно
            if (sender is Button cartButton)
            {
                cartButton.Style = (Style)FindResource("SelectedNavButtonStyle");
            }
        }
    }
}