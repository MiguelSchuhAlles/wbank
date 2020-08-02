using Banking.Domain.Entities;
using Banking.Domain.Entities.Operations;
using Banking.Infrastructure;
using Banking.Service.Interfaces;
using Banking.Shared.Requests;
using Banking.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace Banking.Service.Services
{
    public class OperationService : BaseService, IOperationService
    {
        public OperationService(BankingContext context, IDistributedCache distributedCache) : base(context, distributedCache) { }

        public async Task<Response<Operation>> Deposit(OperationRequestDTO request, int userId)
        {
            var response = await this.ValidateOperation(request, userId);
            if (response.ResponseStatus == ResponseStatus.Error) return response;

            try
            {
                var account = await Context.Accounts.FirstOrDefaultAsync(a => a.Id == request.AccountId && a.UserId == userId);

                if (account == null)
                {
                    response.ResponseStatus = ResponseStatus.Error;
                    response.Message = "Invalid accont number.";
                    return response;
                }

                response.Item = await RegisterOperation(account, request.Amount, OperationType.Deposit);

                response.ResponseStatus = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.ResponseStatus = ResponseStatus.Exception;
                response.Message = $"A fatal error occurred. {ex.Message}";

                return response;
            }

            return response;
        }

        public async Task<Response<Operation>> TicketPayment(PaymentRequestDTO request, int userId)
        {
            var response = await this.ValidateOperation(request, userId);
            if (response.ResponseStatus == ResponseStatus.Error) return response;

            try
            {
                if (request.Code == null || request.Code == "")
                {
                    response.ResponseStatus = ResponseStatus.Error;
                    response.Message = "Code required.";
                    return response;
                }

                var account = await Context.Accounts.FirstOrDefaultAsync(a => a.Id == request.AccountId && a.UserId == userId);

                if (account == null)
                {
                    response.ResponseStatus = ResponseStatus.Error;
                    response.Message = "Invalid accont number.";
                    return response;
                }

                if (account.Balance < request.Amount)
                {
                    response.ResponseStatus = ResponseStatus.Error;
                    response.Message = "Insuficient funds.";
                    return response;
                }

                using (var transaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        var operation = await this.RegisterOperation(account, request.Amount, OperationType.Payment);
                        await this.CreateTicketPayment(operation.Id, request.Code);
                        response.Item = operation;
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                response.ResponseStatus = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.ResponseStatus = ResponseStatus.Exception;
                response.Message = $"A fatal error occurred. {ex.Message}";

                return response;
            }

            return response;
        }

        public async Task<Response<Operation>> Withdraw(OperationRequestDTO request, int userId)
        {
            var response = await this.ValidateOperation(request, userId);
            if (response.ResponseStatus == ResponseStatus.Error) return response;

            try
            {
                var account = await Context.Accounts.FirstOrDefaultAsync(a => a.Id == request.AccountId && a.UserId == userId);

                if (account == null)
                {
                    response.ResponseStatus = ResponseStatus.Error;
                    response.Message = "Invalid accont number.";
                    return response;
                }

                if (account.Balance < request.Amount)
                {
                    response.ResponseStatus = ResponseStatus.Error;
                    response.Message = "Insuficient funds.";
                    return response;
                }

                response.Item = await RegisterOperation(account, request.Amount, OperationType.Withdrawal);

                response.ResponseStatus = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.ResponseStatus = ResponseStatus.Exception;
                response.Message = $"A fatal error occurred. {ex.Message}";

                return response;
            }

            return response;
        }

        public async Task<Operation> RegisterOperation(Account account, decimal amount, OperationType operationType)
        {
            switch (operationType)
            {
                case OperationType.Deposit:
                case OperationType.InterestIncome:
                case OperationType.ReceivedTransference:
                case OperationType.TransferenceReversal:
                    account.Balance += amount;
                    break;
                case OperationType.PointOfSale:
                case OperationType.Transfer:
                case OperationType.Withdrawal:
                case OperationType.Charge:
                case OperationType.Payment:
                    account.Balance -= amount;
                    break;
            }

            Context.Accounts.Update(account);

            var newOperation = new Operation
            {
                Amount = amount,
                Balance = account.Balance,
                Date = DateTime.UtcNow,
                AccountId = account.Id,
                OperationType = operationType
            };

            Context.Operations.Add(newOperation);

            await Context.SaveChangesAsync();

            return newOperation;
        }
         
        private async Task<Response<Operation>> ValidateOperation(OperationRequestDTO request, int userId)
        {
            var response = new Response<Operation>() { ResponseStatus = ResponseStatus.Success };

            var user = await Context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.Password == request.Password);

            if (user == null)
            {
                response.ResponseStatus = ResponseStatus.Error;
                response.Message = "Wrong password.";
                return response;
            }

            if (request.Amount <= 0)
            {
                response.ResponseStatus = ResponseStatus.Error;
                response.Message = "Invalid ammount.";
                return response;
            }

            return response;
        }

        private async Task<TicketPayment> CreateTicketPayment(int operationId, string code)
        {
            var payment = new TicketPayment
            {
                OperationId = operationId,
                Code = code
            };

            Context.TicketPayments.Add(payment);

            await Context.SaveChangesAsync();

            return payment;
        }
    }
}
