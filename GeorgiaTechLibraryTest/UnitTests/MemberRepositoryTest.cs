using Xunit;
using DataAccess.DAO;
using DataAccess.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using Moq;
using DataAccess.Repositories;
using DataAccess.DAO.DAOIntefaces;
using DataAccess.Repositories.RepositoryInterfaces;
using System.Threading.Tasks;

namespace GeorgiaTechLibraryTest.UnitTests
{
    public class MemberRepositoryTest
    {
        private Mock<IDatabaseConnectionFactory> _mockDatabaseConnectionFactory;
        private MemberRepository _memberRepository;
        private string _connectionString;

        public MemberRepositoryTest()
        {
            _connectionString = DatabaseConnectionTest._connectionString;
            _mockDatabaseConnectionFactory = new Mock<IDatabaseConnectionFactory>();
            _mockDatabaseConnectionFactory.Setup(d => d.CreateConnection())
                .Returns(() => new SqlConnection(_connectionString));
            _memberRepository = new MemberRepository(_mockDatabaseConnectionFactory.Object);
        }

        [Fact]
        public async Task CreateMember_CreatesNewMember()
        {
            // Arrange
            var newMember = new Member
            {
                SSN = "1",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                CardNumber = "123456",
                ExpiryDate = DateTime.UtcNow.AddYears(1),
                Photo = "photo_url",
                MemberType = "Regular"
            };

            // Act
            var result = await _memberRepository.CreateMember(newMember);
            await _memberRepository.DeleteMember(newMember.SSN);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newMember.SSN, result.SSN);
            Assert.Equal(newMember.FirstName, result.FirstName);
            Assert.Equal(newMember.LastName, result.LastName);
            Assert.Equal(newMember.PhoneNumber, result.PhoneNumber);
            Assert.Equal(newMember.UserAddress.Street, result.UserAddress.Street);
            Assert.Equal(newMember.CardNumber, result.CardNumber);
            Assert.Equal(newMember.ExpiryDate, result.ExpiryDate);
            Assert.Equal(newMember.Photo, result.Photo);
            Assert.Equal(newMember.MemberType, result.MemberType);
        }

        [Fact]
        public async Task GetMember_WithValidSSN_ReturnsMember()
        {
            // Arrange
            var expectedMember = new Member
            {
                SSN = "2",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                CardNumber = "123456",
                ExpiryDate = DateTime.UtcNow.AddYears(1),
                Photo = "photo_url",
                MemberType = "Regular"
            };

            // Act
            await _memberRepository.CreateMember(expectedMember);
            var result = await _memberRepository.GetMember(expectedMember.SSN);
            await _memberRepository.DeleteMember(expectedMember.SSN);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedMember.SSN, result.SSN);
            Assert.Equal(expectedMember.FirstName, result.FirstName);
            Assert.Equal(expectedMember.LastName, result.LastName);
            Assert.Equal(expectedMember.PhoneNumber, result.PhoneNumber);
            Assert.Equal(expectedMember.UserAddress.Street, result.UserAddress.Street);
            Assert.Equal(expectedMember.CardNumber, result.CardNumber);
            Assert.Equal(expectedMember.Photo, result.Photo);
            Assert.Equal(expectedMember.MemberType, result.MemberType);
        }

        //[Fact]
        //public async Task ListMembers_ReturnsListOfMembers()
        //{
        //    // Arrange
        //    var member1 = new Member
        //    {
        //        SSN = "3",
        //        FirstName = "John",
        //        LastName = "Doe",
        //        PhoneNumber = "1234567890",
        //        UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
        //        CardNumber = "123456",
        //        ExpiryDate = DateTime.UtcNow.AddYears(1),
        //        Photo = "photo_url",
        //        MemberType = "Regular"
        //    };
        //    var member2 = new Member
        //    {
        //        SSN = "4",
        //        FirstName = "Jane",
        //        LastName = "Doe",
        //        PhoneNumber = "0987654321",
        //        UserAddress = new Address { Street = "Second St", StreetNumber = "2", City = "City", ZipCode = "54321" },
        //        CardNumber = "654321",
        //        ExpiryDate = DateTime.UtcNow.AddYears(1),
        //        Photo = "photo_url",
        //        MemberType = "Premium"
        //    };

        //    // Act
        //    await _memberRepository.CreateMember(member1);
        //    await _memberRepository.CreateMember(member2);
        //    var result = await _memberRepository.ListMembers();
        //    await _memberRepository.DeleteMember(member1.SSN);
        //    await _memberRepository.DeleteMember(member2.SSN);

        //    // Assert
        //    Assert.Equal(2, result.Count);
        //    Assert.Contains(result, r => r.SSN == member1.SSN && r.FirstName == member1.FirstName && r.LastName == member1.LastName && r.MemberType == member1.MemberType);
        //    Assert.Contains(result, r => r.SSN == member2.SSN && r.FirstName == member2.FirstName && r.LastName == member2.LastName && r.MemberType == member2.MemberType);
        //}

        [Fact]
        public async Task UpdateMember_UpdatesExistingMember()
        {
            // Arrange
            var originalMember = new Member
            {
                SSN = "5",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                CardNumber = "123456",
                ExpiryDate = DateTime.UtcNow.AddYears(1),
                Photo = "photo_url",
                MemberType = "Regular"
            };
            var updatedMember = new Member
            {
                SSN = "5",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "0987654321",
                UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                CardNumber = "654321",
                ExpiryDate = DateTime.UtcNow.AddYears(2),
                Photo = "new_photo_url",
                MemberType = "Premium"
            };

            // Act
            await _memberRepository.CreateMember(originalMember);
            var result = await _memberRepository.UpdateMember(updatedMember);
            await _memberRepository.DeleteMember(updatedMember.SSN);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedMember.SSN, result.SSN);
            Assert.Equal(updatedMember.FirstName, result.FirstName);
            Assert.Equal(updatedMember.LastName, result.LastName);
            Assert.Equal(updatedMember.PhoneNumber, result.PhoneNumber);
            Assert.Equal(updatedMember.UserAddress.Street, result.UserAddress.Street);
            Assert.Equal(updatedMember.CardNumber, result.CardNumber);
            Assert.Equal(updatedMember.ExpiryDate, result.ExpiryDate);
            Assert.Equal(updatedMember.Photo, result.Photo);
            Assert.Equal(updatedMember.MemberType, result.MemberType);
        }

        [Fact]
        public async Task DeleteMember_RemovesMemberFromDatabase()
        {
            // Arrange
            var member = new Member
            {
                SSN = "6",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                UserAddress = new Address { Street = "Main St", StreetNumber = "1", City = "City", ZipCode = "12345" },
                CardNumber = "123456",
                ExpiryDate = DateTime.UtcNow.AddYears(1),
                Photo = "photo_url",
                MemberType = "Regular"
            };

            // Act
            await _memberRepository.CreateMember(member);
            var result = await _memberRepository.DeleteMember(member.SSN);

            // Assert
            Assert.True(result);
        }
    }
}
