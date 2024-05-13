using Microsoft.Data.SqlClient;
using System.Data;

namespace GeorgiaTechLibrary.DbContext
{
    public interface IDatabaseConnectionFactory
    {
        public SqlConnection CreateConnection();
    }
}
