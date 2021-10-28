using Microsoft.EntityFrameworkCore;

namespace berger.global.repository.Extensions
{
    public static class DataExtensions
    {
        public static void SoftDelete<T>(this DbContext context, T entity) where T : class
        {
            context.Entry(entity).CurrentValues["Deleted"] = true;
            context.Entry(entity).Property("Deleted").IsModified = true;
        }
    }
}