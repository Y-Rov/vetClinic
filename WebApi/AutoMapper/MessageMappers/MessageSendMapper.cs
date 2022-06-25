using Core.Entities;
using Core.ViewModel.MessageViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.MessageMappers;

public class MessageSendMapper : IViewModelMapper<Message, MessageSendViewModel>
{
    public MessageSendViewModel Map(Message source)
    {
        return new MessageSendViewModel()
        {
            ChatRoomId = source.ChatRoomId,
            Text = source.Text
        };
    }
}