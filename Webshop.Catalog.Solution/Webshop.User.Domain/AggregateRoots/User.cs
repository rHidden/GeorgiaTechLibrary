﻿using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;

namespace Webshop.User.Domain.AggregateRoots
{
    public class User : AggregateRoot
    {
        public User(string name)
        {
            Ensure.That(name, nameof(name)).IsNotNullOrEmpty();
            Name = name;
        }

        public User() { } //for ORM

        public string Name { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public Boolean IsBuyer { get; set; }
        public Boolean IsSeller { get; set; }
    }
}
