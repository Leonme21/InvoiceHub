using FluentValidation;
using InvoiceHub.Application.Interfaces;
using InvoiceHub.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceHub.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IInvoiceService, InvoiceService>();

            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}
