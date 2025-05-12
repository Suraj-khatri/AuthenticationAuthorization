using AuthenticationAuthorization.Domain.Options;
using AuthenticationAuthorization.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AuthenticationAuthorization.Domain.Common.Interfaces;
using AuthenticationAuthorization.Domain.Interfaces;
using AuthenticationAuthorization.Infrastructure.Repository;
using AuthenticationAuthorization.Infrastructure.Services;

namespace AuthenticationAuthorization.Infrastructure
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>((provider, options) =>
            {
                // Ensure the Microsoft.EntityFrameworkCore.SqlServer package is installed
                options.UseSqlServer(provider.GetRequiredService<IOptionsSnapshot<ConnectionStringOption>>().Value.DefaultConnection);
            });

            var domainAssembly = Assembly.Load("AuthenticationAuthorization.Domain");
            var applicationAssembly = Assembly.Load("AuthenticationAuthorization.Application");

            var interfaceTypes = domainAssembly.GetTypes()
                .Where(t => t.IsInterface && t.Name.StartsWith("I") && t.Name.EndsWith("Repo"))
                .ToList();

            var implementationTypes = applicationAssembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (var interfaceType in interfaceTypes)
            {
                var implName = interfaceType.Name.Substring(1);
                var implementationType = implementationTypes
                    .FirstOrDefault(t => t.Name == implName && interfaceType.IsAssignableFrom(t));

                if (implementationType != null)
                {
                    services.AddScoped(interfaceType, implementationType);
                }
            }
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
