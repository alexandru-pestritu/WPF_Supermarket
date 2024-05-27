using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_Supermarket.Models.BusinessLayerLogic;
using WPF_Supermarket.Models;
using Wpf_Supermarket.MVVM;
using WPF_Supermarket.Services;

namespace WPF_Supermarket.ViewModels
{
    public class ManufacturersViewModel : ViewModelBase
    {
        private readonly ManufacturerBLL _manufacturerBLL;
        private readonly CountryService _countryService;
        private ObservableCollection<Manufacturer> _manufacturers;
        private Manufacturer _selectedManufacturer;
        private Manufacturer _currentManufacturer;
        private bool _isManufacturerSelected;
        private ObservableCollection<string> _countries;

        public RelayCommand AddManufacturerCommand { get; private set; }
        public RelayCommand EditManufacturerCommand { get; private set; }
        public RelayCommand DeleteManufacturerCommand { get; private set; }

        public ManufacturersViewModel()
        {
            _manufacturerBLL = new ManufacturerBLL();
            _countryService = new CountryService();
            _manufacturers = new ObservableCollection<Manufacturer>();
            _currentManufacturer = new Manufacturer();
            LoadManufacturers();

            AddManufacturerCommand = new RelayCommand(_ => AddManufacturer(), _ => !IsManufacturerSelected);
            EditManufacturerCommand = new RelayCommand(_ => EditManufacturer(), _ => IsManufacturerSelected);
            DeleteManufacturerCommand = new RelayCommand(_ => DeleteManufacturer(), _ => IsManufacturerSelected);
            LoadCountriesAsync();
        }

        public ObservableCollection<Manufacturer> Manufacturers
        {
            get => _manufacturers;
            set
            {
                _manufacturers = value;
                OnPropertyChanged();
            }
        }

        public Manufacturer SelectedManufacturer
        {
            get => _selectedManufacturer;
            set
            {
                _selectedManufacturer = value;
                OnPropertyChanged();

                if (_selectedManufacturer != null)
                {
                    CurrentManufacturer = new Manufacturer
                    {
                        Id = _selectedManufacturer.Id,
                        Name = _selectedManufacturer.Name,
                        OriginCountry = _selectedManufacturer.OriginCountry,
                        Products = _selectedManufacturer.Products
                    };
                    IsManufacturerSelected = true;
                }
                else
                {
                    CurrentManufacturer = new Manufacturer();
                    IsManufacturerSelected = false;
                }

                UpdateCommandStates();
            }
        }

        public Manufacturer CurrentManufacturer
        {
            get => _currentManufacturer;
            set
            {
                _currentManufacturer = value;
                OnPropertyChanged();
            }
        }

        public bool IsManufacturerSelected
        {
            get => _isManufacturerSelected;
            set
            {
                _isManufacturerSelected = value;
                OnPropertyChanged();
                UpdateCommandStates();
            }
        }

        public ObservableCollection<string> Countries
        {
            get => _countries;
            set
            {
                _countries = value;
                OnPropertyChanged();
            }
        }

        private async Task LoadCountriesAsync()
        {
            var countries = await _countryService.GetAllCountriesAsync();
            Countries = new ObservableCollection<string>(countries);
        }

        private void LoadManufacturers()
        {
            var manufacturers = _manufacturerBLL.GetAllManufacturers();
            Manufacturers.Clear();
            foreach (var manufacturer in manufacturers)
            {
                Manufacturers.Add(manufacturer);
            }
        }

        private void AddManufacturer()
        {
            if (ValidateManufacturer(CurrentManufacturer))
            {
                _manufacturerBLL.AddManufacturer(CurrentManufacturer);
                LoadManufacturers();
                CurrentManufacturer = new Manufacturer();
            }
        }

        private void EditManufacturer()
        {
            if (SelectedManufacturer != null && ValidateManufacturer(CurrentManufacturer))
            {
                _manufacturerBLL.UpdateManufacturer(CurrentManufacturer);
                LoadManufacturers();
                CurrentManufacturer = new Manufacturer();
                IsManufacturerSelected = false;
            }
        }

        private void DeleteManufacturer()
        {
            if (SelectedManufacturer != null)
            {
                _manufacturerBLL.DeleteManufacturer(SelectedManufacturer.Id);
                LoadManufacturers();
                CurrentManufacturer = new Manufacturer();
                IsManufacturerSelected = false;
            }
        }

        private bool ValidateManufacturer(Manufacturer manufacturer)
        {
            if (string.IsNullOrWhiteSpace(manufacturer.Name) || string.IsNullOrWhiteSpace(manufacturer.OriginCountry))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void UpdateCommandStates()
        {
            AddManufacturerCommand.RaiseCanExecuteChanged();
            EditManufacturerCommand.RaiseCanExecuteChanged();
            DeleteManufacturerCommand.RaiseCanExecuteChanged();
        }
    }
}
