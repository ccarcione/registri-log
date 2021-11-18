using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using net_registri_log.ApiLog.Middleware;
using net_registri_log.Logs.Middleware;
using System.Linq;

namespace net_registri_log.Providers
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseNetRegistriLog(this IApplicationBuilder app)
        {
            UpdateDatabaseMigrate<RegistriLogDbContext>(app);
            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            
            Logs.Models.Options logsOptions = scope.ServiceProvider.GetRequiredService<Logs.Models.Options>();
            if (logsOptions.Enable)
            {
                app.UseMiddleware<EnrichLogMiddleware>();
            }
            
            ApiLog.Models.Options apiLogOptions = scope.ServiceProvider.GetRequiredService<ApiLog.Models.Options>();
            if (apiLogOptions.Enable)
            {
                app.UseMiddleware<ApiLogMiddleware>();
            }

            return app;
        }

        public static async void UpdateDatabaseMigrate<T>(IApplicationBuilder app) where T : DbContext
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<T>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<T>>();

                string databaseName = context.Database.GetDbConnection().Database;

                if (!await (context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).ExistsAsync())
                {
                    logger.LogDebug($"Database {databaseName} non trovato. Inizializzazione database con migrazione.");
                    await context.Database.MigrateAsync();
                }
                logger.LogDebug($"Database {databaseName} found!");
                if ((await context.Database.GetPendingMigrationsAsync()).Any())
                {
                    logger.LogDebug($"Database {databaseName} not updated. MigrateAsync...");
                    await context.Database.MigrateAsync();
                    logger.LogDebug($"Database Updated.");
                }

                logger.LogDebug($"Check database {databaseName} OK.");
            }
        }
    }
}
