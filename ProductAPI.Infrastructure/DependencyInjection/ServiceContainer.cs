using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductAPI.Domain.Interface;
using ProductAPI.Infrastructure.DbContexts;
using ProductAPI.Infrastructure.ProductRepository;

namespace ProductAPI.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, configuration, configuration["MySeriLog:Filename"] ?? throw new Exception("FilePath for logging missing in config file"));
            services.AddScoped<IProduct, ProductRepo>();
            return services;
        }
        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
