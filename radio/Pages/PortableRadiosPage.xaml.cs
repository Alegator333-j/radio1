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

namespace radio.Pages
{
    /// <summary>
    /// Логика взаимодействия для PortableRadiosPage.xaml
    /// </summary>
    public partial class PortableRadiosPage : Page
    {
        private DataView _allPortableRadios;
        public DataView FilteredPortableRadios { get; private set; }

        public PortableRadiosPage()
        {
            InitializeComponent();
            try
            {
                LoadPortableRadios();
                DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке страницы: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadData(DataTable data)
        {
            if (data == null || data.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных о портативных радиостанциях", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (!data.Columns.Contains("ImagePath"))
            {
                data.Columns.Add("ImagePath", typeof(string));
            }

            foreach (DataRow row in data.Rows)
            {
                var productId = row["ID_Товара"].ToString();
                row["ImagePath"] = GetImagePathForProduct(productId);
            }

            _allPortableRadios = data.DefaultView;
            FilteredPortableRadios = _allPortableRadios;

            LoadManufacturersFilter(data);
            SetupPriceFilter(data);
        }

        private void LoadManufacturersFilter(DataTable data)
        {
            try
            {
                ManufacturerFilter.Items.Clear();
                ManufacturerFilter.Items.Add("Все производители");

                var manufacturers = data.AsEnumerable()
                    .Select(row => row.Field<string>("НазваниеПроизводителя"))
                    .Where(name => !string.IsNullOrEmpty(name))
                    .Distinct()
                    .OrderBy(name => name)
                    .ToList();

                foreach (var manufacturer in manufacturers)
                {
                    ManufacturerFilter.Items.Add(manufacturer);
                }

                ManufacturerFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки производителей: {ex.Message}");
            }
        }

        private void SetupPriceFilter(DataTable data)
        {
            try
            {
                if (data.Rows.Count > 0)
                {
                    var maxPrice = data.AsEnumerable()
                        .Max(row => row.Field<decimal>("Цена"));
                    PriceFilter.Maximum = (double)maxPrice + 1000;
                    PriceFilter.TickFrequency = PriceFilter.Maximum / 10;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при настройке фильтра цены: {ex.Message}");
            }
        }

        private void LoadPortableRadios()
        {
            var viewModel = new MainViewModel();
            var portableRadios = viewModel.GetProductsByCategory(2); // ID категории портативных радиостанций
            LoadData(portableRadios);
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_allPortableRadios == null) return;

            var filtered = _allPortableRadios.ToTable().DefaultView;
            var filters = new System.Text.StringBuilder();

            if (ManufacturerFilter.SelectedIndex > 0)
            {
                var selectedManufacturer = ManufacturerFilter.SelectedItem.ToString();
                filters.Append($"[НазваниеПроизводителя] = '{selectedManufacturer.Replace("'", "''")}'");
            }

            if (PriceFilter.Value > 0)
            {
                if (filters.Length > 0) filters.Append(" AND ");
                filters.Append($"[Цена] >= {PriceFilter.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
            }

            if (QuantityFilter.SelectedIndex == 1)
            {
                if (filters.Length > 0) filters.Append(" AND ");
                filters.Append("[Количество] > 0");
            }
            else if (QuantityFilter.SelectedIndex == 2)
            {
                if (filters.Length > 0) filters.Append(" AND ");
                filters.Append("[Количество] = 0");
            }

            try
            {
                filtered.RowFilter = filters.ToString();
                FilteredPortableRadios = filtered;
                PortableRadiosList.ItemsSource = FilteredPortableRadios;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}\nФильтр: {filters.ToString()}");
            }
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            ManufacturerFilter.SelectedIndex = 0;
            PriceFilter.Value = PriceFilter.Minimum;
            QuantityFilter.SelectedIndex = 0;
            ApplyFilters();
        }

        private string GetImagePathForProduct(string productId)
        {
            return System.IO.File.Exists($"Images/PortableRadios/{productId}.jpg")
                ? $"/Images/PortableRadios/{productId}.jpg"
                : "/Images/placeholder.jpg";
        }
    }
}
