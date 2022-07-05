using System.Security.Claims;
using Core.Entities;
using Core.Exceptions;
using Core.Paginator.Parameters;
using Core.ViewModels.ArticleViewModels;
using Core.ViewModels.CommentViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Test.Fixtures;

namespace WebApi.Test;

public class CommentsControllerTests : IClassFixture<CommentControllerFixture>
{
    private readonly CommentControllerFixture _fixture;

    public CommentsControllerTests(CommentControllerFixture fixture)
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

    private readonly ReadCommentViewModel _readViewModel = new ReadCommentViewModel()
    {
        Id = 1,
        ArticleId = 1,
        AuthorId = 1,
        AuthorName = "Admin Admin",
        Content = "hello",
        CreatedAt = new DateTime(2020, 10, 10, 10, 10, 10),
        Edited = false
    };

    private readonly IEnumerable<ReadCommentViewModel> _readViewModels = new List<ReadCommentViewModel>()
    {
        new ReadCommentViewModel()
        {
            Id = 1,
            ArticleId = 1,
            AuthorId = 1,
            Content = "first hello",
            AuthorName = "Admin Admin",
            CreatedAt = new DateTime(2020, 10, 10, 10, 10, 10),
            Edited = false
        },
        new ReadCommentViewModel()
        {
            Id = 2,
            ArticleId = 1,
            AuthorId = 1,
            Content = "second hello",
            AuthorName = "Admin Admin",
            CreatedAt = new DateTime(2020, 10, 10, 10, 10, 20),
            Edited = false
        },
        new ReadCommentViewModel()
        {
            Id = 1,
            ArticleId = 2,
            AuthorId = 1,
            Content = "third hello",
            AuthorName = "Admin Admin",
            CreatedAt = new DateTime(2020, 10, 10, 10, 10, 30),
            Edited = true
        }
    };

    private readonly IEnumerable<Comment> _comments = new List<Comment>()
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
    
    private readonly UpdateCommentViewModel _updateCommentViewModel = new UpdateCommentViewModel()
    {
        Id = 1,
        Content = "hello"
    };

    private readonly CommentsParameters _concreteArticleParameters = new CommentsParameters() {ArticleId = 1};
    private readonly CommentsParameters _defaultParameters = new CommentsParameters();

    [Fact]
    public async Task GetCommentById_whenIdIsCorrect_thenStatusCodeOkReturned()
    {
        //Arrange
        _fixture.MockCommentService
            .Setup(service =>
                service.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(_comment);
        _fixture.MockReadMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<Comment>()))
            .Returns(_readViewModel);
        //Act
        var result = await _fixture.MockCommentsController.GetByIdAsync(1);
        //Assert
        Assert.NotNull(result);
        Assert.Equal(result, _readViewModel);
    }

    [Fact]
    public async Task GetCommentById_whenIdIsIncorrect_thenStatusCodeNotFoundReturned()
    {
        //Arrange
        _fixture.MockCommentService
            .Setup(service =>
                service.GetByIdAsync(It.IsAny<int>()))
            .Throws<NotFoundException>();
        _fixture.MockReadMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<Comment>()))
            .Returns(_readViewModel);
        //Act
        var result = _fixture.MockCommentsController.GetByIdAsync(1);
        //  Assert
        Assert.NotNull(result);
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }
    
    [Fact]
    public async Task GetAll_whenCommentsListIsNotEmpty_thenStatusOkReturned()
    {
        //  Arrange
        _fixture.MockCommentService
            .Setup(service =>
                service.GetAllCommentsAsync(It.IsAny<CommentsParameters>()))
            .ReturnsAsync(_comments);

        _fixture.MockReadEnumMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<IEnumerable<Comment>>()))
            .Returns(_readViewModels);

        //  Act
        var result = await _fixture.MockCommentsController.GetAsync(_defaultParameters);

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, _readViewModels);
    }

    [Fact]
    public async Task GetAll_whenCommentsListIsEmpty_thenStatusOkReturned()
    {
        //  Arrange
        var emptyComments = new List<Comment>();

        var emptyCommentReadViewModels = new List<ReadCommentViewModel>();

        _fixture.MockCommentService
            .Setup(service =>
                service.GetAllCommentsAsync(It.IsAny<CommentsParameters>()))
            .ReturnsAsync(emptyComments);

        _fixture.MockReadEnumMapper
            .Setup(mapper =>
                mapper.Map(It.Is<IEnumerable<Comment>>(p => p.Equals(emptyComments))))
            .Returns(emptyCommentReadViewModels);

        //  Act
        var result = await _fixture.MockCommentsController.GetAsync(_defaultParameters);

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, emptyCommentReadViewModels);
    }
    
    [Fact]
    public async Task GetAllCommentComments_whenCommentsListIsNotEmpty_thenStatusOkReturned()
    {
        //  Arrange
        _fixture.MockCommentService
            .Setup(service =>
                service.GetAllCommentsAsync(It.IsAny<CommentsParameters>()))
            .ReturnsAsync(_comments);

        _fixture.MockReadEnumMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<IEnumerable<Comment>>()))
            .Returns(_readViewModels);

        //  Act
        var result = await _fixture.MockCommentsController.GetAsync(_concreteArticleParameters);

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, _readViewModels);
    }

    [Fact]
    public async Task GetAllCommentComments_whenCommentsListIsEmpty_thenStatusOkReturned()
    {
        //  Arrange
        var emptyPublishedComments = new List<Comment>();

        var emptyPublishedCommentReadViewModels = new List<ReadCommentViewModel>();

        _fixture.MockCommentService
            .Setup(service =>
                service.GetAllCommentsAsync(It.IsAny<CommentsParameters>()))
            .ReturnsAsync(emptyPublishedComments);

        _fixture.MockReadEnumMapper
            .Setup(mapper =>
                mapper.Map(It.Is<IEnumerable<Comment>>(p => p.Equals(emptyPublishedComments))))
            .Returns(emptyPublishedCommentReadViewModels);

        //  Act
        var result = await _fixture.MockCommentsController.GetAsync(_concreteArticleParameters);

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, emptyPublishedCommentReadViewModels);
    }
    
     [Fact]
    public async Task Delete_whenCommentExist_thenStatusOkReturned()
    {
        //  Arrange
        _fixture.MockUserManager
            .Setup(um => um
                .GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(_requestUser).Verifiable();
        
        _fixture.MockCommentService
            .Setup(service =>
                service.DeleteCommentAsync(
                    It.IsAny<int>(), 
                    It.IsAny<User>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Mock of the controller does not contain httpContext by default.
        //This code is creating a plug to avoid nullReferenceException`s, thrown because
        //the controller retrieves the user by httpContext.User.
        //See WebApi\Controllers\CommentsController.cs:line 75 for the exaple
        _fixture.MockCommentsController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal()
            }
        };
        
        //  Act
        await _fixture.MockCommentsController.DeleteAsync(1);

        //  Assert
        _fixture.MockCommentService.Verify();
        _fixture.MockUserManager.ResetCalls();
    }
    
    [Fact]
    public async Task Delete_whenCommentDoesNotExist_thenStatusNotFoundReturned()
    {
        //  Arrange
        _fixture.MockUserManager
            .Setup(um => um
                .GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(_requestUser).Verifiable();
        
        _fixture.MockCommentService
            .Setup(service =>
                service.DeleteCommentAsync(
                    It.IsAny<int>(),
                    It.IsAny<User>()))
            .Throws<NotFoundException>();
        
        _fixture.MockCommentsController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal()
            }
        };
        
        //  Act
        var result =  _fixture.MockCommentsController.DeleteAsync(65);

        //  Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
        _fixture.MockUserManager.ResetCalls();
    }
    
    [Fact]
    public async Task Delete_whenWrongUser_thenStatusBadRequestReturned()
    {
        //  Arrange
        _fixture.MockUserManager
            .Setup(um => um
                .GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(_requestUser).Verifiable();
        
        _fixture.MockCommentService
            .Setup(service =>
                service.DeleteCommentAsync(
                    It.IsAny<int>(),
                    It.IsAny<User>()))
            .Throws<BadRequestException>();
        
        _fixture.MockCommentsController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal()
            }
        };
        
        //  Act
        var result =  _fixture.MockCommentsController.DeleteAsync(65);

        //  Assert
        await Assert.ThrowsAsync<BadRequestException>(() => result);
        _fixture.MockUserManager.ResetCalls();
    }
    
    [Fact]
    public async Task Create_whenOk_thenStatusOkReturned()
    {
        //  Arrange
        var createCommentViewModel = new CreateCommentViewModel()
        {
            Content = "hello",
            ArticleId = 1
        };
        
        _fixture.MockUserManager
            .Setup(um => um
                .GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(_requestUser).Verifiable();

        _fixture.MockCreateMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<CreateCommentViewModel>()))
            .Returns(_comment);

        _fixture.MockCommentService
            .Setup(service =>
                service.CreateCommentAsync(
                    It.IsAny<Comment>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        _fixture.MockCommentsController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal()
            }
        };
        
        //  Act
        await _fixture.MockCommentsController.CreateAsync(createCommentViewModel);

        //  Assert
        _fixture.MockCreateMapper.Verify(m => m.Map(createCommentViewModel), Times.Once);
        _fixture.MockUserManager.Verify(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>()), Times.Once);
        _fixture.MockCommentService.Verify(s => s.CreateCommentAsync(_comment), Times.Once);
        _fixture.MockUserManager.ResetCalls();
    }
    
    [Fact]
    public async Task Update_whenOk_thenStatusOkReturned()
    {
        //  Arrange
        _fixture.MockUpdateMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<UpdateCommentViewModel>()))
            .Returns(_comment);
        
        _fixture.MockUserManager
            .Setup(um => um
                .GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(_requestUser).Verifiable();

        _fixture.MockCommentService
            .Setup(service =>
                service.UpdateCommentAsync(
                    It.IsAny<Comment>(),
                    It.IsAny<User>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        _fixture.MockCommentsController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal()
            }
        };
        
        //  Act
        await _fixture.MockCommentsController.UpdateAsync(_updateCommentViewModel);

        //  Assert
        _fixture.MockUpdateMapper.Verify(m => m.Map(_updateCommentViewModel), Times.Once);
        _fixture.MockUserManager.Verify(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>()), Times.Once);
        _fixture.MockCommentService.Verify(s => s.UpdateCommentAsync(_comment, _requestUser), Times.Once);
        _fixture.MockUserManager.ResetCalls();
    }

    [Fact]
    public async Task Update_whenCommentDoesNotExist_thenStatusNotFoundReturned()
    {
        //  Arrange
        _fixture.MockUserManager
            .Setup(um => um
                .GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(_requestUser).Verifiable();
        
        _fixture.MockCommentService
            .Setup(service =>
                service.UpdateCommentAsync(
                    It.IsAny<Comment>(),
                    It.IsAny<User>()))
            .Throws<NotFoundException>();
        
        _fixture.MockCommentsController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal()
            }
        };
       
        //  Act
        var result =  _fixture.MockCommentsController.UpdateAsync(_updateCommentViewModel);

        //  Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
        _fixture.MockUserManager.ResetCalls();
    }
    
    [Fact]
    public async Task Update_whenWrongUser_thenStatusBadRequestReturned()
    {
        //  Arrange
        _fixture.MockUserManager
            .Setup(um => um
                .GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(_requestUser).Verifiable();
        
        _fixture.MockCommentService
            .Setup(service =>
                service.UpdateCommentAsync(
                    It.IsAny<Comment>(),
                    It.IsAny<User>()))
            .Throws<BadRequestException>();
        
        _fixture.MockCommentsController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal()
            }
        };
       
        //  Act
        var result =  _fixture.MockCommentsController.UpdateAsync(_updateCommentViewModel);

        //  Assert
        await Assert.ThrowsAsync<BadRequestException>(() => result);
        _fixture.MockUserManager.ResetCalls();
    }
}