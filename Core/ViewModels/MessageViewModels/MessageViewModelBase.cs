namespace Core.ViewModel.MessageViewModels;

public abstract class MessageViewModelBase
{
    public string? Text { get; set; }
    public int ChatRoomId { get; set; }
}