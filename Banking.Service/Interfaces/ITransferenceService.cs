using Banking.Domain.Entities.Operations;
using Banking.Shared.Requests;
using Banking.Shared.Responses;
using System.Threading.Tasks;

namespace Banking.Service.Interfaces
{
    public interface ITransferenceService
    {
        Task<Response<Transference>> Transference(TransferenceRequestDTO request, int userId);
    }
}
