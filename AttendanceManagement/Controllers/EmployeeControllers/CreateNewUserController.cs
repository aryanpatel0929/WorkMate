using AttendanceManagement.Models;
using AttendanceManagement.Models.EmployeeModel;
using AttendanceManagement.Services;
using DatabaseConnection.DapperDatabaseConnection;
using Microsoft.AspNetCore.Mvc;
namespace AttendanceManagement.Controllers.EmployeeControllers
{
    public class CreateNewUserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IDatabaseConnection _connection;
        private readonly ISqlConnection _sqlConnection;
        public CreateNewUserController(IConfiguration configuration, IDatabaseConnection connection, ISqlConnection sqlConnection)
        {
            _configuration = configuration;
            _connection = connection;
            _sqlConnection = sqlConnection;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewUser(EmployeeOverview employee)
        {
            string? ConnectionString = _configuration["SQLConnectionString:ConnectionString"];
            SQLConnection connection = new SQLConnection(_configuration);
            if (!ModelState.IsValid)
            {
                return new StatusCodeResult(404);
            }
            try
            {
                await _sqlConnection.SaveData("[dbo].[SP_Insert_Employee_Overview]", employee);
                var salaryOverview = new EmployeeSalaryOverview
                {
                    SalaryId = new Guid(),
                    EmployeeID = employee.EmployeeID,
                    TotalWorkingDays = 0,
                    TotalSalary = 0m,
                    TotalSalaryDue = 0m,
                    BasicSalary = employee.Salary / 30,
                    BasicSalaryStatus = "current",
                    CreatedOrUpdatedDate = DateTime.Now
                };
                await _sqlConnection.UpdateData("[dbo].[SP_Insert_Employee_Salary_Overview]", salaryOverview);
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
