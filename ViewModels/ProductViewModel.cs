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
using System.Windows.Media.Imaging;
using System.IO;

namespace WPF_Supermarket.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        private readonly ProductBLL _productBLL;
        private readonly CategoriesBLL _categoryBLL;
        private readonly ManufacturerBLL _manufacturerBLL;
        private readonly BarcodeService _barcodeService;
        private ObservableCollection<ProductViewModel> _products;
        private ObservableCollection<Category> _categories;
        private ObservableCollection<Manufacturer> _manufacturers;
        private ProductViewModel _selectedProduct;
        private ProductViewModel _currentProduct;
        private bool _isProductSelected;

        public RelayCommand AddProductCommand { get; private set; }
        public RelayCommand EditProductCommand { get; private set; }
        public RelayCommand DeleteProductCommand { get; private set; }

        public ProductsViewModel()
        {
            _productBLL = new ProductBLL();
            _categoryBLL = new CategoriesBLL();
            _manufacturerBLL = new ManufacturerBLL();
            _barcodeService = new BarcodeService();
            _products = new ObservableCollection<ProductViewModel>();
            _categories = new ObservableCollection<Category>();
            _manufacturers = new ObservableCollection<Manufacturer>();
            _currentProduct = new ProductViewModel();
            LoadCategories();
            LoadManufacturers();
            LoadProducts();

            AddProductCommand = new RelayCommand(_ => AddProduct(), _ => !IsProductSelected);
            EditProductCommand = new RelayCommand(_ => EditProduct(), _ => IsProductSelected);
            DeleteProductCommand = new RelayCommand(_ => DeleteProduct(), _ => IsProductSelected);
        }

        public ObservableCollection<ProductViewModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
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

        public ProductViewModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();

                if (_selectedProduct != null)
                {
                    CurrentProduct = new ProductViewModel
                    {
                        Id = _selectedProduct.Id,
                        Name = _selectedProduct.Name,
                        Barcode = _selectedProduct.Barcode,
                        CategoryId = _selectedProduct.CategoryId,
                        ManufacturerId = _selectedProduct.ManufacturerId,
                        BarcodeImage = _selectedProduct.BarcodeImage
                    };
                    IsProductSelected = true;
                }
                else
                {
                    CurrentProduct = new ProductViewModel();
                    IsProductSelected = false;
                }

                UpdateCommandStates();
            }
        }

        public ProductViewModel CurrentProduct
        {
            get => _currentProduct;
            set
            {
                _currentProduct = value;
                OnPropertyChanged();
            }
        }

        public bool IsProductSelected
        {
            get => _isProductSelected;
            set
            {
                _isProductSelected = value;
                OnPropertyChanged();
                UpdateCommandStates();
            }
        }

        private void LoadProducts()
        {
            var products = _productBLL.GetAllProducts();
            Products.Clear();
            foreach (var product in products)
            {
                var productViewModel = new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Barcode = product.Barcode,
                    CategoryId = product.CategoryId,
                    ManufacturerId = product.ManufacturerId,
                    Category = Categories.FirstOrDefault(c => c.Id == product.CategoryId),
                    Manufacturer = Manufacturers.FirstOrDefault(m => m.Id == product.ManufacturerId),
                    BarcodeImage = GenerateBarcodeImage(product.Barcode)
                };
                Products.Add(productViewModel);
            }
        }

        private void LoadCategories()
        {
            var categories = _categoryBLL.GetAllCategories();
            Categories.Clear();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
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

        private void AddProduct()
        {
            if (ValidateProduct(CurrentProduct))
            {
                var product = new Product
                {
                    Name = CurrentProduct.Name,
                    Barcode = CurrentProduct.Barcode,
                    CategoryId = CurrentProduct.CategoryId,
                    ManufacturerId = CurrentProduct.ManufacturerId
                };

                _productBLL.AddProduct(product);
                LoadProducts();
                CurrentProduct = new ProductViewModel();
            }
        }

        private void EditProduct()
        {
            if (SelectedProduct != null && ValidateProduct(CurrentProduct))
            {
                var product = new Product
                {
                    Id = CurrentProduct.Id,
                    Name = CurrentProduct.Name,
                    Barcode = CurrentProduct.Barcode,
                    CategoryId = CurrentProduct.CategoryId,
                    ManufacturerId = CurrentProduct.ManufacturerId
                };

                _productBLL.UpdateProduct(product);
                LoadProducts();
                CurrentProduct = new ProductViewModel();
                IsProductSelected = false;
            }
        }

        private void DeleteProduct()
        {
            if (SelectedProduct != null)
            {
                _productBLL.DeleteProduct(SelectedProduct.Id);
                LoadProducts();
                CurrentProduct = new ProductViewModel();
                IsProductSelected = false;
            }
        }

        private bool ValidateProduct(ProductViewModel product)
        {
            if (string.IsNullOrWhiteSpace(product.Name) || string.IsNullOrWhiteSpace(product.Barcode) || product.CategoryId == 0 || product.ManufacturerId == 0)
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private BitmapImage GenerateBarcodeImage(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode)) return null;

            var barcodeBytes = _barcodeService.GenerateBarcode(barcode);
            using (var stream = new MemoryStream(barcodeBytes))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                return image;
            }
        }

        private void UpdateCommandStates()
        {
            AddProductCommand.RaiseCanExecuteChanged();
            EditProductCommand.RaiseCanExecuteChanged();
            DeleteProductCommand.RaiseCanExecuteChanged();
        }
    }

    public class ProductViewModel : ViewModelBase
    {
        private BitmapImage _barcodeImage;
        private Category _category;
        private Manufacturer _manufacturer;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public int CategoryId { get; set; }
        public int ManufacturerId { get; set; }
        public Category Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }
        public Manufacturer Manufacturer
        {
            get => _manufacturer;
            set
            {
                _manufacturer = value;
                OnPropertyChanged();
            }
        }
        public BitmapImage BarcodeImage
        {
            get => _barcodeImage;
            set
            {
                _barcodeImage = value;
                OnPropertyChanged();
            }
        }
    }
}
