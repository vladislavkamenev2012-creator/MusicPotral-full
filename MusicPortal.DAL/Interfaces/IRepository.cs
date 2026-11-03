using System.Linq.Expressions;

namespace MusicPortal.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        Task CreateAsync(T item);
        void Update(T item);
        void Delete(T item);
    }
}
