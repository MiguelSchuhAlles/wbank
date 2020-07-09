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
    public class UserTests : BaseTests
    {
        private User _user;

        [OneTimeSetUp]
        public async Task Setup()
        {
            SeedUsers();
            SeedBranches();
            SeedAccounts();

            _user = await Context.Users.Include(u => u.Account).FirstOrDefaultAsync();
        }

        [Test]
        public async Task GetUserShouldSucceed()
        {
            // Arrange
            var id = _user.Id;

            // Act
            var user = await _userService.FindById(id);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreSame(_user, user);
        }

        [Test]
        public async Task GetUsersShouldSucceed()
        {
            // Arrange

            // Act
            var users = await _userService.FindAll();

            // Assert
            Assert.IsNotEmpty(users);
            Assert.Greater(users.Count, 0);
        }
    }
}
