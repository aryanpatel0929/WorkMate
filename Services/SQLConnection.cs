namespace AttendanceManagement.Services
{
    using AttendanceManagement.Models;
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

        public async Task InsertDataIntoDatabase(UserDetails model)
        {
            try
            {
                // Define the SQL query for insertion
                var query = @"INSERT INTO [dbo].[Users] 
                      (UserID, UserName, Address, AadhaarCardNumber, ProfileImage, Gender, Age, Salary, JoiningDate)
                      VALUES (@UserID, @UserName, @Address, @AadhaarCardNumber, @ProfileImage, @Gender, @Age, @Salary, @JoiningDate)";
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
                        command.Parameters.AddWithValue("@UserID", model.UserId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@UserName", model.UserName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Address", model.Address ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@AadhaarCardNumber", model.AadhaarCardNumber ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ProfileImage", model.ProfileImage ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Gender", model.Gender ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Age", model.Age ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Salary", model.Salary ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@JoiningDate", model.JoiningDate ?? (object)DBNull.Value);

                        // Execute the SQL command
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task<UserDetails?> FetchData(string? id)
        {
            if (id == null)
            {
                Console.WriteLine("ID is null. Cannot fetch data.");
                return null;
            }

            UserDetails? userDetails = null;

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
                      FROM Users 
                      WHERE UserID = @UserId";

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(query, connection))
                    {
                        // Add parameter to query
                        command.Parameters.AddWithValue("@UserId", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    Console.WriteLine(DateOnly.FromDateTime((DateTime)reader["JoiningDate"]) as DateOnly?);
                                    // Map data from database to UserDetails object
                                    userDetails = new UserDetails
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
                                }
                            }
                            else
                            {
                                Console.WriteLine($"No data found for ID: {id}");
                            }
                        }
                    }
                }

                return userDetails;
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
