using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.User.Application.Features.Dto;

namespace Webshop.User.Application.Features.GetUsers
{
    public class GetUsersQuery : IQuery<List<UserDto>>
    {
    }
}
