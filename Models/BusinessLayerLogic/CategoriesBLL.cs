using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Supermarket.Models.BusinessLayerLogic
{
    public class CategoriesBLL
    {
        private readonly SupermarketDBContext _context;

        public CategoriesBLL()
        {
            _context = new SupermarketDBContext();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.Find(id);
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            var existingCategory = _context.Categories.Find(category.Id);
            if (existingCategory != null)
            {
                _context.Entry(existingCategory).CurrentValues.SetValues(category);
                _context.SaveChanges();
            }
        }

        public void DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }
    }
}
