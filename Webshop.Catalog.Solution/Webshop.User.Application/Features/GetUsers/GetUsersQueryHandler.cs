using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.User.Application.Contracts.Persistence;
using Webshop.User.Application.Features.Dto;
using Webshop.Domain.Common;

namespace Webshop.User.Application.Features.GetUsers
{
    public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, List<UserDto>>
    {
        private ILogger<GetUsersQueryHandler> logger;
        private IMapper mapper;
        private IUserRepository userRepository;
        public GetUsersQueryHandler(ILogger<GetUsersQueryHandler> logger, IMapper mapper, IUserRepository userRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<Result<List<UserDto>>> Handle(GetUsersQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var queryResult = await userRepository.GetAll();
                List<UserDto> result = new List<UserDto>();
                foreach (var element in queryResult)
                {
                    result.Add(mapper.Map<UserDto>(element));
                }
                return Result.Ok<List<UserDto>>(result);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return Result.Fail<List<UserDto>>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
