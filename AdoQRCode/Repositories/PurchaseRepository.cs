using AdoQRCode.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoQRCode.Repositories
{
    public class PurchaseRepository : IRepository<Purchase>
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private string tableName = $"[dbo].[purchases]";

        public void Add(Purchase purchase, out string message)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string insertSql = $"INSERT INTO {tableName} ([purchaseGUID],[purchaseDate],[productId],[customerName]," +
                    $"[exchangeRate],[priceKZT],[purchaseQR],[shippingQR])" +
                    $"VALUES (@purchaseGUID, @purchaseDate, @productId, @customerName, @exchangeRate, @priceKZT, @purchaseQR, @shippingQR)";
                SqlCommand command = new SqlCommand(insertSql, connection);
                command.Parameters.Add(new SqlParameter("@purchaseGUID", SqlDbType.NVarChar, 255)).Value = purchase.PurchaseGuid.ToString();
                command.Parameters.Add(new SqlParameter("@purchaseDate", SqlDbType.Date)).Value = purchase.PurchaseDate;
                command.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int, 16)).Value = purchase.ProductId;
                command.Parameters.Add(new SqlParameter("@customerName", SqlDbType.NVarChar, 300)).Value = purchase.CustomerName;
                command.Parameters.Add(new SqlParameter("@exchangeRate", SqlDbType.Float)).Value = purchase.ExchangeRate;
                command.Parameters.Add(new SqlParameter("@priceKZT", SqlDbType.Float)).Value = purchase.PriceKZT;
                command.Parameters.Add(new SqlParameter("@purchaseQR", SqlDbType.VarBinary, 1)).Value = purchase.PurchaseQr;
                command.Parameters.Add(new SqlParameter("@shippingQR", SqlDbType.VarBinary, 1)).Value = purchase.ShippingQr;                
                command.ExecuteNonQuery();
            }
            message = $"Данные о покупке №{purchase.PurchaseGuid}";
        }

        public void Delete(int purchaseId, out string message)
        {
            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();
                string deleteSql = $"DELETE FROM {tableName} WHERE [Id]=@id";
                SqlCommand command = new SqlCommand(deleteSql, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 16)).Value= purchaseId;
                command.ExecuteNonQuery();
            }
            message = $"Покупка №{purchaseId} успешно удалена из базы данных";
        }

        public Purchase Read(int purchaseId)
        {
            Purchase purchase = null;

            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();
                string selectSql = $"SELECT * FROM {tableName} WHERE [Id]=@id";
                SqlCommand command = new SqlCommand(selectSql, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 16)).Value = purchaseId;
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    object pId = reader.GetValue(0);
                    object pGuid = reader.GetValue(1);
                    object pDate = reader.GetValue(2);
                    object pProdId = reader.GetValue(3);
                    object pCustName = reader.GetValue(4);
                    object pExRate = reader.GetValue(5);
                    object pPrice = reader.GetValue(6);
                    object pPurQr = reader.GetValue(7);
                    object pShipQr = reader.GetValue(8);

                    purchase = new Purchase
                    {
                        Id = Int32.Parse(pId.ToString()),
                        PurchaseGuid = new Guid(pGuid.ToString()),
                        PurchaseDate = Convert.ToDateTime(pDate.ToString()),
                        ProductId = Int32.Parse(pProdId.ToString()),
                        CustomerName = pCustName.ToString(),
                        ExchangeRate = double.Parse(pExRate.ToString().Replace('.', ',')),
                        PriceKZT = double.Parse(pPrice.ToString().Replace('.', ',')),
                        PurchaseQr = Encoding.ASCII.GetBytes(pPurQr.ToString()),
                        ShippingQr = Encoding.ASCII.GetBytes(pShipQr.ToString())
                    };
                }
            }
            return purchase;
        }

        public IEnumerable<Purchase> ReadAll()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string selectSql = $"SELECT * FROM {tableName}";
                SqlCommand command = new SqlCommand(selectSql, connection);                
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        object pId = reader.GetValue(0);
                        object pGuid = reader.GetValue(1);
                        object pDate = reader.GetValue(2);
                        object pProdId = reader.GetValue(3);
                        object pCustName = reader.GetValue(4);
                        object pExRate = reader.GetValue(5);
                        object pPrice = reader.GetValue(6);
                        object pPurQr = reader.GetValue(7);
                        object pShipQr = reader.GetValue(8);

                        yield return new Purchase
                        {
                            Id = Int32.Parse(pId.ToString()),
                            PurchaseGuid = new Guid(pGuid.ToString()),
                            PurchaseDate = Convert.ToDateTime(pDate.ToString()),
                            ProductId = Int32.Parse(pProdId.ToString()),
                            CustomerName = pCustName.ToString(),
                            ExchangeRate = double.Parse(pExRate.ToString().Replace('.', ',')),
                            PriceKZT = double.Parse(pPrice.ToString().Replace('.', ',')),
                            PurchaseQr = Encoding.ASCII.GetBytes(pPurQr.ToString()),
                            ShippingQr = Encoding.ASCII.GetBytes(pShipQr.ToString())
                        };
                    }
                }
            }
        }

        public void Update(int purchaseId, Purchase updated, out string message)
        {
            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                string updateSql = $"UPDATE {tableName} SET [purchaseGUID]=@purchaseGUID,[purchaseDate]=@purchaseDate,[productId]=@productId," +
                    $"[customerName]=@customerName,[exchangeRate]=@exchangeRate,[priceKZT]=@priceKZT," +
                    $"[purchaseQR]=@purchaseQR,[shippingQR]=@shippingQR WHERE [Id]=@id";
                SqlCommand command = new SqlCommand(updateSql, connection);
                command.Parameters.Add(new SqlParameter("@purchaseGUID", SqlDbType.NVarChar, 255)).Value = updated.PurchaseGuid.ToString();
                command.Parameters.Add(new SqlParameter("@purchaseDate", SqlDbType.Date)).Value = updated.PurchaseDate;
                command.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int, 16)).Value = updated.ProductId;
                command.Parameters.Add(new SqlParameter("@customerName", SqlDbType.NVarChar, 300)).Value = updated.CustomerName;
                command.Parameters.Add(new SqlParameter("@exchangeRate", SqlDbType.Float)).Value = updated.ExchangeRate;
                command.Parameters.Add(new SqlParameter("@priceKZT", SqlDbType.Float)).Value = updated.PriceKZT;
                command.Parameters.Add(new SqlParameter("@purchaseQR", SqlDbType.VarBinary, 1)).Value = updated.PurchaseQr;
                command.Parameters.Add(new SqlParameter("@shippingQR", SqlDbType.VarBinary, 1)).Value = updated.ShippingQr;
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 16)).Value = purchaseId;
                command.ExecuteNonQuery();
            }
            message= $"Параметры покупки №{purchaseId} в базе данных обновлены";
        }
    }
}
