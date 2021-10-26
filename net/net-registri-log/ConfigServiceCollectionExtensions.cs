using Microsoft.Extensions.Configuration;
using net_registri_log;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MyConfigServiceCollectionExtensions
    {
        public static IServiceCollection AddNetRegistriLog(this IServiceCollection services, string connectionString)
        {
            services.AddHttpContextAccessor();

            services.AddDbContext<RegistriLogDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}
