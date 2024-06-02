using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Contracts.Persistence;
using Webshop.Catalog.Application.Features.Product.Dtos;
using Webshop.Domain.Common;

namespace Webshop.Catalog.Application.Features.Product.Queries.GetProductsOfSeller
{
    public class GetProductsOfSellerQueryHandler : IQueryHandler<GetProductsOfSellerQuery, List<ProductDto>>
    {
        private ILogger<GetProductsOfSellerQueryHandler> logger;
        private IMapper mapper;
        private IProductRepository productRepository;
        public GetProductsOfSellerQueryHandler(ILogger<GetProductsOfSellerQueryHandler> logger, IMapper mapper, IProductRepository productRepository) 
        {
            this.logger = logger;
            this.mapper = mapper;
            this.productRepository = productRepository;
        }

        public async Task<Result<List<ProductDto>>> Handle(GetProductsOfSellerQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var queryResult = await productRepository.GetBySellerId(query.SellerId);
                List<ProductDto> result = new List<ProductDto>();
                foreach (var element in queryResult)
                {
                    result.Add(mapper.Map<ProductDto>(element));
                }
                return Result.Ok<List<ProductDto>>(result);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return Result.Fail<List<ProductDto>>(Errors.General.UnspecifiedError(ex.Message));
            }

        }
    }
}
