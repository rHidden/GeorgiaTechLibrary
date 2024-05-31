using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Order.Application.Features.Requests;
using Webshop.Order.Domain.AggregateRoots;

namespace Webshop.Order.Application.Features.Commands.CreateOrder
{
    public class CreateOrderCommand : ICommand
    {
        public CreateOrderCommand(int customerId, int discount, IEnumerable<CreateOrderLineRequest> orderLines)
        {
            CustomerId = customerId;
            Discount = discount;
            OrderLines = orderLines;
        }

        public int CustomerId { get; private set; }
        public int Discount { get; private set; }
        public IEnumerable<CreateOrderLineRequest> OrderLines { get; private set; }
    }
}
