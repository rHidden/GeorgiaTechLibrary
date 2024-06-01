using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Contracts.Persistence;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Domain.Common;
using Webshop.Order.Application.Contracts.Persistence;
using Webshop.Order.Domain.AggregateRoots;

namespace Webshop.Order.Application.Features.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : ICommandHandler<UpdateOrderCommand>
    {
        private ILogger<UpdateOrderCommand> logger;
        private IOrderRepository repository;
        private IProductRepository productRepository;

        public UpdateOrderCommandHandler(ILogger<UpdateOrderCommand> logger, IOrderRepository repository,
            IProductRepository productRepository)
        {
            this.logger = logger;
            this.repository = repository;
            this.productRepository = productRepository;
        }

        public async Task<Result> Handle(UpdateOrderCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                Domain.AggregateRoots.Order order = new Domain.AggregateRoots.Order(command.CustomerId);
                order.Discount = command.Discount;
                foreach (var orderLine in command.OrderLines)
                {
                    Product product = await productRepository.GetById(orderLine.ProductId);
                    order.OrderLines.Append(new OrderLine(product, orderLine.Quantity));
                }
                order.TotalPrice = order.OrderLines.Sum(x => x.SubTotal);
                await repository.UpdateAsync(order);
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
