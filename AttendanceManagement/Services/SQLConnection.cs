namespace AttendanceManagement.Services
{
    using AttendanceManagement.Models;
    using AttendanceManagement.Models.EmployeeModel;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Data.SqlClient;
    using System.Net;
    using System.Reflection;

    public class SQLConnection
    {
        private readonly IConfiguration configuration;

        public SQLConnection(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        public async Task<EmployeeOverview?> FetchData(string? id)
        {
            if (id == null)
            {
                Console.WriteLine("ID is null. Cannot fetch data.");
                return null;
            }

            EmployeeOverview? employee = new EmployeeOverview();

            try
            {
                // Fetch connection string
                string? connectionString = configuration["SQLConnectionString:ConnectionString"];
                if (string.IsNullOrEmpty(connectionString))
                {
                    Console.WriteLine("Connection string is null or empty.");
                    return null;
                }

                // Define SQL query to fetch data
                var query = @"SELECT * 
                      FROM [dbo].[EmployeeOverview] 
                      WHERE [EmployeeID] = @EmployeeID";

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(query, connection))
                    {
                        // Add parameter to query
                        command.Parameters.AddWithValue("@EmployeeID", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    employee = new EmployeeOverview
                                    {
                                        EmployeeID = reader["EmployeeID"] as string,
                                        EmployeeName = reader["EmployeeName"] as string,
                                        Address = reader["Address"] as string,
                                        AadhaarCardNumber = reader["AadhaarCardNumber"] as string,
                                        ProfileImage = reader["ProfileImage"] as string,
                                        Gender = reader["Gender"] as string,
                                        Age = reader["Age"] as int?,
                                        Salary = (decimal)reader["Salary"],
                                        JoiningDate = (DateTime)reader["JoiningDate"]
                                    };
                                }
                            }
                            else
                            {
                                Console.WriteLine($"No data found for ID: {id}");
                            }
                        }
                    }
                }
                return employee;
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
