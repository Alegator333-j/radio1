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
        private CartViewModel _cartVM; // Ссылка на корзину

        public AntennasPage()
        {
            InitializeComponent();

            if (Application.Current.MainWindow?.DataContext is MainWindowViewModel mainVM)
            {
                _cartVM = mainVM.CartViewModel;
            }
        }

        public void LoadData(DataTable data)
        {
            if (data == null || data.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных об антеннах");
                return;
            }

            // Создаем коллекцию продуктов из DataTable
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

        // Обработчик кнопки "Добавить в корзину"
        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Product product)
            {
                var mainWindow = Application.Current.MainWindow;
                if (mainWindow?.DataContext is MainWindowViewModel mainVM)
                {
                    mainVM.CartViewModel.AddToCart(product);
                    MessageBox.Show($"{product.Name} добавлен в корзину!");
                }
            }
        }
    }
}
