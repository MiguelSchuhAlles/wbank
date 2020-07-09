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
    [TestFixture]
    public class WithdrawTests : BaseTests
    {
        private User _user;
        private Account _account;

        [OneTimeSetUp]
        public async Task Setup()
        {
            SeedUsers();
            SeedBranches();
            SeedAccounts();

            _user = (await _userService.FindAll()).FirstOrDefault();
            _account = await _accountService.GetAccount(_user.Id, _user.Account.Id);
        }

        [Test]
        public async Task WithdrawShouldSucceed()
        {
            // Arrange
            var request = new OperationRequestDTO()
            {
                AccountId = _account.Id,
                Amount = 10.50M
            };

            // Act
            var previousBalance = await _accountService.GetBalance(_user.Id, _account.Id);

            var operation = await _operationService.Withdraw(request, _user.Id);

            var newBalance = await _accountService.GetBalance(_user.Id, _account.Id);

            // Assert
            Assert.AreEqual(ResponseStatus.Success, operation.ResponseStatus);
            Assert.AreEqual(previousBalance - request.Amount, newBalance);
            Assert.AreEqual(operation.Item.Balance, newBalance);
        }

        [Test]
        public async Task WithdrawShouldFailNegativeAmount()
        {
            // Arrange
            var request = new OperationRequestDTO()
            {
                AccountId = _account.Id,
                Amount = -1
            };

            // Act
            var operation = await _operationService.Withdraw(request, _user.Id);

            // Assert
            Assert.AreEqual(operation.ResponseStatus, ResponseStatus.Error);
        }

        [Test]
        public async Task WithdrawShouldFailInvalidAccount()
        {
            // Arrange
            var request = new OperationRequestDTO()
            {
                AccountId = 0,
                Amount = 1
            };

            // Act
            var operation = await _operationService.Withdraw(request, _user.Id);

            // Assert
            Assert.AreEqual(operation.ResponseStatus, ResponseStatus.Error);
        }

        [Test]
        public async Task WithdrawShouldFailInsuficientFunds()
        {
            // Arrange
            var previousBalance = await _accountService.GetBalance(_user.Id, _account.Id);

            var request = new OperationRequestDTO()
            {
                AccountId = 0,
                Amount = previousBalance + 1
            };

            // Act
            var operation = await _operationService.Withdraw(request, _user.Id);

            // Assert
            Assert.AreEqual(operation.ResponseStatus, ResponseStatus.Error);
        }
    }
}
