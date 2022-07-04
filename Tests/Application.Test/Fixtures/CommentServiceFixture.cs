using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
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
        
        MockCommentService = new CommentService(
            MockCommentRepository.Object,
            MockLoggerManager.Object);
    }

    public CommentService MockCommentService; 
    public Mock<ICommentRepository> MockCommentRepository;
    public Mock<ILoggerManager> MockLoggerManager;

}