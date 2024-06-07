using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.User.Application.Features.UpdateUser
{
    public class UpdateUserCommand : ICommand
    {
        public UpdateUserCommand(Domain.AggregateRoots.User user)
        {
            Ensure.That(user, nameof(user)).IsNotNull();
            Ensure.That(user.Id, nameof(user.Id)).IsNotDefault();
            Ensure.That(user.Id, nameof(user.Id)).IsGt<int>(0);
            Ensure.That(user.Name, nameof(user.Name)).IsNotNullOrEmpty();
            User = user;
        }

        public Domain.AggregateRoots.User User { get; private set; }
    }
}
