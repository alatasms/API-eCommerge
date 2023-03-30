using ETicaretAPI.Application.Dto_s.User;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application
{
    public static class ServisRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ServisRegistration));
            services.AddHttpClient();
            services.AddAutoMapper(typeof(CreateUser));
        }
    }
}
