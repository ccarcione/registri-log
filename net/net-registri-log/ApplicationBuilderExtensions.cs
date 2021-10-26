using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using net_registri_log.ApiLog.Middleware;
using System.Linq;

namespace net_registri_log.Providers
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseNetRegistriLog(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApiLogMiddleware>();
            UpdateDatabaseMigrate(app);

            return app;
        }

        internal static async void UpdateDatabaseMigrate(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //var options = scope.ServiceProvider.GetRequiredService<Model.Options>();
                var context = scope.ServiceProvider.GetRequiredService<RegistriLogDbContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<RegistriLogDbContext>>();

                string databaseName = context.Database.GetDbConnection().Database;

                if (!await (context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).ExistsAsync())
                {
                    logger.LogDebug($"Database {databaseName} non trovato. Inizializzazione database con migrazione.");
                    await context.Database.MigrateAsync();
                }
                logger.LogDebug($"Database {databaseName} trovato!");
                if ((await context.Database.GetPendingMigrationsAsync()).Any())
                {
                    logger.LogDebug($"Database {databaseName} non aggiornato. Applicazione migrazioni mancanti.");
                    await context.Database.MigrateAsync();
                }

                logger.LogDebug($"Check database {databaseName} OK.");
                logger.LogDebug($"Database aggiornati.");
            }
        }
    }
}
