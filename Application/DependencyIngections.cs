using System.Reflection;
using Application.Options;
using Application.Servises;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyIngections
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

            services.AddTransient<DateTimeService>();

            return services;
        }
    }
}
