using Microsoft.EntityFrameworkCore;

namespace Berger.Extensions.Repository
{
    public abstract class BaseContext<T> : DbContext where T : DbContext
    {
        public BaseContext(DbContextOptions<T> options) : base(options)
        {
            Database.SetCommandTimeout(1000);
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            // Unicode
            builder.Properties<string>().AreUnicode(false);
            builder.Properties<List<string>>().AreUnicode(false);
            builder.Properties<Dictionary<string, string>>().AreUnicode(false);

            // Precisions
            builder.Properties<double>().HavePrecision(18, 2);
            builder.Properties<decimal>().HavePrecision(18, 2);

            // Conversions
            builder.Properties<List<string>>().HaveConversion<StringListConverter<List<string>>>();
            builder.Properties<Dictionary<string, int>>().HaveConversion<JsonConverter<Dictionary<string, int>>>();
            builder.Properties<Dictionary<string, string>>().HaveConversion<JsonConverter<Dictionary<string, string>>>();

            base.ConfigureConventions(builder);
        }
    }
}