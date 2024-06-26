﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Data.Persistence;
using Webshop.Order.Application.Contracts.Persistence;
using Webshop.Order.Domain.AggregateRoots;
using Webshop.Catalog.Domain.AggregateRoots;

namespace Webshop.Order.Persistence
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        public OrderRepository(DataContext context) : base(TableNames.Order.ORDERTABLE, context) { }

        public async Task CreateAsync(Domain.AggregateRoots.Order entity)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string orderCommand = $"insert into {TableName} (TotalPrice, Discount, [Date], CustomerId) values " +
                    $"(@totalPrice, @discount, @date, @customerId);" +
                    $"SELECT CAST(SCOPE_IDENTITY() as int);";
                string orderLineCommand = $"INSERT INTO {TableNames.Order.ORDERLINETABLE} (ProductId, Quantity, SubTotal, OrderId) " +
                    $"VALUES (@productId, @quantity, @subTotal, @OrderId);";
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var orderId = await connection.QuerySingleAsync<int>(orderCommand, entity);

                    foreach (var orderLine in entity.OrderLines)
                    {
                        orderLine.OrderId = orderId;
                        await connection.ExecuteAsync(orderLineCommand, new { ProductId = orderLine.Product.Id, orderLine.Quantity, orderLine.SubTotal, orderLine.OrderId});
                    }
                    transaction.Commit();
                }
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

        public async Task<IEnumerable<Domain.AggregateRoots.Order>> GetAll()
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select o.Id, o.TotalPrice, o.Discount, o.Date, o.CustomerId, " +
                    $"ol.Id, ol.Quantity, ol.SubTotal, p.Id, p.Name, p.SKU, p.Price, p.Currency, p.Description from {TableName} o " +
                    $"join {TableNames.Order.ORDERLINETABLE} ol on ol.OrderId = o.Id " +
                    $"join {TableNames.Catalog.PRODUCTTABLE} p on p.Id = ol.productId";

                var orderDictionary = new Dictionary<int, Domain.AggregateRoots.Order>();

                return (await connection.QueryAsync<Domain.AggregateRoots.Order, 
                    OrderLine, Product, Domain.AggregateRoots.Order>(query, map: (order, orderLine, product) =>
                    {
                        if (!orderDictionary.TryGetValue(order.Id, out var currentOrder))
                        {
                            currentOrder = order;
                            orderDictionary.Add(currentOrder.Id, currentOrder);
                        }
                        orderLine.Product = product;
                        List<OrderLine> orderLines = [.. currentOrder.OrderLines, orderLine];

                        currentOrder.OrderLines = orderLines;
                        return currentOrder;
                    }, splitOn: "Id")).Distinct();
            }
        }

        public async Task<Domain.AggregateRoots.Order> GetById(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select o.Id, o.TotalPrice, o.Discount, o.Date, o.CustomerId, " +
                    $"ol.Id, ol.Quantity, ol.SubTotal, p.Id, p.Name, p.SKU, p.Price, p.Currency, p.Description from {TableName} o " +
                    $"inner join {TableNames.Order.ORDERLINETABLE} ol on ol.OrderId = o.Id " +
                    $"left join {TableNames.Catalog.PRODUCTTABLE} p on p.Id = ol.productId " +
                    $"where o.Id = @Id";
                var orderDictionary = new Dictionary<int, Domain.AggregateRoots.Order>();

                return (await connection.QueryAsync<Domain.AggregateRoots.Order, 
                    OrderLine, Product, Domain.AggregateRoots.Order>(query, map: (order, orderLine, product) =>
                    {
                        if (!orderDictionary.TryGetValue(order.Id, out var currentOrder))
                        {
                            currentOrder = order;
                            orderDictionary.Add(currentOrder.Id, currentOrder);
                        }
                        orderLine.Product = product;
                        List<OrderLine> orderLines = [.. currentOrder.OrderLines, orderLine];

                        currentOrder.OrderLines = orderLines;
                        return currentOrder;
                    }, new { Id = id })).First(); //TODO maybe Single
            }
        }

        public async Task UpdateAsync(Domain.AggregateRoots.Order entity)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $"update {TableName} set TotalPrice = @TotalPrice, Discount =  @discount, CustomerId = @customerId where Id = @id";
                string deleteOrderLinesSql = $"DELETE FROM {TableNames.Order.ORDERLINETABLE} WHERE OrderId = @OrderId";
                string insertOrderLineSql = $"INSERT INTO {TableNames.Order.ORDERLINETABLE} (ProductId, Quantity, SubTotal, OrderId) " +
                    $"VALUES (@ProductId, @Quantity, @SubTotal, @OrderId);";
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync(deleteOrderLinesSql, new { OrderId = entity.Id }, transaction);

                    foreach (var orderLineDto in entity.OrderLines)
                    {
                        await connection.ExecuteAsync(insertOrderLineSql, new
                        {
                            ProductId = orderLineDto.Product.Id,
                            Quantity = orderLineDto.Quantity,
                            SubTotal = orderLineDto.SubTotal,
                            OrderId = entity.Id
                        }, transaction);
                    }

                    await connection.ExecuteAsync(command, entity);

                    transaction.Commit();
                }
            }
        }
    }
}
