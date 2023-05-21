using Microsoft.EntityFrameworkCore;

namespace Berger.Extensions.Repository
{
    public class BaseRepository<T, C> : Repository<T> where T : class where C : DbContext
    {
        private readonly C _context;
        public BaseRepository(C context) : base(context)
        {
            _context = context;
        }
    }
}