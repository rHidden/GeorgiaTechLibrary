using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Webshop.User.Application.Contracts.Persistence;
using Webshop.Data.Persistence;
using Webshop.Domain.Common;

namespace Webshop.User.Persistence
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(DataContext context) : base(TableNames.User.USERTABLE, context) { }
        public async Task CreateAsync(Domain.AggregateRoots.User entity)
        {
            using(var connection = dataContext.CreateConnection())
            {
                string command = $"insert into {TableName} (Name, Address, Address2, City, Region, PostalCode, Country, Email) values (@name, @address, @address2, @city, @region, @postalcode, @country, @email)";
                await connection.ExecuteAsync(command, new { name = entity.Name, address = entity.Address, address2 = entity.Address2, city = entity.City, region = entity.Region, postalcode = entity.PostalCode, country = entity.Country, email = entity.Email });
            }
        }

        public async Task DeleteAsync(int id)
        {
            using(var connection = dataContext.CreateConnection())
            {
                string command = $"delete from {TableName} where id = @id";
                await connection.ExecuteAsync(command, new {id = id});
            }
        }

        public async Task<IEnumerable<Domain.AggregateRoots.User>> GetAll()
        {
            using(var connection = dataContext.CreateConnection())
            {
                string query = $"select * from {TableName}";
                return await connection.QueryAsync<Domain.AggregateRoots.User>(query);
            }
        }

        public async Task<Domain.AggregateRoots.User> GetById(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select * from {TableName} where id = @id";
                return await connection.QuerySingleAsync<Domain.AggregateRoots.User>(query, new {id = id});
            }
        }

        public async Task UpdateAsync(Domain.AggregateRoots.User entity)
        {
           using(var connection = dataContext.CreateConnection())
            {
                string command = $"update {TableName} set Name = @name, Address = @address, Address2 = @address2, City = @city, Region = @region, PostalCode = @postalcode, Country = @country, Email = @email where Id = @id";
                await connection.ExecuteAsync(command, new {
                    id = entity.Id, 
                    name = entity.Name, 
                    address = entity.Address, 
                    address2 = entity.Address2, 
                    city = entity.City, 
                    region = entity.Region,
                    postalcode = entity.PostalCode,
                    country = entity.Country,
                    email = entity.Email
                });
            }
        }
    }
}
