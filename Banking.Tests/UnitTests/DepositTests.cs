using Banking.Domain.Entities;
using Banking.Infrastructure;
using Banking.Shared.Requests;
using Banking.Shared.Responses;
using NUnit.Framework;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Tests.UnitTests
{
    public class DepositTests : BaseTests
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
        public async Task DepositShouldSucceed()
        {
            // Arrange
            var request = new OperationRequestDTO()
            {
                AccountId = _account.Id,
                Amount = 10.50M,
            };

            // Act
            var previousBalance = await _accountService.GetBalance(_user.Id, _account.Id);

            var operation = await _operationService.Deposit(request, _user.Id);

            var newBalance = await _accountService.GetBalance(_user.Id, _account.Id);

            // Assert
            Assert.AreEqual(ResponseStatus.Success, operation.ResponseStatus);
            Assert.AreEqual(previousBalance + request.Amount, newBalance);
            Assert.AreEqual(operation.Item.Balance, newBalance);
        }

        [Test]
        public async Task DepositShouldFailNegativeAmount()
        {
            // Arrange
            var request = new OperationRequestDTO()
            {
                AccountId = _account.Id,
                Amount = -1
            };

            // Act
            var operation = await _operationService.Deposit(request, _user.Id);

            // Assert
            Assert.AreEqual(operation.ResponseStatus, ResponseStatus.Error);
        }

        [Test]
        public async Task DepositShouldFailInvalidAccount()
        {
            // Arrange
            var request = new OperationRequestDTO()
            {
                AccountId = 0,
                Amount = 1
            };

            // Act
            var operation = await _operationService.Deposit(request, _user.Id);

            // Assert
            Assert.AreEqual(operation.ResponseStatus, ResponseStatus.Error);
        }
    }
}
