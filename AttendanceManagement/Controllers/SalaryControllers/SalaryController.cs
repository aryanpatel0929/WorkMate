using AttendanceManagement.Models.AttendanceModel;
using AttendanceManagement.Models.EmployeeModel;
using AttendanceManagement.Services;
using AttendanceManagement.Services.SalaryOverviewServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;

namespace AttendanceManagement.Controllers.SalaryControllers
{
    public class SalaryController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache memoryCache;
        public SalaryController(IConfiguration configuration, IMemoryCache memoryCache)
        {
            _configuration = configuration;
            this.memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSalary(EmployeeSalaryPayment employeeSalaryPayment)
        {
            string? ConnectionString = _configuration["SQLConnectionString:ConnectionString"];
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Message = "Error in adding salary";
                    return Json(new { success = false });
                }
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                    string query = "INSERT INTO EmployeeSalaryPayment (EmployeeID, PaidAmount, PaymentMode, PaymentDate, PaymentReceivedBy) VALUES (@EmployeeID, @PaidAmount, @PaymentMode, @PaymentDate, @PaymentReceivedBy)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", employeeSalaryPayment.EmployeeID);
                        command.Parameters.AddWithValue("@PaidAmount", employeeSalaryPayment.PaidAmount);
                        command.Parameters.AddWithValue("@PaymentMode", employeeSalaryPayment.PaymentMode);
                        command.Parameters.AddWithValue("@PaymentDate", employeeSalaryPayment.PaymentDate);
                        command.Parameters.AddWithValue("@PaymentReceivedBy", employeeSalaryPayment.PaymentReceivedBy);
                        await command.ExecuteNonQueryAsync();
                    }
                    EmployeeSalaryOverview employeeSalaryOverview = new EmployeeSalaryOverview();
                    FetchDataSalaryOverview fetchDataSalaryOverview = new FetchDataSalaryOverview(_configuration, memoryCache);
                    employeeSalaryOverview = await fetchDataSalaryOverview.FetchOverviewData(employeeSalaryPayment.EmployeeID ?? "");
                    var updateSalaryOverviewQuery = @"UPDATE [dbo].[EmployeeSalaryOverview]
                                      SET TotalSalaryDue = @TotalSalaryDue
                                      WHERE EmployeeID = '7985881438'";

                    var commandquery = new SqlCommand(updateSalaryOverviewQuery, connection);
                    commandquery.Parameters.AddWithValue("@TotalSalaryDue", employeeSalaryOverview.TotalSalaryDue - employeeSalaryPayment.PaidAmount);
                    await commandquery.ExecuteNonQueryAsync();
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error in adding salary";
                Console.WriteLine(ex.ToString());
                return Json(new { success = false });
            }
        }
    }
}
