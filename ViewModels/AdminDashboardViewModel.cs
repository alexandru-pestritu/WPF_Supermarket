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
    public class AdminDashboardViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;
        private readonly Frame _frame;

        public AdminDashboardViewModel(NavigationService navigationService, Frame frame)
        {
            _navigationService = navigationService;
            _frame = frame;

            NavigateToUsersCommand = new RelayCommand(_ => NavigateTo(new UsersPage()));
            NavigateToCategoriesCommand = new RelayCommand(_ => NavigateTo(new CategoriesPage()));
            NavigateToManufacturersCommand = new RelayCommand(_ => NavigateTo(new ManufacturersPage()));
            NavigateToProductsCommand = new RelayCommand(_ => NavigateTo(new ProductsPage()));
            NavigateToInventoryCommand = new RelayCommand(_ => NavigateTo(new InventoryPage()));
            LogoutCommand = new RelayCommand(_ => Logout());
        }

        public ICommand NavigateToUsersCommand { get; }
        public ICommand NavigateToCategoriesCommand { get; }
        public ICommand NavigateToManufacturersCommand { get; }
        public ICommand NavigateToProductsCommand { get; }
        public ICommand NavigateToInventoryCommand { get; }
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
