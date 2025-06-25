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
    /// Логика взаимодействия для AntennasPage.xaml
    /// </summary>
    public partial class AntennasPage : Page
    {
        private DataView _allAntennas;
        public DataView FilteredAntennas { get; private set; }

        public AntennasPage()
        {
            InitializeComponent();
            try
            {
                LoadAntennas();
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
                MessageBox.Show("Нет данных для отображения", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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

            _allAntennas = data.DefaultView;
            FilteredAntennas = _allAntennas;

            LoadManufacturersFilter(data);
            SetupPriceFilter(data);
        }

        private void LoadManufacturersFilter(DataTable data)
        {
            try
            {
                // Очищаем ComboBox
                ManufacturerFilter.Items.Clear();

                // Добавляем элемент "Все производители"
                ManufacturerFilter.Items.Add("Все производители");

                // Получаем уникальные названия производителей
                var manufacturers = data.AsEnumerable()
                    .Select(row => row.Field<string>("НазваниеПроизводителя"))
                    .Where(name => !string.IsNullOrEmpty(name))
                    .Distinct()
                    .OrderBy(name => name)
                    .ToList();

                // Добавляем производителей в ComboBox
                foreach (var manufacturer in manufacturers)
                {
                    ManufacturerFilter.Items.Add(manufacturer);
                }

                // Устанавливаем первый элемент по умолчанию
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

        private void LoadAntennas()
        {
            var viewModel = new MainViewModel();
            var antennas = viewModel.GetProductsByCategory(3);
            LoadData(antennas);
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_allAntennas == null) return;

            var filtered = _allAntennas.ToTable().DefaultView;
            var filters = new System.Text.StringBuilder();

            // Фильтр по производителю
            if (ManufacturerFilter.SelectedIndex > 0)
            {
                var selectedManufacturer = ManufacturerFilter.SelectedItem.ToString();
                filters.Append($"[НазваниеПроизводителя] = '{selectedManufacturer.Replace("'", "''")}'");
            }

            // Фильтр по цене
            if (PriceFilter.Value > 0)
            {
                if (filters.Length > 0) filters.Append(" AND ");
                // Для числовых значений не нужно добавлять кавычки
                filters.Append($"[Цена] >= {PriceFilter.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
            }

            // Фильтр по количеству
            if (QuantityFilter.SelectedIndex == 1) // Только в наличии
            {
                if (filters.Length > 0) filters.Append(" AND ");
                filters.Append("[Количество] > 0");
            }
            else if (QuantityFilter.SelectedIndex == 2) // Нет в наличии
            {
                if (filters.Length > 0) filters.Append(" AND ");
                filters.Append("[Количество] = 0");
            }

            try
            {
                filtered.RowFilter = filters.ToString();
                FilteredAntennas = filtered;
                AntennasList.ItemsSource = FilteredAntennas;
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
            return System.IO.File.Exists($"Images/Antennas/{productId}.jpg")
                ? $"/Images/Antennas/{productId}.jpg"
                : "/Images/placeholder.jpg";
        }
    }
}
