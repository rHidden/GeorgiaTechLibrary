using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Order.Application.Features.Commands.CreateOrder;
using Webshop.Order.Application.Features.Commands.DeleteOrder;
using Webshop.Order.Application.Features.Commands.UpdateOrder;
using Webshop.Order.Application.Features.Dtos;
using Webshop.Order.Application.Features.Queries.GetOrder;
using Webshop.Order.Application.Features.Queries.GetOrders;
using Webshop.Order.Application.Features.Queries.GetOrdersOfBuyer;
using Webshop.Order.Application.Features.Requests;

namespace Webshop.Order.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        private IDispatcher dispatcher;
        private ILogger<OrderController> logger;
        private IMapper mapper;

        public OrderController(IDispatcher dispatcher, ILogger<OrderController> logger, IMapper mapper)
        {
            this.dispatcher = dispatcher;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody]CreateOrderRequest request)
        {
            CreateOrderRequest.Validator validator = new CreateOrderRequest.Validator();
            var validationResult = await validator.ValidateAsync(request);
            if (validationResult.IsValid)
            {
                CreateOrderCommand command = new CreateOrderCommand(request.UserId, request.Discount, request.OrderLines);
                var commandResult = await this.dispatcher.Dispatch(command);
                return FromResult(commandResult);
            }
            else
            {
                this.logger.LogError(string.Join(",", validationResult.Errors.Select(e=>e.ErrorMessage)));
                return Error(validationResult.Errors);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest request, int id)
        {
            UpdateOrderRequest.Validator validator = new UpdateOrderRequest.Validator();
            var validationResult = await validator.ValidateAsync(request);
            if (validationResult.IsValid)
            {
                UpdateOrderCommand command = new UpdateOrderCommand(request.UserId, request.Discount, request.OrderLines);
                var commandResult = await this.dispatcher.Dispatch(command);
                return FromResult(commandResult);
            }
            else
            {
                this.logger.LogError(string.Join(",", validationResult.Errors.Select(e => e.ErrorMessage)));
                return Error(validationResult.Errors);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            DeleteOrderCommand command = new DeleteOrderCommand(id);
            var commandResult = await this.dispatcher.Dispatch(command);
            return FromResult(commandResult);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            GetOrderQuery query = new GetOrderQuery(id);
            var result = await this.dispatcher.Dispatch(query);
            return FromResult<OrderDto>(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            GetOrdersQuery query = new GetOrdersQuery();
            var result = await this.dispatcher.Dispatch(query);
            return FromResult<IEnumerable<OrderDto>>(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersOfBuyer(int buyerId)
        {
            GetOrdersOfBuyerQuery query = new GetOrdersOfBuyerQuery(buyerId);
            var result = await this.dispatcher.Dispatch(query);
            return FromResult<List<OrderDto>>(result);
        }
    }
}
