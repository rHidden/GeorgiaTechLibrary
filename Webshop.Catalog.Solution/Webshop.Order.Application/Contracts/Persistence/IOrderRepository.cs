using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts.Persistence;
using Webshop.Domain.Common;

namespace Webshop.Order.Application.Contracts.Persistence
{
    public interface IOrderRepository : IRepository<Domain.AggregateRoots.Order>
    {
    }
}
