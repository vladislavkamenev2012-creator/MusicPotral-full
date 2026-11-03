using Microsoft.EntityFrameworkCore;
using MusicPortal.DAL.Entities;

namespace MusicPortal.DAL.EF
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Song> Songs => Set<Song>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", PasswordHash = "admin123", Role = "Admin", IsApproved = true },
                new User { Id = 2, Username = "user1", PasswordHash = "user123", Role = "User", IsApproved = true }
            );

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Rock" },
                new Genre { Id = 2, Name = "Pop" },
                new Genre { Id = 3, Name = "Hip-Hop" }
            );

            modelBuilder.Entity<Song>().HasData(
                new Song { Id = 1, Title = "Bohemian Rhapsody", Artist = "Queen", GenreId = 1, UserId = 1, FilePath = "/uploads/sample.mp3" },
                new Song { Id = 2, Title = "Blinding Lights", Artist = "The Weeknd", GenreId = 2, UserId = 2, FilePath = "/uploads/sample.mp3" }
            );
        }
    }
}
