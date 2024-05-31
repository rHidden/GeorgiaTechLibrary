using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Order.Application.Contracts.Persistence;
using Webshop.Order.Application.Features.Dtos;

namespace Webshop.Order.Application.Features.Queries.GetOrder
{
    public class GetOrderQueryHandler : IQueryHandler<GetOrderQuery, OrderDto>
    {
        private ILogger<GetOrderQuery> logger;
        private IMapper mapper;
        private IOrderRepository repository;
        public GetOrderQueryHandler(ILogger<GetOrderQuery> logger, IMapper mapper, IOrderRepository repository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<Result<OrderDto>> Handle(GetOrderQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var order = await repository.GetById(query.OrderNumber);
                return mapper.Map<OrderDto>(order);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return Result.Fail<OrderDto>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
