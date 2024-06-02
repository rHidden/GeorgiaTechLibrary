using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Catalog.Application.Contracts.Persistence;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Data.Persistence;
using Webshop.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Catalog.Persistence
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(DataContext context) : base(TableNames.Catalog.PRODUCTTABLE, context) { }

        public async Task<Result> AddProductToCategory(int productId, int categoryId)
        {
            using(var connection = dataContext.CreateConnection())
            {
                string command = $"insert into {TableNames.Catalog.PRODUCTCATEGORYTABLE} (productId, categoryId) values (@pid, @cid)";
                await connection.ExecuteAsync(command, new {pid = productId, cid = categoryId});
                return Result.Ok();
            }
        }

        public async Task CreateAsync(Product entity)
        {
            using(var connection  = dataContext.CreateConnection())
            {
                string command = $"insert into {TableName} (Name, SKU, Price, Currency, Description, AmountInStock, MinStock, SellerId) values (@name, @sku, @price, @currency, @description, @stock, @minstock, @sellerId)";
                await connection.ExecuteAsync(command, new
                {
                    name = entity.Name,
                    sku = entity.SKU,
                    price = entity.Price,
                    currency = entity.Currency,
                    description = entity.Description,
                    stock = entity.AmountInStock,
                    minstock = entity.MinStock,
                    sellerId = entity.Seller.Id
                });

            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $"delete from {TableName} where id =  @id";
                await connection.ExecuteAsync(command, new { id = id });
            }
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select p.Id, p.Name, p.SKU, p.Price, p.Currency, p.Description, p.AmountInStock, p.MinStock, c.Id, c.Name, c.Address, c.Address2, c.City, c.Region, c.PostalCode, c.Country, c.Email, c.SellerFlag, c.SellerReview from {TableName} p join {TableNames.Customer.CUSTOMERTABLE} c on c.Id = p.SellerId";
                return await connection.QueryAsync<Product, Seller, Product>(query, map: (product, seller) =>
                {
                    product.Seller = seller;
                    return product;
                }, splitOn: "c.Id");
            }
        }

        public async Task<IEnumerable<Product>> GetAllFromCategory(int categoryId)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select p.Id, p.Name, p.SKU, p.Price, p.Currency, p.Description, p.AmountInStock, p.MinStock, c.Id, c.Name, c.Address, c.Address2, c.City, c.Region, c.PostalCode, c.Country, c.Email, c.SellerFlag, c.SellerReview from {TableName} p join {TableNames.Catalog.PRODUCTCATEGORYTABLE} pc on p.Id = pc.ProductId join {TableNames.Customer.CUSTOMERTABLE} c on c.Id = p.SellerId where b.CategoryId = @categoryid";
                return await connection.QueryAsync<Product, Seller, Product>(query, map: (product, seller) =>
                {
                    product.Seller = seller;
                    return product;
                },
                new {categoryid = categoryId},
                splitOn: "c.Id");
            }
        }

        public async Task<Product> GetById(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select p.Id, p.Name, p.SKU, p.Price, p.Currency, p.Description, p.AmountInStock, p.MinStock, c.Id, c.Name, c.Address, c.Address2, c.City, c.Region, c.PostalCode, c.Country, c.Email, c.SellerReview from {TableName} p join {TableNames.Customer.CUSTOMERTABLE} c on c.Id = p.SellerId where p.id = @id";
                return (await connection.QueryAsync<Product, Seller, Product>(query, map: (product, seller) =>
                {
                    product.Seller = seller;
                    return product;
                },
                new { id = id },
                splitOn: "Id")).Single();
            }
        }

        public async Task<Result> RemoveProductFromCategory(int productId, int categoryId)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $"delete from {TableNames.Catalog.PRODUCTCATEGORYTABLE} where productId = @pid and categoryId = @cid";
                await connection.ExecuteAsync(command, new { pid = productId, cid = categoryId });
                return Result.Ok();
            }
        }

        public async Task UpdateAsync(Product entity)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $"update {TableName} set name = @name, description =  @desc, currency = @curr, price = @price, AmountInStock = @amount, MinStock = @min where id = @id";
                await connection.ExecuteAsync(command, new { 
                    name = entity.Name, 
                    desc = entity.Description, 
                    curr = entity.Currency, 
                    price = entity.Price, 
                    amount = entity.AmountInStock, 
                    min = entity.MinStock, 
                    id = entity.Id 
                });

            }
        }
    }
}
