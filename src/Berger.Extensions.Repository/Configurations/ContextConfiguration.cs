using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Berger.Extensions.Repository
{
    public static class ContextConfiguration
    {
        public static void ConfigureDatabase<T>(this IServiceCollection services, IConfiguration configuration) where T : DbContext
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
                ServiceLifetime.Transient
            );
        }
    }
}