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

namespace Webshop.User.Application.Features.GetUser
{
    public class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserDto>
    {
        private ILogger<GetUserQueryHandler> logger;
        private IMapper mapper;
        private IUserRepository userRepository;
        public GetUserQueryHandler(ILogger<GetUserQueryHandler> logger, IMapper mapper, IUserRepository userRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<Result<UserDto>> Handle(GetUserQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                Domain.AggregateRoots.User user = await userRepository.GetById(query.UserId);
                return mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return Result.Fail<UserDto>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
