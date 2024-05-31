using FluentValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Order.Application.Features.Requests
{
    public class CreateOrderLineRequest
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }

        public class Validator : AbstractValidator<CreateOrderLineRequest>
        {
            public Validator()
            {
                //Quantity
                RuleFor(r => r.Quantity)
                    .NotEmpty().WithMessage(Errors.General.ValueIsEmpty(nameof(Quantity)).Message)
                    .GreaterThan(0).WithMessage(Errors.General.ValueTooSmall(nameof(Quantity), 0).Message);
                //Productid
                RuleFor(r => r.ProductId)
                    .NotEmpty().WithMessage(Errors.General.ValueIsEmpty(nameof(ProductId)).Message)
                    .GreaterThan(0).WithMessage(Errors.General.ValueTooSmall(nameof(ProductId), 0).Message);
            }
        }
    }
}
