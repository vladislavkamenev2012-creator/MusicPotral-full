using Microsoft.EntityFrameworkCore;
using MusicPortal.DAL.EF;
using MusicPortal.DAL.Interfaces;
using System.Linq.Expressions;

namespace MusicPortal.DAL.Repositories
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _db;
        private readonly DbSet<T> _dbSet;

        public EFRepository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = db.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate);

        public async Task CreateAsync(T item) => await _dbSet.AddAsync(item);

        public void Update(T item) => _dbSet.Update(item);

        public void Delete(T item) => _dbSet.Remove(item);
    }
}
