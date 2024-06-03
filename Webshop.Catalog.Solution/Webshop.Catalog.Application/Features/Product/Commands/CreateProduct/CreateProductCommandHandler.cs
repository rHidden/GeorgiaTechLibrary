using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Contracts.Persistence;
using Webshop.Customer.Application.Contracts.Persistence;
using Webshop.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Catalog.Application.Features.Product.Commands.CreateProduct
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
    {
        private ILogger<CreateProductCommand> logger;
        private IProductRepository productRepository;
        private ICustomerRepository customerRepository;
        public CreateProductCommandHandler(ILogger<CreateProductCommand> logger, IProductRepository productRepository, 
            ICustomerRepository customerRepository)
        {
            this.logger = logger;
            this.productRepository = productRepository;
            this.customerRepository = customerRepository;
        }

        public async Task<Result> Handle(CreateProductCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                Domain.AggregateRoots.Product newProduct = new Domain.AggregateRoots.Product(command.Name, command.SKU, command.Price, command.Currency);
                Seller seller = await customerRepository.GetSellerById(command.CustomerId);
                newProduct.Seller = seller;
                await this.productRepository.CreateAsync(newProduct);
                return Result.Ok();
            }
            catch(Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
