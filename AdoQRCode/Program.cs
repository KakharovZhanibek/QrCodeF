using AdoQRCode.Models;
using AdoQRCode.Repositories;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static QRCoder.PayloadGenerator;

namespace AdoQRCode
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Введите email отправителя и пароль*/
            UserService user = new UserService();
            user.Start();
        }
    }
}
