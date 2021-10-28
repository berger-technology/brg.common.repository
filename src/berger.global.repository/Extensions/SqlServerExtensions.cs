using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace berger.global.repository.Extensions
{
    public static class SqlServerExtensions
    {
        public static void ConfigureSql<T>(this IServiceCollection services, IConfiguration configuration, string name) where T : DbContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            AddDbContext<T>(services, configuration, name);
        }

        public static void ConfigureSql<T>(this IServiceCollection services, IConfiguration configuration) where T : DbContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            AddDbContext<T>(services, configuration, "AzureSqlServer");
        }

        private static void AddDbContext<T>(IServiceCollection services, IConfiguration configuration, string name) where T : DbContext
        {
            var connection = configuration.GetConnectionString(name);

            if (string.IsNullOrEmpty(connection))
                throw new FileNotFoundException($"The configuration file for the connection {name} was not found.");

            services.AddDbContext<T>(options =>
            {
                options.UseSqlServer(connection);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }
    }
}