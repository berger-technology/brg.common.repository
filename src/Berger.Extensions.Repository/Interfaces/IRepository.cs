using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Berger.Extensions.Repository.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();
        IQueryable<T> IgnoreQueryFilters();
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        T GetByID(Guid id);
        T Add(T element, bool detach = false);
        void Add(IQueryable<T> elements, bool detach = false);
        void Update(T element);
        void Delete(Guid id);
        void Delete(IQueryable<T> elements);
        Task<T> AddAsync(T element);
        Task UpdateAsync(T element);
        Task DeleteAsync(Guid id);
    }
}