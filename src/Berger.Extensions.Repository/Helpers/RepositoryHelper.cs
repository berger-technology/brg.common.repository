using Microsoft.EntityFrameworkCore;

namespace Berger.Extensions.Repository
{
    public static class RepositoryHelper
    {
        public static void SoftDelete<T>(this DbContext context, T entity) where T : class
        {
            context.Entry(entity).CurrentValues[Values.Deleted] = true;
            context.Entry(entity).Property(Values.Deleted).IsModified = true;
        }
        public static void Detach<T>(this DbContext context, T element)
        {
            context.Entry(element).State = EntityState.Detached;
        }
        public static void Detach<T>(this DbContext context, IQueryable<T> elements)
        {
            foreach (var element in elements)
                context.Entry(element).State = EntityState.Detached;
        }
    }
}