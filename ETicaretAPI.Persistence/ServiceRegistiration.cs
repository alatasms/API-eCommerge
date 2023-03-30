using Microsoft.EntityFrameworkCore;
using ETicaretAPI.Persistence.Contexts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Persistence.Repositories;
using ETicaretAPI.Application.Repositories.File;
using ETicaretAPI.Persistence.Repositories.File;
using ETicaretAPI.Domain.Entities.Identity;
using ETicaretAPI.Application.Abstraction.Services;
using ETicaretAPI.Persistence.Services;
using ETicaretAPI.Application.Dto_s.User;
using Microsoft.AspNetCore.Identity;
using ETicaretAPI.Persistence.Configurations.CustomProviders.Email;
using ETicaretAPI.Persistence.Configurations.CustomProviders.Password;

namespace ETicaretAPI.Persistence
{
    public static class ServiceRegistiration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ETicaretAPIDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")), ServiceLifetime.Scoped);


            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

                options.SignIn.RequireConfirmedEmail = true;

                options.Tokens.EmailConfirmationTokenProvider = "EmailConfirmationTokenProvider";
                options.Tokens.PasswordResetTokenProvider = "ResetPasswordTokenProvider";

            })
                .AddEntityFrameworkStores<ETicaretAPIDbContext>()
            .AddTokenProvider<CustomEmailConfirmationTokenProvider<AppUser>>("EmailConfirmationTokenProvider") // A custom provider that changes EmailConfirmation token settings without affecting other tokens.
            .AddTokenProvider<CustomResetPasswordTokenProvider<AppUser>>("ResetPasswordTokenProvider"); // A custom provider that changes ResetPassword token settings without affecting other tokens.

            //EmailConfirmation Token options
            services.Configure<CustomEmailConfirmationTokenProviderOptions>(o =>
            {
                o.TokenLifespan = TimeSpan.FromDays(3);
            });

            //ResetPassword Token options
            services.Configure<CustomResetPasswordTokenProviderOptions>(o =>
            {
                o.TokenLifespan = TimeSpan.FromDays(1);
            });

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();
            services.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
            services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
     

        }
    }
}
