using net_registri_log;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MyConfigServiceCollectionExtensions
    {
        public static IServiceCollection AddNetRegistriLog(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddDbContext<RegistriLogDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("NetRegistriLog"));
            });

            services.AddSingleton<net_registri_log.ApiLog.Models.Options>(GetApiLogOptions(configuration));
            return services;
        }

        private static net_registri_log.ApiLog.Models.Options GetApiLogOptions(IConfiguration configuration)
            => configuration.GetSection("net-registri-log:ApiLog.Options").Get<net_registri_log.ApiLog.Models.Options>();
        
    }
}
