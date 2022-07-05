using System.Linq.Expressions;
using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.Test;

public class CommentServiceTests : IClassFixture<CommentServiceFixture>
{
    private readonly CommentServiceFixture _fixture;

    public CommentServiceTests(CommentServiceFixture fixture)
    {
        _fixture = fixture;
    }
    
    private readonly Comment _comment = new Comment()
    {
        Id = 1,
        ArticleId = 1,
        AuthorId = 1,
        Content = "hello",
        CreatedAt = new DateTime(2020, 10, 10, 10, 10, 10),
        Edited = false
    };

    private readonly Comment _updatedComment = new Comment()
    {
        Id = 1,
        Content = "updated hello",
    };
    
    private readonly IList<Comment> _comments = new List<Comment>()
    {
        new Comment()
        {
            Id = 1,
            ArticleId = 1,
            AuthorId = 1,
            Content = "first hello",
            CreatedAt = new DateTime(2020, 10, 10, 10, 10, 10),
            Edited = false
        },
        new Comment()
        {
            Id = 2,
            ArticleId = 1,
            AuthorId = 1,
            Content = "second hello",
            CreatedAt = new DateTime(2020, 10, 10, 10, 10, 20),
            Edited = false
        },
        new Comment()
        {
            Id = 1,
            ArticleId = 2,
            AuthorId = 1,
            Content = "third hello",
            CreatedAt = new DateTime(2020, 10, 10, 10, 10, 30),
            Edited = true
        }
    };

    private readonly User _requestUser = new User()
    {
        Id = 1,
        FirstName = "Admin",
        LastName = "Admin"
    };
    
    [Fact]
    public async Task GetAllCommentsAsync_whenCommentsListIsNotEmpty_thanReturnCommentsList()
    {
        _fixture.MockCommentRepository
            .Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Comment, bool>>>(),
                It.IsAny<Func<IQueryable<Comment>, IOrderedQueryable<Comment>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(_comments);
        
        //Act
        var result = await _fixture.MockCommentService.GetAllCommentsAsync();
        
        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(_comments, result);
    }
    
    [Fact]
    public async Task GetAllCommentsAsync_whenCommentsListIsEmpty_thanReturnEmptyCommentsList()
    {
        //Arrange
        var emptyComments = new List<Comment>();
        
        _fixture.MockCommentRepository
            .Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Comment, bool>>>(),
                It.IsAny<Func<IQueryable<Comment>, IOrderedQueryable<Comment>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(emptyComments);
        
        //Act
        var result = await _fixture.MockCommentService.GetAllCommentsAsync();
        
        //Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        Assert.Equal(emptyComments, result);
    }
    
    [Fact]
    public async Task GetByIdAsync_whenCommentExist_thanReturnTheComment()
    {
        //Arrange
        _fixture.MockCommentRepository
            .Setup(repo => repo.GetById(
                It.Is<int>(x => x == _comment.Id), 
                It.IsAny<string>()))
            .ReturnsAsync(_comment);
        
        //Act
        var result = await _fixture.MockCommentService.GetByIdAsync(_comment.Id);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(_comment, result);
    }
    
    [Fact]
    public async Task GetByIdAsync_whenCommentDoesNotExist_thanThrowNotFound()
    {
        //Arrange
        _fixture.MockCommentRepository
            .Setup(repo => repo.GetById(
                It.IsAny<int>(), 
                It.IsAny<string>()))
            .ReturnsAsync(() => null);
        
        //Act
        var result = _fixture.MockCommentService.GetByIdAsync(10);
        
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }
    
    [Fact]
    public async Task GetAllArticleCommentsAsync_whenCommentsListIsNotEmpty_thanReturnCommentsList()
    {
        IList<Comment> articleComments = new List<Comment>()
        {
            new Comment()
            {
                Id = 1,
                ArticleId = 1,
                AuthorId = 1,
                Content = "first hello",
                CreatedAt = new DateTime(2020, 10, 10, 10, 10, 10),
                Edited = false
            },
            new Comment()
            {
                Id = 2,
                ArticleId = 1,
                AuthorId = 1,
                Content = "second hello",
                CreatedAt = new DateTime(2020, 10, 10, 10, 10, 20),
                Edited = false
            }
        };
        
        _fixture.MockCommentRepository
            .Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Comment, bool>>>(),
                It.IsAny<Func<IQueryable<Comment>, IOrderedQueryable<Comment>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(articleComments);
        
        //Act
        var result = await _fixture.MockCommentService.GetAllCommentsAsync();
        
        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(articleComments, result);
    }

    [Fact]
    public async Task CreateCommentAsync_whenNormal_thenSuccess()
    {
        //Arrange
        _fixture.MockCommentRepository
            .Setup(r => r.InsertAsync(It.IsAny<Comment>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        _fixture.MockCommentRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        await _fixture.MockCommentService.CreateCommentAsync(_comment);
        
        //Assert
        _fixture.MockCommentRepository
            .Verify(r => r.InsertAsync(_comment), Times.Once);
        _fixture.MockCommentRepository
            .Verify(r => r.SaveChangesAsync(), Times.Once);
        _fixture.MockCommentRepository.ResetCalls();
    }
    
    [Fact]
    public async Task CreateCommentAsync_whenUserDontExist_thenThrowNotFound()
    {
        //Arrange
        _fixture.MockCommentRepository
            .Setup(r => r.InsertAsync(It.IsAny<Comment>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();

        _fixture.MockCommentRepository
            .Setup(r => r.SaveChangesAsync())
            .Throws<DbUpdateException>();
        
        //Act
        var result = _fixture.MockCommentService.CreateCommentAsync(_comment);
        
        //Assert
        _fixture.MockCommentRepository
            .Verify(r => r.InsertAsync(_comment), Times.Once);
        await Assert.ThrowsAsync<NotFoundException>(() => result);
        
        _fixture.MockCommentRepository.ResetCalls();
    }
    
    [Fact]
    public async Task DeleteCommentAsync_whenNormal_thenSuccess()
    {
        //Arrange
        _fixture.MockCommentRepository
            .Setup(r=> r.GetById(
                It.IsAny<int>(),
                It.IsAny<string>()))
            .ReturnsAsync(_comment);

        _fixture.MockCommentRepository
            .Setup(r => r.Delete(It.IsAny<Comment>()))
            .Verifiable();
        
        _fixture.MockCommentRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        await _fixture.MockCommentService.DeleteCommentAsync(1, _requestUser);
        
        //Assert
        _fixture.MockCommentRepository
            .Verify(r => r.SaveChangesAsync(), Times.Once);
        _fixture.MockCommentRepository.ResetCalls();
    }
    
    [Fact]
    public async Task DeleteCommentAsync_whenWrongUser_thenThrowBadRequest()
    {
        //Arrange
        _fixture.MockCommentRepository
            .Setup(r=> r.GetById(
                It.IsAny<int>(),
                It.IsAny<string>()))
            .ReturnsAsync(_comment);

        _fixture.MockCommentRepository
            .Setup(r => r.Delete(It.IsAny<Comment>()))
            .Verifiable();
        
        _fixture.MockCommentRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        var result = _fixture.MockCommentService.DeleteCommentAsync(1, new User(){Id = 10});
        
        //Assert
        await Assert.ThrowsAsync<BadRequestException>(() => result);
        _fixture.MockCommentRepository.ResetCalls();
    }
    
    [Fact]
    public async Task DeleteCommentAsync_whenCommentDontExist_thenThrowNotFound()
    {
        //Arrange
        _fixture.MockCommentRepository
            .Setup(r => r.GetById(
                It.IsAny<int>(),
                It.IsAny<string>()))
            .ThrowsAsync(new NotFoundException());

        _fixture.MockCommentRepository
            .Setup(r => r.Delete(It.IsAny<Comment>()))
            .Verifiable();
        
        _fixture.MockCommentRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        var result = _fixture.MockCommentService.DeleteCommentAsync(1, _requestUser);
        
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
        _fixture.MockCommentRepository.ResetCalls();
    }
    
        [Fact]
    public async Task UpdateCommentAsync_whenNormal_thenSuccess()
    {
        //Arrange
        _fixture.MockCommentRepository
            .Setup(r=> r.GetById(
                It.IsAny<int>(),
                It.IsAny<string>()))
            .ReturnsAsync(_comment);

        _fixture.MockCommentRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        await _fixture.MockCommentService.UpdateCommentAsync(_updatedComment, _requestUser);
        
        //Assert
        _fixture.MockCommentRepository
            .Verify(r => r.GetById(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        _fixture.MockCommentRepository
            .Verify(r => r.SaveChangesAsync(), Times.Once);
        _fixture.MockCommentRepository.ResetCalls();
    }
    
    [Fact]
    public async Task UpdateCommentAsync_whenWrongUser_thenThrowBadRequest()
    {
        //Arrange
        _fixture.MockCommentRepository
            .Setup(r=> r.GetById(
                It.IsAny<int>(),
                It.IsAny<string>()))
            .ReturnsAsync(_comment);

        _fixture.MockCommentRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        var result = _fixture.MockCommentService.UpdateCommentAsync(_updatedComment, new User(){Id = 10});
        
        //Assert
        await Assert.ThrowsAsync<BadRequestException>(() => result);
        _fixture.MockCommentRepository.ResetCalls();
    }
    
    [Fact]
    public async Task UpdateCommentAsync_whenCommentDontExist_thenThrowNotFound()
    {
        //Arrange
        _fixture.MockCommentRepository
            .Setup(r => r.GetById(
                It.IsAny<int>(),
                It.IsAny<string>()))
            .ThrowsAsync(new NotFoundException());

        _fixture.MockCommentRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        var result = _fixture.MockCommentService.UpdateCommentAsync(_updatedComment, _requestUser);
        
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
        _fixture.MockCommentRepository.ResetCalls();
    }
}