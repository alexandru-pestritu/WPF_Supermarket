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
        private decimal _receiptTotal;

        public ReceiptViewModel()
        {
            _receiptsBLL = new ReceiptsBLL();
            _receiptItems = new ObservableCollection<ProductReceipt>(_receiptsBLL.GetCurrentReceiptItems());
            CompleteReceiptCommand = new RelayCommand(_ => CompleteReceipt());
            UpdateReceiptTotal();
        }

        public ObservableCollection<ProductReceipt> ReceiptItems
        {
            get => _receiptItems;
            set { _receiptItems = value; OnPropertyChanged(); }
        }

        public decimal ReceiptTotal
        {
            get => _receiptTotal;
            set { _receiptTotal = value; OnPropertyChanged(); }
        }

        public ICommand CompleteReceiptCommand { get; }

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
        }

        private void UpdateReceiptTotal()
        {
            ReceiptTotal = _receiptsBLL.CalculateTotal();
        }
    }
}
