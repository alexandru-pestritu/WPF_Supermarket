using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Supermarket.Services;

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
                // Ensure Cashier is not null
                var cashier = context.Users.Find(UserSession.Instance.UserId);
                if (cashier == null)
                {
                    throw new InvalidOperationException("Cashier not found in the database.");
                }

                // Create new Receipt instance
                var newReceipt = new Receipt
                {
                    Date = receipt.Date,
                    CashierId = cashier.Id,
                    Total = receipt.Total,
                    Cashier = cashier,
                    ProductReceipts = new List<ProductReceipt>()
                };

                // Add Receipt to context
                context.Receipts.Add(newReceipt);
                context.SaveChanges();

                // Add ProductReceipts to context
                foreach (var item in _currentReceiptItems)
                {
                    // Ensure Product is not null
                    var product = context.Products.Find(item.ProductId);
                    if (product == null)
                    {
                        throw new InvalidOperationException($"Product with ID {item.ProductId} not found in the database.");
                    }

                    // Create new ProductReceipt instance
                    var newProductReceipt = new ProductReceipt
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        Subtotal = item.Subtotal,
                        ReceiptId = newReceipt.Id,
                        Product = product
                    };

                    context.ProductReceipts.Add(newProductReceipt);

                    UpdateStockQuantities(context, item.ProductId, item.Quantity);
                }

                context.SaveChanges();
            }

            // Clear current receipt
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
