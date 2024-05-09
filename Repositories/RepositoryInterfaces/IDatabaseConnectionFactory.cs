using Microsoft.Data.SqlClient;
using System.Data;

namespace GeorgiaTechLibrary.Repositories.RepositoryInterfaces
{
    public interface IDatabaseConnectionFactory
    {
        public SqlConnection CreateConnection();
    }
}
