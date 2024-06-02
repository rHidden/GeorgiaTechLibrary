using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Webshop.Domain.Common;
using Webshop.Order.Application.Contracts.Persistence;
using Webshop.Order.Application.Features.Dtos;
using Webshop.Order.Application.Features.Queries.GetOrders;

namespace Webshop.Order.Application.Test.QueryHandlers
{
    public class GetOrdersQueryHandlerTests
    {
        [Test]
        public async Task Handle_ValidQuery_ShouldReturnOrderDtoCollection()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<GetOrdersQueryHandler>>();
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IOrderRepository>();

            var handler = new GetOrdersQueryHandler(loggerMock.Object, mapperMock.Object, repositoryMock.Object);

            var query = new GetOrdersQuery();

            var orders = new List<Domain.AggregateRoots.Order>
            {
                new Domain.AggregateRoots.Order(1) { Id = 1, Discount = 10, TotalPrice = 100 },
                new Domain.AggregateRoots.Order(2) { Id = 2, Discount = 15, TotalPrice = 150 }
            };

            mapperMock.Setup(mapper => mapper.Map<IEnumerable<OrderDto>>(orders))
                      .Returns(new List<OrderDto>
                      {
                          new OrderDto { Id = 1 },
                          new OrderDto { Id = 2 }
                      });

            repositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(orders);

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<IEnumerable<OrderDto>>(result.Value);
            Assert.AreEqual(2, ((List<OrderDto>)result.Value).Count);

            // Verify that accessing Value property on a successful result does not throw an exception
            Assert.DoesNotThrow(() =>
            {
                var orderDtos = result.Value;
            });
        }

        [Test]
        public async Task Handle_InvalidQuery_ShouldFail()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<GetOrdersQueryHandler>>();
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IOrderRepository>();

            var handler = new GetOrdersQueryHandler(loggerMock.Object, mapperMock.Object, repositoryMock.Object);

            var query = new GetOrdersQuery();

            // Simulate repository throwing an exception
            repositoryMock.Setup(repo => repo.GetAll())
                          .ThrowsAsync(new Exception("Unable to retrieve orders"));

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.IsFalse(result.Success);

            // Verify that accessing Value property on a failed result throws InvalidOperationException
            Assert.Throws<InvalidOperationException>(() =>
            {
                var orderDtos = result.Value;
            });

            // Verify that logging is called with the correct exception message
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ),
                Times.Once
            );
        }

    }
}
