using Microsoft.EntityFrameworkCore;
using Berger.Extensions.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Berger.Extensions.Repository
{
    public class ApplicationContext : IContext
    {
        private Guid _applicationId;
        public Guid ApplicationId => _applicationId;

        public ApplicationContext()
        {
            _applicationId = Guid.Empty;
        }

        public void SetContext(Guid applicationId)
        {
            _applicationId = applicationId;
        }
        public void SetApplication(Guid applicationId)
        {
            _applicationId = applicationId;
        }
    }
    public static class SqlServerConfiguration
    {
        public static IServiceCollection ConfigureDbContext<T>(this IServiceCollection services, IConfiguration configuration, string name, bool tracking = true) where T : DbContext
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            var connection = configuration.GetConnectionString(name);

            if (string.IsNullOrEmpty(connection))
                throw new FileNotFoundException(Errors.ConfigNotFound);

            services.AddScoped<IContext, ApplicationContext>().AddDbContext<T>(options =>
            {
                options.UseSqlServer(connection, e => e.EnableRetryOnFailure());

                if (!tracking)
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            },
            ServiceLifetime.Transient);

            return services;
        }
        public static void ConfigureDbContextFactory<T>(this IServiceCollection services, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Transient) where T : DbContext
        {
            services.AddDbContextFactory<T>(delegate (DbContextOptionsBuilder options)
            {
                options.EnableSensitiveDataLogging();

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