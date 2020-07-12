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
using Banking.Shared.Responses;
using System.Threading;

namespace Banking.Service.Services
{
    public class AccountService : BaseService, IAccountService
    {
        public AccountService(BankingContext context) : base(context) { }

        public async ValueTask<Account> GetAccount(int accountId, int userId, CancellationToken ct = default)
            => await Context.Accounts
                .Include(account => account.User)
                .Include(account => account.Branch)
                .FirstOrDefaultAsync(account => account.UserId == userId && account.Id == accountId, ct);

        //TODO: create account

        //TODO: update account

        public async ValueTask<decimal> GetBalance(int accountId, int userId, CancellationToken ct = default)
        {
            var balance = await (from account in Context.Accounts
                                 where account.UserId == userId
                                   && account.Id == accountId
                                 select account.Balance).FirstOrDefaultAsync(ct);

            return balance;
        }

        public async ValueTask<IList<Operation>> GetOperationHistory(int accountId, int userId, DateTime start, Expression<Func<Operation, bool>> where = null, CancellationToken ct = default)
        {
            var operations = await (from operation in Context.Operations
                                    where operation.Account.UserId == userId
                                       && operation.AccountId == accountId
                                       && operation.Date >= start
                                       && operation.OperationType != OperationType.InterestIncome
                                    orderby operation.Date descending
                                    select operation)
                              .Where(where ?? (_ => true))
                              .Include(o => o.Account)
                              .ToListAsync(ct);

            return operations;
        }

        public async ValueTask<IList<TimeSeriesDataPoint<decimal>>> GetInterestByMonth(int accountId, int userId, DateTime start, CancellationToken ct = default)
        {
            var points = (await Context.Operations
                .Where(o => o.OperationType == OperationType.InterestIncome && o.AccountId == accountId && o.Date >= start)
                .GroupBy(o => new { Date = new DateTime(o.Date.Year, o.Date.Month, 1) })
                .Select(g => new TimeSeriesDataPoint<decimal>
                {
                    Timestamp = g.Key.Date,
                    Value = g.Sum(o => o.Amount)
                }).ToListAsync(ct)).OrderBy(o => o.Timestamp);

            return points.ToList();

        }
    }
}
