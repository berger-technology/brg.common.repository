using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Berger.Extensions.Repository
{
    public static class SeedHelper
    {
        public static void SeedFromFile(this DbContext context, string path)
        {
            var script = File.ReadAllText(path, Encoding.UTF8);

            context.Database.ExecuteSqlRaw(script);
        }
        public static void SeedFromLargeFile(this DbContext context, string path, int bufferSize)
        {
            var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.SequentialScan);

            using (StreamReader reader = new StreamReader(fs))
            {
                StringBuilder sb = new StringBuilder();

                while (!reader.EndOfStream)
                {
                    context.Database.ExecuteSqlRaw(reader.ReadLine());
                }
            }

            fs.Close();
            fs.Dispose();
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