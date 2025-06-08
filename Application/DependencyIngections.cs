using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyIngections
    {
        public static IServiceCollection AddApliication(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
