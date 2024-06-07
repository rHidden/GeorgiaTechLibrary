using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Catalog.Domain.AggregateRoots;

namespace Webshop.User.Application.Features.Dto
{
    public class SellerDto : UserDto
    {
        public List<Product> Inventory { get; set; } = new List<Product>();
    }
}
