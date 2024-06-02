using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.User.Domain.AggregateRoots
{
    public class Buyer : User
    {
        public List<Webshop.Order.Domain.AggregateRoots.Order> Orders { get; set; }

        public Buyer() { }

        public Buyer(string name, string address, string address2, string city, string region, string postalCode,
            string country, string email) 
        {
            Name = name;
            Address = address;
            Address2 = address2;
            City = city;
            Region = region;
            PostalCode = postalCode;
            Country = country;
            Email = email;
            Orders = new List<Order.Domain.AggregateRoots.Order>();
        }
    }
}
