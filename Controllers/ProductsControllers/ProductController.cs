using AttendanceManagement.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using static AttendanceManagement.Models.RegisterProduct;

namespace AttendanceManagement.Controllers.ProductsControllers
{
    public class ProductController : Controller
    {
        private readonly IConfiguration configuration;
        public ProductController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            List<RegisterProduct> products = new List<RegisterProduct>();
            ChartProductByMonth chartProductByMonth = new ChartProductByMonth();
            ChartProductByClient chartProductByClient = new ChartProductByClient();
            ProductModel productModel = new ProductModel();
            ChatDataofAmountPaid chatDataofAmount = new ChatDataofAmountPaid();
            products = await FetchProductDetails();
            chartProductByMonth = await FetchDataForFirstChart();
            chartProductByClient = await FetchDataForSecondChart();
            chatDataofAmount = await FetchData();
            float? totalSum = 0;
            foreach(var item in products)
            {
                totalSum = totalSum + item.Price;
            }
            ViewBag.TotalSum = totalSum;
            productModel.Products = products;
            productModel.ChartProductByMonth = chartProductByMonth;
            productModel.ChartProductByClient = chartProductByClient;
            productModel.chatDataofAmountPaid = chatDataofAmount;
            return View(productModel);
        }

        private async Task<List<T>> excuteReader<T>(string query, Func<SqlDataReader, T> map)
        {
            List<T> result = new List<T>();
            try
            {
                string? connectionString = configuration["SQLConnectionString:ConnectionString"] ?? "";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using var sqlCommand = new SqlCommand(query,connection);
                    using var reader = await sqlCommand.ExecuteReaderAsync();
                    while(await reader.ReadAsync())
                    {
                        result.Add(map(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }
        public async Task<List<RegisterProduct>?> FetchProductDetails()
        {
            // Define SQL query to fetch data
            var query = @"SELECT * FROM Products";
            return await excuteReader(query, reader =>
            {
                return new RegisterProduct
                {
                    ProductID = reader["ProductID"] as string,
                    ProductName = reader["ProductName"] as string,
                    ProductType = reader["ProductType"] as string,
                    ProductSize = (float?)((double)reader["ProductSize"] as double?),
                    ClientName = reader["ClientName"] as string,
                    Price = reader["Price"] as int?,
                    OrderDate = DateOnly.FromDateTime((DateTime)reader["OrderDate"]) as DateOnly?,
                    DeliveryDate = DateOnly.FromDateTime((DateTime)reader["DeliveryDate"]) as DateOnly?,
                };
            });
        }

        public async Task<ChartProductByMonth> FetchDataForFirstChart()
        {
            List<string> shortMonths = new List<string>
            {
                "Jan", "Feb", "Mar", "Apr", "May", "Jun",
                "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
            };
            var monthnumber = 0; ;
            var query = @"select MONTH(OrderDate) as MonthName, count(*) as ProductCount from [dbo].[Products] group by month(OrderDate)";
            var data = await excuteReader(query, reader =>
            {
                monthnumber = (int)reader["MonthName"];
                return new
                {
                    Month = shortMonths[monthnumber - 1],
                    CountProduct = (int)reader["ProductCount"]
                };
            });
            return new ChartProductByMonth
            {
                MonthName = (List<string>)data.Select(d => d.Month).ToList(),
                ProductCount = (List<int>)data.Select(d => d.CountProduct).ToList()
            };
        }

        public async Task<ChartProductByClient> FetchDataForSecondChart()
        {
            // Define SQL query to fetch data
            var query = @"select [ClientName] as ClientName, sum([Price]) as TotalPrice from [dbo].[Products] group by [ClientName]";
            var data = await excuteReader(query, reader => 
            {
                return new
                {
                    clientName = (string)reader["ClientName"],
                    productPrice = (int)reader["TotalPrice"]
                };
            });
            return new ChartProductByClient
            {
                ClientName = (List<string>)data.Select(p => p.clientName).ToList(),
                ProductPriceSum = (List<int>)data.Select(p => p.productPrice).ToList()
            };
        }


        public async Task<ChatDataofAmountPaid?> FetchData()
        {
            var query = @"select [ClientName] as ClientName, sum([AmountPaid]) as TotalPrice from [dbo].[ProductAmountPaid] group by [ClientName]";
            var data = await excuteReader(query, reader =>
            {
                return new
                {
                    clientname = (string)reader["ClientName"],
                    amountpaid = (int)reader["TotalPrice"]
                };
            });
            return new ChatDataofAmountPaid
            {
                ClientName = data.Select(p => p.clientname).ToList(),
                AmountPaid = data.Select(p => p.amountpaid).ToList(),
                TotalPendingAmount = data.Select(d => d.amountpaid).Sum()
            };
        }
    }
}
