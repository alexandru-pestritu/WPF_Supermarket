using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf_Supermarket.MVVM;
using WPF_Supermarket.Services;
using WPF_Supermarket.Views;

namespace WPF_Supermarket.ViewModels
{
    public class CashierDashboardViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;
        private readonly Frame _frame;

        public CashierDashboardViewModel(NavigationService navigationService, Frame frame)
        {
            _navigationService = navigationService;
            _frame = frame;

            NavigateToProductsCommand = new RelayCommand(_ => NavigateTo(new ProductsPage()));
            NavigateToReceiptsCommand = new RelayCommand(_ => NavigateTo(new ReceiptsPage()));
            LogoutCommand = new RelayCommand(_ => Logout());
        }

        public ICommand NavigateToProductsCommand { get; }
        public ICommand NavigateToReceiptsCommand { get; }
        public ICommand LogoutCommand { get; }

        private void NavigateTo(Page page)
        {
            _frame.Navigate(page);
        }

        private void Logout()
        {
            _navigationService.Navigate(new LoginPage(_navigationService));
        }
    }
}
