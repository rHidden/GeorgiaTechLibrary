using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Contracts.Persistence;
using Webshop.Catalog.Application.Features.Product.Queries.GetProduct;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Category.Application.Features.Category.Dtos;
using Webshop.Customer.Application.Contracts.Persistence;
using Webshop.Domain.AggregateRoots;
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
        private ICustomerRepository customerRepository;

        public CreateOrderCommandHandler(ILogger<CreateOrderCommand> logger, IOrderRepository orderRepository,
            IProductRepository productRepository, ICustomerRepository customerRepository)
        {
            this.logger = logger;
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
            this.customerRepository = customerRepository;
        }

        public async Task<Result> Handle(CreateOrderCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    Domain.AggregateRoots.Order newOrder = new Domain.AggregateRoots.Order(command.CustomerId);
                    newOrder.Discount = command.Discount;
                    Buyer buyer = await customerRepository.GetBuyerById(command.CustomerId);
                    if (buyer == null)
                    {
                        throw new Exception("Customer is not a buyer.");
                    }
                    List<OrderLine> orderLines = new List<OrderLine>();
                    foreach (var orderLine in command.OrderLines)
                    {
                        Product product = await productRepository.GetById(orderLine.ProductId);
                        orderLine.Quantity = orderLine.Quantity < product.AmountInStock ? orderLine.Quantity : product.AmountInStock;
                        orderLines.Add(new OrderLine(product, orderLine.Quantity));
                        product.AmountInStock -= orderLine.Quantity;
                        await productRepository.UpdateAsync(product);
                    }
                    newOrder.OrderLines = orderLines;
                    newOrder.TotalPrice = newOrder.OrderLines.Sum(x => x.SubTotal) * (1 - newOrder.Discount / 100.0);
                    await orderRepository.CreateAsync(newOrder);
                    scope.Complete();
                    return Result.Ok();
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return Result.Fail(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
