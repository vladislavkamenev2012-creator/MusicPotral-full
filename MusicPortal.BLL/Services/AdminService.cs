using Microsoft.EntityFrameworkCore;
using MusicPortal.BLL.DTOs;
using MusicPortal.BLL.Interfaces;
using MusicPortal.DAL.Entities;
using MusicPortal.DAL.Interfaces;

namespace MusicPortal.BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _uow;

        public AdminService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            var users = await _uow.Users.GetAllAsync();
            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role,
                IsApproved = u.IsApproved
            });
        }

        public async Task ApproveUserAsync(int userId)
        {
            var user = await _uow.Users.GetByIdAsync(userId);
            if (user != null)
            {
                user.IsApproved = true;
                user.Role = "User";
                _uow.Users.Update(user);
                await _uow.SaveChangesAsync();
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _uow.Users.GetByIdAsync(userId);
            if (user != null)
            {
                _uow.Users.Delete(user);
                await _uow.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<GenreDTO>> GetGenresAsync()
        {
            var genres = await _uow.Genres.GetAllAsync();
            return genres.Select(g => new GenreDTO { Id = g.Id, Name = g.Name });
        }

        public async Task AddGenreAsync(string name)
        {
            await _uow.Genres.CreateAsync(new Genre { Name = name });
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteGenreAsync(int id)
        {
            var genre = await _uow.Genres.GetByIdAsync(id);
            if (genre != null)
            {
                _uow.Genres.Delete(genre);
                await _uow.SaveChangesAsync();
            }
        }
    }
}
