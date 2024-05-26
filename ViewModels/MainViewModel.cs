using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf_Supermarket.MVVM;
using WPF_Supermarket.Services;

namespace WPF_Supermarket.ViewModels
{
    public class MainViewModel
    {
        private readonly NavigationService _navigationService;
        public MainViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
