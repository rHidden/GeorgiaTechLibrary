using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Catalog.Domain.AggregateRoots;

namespace Webshop.User.Domain.AggregateRoots
{
    public class Seller : User
    {
        public List<Product> Inventory { get; set; }

        public Seller() { }

        public Seller(string name, string address, string address2, string city, string region, string postalCode,
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
            Inventory = new List<Product>();
        }

    }
}
