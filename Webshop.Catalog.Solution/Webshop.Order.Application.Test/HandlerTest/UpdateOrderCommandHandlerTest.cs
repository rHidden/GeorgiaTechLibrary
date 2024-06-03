using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Contracts.Persistence;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Domain.AggregateRoots;
using Webshop.Order.Application.Contracts.Persistence;
using Webshop.Order.Application.Features.Commands.UpdateOrder;
using Webshop.Order.Application.Features.Requests;
using Webshop.Order.Domain.AggregateRoots;

namespace Webshop.Order.Application.Test.HandlerTest
{
    public class UpdateOrderCommandHandlerTest
    {
        [Test]
        public async Task Handle_InvalidCommand_InvalidDiscount_ShouldFail()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<UpdateOrderCommand>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();

            var handler = new UpdateOrderCommandHandler(
                loggerMock.Object,
                orderRepositoryMock.Object,
                productRepositoryMock.Object);

            var command = new UpdateOrderCommand(
                id: 1,
                customerId: 1,
                discount: 20, // Discount out of range (not between 0 and 15)
                orderLines: new List<UpdateOrderLineRequest>
                {
                    new UpdateOrderLineRequest { ProductId = 1, Quantity = 2 }
                });

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.IsFalse(result.Success);
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

        [Test]
        public async Task Handle_InvalidCommand_NoProductFound_ShouldFail()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<UpdateOrderCommand>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();

            var handler = new UpdateOrderCommandHandler(
                loggerMock.Object,
                orderRepositoryMock.Object,
                productRepositoryMock.Object);

            var command = new UpdateOrderCommand(
                id: 1,
                customerId: 1,
                discount: 5,
                orderLines: new List<UpdateOrderLineRequest>
                {
                    new UpdateOrderLineRequest { ProductId = 1, Quantity = 2 }
                });

            // Simulate product not found
            productRepositoryMock.Setup(repo => repo.GetById(1)).ReturnsAsync((Product)null);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.IsFalse(result.Success);
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

        [Test]
        public async Task Handle_ValidCommand_ShouldSucceed()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<UpdateOrderCommand>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();

            var handler = new UpdateOrderCommandHandler(
                loggerMock.Object,
                orderRepositoryMock.Object,
                productRepositoryMock.Object);

            var command = new UpdateOrderCommand(
                id: 1,
                customerId: 1,
                discount: 5,
                orderLines: new List<UpdateOrderLineRequest>
                {
            new UpdateOrderLineRequest { ProductId = 1, Quantity = 2 }
                });

            var product1 = new Product { Id = 1, AmountInStock = 5 }; // Sufficient stock
            productRepositoryMock.Setup(repo => repo.GetById(1)).ReturnsAsync(product1);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.IsTrue(result.Success);
            loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ),
                Times.Never // Ensure that logging is not called for successful execution
            );
        }

        [Test]
        public async Task Handle_ValidCommand_CorrectTotalPrice()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<UpdateOrderCommand>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();

            var handler = new UpdateOrderCommandHandler(
                loggerMock.Object,
                orderRepositoryMock.Object,
                productRepositoryMock.Object);

            var command = new UpdateOrderCommand(
                id: 1,
                customerId: 1,
                discount: 5,
                orderLines: new List<UpdateOrderLineRequest>
                {
            new UpdateOrderLineRequest { ProductId = 1, Quantity = 2 },
            new UpdateOrderLineRequest { ProductId = 2, Quantity = 3 }
                });

            var product1 = new Product { Id = 1, Price = 10, AmountInStock = 5 };
            var product2 = new Product { Id = 2, Price = 20, AmountInStock = 10 };
            productRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                                .ReturnsAsync((int id) => id == 1 ? product1 : product2);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.IsTrue(result.Success);
            orderRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.AggregateRoots.Order>()), Times.Once);

            // Check if the total price is correctly calculated
            orderRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Domain.AggregateRoots.Order>(order =>
                order.TotalPrice == (2 * product1.Price + 3 * product2.Price) * (1 - command.Discount / 100.0)
            )), Times.Once);
        }

    }
}
