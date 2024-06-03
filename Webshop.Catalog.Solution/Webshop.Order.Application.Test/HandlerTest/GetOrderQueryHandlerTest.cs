using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Webshop.Domain.Common;
using Webshop.Order.Application.Contracts.Persistence;
using Webshop.Order.Application.Features.Dtos;
using Webshop.Order.Application.Features.Queries.GetOrder;

namespace Webshop.Order.Application.Test.HandlerTest
{
    public class GetOrderQueryHandlerTest
    {
        [Test]
        public async Task Handle_ValidQuery_ShouldReturnOrderDto()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<GetOrderQuery>>();
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IOrderRepository>();

            var handler = new GetOrderQueryHandler(loggerMock.Object, mapperMock.Object, repositoryMock.Object);

            var query = new GetOrderQuery(orderNumber: 123);

            var order = new Domain.AggregateRoots.Order(1) { Id = 123, Discount = 10, TotalPrice = 100 };

            mapperMock.Setup(mapper => mapper.Map<OrderDto>(order)).Returns(new OrderDto { Id = 123 });

            repositoryMock.Setup(repo => repo.GetById(query.OrderNumber)).ReturnsAsync(order);

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(123, result.Value.Id);
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ),
                Times.Never // Ensure that logging is not called for successful execution
            );
        }

        [Test]
        public async Task Handle_InvalidQuery_ShouldFail()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<GetOrderQuery>>();
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IOrderRepository>();

            var handler = new GetOrderQueryHandler(loggerMock.Object, mapperMock.Object, repositoryMock.Object);

            var query = new GetOrderQuery(orderNumber: 123);

            // Simulate repository throwing an exception
            repositoryMock.Setup(repo => repo.GetById(query.OrderNumber))
                          .ThrowsAsync(new Exception("Unable to retrieve order"));

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.IsFalse(result.Success);

            // Verify that accessing Value property on a failed result throws InvalidOperationException
            Assert.Throws<InvalidOperationException>(() =>
            {
                var orderDto = result.Value;
            });
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
