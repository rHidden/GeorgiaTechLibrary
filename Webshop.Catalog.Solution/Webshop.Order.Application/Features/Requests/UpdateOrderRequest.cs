using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Webshop.Domain.Common;

namespace Webshop.Order.Application.Features.Requests
{
    public class UpdateOrderRequest
    {
        public int UserId { get; set; }
        public int Discount { get; set; }
        public List<UpdateOrderLineRequest> OrderLines { get; set; }

        public class Validator : AbstractValidator<UpdateOrderRequest>
        {
            public Validator() 
            {
                //UserId
                RuleFor(r => r.UserId)
                    .NotEmpty().WithMessage(Errors.General.ValueIsEmpty(nameof(UserId)).Message)
                    .GreaterThan(0).WithMessage(Errors.General.ValueTooSmall(nameof(UserId), 0).Message);
                //Discount
                RuleFor(r => r.Discount)
                    .GreaterThanOrEqualTo(0).WithMessage(Errors.General.ValueTooSmall(nameof(Discount), 0).Message)
                    .LessThanOrEqualTo(15).WithMessage(Errors.General.ValueTooLarge(nameof(Discount), 15).Message);
                //OrderLines
                RuleFor(r => r.OrderLines)
                    .NotEmpty().WithMessage(Errors.General.ValueIsEmpty(nameof(OrderLines)).Message);
                RuleForEach(r => r.OrderLines).SetValidator(new UpdateOrderLineRequest.Validator());
            }
        }
    }
}
