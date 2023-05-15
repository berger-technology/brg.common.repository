using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Berger.Extensions.Repository
{
    public static class SqlServerConfiguration
    {
        public static IServiceCollection ConfigureDbContext<T>(this IServiceCollection services, IConfiguration configuration, string name, bool tracking = true) where T : DbContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var connection = configuration.GetConnectionString(name);

            if (string.IsNullOrEmpty(connection))
                throw new FileNotFoundException($"The application configuration file for the connection {name} was not found.");

            services.AddDbContext<T>(options =>
            {
                options.UseSqlServer(connection, e => e.EnableRetryOnFailure());

                if (tracking == false)
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            return services;
        }
        public static void ConfigureDbContextFactory<T>(this IServiceCollection services, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped) where T : DbContext
        {
            services.AddDbContextFactory<T>(delegate (DbContextOptionsBuilder options)
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                options.ConfigureWarnings(builder =>
                {
                    builder.Ignore(RelationalEventId.BoolWithDefaultWarning);
                    builder.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning);
                });

            },
                lifetime
            );
        }
    }
}