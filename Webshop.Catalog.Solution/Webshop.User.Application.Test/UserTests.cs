using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Webshop.User.Application.Contracts.Persistence;
using Webshop.User.Application.Features.CreateUser;
using Webshop.User.Application.Features.DeleteUser;
using Webshop.User.Application.Features.UpdateUser;
using Webshop.Domain.Common;
using Xunit;

namespace Webshop.User.Application.Test
{
    public class UserTests
    {
        [Fact]
        public void CreateUserCommand_InValid_ExpectFailure()
        {
            Action a = () => new CreateUserCommand(null);
            Assert.Throws<ArgumentNullException>(a);         
        }

        [Fact]
        public void UpdateUserCommand_InValid_ExpectFailure()
        {
            Action a = () => new UpdateUserCommand(null);
            Assert.Throws<ArgumentNullException>(a);
        }

        [Fact]
        public void DeleteUserCommand_InValid_ExpectFailure()
        {
            Action a = () => new DeleteUserCommand(0);
            Assert.Throws<ArgumentException>(a);
        }

        [Fact]
        public void CreateUserCommand_Valid_ExpectSuccess() 
        {
            //for the sake of completeness
            CreateUserCommand command = new CreateUserCommand(new Domain.AggregateRoots.User("Centisoft"));
        }

        [Fact]
        public async Task CreateUserCommandHandler_Invoke_repositorycreate_expect_success() 
        {
            //arrange
            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<CreateUserCommandHandler>>();            
            var userRepositoryMock = new Mock<IUserRepository>();            
            Domain.AggregateRoots.User user = new Domain.AggregateRoots.User("Centisoft");            
            CreateUserCommand command = new CreateUserCommand(user);
            CreateUserCommandHandler handler = new CreateUserCommandHandler(loggerMock.Object, userRepositoryMock.Object);
            //act
            Result result = await handler.Handle(command);
            //assert
            userRepositoryMock.Verify((m => m.CreateAsync(user)), Times.Once);            
            Assert.True(result.Success);
        }

        [Fact]
        public async Task UpdateUserCommandHandler_Invoke_repositoryupdate_expect_success()
        {
            //arrange
            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<UpdateUserCommandHandler>>();
            var userRepositoryMock = new Mock<IUserRepository>();
            Domain.AggregateRoots.User user = new Domain.AggregateRoots.User("Centisoft") { Id = 1};
            UpdateUserCommand command = new UpdateUserCommand(user);
            UpdateUserCommandHandler handler = new UpdateUserCommandHandler(loggerMock.Object, userRepositoryMock.Object);
            //act
            Result result = await handler.Handle(command);
            //assert
            userRepositoryMock.Verify((m => m.UpdateAsync(user)), Times.Once);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteUserCommandHandler_Invoke_repositorydelete_expect_success()
        {
            //arrange
            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<DeleteUserCommandHandler>>();
            var userRepositoryMock = new Mock<IUserRepository>();
            DeleteUserCommand command = new DeleteUserCommand(1);
            DeleteUserCommandHandler handler = new DeleteUserCommandHandler(loggerMock.Object, userRepositoryMock.Object);
            //act
            Result result = await handler.Handle(command);
            //assert
            userRepositoryMock.Verify((m => m.DeleteAsync(1)), Times.Once);
            Assert.True(result.Success);
        }
    }
}
