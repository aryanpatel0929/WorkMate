using Microsoft.Extensions.Caching.Memory;
using AttendanceManagement.Models.EmployeeModel;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace AttendanceManagement.Services.SalaryOverviewServices
{
    public class FetchDataSalaryOverview
    {
        private readonly IConfiguration configuration;
        private readonly IMemoryCache memoryCache;
        public FetchDataSalaryOverview(IConfiguration configuration, IMemoryCache memoryCache)
        {
            this.configuration = configuration;
            this.memoryCache = memoryCache;
        }

        public async Task<EmployeeSalaryOverview> FetchOverviewData(string employeeID)
        {
            try
            {
                EmployeeSalaryOverview employeeSalaryOverview = new EmployeeSalaryOverview();
                string? connectionString = configuration["SQLConnectionString:ConnectionString"];
                string sqlquery = "select * from[dbo].[EmployeeSalaryOverview] where EmployeeID = '7985881438' and trim(lower(BasicSalaryStatus)) = 'current'";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                    using (var command = new SqlCommand(sqlquery,connection))
                    {
                        //command.Parameters.Add("@employeeID", System.Data.SqlDbType.Text).Value = employeeID;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    employeeSalaryOverview = new EmployeeSalaryOverview
                                    {
                                        EmployeeID = reader["EmployeeID"] as string,
                                        TotalWorkingDays = (int)reader["TotalWorkingDays"],
                                        TotalSalary = (decimal)reader["TotalSalary"],
                                        TotalSalaryDue = (decimal)reader["TotalSalaryDue"],
                                        BasicSalary = (decimal)reader["BasicSalary"]
                                    };
                                }
                            }
                        }
                    }
                }
                return employeeSalaryOverview;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
