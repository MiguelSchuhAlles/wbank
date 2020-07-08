using Banking.Shared.Responses;
using System.Threading.Tasks;

namespace Banking.Service.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticationResponse> Authenticate(string email, string password);
    }
}
