using MusicPortal.DAL.EF;
using MusicPortal.DAL.Entities;
using MusicPortal.DAL.Interfaces;

namespace MusicPortal.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private EFRepository<User>? _userRepository;
        private EFRepository<Genre>? _genreRepository;
        private EFRepository<Song>? _songRepository;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }

        public IRepository<User> Users => _userRepository ??= new EFRepository<User>(_db);
        public IRepository<Genre> Genres => _genreRepository ??= new EFRepository<Genre>(_db);
        public IRepository<Song> Songs => _songRepository ??= new EFRepository<Song>(_db);

        public async Task<int> SaveChangesAsync() => await _db.SaveChangesAsync();

        public void Dispose() => _db.Dispose();
    }
}
