using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Webshop.Domain.Common;

namespace Webshop.Order.Domain.AggregateRoots
{
    public class Order : AggregateRoot
    {
        private double _discount;
        private IEnumerable<OrderLine> _orderLines;

        public Order(int customerId)
        {
            Ensure.That(customerId, nameof(customerId)).IsGt(0);
            CustomerId = customerId;
            Date = DateTime.Now;
            OrderLines = new List<OrderLine>();
        }

        public Order() 
        {
            OrderLines = new List<OrderLine>();
        } //for ORM
        public double TotalPrice { get; set; }
        public double Discount
        {
            get { return _discount; }
            set
            {
                Ensure.That(value, nameof(Discount)).IsInRange(0, 15);
                _discount = value;
            }
        }
        public DateTime Date { get; private set; }
        public int CustomerId { get; private set; }
        public IEnumerable<OrderLine> OrderLines
        {
            get { return _orderLines; }
            set
            {
                Ensure.That(value, nameof(OrderLines)).IsNotNull();
                _orderLines = value;
            }
        }
    }
}
