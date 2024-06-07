using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.User.Application.Contracts.Persistence;
using Webshop.User.Application.Features.Dto;

namespace Webshop.User.Application.Features.GetBuyers
{
    public class GetBuyersQueryHandler : IQueryHandler<GetBuyersQuery, List<BuyerDto>>
    {
        private ILogger<GetBuyersQueryHandler> logger;
        private IMapper mapper;
        private IUserRepository userRepository;

        public GetBuyersQueryHandler(ILogger<GetBuyersQueryHandler> logger, IMapper mapper, IUserRepository userRepository) 
        {
            this.logger = logger;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<Result<List<BuyerDto>>> Handle(GetBuyersQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var queryResult = await userRepository.GetAllBuyers();
                List<BuyerDto> result = new List<BuyerDto>();
                foreach (var element in queryResult)
                {
                    result.Add(mapper.Map<BuyerDto>(element));
                }
                return Result.Ok<List<BuyerDto>>(result);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return Result.Fail<List<BuyerDto>>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
