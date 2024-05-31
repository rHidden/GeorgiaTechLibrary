﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Order.Application.Features.Dtos;

namespace Webshop.Order.Application.Features.Queries.GetOrders
{
    public class GetOrdersQuery : IQuery<IEnumerable<OrderDto>>
    {
    }
}
