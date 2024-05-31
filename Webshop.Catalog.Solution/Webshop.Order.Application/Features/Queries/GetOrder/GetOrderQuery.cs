using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Order.Application.Features.Dtos;

namespace Webshop.Order.Application.Features.Queries.GetOrder
{
    public class GetOrderQuery : IQuery<OrderDto>
    {
        public GetOrderQuery(int orderNumber)
        {
            OrderNumber = orderNumber;
        }

        public int OrderNumber { get; private set; }
    }
}
