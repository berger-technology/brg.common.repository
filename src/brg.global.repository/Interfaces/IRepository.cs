using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace brg.global.repository.Interfaces
{
    public interface IRepository<T>
    {
        #region Sync
        T Add(T entity);
        void Add(IQueryable<T> entities);
        void Delete(Guid id);
        T GetByID(Guid id);
        T Get(string key);
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);
        IQueryable<T> Get();
        int Count();
        void Update(Guid id, T entity);
        #endregion

        #region Async
        Task<T> AddAsync(T element);
        Task UpdateAsync(T element);
        Task DeleteAsync(Guid id);
        Task<T> GetByIdAsync(Guid id);
        #endregion
    }
}