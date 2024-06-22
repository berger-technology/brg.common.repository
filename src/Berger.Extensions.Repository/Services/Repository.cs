using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Berger.Extensions.Abstractions;

namespace Berger.Extensions.Repository
{
    public abstract class Repository<T> : IRepository<T>, IDisposable where T : class
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
        public IQueryable<T> Get(bool tracking = false)
        {
            return tracking ? _entity : _entity.AsNoTracking();
        }
        public IQueryable<T> Get(Expression<Func<T, bool>> expression)
        {
            return Get().Where(expression);
        }
        public IQueryable<T> GetIgnoreFilters()
        {
            return Get().IgnoreQueryFilters();
        }
        public T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return Get().Where(expression).FirstOrDefault();
        }
        public T GetById(Guid id)
        {
            return _entity.Find(id);
        }
        public T Add(T element, bool detach = false)
        {
            _entity.Add(element);

            this.SaveChanges();

            if (detach)
                _context.Detach(element);

            return element;
        }
        public void Add(IQueryable<T> elements, bool detach = false)
        {
            foreach (var element in elements)
                _entity.Add(element);

            this.SaveChanges();

            if (detach)
                _context.Detach(elements);
        }
        public T Update(T element)
        {
            _context.Entry(element).State = EntityState.Modified;

            this.SaveChanges();

            return element;
        }
        public void Delete(Guid id)
        {
            var element = GetById(id);

            _context.SoftDelete(element);

            this.SaveChanges();
        }
        public void Delete(IQueryable<T> elements)
        {
            foreach (var element in elements)
                _context.SoftDelete<T>(element);

            this.SaveChanges();
        }
        public async Task<T> AddAsync(T element)
        {
            await _entity.AddAsync(element);

            await this.SaveChangesAsync();

            return element;
        }
        public async Task<T> UpdateAsync(T element)
        {
            _entity.Update(element);

            await this.SaveChangesAsync();

            return element;
        }
        public async Task UpdateAsync(Func<T, string> field, string value)
        {
            await _entity.ExecuteUpdateAsync(e => e.SetProperty(field, field + value));
        }
        public async Task DeleteAsync(Guid id)
        {
            var element = GetById(id);

            _context.SoftDelete(element);

            await this.SaveChangesAsync();
        }
        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }
        public async virtual Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
        #endregion
    }
}