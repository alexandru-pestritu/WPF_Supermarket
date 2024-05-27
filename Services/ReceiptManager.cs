using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Supermarket.Models;

namespace WPF_Supermarket.Services
{
    public class ReceiptManager
    {
        private static ReceiptManager _instance;
        private static readonly object _lock = new object();

        public ObservableCollection<ProductReceipt> CurrentReceiptItems { get; private set; }
        public decimal ReceiptTotal => CalculateTotal();

        private ReceiptManager()
        {
            CurrentReceiptItems = new ObservableCollection<ProductReceipt>();
        }

        public static ReceiptManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ReceiptManager();
                    }
                    return _instance;
                }
            }
        }

        public void AddProductToReceipt(ProductReceipt productReceipt)
        {
            CurrentReceiptItems.Add(productReceipt);
        }

        public void ClearReceipt()
        {
            CurrentReceiptItems.Clear();
        }

        private decimal CalculateTotal()
        {
            decimal total = 0;
            foreach (var item in CurrentReceiptItems)
            {
                total += item.Subtotal;
            }
            return total;
        }

        public void CompleteReceipt(Receipt receipt)
        {
            // Save the receipt and update stock quantities here
            // This is just a placeholder for the actual implementation
            // Save receipt to the database and update stock quantities
        }
    }
}

