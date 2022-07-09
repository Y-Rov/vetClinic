using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Paginator;
using Core.Paginator.Parameters;
using Core.ViewModels;
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
        MockReadPagedMapper = fixture
            .Freeze<Mock<IViewModelMapper<PagedList<Article>, PagedReadViewModel<ReadArticleViewModel>>>>();

        Article = GetArticle();
        ExpectedReadArticleViewModel = GetExpectedReadArticleViewModel();
        CreateArticleViewModel = GetCreateArticleViewModel();
        UpdateArticleViewModel = GetUpdateArticleViewModel();
        PagedArticles = GetPagedArticles();
        PagedReadViewModel = GetPagedReadViewModel();
        Parameters = GetParameters();
        
        MockArticleController = new ArticlesController(
            MockArticleService.Object, 
            MockCreateMapper.Object,
            MockUpdateMapper.Object, 
            MockReadMapper.Object, 
            MockReadPagedMapper.Object);
    }
    
    public ArticlesController MockArticleController { get; }
    public Mock<IArticleService> MockArticleService { get; }
    public Mock<IViewModelMapper<CreateArticleViewModel, Article>> MockCreateMapper { get; }
    public Mock<IViewModelMapper<UpdateArticleViewModel, Article>> MockUpdateMapper { get; }
    public Mock<IViewModelMapper<Article, ReadArticleViewModel>> MockReadMapper { get; }
    public Mock<IViewModelMapper<PagedList<Article>, PagedReadViewModel<ReadArticleViewModel>>> MockReadPagedMapper { get; }

    public Article Article { get; }
    public ReadArticleViewModel ExpectedReadArticleViewModel { get; }
    public CreateArticleViewModel CreateArticleViewModel { get; }
    public UpdateArticleViewModel UpdateArticleViewModel  { get; }
    public PagedList<Article> PagedArticles { get; }
    public PagedReadViewModel<ReadArticleViewModel> PagedReadViewModel { get; }
    public ArticleParameters Parameters { get; }
    
    private Article GetArticle()
    {
        var article = new Article()
        {
            Id = 1,
            AuthorId = 1,
            Body = "article body",
            Title = "article title",
            CreatedAt = new DateTime(2020, 10, 10, 10, 10, 10),
            Published = true,
            Edited = false,
        };
        return article;
    }

    private ReadArticleViewModel GetExpectedReadArticleViewModel()
    {
        var readViewModel = new ReadArticleViewModel()
        {
            Id = 1,
            AuthorId = 1,
            Body = "article body",
            Title = "article title",
            CreatedAt = new DateTime(2020, 10, 10, 10, 10, 10),
            Published = true,
            Edited = false,
        };
        return readViewModel;
    }

    private CreateArticleViewModel GetCreateArticleViewModel()
    {
        var createArticleViewModel = new CreateArticleViewModel()
        {
            AuthorId = 1,
            Body = "article body",
            Title = "article title",
            Published = true
        };
        return createArticleViewModel;
    }

    private UpdateArticleViewModel GetUpdateArticleViewModel()
    {
        var updateArticleViewModel = new UpdateArticleViewModel()
        {
            Id = 1,
            Body = "article body",
            Title = "article title",
            Published = true
        };
        return updateArticleViewModel;
    }

    private PagedList<Article> GetPagedArticles()
    {
        var articles = new List<Article>()
        {
            new Article()
            {
                Id = 1,
                AuthorId = 1,
                Body = "article body",
                Title = "article title",
                CreatedAt = new DateTime(2020, 10, 10, 10, 10, 10),
                Published = true,
                Edited = false,
            },
            new Article()
            {
                Id = 2,
                AuthorId = 1,
                Body = "article body",
                Title = "article title",
                CreatedAt = new DateTime(2020, 10, 11, 10, 10, 10),
                Published = false,
                Edited = true,
            },
            new Article()
            {
                Id = 3,
                AuthorId = 1,
                Body = "article body",
                Title = "article title",
                CreatedAt = new DateTime(2020, 10, 12, 10, 10, 10),
                Published = false,
                Edited = false,
            }
        };
        var pagedArticles = new PagedList<Article>(articles, 3, 1, 5);
        return pagedArticles;
    }

    private PagedReadViewModel<ReadArticleViewModel> GetPagedReadViewModel()
    {
        var readViewModels = new List<ReadArticleViewModel>()
        {
            new ReadArticleViewModel()
            {
                Id = 1,
                AuthorId = 1,
                Body = "article body",
                Title = "article title",
                CreatedAt = new DateTime(2020, 10, 10, 10, 10, 10),
                Published = true,
                Edited = false,
            },
            new ReadArticleViewModel()
            {
                Id = 2,
                AuthorId = 1,
                Body = "article body",
                Title = "article title",
                CreatedAt = new DateTime(2020, 10, 11, 10, 10, 10),
                Published = false,
                Edited = true,
            },
            new ReadArticleViewModel()
            {
                Id = 3,
                AuthorId = 1,
                Body = "article body",
                Title = "article title",
                CreatedAt = new DateTime(2020, 10, 12, 10, 10, 10),
                Published = false,
                Edited = false,
            }
        };
        
        var readPagedViewModels = new PagedReadViewModel<ReadArticleViewModel>()
        {
            CurrentPage = 1,
            Entities = readViewModels,
            HasNext = false,
            HasPrevious = false,
            PageSize = 5,
            TotalCount = 3,
            TotalPages = 1
        };
        return readPagedViewModels;
    }

    private ArticleParameters GetParameters()
    {
        var parameters = new ArticleParameters()
        {
            FilterParam = "hello",
            OrderByParam = "Title",
            OrderByDirection = "desc",
            PageNumber = 1,
            PageSize = 5
        };
        return parameters;
    }
}