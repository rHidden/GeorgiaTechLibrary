﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Contracts.Persistence;
using Webshop.Category.Application.Features.Category.Dtos;
using Webshop.Domain.Common;

namespace Webshop.Category.Application.Features.Category.Queries.GetChildCategories
{
    public class GetChildCategoriesQueryHandler : IQueryHandler<GetChildCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private ILogger<GetChildCategoriesQueryHandler> logger;
        private IMapper mapper;
        private ICategoryRepository categoryRepository;
        public GetChildCategoriesQueryHandler(ILogger<GetChildCategoriesQueryHandler> logger, IMapper mapper, ICategoryRepository categoryRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
        }

        public async Task<Result<IEnumerable<CategoryDto>>> Handle(GetChildCategoriesQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var childCategoriesResult = await this.categoryRepository.GetChildCategories(query.ParentCategoryId);
                return Result.Ok(this.mapper.Map<IEnumerable<CategoryDto>>(childCategoriesResult));
            }
            catch(Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail<IEnumerable<CategoryDto>>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
