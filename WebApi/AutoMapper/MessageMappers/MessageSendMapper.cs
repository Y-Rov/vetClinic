using Core.Entities;
using Core.ViewModel.MessageViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.MessageMappers;

public class MessageSendMapper : IViewModelMapper<MessageSendViewModel, Message>
{
    public Message Map(MessageSendViewModel source)
    {
        return new Message()
        {
            Text = source.Text
        };
    }
}