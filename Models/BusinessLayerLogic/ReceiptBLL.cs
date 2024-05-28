using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Supermarket.Models.BusinessLayerLogic
{
    public class ReceiptsBLL
    {
        private static List<ProductReceipt> _currentReceiptItems = new List<ProductReceipt>();

        public List<ProductReceipt> GetCurrentReceiptItems()
        {
            return _currentReceiptItems;
        }

        public void AddProductToReceipt(ProductReceipt productReceipt)
        {
            _currentReceiptItems.Add(productReceipt);
        }

        public void ClearReceipt()
        {
            _currentReceiptItems.Clear();
        }

        public decimal CalculateTotal()
        {
            return _currentReceiptItems.Sum(item => item.Subtotal);
        }

        public void CompleteReceipt(Receipt receipt)
        {
            using (var context = new SupermarketDBContext())
            {
                context.Receipts.Add(receipt);
                context.SaveChanges();

                foreach (var item in _currentReceiptItems)
                {
                    item.ReceiptId = receipt.Id;
                    context.ProductReceipts.Add(item);

                    UpdateStockQuantities(context, item.ProductId, item.Quantity);
                }

                context.SaveChanges();
            }

            ClearReceipt();
        }

        private void UpdateStockQuantities(SupermarketDBContext context, int productId, int quantity)
        {
            var stocks = context.Inventory
                .Where(s => s.ProductId == productId && s.IsActive)
                .OrderBy(s => s.SupplyDate)
                .ToList();

            foreach (var stock in stocks)
            {
                if (stock.Quantity >= quantity)
                {
                    stock.Quantity -= quantity;
                    if (stock.Quantity == 0)
                    {
                        stock.IsActive = false;
                    }
                    break;
                }
                else
                {
                    quantity -= stock.Quantity;
                    stock.Quantity = 0;
                    stock.IsActive = false;
                }
            }

            var expiredStocks = context.Inventory
                .Where(s => s.ExpiryDate.HasValue && s.ExpiryDate.Value < DateTime.Now)
                .ToList();

            foreach (var stock in expiredStocks)
            {
                stock.IsActive = false;
            }
        }
    }
}
