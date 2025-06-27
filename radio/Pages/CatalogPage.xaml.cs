using System;
using System.Data;
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
using System.Diagnostics;

namespace radio.Pages
{
    /// <summary>
    /// Логика взаимодействия для CatalogPage.xaml
    /// </summary>
    public partial class CatalogPage : Page
    {
        private readonly MainViewModel _viewModel = new MainViewModel();

        public CatalogPage()
        {
            InitializeComponent();
        }

        private void RadioStationsButton_Click(object sender, RoutedEventArgs e)
        {
            var page = new RadioStationsPage();
            page.LoadData(_viewModel.GetProductsByCategory(1)); // ID категории радиостанций
            NavigationService?.Navigate(page);
        }

        private void PortableRadiosButton_Click(object sender, RoutedEventArgs e)
        {
            var page = new PortableRadiosPage();
            page.LoadData(_viewModel.GetProductsByCategory(2)); // ID портативных радиостанций
            NavigationService?.Navigate(page);
        }

        private void AntennasButton_Click(object sender, RoutedEventArgs e)
        {
            var page = new AntennasPage();
            page.LoadData(_viewModel.GetProductsByCategory(3)); // ID категории антенн
            NavigationService?.Navigate(page);
        }

        private void MeasureButton_Click(object sender, RoutedEventArgs e)
        {
            var page = new MeasurePage();
            page.LoadData(_viewModel.GetProductsByCategory(4)); // ID измерительных приборов
            NavigationService?.Navigate(page);
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            var cartPage = new CartPage();

            // Получаем CartViewModel из MainWindow
            if (Application.Current.MainWindow?.DataContext is MainWindowViewModel mainVM)
            {
                cartPage.DataContext = mainVM.CartViewModel;
            }
            else
            {
                // Отладочное сообщение
                Debug.WriteLine("Не удалось получить MainWindowViewModel");
            }

            NavigationService?.Navigate(cartPage);
        }
    }
}
