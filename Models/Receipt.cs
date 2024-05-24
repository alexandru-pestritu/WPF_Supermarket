using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Supermarket.Models
{
    public class Receipt
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CashierId { get; set; }
        public decimal Total { get; set; }
        public virtual User Cashier { get; set; }
        public virtual ICollection<ProductReceipt> ProductReceipts { get; set; }
    }
}
