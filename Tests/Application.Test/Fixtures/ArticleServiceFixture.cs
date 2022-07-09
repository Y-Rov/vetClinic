using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Paginator;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Application.Test.Fixtures;

public class ArticleServiceFixture
{
    public ArticleServiceFixture()
    {
        var fixture =
            new Fixture().Customize(new AutoMoqCustomization());

        MockArticleRepository = fixture.Freeze<Mock<IArticleRepository>>();
        MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();
        MockImageParser = fixture.Freeze<Mock<IImageParser>>();

        ExpectedArticle = GetArticle();
        ExpectedArticles = GetArticles();
        PagedArticles = GetPagedArticles();

        MockArticleService = new ArticleService(
            MockArticleRepository.Object,
            MockLoggerManager.Object,
            MockImageParser.Object);
    }
    
    public ArticleService MockArticleService { get; }
    public Mock<IArticleRepository> MockArticleRepository { get; }
    public Mock<ILoggerManager> MockLoggerManager { get; }
    public Mock<IImageParser> MockImageParser { get; }
    
    public Article ExpectedArticle { get; }
    public List<Article> ExpectedArticles { get; }
    public PagedList<Article> PagedArticles { get; }

    private Article GetArticle()
    {
        var article = new Article
        {
            Id = 1,
            AuthorId = 1,
            Body = "<ul><li><p class=\"mat-title\">Hello</li><li>World</li></ul>",
            CreatedAt = new DateTime(2020, 10, 10, 10, 10, 30),
            Edited = false,
            Published = true,
            Title = "Hello"
        };
        return article;
    }
    private List<Article> GetArticles()
    {
        var articles = new List<Article>()
        {
            new Article
            {
                Id = 1,
                AuthorId = 1,
                Body = "<ul><li><p class=\"mat-title\">Hello</li><li>World</li></ul>",
                CreatedAt = new DateTime(2020, 10, 10, 10, 10, 10),
                Edited = true,
                Published = true,
                Title = "Hello"
            },
            new Article
            {
                Id = 2,
                AuthorId = 1,
                Body = "<ul><li><p class=\"mat-title\">Hello</li><li>World</li></ul>",
                CreatedAt = new DateTime(2020, 10, 10, 10, 10, 30),
                Edited = false,
                Published = false,
                Title = "Hello"
            },
            new Article
            {
                Id = 3,
                AuthorId = 1,
                Body = "<ul><li><p class=\"mat-title\">Hello</li><li>World</li></ul>",
                CreatedAt = new DateTime(2020, 10, 10, 10, 10, 20),
                Edited = false,
                Published = true,
                Title = "Hello"
            }
        };
        return articles;
    }

    private PagedList<Article> GetPagedArticles()
    {
        var pagedArticles = new PagedList<Article>(ExpectedArticles, 3, 1, 5);
        return pagedArticles;
    }
}