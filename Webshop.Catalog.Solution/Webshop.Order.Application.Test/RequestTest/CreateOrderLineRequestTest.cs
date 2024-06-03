using FluentValidation;
using NUnit.Framework;
using Webshop.Order.Application.Features.Requests;

namespace Webshop.Order.Application.Test.RequestTest
{
    public class CreateOrderLineRequestTest
    {
        [Test]
        public void TestCreateOrderLineRequest_InvalidQuantityIsZero_ExpectFailure()
        {
            //Arrange
            int quantity = 0;
            int productId = 1;
            CreateOrderLineRequest req = new CreateOrderLineRequest
            {
                Quantity = quantity,
                ProductId = productId
            };
            CreateOrderLineRequest.Validator validator = new CreateOrderLineRequest.Validator();

            //Act
            var validationResults = validator.Validate(req);

            //Assert
            Assert.That(validationResults.Errors.Count, Is.EqualTo(2));
            Assert.That(validationResults.Errors[0].ErrorCode, Is.EqualTo("NotEmptyValidator"));
            Assert.That(validationResults.Errors[0].PropertyName, Is.EqualTo("Quantity"));
            Assert.That(validationResults.Errors[1].ErrorCode, Is.EqualTo("GreaterThanValidator"));
            Assert.That(validationResults.Errors[1].PropertyName, Is.EqualTo("Quantity"));
        }

        [Test]
        public void TestCreateOrderLineRequest_InvalidQuantityIsNegative_ExpectFailure()
        {
            //Arrange
            int quantity = -1;
            int productId = 1;
            CreateOrderLineRequest req = new CreateOrderLineRequest
            {
                Quantity = quantity,
                ProductId = productId
            };
            CreateOrderLineRequest.Validator validator = new CreateOrderLineRequest.Validator();

            //Act
            var validationResults = validator.Validate(req);

            //Assert
            Assert.That(validationResults.Errors.Count, Is.EqualTo(1));
            Assert.That(validationResults.Errors[0].ErrorCode, Is.EqualTo("GreaterThanValidator"));
            Assert.That(validationResults.Errors[0].PropertyName, Is.EqualTo("Quantity"));
        }

        [Test]
        public void TestCreateOrderLineRequest_InvalidProductIdIsZero_ExpectFailure()
        {
            //Arrange
            int quantity = 1;
            int productId = 0;
            CreateOrderLineRequest req = new CreateOrderLineRequest
            {
                Quantity = quantity,
                ProductId = productId
            };
            CreateOrderLineRequest.Validator validator = new CreateOrderLineRequest.Validator();

            //Act
            var validationResults = validator.Validate(req);

            //Assert
            Assert.That(validationResults.Errors.Count, Is.EqualTo(2));
            Assert.That(validationResults.Errors[0].ErrorCode, Is.EqualTo("NotEmptyValidator"));
            Assert.That(validationResults.Errors[0].PropertyName, Is.EqualTo("ProductId"));
            Assert.That(validationResults.Errors[1].ErrorCode, Is.EqualTo("GreaterThanValidator"));
            Assert.That(validationResults.Errors[1].PropertyName, Is.EqualTo("ProductId"));
        }

        [Test]
        public void TestCreateOrderLineRequest_InvalidProductIdIsNegative_ExpectFailure()
        {
            //Arrange
            int quantity = 1;
            int productId = -1;
            CreateOrderLineRequest req = new CreateOrderLineRequest
            {
                Quantity = quantity,
                ProductId = productId
            };
            CreateOrderLineRequest.Validator validator = new CreateOrderLineRequest.Validator();

            //Act
            var validationResults = validator.Validate(req);

            //Assert
            Assert.That(validationResults.Errors.Count, Is.EqualTo(1));
            Assert.That(validationResults.Errors[0].ErrorCode, Is.EqualTo("GreaterThanValidator"));
            Assert.That(validationResults.Errors[0].PropertyName, Is.EqualTo("ProductId"));
        }

        [Test]
        public void TestCreateOrderLineRequest_ValidInput_ExpectSuccess()
        {
            //Arrange
            int quantity = 5;
            int productId = 10;
            CreateOrderLineRequest req = new CreateOrderLineRequest
            {
                Quantity = quantity,
                ProductId = productId
            };
            CreateOrderLineRequest.Validator validator = new CreateOrderLineRequest.Validator();

            //Act
            var validationResults = validator.Validate(req);

            //Assert
            Assert.That(validationResults.Errors.Count, Is.EqualTo(0));
        }
    }
}
