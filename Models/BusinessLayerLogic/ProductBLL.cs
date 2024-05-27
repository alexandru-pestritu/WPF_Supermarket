using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace WPF_Supermarket.Models.BusinessLayerLogic
{
    public class ProductBLL
    {
        private readonly SupermarketDBContext _context;

        public ProductBLL()
        {
            _context = new SupermarketDBContext();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.Include(p => p.Category).Include(p => p.Manufacturer).ToList();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.Include(p => p.Category).Include(p => p.Manufacturer).FirstOrDefault(p => p.Id == id);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            var existingProduct = _context.Products.Find(product.Id);
            if (existingProduct != null)
            {
                _context.Entry(existingProduct).CurrentValues.SetValues(product);
                _context.SaveChanges();
            }
        }

        public void DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Product> SearchProducts(string name, string barcode,  int? manufacturerId, int? categoryId)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            if (!string.IsNullOrEmpty(barcode))
                query = query.Where(p => p.Barcode.Contains(barcode));

            if (manufacturerId.HasValue)
                query = query.Where(p => p.ManufacturerId == manufacturerId.Value);

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            return query.Include(p => p.Category).Include(p => p.Manufacturer).Include(p => p.Inventory).ToList();
        }
    }
}
