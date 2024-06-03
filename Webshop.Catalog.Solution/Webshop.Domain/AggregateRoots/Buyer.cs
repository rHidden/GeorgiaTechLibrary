using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Domain.AggregateRoots
{
    public class Buyer : Customer
    {

        public Buyer(string name, string description) : base(name)
        {
            Ensure.That(description, nameof(description)).IsNotNullOrEmpty();
            BuyerDescription = description;
        }
        public Buyer() : base() { } //for ORM

        public string BuyerDescription { get; set; }
    }
}
