using Banking.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Banking.Service.Services
{
    public abstract class BaseService
    {
        public BankingContext Context { get; set; }

        protected readonly IDistributedCache _distributedCache;

        protected readonly JsonSerializerOptions _serializationOptions;

        public BaseService(BankingContext context, IDistributedCache distributedCache)
        {
            this.Context = context;

            _distributedCache = distributedCache;

            _serializationOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

        }
    }
}
