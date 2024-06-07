using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.User.Application.Features.GetSellers;
using Webshop.User.Application.Features.Dto;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Webshop.User.Application.Contracts.Persistence;
using Webshop.Domain.Common;
using Microsoft.Extensions.Primitives;
using System.Threading;

namespace Webshop.User.Application.Features.GetSellers
{
    public class GetSellersQueryHandler : IQueryHandler<GetSellersQuery, List<SellerDto>>
    {
        private ILogger<GetSellersQueryHandler> logger;
        private IMapper mapper;
        private IUserRepository userRepository;

        public GetSellersQueryHandler(ILogger<GetSellersQueryHandler> logger, IMapper mapper, IUserRepository userRepository) 
        {
            this.logger = logger;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<Result<List<SellerDto>>> Handle(GetSellersQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var queryResult = await userRepository.GetAllBuyers();
                List<SellerDto> result = new List<SellerDto>();
                foreach (var element in queryResult) {
                    result.Add(mapper.Map<SellerDto>(element));
                }
                return Result.Ok<List<SellerDto>>(result);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return Result.Fail<List<SellerDto>>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
