namespace Core.Entities;

public class Message
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public DateTimeOffset SentAt { get; set; }
    public bool IsRead { get; set; }
    public bool IsReplied { get; set; }

    public int SenderId { get; set; }
    public User Sender { get; set; } = null!;

    public int ChatRoomId { get; set; }
    public ChatRoom ChatRoom { get; set; } = null!;
}