using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_Supermarket.MVVM;
using WPF_Supermarket.Models.BusinessLayerLogic;
using WPF_Supermarket.Models;
using System.Windows.Input;
using System.Windows;

namespace WPF_Supermarket.ViewModels
{
    public class UsersViewModel : ViewModelBase
    {
        private readonly UserBLL _userBLL;
        private ObservableCollection<User> _users;
        private User _selectedUser;
        private User _currentUser;
        private bool _isUserSelected;

        public RelayCommand AddUserCommand { get; private set; }
        public RelayCommand EditUserCommand { get; private set; }
        public RelayCommand DeleteUserCommand { get; private set; }

        public UsersViewModel()
        {
            _userBLL = new UserBLL();
            _users = new ObservableCollection<User>();
            _currentUser = new User();
            LoadUsers();

            AddUserCommand = new RelayCommand(_ => AddUser(), _ => !IsUserSelected);
            EditUserCommand = new RelayCommand(_ => EditUser(), _ => IsUserSelected);
            DeleteUserCommand = new RelayCommand(_ => DeleteUser(), _ => IsUserSelected);
        }

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();

                if (_selectedUser != null)
                {
                    CurrentUser = new User
                    {
                        Id = _selectedUser.Id,
                        Name = _selectedUser.Name,
                        Password = _selectedUser.Password,
                        UserType = _selectedUser.UserType
                    };
                    IsUserSelected = true;
                }
                else
                {
                    CurrentUser = new User();
                    IsUserSelected = false;
                }
            }
        }

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public bool IsUserSelected
        {
            get => _isUserSelected;
            set
            {
                _isUserSelected = value;
                OnPropertyChanged();
                UpdateCommandStates();
            }
        }

        private void LoadUsers()
        {
            var users = _userBLL.GetAllUsers();
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        private void AddUser()
        {
            if(ValidateUser(CurrentUser))
            {
                _userBLL.AddUser(CurrentUser);
                LoadUsers();
                CurrentUser = new User();
            }
        }

        private void EditUser()
        {
            if (SelectedUser != null && ValidateUser(CurrentUser))
            {
                _userBLL.UpdateUser(CurrentUser);
                LoadUsers();
                CurrentUser = new User();
                IsUserSelected = false;
            }
        }

        private void DeleteUser()
        {
            if (SelectedUser != null)
            {
                _userBLL.DeleteUser(SelectedUser.Id);
                LoadUsers();
                CurrentUser = new User();
                IsUserSelected = false;
            }
        }

        private bool ValidateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.UserType))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void UpdateCommandStates()
        {
            AddUserCommand.RaiseCanExecuteChanged();
            EditUserCommand.RaiseCanExecuteChanged();
            DeleteUserCommand.RaiseCanExecuteChanged();
        }

    }
}
