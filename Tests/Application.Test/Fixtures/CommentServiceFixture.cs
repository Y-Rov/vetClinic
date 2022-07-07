using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Moq;

namespace Application.Test.Fixtures;

public class CommentServiceFixture
{
    public CommentServiceFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockCommentRepository = fixture.Freeze<Mock<ICommentRepository>>();
        MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();

        ExpectedComment = GetExpectedComment();
        UpdatedComment = GetUpdatedComment();
        ExpectedComments = GetExpectedComments();
        RequestUser = GetRequestUser();
        
        MockCommentService = new CommentService(
            MockCommentRepository.Object,
            MockLoggerManager.Object);
    }

    public CommentService MockCommentService { get; }
    public Mock<ICommentRepository> MockCommentRepository { get; }
    public Mock<ILoggerManager> MockLoggerManager { get; }

    public Comment ExpectedComment { get; }
    public Comment UpdatedComment { get; }
    public IList<Comment> ExpectedComments { get; }
    public User RequestUser { get; }

    private Comment GetExpectedComment()
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
    private Comment GetUpdatedComment()
    {
        var updatedComment = new Comment()
        {
            Id = 1,
            Content = "updated hello",
        };
        return updatedComment;
    }
    private IList<Comment> GetExpectedComments()
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
}