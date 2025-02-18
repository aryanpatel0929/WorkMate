using Dapper;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DatabaseConnection.DapperDatabaseConnection
{
    public class SqlDataAccess : ISqlConnection
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public SqlDataAccess()
        {
            _connectionString = "Server=localhost,1433; Database=msdb; User=sa; Password=yourStrong(!)Password;TrustServerCertificate=true";
        }

        public async Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<T>> LoadDataAllData<T>(string storedProcedure)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<T>(storedProcedure, commandType: CommandType.StoredProcedure);
        }

        public async Task SaveData<T>(string storedProcedure, T parameters)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateData<T>(string storedProcedure, T parameters)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteData<T>(string storedProcedure, T parameters)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
    }
}