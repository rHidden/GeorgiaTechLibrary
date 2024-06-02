using FluentValidation;
using NUnit.Framework;
using System.Collections.Generic;
using Webshop.Order.Application.Features.Requests;

namespace Webshop.Order.Application.Test.RequestTest
{
    public class CreateOrderRequestTest
    {
        [Test]
        public void TestCreateOrderRequest_InvalidCustomerIdIsZero_ExpectFailure()
        {
            //Arrange
            int customerId = 0;
            int discount = 5;
            int quantity = 3;
            int productId = 7;
            List<CreateOrderLineRequest> orderLines = new List<CreateOrderLineRequest> { new CreateOrderLineRequest() 
                { 
                    Quantity = quantity, 
                    ProductId = productId 
                } 
            };
            CreateOrderRequest req = new CreateOrderRequest
            {
                CustomerId = customerId,
                Discount = discount,
                OrderLines = orderLines
            };
            CreateOrderRequest.Validator validator = new CreateOrderRequest.Validator();

            //Act
            var validationResults = validator.Validate(req);

            //Assert
            Assert.That(validationResults.Errors.Count, Is.EqualTo(2));
            Assert.That(validationResults.Errors[0].ErrorCode, Is.EqualTo("NotEmptyValidator"));
            Assert.That(validationResults.Errors[0].PropertyName, Is.EqualTo("CustomerId"));
            Assert.That(validationResults.Errors[1].ErrorCode, Is.EqualTo("GreaterThanValidator"));
            Assert.That(validationResults.Errors[1].PropertyName, Is.EqualTo("CustomerId"));
        }

        [Test]
        public void TestCreateOrderRequest_InvalidCustomerIdIsNegative_ExpectFailure()
        {
            //Arrange
            int customerId = -1;
            int discount = 5;
            int quantity = 3;
            int productId = 7;
            List<CreateOrderLineRequest> orderLines = new List<CreateOrderLineRequest> { new CreateOrderLineRequest()
                {
                    Quantity = quantity,
                    ProductId = productId
                }
            };
            CreateOrderRequest req = new CreateOrderRequest
            {
                CustomerId = customerId,
                Discount = discount,
                OrderLines = orderLines
            };
            CreateOrderRequest.Validator validator = new CreateOrderRequest.Validator();

            //Act
            var validationResults = validator.Validate(req);

            //Assert
            Assert.That(validationResults.Errors.Count, Is.EqualTo(1));
            Assert.That(validationResults.Errors[0].ErrorCode, Is.EqualTo("GreaterThanValidator"));
            Assert.That(validationResults.Errors[0].PropertyName, Is.EqualTo("CustomerId"));
        }

        [Test]
        public void TestCreateOrderRequest_InvalidDiscountIsNegative_ExpectFailure()
        {
            //Arrange
            int customerId = 1;
            int discount = -1;
            int quantity = 3;
            int productId = 7;
            List<CreateOrderLineRequest> orderLines = new List<CreateOrderLineRequest> { new CreateOrderLineRequest()
                {
                    Quantity = quantity,
                    ProductId = productId
                }
            };
            CreateOrderRequest req = new CreateOrderRequest
            {
                CustomerId = customerId,
                Discount = discount,
                OrderLines = orderLines
            };
            CreateOrderRequest.Validator validator = new CreateOrderRequest.Validator();

            //Act
            var validationResults = validator.Validate(req);

            //Assert
            Assert.That(validationResults.Errors.Count, Is.EqualTo(1));
            Assert.That(validationResults.Errors[0].ErrorCode, Is.EqualTo("GreaterThanOrEqualValidator"));
            Assert.That(validationResults.Errors[0].PropertyName, Is.EqualTo("Discount"));
        }

        [Test]
        public void TestCreateOrderRequest_InvalidDiscountIsTooLarge_ExpectFailure()
        {
            //Arrange
            int customerId = 1;
            int discount = 20;
            int quantity = 3;
            int productId = 7;
            List<CreateOrderLineRequest> orderLines = new List<CreateOrderLineRequest> { new CreateOrderLineRequest()
                {
                    Quantity = quantity,
                    ProductId = productId
                }
            };
            CreateOrderRequest req = new CreateOrderRequest
            {
                CustomerId = customerId,
                Discount = discount,
                OrderLines = orderLines
            };
            CreateOrderRequest.Validator validator = new CreateOrderRequest.Validator();

            //Act
            var validationResults = validator.Validate(req);

            //Assert
            Assert.That(validationResults.Errors.Count, Is.EqualTo(1));
            Assert.That(validationResults.Errors[0].ErrorCode, Is.EqualTo("LessThanOrEqualValidator"));
            Assert.That(validationResults.Errors[0].PropertyName, Is.EqualTo("Discount"));
        }

        [Test]
        public void TestCreateOrderRequest_InvalidOrderLinesIsEmpty_ExpectFailure()
        {
            //Arrange
            int customerId = 1;
            int discount = 5;
            List<CreateOrderLineRequest> orderLines = new List<CreateOrderLineRequest>();
            CreateOrderRequest req = new CreateOrderRequest
            {
                CustomerId = customerId,
                Discount = discount,
                OrderLines = orderLines
            };
            CreateOrderRequest.Validator validator = new CreateOrderRequest.Validator();

            //Act
            var validationResults = validator.Validate(req);

            //Assert
            Assert.That(validationResults.Errors.Count, Is.EqualTo(1));
            Assert.That(validationResults.Errors[0].ErrorCode, Is.EqualTo("NotEmptyValidator"));
            Assert.That(validationResults.Errors[0].PropertyName, Is.EqualTo("OrderLines"));
        }

        [Test]
        public void TestCreateOrderRequest_ValidInput_ExpectSuccess()
        {
            //Arrange
            int customerId = 1;
            int discount = 10;
            int quantity = 3;
            int productId = 7;
            List<CreateOrderLineRequest> orderLines = new List<CreateOrderLineRequest> { new CreateOrderLineRequest()
                {
                    Quantity = quantity,
                    ProductId = productId
                }
            };
            CreateOrderRequest req = new CreateOrderRequest
            {
                CustomerId = customerId,
                Discount = discount,
                OrderLines = orderLines
            };
            CreateOrderRequest.Validator validator = new CreateOrderRequest.Validator();

            //Act
            var validationResults = validator.Validate(req);

            //Assert
            Assert.That(validationResults.Errors.Count, Is.EqualTo(0));
        }
    }
}
