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
using Banking.Domain.Entities.Operations;
using Google.Protobuf.WellKnownTypes;
using Banking.Service.Interfaces;

namespace Banking.BackgroundTasks
{
    public class TransferenceProcessmentService : IHostedService
    {
        public const int WBankNumber = 987;

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TransferenceProcessmentService> _logger;

        private CompositeDisposable _disposables;

        public TransferenceProcessmentService(IServiceScopeFactory factory, ILogger<TransferenceProcessmentService> logger)
        {
            this._scopeFactory = factory;
            this._logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting transference processment service...");

            _disposables?.Dispose();
            _disposables = new CompositeDisposable();
            _disposables.Add(this.ScheduleTransferenceProcessment());

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping transference processment service...");

            _disposables?.Dispose();

            return Task.CompletedTask;
        }

        private IDisposable ScheduleTransferenceProcessment()
        {
            return TaskPoolScheduler.Default.ScheduleAsync(async (s, ct) =>
            {
                while (!ct.IsCancellationRequested)
                {
                    //TODO: process transferences just in business days and horaries

                    Thread.Sleep(TimeSpan.FromMinutes(5));

                    _logger.LogInformation($"Processing transferences.");

                    await ProcessTransferences();
                }
            });
        }

        private async Task ProcessTransferences()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BankingContext>();

                var pendingTransferences = await (from transference in context.Transferences
                                      where transference.Status == TransferenceStatus.Pending
                                      select transference).Include(t => t.Account).ToListAsync();

                foreach (var transference in pendingTransferences)
                {
                    try
                    {
                        //TODO: use transaction

                        if (transference.RecipientBankNumber == WBankNumber)
                        {
                            var successful = await RegisterTransference(transference);

                            if (successful)
                            {
                                transference.Status = TransferenceStatus.Processed;
                                _logger.LogInformation($"Transference Id = {transference.Id} processed successfully.");
                            }
                            else
                            {
                                transference.Status = TransferenceStatus.Rejected;
                                _logger.LogInformation($"Transference Id = {transference.Id} failed and has been rejected.");
                            }  
                        }
                        else
                        {
                            if(transference.RecipientName == "" 
                                || transference.RecipientCode == ""
                                || transference.RecipientAccountCode == ""
                                || transference.RecipientBranchNumber == 0
                                || transference.RecipientBankNumber == 0)
                            {
                                transference.Status = TransferenceStatus.Rejected;
                                await ReverseTransference(transference);

                                _logger.LogInformation($"Transference Id = {transference.Id} rejected because of incorrect recipient data.");
                            }
                            else
                            {
                                transference.Status = TransferenceStatus.Processed;

                                //Here would send it to other bank

                                _logger.LogInformation($"Transference Id = {transference.Id} processed successfully.");
                            }
                        }

                        context.Transferences.Update(transference);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Failed to process transference Id = {transference.Id}. Ex: {ex.Message}");
                    }
                }
            }
        }

        private async Task<bool> RegisterTransference(Transference transference)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BankingContext>();
                var operationService = scope.ServiceProvider.GetRequiredService<IOperationService>();

                try
                {
                    var account = await context.Accounts.FirstOrDefaultAsync(a => a.Code == transference.RecipientAccountCode && a.Branch.Number == transference.RecipientBranchNumber);
                    if (account == null) return false;

                    var operaton = await operationService.RegisterOperation(account, transference.Amount, OperationType.ReceivedTransference);
                    if (operaton == null) return false;

                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Exception during finalization of Transference Id = {transference.Id}. Ex: {ex.Message}");
                    return false;
                }
            }
        }

        private async Task ReverseTransference(Transference transference)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BankingContext>();
                var operationService = scope.ServiceProvider.GetRequiredService<IOperationService>();

                await operationService.RegisterOperation(transference.Account, transference.Amount, OperationType.TransferenceReversal);
            }
        }
    }
}
