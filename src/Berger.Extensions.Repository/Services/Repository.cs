using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Berger.Extensions.Repository.Auxiliar;
using Berger.Extensions.Repository.Interfaces;

namespace Berger.Extensions.Repository.Services
{
    public abstract class Repository<T> : IDisposable, IRepository<T> where T : class
    {
        #region Properties
        private readonly DbSet<T> _entity;

        private readonly DbContext _context;
        #endregion

        #region Constructors
        protected Repository(DbContext context)
        {
            _entity = context.Set<T>();

            _context = context;
        }
        #endregion

        #region Methods
        public IQueryable<T> IgnoreQueryFilters()
        {
            return _entity.IgnoreQueryFilters();
        }

        public IQueryable<T> Get()
        {
            return _entity;
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
            return _entity.Find(id);
        }
        public T Add(T element, bool detach = false)
        {
            _entity.Add(element);

            _context.SaveChanges();

            if (detach)
                _context.Detach(element);

            return element;
        }
        public void Add(IQueryable<T> elements, bool detach = false)
        {
            foreach (var element in elements)
                _entity.Add(element);

            _context.SaveChanges();

            if (detach)
                _context.Detach(elements);
        }
        public T Update(T element)
        {
            _context.Entry(element).State = EntityState.Modified;

            _context.SaveChanges();

            return element;
        }
        public void Delete(Guid id)
        {
            var element = GetByID(id);

            _context.SoftDelete(element);

            _context.SaveChanges();
        }
        public void Delete(IQueryable<T> elements)
        {
            foreach (var element in elements)
                _context.SoftDelete<T>(element);

            _context.SaveChanges();
        }
        public async Task<T> AddAsync(T element)
        {
            await _entity.AddAsync(element);

            await _context.SaveChangesAsync();

            return element;
        }
        public async Task<T> UpdateAsync(T element)
        {
            _entity.Update(element);

            await _context.SaveChangesAsync();

            return element;
        }
        public async Task UpdateAsync(Func<T, string> field, string value)
        {
            await _entity.ExecuteUpdateAsync(s => s.SetProperty(field, field + value));
        }
        public async Task DeleteAsync(Guid id)
        {
            var element = GetByID(id);

            _context.SoftDelete(element);

            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
        #endregion
    }
}