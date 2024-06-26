﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Webshop.Customer.Application.Contracts.Persistence;
using Webshop.Data.Persistence;
using Webshop.Domain.AggregateRoots;

namespace Webshop.Customer.Persistence
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public CustomerRepository(DataContext context) : base(TableNames.Customer.CUSTOMERTABLE, context) { }
        public async Task CreateAsync(Domain.AggregateRoots.Customer entity)
        {
            using(var connection = dataContext.CreateConnection())
            {
                string command = $"insert into {TableName} (Name, Address, Address2, City, Region, PostalCode, Country, Email, BuyerFlag, SellerFlag) values (@name, @address, @address2, @city, @region, @postalcode, @country, @email, 0, 0)";
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

        public async Task<IEnumerable<Domain.AggregateRoots.Customer>> GetAll()
        {
            using(var connection = dataContext.CreateConnection())
            {
                string query = $"select * from {TableName}";
                return await connection.QueryAsync<Domain.AggregateRoots.Customer>(query);
            }
        }

        public async Task<Domain.AggregateRoots.Customer> GetById(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select * from {TableName} where id = @id";
                return await connection.QuerySingleAsync<Domain.AggregateRoots.Customer>(query, new {id = id});
            }
        }

        public async Task UpdateAsync(Domain.AggregateRoots.Customer entity)
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

        public async Task<Domain.AggregateRoots.Seller> GetSellerById(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select * from {TableName} where id = @id and SellerFlag = 1";
                return await connection.QuerySingleAsync<Domain.AggregateRoots.Seller>(query, new { id = id });
            }
        }

        public async Task RegisterSeller(int id, double review)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"update {TableName} set SellerFlag = 1, SellerReview = @review where id = @id";
                await connection.ExecuteAsync(query, new { review = review, id = id });
            }
        }


        public async Task UpdateSeller(int id, double review)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"update {TableName} set SellerReview = @review where id = @id";
                await connection.ExecuteAsync(query, new { review = review, id = id });
            }
        }


        public async Task<Domain.AggregateRoots.Buyer> GetBuyerById(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select * from {TableName} where id = @id and BuyerFlag = 1";
                return await connection.QuerySingleAsync<Domain.AggregateRoots.Buyer>(query, new { id = id });
            }
        }

        public async Task RegisterBuyer(int id, string descrtiption)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"update {TableName} set BuyerFlag = 1, BuyerDescription = @description where id = @id";
                await connection.ExecuteAsync(query, new { descrtiption = descrtiption, id = id });
            }
        }


        public async Task UpdateBuyer(int id, string description)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"update {TableName} set BuyerDescription = @description where id = @id";
                await connection.ExecuteAsync(query, new { description = description, id = id });
            }
        }
    }
}
