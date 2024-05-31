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

        public int CustomerId { get; private set; }
        public int Discount { get; private set; }
        public List<UpdateOrderLineRequest> OrderLines { get; private set; }

        public class Validator : AbstractValidator<UpdateOrderRequest>
        {
            public Validator() 
            {
                //CustomerId
                RuleFor(r => r.CustomerId)
                    .NotEmpty().WithMessage(Errors.General.ValueIsEmpty(nameof(CustomerId)).Message)
                    .GreaterThan(0).WithMessage(Errors.General.ValueTooSmall(nameof(CustomerId), 0).Message);
                //Discount
                RuleFor(r => r.Discount)
                    .NotEmpty().WithMessage(Errors.General.ValueIsEmpty(nameof(Discount)).Message)
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
