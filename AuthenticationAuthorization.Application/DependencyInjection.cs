using AuthenticationAuthorization.Application.Mapping;
using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection; // Add this using directive at the top of the file

namespace AuthenticationAuthorization.Application
{
    public static class DependencyInjection // Fix the class declaration
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly(); // Or load another if needed

            var types = assembly.GetTypes();

            //var interfaceTypes = types.Where(t => t.IsInterface && t.Name.StartsWith("I")).ToList();
            var interfaces = types
                .Where(t => t.IsInterface && t.Name.StartsWith("I") && t.Name.EndsWith("Service"))
                .ToList();

            foreach (var interfaceType in interfaces)
            {
                var implName = interfaceType.Name.Substring(1); // remove 'I'
                var implementationType = types.FirstOrDefault(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    t.Name == implName &&
                    interfaceType.IsAssignableFrom(t));

                if (implementationType != null)
                {
                    services.AddScoped(interfaceType, implementationType);
                }
            }
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                cfg.NotificationPublisher = new ForeachAwaitPublisher();
            });
            // Automapper , register profiles in current assembly only
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }
    }
}
