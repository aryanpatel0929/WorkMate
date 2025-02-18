namespace DatabaseConnection.DapperDatabaseConnection
{
    using System.Data;
    using System.Threading.Tasks;
    public interface ISqlConnection
    {
        Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters);
        Task<IEnumerable<T>> LoadDataAllData<T>(string storeProcedure);
        Task SaveData<T>(string storeProcedure, T parameters);
        Task UpdateData<T>(string storeProcedure, T parameters);
        Task DeleteData<T>(string storeProcedure, T parameters);
    }   
}