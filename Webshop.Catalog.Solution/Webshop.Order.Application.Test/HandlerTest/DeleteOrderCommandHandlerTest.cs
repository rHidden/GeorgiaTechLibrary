using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Webshop.Domain.Common;
using Webshop.Order.Application.Features.Commands.DeleteOrder;
using Webshop.Order.Application.Contracts.Persistence;

namespace Webshop.Order.Application.Test.HandlerTest
{
    public class DeleteOrderCommandHandlerTest
    {
        [Test]
        public async Task Handle_ValidCommand_ShouldSucceed()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<DeleteOrderCommand>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();

            var handler = new DeleteOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            var command = new DeleteOrderCommand(orderNumber: 123);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.IsTrue(result.Success);
            orderRepositoryMock.Verify(repo => repo.DeleteAsync(command.OrderNumber), Times.Once);
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
        public async Task Handle_InvalidCommand_ShouldFail()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<DeleteOrderCommand>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();

            var handler = new DeleteOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            var command = new DeleteOrderCommand(orderNumber: 123);

            // Simulate repository throwing an exception
            orderRepositoryMock.Setup(repo => repo.DeleteAsync(command.OrderNumber))
                               .ThrowsAsync(new Exception("Unable to delete order"));

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.IsFalse(result.Success);
            orderRepositoryMock.Verify(repo => repo.DeleteAsync(command.OrderNumber), Times.Once);
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
