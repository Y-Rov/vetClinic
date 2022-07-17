using Core.Emuns;

namespace Core.ViewModels.ChatRoomViewModels;

public class ChatRoomGetViewModel
{
    public int Id { get; set; }
    public ChatType Type { get; set; }
    public int InterlocutorId { get; set; }
    public string? Name { get; set; }
}