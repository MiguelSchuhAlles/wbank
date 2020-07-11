using Banking.BackgroundTasks;
using Banking.Service.Interfaces;
using Banking.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

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

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "WBank",
                    Description = "WBank API",
                    Contact = new OpenApiContact() { Name = "Miguel Schuh Alles", Email = "miguelschuhalles@gmail.com" }
                });
            });
        }
    }
}
