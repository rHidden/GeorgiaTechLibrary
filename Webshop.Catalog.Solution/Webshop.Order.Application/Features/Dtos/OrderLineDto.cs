using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Catalog.Application.Features.Product.Dtos;
using Webshop.Catalog.Domain.AggregateRoots;

namespace Webshop.Order.Application.Features.Dtos
{
    public class OrderLineDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double SubTotal { get; set; }
        public ProductDto Product { get; set; }
        public int OrderId { get; set; }
    }
}
