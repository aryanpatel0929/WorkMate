using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace AttendanceManagement.Controllers
{
    public class CreateNewUserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IDatabaseConnection _connection;
        public CreateNewUserController(IConfiguration configuration, IDatabaseConnection connection)
        {
            _configuration = configuration;
            _connection = connection;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewUser(UserDetails userDetails)
        {
            string? ConnectionString = _configuration["SQLConnectionString:ConnectionString"];
            SQLConnection connection = new SQLConnection(_configuration);
            if (!ModelState.IsValid)
            {
                return new StatusCodeResult(404);
            }
            try
            {
                await connection.InsertDataIntoDatabase(userDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
