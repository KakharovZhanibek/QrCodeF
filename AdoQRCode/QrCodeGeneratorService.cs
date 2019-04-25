using AdoQRCode.Models;
using AdoQRCode.Repositories;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;

namespace AdoQRCode
{
    public class QrCodeGeneratorService
    {
        private ProductRepository _productRepository;

        public QrCodeGeneratorService()
        {
            _productRepository = new ProductRepository();
        }

        public byte[] GetQrCodePurchaseInfo(Purchase purchase, out string path)
        {
            byte[] tmp = null;

            Product product = _productRepository.Read(purchase.ProductId);
            string purchaseInfo = $"{purchase.PurchaseDate.ToShortDateString()} покупателем {purchase.CustomerName} " +
                $"была совершена покупка {product.Name} на сумму {purchase.PriceKZT}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(purchaseInfo, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            Console.WriteLine("Введите путь :");
            string pathToSave = Console.ReadLine();
            string fileName = $"{"purchaseInfo_" + purchase.PurchaseGuid.ToString()}.png";
            path = Path.Combine(pathToSave, fileName);
            qrCodeImage.Save(path);

            using (MemoryStream ms = new MemoryStream())
            {
                qrCodeImage.Save(ms, ImageFormat.Png);
                tmp = ms.ToArray();
                return tmp;
            }
        }

        public byte[] GetQrCodeShippingInfo(Purchase purchase, out string path)
        {
            byte[] tmp = null;
            Random rnd = new Random();
            double latitude = Math.Round(43 + rnd.NextDouble(), 5);
            double longitude = Math.Round(76 + rnd.NextDouble(), 5);

            Geolocation generator = new Geolocation(latitude.ToString(), longitude.ToString());
            string payload = generator.ToString();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            string pathToSave = ConfigurationManager.AppSettings["qrCodesOutputDirectory"];
            string fileName = $"{"shippingInfo_" + purchase.PurchaseGuid.ToString()}.png";
            path = Path.Combine(pathToSave, fileName);
            qrCodeImage.Save(path);

            using (MemoryStream ms = new MemoryStream())
            {
                qrCodeImage.Save(ms, ImageFormat.Png);
                tmp = ms.ToArray();
                return tmp;
            }
        }
    }
}
