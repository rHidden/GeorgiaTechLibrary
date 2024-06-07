using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;

namespace Webshop.Order.Domain.AggregateRoots
{
    public class Order : AggregateRoot
    {
        public Order(int userId)
        {
            UserId = userId;
            Date = DateTime.Now;
            OrderLines = new List<OrderLine>();
        }

        public Order() { } //for ORM
        public double TotalPrice { get; set; }
        public int Discount { get; set; }
        public DateTime Date { get; private set; }
        public int UserId { get; private set; }
        public IEnumerable<OrderLine> OrderLines { get; set; }
    }
}
