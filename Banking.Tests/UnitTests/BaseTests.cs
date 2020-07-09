using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Banking.Service.Interfaces;
using Banking.Service.Services;
using Banking.Infrastructure;
using Banking.Shared;
using System.Linq;
using System.Collections.Generic;
using Banking.Domain.Entities;
using Banking.Domain.Entities.Operations;
using System;

namespace Banking.Tests.UnitTests
{
    public class BaseTests
    {
        public ServiceProvider _serviceProvider;
        public BankingContext Context { get; set; }

        public IOperationService _operationService;
        public IAccountService _accountService;
        public IUserService _userService;

        [OneTimeSetUp]
        public void BaseSetup()
        {
            var services = new ServiceCollection();

            services.AddDbContext<BankingContext>
                (options => options
                    .UseInMemoryDatabase("banking")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                );

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IOperationService, OperationService>();
            services.AddTransient<IAccountService, AccountService>();

            services.AddSingleton(_ => new Settings() { Secret = "" });

            _serviceProvider = services.BuildServiceProvider();

            Context = _serviceProvider.GetService<BankingContext>();
            _operationService = _serviceProvider.GetRequiredService<IOperationService>();
            _accountService = _serviceProvider.GetRequiredService<IAccountService>();
            _userService = _serviceProvider.GetRequiredService<IUserService>();
        }

        protected void SeedUsers()
        {
            if (!Context.Users.Any())
            {
                var users = new List<User>
                {
                    new User { Id = 1, Name = "Admin", Phone = "+55 51 987654321", Password = "admin", Email = "admin@gmail.com", Enabled = true },
                    new User { Id = 2, Name = "Miguel", Phone = "+55 51 987654321", Password = "miguel", Email = "miguel@gmail.com", Enabled = true }
                };

                Context.AddRange(users);
                Context.SaveChanges();
            }
        }

        protected void SeedBranches()
        {
            if (!Context.Branches.Any())
            {
                var branches = new List<Branch>
                {
                    new Branch { Id = 1, Number = 1234 },
                    new Branch { Id = 2, Number = 4567 }
                };

                Context.AddRange(branches);
                Context.SaveChanges();
            }
        }

        protected void SeedAccounts()
        {
            if (!Context.Accounts.Any())
            {
                var accounts = new List<Account>
                {
                    new Account { Id = 1, Code = "123456", UserId = 1, BranchId = 1, Enabled = true, Balance = 50000m, Rentabilize = false },
                    new Account { Id = 2, Code = "122334", UserId = 2, BranchId = 1, Enabled = true, Balance = 10000m, Rentabilize = true }
                };

                Context.AddRange(accounts);
                Context.SaveChanges();
            }
        }

        protected void SeedTransactions()
        {
            if (!Context.Operations.Any())
            {
                foreach (var account in Context.Accounts)
                {
                    var operations = new List<Operation>
                    {
                        new Operation { OperationType = OperationType.Deposit, Amount = 50000, AccountId = account.Id, Description = "", Balance = account.Balance + 50000, Date = DateTime.Now },
                        new Operation { OperationType = OperationType.Withdrawal, Amount = 10000, AccountId = account.Id, Description = "", Balance = account.Balance - 10000, Date = DateTime.Now },
                        new Operation { OperationType = OperationType.Payment, Amount = 5000, AccountId = account.Id, Description = "", Balance = account.Balance - 5000, Date = DateTime.Now },
                    };

                    Context.AddRange(operations);
                }
                Context.SaveChanges();
            }
        }
    }
}