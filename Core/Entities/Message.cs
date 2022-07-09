namespace Core.Entities;

public class Message
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public DateTime SentAt { get; set; }
    public int SenderId { get; set; }
    public User Sender { get; set; } = null!;

    public int ChatRoomId { get; set; }
    public ChatRoom ChatRoom { get; set; } = null!;

    public IEnumerable<UserChatRoom> LastReadByUsers { get; set; } = new List<UserChatRoom>();
}