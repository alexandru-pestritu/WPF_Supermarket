using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Supermarket.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public int CategoryId { get; set; }
        public int ManufacturerId { get; set; }
        public virtual Category Category { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual ICollection<Stock> Inventory { get; set; }
        public virtual ICollection<ProductReceipt> ProductReceipts { get; set; }
        public virtual ICollection<Offer> Offers { get; set; }
    }
}
