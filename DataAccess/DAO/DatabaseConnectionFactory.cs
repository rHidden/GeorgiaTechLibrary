using DataAccess.DAO.DAOIntefaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DataAccess.DAO
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
