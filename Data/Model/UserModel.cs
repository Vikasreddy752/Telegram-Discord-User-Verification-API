namespace TelegramAPI.Data.Model
{
    public class UserModel
    {
        public int Id { get; set; }
        public long TelegramUserId { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public long ChatId { get; set; } // Group ID
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
