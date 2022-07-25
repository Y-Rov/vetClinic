using System.Security.Claims;
using Core.Entities;
using Core.Exceptions;
using Core.ViewModel.MessageViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using WebApi.Test.Fixtures;

namespace WebApi.Test;

public class MessagesControllerTests : IClassFixture<MessagesControllerFixture>
{
    private readonly MessagesControllerFixture _messagesControllerFixture;

    public MessagesControllerTests(MessagesControllerFixture messagesControllerFixture)
    {
        _messagesControllerFixture = messagesControllerFixture;
    }

    [Fact]
    public async Task GetMessagesInChatRoomAsync_ShouldReturnMessages()
    {
        // Arrange 
        int chatRoomId = 1;
        (int skip, int take) = (0, 20);

        _messagesControllerFixture.MockMessageService
            .Setup(service =>
                service.GetMessagesInChatRoomAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(_messagesControllerFixture.ExpectedMessages);

        _messagesControllerFixture.MockEnumMessageMapper
            .Setup(mapper =>
                mapper.Map(It.Is<IEnumerable<Message>>(m =>
                    m.Equals(_messagesControllerFixture.ExpectedMessages))))
            .Returns(_messagesControllerFixture.ExpectedMessageGetViewModels);

        // Act
        var result =
            await _messagesControllerFixture.MessagesController.GetMessagesInChatRoomAsync(chatRoomId, skip, take);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_messagesControllerFixture.ExpectedMessageGetViewModels, result);
    }

    [Fact]
    public async Task GetMessagesInChatRoomAsync_ShouldReturnEmptyMessagesList()
    {
        // Arrange 
        int chatRoomId = 1;
        (int skip, int take) = (0, 20);

        _messagesControllerFixture.MockMessageService
            .Setup(service =>
                service.GetMessagesInChatRoomAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(_messagesControllerFixture.ExpectedEmptyMessagesList);

        _messagesControllerFixture.MockEnumMessageMapper
            .Setup(mapper =>
                mapper.Map(It.Is<IEnumerable<Message>>(m =>
                    m.Equals(_messagesControllerFixture.ExpectedMessages))))
            .Returns(_messagesControllerFixture.ExpectedEmptyMessageGetViewModels);

        // Act
        var result =
            await _messagesControllerFixture.MessagesController.GetMessagesInChatRoomAsync(chatRoomId, skip, take);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMessagesInChatRoomAsync_ShouldThrowNotFoundException_WhenChatRoomDoesNotExist()
    {
        // Arrange 
        int chatRoomId = 1;
        (int skip, int take) = (0, 20);

        _messagesControllerFixture.MockMessageService
            .Setup(service =>
                service.GetMessagesInChatRoomAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Throws<NotFoundException>();

        // Act
        var result = _messagesControllerFixture.MessagesController.GetMessagesInChatRoomAsync(chatRoomId, skip, take);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }

    [Fact]
    public async Task GetUnreadMessagesAsync_ShouldReturnMessages()
    {
        // Arrange 
        _messagesControllerFixture.MockMessageService
            .Setup(service =>
                service.GetUnreadMessagesAsync(It.IsAny<int>()))
            .ReturnsAsync(_messagesControllerFixture.ExpectedMessages);

        _messagesControllerFixture.MockUserManager
            .Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(_messagesControllerFixture.ExpectedUser);

        _messagesControllerFixture.MockEnumMessageMapper
            .Setup(mapper =>
                mapper.Map(It.Is<IEnumerable<Message>>(m =>
                    m.Equals(_messagesControllerFixture.ExpectedMessages))))
            .Returns(_messagesControllerFixture.ExpectedMessageGetViewModels);

        // Act
        var result = await _messagesControllerFixture.MessagesController.GetUnreadMessagesAsync();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(_messagesControllerFixture.ExpectedMessageGetViewModels, result);
    }

    [Fact]
    public async Task GetUnreadMessagesAsync_ShouldReturnEmptyMessagesList()
    {
        // Arrange 
        _messagesControllerFixture.MockMessageService
            .Setup(service =>
                service.GetUnreadMessagesAsync(It.IsAny<int>()))
            .ReturnsAsync(_messagesControllerFixture.ExpectedEmptyMessagesList);

        _messagesControllerFixture.MockUserManager
            .Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(_messagesControllerFixture.ExpectedUser);

        _messagesControllerFixture.MockEnumMessageMapper
            .Setup(mapper =>
                mapper.Map(It.Is<IEnumerable<Message>>(m =>
                    m.Equals(_messagesControllerFixture.ExpectedMessages))))
            .Returns(_messagesControllerFixture.ExpectedEmptyMessageGetViewModels);

        // Act
        var result = await _messagesControllerFixture.MessagesController.GetUnreadMessagesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public void SendPrivateMessageAsync_ShouldSuccessfullySendMessage()
    {
        // Arrange
        _messagesControllerFixture.MockUserManager
            .Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(_messagesControllerFixture.ExpectedUser);

        _messagesControllerFixture.MockChatRoomService
            .Setup(service => service.EnsurePrivateRoomCreatedAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(_messagesControllerFixture.ExpectedPrivateChatRoom)
            .Verifiable();

        _messagesControllerFixture.MockMessageGetMapper
            .Setup(service => service.Map(It.IsAny<Message>()))
            .Returns(_messagesControllerFixture.ExpectedMessageGetViewModel);

        _messagesControllerFixture.MockHubClients
            .Setup(clients => clients.User(It.IsAny<string>()))
            .Returns(_messagesControllerFixture.MockClientProxy.Object);

        _messagesControllerFixture.MockHubContext
            .Setup(x => x.Clients)
            .Returns(() => _messagesControllerFixture.MockHubClients.Object);

        // Act 
        var result = _messagesControllerFixture.MessagesController
            .SendPrivateMessageAsync(_messagesControllerFixture.ExpectedMessageSendViewModel);

        // Assert
        _messagesControllerFixture.MockHubContext
            .Verify(hubContext => hubContext.Clients, Times.Once);
        
        _messagesControllerFixture.MockHubContext.Verify();
        Assert.NotNull(result);
    }
    
    [Fact]
    public void SendPrivateMessageAsync_ShouldThrowNotFoundException_WhenReceiverDoesNotExist()
    {
        // Arrange
        _messagesControllerFixture.MockUserManager
            .Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(_messagesControllerFixture.ExpectedUser);

        _messagesControllerFixture.MockChatRoomService
            .Setup(service => service.EnsurePrivateRoomCreatedAsync(It.IsAny<int>(), It.IsAny<int>()))
            .Throws<NotFoundException>();
        
        // Act 
        var result = _messagesControllerFixture.MessagesController
            .SendPrivateMessageAsync(_messagesControllerFixture.ExpectedMessageSendViewModel);

        // Assert
        Assert.ThrowsAsync<NotFoundException>(() => result);
    }
        
    [Fact]
    public void ReadMessageAsync_ShouldSuccessfullyReadMessage()
    {
        // Arrange
        int messageId = 1;
        
        _messagesControllerFixture.MockUserManager
            .Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(_messagesControllerFixture.ExpectedUser);
        
        _messagesControllerFixture.MockMessageService
            .Setup(service => service.ReadAsync(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(Task.FromResult<object?>(null))
            .Verifiable();
        
        // Act
        var result = _messagesControllerFixture.MessagesController.ReadMessageAsync(messageId);

        // Assert
        _messagesControllerFixture.MockMessageService.Verify();
        Assert.NotNull(result);
    }

    [Fact] 
    public void ReadMessageAsync_ShouldThrowNotFoundException_WhenMessageDoesNotExist()
    {
        // Arrange
        int messageId = 1;
        
        _messagesControllerFixture.MockUserManager
            .Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(_messagesControllerFixture.ExpectedUser);

        _messagesControllerFixture.MockMessageService
            .Setup(service => service.ReadAsync(It.IsAny<int>(), It.IsAny<int>()))
            .Throws<NotFoundException>();
        
        // Act
        var result = _messagesControllerFixture.MessagesController.ReadMessageAsync(messageId);

        // Assert
        Assert.ThrowsAsync<NotFoundException>(() => result);
    }
}