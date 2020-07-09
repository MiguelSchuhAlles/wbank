using Banking.BackgroundTasks;
using Banking.Service.Interfaces;
using Banking.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.API
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IUserService, UserService>()
                .AddTransient<ITransferService, TransferService>()
                .AddTransient<IOperationService, OperationService>()
                .AddTransient<IAccountService, AccountService>()
                .AddTransient<IAuthService, AuthService>();
        }

        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            return services
                .AddHostedService<RemunerationService>();
        }
    }
}
