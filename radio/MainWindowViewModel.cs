using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radio
{
    public class MainWindowViewModel
    {
        // Реализация синглтона через Lazy для потокобезопасности
        private static readonly Lazy<MainWindowViewModel> _instance =
            new Lazy<MainWindowViewModel>(() => new MainWindowViewModel());

        public static MainWindowViewModel Instance => _instance.Value;

        public CartViewModel CartViewModel { get; }

        private MainWindowViewModel()
        {
            CartViewModel = new CartViewModel();
        }
    }
}
