using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Application.Test.Fixtures;

public class CommentServiceFixture
{
    public CommentServiceFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockCommentRepository = fixture.Freeze<Mock<ICommentRepository>>();
        MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();
        MockUserManager = fixture.Freeze<Mock<UserManager<User>>>();
        
        MockCommentService = new CommentService(
            MockCommentRepository.Object,
            MockLoggerManager.Object,
            MockUserManager.Object);
    }

    public CommentService MockCommentService; 
    public Mock<ICommentRepository> MockCommentRepository;
    public Mock<ILoggerManager> MockLoggerManager;
    public Mock<UserManager<User>> MockUserManager;
}