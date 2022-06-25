namespace Core.Entities;

public class ChatRoom
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    
    public IEnumerable<User> Users { get; set; } = new List<User>();
    public IEnumerable<Message> Messages { get; set; } = new List<Message>();
}