using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Supermarket.Models;
using Wpf_Supermarket.MVVM;
using WPF_Supermarket.Models.BusinessLayerLogic;
using WPF_Supermarket.Services;

namespace WPF_Supermarket.ViewModels
    {
        public class ProductsSearchViewModel : ViewModelBase
        {
            private readonly ProductBLL _productBLL;
            private readonly CategoriesBLL _categoryBLL;
            private readonly ManufacturerBLL _manufacturerBLL;
            private readonly ReceiptsBLL _receiptsBLL;
            private string _searchName;
            private string _searchBarcode;
            private Manufacturer _selectedManufacturer;
            private Category _selectedCategory;
            private Product _selectedProduct;
            private int _productQuantity;
            private ObservableCollection<Product> _searchResults;
            private ObservableCollection<Category> _categories;
            private ObservableCollection<Manufacturer> _manufacturers;

            public ProductsSearchViewModel()
            {
                _productBLL = new ProductBLL();
                _categoryBLL = new CategoriesBLL();
                _manufacturerBLL = new ManufacturerBLL();
                _receiptsBLL = new ReceiptsBLL();
                _categories = new ObservableCollection<Category>(_categoryBLL.GetAllCategories());
                _manufacturers = new ObservableCollection<Manufacturer>(_manufacturerBLL.GetAllManufacturers());
                _searchResults = new ObservableCollection<Product>();
                SearchCommand = new RelayCommand(_ => SearchProducts());
                AddToReceiptCommand = new RelayCommand(_ => AddToReceipt(), _ => IsProductSelected);
            }

            public string SearchName
            {
                get => _searchName;
                set { _searchName = value; OnPropertyChanged(); }
            }

            public string SearchBarcode
            {
                get => _searchBarcode;
                set { _searchBarcode = value; OnPropertyChanged(); }
            }

            public Manufacturer SelectedManufacturer
            {
                get => _selectedManufacturer;
                set { _selectedManufacturer = value; OnPropertyChanged(); }
            }

            public Category SelectedCategory
            {
                get => _selectedCategory;
                set { _selectedCategory = value; OnPropertyChanged(); }
            }

            public Product SelectedProduct
            {
                get => _selectedProduct;
                set { _selectedProduct = value; OnPropertyChanged(); UpdateAddToReceiptCommandState(); }
            }

            public int ProductQuantity
            {
                get => _productQuantity;
                set { _productQuantity = value; OnPropertyChanged(); }
            }

            public ObservableCollection<Product> SearchResults
            {
                get => _searchResults;
                set { _searchResults = value; OnPropertyChanged(); }
            }

            public ObservableCollection<Category> Categories
            {
                get => _categories;
                set { _categories = value; OnPropertyChanged(); }
            }

            public ObservableCollection<Manufacturer> Manufacturers
            {
                get => _manufacturers;
                set { _manufacturers = value; OnPropertyChanged(); }
            }

            public bool IsProductSelected => SelectedProduct != null && ProductQuantity>0;

            public ICommand SearchCommand { get; }
            public ICommand AddToReceiptCommand { get; }

            private void SearchProducts()
            {
                var results = _productBLL.SearchProducts(SearchName, SearchBarcode, SelectedManufacturer?.Id, SelectedCategory?.Id);
                SearchResults = new ObservableCollection<Product>(results);
            }

            private void AddToReceipt()
            {
                if (SelectedProduct != null && ProductQuantity > 0)
                {
                    var productReceipt = new ProductReceipt
                    {
                        ProductId = SelectedProduct.Id,
                        Product = SelectedProduct,
                        Quantity = ProductQuantity,
                        Subtotal = SelectedProduct.Inventory.FirstOrDefault()?.SellingPrice * ProductQuantity ?? 0
                    };

                    _receiptsBLL.AddProductToReceipt(productReceipt);
                    ProductQuantity = 0;
                }
            }

            private void UpdateAddToReceiptCommandState()
            {
                (AddToReceiptCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
    }
