namespace MusicPortal.DAL.Entities
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public int GenreId { get; set; }
        public Genre? Genre { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
