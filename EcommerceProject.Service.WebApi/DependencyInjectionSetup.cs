using EcommerceProject.Application.Interface;
using EcommerceProject.Application.Main;
using EcommerceProject.Domain.Core;
using EcommerceProject.Domain.Interface;
using EcommerceProject.Infrastructure.Data;
using EcommerceProject.Infrastructure.Interface;
using EcommerceProject.Infrastructure.Repository;
using EcommerceProject.Transversal.Common;
using EcommerceProject.Transversal.Mapper;

namespace EcommerceProject.Service.WebApi
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAutoMapper(x => x.AddProfile(new MappingsProfile()));
            services.AddMvc();
            services.AddSingleton<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<ICustomersDomain, CustomersDomain>();
            services.AddScoped<ICustomersApplication, CustomersApplication>();
            services.AddScoped<ICustomersRepository, CustomerRepository>();
            return services;
        }
    }
}
