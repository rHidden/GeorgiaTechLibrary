using DataAccess.DAO.DAOIntefaces;
using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;
using System.Data.Common;

namespace DataAccess.Repositories
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public LibraryRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _connectionFactory = databaseConnectionFactory;
        }

        private void AddParameter(DbCommand command, string name, object value)
        {
            var param = command.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            command.Parameters.Add(param);
        }

        public async Task<Library> GetLibrary(string libraryName)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();

                    // Get Library details
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM Library WHERE Name = @Name";
                    AddParameter(command, "@Name", libraryName);

                    Library library = null;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            library = new Library
                            {
                                Name = reader["Name"].ToString(),
                                LibraryAddress = new Address
                                {
                                    Street = reader["Street"].ToString(),
                                    StreetNumber = reader["StreetNumber"].ToString(),
                                    City = reader["City"].ToString(),
                                    ZipCode = reader["Zipcode"].ToString()
                                }
                            };
                        }
                    }

                    if (library == null) return null;

                    // Get Users
                    command.CommandText = "SELECT * FROM [User] WHERE LibraryName = @LibraryName";
                    command.Parameters.Clear();
                    AddParameter(command, "@LibraryName", libraryName);
                    library.Users = new List<User>();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            library.Users.Add(new User
                            {
                                SSN = reader["SSN"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                PhoneNum = reader["PhoneNumber"].ToString(),
                                UserAddress = new Address
                                {
                                    Street = reader["Street"].ToString(),
                                    StreetNumber = reader["StreetNumber"].ToString(),
                                    City = reader["City"].ToString(),
                                    ZipCode = reader["Zipcode"].ToString()
                                }
                            });
                        }
                    }

                    // Get BookInstances
                    command.CommandText = @"
                        SELECT bi.*, b.ISBN, b.CanLoan, b.Description, b.SubjectArea
                        FROM BookInstance bi
                        JOIN Book b ON bi.BookISBN = b.ISBN
                        WHERE bi.LibraryName = @LibraryName";
                    command.Parameters.Clear();
                    AddParameter(command, "@LibraryName", libraryName);
                    library.BookInstances = new List<BookInstance>();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            library.BookInstances.Add(new BookInstance
                            {
                                Id = (int)reader["Id"],
                                Book = new Book
                                {
                                    ISBN = reader["ISBN"].ToString(),
                                    CanLoan = (bool)reader["CanLoan"],
                                    Description = reader["Description"].ToString(),
                                    SubjectArea = reader["SubjectArea"].ToString()
                                },
                                IsLoaned = (bool)reader["IsLoaned"]
                            });
                        }
                    }

                    // Get DigitalItems
                    command.CommandText = @"
                        SELECT dil.*, di.*
                        FROM DigitalItemLibrary dil
                        JOIN DigitalItem di ON dil.DigitalItemId = di.Id
                        WHERE dil.LibraryName = @LibraryName";
                    command.Parameters.Clear();
                    AddParameter(command, "@LibraryName", libraryName);
                    library.DigitalItems = new List<DigitalItem>();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            library.DigitalItems.Add(new DigitalItem
                            {
                                Id = (int)reader["Id"],
                                Name = reader["Name"].ToString(),
                                Author = reader["Author"].ToString(),
                                Format = reader["Format"].ToString(),
                                Size = double.Parse(reader["Size"].ToString())
                            });
                        }
                    }

                    return library;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving library: {ex.Message}");
            }
            return null;
        }

        public async Task<List<Library>> ListLibraries()
        {
            var libraries = new List<Library>();
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();

                    // Get Libraries
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM Library";

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var libraryName = reader["Name"].ToString();
                            libraries.Add(await GetLibrary(libraryName));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error listing libraries: {ex.Message}");
            }
            return libraries;
        }

        public async Task<Library> CreateLibrary(Library library)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO Library (Name, Street, StreetNumber, City, Zipcode) VALUES (@Name, @Street, @StreetNumber, @City, @Zipcode)";

                    AddParameter(command, "@Name", library.Name);
                    AddParameter(command, "@Street", library.LibraryAddress.Street);
                    AddParameter(command, "@StreetNumber", library.LibraryAddress.StreetNumber);
                    AddParameter(command, "@City", library.LibraryAddress.City);
                    AddParameter(command, "@Zipcode", library.LibraryAddress.ZipCode);

                    await command.ExecuteNonQueryAsync();
                    return library;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating library: {ex.Message}");
            }
            return null;
        }

        public async Task<Library> UpdateLibrary(Library library)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();

                    var updateFields = new List<string>();
                    if (library.LibraryAddress != null)
                    {
                        if (!string.IsNullOrEmpty(library.LibraryAddress.Street))
                        {
                            updateFields.Add("Street = @Street");
                            AddParameter(command, "@Street", library.LibraryAddress.Street);
                        }

                        if (!string.IsNullOrEmpty(library.LibraryAddress.StreetNumber))
                        {
                            updateFields.Add("StreetNumber = @StreetNumber");
                            AddParameter(command, "@StreetNumber", library.LibraryAddress.StreetNumber);
                        }

                        if (!string.IsNullOrEmpty(library.LibraryAddress.City))
                        {
                            updateFields.Add("City = @City");
                            AddParameter(command, "@City", library.LibraryAddress.City);
                        }

                        if (!string.IsNullOrEmpty(library.LibraryAddress.ZipCode))
                        {
                            updateFields.Add("Zipcode = @Zipcode");
                            AddParameter(command, "@Zipcode", library.LibraryAddress.ZipCode);
                        }
                    }

                    if (updateFields.Count == 0)
                    {
                        Console.WriteLine("No fields to update");
                        return null;
                    }

                    command.CommandText = $"UPDATE Library SET {string.Join(", ", updateFields)} WHERE Name = @Name";
                    AddParameter(command, "@Name", library.Name);

                    await command.ExecuteNonQueryAsync();

                    return await GetLibrary(library.Name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating library: {ex.Message}");
                return null;
            }
        }


        public async Task<Library> DeleteLibrary(string libraryName)
        {
            try
            {
                var library = await GetLibrary(libraryName);

                if (library == null)
                {
                    Console.WriteLine("Library not found");
                    return null;
                }

                using (var connection = _connectionFactory.CreateConnection())
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM Library WHERE Name = @Name";

                    AddParameter(command, "@Name", libraryName);

                    await command.ExecuteNonQueryAsync();
                }

                return library;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting library: {ex.Message}");
                return null;
            }
        }

    }
}
