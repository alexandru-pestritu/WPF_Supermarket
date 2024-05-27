using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.ComponentModel.DataAnnotations.Schema;
using WPF_Supermarket.Services;

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

        [NotMapped]
        public BitmapImage BarcodeImage
        {
            get
            {
                if (_barcodeImage == null && !string.IsNullOrEmpty(Barcode))
                {
                    _barcodeImage = BarcodeService.GenerateBarcodeImage(Barcode);
                }
                return _barcodeImage;
            }
        }
        private BitmapImage _barcodeImage;
    }
}
