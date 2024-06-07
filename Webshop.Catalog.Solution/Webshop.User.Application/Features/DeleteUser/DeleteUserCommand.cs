using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.User.Application.Features.DeleteUser
{
    public class DeleteUserCommand : ICommand
    {
        public DeleteUserCommand(int userId)
        {
            Ensure.That(userId, nameof(userId)).IsNotDefault<int>(); //no default, or zero
            Ensure.That(userId, nameof(userId)).IsGt<int>(0); //no negative id
            UserId = userId;
        }

        public int UserId { get; private set; }
    }
}
