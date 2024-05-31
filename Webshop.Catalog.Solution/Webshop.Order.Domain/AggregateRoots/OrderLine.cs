using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Order.Domain.AggregateRoots
{
    public class OrderLine : AggregateRoot
    {
        public OrderLine(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
            SubTotal = quantity * product.Price;
        }

        public OrderLine() { } //for ORM
        public int Quantity { get; set; }
        public double SubTotal { get; set; }
        public Product Product { get; set; }
        public int OrderId { get; set; }
    }
}
