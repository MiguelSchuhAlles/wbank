using Banking.Domain.Entities;
using Banking.Domain.Entities.Operations;
using Banking.Shared.Requests;
using Banking.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Service.Interfaces
{
    public interface IAccountService
    {
        ValueTask<Account> GetAccount(int accountId, int userId, CancellationToken ct = default);
        ValueTask<decimal> GetBalance(int accountId, int userId, CancellationToken ct = default);
        ValueTask<IList<Operation>> GetOperationHistory(int accountId, int userId, DateTime start, Expression<Func<Operation, bool>> where = null, CancellationToken ct = default);
        ValueTask<IList<TimeSeriesDataPoint<decimal>>> GetInterestByMonth(int accountId, int userId, DateTime start, CancellationToken ct = default);
    }
}
