using Core.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.BusinessData.Interceptors;
using Persistence.BusinessData.Services;


namespace Persistence.BusinessData
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceBusinessDataServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<EntitySaveChangesInterceptor>();

            services.AddDbContext<ISupermarketDbContext, SupermarketDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("SMWConnect"), builder =>
                {
                    builder.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName);
                    builder.EnableRetryOnFailure();
                }));

            services.AddScoped<SupermarketDbContextInitialiser>();

            services.AddSingleton<IDateTimeService, DateTimeService>();

            return services;
        }
    }

}
