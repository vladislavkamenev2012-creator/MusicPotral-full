using MusicPortal.DAL.Entities;

namespace MusicPortal.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Genre> Genres { get; }
        IRepository<Song> Songs { get; }
        Task<int> SaveChangesAsync();
    }
}
