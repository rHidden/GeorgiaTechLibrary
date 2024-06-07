using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.User.Application.Features.Dto
{
    public class BuyerDto : UserDto
    {
        public List<Webshop.Order.Domain.AggregateRoots.Order> Orders { get; set; } = new List<Webshop.Order.Domain.AggregateRoots.Order>();
    }
}
