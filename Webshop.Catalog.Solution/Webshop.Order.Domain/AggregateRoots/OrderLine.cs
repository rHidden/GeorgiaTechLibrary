using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Order.Domain.AggregateRoots
{
    public class OrderLine : AggregateRoot
    {
        private int _orderId;
        public OrderLine(Product product, int quantity)
        {
            Ensure.That(product, nameof(product)).IsNotNull();
            Ensure.That(quantity, nameof(quantity)).IsGt(0);
            Product = product;
            Quantity = quantity;
            SubTotal = quantity * product.Price;
        }

        public OrderLine() { } //for ORM
        public int Quantity { get; private set; }
        public double SubTotal { get; private set; }
        public Product Product { get; set; }
        public int OrderId
        {
            get { return _orderId; }
            set
            {
                Ensure.That(value, nameof(OrderId)).IsGt(0);
                _orderId = value;
            }
        }
    }
}
