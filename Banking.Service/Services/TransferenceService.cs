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
    public class TransferenceService : BaseService, ITransferenceService
    {
        public TransferenceService(BankingContext context, IDistributedCache distributedCache) : base(context, distributedCache) { }

        public async Task<Response<Transference>> Transference(TransferenceRequestDTO request, int userId)
        {
            var response = new Response<Transference>() { ResponseStatus = ResponseStatus.Success };

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

                response.Item = await RegisterTransference(request, account);
                response.ResponseStatus = ResponseStatus.Success;
                response.Message = "Transference has been scheduled successfully.";
            }
            catch (Exception ex)
            {
                response.ResponseStatus = ResponseStatus.Exception;
                response.Message = $"A fatal error occurred. {ex.Message}";

                return response;
            }

            return response;
        }

        private async Task<Transference> RegisterTransference(TransferenceRequestDTO request, Account account)
        {
            //setting new balance
            account.Balance -= request.Amount;
            Context.Accounts.Update(account);

            //registering operation
            var newOperation = new Operation
            {
                Amount = request.Amount,
                Balance = account.Balance,
                Date = DateTime.UtcNow,
                AccountId = account.Id,
                OperationType = OperationType.Transfer
            };

            Context.Operations.Add(newOperation);

            //creating a pending transference (to be processed)
            var newTransference = new Transference
            {
                Status = TransferenceStatus.Pending,
                Amount = request.Amount,
                AccountId = account.Id,
                Description = request.Description,
                Date = DateTime.UtcNow,
                SameTitularity = request.SameTitularity,
                RecipientName = request.RecipientName,
                RecipientCode = request.RecipientCode,
                RecipientAccountCode = request.RecipientAccountCode,
                RecipientBranchNumber = request.RecipientBranchNumber,
                RecipientBankNumber = request.RecipientBankNumber
            };

            Context.Transferences.Add(newTransference);

            await Context.SaveChangesAsync();

            return newTransference;
        }
    }
}
