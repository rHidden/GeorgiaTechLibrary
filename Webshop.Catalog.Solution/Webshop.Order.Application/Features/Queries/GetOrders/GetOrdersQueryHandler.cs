using AutoMapper;
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
using Webshop.Order.Application.Features.Dtos;

namespace Webshop.Order.Application.Features.Queries.GetOrders
{
    public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, IEnumerable<OrderDto>>
    {
        private ILogger<GetOrdersQueryHandler> logger;
        private IMapper mapper;
        private IOrderRepository repository;
        public GetOrdersQueryHandler(ILogger<GetOrdersQueryHandler> logger, IMapper mapper, IOrderRepository repository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<Result<IEnumerable<OrderDto>>> Handle(GetOrdersQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await repository.GetAll();
                return Result.Ok(mapper.Map<IEnumerable<OrderDto>>(result));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return Result.Fail<IEnumerable<OrderDto>>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
