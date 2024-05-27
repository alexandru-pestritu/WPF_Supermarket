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

namespace WPF_Supermarket.ViewModels
{
    public class StocksViewModel : ViewModelBase
    {
        private readonly StockBLL _stockBLL;
        private readonly ProductBLL _productBLL;
        private ObservableCollection<Stock> _stocks;
        private ObservableCollection<Product> _products;
        private Stock _selectedStock;
        private Stock _currentStock;
        private bool _isStockSelected;
        private decimal _profitPercentage;

        public RelayCommand AddStockCommand { get; private set; }
        public RelayCommand EditStockCommand { get; private set; }
        public RelayCommand DeleteStockCommand { get; private set; }

        public StocksViewModel()
        {
            _stockBLL = new StockBLL();
            _productBLL = new ProductBLL();
            _stocks = new ObservableCollection<Stock>();
            _products = new ObservableCollection<Product>();
            _currentStock = new Stock();

            _profitPercentage = _stockBLL.GetProfitPercentage();
            LoadStocks();
            LoadProducts();

            AddStockCommand = new RelayCommand(_ => AddStock(), _ => !IsStockSelected);
            EditStockCommand = new RelayCommand(_ => EditStock(), _ => IsStockSelected);
            DeleteStockCommand = new RelayCommand(_ => DeleteStock(), _ => IsStockSelected);
        }

        public ObservableCollection<Stock> Stocks
        {
            get => _stocks;
            set
            {
                _stocks = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public Stock SelectedStock
        {
            get => _selectedStock;
            set
            {
                _selectedStock = value;
                OnPropertyChanged();

                if (_selectedStock != null)
                {
                    CurrentStock = new Stock
                    {
                        Id = _selectedStock.Id,
                        ProductId = _selectedStock.ProductId,
                        Product = _selectedStock.Product,
                        Quantity = _selectedStock.Quantity,
                        MeasureUnit = _selectedStock.MeasureUnit,
                        SupplyDate = _selectedStock.SupplyDate,
                        ExpiryDate = _selectedStock.ExpiryDate,
                        PurchasePrice = _selectedStock.PurchasePrice,
                        SellingPrice = _selectedStock.SellingPrice
                    };
                    IsStockSelected = true;
                }
                else
                {
                    CurrentStock = new Stock();
                    IsStockSelected = false;
                }

                UpdateCommandStates();
            }
        }

        public Stock CurrentStock
        {
            get => _currentStock;
            set
            {
                _currentStock = value;
                OnPropertyChanged();
            }
        }

        public bool IsStockSelected
        {
            get => _isStockSelected;
            set
            {
                _isStockSelected = value;
                OnPropertyChanged();
                UpdateCommandStates();
            }
        }

        private void LoadStocks()
        {
            var stocks = _stockBLL.GetAllStocks();
            Stocks.Clear();
            foreach (var stock in stocks)
            {
                Stocks.Add(stock);
            }
        }

        private void LoadProducts()
        {
            var products = _productBLL.GetAllProducts();
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }

        private void AddStock()
        {
            if (ValidateStock(CurrentStock))
            {
                var stock = new Stock
                {
                    ProductId = CurrentStock.ProductId,
                    Quantity = CurrentStock.Quantity,
                    MeasureUnit = CurrentStock.MeasureUnit,
                    SupplyDate = DateTime.Now,
                    ExpiryDate = CurrentStock.ExpiryDate,
                    PurchasePrice = CurrentStock.PurchasePrice,
                    SellingPrice = CalculateSellingPrice(CurrentStock.PurchasePrice)
                };

                _stockBLL.AddStock(stock);
                LoadStocks();
                CurrentStock = new Stock();
            }
        }

        private void EditStock()
        {
            if (SelectedStock != null && ValidateStock(CurrentStock))
            {
                var stock = _stockBLL.GetStockById(CurrentStock.Id);
                if (stock != null)
                {
                    stock.Quantity = CurrentStock.Quantity;
                    stock.MeasureUnit = CurrentStock.MeasureUnit;
                    stock.ExpiryDate = CurrentStock.ExpiryDate;
                    stock.SellingPrice = CurrentStock.SellingPrice >= stock.PurchasePrice ? CurrentStock.SellingPrice : stock.SellingPrice;

                    _stockBLL.UpdateStock(stock);
                    LoadStocks();
                    CurrentStock = new Stock();
                    IsStockSelected = false;
                }
            }
        }

        private void DeleteStock()
        {
            if (SelectedStock != null)
            {
                _stockBLL.DeleteStock(SelectedStock.Id);
                LoadStocks();
                CurrentStock = new Stock();
                IsStockSelected = false;
            }
        }

        private bool ValidateStock(Stock stock)
        {
            if (stock == null)
            {
                MessageBox.Show("All fields must be filled out correctly.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (stock.ProductId <= 0)
            {
                MessageBox.Show("Product must be selected.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (stock.Quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than 0.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if(stock.MeasureUnit == null || stock.MeasureUnit.Length == 0)
            {
                MessageBox.Show("Measure unit must be filled out.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (stock.PurchasePrice <= 0)
            {
                MessageBox.Show("Purchase price must be greater than 0.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (stock.ExpiryDate.HasValue && stock.ExpiryDate.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Expiry date must be later than today.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private decimal CalculateSellingPrice(decimal purchasePrice)
        {
            return purchasePrice * (1 + _profitPercentage);
        }

        private void UpdateCommandStates()
        {
            AddStockCommand.RaiseCanExecuteChanged();
            EditStockCommand.RaiseCanExecuteChanged();
            DeleteStockCommand.RaiseCanExecuteChanged();
        }
    }
}
