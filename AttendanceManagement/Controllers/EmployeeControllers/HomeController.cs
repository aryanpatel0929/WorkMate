using AttendanceManagement.Models;
using AttendanceManagement.Models.EmployeeModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using DatabaseConnection.DapperDatabaseConnection;
using Microsoft.AspNetCore.Authorization;

namespace AttendanceManagement.Controllers.EmployeeControllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache memoryCache;
        private ISqlConnection sqlConnection;


        public HomeController(IConfiguration configuration, IMemoryCache memoryCache, ISqlConnection sqlConnection)
        {
            _configuration = configuration;
            this.memoryCache = memoryCache;
            this.sqlConnection = sqlConnection;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<EmployeeOverview>? employees = new List<EmployeeOverview>();
            employees = await sqlConnection.LoadDataAllData<EmployeeOverview>("[dbo].[SP_GetAllEmployeeOverview]");
            if (employees == null)
            {
                return View("Index", "Error");
            }
            return View(employees);
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
    }
}
