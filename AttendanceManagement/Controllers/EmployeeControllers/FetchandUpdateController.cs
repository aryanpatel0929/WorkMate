using AttendanceManagement.Models.EmployeeModel;
using AttendanceManagement.Services;
using DatabaseConnection.DapperDatabaseConnection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AttendanceManagement.Controllers.EmployeeControllers
{
    public class FetchandUpdateController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ISqlConnection _sqlConnection;
        public FetchandUpdateController(IConfiguration configuration, ISqlConnection sqlConnection)
        {
            _configuration = configuration;
            _sqlConnection = sqlConnection;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FetchUserDetails(string id)
        {
            EmployeeOverview? employee = new EmployeeOverview();
            try
            {
                var employeeData = await _sqlConnection.LoadData<EmployeeOverview, dynamic>("[dbo].[SP_GetEmployeeOverviewById]", new { EmployeeID = id });
                employee = employeeData.FirstOrDefault();

                if (employee == null)
                {
                    Console.WriteLine($"No user details found for ID: {id}");
                    // You can redirect to an error page or a different view if no data is found
                    return RedirectToAction("Error");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while fetching user details: {ex.Message}");
                // Optionally redirect to an error page
                return RedirectToAction("Error");
            }

            // Return the UserDetailsView if data is found
            return View("UserDetailsView", employee);
        }

        public IActionResult DisplayUserDetails()
        {
            return View();
        }
    }

}
