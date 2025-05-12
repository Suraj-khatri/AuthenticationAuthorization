using AuthenticationAuthorization.Application;
using AuthenticationAuthorization.Domain;
using AuthenticationAuthorization.Infrastructure;
namespace AuthenticationAuthorization.API
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddApiDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationDI() 
                .AddInfrastructureDI() 
                .AddDomainDI(configuration); 
            return services;
        }
    }
}
