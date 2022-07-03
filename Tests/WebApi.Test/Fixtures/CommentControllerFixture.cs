using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
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
        MockReadEnumMapper = fixture.Freeze<Mock<IEnumerableViewModelMapper<IEnumerable<Comment>, IEnumerable<ReadCommentViewModel>>>>();
        MockUserManager = fixture.Freeze<Mock<UserManager<User>>>();

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
    public Mock<IEnumerableViewModelMapper<IEnumerable<Comment>, IEnumerable<ReadCommentViewModel>>> MockReadEnumMapper { get; }
    public Mock<UserManager<User>> MockUserManager { get; }

}