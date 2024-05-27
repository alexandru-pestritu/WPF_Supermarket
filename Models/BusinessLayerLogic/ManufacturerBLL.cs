using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Supermarket.Models.BusinessLayerLogic
{
    public class ManufacturerBLL
    {
        private readonly SupermarketDBContext _context;

        public ManufacturerBLL()
        {
            _context = new SupermarketDBContext();
        }

        public IEnumerable<Manufacturer> GetAllManufacturers()
        {
            return _context.Manufacturers.Include(m => m.Products).ToList();
        }

        public Manufacturer GetManufacturerById(int id)
        {
            return _context.Manufacturers.Include(m => m.Products).FirstOrDefault(m => m.Id == id);
        }

        public void AddManufacturer(Manufacturer manufacturer)
        {
            _context.Manufacturers.Add(manufacturer);
            _context.SaveChanges();
        }

        public void UpdateManufacturer(Manufacturer manufacturer)
        {
            var existingManufacturer = _context.Manufacturers.Find(manufacturer.Id);
            if (existingManufacturer != null)
            {
                _context.Entry(existingManufacturer).CurrentValues.SetValues(manufacturer);
                _context.SaveChanges();
            }
        }

        public void DeleteManufacturer(int id)
        {
            var manufacturer = _context.Manufacturers.Find(id);
            if (manufacturer != null)
            {
                _context.Manufacturers.Remove(manufacturer);
                _context.SaveChanges();
            }
        }
    }
}
