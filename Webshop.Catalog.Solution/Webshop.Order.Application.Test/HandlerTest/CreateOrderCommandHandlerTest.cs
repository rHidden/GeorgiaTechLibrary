using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Contracts.Persistence;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Customer.Application.Contracts.Persistence;
using Webshop.Domain.AggregateRoots;
using Webshop.Domain.Common;
using Webshop.Order.Application.Contracts.Persistence;
using Webshop.Order.Application.Features.Commands.CreateOrder;
using Webshop.Order.Application.Features.Requests;

namespace Webshop.Order.Application.Test.HandlerTest
{
    public class CreateOrderCommandHandlerTest
    {
        [Test]
        public async Task Handle_InvalidCommand_ZeroStock_ShouldFail()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CreateOrderCommand>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();

            var handler = new CreateOrderCommandHandler(
                loggerMock.Object,
                orderRepositoryMock.Object,
                productRepositoryMock.Object,
                customerRepositoryMock.Object);

            var command = new CreateOrderCommand(
                customerId: 1,
                discount: 5,
                orderLines: new List<CreateOrderLineRequest>
                {
                    new CreateOrderLineRequest { ProductId = 1, Quantity = 2 }
                });

            var product1 = new Product { Id = 1, AmountInStock = 0 }; // Zero stock
            productRepositoryMock.Setup(repo => repo.GetById(1)).ReturnsAsync(product1);

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
        public async Task Handle_InvalidCommand_CustomerNotBuyer_ShouldFail()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CreateOrderCommand>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();

            var handler = new CreateOrderCommandHandler(
                loggerMock.Object,
                orderRepositoryMock.Object,
                productRepositoryMock.Object,
                customerRepositoryMock.Object);

            var command = new CreateOrderCommand(
                customerId: 1,
                discount: 5,
                orderLines: new List<CreateOrderLineRequest>
                {
                    new CreateOrderLineRequest { ProductId = 1, Quantity = 2 }
                });

            customerRepositoryMock.Setup(repo => repo.GetBuyerById(1)).ReturnsAsync((Buyer)null);

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
        public async Task Handle_InvalidCommand_InvalidDiscount_ShouldFail()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CreateOrderCommand>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();

            var handler = new CreateOrderCommandHandler(
                loggerMock.Object,
                orderRepositoryMock.Object,
                productRepositoryMock.Object,
                customerRepositoryMock.Object);

            var command = new CreateOrderCommand(
                customerId: 1,
                discount: 20, // Discount out of range (not between 0 and 15)
                orderLines: new List<CreateOrderLineRequest>
                {
                    new CreateOrderLineRequest { ProductId = 1, Quantity = 2 }
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
            var loggerMock = new Mock<ILogger<CreateOrderCommand>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();

            var handler = new CreateOrderCommandHandler(
                loggerMock.Object,
                orderRepositoryMock.Object,
                productRepositoryMock.Object,
                customerRepositoryMock.Object);

            var command = new CreateOrderCommand(
                customerId: 1,
                discount: 5,
                orderLines: new List<CreateOrderLineRequest>
                {
                    new CreateOrderLineRequest { ProductId = 1, Quantity = 2 }
                });

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
        public async Task Handle_InvalidCommand_NoCustomerFound_ShouldFail()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CreateOrderCommand>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();

            var handler = new CreateOrderCommandHandler(
                loggerMock.Object,
                orderRepositoryMock.Object,
                productRepositoryMock.Object,
                customerRepositoryMock.Object);

            var command = new CreateOrderCommand(
                customerId: 1,
                discount: 5,
                orderLines: new List<CreateOrderLineRequest>
                {
                    new CreateOrderLineRequest { ProductId = 1, Quantity = 2 }
                });

            customerRepositoryMock.Setup(repo => repo.GetBuyerById(1)).ReturnsAsync((Buyer)null);

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
        public async Task Handle_ValidCommand_ShouldCreateOrder()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CreateOrderCommand>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();

            var handler = new CreateOrderCommandHandler(
                loggerMock.Object,
                orderRepositoryMock.Object,
                productRepositoryMock.Object,
                customerRepositoryMock.Object);

            var command = new CreateOrderCommand(
                customerId: 1,
                discount: 5,
                orderLines: new List<CreateOrderLineRequest>
                {
                    new CreateOrderLineRequest { ProductId = 1, Quantity = 2 },
                    new CreateOrderLineRequest { ProductId = 2, Quantity = 3 }
                });

            var product1 = new Product { Id = 1, AmountInStock = 10 };
            var product2 = new Product { Id = 2, AmountInStock = 5 };

            productRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync((int id) => id switch
                {
                    1 => product1,
                    2 => product2,
                    _ => null
                });

            var buyer = new Buyer { Id = 1 };
            customerRepositoryMock.Setup(repo => repo.GetBuyerById(1)).ReturnsAsync(buyer);

            orderRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Domain.AggregateRoots.Order>()));

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.IsTrue(result.Success);
            orderRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Domain.AggregateRoots.Order>()), Times.Once);
            productRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Product>()), Times.Exactly(2));
        }

        [Test]
        public async Task Handle_ValidCommand_CorrectTotalPrice()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CreateOrderCommand>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();

            var handler = new CreateOrderCommandHandler(
                loggerMock.Object,
                orderRepositoryMock.Object,
                productRepositoryMock.Object,
                customerRepositoryMock.Object);

            var command = new CreateOrderCommand(
                customerId: 1,
                discount: 5,
                orderLines: new List<CreateOrderLineRequest>
                {
                    new CreateOrderLineRequest { ProductId = 1, Quantity = 2 },
                    new CreateOrderLineRequest { ProductId = 2, Quantity = 3 }
                });

            var product1 = new Product { Id = 1, Price = 10, AmountInStock = 5 };
            var product2 = new Product { Id = 2, Price = 20, AmountInStock = 10 };
            productRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                                .ReturnsAsync((int id) => id == 1 ? product1 : product2);

            var buyer = new Buyer { Id = 1 };
            customerRepositoryMock.Setup(repo => repo.GetBuyerById(1)).ReturnsAsync(buyer);

            orderRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Domain.AggregateRoots.Order>()));
            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.IsTrue(result.Success);
            orderRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Domain.AggregateRoots.Order>()), Times.Once);

            // Check if the total price is correctly calculated
            orderRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<Domain.AggregateRoots.Order>(order =>
                order.TotalPrice == (2 * product1.Price + 3 * product2.Price) * (1 - command.Discount / 100.0)
            )), Times.Once);
        }

    }
}
