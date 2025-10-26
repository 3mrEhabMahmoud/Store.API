using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.Services.Abstractions;
using Store.Services.Mapping.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ApplicationServices (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IServiceManager, ServiceManager>();

            services.AddAutoMapper(M => M.AddProfile(new ProductProfile(configuration)));
            return services;
        }
    }
}
