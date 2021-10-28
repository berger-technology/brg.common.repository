using System;
using System.Linq;
using System.Linq.Expressions;

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
        void Update(Guid id, T element);
        void Delete(Guid id);
    }
}