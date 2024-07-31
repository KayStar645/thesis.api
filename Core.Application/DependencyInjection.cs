using Core.Application.Common.Interfaces;
using Core.Application.Profiles;
using Core.Application.Services;
using Core.Domain.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;
using System.Reflection;

namespace Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CommonMappingProfile());
                cfg.AddProfile(new ModuleMappingProfile());
            }).CreateMapper());

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddScoped<IPermissionService, PermissionService>();

            services.AddScoped<ISieveProcessor, SieveProcessor>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddScoped<IAmazonS3Service, AmazonS3Service>();

            services.AddScoped<ICLHUIService, CLHUIService>();

            return services;
        }
    }
}
