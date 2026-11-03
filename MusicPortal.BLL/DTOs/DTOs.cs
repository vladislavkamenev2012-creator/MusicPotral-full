namespace MusicPortal.BLL.DTOs
{
    public class SongDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public int GenreId { get; set; }
        public string GenreName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
    }

    public class GenreDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
    }
}
