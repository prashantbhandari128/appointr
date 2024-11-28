using Appointr.Persistence.Context;
using Appointr.Persistence.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Appointr.Persistence.Repository.Implementation
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;
        private bool _disposed = false;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public void Insert(TEntity entity) => _context.Set<TEntity>().Add(entity);

        public async Task InsertAsync(TEntity entity) => await _context.Set<TEntity>().AddAsync(entity);

        public void InsertRange(IEnumerable<TEntity> entities) => _context.Set<TEntity>().AddRange(entities);

        public async Task InsertRangeAsync(IEnumerable<TEntity> entities) => await _context.Set<TEntity>().AddRangeAsync(entities);

        public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);

        public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);

        public void DeleteRange(IEnumerable<TEntity> entities) => _context.Set<TEntity>().RemoveRange(entities);

        public int Count() => _context.Set<TEntity>().Count();

        public async Task<int> CountAsync() => await _context.Set<TEntity>().CountAsync();

        public List<TEntity> List() => _context.Set<TEntity>().ToList();

        public List<TEntity> List(int page, int pageSize) => _context.Set<TEntity>().Skip((page - 1) * pageSize).Take(pageSize).ToList();

        public async Task<List<TEntity>> ListAsync() => await _context.Set<TEntity>().ToListAsync();

        public async Task<List<TEntity>> ListAsync(int page, int pageSize) => await _context.Set<TEntity>().Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        public TEntity? Find(Guid id) => _context.Set<TEntity>().Find(id);

        public async Task<TEntity?> FindAsync(Guid id) => await _context.Set<TEntity>().FindAsync(id);

        public IEnumerable<TEntity> GetEnumerable() => _context.Set<TEntity>().AsEnumerable();

        public IAsyncEnumerable<TEntity> GetAsyncEnumerable() => _context.Set<TEntity>().AsAsyncEnumerable();

        public IQueryable<TEntity> GetQueryable() => _context.Set<TEntity>().AsQueryable();

        public DbSet<TEntity> GetEntitySet() => _context.Set<TEntity>();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
