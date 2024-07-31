using Core.Application.Common.Interfaces;
using UI.WebApi.Services;

namespace UI.WebApi
{
    public static class DependencyInjection
    {
        public static void AddWebApiServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddEndpointsApiExplorer();
        }
    }
}
