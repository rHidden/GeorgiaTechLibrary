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
        public UpdateOrderCommand(int userId, int discount, IEnumerable<UpdateOrderLineRequest> orderLines)
        {
            UserId = userId;
            Discount = discount;
            OrderLines = orderLines;
        }

        public int UserId { get; private set; }
        public int Discount { get; private set; }
        public IEnumerable<UpdateOrderLineRequest> OrderLines { get; private set; }
    }
}
