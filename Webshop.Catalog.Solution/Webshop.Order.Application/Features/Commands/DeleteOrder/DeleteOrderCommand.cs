using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Order.Application.Features.Commands.DeleteOrder
{
    public class DeleteOrderCommand : ICommand
    {
        public DeleteOrderCommand(int orderNumber)
        {
            OrderNumber = orderNumber;
        }

        public int OrderNumber { get; private set; }
    }
}
