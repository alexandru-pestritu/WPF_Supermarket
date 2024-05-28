using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Supermarket.Models.BusinessLayerLogic;
using WPF_Supermarket.Models;
using Wpf_Supermarket.MVVM;
using WPF_Supermarket.Services;
using System.Windows;

namespace WPF_Supermarket.ViewModels
{
    public class ReceiptViewModel : ViewModelBase
    {
        private readonly ReceiptsBLL _receiptsBLL;
        private ObservableCollection<ProductReceipt> _receiptItems;
        private ObservableCollection<Receipt> _allReceipts;
        private ObservableCollection<Receipt> _filteredReceipts;
        private ProductReceipt _selectedReceiptItem;
        private DateTime _filterDate;
        private decimal _receiptTotal;

        public ReceiptViewModel()
        {
            _receiptsBLL = new ReceiptsBLL();
            _receiptItems = new ObservableCollection<ProductReceipt>(_receiptsBLL.GetCurrentReceiptItems());
            _allReceipts = new ObservableCollection<Receipt>(_receiptsBLL.GetReceiptsByCashier(UserSession.Instance.UserId));
            _filteredReceipts = new ObservableCollection<Receipt>(_allReceipts);
            _filterDate = DateTime.Now;
            CompleteReceiptCommand = new RelayCommand(_ => CompleteReceipt());
            DeleteProductCommand = new RelayCommand(_ => DeleteProduct(), _ => CanDeleteProduct());
            ClearReceiptCommand = new RelayCommand(_ => ClearReceipt());
            UpdateReceiptTotal();
        }

        public ObservableCollection<ProductReceipt> ReceiptItems
        {
            get => _receiptItems;
            set { _receiptItems = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Receipt> FilteredReceipts
        {
            get => _filteredReceipts;
            set { _filteredReceipts = value; OnPropertyChanged(); }
        }

        public ProductReceipt SelectedReceiptItem
        {
            get => _selectedReceiptItem;
            set { _selectedReceiptItem = value; OnPropertyChanged(); }
        }

        public DateTime FilterDate
        {
            get => _filterDate;
            set { _filterDate = value; OnPropertyChanged(); FilterReceipts(); }
        }

        public decimal ReceiptTotal
        {
            get => _receiptTotal;
            set { _receiptTotal = value; OnPropertyChanged(); }
        }

        public ICommand CompleteReceiptCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand ClearReceiptCommand { get; }

        private void CompleteReceipt()
        {
            if (ReceiptItems.Count == 0)
            {
                MessageBox.Show("Receipt is empty. Please add products to the receipt.");
                return;
            }

            var receipt = new Receipt
            {
                Date = DateTime.Now,
                CashierId = UserSession.Instance.UserId,
                Total = ReceiptTotal,
                ProductReceipts = ReceiptItems.ToList()
            };

            _receiptsBLL.CompleteReceipt(receipt);
            ReceiptItems.Clear();
            UpdateReceiptTotal();

            _allReceipts.Add(receipt);
            FilterReceipts();
        }

        private void DeleteProduct()
        {
            if (SelectedReceiptItem != null)
            {
                _receiptsBLL.RemoveProductFromReceipt(SelectedReceiptItem);
                ReceiptItems.Remove(SelectedReceiptItem);
                SelectedReceiptItem = null;
                UpdateReceiptTotal();
            }
        }

        private bool CanDeleteProduct()
        {
            return SelectedReceiptItem != null;
        }

        private void ClearReceipt()
        {
            _receiptsBLL.ClearReceipt();
            ReceiptItems.Clear();
            UpdateReceiptTotal();
        }

        private void UpdateReceiptTotal()
        {
            ReceiptTotal = _receiptsBLL.CalculateTotal();
        }

        private void FilterReceipts()
        {
            if (FilterDate == default(DateTime))
            {
                FilteredReceipts = new ObservableCollection<Receipt>(_allReceipts);
            }
            else
            {
                FilteredReceipts = new ObservableCollection<Receipt>(_allReceipts.Where(r => r.Date.Date == FilterDate.Date));
            }
        }
    }
}
