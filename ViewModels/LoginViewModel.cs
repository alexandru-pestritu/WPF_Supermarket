using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf_Supermarket.MVVM;
using WPF_Supermarket.Services;
using WPF_Supermarket.Views;

namespace WPF_Supermarket.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        private string _password;
        private readonly NavigationService _navigationService;

        public LoginViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
            LoginCommand = new RelayCommand(OnLogin);
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        private void OnLogin(object parameter)
        {
            // Implementare logică autentificare
            if (Username == "admin")
            {
                _navigationService.Navigate(new AdminDashboardPage(_navigationService));
            }
            else if (Username == "cashier")
            {
                _navigationService.Navigate(new CashierDashboardPage(_navigationService));
            }
            else
            {
                // Mesaj de eroare autentificare
            }
        }
    }
}
