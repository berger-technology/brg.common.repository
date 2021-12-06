using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Berger.Global.Repository.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);
        T GetByID(Guid id);
        T Add(T element);
        void Add(IQueryable<T> elements);
        void Update(T element);
        void Delete(Guid id);
        Task<T> AddAsync(T element);
        Task UpdateAsync(T element);
        Task DeleteAsync(Guid id);
    }
}