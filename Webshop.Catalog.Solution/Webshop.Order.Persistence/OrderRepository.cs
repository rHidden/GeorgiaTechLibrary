using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Data.Persistence;
using Webshop.Order.Application.Contracts.Persistence;

namespace Webshop.Order.Persistence
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        public OrderRepository(DataContext context) : base(TableNames.Order.ORDERTABLE, context) { }

        public async Task CreateAsync(Domain.AggregateRoots.Order entity)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string orderCommand = $"insert into {TableName} (TotalPrice, Discount, Date, CustomerId) values " +
                    $"(@totalPrice, @discount, @date, @customerId);" +
                    $"SELECT CAST(SCOPE_IDENTITY() as int);";
                string orderLineCommand = $"INSERT INTO OrderLines (ProductId, Quantity, SubTotal, OrderId) " +
                    $"VALUES (@productId, @quantity, @subTotal, @OrderId);";
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var orderId = await connection.ExecuteAsync(orderCommand, entity);

                    foreach (var orderLine in entity.OrderLines)
                    {
                        orderLine.OrderId = orderId;
                        await connection.ExecuteAsync(orderLineCommand, orderLine);
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
                string query = $"select o.Id, o.TotalPrice, o.Discount, o.Date, o.CustomerId " +
                    $"ol.Id, ol.Quantity, ol.SubTotal, ol.ProductId from {TableName} o " +
                    $"inner join {TableNames.Order.ORDERLINETABLE} ol on ol.OrderId = o.Id";

                var orderDictionary = new Dictionary<int, Domain.AggregateRoots.Order>();

                return (await connection.QueryAsync<Domain.AggregateRoots.Order, 
                    Domain.AggregateRoots.OrderLine, Domain.AggregateRoots.Order>(query, map: (order, orderLine) =>
                    {
                        if (!orderDictionary.TryGetValue(order.Id, out var currentOrder))
                        {
                            currentOrder = order;
                            orderDictionary.Add(currentOrder.Id, currentOrder);
                        }

                        currentOrder.OrderLines.Append(orderLine);
                        return currentOrder;
                    }, splitOn: "Id")).Distinct();
            }
        }

        public async Task<Domain.AggregateRoots.Order> GetById(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select o.Id, o.TotalPrice, o.Discount, o.Date, o.CustomerId " +
                    $"ol.Id, ol.Quantity, ol.SubTotal, ol.ProductId from {TableName} o " +
                    $"inner join {TableNames.Order.ORDERLINETABLE} ol on ol.OrderId = o.Id " +
                    $"where id = @Id";
                var orderDictionary = new Dictionary<int, Domain.AggregateRoots.Order>();

                return (await connection.QueryAsync<Domain.AggregateRoots.Order, 
                    Domain.AggregateRoots.OrderLine, Domain.AggregateRoots.Order>(query, map: (order, orderLine) =>
                    {
                        if (!orderDictionary.TryGetValue(order.Id, out var currentOrder))
                        {
                            currentOrder = order;
                            orderDictionary.Add(currentOrder.Id, currentOrder);
                        }

                        currentOrder.OrderLines.Append(orderLine);
                        return currentOrder;
                    }, new { Id = id })).First(); //TODO maybe Single
            }
        }

        public async Task UpdateAsync(Domain.AggregateRoots.Order entity)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $"update {TableName} set TotalPrice = @TotalPrice, Discount =  @discount, CustomerId = @customerId";
                string deleteOrderLinesSql = $"DELETE FROM OrderLines WHERE OrderId = @OrderId";
                string insertOrderLineSql = $"INSERT INTO OrderLines (ProductId, Quantity, SubTotal, OrderId) " +
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
