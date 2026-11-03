using MusicPortal.BLL.DTOs;

namespace MusicPortal.BLL.Interfaces
{
    public interface ISongService
    {
        Task<(IEnumerable<SongDTO> Items, int TotalCount)> GetSongsAsync(string? search, int? genreId, string? sortOrder, int page, int pageSize);
        Task<SongDTO?> GetSongByIdAsync(int id);
        Task AddSongAsync(SongDTO dto);
        Task DeleteSongAsync(int id);
    }

    public interface IAdminService
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync();
        Task ApproveUserAsync(int userId);
        Task DeleteUserAsync(int userId);
        Task<IEnumerable<GenreDTO>> GetGenresAsync();
        Task AddGenreAsync(string name);
        Task DeleteGenreAsync(int id);
    }
}
