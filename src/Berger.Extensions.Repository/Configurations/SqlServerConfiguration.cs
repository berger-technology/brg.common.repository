using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Berger.Extensions.Repository
{
    public static class SqlServerConfiguration
    {
        public static IServiceCollection ConfigureDbContext<T>(this IServiceCollection services, IConfiguration configuration, string name) where T : DbContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            return Configure<T>(services, configuration, name);
        }
        private static IServiceCollection Configure<T>(IServiceCollection services, IConfiguration configuration, string name, bool tracking = true) where T : DbContext
        {
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
    }
}