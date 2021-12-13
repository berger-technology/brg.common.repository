using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Berger.Global.Repository.Interfaces;
using Berger.Global.Repository.Extensions;

namespace Berger.Global.Repository.Services
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

        #region Methods
        public IQueryable<T> Get()
        {
            return _context.Set<T>();
        }
        public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return Get().Where(predicate);
        }

        public T GetByID(Guid id)
        {
            return _context.Set<T>().Find(id);
        }
        public T Add(T element, bool detach = false)
        {
            _context.Set<T>().Add(element);
            _context.SaveChanges();

            if (detach)
                _context.Detach(element);

            return element;
        }
        public void Add(IQueryable<T> elements, bool detach = false)
        {
            foreach (var entity in elements)
                _context.Set<T>().Add(entity);

            _context.SaveChanges();

            if (detach)
                _context.Detach(elements);
        }

        public void BulkInsert(IQueryable<T> elements)
        {
            _context.BulkInsert(elements);
        }
        public void BulkDelete(IQueryable<T> elements)
        {
            _context.BulkDelete(elements);
        }
        public void Update(T element)
        {
            _context.Entry(element).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void Delete(Guid id)
        {
            var element = GetByID(id);

            _context.SoftDelete<T>(element);
            _context.SaveChanges();
        }
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
            var element = GetByID(id);

            _context.Set<T>();
            _context.SoftDelete<T>(element);

            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
        #endregion
    }
}