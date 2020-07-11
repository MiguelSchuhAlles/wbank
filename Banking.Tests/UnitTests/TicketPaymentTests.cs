using Banking.Domain.Entities;
using Banking.Shared.Requests;
using Banking.Shared.Responses;
using NUnit.Framework;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Tests.UnitTests
{
    [TestFixture]
    public class TicketPaymentTests : BaseTests
    {
        private User _user;
        private Account _account;

        [OneTimeSetUp]
        public async Task Setup()
        {
            SeedUsers();
            SeedBranches();
            SeedAccounts();

            _user = (await _userService.GetAll()).FirstOrDefault();
            _account = await _accountService.GetAccount(_user.Id, _user.Account.Id);
        }

        [Test]
        public async Task TicketPaymentShouldSucceed()
        {
            // Arrange
            var request = new PaymentRequestDTO()
            {
                AccountId = _account.Id,
                Amount = 10.50M,
                Code = "0123456789"
            };

            // Act
            var previousBalance = await _accountService.GetBalance(_user.Id, _account.Id);

            var operation = await _operationService.TicketPayment(request, _user.Id);

            var newBalance = await _accountService.GetBalance(_user.Id, _account.Id);

            // Assert
            Assert.AreEqual(ResponseStatus.Success, operation.ResponseStatus);
            Assert.AreEqual(previousBalance - request.Amount, newBalance);
            Assert.AreEqual(operation.Item.Balance, newBalance);
        }

        [Test]
        public async Task TicketPaymentShouldFailNegativeAmount()
        {
            // Arrange
            var request = new PaymentRequestDTO()
            {
                AccountId = _account.Id,
                Amount = -1,
                Code = "0123456789"
            };

            // Act
            var operation = await _operationService.TicketPayment(request, _user.Id);

            // Assert
            Assert.AreEqual(operation.ResponseStatus, ResponseStatus.Error);
        }

        [Test]
        public async Task TicketPaymentShouldFailInvalidAccount()
        {
            // Arrange
            var request = new PaymentRequestDTO()
            {
                AccountId = 0,
                Amount = 1,
                Code = "0123456789"
            };

            // Act
            var operation = await _operationService.TicketPayment(request, _user.Id);

            // Assert
            Assert.AreEqual(operation.ResponseStatus, ResponseStatus.Error);
        }

        [Test]
        public async Task TicketPaymentShouldFailInvalidCode()
        {
            // Arrange
            var request = new PaymentRequestDTO()
            {
                AccountId = 0,
                Amount = 1,
                Code = ""
            };

            // Act
            var operation = await _operationService.TicketPayment(request, _user.Id);

            // Assert
            Assert.AreEqual(operation.ResponseStatus, ResponseStatus.Error);
        }

        [Test]
        public async Task TicketPaymentShouldFailInsuficientFunds()
        {
            // Arrange
            var previousBalance = await _accountService.GetBalance(_user.Id, _account.Id);

            var request = new PaymentRequestDTO()
            {
                AccountId = 0,
                Amount = previousBalance + 1,
                Code = "0123456789"
            };

            // Act
            var operation = await _operationService.TicketPayment(request, _user.Id);

            // Assert
            Assert.AreEqual(operation.ResponseStatus, ResponseStatus.Error);
        }
    }
}

