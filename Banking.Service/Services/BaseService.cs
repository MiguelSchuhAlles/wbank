using Banking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Service.Services
{
    public abstract class BaseService
    {
        public BankingContext Context { get; set; }

        public BaseService(BankingContext context)
        {
            this.Context = context;
        }
    }
}
