using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Order.Application.Features.Requests;

namespace Webshop.Order.Application.Features.Commands.UpdateOrder
{
    public class UpdateOrderCommand : ICommand
    {
        public UpdateOrderCommand(int customerId, int discount, IEnumerable<UpdateOrderLineRequest> orderLines)
        {
            CustomerId = customerId;
            Discount = discount;
            OrderLines = orderLines;
        }

        public int CustomerId { get; private set; }
        public int Discount { get; private set; }
        public IEnumerable<UpdateOrderLineRequest> OrderLines { get; private set; }
    }
}
