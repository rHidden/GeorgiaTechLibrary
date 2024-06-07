using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.User.Application.Contracts.Persistence;
using Webshop.Domain.Common;

namespace Webshop.User.Application.Features.DeleteUser
{
    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
    {
        private ILogger<DeleteUserCommandHandler> logger;
        private IUserRepository userRepository;
        public DeleteUserCommandHandler(ILogger<DeleteUserCommandHandler> logger, IUserRepository userRepository)
        {
            this.logger = logger;
            this.userRepository = userRepository;
        }

        public async Task<Result> Handle(DeleteUserCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await userRepository.DeleteAsync(command.UserId);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return Result.Fail(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
