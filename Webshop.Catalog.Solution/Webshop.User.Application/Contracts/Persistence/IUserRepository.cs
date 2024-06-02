using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts.Persistence;

namespace Webshop.User.Application.Contracts.Persistence
{
    public interface IUserRepository : IRepository<Webshop.User.Domain.AggregateRoots.User>
    {        
    }
}
