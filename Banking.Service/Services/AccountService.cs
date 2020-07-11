using Banking.Domain.Entities;
using Banking.Domain.Entities.Operations;
using Banking.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.Service.Interfaces;
using System.Linq.Expressions;

namespace Banking.Service.Services
{
    public class AccountService : BaseService, IAccountService
    {
        public AccountService(BankingContext context) : base(context) { }

        public async ValueTask<Account> GetAccount(int userId, int accountId)
            => await this.Context.Accounts
                .Include(account => account.User)
                .Include(account => account.Branch)
                .FirstOrDefaultAsync(account => account.UserId == userId && account.Id == accountId);

        //TODO: create account

        //TODO: update account

        public async ValueTask<decimal> GetBalance(int userId, int accountId)
        {
            var balance = await (from account in this.Context.Accounts
                                 where account.UserId == userId
                                   && account.Id == accountId
                                 select account.Balance).FirstOrDefaultAsync();

            return balance;
        }

        public async ValueTask<IList<Operation>> GetOperationHistory(int userId, int accountId, DateTime start, Expression<Func<Operation, bool>> where = null)
        {
            var operations = await (from operation in this.Context.Operations
                                    where operation.Account.UserId == userId
                                       && operation.AccountId == accountId
                                       && operation.Date >= start
                                       && operation.OperationType != OperationType.InterestIncome
                                    orderby operation.Date
                                    select operation)
                              .Where(where ?? (_ => true))
                              .Include(o => o.Account)
                              .ToListAsync();

            return operations;
        }

        public async ValueTask<IList<Operation>> GetInterestByMonth(int userId, int accountId, DateTime start)
        {
            throw new NotImplementedException();
        }
    }
}
