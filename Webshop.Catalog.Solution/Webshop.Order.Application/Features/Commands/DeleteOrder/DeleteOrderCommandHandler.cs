using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Order.Application.Contracts.Persistence;

namespace Webshop.Order.Application.Features.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderCommand>
    {
        private ILogger<DeleteOrderCommand> logger;
        private IOrderRepository repository;
        public DeleteOrderCommandHandler(ILogger<DeleteOrderCommand> logger, IOrderRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        public async Task<Result> Handle(DeleteOrderCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await repository.DeleteAsync(command.OrderNumber);
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
