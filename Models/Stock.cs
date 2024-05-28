using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Supermarket.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string MeasureUnit { get; set; }
        public DateTime SupplyDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual Product Product { get; set; }
    }
}
