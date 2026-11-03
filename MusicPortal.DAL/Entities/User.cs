namespace MusicPortal.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // "Admin", "User", "Pending"
        public bool IsApproved { get; set; } = false;
        public List<Song> Songs { get; set; } = new();
    }
}
