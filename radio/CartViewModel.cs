using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace radio
{
    // Добавлен класс CartItem, который отсутствовал (была ошибка CS0246)
    public class CartItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
    }


    // Реализация RelayCommand для работы команд (была ошибка CS0305)
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object parameter) => _execute();
    }

    // Убрана дублирующая строка объявления класса (была ошибка CS0101)
    public class CartViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<CartItem> _cartItems;
        public ObservableCollection<CartItem> CartItems
        {
            get => _cartItems;
            set
            {
                _cartItems = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public DateTime? OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string DeliveryAddress { get; set; }
        public string Comments { get; set; }
        public ICommand PlaceOrderCommand { get; }
        public decimal TotalPrice => CartItems?.Sum(item => item.Price * item.Quantity) ?? 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public CartViewModel()
        {
            CartItems = new ObservableCollection<CartItem>();
            PlaceOrderCommand = new RelayCommand(PlaceOrder);
        }

        public void AddToCart(Product product)
        {
            Debug.WriteLine($"Попытка добавить товар: {product?.Name ?? "null"}");

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
                    Quantity = 1,
                    Description = product.Description,
                    Manufacturer = product.Manufacturer
                });
            }

            OnPropertyChanged(nameof(CartItems));
            OnPropertyChanged(nameof(TotalPrice));

            Debug.WriteLine($"Товар добавлен. Всего в корзине: {CartItems.Count}");
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PlaceOrder()
        {
            if (OrderDate == null || string.IsNullOrEmpty(CustomerName))
            {
                MessageBox.Show("Пожалуйста, укажите дату заказа и ваше имя");
                return;
            }
            MessageBox.Show($"Заказ оформлен на {OrderDate.Value.ToShortDateString()}");
        }
    }
}