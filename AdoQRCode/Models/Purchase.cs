using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AdoQRCode.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public Guid PurchaseGuid { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int ProductId { get; set; }
        public string CustomerName { get; set; }
        public double ExchangeRate { get; set; }
        public double PriceKZT { get; set; }
        public byte[] PurchaseQr { get; set; }
        public byte[] ShippingQr { get; set; }

        public Purchase()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("https://nationalbank.kz/rss/rates_all.xml");

            string tmp = string.Empty;
            foreach (XmlNode item in doc.SelectNodes("//rss/channel/item"))
            {
                if (item.SelectSingleNode("title").InnerText == "USD")
                    tmp = item.SelectSingleNode("description").InnerText;
            }
            string s = tmp.Replace('.', ',');
            ExchangeRate = double.Parse(s);            
        }
    }
}
