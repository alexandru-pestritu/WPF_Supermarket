using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf_Supermarket.MVVM;
using WPF_Supermarket.Models;
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
            using (var context = new SupermarketDBContext())
            {
                var user = context.Users
                    .FirstOrDefault(u => u.Name == Username && u.Password == Password);

                if (user != null)
                {
                    MessageBox.Show($"Successful login for user {user.Name} with role {user.UserType}.");

                        if (user.UserType == "Admin")
                        {
                            _navigationService.Navigate(new AdminDashboardPage(_navigationService));
                        }
                        else if (user.UserType == "Cashier")
                        {
                            _navigationService.Navigate(new CashierDashboardPage(_navigationService));
                        }
                }
                else
                {
                    MessageBox.Show("Incorrect username and/or password.");
                }
            }
        }
    }
}
