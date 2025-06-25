using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;

namespace radio
{
    public class CartViewModel
    {

        // Метод для добавления товара в корзину
        public void AddToCart(Product product)
        {
            var existingItem = CartItems.FirstOrDefault(item => item.ProductId == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                CartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1
                });
            }

            MessageBox.Show($"{product.Name} добавлен в корзину!");
        }

        public ObservableCollection<CartItem> CartItems { get; set; }
        public DateTime? OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string DeliveryAddress { get; set; }
        public string Comments { get; set; }

        public ICommand PlaceOrderCommand { get; }

        public CartViewModel()
        {

            // Инициализация свойств
            CartItems = new ObservableCollection<CartItem>();
            OrderDate = DateTime.Today;

            // Команда для оформления заказа
            PlaceOrderCommand = new RelayCommand(PlaceOrder);

            // Загрузка тестовых данных (в реальном приложении загружайте из корзины)
            LoadTestData();
        }

        private void LoadTestData()
        {
            CartItems.Add(new CartItem { Name = "Радиостанция", Quantity = 1, Price = 12000 });
            CartItems.Add(new CartItem { Name = "Антенна", Quantity = 2, Price = 3500 });
        }

        private void PlaceOrder()
        {
            // Логика оформления заказа
            if (OrderDate == null || string.IsNullOrEmpty(CustomerName))
            {
                MessageBox.Show("Пожалуйста, укажите дату заказа и ваше имя");
                return;
            }

            // Здесь должна быть логика сохранения заказа
            MessageBox.Show($"Заказ оформлен на {OrderDate.Value.ToShortDateString()}");
        }
    }

    public class CartItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}