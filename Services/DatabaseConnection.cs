using Microsoft.Data.SqlClient;

namespace AttendanceManagement.Services
{
    public interface IDatabaseConnection
    {
        public SqlConnection CreateSqlConnection();
    }

    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly IConfiguration configuration;
        public DatabaseConnection(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public SqlConnection CreateSqlConnection()
        {
            string? connectionString = configuration["SQLConnectionString:ConnectionString"];
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
