using AttendanceManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Security.Cryptography;

namespace AttendanceManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache memoryCache;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IMemoryCache memoryCache)
        {
            this._logger = logger;
            this._configuration = configuration;
            this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            //var loggedInUser = memoryCache.Get("userLogin");
            //if (loggedInUser == null)
            //{
            //    return RedirectToAction("Index", "Login");
            //}
            //else
            //{

            //}
            List<UserDetails> users = new List<UserDetails>();
            users = await FetchAllUserDetails();
            return View(users);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<List<UserDetails>?> FetchAllUserDetails()
        {

            List<UserDetails>? userDetails = new List<UserDetails>();
            try
            {
                // Fetch connection string
                string? connectionString = _configuration["SQLConnectionString:ConnectionString"];
                if (string.IsNullOrEmpty(connectionString))
                {
                    Console.WriteLine("Connection string is null or empty.");
                    return null;
                }

                // Define SQL query to fetch data
                var query = @"SELECT * FROM Users";

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    var userDetail = new UserDetails
                                    {
                                        UserId = reader["UserId"] as string,
                                        UserName = reader["UserName"] as string,
                                        Address = reader["Address"] as string,
                                        AadhaarCardNumber = reader["AadhaarCardNumber"] as string,
                                        ProfileImage = reader["ProfileImage"] as string,
                                        Gender = reader["Gender"] as string,
                                        Age = reader["Age"] as int?,
                                        Salary = (float?)((double)reader["Salary"] as double?),
                                        JoiningDate = DateOnly.FromDateTime((DateTime)reader["JoiningDate"]) as DateOnly?
                                    };
                                    userDetails.Add(userDetail);
                                }
                                
                            }
                            else
                            {
                                Console.WriteLine($"No data found");
                            }
                        }
                    }
                }
                return userDetails ??[];
            }
            catch (SqlException sqlEx)
            {
                // Log SQL-specific exceptions
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return null;
        }
    }
}
