using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Order.Application.Contracts.Persistence;

using Webshop.Domain.Common;
using Webshop.Order.Application.Features.Dtos;

namespace Webshop.Order.Application.Features.Queries.GetOrdersOfBuyer
{
    public class GetOrdersOfBuyerQueryHandler : IQueryHandler<GetOrdersOfBuyerQuery, List<OrderDto>>
    {
        private ILogger<GetOrdersOfBuyerQueryHandler> logger;
        private IMapper mapper;
        private IOrderRepository orderRepository;
        public GetOrdersOfBuyerQueryHandler(ILogger<GetOrdersOfBuyerQueryHandler> logger, IMapper mapper, IOrderRepository orderRepository)
        {
            this.logger=logger;
            this.mapper=mapper;
            this.orderRepository=orderRepository;
        }

        public async Task<Result<List<OrderDto>>> Handle(GetOrdersOfBuyerQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var queryResult = await orderRepository.GetByBuyerId(query.BuyerId);
                List<OrderDto> result = new List<OrderDto>();
                foreach (var element in queryResult)
                {
                    result.Add(mapper.Map<OrderDto>(element));
                }
                return Result.Ok<List<OrderDto>>(result);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return Result.Fail<List<OrderDto>>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
