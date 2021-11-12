using Microsoft.EntityFrameworkCore;

namespace Berger.Global.Repository.Extensions
{
    public static class DataExtension
    {
        public static void SoftDelete<T>(this DbContext context, T entity) where T : class
        {
            context.Entry(entity).CurrentValues["Deleted"] = true;
            context.Entry(entity).Property("Deleted").IsModified = true;
        }
    }
}