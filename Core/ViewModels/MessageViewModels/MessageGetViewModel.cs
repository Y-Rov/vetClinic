namespace Core.ViewModel.MessageViewModels;

public class MessageGetViewModel : MessageViewModelBase
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public DateTimeOffset SentAt { get; set; }
    public bool IsRead { get; set; }
    public bool IsReplied { get; set; }

    public int SenderId { get; set; }
    public string? SenderName { get; set; }
    
    public int ChatRoomId { get; set; }
}