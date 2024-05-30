using DataAccess.DAO.DAOIntefaces;
using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;
using System.Data.Common;
using Dapper;

namespace DataAccess.Repositories
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public LibraryRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _connectionFactory = databaseConnectionFactory;
        }

        public async Task<Library> GetLibrary(string libraryName)
        {
            //string sql = "SELECT library.[Name], library.[Street], library.[StreetNumber], " +
            //    "library.[City], library.[Zipcode], " +
            //    "FROM Library" +
            //    "WHERE library.[Name] = @LibraryName";
            //using (var connection = _connectionFactory.CreateConnection())
            //{

            //    await connection.OpenAsync();

            //    // Get Library details
            //    var command = connection.CreateCommand();
            //    command.CommandText = "SELECT * FROM Library WHERE Name = @Name";
            //    AddParameter(command, "@Name", libraryName);

            //    Library library = null;
            //    using (var reader = await command.ExecuteReaderAsync())
            //    {
            //        if (await reader.ReadAsync())
            //        {
            //            library = new Library
            //            {
            //                Name = reader["Name"].ToString(),
            //                LibraryAddress = new Address
            //                {
            //                    Street = reader["Street"].ToString(),
            //                    StreetNumber = reader["StreetNumber"].ToString(),
            //                    City = reader["City"].ToString(),
            //                    ZipCode = reader["Zipcode"].ToString()
            //                }
            //            };
            //        }
            //    }

            //    if (library == null) return null;

            //    // Get Users
            //    command.CommandText = "SELECT * FROM user WHERE LibraryName = @LibraryName";
            //    command.Parameters.Clear();
            //    AddParameter(command, "@LibraryName", libraryName);
            //    library.Users = new List<User>();

            //    using (var reader = await command.ExecuteReaderAsync())
            //    {
            //        while (await reader.ReadAsync())
            //        {
            //            library.Users.Add(new User
            //            {
            //                SSN = reader["SSN"].ToString(),
            //                FirstName = reader["FirstName"].ToString(),
            //                LastName = reader["LastName"].ToString(),
            //                PhoneNumber = reader["PhoneNumber"].ToString(),
            //                UserAddress = new Address
            //                {
            //                    Street = reader["Street"].ToString(),
            //                    StreetNumber = reader["StreetNumber"].ToString(),
            //                    City = reader["City"].ToString(),
            //                    ZipCode = reader["Zipcode"].ToString()
            //                }
            //            });
            //        }
            //    }

            //    // Get BookInstances
            //    command.CommandText = @"
            //        SELECT bi.*, b.ISBN, b.Status, b.Description, b.SubjectArea
            //        FROM BookInstance bi
            //        JOIN Book b ON bi.BookISBN = b.ISBN
            //        WHERE bi.LibraryName = @LibraryName";
            //    command.Parameters.Clear();
            //    AddParameter(command, "@LibraryName", libraryName);
            //    library.BookInstances = new List<BookInstance>();

            //    using (var reader = await command.ExecuteReaderAsync())
            //    {
            //        while (await reader.ReadAsync())
            //        {
            //            library.BookInstances.Add(new BookInstance
            //            {
            //                Id = (int)reader["Id"],
            //                Book = new Book
            //                {
            //                    ISBN = reader["ISBN"].ToString(),
            //                    Status = (bool)reader["Status"],
            //                    Description = reader["Description"].ToString(),
            //                    SubjectArea = reader["SubjectArea"].ToString()
            //                },
            //                IsLoaned = (bool)reader["IsLoaned"]
            //            });
            //        }
            //    }

            //    // Get DigitalItems
            //    command.CommandText = @"
            //        SELECT dil.*, di.*
            //        FROM DigitalItemLibrary dil
            //        JOIN DigitalItem di ON dil.DigitalItemId = di.Id
            //        WHERE dil.LibraryName = @LibraryName";
            //    command.Parameters.Clear();
            //    AddParameter(command, "@LibraryName", libraryName);
            //    library.DigitalItems = new List<DigitalItem>();

            //    using (var reader = await command.ExecuteReaderAsync())
            //    {
            //        while (await reader.ReadAsync())
            //        {
            //            library.DigitalItems.Add(new DigitalItem
            //            {
            //                Id = (int)reader["Id"],
            //                Name = reader["Name"].ToString(),
            //                Author = reader["Author"].ToString(),
            //                Format = reader["Format"].ToString(),
            //                Size = double.Parse(reader["Size"].ToString())
            //            });
            //        }
            //    }

            //    return library;
            //}
            return null;
        }

        public async Task<List<Library>> ListLibraries()
        {
            string sql = "SELECT * FROM Library";
            using (var connection = _connectionFactory.CreateConnection())
            {
                var libraries = (await connection.QueryAsync<Library, Address, Library>(sql, map: ((library, address) =>
                {
                    library.LibraryAddress = address;
                    return library;
                }), splitOn: "Street")).AsQueryable().AsList();
                return libraries;
            }
        }

        public async Task<Library> CreateLibrary(Library library)
        {
            string sql = "INSERT INTO Library (Name, Street, StreetNumber, City, Zipcode) VALUES (@Name, @Street, @StreetNumber, @City, @Zipcode)";
            using (var connection = _connectionFactory.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    Name = library.Name,
                    Street = library.LibraryAddress.Street,
                    StreetNumber = library.LibraryAddress.StreetNumber,
                    ZipCode = library.LibraryAddress.ZipCode,
                    City = library.LibraryAddress.City
                });
                return library;
            }
        }

        public async Task<Library> UpdateLibrary(Library library)
        {
            string sql = "UPDATE [Library] SET [Street] = @Street, [StreetNumber] = @StreetNumber, " +
                "[City] = @City, [Zipcode] = @Zipcode WHERE Name = @Name";
            using (var connection = _connectionFactory.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    Name = library.Name,
                    Street = library.LibraryAddress.Street,
                    StreetNumber = library.LibraryAddress.StreetNumber,
                    ZipCode = library.LibraryAddress.ZipCode,
                    City = library.LibraryAddress.City
                });
                return library;
            }
        }


        public async Task<bool> DeleteLibrary(string libraryName)
        {
            string sql = "DELETE FROM Library WHERE Name = @Name";

            using (var connection = _connectionFactory.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync(sql, new { Name = libraryName });

                return rowsAffected == 1;
            }
        }
    }
}
