using AuthenticationAuthorization.Domain.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddDomainDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConnectionStringOption>(configuration.GetSection(ConnectionStringOption.SectionName));
            services.Configure<JwtOption>(configuration.GetSection(JwtOption.SectionName));

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // Replace AddHttpContextAccessor with TryAddSingleton

            return services;
        }
    }
}
