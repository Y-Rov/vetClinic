using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Emuns;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModel.MessageViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Moq;
using WebApi.AutoMapper.Interface;
using WebApi.Controllers;
using WebApi.SignalR.Hubs;

namespace WebApi.Test.Fixtures;

public class MessagesControllerFixture
{
    public MessagesControllerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        ExpectedMessageGetViewModel = GetMessageGetViewModel();
        ExpectedMessageSendViewModel = GetMessageSendViewModel();
        ExpectedMessages = GetMessages();
        ExpectedEmptyMessagesList = GetEmptyMessagesList();
        ExpectedUser = GetUser();
        ExpectedPrivateChatRoom = GetPrivateChatRoom();
        ExpectedMessageGetViewModels = GetMessageGetViewModels();
        ExpectedEmptyMessageGetViewModels = GetEmptyMessageGetViewModelList();
        
        MockMessageService = fixture.Freeze<Mock<IMessageService>>();
        MockChatRoomService = fixture.Freeze<Mock<IChatRoomService>>();
        MockUserManager = fixture.Freeze<Mock<UserManager<User>>>();
        
        MockClientProxy = fixture.Freeze<Mock<IClientProxy>>();
        MockHubClients = fixture.Freeze<Mock<IHubClients>>();
        MockHubContext = fixture.Freeze<Mock<IHubContext<MessageHub>>>();

        MockEnumMessageMapper = fixture.Freeze<Mock<IEnumerableViewModelMapper<IEnumerable<Message>, IEnumerable<MessageGetViewModel>>>>();
        MockMessageGetMapper = fixture.Freeze<Mock<IViewModelMapper<Message, MessageGetViewModel>>>();
        MockMessageSendMapper = fixture.Freeze<Mock<IViewModelMapper<MessageSendViewModel, Message>>>();
        
        MessagesController = new MessagesController(
            MockMessageService.Object,
            MockEnumMessageMapper.Object,
            MockMessageGetMapper.Object,
            MockUserManager.Object,
            MockHubContext.Object,
            MockChatRoomService.Object,
            MockMessageSendMapper.Object);
    }

    public MessagesController MessagesController { get; }
    
    public Mock<IMessageService> MockMessageService { get; }
    public Mock<IChatRoomService> MockChatRoomService { get; }
    public Mock<UserManager<User>> MockUserManager { get; }

    public Mock<IClientProxy> MockClientProxy { get; }
    public Mock<IHubClients> MockHubClients { get; }
    public Mock<IHubContext<MessageHub>> MockHubContext { get; }
    
    public Mock<IEnumerableViewModelMapper<IEnumerable<Message>, IEnumerable<MessageGetViewModel>>> MockEnumMessageMapper { get; }
    public Mock<IViewModelMapper<Message, MessageGetViewModel>> MockMessageGetMapper { get; }
    public Mock<IViewModelMapper<MessageSendViewModel, Message>> MockMessageSendMapper { get; }

    public MessageGetViewModel ExpectedMessageGetViewModel { get; }
    public MessageSendViewModel ExpectedMessageSendViewModel { get; }
    public IEnumerable<Message> ExpectedMessages { get; }
    public IEnumerable<MessageGetViewModel> ExpectedMessageGetViewModels { get; }
    public IEnumerable<Message> ExpectedEmptyMessagesList { get; }
    public IEnumerable<MessageGetViewModel> ExpectedEmptyMessageGetViewModels { get; }
    public User ExpectedUser { get; }
    public ChatRoom ExpectedPrivateChatRoom { get; }
    
    private MessageGetViewModel GetMessageGetViewModel()
    {
        return new MessageGetViewModel()
        {
            Id = 1,
            ChatRoomId = 1,
            SenderId = 1,
            SentAt = DateTime.Now,
            Text = "Message text"
        };
    }
    private MessageSendViewModel GetMessageSendViewModel()
    {
        return new MessageSendViewModel()
        {
            ReceiverId = 1,
            Text = "Message text"
        };
    }
    
    private IEnumerable<Message> GetMessages()
    {
        var messages = new List<Message>()
        {
            new Message()
            {
                Id = 1,
                ChatRoomId = 1,
                SenderId = 1,
                SentAt = DateTime.Now,
                Text = "Message text"
            },
            new Message()
            {
                Id = 2,
                ChatRoomId = 1,
                SenderId = 2,
                SentAt = DateTime.Now,
                Text = "Message text"
            },
            new Message()
            {
                Id = 3,
                ChatRoomId = 1,
                SenderId = 1,
                SentAt = DateTime.Now,
                Text = "Message text"
            }
        };
        return messages;
    }

    private IEnumerable<MessageGetViewModel> GetMessageGetViewModels()
    {
        var messages = new List<MessageGetViewModel>()
        {
            new MessageGetViewModel()
            {
                Id = 1,
                ChatRoomId = 1,
                SenderId = 1,
                SentAt = DateTime.Now,
                Text = "Message text"
            },
            new MessageGetViewModel()
            {
                Id = 2,
                ChatRoomId = 1,
                SenderId = 2,
                SentAt = DateTime.Now,
                Text = "Message text"
            },
            new MessageGetViewModel()
            {
                Id = 3,
                ChatRoomId = 1,
                SenderId = 1,
                SentAt = DateTime.Now,
                Text = "Message text"
            }
        };
        return messages;
    }
    private IEnumerable<Message> GetEmptyMessagesList()
    {
        return new List<Message>();
    }

    private IEnumerable<MessageGetViewModel> GetEmptyMessageGetViewModelList()
    {
        return new List<MessageGetViewModel>();
    }
    private User GetUser()
    {
        return new User()
        {
            Id = 1,
            Email = "testuser@gmail.com"
        };
    }
    private ChatRoom GetPrivateChatRoom()
    {
        return new ChatRoom()
        {
            Id = 1,
            Type = ChatType.Private
        };
    }
}