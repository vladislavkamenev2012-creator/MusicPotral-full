using Microsoft.EntityFrameworkCore;
using MusicPortal.BLL.DTOs;
using MusicPortal.BLL.Interfaces;
using MusicPortal.DAL.Entities;
using MusicPortal.DAL.Interfaces;

namespace MusicPortal.BLL.Services
{
    public class SongService : ISongService
    {
        private readonly IUnitOfWork _uow;

        public SongService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<(IEnumerable<SongDTO> Items, int TotalCount)> GetSongsAsync(string? search, int? genreId, string? sortOrder, int page, int pageSize)
        {
            var query = _uow.Songs.Find(s => true);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s => s.Title.Contains(search) || s.Artist.Contains(search));
            }

            if (genreId.HasValue && genreId > 0)
            {
                query = query.Where(s => s.GenreId == genreId.Value);
            }

            query = sortOrder switch
            {
                "title_desc" => query.OrderByDescending(s => s.Title),
                "artist" => query.OrderBy(s => s.Artist),
                "artist_desc" => query.OrderByDescending(s => s.Artist),
                _ => query.OrderBy(s => s.Title),
            };

            int totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new SongDTO
                {
                    Id = s.Id,
                    Title = s.Title,
                    Artist = s.Artist,
                    FilePath = s.FilePath,
                    GenreId = s.GenreId,
                    GenreName = s.Genre != null ? s.Genre.Name : "",
                    UserId = s.UserId,
                    UserName = s.User != null ? s.User.Username : ""
                })
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<SongDTO?> GetSongByIdAsync(int id)
        {
            var s = await _uow.Songs.GetByIdAsync(id);
            if (s == null) return null;

            return new SongDTO
            {
                Id = s.Id,
                Title = s.Title,
                Artist = s.Artist,
                FilePath = s.FilePath,
                GenreId = s.GenreId
            };
        }

        public async Task AddSongAsync(SongDTO dto)
        {
            var song = new Song
            {
                Title = dto.Title,
                Artist = dto.Artist,
                FilePath = dto.FilePath,
                GenreId = dto.GenreId,
                UserId = dto.UserId
            };
            await _uow.Songs.CreateAsync(song);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteSongAsync(int id)
        {
            var song = await _uow.Songs.GetByIdAsync(id);
            if (song != null)
            {
                _uow.Songs.Delete(song);
                await _uow.SaveChangesAsync();
            }
        }
    }
}
