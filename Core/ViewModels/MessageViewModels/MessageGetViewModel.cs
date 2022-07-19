namespace Core.ViewModel.MessageViewModels;

public class MessageGetViewModel : MessageViewModelBase
{
    public int Id { get; set; }
    public DateTime SentAt { get; set; }

    public int SenderId { get; set; }
    
    public int ChatRoomId { get; set; }
}