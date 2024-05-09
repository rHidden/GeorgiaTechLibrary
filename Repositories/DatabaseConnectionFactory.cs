using GeorgiaTechLibrary.Repositories.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GeorgiaTechLibrary.Repositories
{
    public class DatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string _connectionString;
        public DatabaseConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
