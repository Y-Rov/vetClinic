using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Paginator.Parameters;
using Core.ViewModels.CommentViewModels;
using Microsoft.AspNetCore.Identity;
using Moq;
using WebApi.AutoMapper.Interface;
using WebApi.Controllers;

namespace WebApi.Test.Fixtures;

public class CommentControllerFixture
{
    public CommentControllerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockCommentService = fixture.Freeze<Mock<ICommentService>>();
        MockCreateMapper = fixture.Freeze<Mock<IViewModelMapper<CreateCommentViewModel, Comment>>>();
        MockUpdateMapper = fixture.Freeze<Mock<IViewModelMapper<UpdateCommentViewModel, Comment>>>();
        MockReadMapper = fixture.Freeze<Mock<IViewModelMapper<Comment, ReadCommentViewModel>>>();
        MockReadEnumMapper =
            fixture.Freeze<Mock<IEnumerableViewModelMapper<IEnumerable<Comment>, IEnumerable<ReadCommentViewModel>>>>();
        MockUserManager = fixture.Freeze<Mock<UserManager<User>>>();

        Comment = GetComment();
        ExpectedReadViewModel = GetExpectedReadCommentViewModel();
        Comments = GetComments();
        ExpectedReadViewModels = GetExpectedReadViewModels();
        RequestUser = GetRequestUser();
        UpdateCommentViewModel = GetUpdateCommentViewModel();
        ConcreteArticleParameters = GetConcreteArticleParameters();
        DefaultParameters = GetDefaultParameters();

        MockCommentsController = new CommentsController(
            MockCommentService.Object,
            MockCreateMapper.Object,
            MockUpdateMapper.Object,
            MockReadMapper.Object,
            MockReadEnumMapper.Object,
            MockUserManager.Object);
    }

    public CommentsController MockCommentsController { get; }
    public Mock<ICommentService> MockCommentService { get; }
    public Mock<IViewModelMapper<CreateCommentViewModel, Comment>> MockCreateMapper { get; }
    public Mock<IViewModelMapper<UpdateCommentViewModel, Comment>> MockUpdateMapper { get; }
    public Mock<IViewModelMapper<Comment, ReadCommentViewModel>> MockReadMapper { get; }

    public Mock<IEnumerableViewModelMapper<IEnumerable<Comment>, IEnumerable<ReadCommentViewModel>>> MockReadEnumMapper
    {
        get;
    }

    public Mock<UserManager<User>> MockUserManager { get; }

    public Comment Comment { get; }
    public ReadCommentViewModel ExpectedReadViewModel { get; }
    public IEnumerable<Comment> Comments { get; }
    public IEnumerable<ReadCommentViewModel> ExpectedReadViewModels { get; }
    public User RequestUser { get; }
    public UpdateCommentViewModel UpdateCommentViewModel { get; }
    public CommentsParameters ConcreteArticleParameters { get; }
    public CommentsParameters DefaultParameters { get; }

    private Comment GetComment()
    {
        var comment = new Comment()
        {
            Id = 1,
            ArticleId = 1,
            AuthorId = 1,
            Content = "hello",
            CreatedAt = new DateTime(2020, 10, 10, 10, 10, 10),
            Edited = false
        };
        return comment;
    }

    private ReadCommentViewModel GetExpectedReadCommentViewModel()
    {
        var readViewModel = new ReadCommentViewModel()
        {
            Id = 1,
            ArticleId = 1,
            AuthorId = 1,
            AuthorName = "Admin Admin",
            Content = "hello",
            CreatedAt = new DateTime(2020, 10, 10, 10, 10, 10),
            Edited = false
        };
        return readViewModel;
    }

    private IEnumerable<Comment> GetComments()
    {
        var comments = new List<Comment>()
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
        return comments;
    }

    private IEnumerable<ReadCommentViewModel> GetExpectedReadViewModels()
    {
        var readViewModels = new List<ReadCommentViewModel>()
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
        return readViewModels;
    }
    
    private User GetRequestUser()
    {
        var requestUser = new User()
        {
            Id = 1,
            FirstName = "Admin",
            LastName = "Admin"
        };
        return requestUser;
    }

    private UpdateCommentViewModel GetUpdateCommentViewModel()
    {
        var updateCommentViewModel = new UpdateCommentViewModel()
        {
            Id = 1,
            Content = "hello"
        };
        return updateCommentViewModel;
    }
    
    private CommentsParameters GetConcreteArticleParameters()
    {
        var concreteArticleParameters = new CommentsParameters() {ArticleId = 1};
        return concreteArticleParameters;
    }

    private CommentsParameters GetDefaultParameters()
    {
        var defaultParameters = new CommentsParameters();
        return defaultParameters;
    }
}