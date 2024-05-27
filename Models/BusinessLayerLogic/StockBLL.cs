using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.IO;

namespace WPF_Supermarket.Models.BusinessLayerLogic
{
    public class StockBLL
    {
        private readonly SupermarketDBContext _context;

        public StockBLL()
        {
            _context = new SupermarketDBContext();
        }

        public IEnumerable<Stock> GetAllStocks()
        {
            return _context.Inventory.Include(s => s.Product).ToList();
        }

        public Stock GetStockById(int id)
        {
            return _context.Inventory.Include(s => s.Product).FirstOrDefault(s => s.Id == id);
        }

        public void AddStock(Stock stock)
        {
            _context.Inventory.Add(stock);
            _context.SaveChanges();
        }

        public void UpdateStock(Stock stock)
        {
            var existingStock = _context.Inventory.Find(stock.Id);
            if (existingStock != null)
            {
                _context.Entry(existingStock).CurrentValues.SetValues(stock);
                _context.SaveChanges();
            }
        }

        public void DeleteStock(int id)
        {
            var stock = _context.Inventory.Find(id);
            if (stock != null)
            {
                _context.Inventory.Remove(stock);
                _context.SaveChanges();
            }
        }

        public decimal GetProfitPercentage()
        {
            const string configFilePath = "profit.config";
            if (File.Exists(configFilePath))
            {
                var markupText = File.ReadAllText(configFilePath);
                if (decimal.TryParse(markupText, out decimal markup))
                {
                    return markup;
                }
            }
            return 0.20m; 
        }
    }
}
