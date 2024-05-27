using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using System.Drawing;

namespace WPF_Supermarket.Services
{
    public class BarcodeService
    {
        public byte[] GenerateBarcode(string text)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 100,
                    Width = 300
                }
            };

            using (Bitmap bitmap = writer.Write(text))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
