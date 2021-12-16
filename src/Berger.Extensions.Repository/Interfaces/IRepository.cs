using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Berger.Extensions.Repository.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);
        T GetByID(Guid id);
        T Add(T element, bool detach = false);
        void Add(IQueryable<T> elements, bool detach = false);
        void BulkInsert(IQueryable<T> elements);
        void Update(T element);
        void BulkUpdate(IQueryable<T> elements);
        void Delete(Guid id);
        void BulkDelete(IQueryable<T> elements);
        Task<T> AddAsync(T element);
        Task UpdateAsync(T element);
        Task DeleteAsync(Guid id);
    }
}