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

namespace Webshop.User.Application.Features.CreateUser
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private ILogger<CreateUserCommandHandler> logger;
        private IUserRepository userRepository;
        public CreateUserCommandHandler(ILogger<CreateUserCommandHandler> logger, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<Result> Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await userRepository.CreateAsync(command.User);
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
