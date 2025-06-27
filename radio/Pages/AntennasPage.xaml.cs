using System;
using System.Collections.Generic;
using System.Data;
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
using System.Collections.ObjectModel;



namespace radio.Pages
{
    /// <summary>
    /// Логика взаимодействия для AntennasPage.xaml
    /// </summary>
    public partial class AntennasPage : Page
    {
        private readonly CartViewModel _cartViewModel;

        public AntennasPage()
        {
            InitializeComponent();
            _cartViewModel = GetCartViewModel();
            ProductsListView.ItemsSource = LoadAntennasFromDatabase();
        }

        private CartViewModel GetCartViewModel()
        {
            // Способ 1: Через MainWindowViewModel.Instance
            return MainWindowViewModel.Instance?.CartViewModel;

            // ИЛИ Способ 2: Через DataContext MainWindow
            // if (Application.Current.MainWindow?.DataContext is MainWindowViewModel mainVM)
            // {
            //     return mainVM.CartViewModel;
            // }
            // return null;
        }

        private ObservableCollection<Product> LoadAntennasFromDatabase()
        {
            var antennas = new ObservableCollection<Product>();
            // Пример загрузки данных (замените на ваш источник)
            antennas.Add(new Product { Id = 1, Name = "Антенна Diamond X50A", Price = 7999.99m });
            antennas.Add(new Product { Id = 2, Name = "Антенна Opek HVT-400B", Price = 11999.99m });
            return antennas;
        }

        public void LoadData(DataTable data)
        {
            if (data == null || data.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных об антеннах");
                return;
            }

            var products = new ObservableCollection<Product>();
            foreach (DataRow row in data.Rows)
            {
                products.Add(new Product
                {
                    Id = Convert.ToInt32(row["ID_Товара"]),
                    Name = row["Название"].ToString(),
                    Price = Convert.ToDecimal(row["Цена"]),
                    Description = row["Описание"].ToString(),
                    Manufacturer = row["НазваниеПроизводителя"].ToString()
                });
            }
            ProductsListView.ItemsSource = products;
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Product product)
            {
                try
                {
                    if (_cartViewModel != null)
                    {
                        _cartViewModel.AddToCart(product);
                        MessageBox.Show($"{product.Name} добавлен в корзину!", "Успех",
                                      MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        throw new NullReferenceException("Корзина не доступна");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении в корзину: {ex.Message}", "Ошибка",
                                   MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
