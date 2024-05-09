using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Repositories.RepositoryInterfaces;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;

namespace GeorgiaTechLibrary.Repositories
{
    public class MemberRepository : IMemberRepository { 
        private readonly IDatabaseConnectionFactory _connectionFactory;
        public MemberRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _connectionFactory = databaseConnectionFactory;
        }
        public async Task<Member> CreateMember(Member member)
        {
            using var con = _connectionFactory.CreateConnection();
            await con.OpenAsync();
            var sql = @"INSERT INTO Member (SSN, FirstName, LastName, PhoneNumber, Street, StreetNumber, City, Zipcode, CardNumber, ExpiryDate, Photo, Type) VALUES
                    (@SSN, @FirstName, @LastName, @Street, @StreetNumber, @City, @Zipcode, @CardNumber, @ExpiryDate, @Photo, @Type)";
            using (SqlCommand command = new SqlCommand(sql, con))
            {
                command.Parameters.AddWithValue("@SSN", member.SSN);
                command.Parameters.AddWithValue("@FirstName", member.FirstName);
                command.Parameters.AddWithValue("@LastName", member.LastName);
                command.Parameters.AddWithValue("@PhoneNumber", member.StreetNumber);
                command.Parameters.AddWithValue("@Street", member.Street);
                command.Parameters.AddWithValue("@StreetNumber", member.StreetNumber);
                command.Parameters.AddWithValue("@City", member.City);
                command.Parameters.AddWithValue("@Zipcode", member.Zipcode);
                command.Parameters.AddWithValue("@CardNumber", member.CardNumber);
                command.Parameters.AddWithValue("@ExpiryDate", member.ExpiryDate);
                command.Parameters.AddWithValue("@Photo", member.Photo);
                command.Parameters.AddWithValue("@Type", member.Type);

                await command.ExecuteNonQueryAsync();
            };
            return member;
        }


        public async Task DeleteMember(int SSN)
        {
            using var con = _connectionFactory.CreateConnection();
            await con.OpenAsync();

            using var transaction = con.BeginTransaction();
            var sql = @"DELETE FROM Member WHERE SSN = @SSN";

            using (SqlCommand command = new SqlCommand(sql, con, transaction))
            {
                try
                {
                    command.Parameters.AddWithValue("@SSN", SSN);
                    var affectedRows = await command.ExecuteNonQueryAsync();

                    if (affectedRows > 0)
                    {
                        transaction.Commit();
                        await Console.Out.WriteLineAsync($"Info: Member with SSN {SSN} was successfully deleted.");
                    }
                    else
                    {
                        transaction.Rollback();
                        await Console.Out.WriteLineAsync($"Warning: No Member found with SSN: {SSN} - no changes were made.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    await Console.Out.WriteLineAsync($"Error: Failed to delete Member with SSN: {SSN}. Error: {ex.Message}");
                }
            }
        }

        public async Task<Member> GetMember(int SSN)
        {
            try
            {
                using var con = _connectionFactory.CreateConnection();
                await con.OpenAsync();
                var sql = @"SELECT * FROM Member WHERE SSN = @SSN";
                using (SqlCommand command = new SqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@SSN", SSN);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var member = new Member
                            {
                                SSN = reader.GetInt32(reader.GetOrdinal("SSN")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                Street = reader.GetString(reader.GetOrdinal("Street")),
                                StreetNumber = reader.GetString(reader.GetOrdinal("StreetNumber")),
                                City = reader.GetString(reader.GetOrdinal("City")),
                                Zipcode = reader.GetString(reader.GetOrdinal("Zipcode")),
                                CardNumber = reader.GetString(reader.GetOrdinal("CardNumber")),
                                ExpiryDate = reader.GetString(reader.GetOrdinal("ExpiryDate")),
                                Photo = reader.GetString(reader.GetOrdinal("Photo")),
                                Type = reader.GetString(reader.GetOrdinal("Type")),
                        };
                            await Console.Out.WriteLineAsync($"Info: Member with SSN {SSN} was found.");
                            return member;
                        }
                        else
                        {
                            await Console.Out.WriteLineAsync($"Warning: Member with SSN {SSN} was not found.");
                            return null;
                        }
                    }
                };
            }
            catch(Exception ex) {
                throw new Exception("Error retrieving data", ex);
            }

        }

        public async Task<List<Member>> ListMembers()
        {
            using var con = _connectionFactory.CreateConnection();
            await con.OpenAsync();
            var sql = @"SELECT * FROM Member";

            using (SqlCommand command = new SqlCommand(sql, con))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var members = new List<Member>();

                    while (await reader.ReadAsync())
                    {
                        var member = new Member
                        {
                            SSN = reader.GetInt32(reader.GetOrdinal("SSN")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                            Street = reader.GetString(reader.GetOrdinal("Street")),
                            StreetNumber = reader.GetString(reader.GetOrdinal("StreetNumber")),
                            City = reader.GetString(reader.GetOrdinal("City")),
                            Zipcode = reader.GetString(reader.GetOrdinal("Zipcode")),
                            CardNumber = reader.GetString(reader.GetOrdinal("CardNumber")),
                            ExpiryDate = reader.GetString(reader.GetOrdinal("ExpiryDate")),
                            Photo = reader.GetString(reader.GetOrdinal("Photo")),
                            Type = reader.GetString(reader.GetOrdinal("Type")),
                        };

                        members.Add(member);
                    }

                    if (members.Count > 0)
                    {
                        await Console.Out.WriteLineAsync($"Info: Found {members.Count} members.");
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync("Warning: No members were found.");
                    }

                    return members;
                }
            }
        }

        public async Task UpdateMember(Member member)
        {
            using var con = _connectionFactory.CreateConnection();
            await con.OpenAsync();

            using var transaction = con.BeginTransaction();
            var sql = @"UPDATE Member SET @FirstName, @LastName, @Street, @StreetNumber, @City, @Zipcode, @CardNumber, @ExpiryDate, @Photo, @Type WHERE SSN = @SSN";

            using (SqlCommand command = new SqlCommand(sql, con, transaction))
            {
                try
                {
                    // Prevention against SQL injection
                    command.Parameters.AddWithValue("@SSN", member.SSN);
                    command.Parameters.AddWithValue("@FirstName", member.FirstName);
                    command.Parameters.AddWithValue("@LastName", member.LastName);
                    command.Parameters.AddWithValue("@PhoneNumber", member.StreetNumber);
                    command.Parameters.AddWithValue("@Street", member.Street);
                    command.Parameters.AddWithValue("@StreetNumber", member.StreetNumber);
                    command.Parameters.AddWithValue("@City", member.City);
                    command.Parameters.AddWithValue("@Zipcode", member.Zipcode);
                    command.Parameters.AddWithValue("@CardNumber", member.CardNumber);
                    command.Parameters.AddWithValue("@ExpiryDate", member.ExpiryDate);
                    command.Parameters.AddWithValue("@Photo", member.Photo);
                    command.Parameters.AddWithValue("@Type", member.Type);

                    var affectedRows = await command.ExecuteNonQueryAsync();

                    if (affectedRows > 0)
                    {
                        transaction.Commit();
                        await Console.Out.WriteLineAsync($"Info: Member with SSN: {member.SSN} was successfully updated.");
                    }
                    else
                    {
                        transaction.Rollback();
                        await Console.Out.WriteLineAsync($"Warning: Member with SSN: {member.SSN} was not found - no changes were made.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    await Console.Out.WriteLineAsync($"Error: Failed to update Member with SSN {member.SSN}. Error: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
