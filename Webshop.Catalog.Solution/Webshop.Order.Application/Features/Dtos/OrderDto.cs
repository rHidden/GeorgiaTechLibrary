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
        public int Id { get; set; }
        public double TotalPrice { get; set; }
        public int Discount { get; set; }
        public DateTime Date { get; private set; }
        public int CustomerId { get; private set; }
        public IEnumerable<OrderLineDto> OrderLines { get; set; } = Enumerable.Empty<OrderLineDto>();
    }
}
