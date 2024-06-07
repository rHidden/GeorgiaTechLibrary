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

namespace Webshop.User.Application.Features.UpdateUser
{
    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
    {
        private ILogger<UpdateUserCommandHandler> logger;
        private IUserRepository userRepository;
        public UpdateUserCommandHandler(ILogger<UpdateUserCommandHandler> logger, IUserRepository userRepository)
        {
            this.logger = logger;
            this.userRepository = userRepository;
        }

        public async Task<Result> Handle(UpdateUserCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await userRepository.UpdateAsync(command.User);
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
