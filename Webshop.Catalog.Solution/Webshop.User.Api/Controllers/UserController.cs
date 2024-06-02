using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.User.Application.Features.CreateUser;
using Webshop.User.Application.Features.DeleteUser;
using Webshop.User.Application.Features.Dto;
using Webshop.User.Application.Features.GetUser;
using Webshop.User.Application.Features.GetUsers;
using Webshop.User.Application.Features.Requests;
using Webshop.User.Application.Features.UpdateUser;
using Webshop.Domain.Common;

namespace Webshop.User.Api.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : BaseController
    {
        private IDispatcher dispatcher;
        private IMapper mapper;
        private ILogger<UserController> logger;
        public UserController(IDispatcher dispatcher, IMapper mapper, ILogger<UserController> logger)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            GetUsersQuery query = new GetUsersQuery();
            Result<List<UserDto>> result = await this.dispatcher.Dispatch(query);
            if (result.Success)
            {
                return FromResult<List<UserDto>>(result);
            }
            else
            {
                this.logger.LogError(result.Error.Message);
                return Error(result.Error);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            GetUserQuery query = new GetUserQuery(id);
            Result<UserDto> result = await this.dispatcher.Dispatch(query);
            if(result.Success)
            {
                return FromResult<UserDto>(result);
            }
            else
            {
                this.logger.LogError(result.Error.Message);
                return Error(result.Error);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]CreateUserRequest request)
        {
            CreateUserRequest.Validator validator = new CreateUserRequest.Validator();
            var result = validator.Validate(request);
            if (result.IsValid)
            {
                Webshop.User.Domain.AggregateRoots.User user = this.mapper.Map<Webshop.User.Domain.AggregateRoots.User>(request);
                CreateUserCommand command = new CreateUserCommand(user);
                Result createResult = await this.dispatcher.Dispatch(command);
                return Ok(createResult);
            }
            else
            {
                this.logger.LogError(string.Join(",", result.Errors.Select(x => x.ErrorMessage)));
                return Error(result.Errors);
            }            
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            DeleteUserCommand command = new DeleteUserCommand(id);
            Result result = await this.dispatcher.Dispatch(command);
            if (result.Success)
            {
                return FromResult(result);
            }
            else
            {
                this.logger.LogError(string.Join(",", result.Error.Message));
                return Error(result.Error);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody]UpdateUserRequest request)
        {
            UpdateUserRequest.Validator validator = new UpdateUserRequest.Validator();
            var result = validator.Validate(request);
            if (result.IsValid)
            {
                Webshop.User.Domain.AggregateRoots.User user = this.mapper.Map<Domain.AggregateRoots.User>(request);
                UpdateUserCommand command = new UpdateUserCommand(user);
                Result updateResult = await this.dispatcher.Dispatch(command);
                return Ok(updateResult);
            }
            else
            {
                this.logger.LogError(string.Join(",", result.Errors.Select(x => x.ErrorMessage)));
                return Error(result.Errors);
            }
        }
    }
}
