using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.User.Application.Features.Dto;

namespace Webshop.User.Application.Features.GetUser
{
    public class GetUserQuery : IQuery<UserDto>
    {
        public GetUserQuery(int userId)
        {
            Ensure.That(userId, nameof(userId)).IsNotDefault<int>();
            Ensure.That(userId, nameof(userId)).IsGt<int>(0);
            UserId = userId;
        }

        public int UserId { get; private set; }
    }
}
