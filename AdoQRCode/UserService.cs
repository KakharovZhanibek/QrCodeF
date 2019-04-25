using AdoQRCode.Models;
using AdoQRCode.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdoQRCode
{
    public class UserService
    {
        ProductRepository productDb = new ProductRepository();
        PurchaseRepository purchaseDb = new PurchaseRepository();
        QrCodeGeneratorService qr = new QrCodeGeneratorService();
        
        public void Start()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=================================");
                Console.WriteLine("ID\tНазвание \\ Цена(в USD)");
                Console.WriteLine("---------------------------------");
                List<Product> products = productDb.ReadAll().ToList();
                foreach (Product item in products)
                {
                    Console.WriteLine($"{item.Id}\t{item.Name} \\ ${item.PriceUSD}");
                }
                Console.WriteLine("---------------------------------\n");
                Console.Write("Выберите ID товара (0-выход): ");
                int choice = Int32.Parse(Console.ReadLine());
                if (choice == 0)
                    break;
                else if (choice > products.Count() || choice < 0)
                    Console.WriteLine("Товара с таким ID в магазине нет");
                else
                    PurchaseMenu(choice);
            }
        }

        public void PurchaseMenu(int productId)
        {
            Product product = productDb.Read(productId);
            Purchase purchase = new Purchase { PurchaseGuid = Guid.NewGuid(), PurchaseDate = DateTime.Now, ProductId = product.Id };
            purchase.PriceKZT = product.PriceUSD * purchase.ExchangeRate;
            string purchaseInfoPath = string.Empty;
            string shippingInfoPath = string.Empty;
            string message = string.Empty;

            Console.Clear();
            Console.WriteLine("Форма покупки");
            Console.WriteLine("=================================");
            Console.WriteLine($"Вы покупате {product.Name}.\nСумма покупки составляет {purchase.PriceKZT} по текущему курсу");
            Console.WriteLine("---------------------------------");
            Console.Write("Введите свое имя: ");
            purchase.CustomerName= Console.ReadLine();
            purchase.PurchaseQr = qr.GetQrCodePurchaseInfo(purchase, out purchaseInfoPath);
            purchase.ShippingQr = qr.GetQrCodeShippingInfo(purchase, out shippingInfoPath);
            purchaseDb.Add(purchase, out message); //покупка добавлена в базу данных

            Console.Write("Введите свой e-mail: ");
            string email = Console.ReadLine();
            MailAddress from = new MailAddress(/*email отправителя*/"z.kontra@mail.ru", "Korean Shop");
            MailAddress to = new MailAddress(email);
            MailMessage mess = new MailMessage(from, to);
            mess.Subject = message;
            mess.Attachments.Add(new Attachment(purchaseInfoPath));
            mess.Attachments.Add(new Attachment(shippingInfoPath));
            
            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
            smtp.Credentials = new NetworkCredential(/*email отправителя*/"z.kontra@mail.ru", /*пароль*/"12*11*2000zhks");
            smtp.EnableSsl = true;
            smtp.Send(mess);
            Console.WriteLine("Информация о покупке и месте выдачи заказа отправлены Вам на почту");
            Thread.Sleep(2000);
        }
    }
}
