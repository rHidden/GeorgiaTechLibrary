using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Order.Application.Features.Dtos;

namespace Webshop.Order.Application.Features.Queries.GetOrdersOfBuyer
{
    public class GetOrdersOfBuyerQuery : IQuery<List<OrderDto>>
    {
        public GetOrdersOfBuyerQuery(int buyerId)
            {
                BuyerId = buyerId;
            }

            public int BuyerId { get; private set; }
    }
}
