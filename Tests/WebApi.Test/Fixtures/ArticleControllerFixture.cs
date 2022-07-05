using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.ArticleViewModels;
using Moq;
using WebApi.AutoMapper.Interface;
using WebApi.Controllers;

namespace WebApi.Test.Fixtures;

public class ArticleControllerFixture
{
    public ArticleControllerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockArticleService = fixture.Freeze<Mock<IArticleService>>();
        MockCreateMapper = fixture.Freeze<Mock<IViewModelMapper<CreateArticleViewModel, Article>>>();
        MockUpdateMapper = fixture.Freeze<Mock<IViewModelMapper<UpdateArticleViewModel, Article>>>();
        MockReadMapper = fixture.Freeze<Mock<IViewModelMapper<Article, ReadArticleViewModel>>>();
        MockEnumerableViewModelMapper = fixture
            .Freeze<Mock<IEnumerableViewModelMapper<IEnumerable<Article>, IEnumerable<ReadArticleViewModel>>>>();

        MockArticleController = new ArticlesController(
            MockArticleService.Object, 
            MockCreateMapper.Object,
            MockUpdateMapper.Object, 
            MockReadMapper.Object, 
            MockEnumerableViewModelMapper.Object);
    }
    
    public ArticlesController MockArticleController { get; }
    public Mock<IArticleService> MockArticleService { get; }
    public Mock<IViewModelMapper<CreateArticleViewModel, Article>> MockCreateMapper { get; }
    public Mock<IViewModelMapper<UpdateArticleViewModel, Article>> MockUpdateMapper { get; }
    public Mock<IViewModelMapper<Article, ReadArticleViewModel>> MockReadMapper { get; }
    public Mock<IEnumerableViewModelMapper<IEnumerable<Article>, IEnumerable<ReadArticleViewModel>>> MockEnumerableViewModelMapper { get; }

}