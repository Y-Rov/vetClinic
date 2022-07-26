using System.Linq.Expressions;
using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace Application.Test;

public class MessageServiceTests : IClassFixture<MessageServiceFixture>, IDisposable
{
    private readonly MessageServiceFixture _messageServiceFixture;
    private bool _disposed;
    
    public MessageServiceTests(MessageServiceFixture messageServiceFixture)
    {
        _messageServiceFixture = messageServiceFixture;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _messageServiceFixture.MockMessageRepository.ResetCalls();
            _messageServiceFixture.MockChatRoomRepository.ResetCalls();
            _messageServiceFixture.MockUserChatRoomRepository.ResetCalls();
        }

        _disposed = true;
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnSingleMessage()
    {
        // Arrange
        int id = 6;
        
        _messageServiceFixture.MockMessageRepository
            .Setup(repo => repo.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Message, bool>>>(),
                It.IsAny<Func<IQueryable<Message>, IIncludableQueryable<Message, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(_messageServiceFixture.ExpectedSingleMessage);

        // Act
        Message? result = await _messageServiceFixture.MessageService.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result!.Id, _messageServiceFixture.ExpectedSingleMessage.Id);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull()
    {
        // Arrange
        int id = 20;
        
        _messageServiceFixture.MockMessageRepository
            .Setup(repo => repo.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Message, bool>>>(),
                It.IsAny<Func<IQueryable<Message>, IIncludableQueryable<Message, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(_messageServiceFixture.ExpectedNullMessage);

        // Act
        Message? result = await _messageServiceFixture.MessageService.GetByIdAsync(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetMessagesInChatRoomAsync_ShouldReturnMessages()
    {
        // Arrange
        int chatRoomId = 1;
        (int skip, int take) = (0, 20);
        
        _messageServiceFixture.MockChatRoomRepository.Setup(repo =>
                repo.ExistsAsync(It.Is<int>(id => id == chatRoomId)))
            .ReturnsAsync(true);

        _messageServiceFixture.MockMessageRepository.Setup(repo =>
                repo.QueryAsync(
                    It.IsAny<Expression<Func<Message, bool>>>(),
                    It.IsAny<Func<IQueryable<Message>, IOrderedQueryable<Message>>>(),
                    It.IsAny<Func<IQueryable<Message>, IIncludableQueryable<Message, object>>>(),
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(_messageServiceFixture.ExpectedMessagesInChatRoom);

        _messageServiceFixture.MockLoggerManager
            .Setup(logger => logger.LogInfo(It.IsAny<string>()))
            .Verifiable();
        
        // Act 
        var result = await _messageServiceFixture.MessageService
            .GetMessagesInChatRoomAsync(chatRoomId, skip, take);

        // Assert
        _messageServiceFixture.MockLoggerManager.Verify();
        Assert.NotNull(result);
        Assert.Equal(_messageServiceFixture.ExpectedMessagesInChatRoom, result);
        Assert.Equal(_messageServiceFixture.ExpectedMessagesInChatRoom.Count, result.Count());
    }
    
    [Fact]
    public async Task GetMessagesInChatRoomAsync_ShouldReturnEmptyList()
    {
        // Arrange
        int chatRoomId = 1;
        (int skip, int take) = (0, 20);
        
        _messageServiceFixture.MockChatRoomRepository.Setup(repo =>
                repo.ExistsAsync(It.Is<int>(id => id == chatRoomId)))
            .ReturnsAsync(true);

        _messageServiceFixture.MockMessageRepository.Setup(repo =>
                repo.QueryAsync(
                    It.IsAny<Expression<Func<Message, bool>>>(),
                    It.IsAny<Func<IQueryable<Message>, IOrderedQueryable<Message>>>(),
                    It.IsAny<Func<IQueryable<Message>, IIncludableQueryable<Message, object>>>(),
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(_messageServiceFixture.ExpectedEmptyMessagesList);
        
        _messageServiceFixture.MockLoggerManager
            .Setup(logger => logger.LogInfo(It.IsAny<string>()))
            .Verifiable();
        
        // Act 
        var result = await _messageServiceFixture.MessageService
            .GetMessagesInChatRoomAsync(chatRoomId, skip, take);

        // Assert
        _messageServiceFixture.MockLoggerManager.Verify();
        Assert.NotNull(result);
        Assert.Equal(_messageServiceFixture.ExpectedEmptyMessagesList, result);
        Assert.Equal(_messageServiceFixture.ExpectedEmptyMessagesList.Count, result.Count());
    }
    
    [Fact]
    public void GetMessagesInChatRoomAsync_ShouldThrowNotFoundException_WhenChatRoomDoesNotExist()
    {
        // Arrange
        int chatRoomId = 1;
        (int skip, int take) = (0, 20);
        
        _messageServiceFixture.MockChatRoomRepository.Setup(repo =>
                repo.ExistsAsync(It.Is<int>(id => id == chatRoomId)))
            .ReturnsAsync(false);
        
        _messageServiceFixture.MockLoggerManager
            .Setup(logger => logger.LogWarn(It.IsAny<string>()))
            .Verifiable();

        // Act 
        Task<IEnumerable<Message>> result = _messageServiceFixture.MessageService
            .GetMessagesInChatRoomAsync(chatRoomId, skip, take);

        // Assert
        _messageServiceFixture.MockLoggerManager.Verify();
        Assert.ThrowsAsync<NotFoundException>(() => result);
    }
    
    [Fact]
    public void CreateAsync_ShouldCreateMessage()
    {
        // Arrange 
        _messageServiceFixture.MockChatRoomRepository
            .Setup(repo => repo.ExistsAsync(It.IsAny<int>()))
            .ReturnsAsync(true);

        _messageServiceFixture.MockLoggerManager
            .Setup(logger => logger.LogInfo(It.IsAny<string>()))
            .Verifiable();
        
        // Act
        var result = _messageServiceFixture.MessageService.CreateAsync(_messageServiceFixture.ExpectedSingleMessage);

        // Assert
        _messageServiceFixture.MockLoggerManager.Verify();
        
        _messageServiceFixture.MockMessageRepository
            .Verify(repo => repo.InsertAsync(It.IsAny<Message>()), Times.Once);
        
        _messageServiceFixture.MockMessageRepository
            .Verify(repo => repo.SaveChangesAsync(), Times.Once);
        
        Assert.NotNull(result);
    }
    
    [Fact]
    public void CreateAsync_ShouldThrowNotFoundException_WhenChatRoomDoesNotExist()
    {
        // Arrange 
        _messageServiceFixture.MockChatRoomRepository
            .Setup(repo => repo.ExistsAsync(It.IsAny<int>()))
            .ReturnsAsync(false);
        
        _messageServiceFixture.MockLoggerManager
            .Setup(logger => logger.LogWarn(It.IsAny<string>()))
            .Verifiable();

        // Act
        var result = _messageServiceFixture.MessageService.CreateAsync(_messageServiceFixture.ExpectedSingleMessage);

        // Assert
        _messageServiceFixture.MockLoggerManager.Verify();
        Assert.ThrowsAsync<NotFoundException>(() => result);
    }

    [Fact]
    public void ReadAsync_ShouldSuccessfullyReadAndUpdateLastReadMessage()
    {
        // Arrange
        int readerId = 1;
        int messageId = 1;

        var userChatRoom = _messageServiceFixture.ExpectedUserChatRoomWithLastReadMessage;
        userChatRoom!.LastReadMessage!.SentAt = DateTime.MinValue;

        _messageServiceFixture.MockMessageRepository
            .Setup(repo => repo.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Message, bool>>>(),
                It.IsAny<Func<IQueryable<Message>, IIncludableQueryable<Message, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(_messageServiceFixture.ExpectedMessageWithMaxDateTime);
        
        _messageServiceFixture.MockUserChatRoomRepository
            .Setup(repo => repo.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<UserChatRoom, bool>>>(),
                It.IsAny<Func<IQueryable<UserChatRoom>, IIncludableQueryable<UserChatRoom, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(userChatRoom);
        
        _messageServiceFixture.MockLoggerManager
            .Setup(logger => logger.LogInfo(It.IsAny<string>()))
            .Verifiable();

        // Act
        var result = _messageServiceFixture.MessageService.ReadAsync(readerId, messageId);
        
        // Assert
        _messageServiceFixture.MockLoggerManager.Verify();
        
        _messageServiceFixture.MockMessageRepository
            .Verify(repo => repo.SaveChangesAsync(), Times.Once);
        
        Assert.NotNull(result);
    }

    [Fact]
    public void ReadAsync_ShouldSuccessfullyReadWithoutUpdatingLastReadMessage()
    {
        // Arrange
        int readerId = 1;
        int messageId = 1;

        var userChatRoom = _messageServiceFixture.ExpectedUserChatRoomWithLastReadMessage;
        userChatRoom!.LastReadMessage!.SentAt = DateTime.MaxValue;

        _messageServiceFixture.MockMessageRepository
            .Setup(repo => repo.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Message, bool>>>(),
                It.IsAny<Func<IQueryable<Message>, IIncludableQueryable<Message, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(_messageServiceFixture.ExpectedMessageWithMinDateTime);
        
        _messageServiceFixture.MockUserChatRoomRepository
            .Setup(repo => repo.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<UserChatRoom, bool>>>(),
                It.IsAny<Func<IQueryable<UserChatRoom>, IIncludableQueryable<UserChatRoom, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(userChatRoom);
        
        _messageServiceFixture.MockLoggerManager
            .Setup(logger => logger.LogInfo(It.IsAny<string>()))
            .Verifiable();

        // Act
        var result = _messageServiceFixture.MessageService.ReadAsync(readerId, messageId);
        
        // Assert
        _messageServiceFixture.MockLoggerManager.Verify();
        
        _messageServiceFixture.MockMessageRepository
            .Verify(repo => repo.SaveChangesAsync(), Times.Never);

        Assert.NotNull(result);
    }

    [Fact]
    public void ReadAsync_ShouldThrowNotFoundException_WhenMessagesDoesNotExist()
    {
        // Arrange
        int readerId = 1;
        int messageId = 1;
        
        _messageServiceFixture.MockMessageRepository
            .Setup(repo => repo.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Message, bool>>>(),
                It.IsAny<Func<IQueryable<Message>, IIncludableQueryable<Message, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(_messageServiceFixture.ExpectedNullMessage);
        
        _messageServiceFixture.MockLoggerManager
            .Setup(logger => logger.LogWarn(It.IsAny<string>()))
            .Verifiable();
        
        // Act
        var result = _messageServiceFixture.MessageService.ReadAsync(readerId, messageId);
        
        // Assert
        _messageServiceFixture.MockLoggerManager.Verify();
        Assert.ThrowsAsync<NotFoundException>(() => result);
    }
}