using AttendanceManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

namespace AttendanceManagement.Controllers.ProductsControllers
{
    public class ProductAmountPaidController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        public ProductAmountPaidController(IConfiguration configuration, IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InsertPaidAmount(ProductAmountPaidClass productAmountPaidClass)
        {
            try
            {
                // Define the SQL query for insertion
                var query = @"INSERT INTO [dbo].[ProductAmountPaid]
                      (ClientName, AmountPaid, Date)
                      VALUES (@ClientName, @AmountPaid, @Date)";
                string? connectionString = _configuration["SQLConnectionString:ConnectionString"];
                if (string.IsNullOrEmpty(connectionString))
                {
                    Console.WriteLine("Connection string is null or empty.");
                }
                // Establish database connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Prepare the SQL command
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ClientName", productAmountPaidClass.ClientName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@AmountPaid", productAmountPaidClass.AmountPaid ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Date", productAmountPaidClass.Date ?? (object)DBNull.Value);

                        // Execute the SQL command
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return RedirectToAction("Index", "Product");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return RedirectToAction("Index", "Product");
            }
        }
    }
}
