using Application.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyIngections
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var conectionDb = configuration.GetConnectionString("PostgreSql");

            services.AddDbContext<ChatAppDbContext>(builder =>
                builder.UseNpgsql(conectionDb));

            services.AddScoped<IChatDbContext, ChatAppDbContext>();

            return services;
        }
    }
}
