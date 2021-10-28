using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using berger.global.repository.Extensions;
using berger.global.repository.Interfaces;

namespace berger.global.repository.Services
{
    public abstract class Repository<T> : IDisposable, IRepository<T> where T : class
    {
        #region Properties
        private readonly DbContext _context;
        #endregion

        #region Constructors
        protected Repository(DbContext context)
        {
            _context = context;
        }
        #endregion

        #region Sync Methods
        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public void Add(IQueryable<T> entities)
        {
            foreach (var entity in entities)
            {
                _context.Set<T>().Add(entity);
            }

            _context.SaveChanges();

        }
        public void Delete(Guid id)
        {
            var element = _context.Set<T>().Find(id);

            _context.Set<T>();
            _context.SoftDelete<T>(element);
            _context.SaveChanges();
        }
        public IQueryable<T> Get()
        {
            return _context.Set<T>();
        }
        public T GetByID(Guid id)
        {
            return _context.Set<T>().Find(id);
        }
        public T Get(string key)
        {
            return _context.Set<T>().Find(key);
        }
        public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return Get().Where(predicate);
        }
        public void Update(Guid id, T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }
        public int Count()
        {
            return Get().Count();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
        #endregion

        #region Async Methods
        public async Task<T> AddAsync(T element)
        {
            await _context.Set<T>().AddAsync(element);
            await _context.SaveChangesAsync();

            return element;
        }

        public async Task UpdateAsync(T element)
        {
            _context.Set<T>().Update(element);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var element = _context.Set<T>().Find(id);

            _context.Set<T>();
            _context.SoftDelete<T>(element);

            await _context.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        #endregion
    }
}