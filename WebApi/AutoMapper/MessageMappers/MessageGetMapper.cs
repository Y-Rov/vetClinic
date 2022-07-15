using Core.Entities;
using Core.ViewModel.MessageViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.MessageMappers;

public class MessageGetMapper : IViewModelMapper<Message, MessageGetViewModel>
{
    public MessageGetViewModel Map(Message source)
    {
        return new MessageGetViewModel()
        {
            Id = source.Id,
            Text = source.Text,
            ChatRoomId = source.ChatRoomId,
            SenderId = source.SenderId,
            SentAt = source.SentAt
        };
    }
}