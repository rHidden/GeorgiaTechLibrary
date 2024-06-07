using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Order.Domain.AggregateRoots;

namespace Webshop.Order.Application.Features.Dtos
{
    public class OrderDto
    {
        public int OrderNumber { get; set; }
        public double TotalPrice { get; set; }
        public int Discount { get; set; }
        public DateTime Date { get; private set; }
        public int UserId { get; private set; }
        public IEnumerable<OrderLine> OrderLines { get; set; } = Enumerable.Empty<OrderLine>();
    }
}
