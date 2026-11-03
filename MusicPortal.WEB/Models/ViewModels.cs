using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MusicPortal.BLL.DTOs;

namespace MusicPortal.WEB.Models
{
    public class SongListViewModel
    {
        public IEnumerable<SongDTO> Songs { get; set; } = new List<SongDTO>();
        public IEnumerable<GenreDTO> Genres { get; set; } = new List<GenreDTO>();
        public string? Search { get; set; }
        public int? GenreId { get; set; }
        public string SortOrder { get; set; } = "title";
        public PageInfo PageInfo { get; set; } = new PageInfo();
    }

    public class PageInfo
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }

    public class CreateSongViewModel
    {
        [Required(ErrorMessage = "ReqTitle")]
        [StringLength(100, ErrorMessage = "TitleMaxLen")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "ReqArtist")]
        public string Artist { get; set; } = string.Empty;

        [Required(ErrorMessage = "ReqGenre")]
        public int? GenreId { get; set; }

        [Required(ErrorMessage = "ReqFile")]
        public IFormFile? AudioFile { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "ReqUsername")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "ReqPassword")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "ReqUsername")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "ReqPassword")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
