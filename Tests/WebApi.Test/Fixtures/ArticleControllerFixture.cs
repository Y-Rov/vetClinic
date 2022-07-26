using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Paginator;
using Core.Paginator.Parameters;
using Core.ViewModels;
using Core.ViewModels.ArticleViewModels;
using Microsoft.AspNetCore.Identity;
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
        MockUserManager = fixture.Freeze<Mock<UserManager<User>>>();
        MockImageService = fixture.Freeze<Mock<IImageService>>();
        
        Article = GetArticle();
        ExpectedReadArticleViewModel = GetExpectedReadArticleViewModel();
        CreateArticleViewModel = GetCreateArticleViewModel();
        UpdateArticleViewModel = GetUpdateArticleViewModel();
        PagedArticles = GetPagedArticles();
        PagedReadViewModel = GetPagedReadViewModel();
        Parameters = GetParameters();
        RequestUser = GetRequestUser();
        ImageLink = GetImageLink();
        
        MockArticleController = new ArticlesController(
            MockArticleService.Object, 
            MockCreateMapper.Object,
            MockUpdateMapper.Object, 
            MockReadMapper.Object, 
            MockReadPagedMapper.Object,
            MockUserManager.Object,
            MockImageService.Object);
    }
    
    public ArticlesController MockArticleController { get; }
    public Mock<IArticleService> MockArticleService { get; }
    public Mock<IViewModelMapper<CreateArticleViewModel, Article>> MockCreateMapper { get; }
    public Mock<IViewModelMapper<UpdateArticleViewModel, Article>> MockUpdateMapper { get; }
    public Mock<IViewModelMapper<Article, ReadArticleViewModel>> MockReadMapper { get; }
    public Mock<IViewModelMapper<PagedList<Article>, PagedReadViewModel<ReadArticleViewModel>>> MockReadPagedMapper { get; }
    public Mock<UserManager<User>> MockUserManager { get; }
    public Mock<IImageService> MockImageService { get; }


    public Article Article { get; }
    public ReadArticleViewModel ExpectedReadArticleViewModel { get; }
    public CreateArticleViewModel CreateArticleViewModel { get; }
    public UpdateArticleViewModel UpdateArticleViewModel  { get; }
    public PagedList<Article> PagedArticles { get; }
    public PagedReadViewModel<ReadArticleViewModel> PagedReadViewModel { get; }
    public ArticleParameters Parameters { get; }
    public User RequestUser { get; }
    public string ImageLink { get; }

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

    private string GetImageLink()
    {
        return "http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/439e7759-7de1-42e8-ad6d-8bed3723b676.png";
    }
}