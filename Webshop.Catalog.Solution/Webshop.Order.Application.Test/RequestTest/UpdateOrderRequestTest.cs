using FluentValidation;
using NUnit.Framework;
using System.Collections.Generic;
using Webshop.Order.Application.Features.Requests;

namespace Webshop.Order.Application.Test.RequestTest
{
    public class UpdateOrderRequestTest
    {
        [Test]
        public void TestUpdateOrderRequest_InvalidCustomerIdIsZero_ExpectFailure()
        {
            //Arrange
            int customerId = 0;
            int discount = 5;
            int quantity = 3;
            int productId = 7;
            List<UpdateOrderLineRequest> orderLines = new List<UpdateOrderLineRequest> { new UpdateOrderLineRequest()
                {
                    Quantity = quantity,
                    ProductId = productId
                }
            };
            UpdateOrderRequest req = new UpdateOrderRequest
            {
                CustomerId = customerId,
                Discount = discount,
                OrderLines = orderLines
            };
            UpdateOrderRequest.Validator validator = new UpdateOrderRequest.Validator();

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
        public void TestUpdateOrderRequest_InvalidCustomerIdIsNegative_ExpectFailure()
        {
            //Arrange
            int customerId = -1;
            int discount = 5;
            int quantity = 3;
            int productId = 7;
            List<UpdateOrderLineRequest> orderLines = new List<UpdateOrderLineRequest> { new UpdateOrderLineRequest()
                {
                    Quantity = quantity,
                    ProductId = productId
                }
            };
            UpdateOrderRequest req = new UpdateOrderRequest
            {
                CustomerId = customerId,
                Discount = discount,
                OrderLines = orderLines
            };
            UpdateOrderRequest.Validator validator = new UpdateOrderRequest.Validator();

            //Act
            var validationResults = validator.Validate(req);

            //Assert
            Assert.That(validationResults.Errors.Count, Is.EqualTo(1));
            Assert.That(validationResults.Errors[0].ErrorCode, Is.EqualTo("GreaterThanValidator"));
            Assert.That(validationResults.Errors[0].PropertyName, Is.EqualTo("CustomerId"));
        }

        [Test]
        public void TestUpdateOrderRequest_InvalidDiscountIsNegative_ExpectFailure()
        {
            //Arrange
            int customerId = 1;
            int discount = -1;
            int quantity = 3;
            int productId = 7;
            List<UpdateOrderLineRequest> orderLines = new List<UpdateOrderLineRequest> { new UpdateOrderLineRequest()
                {
                    Quantity = quantity,
                    ProductId = productId
                }
            };
            UpdateOrderRequest req = new UpdateOrderRequest
            {
                CustomerId = customerId,
                Discount = discount,
                OrderLines = orderLines
            };
            UpdateOrderRequest.Validator validator = new UpdateOrderRequest.Validator();

            //Act
            var validationResults = validator.Validate(req);

            //Assert
            Assert.That(validationResults.Errors.Count, Is.EqualTo(1));
            Assert.That(validationResults.Errors[0].ErrorCode, Is.EqualTo("GreaterThanOrEqualValidator"));
            Assert.That(validationResults.Errors[0].PropertyName, Is.EqualTo("Discount"));
        }

        [Test]
        public void TestUpdateOrderRequest_InvalidDiscountIsTooLarge_ExpectFailure()
        {
            //Arrange
            int customerId = 1;
            int discount = 20;
            int quantity = 3;
            int productId = 7;
            List<UpdateOrderLineRequest> orderLines = new List<UpdateOrderLineRequest> { new UpdateOrderLineRequest()
                {
                    Quantity = quantity,
                    ProductId = productId
                }
            };
            UpdateOrderRequest req = new UpdateOrderRequest
            {
                CustomerId = customerId,
                Discount = discount,
                OrderLines = orderLines
            };
            UpdateOrderRequest.Validator validator = new UpdateOrderRequest.Validator();

            //Act
            var validationResults = validator.Validate(req);

            //Assert
            Assert.That(validationResults.Errors.Count, Is.EqualTo(1));
            Assert.That(validationResults.Errors[0].ErrorCode, Is.EqualTo("LessThanOrEqualValidator"));
            Assert.That(validationResults.Errors[0].PropertyName, Is.EqualTo("Discount"));
        }

        [Test]
        public void TestUpdateOrderRequest_InvalidOrderLinesIsEmpty_ExpectFailure()
        {
            //Arrange
            int customerId = 1;
            int discount = 5;
            List<UpdateOrderLineRequest> orderLines = new List<UpdateOrderLineRequest>();
            UpdateOrderRequest req = new UpdateOrderRequest
            {
                CustomerId = customerId,
                Discount = discount,
                OrderLines = orderLines
            };
            UpdateOrderRequest.Validator validator = new UpdateOrderRequest.Validator();

            //Act
            var validationResults = validator.Validate(req);

            //Assert
            Assert.That(validationResults.Errors.Count, Is.EqualTo(1));
            Assert.That(validationResults.Errors[0].ErrorCode, Is.EqualTo("NotEmptyValidator"));
            Assert.That(validationResults.Errors[0].PropertyName, Is.EqualTo("OrderLines"));
        }

        [Test]
        public void TestUpdateOrderRequest_ValidInput_ExpectSuccess()
        {
            //Arrange
            int customerId = 1;
            int discount = 10;
            int quantity = 3;
            int productId = 7;
            List<UpdateOrderLineRequest> orderLines = new List<UpdateOrderLineRequest> { new UpdateOrderLineRequest()
                {
                    Quantity = quantity,
                    ProductId = productId
                }
            };
            UpdateOrderRequest req = new UpdateOrderRequest
            {
                CustomerId = customerId,
                Discount = discount,
                OrderLines = orderLines
            };
            UpdateOrderRequest.Validator validator = new UpdateOrderRequest.Validator();

            //Act
            var validationResults = validator.Validate(req);

            //Assert
            Assert.That(validationResults.Errors.Count, Is.EqualTo(0));
        }
    }
}
