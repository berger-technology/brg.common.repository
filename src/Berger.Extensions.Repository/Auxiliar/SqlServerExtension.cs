using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Berger.Extensions.Repository.Auxiliar
{
    public static class SqlServerExtension
    {
        public static void ConfigureSql<T>(this IServiceCollection services, IConfiguration configuration, string name) where T : DbContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            AddDbContext<T>(services, configuration, name);
        }
        private static void AddDbContext<T>(IServiceCollection services, IConfiguration configuration, string name, bool tracking = true) where T : DbContext
        {
            var connection = configuration.GetConnectionString(name);

            if (string.IsNullOrEmpty(connection))
                throw new FileNotFoundException($"The configuration file for the connection {name} was not found.");

            services.AddDbContext<T>(options =>
            {
                options.UseSqlServer(connection);

                if (tracking == false)
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }
    }
}