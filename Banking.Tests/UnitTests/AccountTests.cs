using Banking.Domain.Entities;
using Banking.Domain.Entities.Operations;
using Banking.Infrastructure;
using Banking.Shared.Requests;
using Banking.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Tests.UnitTests
{
    [TestFixture]
    public class AccountTests : BaseTests
    {
        private User _user;

        [OneTimeSetUp]
        public async Task Setup()
        {
            SeedUsers();
            SeedBranches();
            SeedAccounts();
            SeedTransactions();

            _user = await Context.Users.Include(u => u.Account).FirstOrDefaultAsync();
        }

        [Test]
        public async Task GetAccountShouldSucceed()
        {
            // Arrange
            var id = _user.Account.Id;

            // Act
            var account = await _accountService.GetAccount(id, _user.Id);

            // Assert
            Assert.IsNotNull(account);
            Assert.AreSame(_user.Account, account);
        }

        [Test]
        public async Task GetAccountShouldFailWrongId()
        {
            // Arrange
            var id = 0;

            // Act
            var account = await _accountService.GetAccount(id, _user.Id);

            // Assert
            Assert.IsNull(account);
        }

        [Test]
        public async Task GetBalanceShouldSucceed()
        {
            // Arrange
            var id = _user.Account.Id;

            // Act
            var balance = await _accountService.GetBalance(id, _user.Id);

            // Assert
            Assert.AreEqual(_user.Account.Balance, balance);
        }

        [Test]
        public async Task GetBalanceShouldFailWrongId()
        {
            // Arrange
            var id = 0;

            // Act
            var balance = await _accountService.GetBalance(id, _user.Id);

            // Assert
            Assert.AreEqual(0, balance);
        }

        [Test]
        public async Task GetOperationHistoryShouldSucceed()
        {
            // Arrange
            var start = DateTime.Now.Date.AddDays(-1);
            var id = _user.Account.Id;


            // Act
            var history = await _accountService.GetOperationHistory(id, _user.Id, start);

            // Assert
            Assert.IsNotEmpty(history);
            Assert.Greater(history.Count(), 0);
        }

        [Test]
        public async Task GetOperationHistoryShouldFailWrongId()
        {
            // Arrange
            var start = DateTime.Now.Date.AddDays(-1);
            var id = 0;


            // Act
            var history = await _accountService.GetOperationHistory(id, _user.Id, start);

            // Assert
            Assert.IsEmpty(history);
        }

        [Test]
        public async Task GetOperationHistoryShouldFailWrongDate()
        {
            // Arrange
            var start = DateTime.Now;
            var id = _user.Account.Id;


            // Act
            var history = await _accountService.GetOperationHistory(id, _user.Id, start);

            // Assert
            Assert.IsEmpty(history);
        }

        [Test]
        public async Task GetOperationHistoryShouldIncludeNewOperation()
        {
            // Arrange
            var start = DateTime.Now;
            var id = _user.Account.Id;


            // Act
            var newOperation = new Operation { OperationType = OperationType.Transfer, Amount = 1, AccountId = _user.Account.Id, Description = "", Balance = _user.Account.Balance + 1, Date = DateTime.Now };
            Context.Add(newOperation);
            Context.SaveChanges();

            var history = await _accountService.GetOperationHistory(id, _user.Id, start);

            // Assert
            Assert.That(history.Any(o => o.Equals(newOperation)));
        }
    }
}
