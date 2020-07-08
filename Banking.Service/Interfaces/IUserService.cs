using Banking.Domain.Entities;
using Banking.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Service.Interfaces
{
    public interface IUserService
    {
        ValueTask<IList<User>> FindAll(CancellationToken ct = default);

        ValueTask<User> FindById(int id, CancellationToken ct = default);
    }
}
