using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace radio.Pages
{
    /// <summary>
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        public CartPage()
        {
            InitializeComponent();

            Debug.WriteLine($"Инициализация CartPage. DataContext is CartViewModel: {DataContext is CartViewModel}");

            if (Application.Current.MainWindow?.DataContext is MainWindowViewModel mainVM)
            {
                DataContext = mainVM.CartViewModel;
            }
        }
    }
}
