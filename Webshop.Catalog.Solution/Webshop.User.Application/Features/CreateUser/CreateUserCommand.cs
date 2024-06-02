using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.User.Application.Features.CreateUser
{
    public class CreateUserCommand : ICommand
    {
        public CreateUserCommand(Domain.AggregateRoots.User user)
        {
            Ensure.That(user, nameof(user)).IsNotNull();
            User = user;
        }

        public Domain.AggregateRoots.User User { get; private set; }
    }
}
