using System.Data;
using AttendanceManagement.Models.AttendanceModel;
using AttendanceManagement.Models.EmployeeModel;
using AttendanceManagement.Services;
using AttendanceManagement.Services.SalaryOverviewServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;

namespace AttendanceManagement.Controllers.AttendanceControllers
{
    public class AttendanceController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache memoryCache;
        public AttendanceController(IConfiguration configuration, IMemoryCache memoryCache)
        {
            this._configuration = configuration;
            this.memoryCache = memoryCache;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAttendance(Attendance attendance)
        {
            try
            {
                decimal? salary = 0;
                List<EmployeeOverview>? employeeData = memoryCache.Get("employeeData") as List<EmployeeOverview> ?? new List<EmployeeOverview>();
                if (employeeData == null)
                {
                    SQLConnection connection = new SQLConnection(_configuration);
                    EmployeeOverview? employeeDataById = await connection.FetchData(attendance.EmployeeID).ConfigureAwait(false);
                    if (employeeDataById == null)
                    {
                        // Handle case where employee data is not found
                        throw new InvalidOperationException("Employee data not found.");
                    }
                    salary = employeeDataById.Salary;
                }
                else
                {
                    foreach (var employee in employeeData)
                    {
                        if (employee.EmployeeID == attendance.EmployeeID)
                        {
                            salary = employee.Salary;
                            break;
                        }
                    }
                }

                string? connectionString = _configuration["SQLConnectionString:ConnectionString"];
                var attendanceQuery = @"INSERT INTO [dbo].[Attendance] 
                      (EmployeeID, Date, IsPresent, WorkingHours, Remarks)
                      VALUES (@EmployeeID, @Date, @IsPresent, @WorkingHours, @Remarks)";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                    using (var command = new SqlCommand(attendanceQuery, connection))
                    {
                        command.Parameters.Add("@EmployeeID", SqlDbType.Text).Value = attendance.EmployeeID;
                        command.Parameters.Add("@Date", SqlDbType.DateTime).Value = attendance.Date;
                        command.Parameters.Add("@IsPresent", SqlDbType.Bit).Value = attendance.IsPresent?1:0;
                        command.Parameters.Add("@WorkingHours", SqlDbType.Int).Value = attendance.WorkingHours;
                        command.Parameters.Add("@Remarks", SqlDbType.Text).Value = attendance.Remarks;
                        await command.ExecuteNonQueryAsync();
                    }
                    EmployeeSalaryOverview employeeSalaryOverview = new EmployeeSalaryOverview();
                    FetchDataSalaryOverview fetchDataSalaryOverview = new FetchDataSalaryOverview(_configuration, memoryCache);
                    employeeSalaryOverview = await fetchDataSalaryOverview.FetchOverviewData(attendance.EmployeeID??"");
                    var updateSalaryOverviewQuery = @"UPDATE [dbo].[EmployeeSalaryOverview]
                                      SET TotalWorkingDays = @TotalWorkingDays, 
                                          TotalSalary = @TotalSalary, 
                                          TotalSalaryDue = @TotalSalaryDue
                                      WHERE EmployeeID = '7985881438'";

                    var commandquery = new SqlCommand(updateSalaryOverviewQuery, connection);
                    commandquery.Parameters.AddWithValue("@TotalWorkingDays", employeeSalaryOverview.TotalWorkingDays + 1);
                    commandquery.Parameters.AddWithValue("@TotalSalary", employeeSalaryOverview.TotalSalary + employeeSalaryOverview.BasicSalary);
                    commandquery.Parameters.AddWithValue("@TotalSalaryDue", employeeSalaryOverview.TotalSalaryDue + employeeSalaryOverview.BasicSalary);
                    await commandquery.ExecuteNonQueryAsync();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index");
            }
        }
    }
}
