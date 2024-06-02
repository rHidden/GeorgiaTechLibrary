using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Contracts.Persistence;
using Webshop.Catalog.Application.Features.Product.Queries.GetProduct;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Category.Application.Features.Category.Dtos;
using Webshop.Domain.Common;
using Webshop.Order.Application.Contracts.Persistence;
using Webshop.Order.Domain.AggregateRoots;

namespace Webshop.Order.Application.Features.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
    {
        private ILogger<CreateOrderCommand> logger;
        private IOrderRepository orderRepository;
        private IProductRepository productRepository;

        public CreateOrderCommandHandler(ILogger<CreateOrderCommand> logger, IOrderRepository orderRepository,
            IProductRepository productRepository)
        {
            this.logger = logger;
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
        }

        public async Task<Result> Handle(CreateOrderCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                Domain.AggregateRoots.Order newOrder = new Domain.AggregateRoots.Order(command.UserId);
                newOrder.Discount = command.Discount;
                foreach (var orderLine in command.OrderLines)
                {
                    Product product = await productRepository.GetById(orderLine.ProductId);
                    newOrder.OrderLines.Append(new OrderLine(product, orderLine.Quantity));
                }
                newOrder.TotalPrice = newOrder.OrderLines.Sum(x => x.SubTotal);
                await orderRepository.CreateAsync(newOrder);
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
