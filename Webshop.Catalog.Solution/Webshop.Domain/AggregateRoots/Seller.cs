using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;

namespace Webshop.Domain.AggregateRoots
{
    public class Seller : Customer
    {
        private double _review;
        public Seller(string name) : base(name) { }

        public Seller() : base() { } //for ORM

        public double SellerReview
        {
            get { return _review; }
            set
            {
                Ensure.That(value, nameof(SellerReview)).IsInRange(0, 5);
                _review = value;
            }
        }
    }
}
