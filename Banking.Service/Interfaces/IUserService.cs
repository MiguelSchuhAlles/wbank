﻿using Banking.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Service.Interfaces
{
    public interface IUserService
    {
        ValueTask<IList<User>> GetAll(CancellationToken ct = default);

        ValueTask<User> GetById(int id, CancellationToken ct = default);
    }
}
