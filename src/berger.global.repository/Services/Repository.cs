using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Berger.Global.Repository.Extensions;
using Berger.Global.Repository.Interfaces;

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
        public T Add(T element)
        {
            _context.Set<T>().Add(element);
            _context.SaveChanges();

            return element;
        }
        public void Add(IQueryable<T> elements)
        {
            try
            {
                foreach (var entity in elements)
                {
                    _context.Set<T>().Add(entity);
                }

                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(T element)
        {
            _context.Set<T>().Update(element);

            _context.SaveChanges();
        }
        public void Update(Guid id, T element)
        {
            _context.Set<T>().Update(element);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var element = _context.Set<T>().Find(id);

            _context.Set<T>();
            _context.SoftDelete<T>(element);

            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
        #endregion
    }
}