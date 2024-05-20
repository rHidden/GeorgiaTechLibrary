using Xunit;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using System.Data;

namespace DataAccessTest
{
    public class DatabaseConnectionTest
    {
        public static readonly string _connectionString = "Server=(localdb)\\localDB1;Initial Catalog=GeorgiaTechLibrary;Integrated Security=true;TrustServerCertificate=true";

        [Fact]
        public async Task Connection_IsOpenAfterCreation()
        {
            // Arrange
            var connection = new SqlConnection(_connectionString);

            // Act
            await connection.OpenAsync();

            // Assert
            Assert.True(connection.State == ConnectionState.Open);
        }

        [Fact]
        public async Task QueryExecutesSuccessfully()
        {
            // Arrange
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var commandText = "SELECT @@VERSION AS Version";
            var command = new SqlCommand(commandText, connection);

            // Act
            var version = await command.ExecuteScalarAsync();

            // Assert
            Assert.NotNull(version);
            Assert.NotEmpty(version.ToString());
        }

        [Fact]
        public async Task Connection_ClosesAfterExecution()
        {
            // Arrange
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            // Act
            await connection.CloseAsync();

            // Assert
            Assert.True(connection.State == ConnectionState.Closed);
        }
    }
}
