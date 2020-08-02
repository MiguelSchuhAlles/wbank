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
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Banking.Service.Services
{
    public class AccountService : BaseService, IAccountService
    {
        public AccountService(BankingContext context, IDistributedCache distributedCache) : base(context, distributedCache) { }

        public async ValueTask<Account> GetAccount(int accountId, int userId, CancellationToken ct = default)
        {
            var cacheAccount = _distributedCache.GetString($"account:{accountId}");
            Account account;

            if (cacheAccount != null)
            {
                account = JsonSerializer.Deserialize<Account>(cacheAccount, _serializationOptions);
            }
            else
            {
                account = await Context.Accounts
                            .Include(a => a.User)
                            .Include(a => a.Branch)
                            .FirstOrDefaultAsync(a => a.UserId == userId && a.Id == accountId, ct);

                _distributedCache.SetString($"account:{account.Id}", JsonSerializer.Serialize(account, _serializationOptions));
            }

            return account;
        }

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
