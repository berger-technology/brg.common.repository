using Microsoft.Extensions.DependencyInjection;

namespace Berger.Extensions.Repository
{
    public static class RepositoryConfiguration
    {
        public static void ConfigureRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<,>));
        }
    }
}