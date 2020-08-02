using Banking.Domain.Entities;
using Banking.Infrastructure;
using Banking.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Service.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(BankingContext context, IDistributedCache distributedCache) : base(context, distributedCache) { }

        public async ValueTask<IList<User>> GetAll(CancellationToken ct = default)
            => await Context.Users.Include(u => u.Account).ToListAsync(ct);

        public async ValueTask<User> GetById(int id, CancellationToken ct = default)
            => await Context.Users.Include(u => u.Account).FirstOrDefaultAsync(u => u.Id == id, ct);

        //TODO: create user

        //TODO: update user
    }
}
