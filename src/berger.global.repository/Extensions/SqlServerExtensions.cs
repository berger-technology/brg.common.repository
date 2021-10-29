using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Berger.Global.Repository.Extensions
{
    public static class SqlServerExtensions
    {
        public static void ConfigureSql<T>(this IServiceCollection services, IConfiguration configuration, string name) where T : DbContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            AddDbContext<T>(services, configuration, name);
        }

        public static void ConfigureSqlEnvironment<T>(this IServiceCollection services, string name) where T : DbContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            AddDbContext<T>(services, name);
        }

        private static void AddDbContext<T>(IServiceCollection services, IConfiguration configuration, string name) where T : DbContext
        {
            var connection = configuration.GetConnectionString(name);

            if (string.IsNullOrEmpty(connection))
                throw new FileNotFoundException($"The configuration file for the connection {name} was not found.");

            Configure<T>(services, connection);
        }
        private static void AddDbContext<T>(IServiceCollection services, string name) where T : DbContext
        {
            var connection = Environment.GetEnvironmentVariable(name);

            if (string.IsNullOrEmpty(connection))
                throw new ArgumentException($"The configuration environment variable for the connection {name} was not found.");

            Configure<T>(services, connection);
        }
        private static void Configure<T>(IServiceCollection services, string connection) where T : DbContext
        {
            services.AddDbContext<T>(options =>
            {
                options.UseSqlServer(connection);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }
    }
}