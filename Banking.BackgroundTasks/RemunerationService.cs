using Banking.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Banking.Service.Interfaces;

namespace Banking.BackgroundTasks
{
    public class RemunerationService : IHostedService
    {
        public const decimal AnnualInterestRate = 0.0215M;

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RemunerationService> _logger;

        private CompositeDisposable _disposables;

        public RemunerationService(IServiceScopeFactory factory, ILogger<RemunerationService> logger)
        {
            this._scopeFactory = factory;
            this._logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting remuneration service...");

            _disposables?.Dispose();
            _disposables = new CompositeDisposable();
            _disposables.Add(this.ScheduleRemuneration());

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping remuneration service...");

            _disposables?.Dispose();

            return Task.CompletedTask;
        }

        private IDisposable ScheduleRemuneration()
        {
            return TaskPoolScheduler.Default.ScheduleAsync(async (s, ct) =>
            {
                while (!ct.IsCancellationRequested)
                {
                    //TODO: implement remuneration just in business days

                    var now = DateTime.Now;
                    var midnight = DateTime.Today.AddDays(1);

                    Thread.Sleep(midnight.Subtract(now));

                    _logger.LogInformation($"Calculating daily remuneration for all accounts.");

                    await GenerateInterest();
                }
            });
        }

        private async Task GenerateInterest()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BankingContext>();
                var operationService = scope.ServiceProvider.GetRequiredService<IOperationService>();

                var accounts = await (from account in context.Accounts
                                      where account.Enabled 
                                        && account.Rentabilize
                                        && account.Balance > 0
                                      select account).ToListAsync();

                foreach(var account in accounts)
                {
                    try
                    {
                        //TODO: Income tax and IOF

                        var amount = account.Balance * (AnnualInterestRate / 365);
                        var operation = await operationService.RegisterOperation(account, amount, Domain.Entities.Operations.OperationType.InterestIncome);

                        _logger.LogInformation($"Added R$ {amount} of (daily interest) to account Id = {account.Id}. New balance = {account.Balance}.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Failed to rentabilize account Id = {account.Id}. Ex: {ex.Message}");
                    }
                }
            }
        }
    }
}
