using AttendanceManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace AttendanceManagement.Controllers.ProductsControllers
{
    public class RegisterProductController : Controller
    {
        private readonly IConfiguration configuration;
        public RegisterProductController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewProduct(RegisterProduct registerProduct)
        {
            try
            {
                // Define the SQL query for insertion
                var query = @"INSERT INTO [dbo].[Products] 
                      (ProductID, ProductName, ProductDescription, ProductType, ProductSize, ClientName, Price, OrderDate, DeliveryDate)
                      VALUES (@ProductID, @ProductName, @ProductDescription, @ProductType, @ProductSize, @ClientName, @Price, @OrderDate, @DeliveryDate)";
                string? connectionString = configuration["SQLConnectionString:ConnectionString"];
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
                        command.Parameters.AddWithValue("@ProductID", registerProduct.ProductID ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ProductName", registerProduct.ProductName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ProductDescription", registerProduct.ProductDescription ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ProductType", registerProduct.ProductType ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ProductSize", registerProduct.ProductSize ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ClientName", registerProduct.ClientName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Price", registerProduct.Price ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@OrderDate", registerProduct.OrderDate ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@DeliveryDate", registerProduct.DeliveryDate ?? (object)DBNull.Value);

                        // Execute the SQL command
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return RedirectToAction("Index", "RegisterProduct");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "RegisterProduct");
            }
        }
    }
}
