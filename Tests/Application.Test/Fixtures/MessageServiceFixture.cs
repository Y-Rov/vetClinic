using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Moq;

namespace Application.Test.Fixtures;

public class MessageServiceFixture
{
    public MessageServiceFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        ExpectedMessagesInChatRoom = GetMessagesInChatRoom();
        ExpectedUnreadMessages = GetUnreadMessages();
        ExpectedEmptyMessagesList = GetEmptyMessages();
        ExpectedSingleMessage = GetSingleMessage();
        ExpectedMessageWithMinDateTime = GetMessageWithMinDateATime();
        ExpectedMessageWithMaxDateTime = GetMessageWithMaxDateATime();
        ExpectedUserChatRoomWithLastReadMessage = GetUserChatRoomWithLastReadMessage();
        ExpectedUserChatRoomWithNullLastReadMessage = GetUserChatRoomWithNullLastReadMessage();

        MockMessageRepository = fixture.Freeze<Mock<IMessageRepository>>();
        MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();
        MockUserService = fixture.Freeze<Mock<IUserService>>();
        MockUserChatRoomRepository = fixture.Freeze<Mock<IUserChatRoomRepository>>();
        MockChatRoomRepository = fixture.Freeze<Mock<IChatRoomRepository>>();

        MessageService = new MessageService(
            MockMessageRepository.Object,
            MockUserChatRoomRepository.Object,
            MockChatRoomRepository.Object,
            MockUserService.Object,
            MockLoggerManager.Object);
    }

    public MessageService MessageService { get; }
    
    public Mock<IMessageRepository> MockMessageRepository { get; }
    public Mock<ILoggerManager> MockLoggerManager { get; }
    public Mock<IUserService> MockUserService { get; }
    public Mock<IUserChatRoomRepository> MockUserChatRoomRepository { get; }
    public Mock<IChatRoomRepository> MockChatRoomRepository { get; }

    public List<Message> ExpectedMessagesInChatRoom { get; }
    public List<Message> ExpectedUnreadMessages { get; }
    public List<Message> ExpectedEmptyMessagesList { get; }
    public Message ExpectedSingleMessage { get; }
    public Message ExpectedMessageWithMinDateTime { get; }
    public Message ExpectedMessageWithMaxDateTime { get; }
    public Message? ExpectedNullMessage { get => null; }
    public UserChatRoom ExpectedUserChatRoomWithLastReadMessage { get; }
    public UserChatRoom ExpectedUserChatRoomWithNullLastReadMessage { get; }
    public UserChatRoom? ExpectedNullUserChatRoom { get => null; }
    
    private List<Message> GetMessagesInChatRoom()
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
    private List<Message> GetUnreadMessages()
    {
        var messages = new List<Message>()
        {
            new Message()
            {
                Id = 4,
                ChatRoomId = 2,
                SenderId = 3,
                SentAt = DateTime.Now,
                Text = "Message text"
            },
            new Message()
            {
                Id = 5,
                ChatRoomId = 2,
                SenderId = 3,
                SentAt = DateTime.Now,
                Text = "Message text"
            }
        };

        return messages;
    }
    private Message GetSingleMessage()
    {
        return new Message()
        {
            Id = 6,
            ChatRoomId = 2,
            SenderId = 3,
            SentAt = DateTime.Now,
            Text = "Message text"
        };
    }
    private Message GetMessageWithMinDateATime()
    {
        return new Message()
        {
            Id = 1,
            ChatRoomId = 1,
            SenderId = 1,
            SentAt = DateTime.MinValue,
            Text = "Message text"
        };
    }
    private Message GetMessageWithMaxDateATime()
    {
        return new Message()
        {
            Id = 1,
            ChatRoomId = 1,
            SenderId = 1,
            SentAt = DateTime.MaxValue,
            Text = "Message text"
        };
    }
    private List<Message> GetEmptyMessages()
    {
        return new List<Message>();
    }
    private UserChatRoom GetUserChatRoomWithNullLastReadMessage()
    {
        return new UserChatRoom()
        {
            ChatRoomId = 1,
            UserId = 1
        };
    }
    private UserChatRoom GetUserChatRoomWithLastReadMessage()
    {
        return new UserChatRoom()
        {
            ChatRoomId = 2,
            UserId = 1,
            LastReadMessageId = 6,
            LastReadMessage = GetSingleMessage()
        };
    }
}