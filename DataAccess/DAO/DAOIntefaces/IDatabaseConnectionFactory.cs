using Microsoft.Data.SqlClient;
using System.Data;

namespace DataAccess.DAO.DAOIntefaces
{
    public interface IDatabaseConnectionFactory
    {
        public SqlConnection CreateConnection();
    }
}
