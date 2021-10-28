using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Berger.Global.Repository.Extensions
{
    public static class SeedExtensions
    {
        public static void SeedFromFile(this DbContext context, string path)
        {
            var script = File.ReadAllText(path, Encoding.UTF8);

            context.Database.ExecuteSqlRaw(script);
        }
        public static void SeedFromLargeFile(this DbContext context, string path)
        {
            using (StreamReader sr = File.OpenText(path))
            {
                string line = string.Empty;

                while ((line = sr.ReadLine()) != null)
                {
                    context.Database.ExecuteSqlRaw(line);
                }
            }
        }
    }
}