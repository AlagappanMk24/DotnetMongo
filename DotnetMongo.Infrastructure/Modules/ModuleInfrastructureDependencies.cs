using DotnetMongo.Application.Contracts.Persistence;
using DotnetMongo.Application.Contracts.Services;
using DotnetMongo.Application.Services;
using DotnetMongo.Infrastructure.Repositories;
using DotnetMongo.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetMongo.Infrastructure.Modules
{
    public static class ModuleInfrastructureDependencies
    {
        public static void AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderItemService,OrderItemService>();   
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}
