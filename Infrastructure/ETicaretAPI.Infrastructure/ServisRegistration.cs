using ETicaretAPI.Application.Abstraction.Services;
using ETicaretAPI.Application.Abstraction.Storage;
using ETicaretAPI.Application.Abstraction.Token;
using ETicaretAPI.Infrastructure.enums;
using ETicaretAPI.Infrastructure.Services.Mail;
using ETicaretAPI.Infrastructure.Services.Storage;
using ETicaretAPI.Infrastructure.Services.Storage.Azure;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using ETicaretAPI.Infrastructure.Services.Token.JWT;
using Microsoft.Extensions.DependencyInjection;

namespace ETicaretAPI.Infrastructure
{
    public static class  ServisRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IStorageService, StorageService>(); //Storage unit service
            services.AddScoped<ITokenHandler, JwtHandler>(); //Jwt creation service
            services.AddScoped<IMailService, EmailService>(); //Email Service
            services.AddScoped<ITokenFactory, TokenFactory>(); // Refresh Token generation service
        }

        public static void AddStorage<T>(this IServiceCollection services) where T: Storage, IStorage
        {
            services.AddScoped<IStorage, T>();
        }

        public static void AddStorage(this IServiceCollection services, StorageType storageType) 
        {
            switch (storageType)
            {
                case StorageType.Local:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    services.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageType.Aws:
                    break;
                default:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }
    }
}
