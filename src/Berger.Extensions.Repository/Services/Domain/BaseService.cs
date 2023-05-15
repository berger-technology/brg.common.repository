using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Berger.Extensions.Repository
{
    public class BaseService<T> : IRepository<T>  where T : class
    {
        #region Properties
        public readonly IRepository<T> _repository;
        public readonly IServiceProvider _services;
        #endregion

        #region Constructors
        public BaseService(IServiceProvider services)
        {
            _services = services;

            _repository = (IRepository<T>)_services.GetService(typeof(IRepository<T>));
        }
        public T Add(T element, bool detach = false)
        {
            return _repository.Add(element);
        }
        public void Add(IQueryable<T> elements, bool detach = false)
        {
            _repository.Add(elements, detach);
        }
        public async Task<T> AddAsync(T element)
        {
            return await _repository.AddAsync(element);
        }
        public void Delete(Guid id)
        {
            _repository.Delete(id);
        }
        public void Delete(IQueryable<T> elements)
        {
            _repository.Delete(elements);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
        public T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return _repository.FirstOrDefault(expression);
        }
        public IQueryable<T> Get()
        {
            return _repository.Get();
        }
        public IQueryable<T> Get(Expression<Func<T, bool>> expression)
        {
            return _repository.Get(expression);
        }
        public T GetById(Guid id)
        {
            return _repository.GetById(id);
        }
        public IQueryable<T> GetIgnoreFilters()
        {
            return _repository.GetIgnoreFilters();
        }
        public T Update(T element)
        {
            return _repository.Update(element);
        }
        public async Task<T> UpdateAsync(T element)
        {
            return await _repository.UpdateAsync(element);
        }
        public async Task UpdateAsync(Func<T, string> field, string value)
        {
            await _repository.UpdateAsync(field, value);
        }
        #endregion
    }
}