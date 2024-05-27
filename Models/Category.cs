using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace WPF_Supermarket.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        [NotMapped]
        public int ProductCount => Products?.Count ?? 0;

        [NotMapped]
        public decimal TotalValue
        {
            get
            {
                return Products?.Sum(p => p.Inventory.Sum(i => i.Quantity * i.SellingPrice)) ?? 0;
            }
        }
    }
}
