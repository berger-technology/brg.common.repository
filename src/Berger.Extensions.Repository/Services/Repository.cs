using Berger.Extensions.Repository.Interfaces;
using Berger.Extensions.Repository.Auxiliar;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Berger.Extensions.Repository.Services
{
    public abstract class Repository<T> : IDisposable, IRepository<T> where T : class
    {
        #region Properties
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        #endregion

        #region Constructors
        protected Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        #endregion

        #region Methods
        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async virtual Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IQueryable<T> Get()
        {
            return _dbSet;
        }
        public IQueryable<T> IgnoreQueryFilters()
        {
            return _dbSet.IgnoreQueryFilters();
        }
        public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return Get().Where(predicate);
        }
        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return Get().Where(predicate).FirstOrDefault();
        }
        public T GetByID(Guid id)
        {
            return _dbSet.Find(id);
        }
        public T Add(T element, bool detach = false)
        {
            _dbSet.Add(element);
            SaveChanges();

            if (detach)
                _context.Detach(element);

            return element;
        }
        public void Add(IQueryable<T> elements, bool detach = false)
        {
            foreach (var entity in elements)
                _dbSet.Add(entity);

            SaveChanges();

            if (detach)
                _context.Detach(elements);
        }
        public void Update(T element)
        {
            _context.Entry(element).State = EntityState.Modified;
            SaveChanges();
        }
        public void Delete(Guid id)
        {
            var element = GetByID(id);

            _context.SoftDelete(element);
            SaveChanges();
        }
        public void Delete(IQueryable<T> elements)
        {
            foreach (var element in elements)
                _context.SoftDelete<T>(element);

            SaveChanges();
        }
        public async Task<T> AddAsync(T element)
        {
            await _dbSet.AddAsync(element);
            await SaveChangesAsync();

            return element;
        }
        public async Task UpdateAsync(T element)
        {
            _dbSet.Update(element);

            await SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            var element = GetByID(id);

            _context.SoftDelete(element);

            await SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
        #endregion
    }
}