using System.Linq.Expressions;
using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Moq;

namespace Application.Test;

public class ArticleServiceTests : IClassFixture<ArticleServiceFixture>
{
    private readonly ArticleServiceFixture _fixture;

    public ArticleServiceTests(ArticleServiceFixture fixture)
    {
        _fixture = fixture;
    }

    private readonly Article _article = new Article
    {
        Id = 1,
        AuthorId = 1,
        Body = "<ul><li><p class=\"mat-title\">Hello</li><li>World</li></ul>",
        CreatedAt = new DateTime(2020, 10, 10, 10, 10, 30),
        Edited = false,
        Published = true,
        Title = "Hello"
    };

    private readonly IList<Article> _articles = new List<Article>()
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

    [Fact]
    public async Task GetAllArticlesAsync_whenArticlesListIsNotEmpty_thanReturnArticlesList()
    {
        _fixture.MockArticleRepository
            .Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Article, bool>>>(),
                It.IsAny<Func<IQueryable<Article>, IOrderedQueryable<Article>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(_articles);
        
        //Act
        var result = await _fixture.MockArticleService.GetAllArticlesAsync();
        
        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(_articles, result);
    }
    
    [Fact]
    public async Task GetAllArticlesAsync_whenArticlesListIsEmpty_thanReturnEmptyArticlesList()
    {
        //Arrange
        var emptyArticles = new List<Article>();
        
        _fixture.MockArticleRepository
            .Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Article, bool>>>(),
                It.IsAny<Func<IQueryable<Article>, IOrderedQueryable<Article>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(emptyArticles);
        
        //Act
        var result = await _fixture.MockArticleService.GetAllArticlesAsync();
        
        //Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        Assert.Equal(emptyArticles, result);
    }
    
    [Fact]
    public async Task GetByIdAsync_whenArticleExist_thanReturnTheArticle()
    {
        //Arrange
        _fixture.MockArticleRepository
            .Setup(repo => repo.GetById(
                It.Is<int>(x => x == _article.Id), 
                It.IsAny<string>()))
            .ReturnsAsync(_article);
        
        //Act
        var result = await _fixture.MockArticleService.GetByIdAsync(_article.Id);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(_article, result);
    }
    
    [Fact]
    public async Task GetByIdAsync_whenArticleDoesNotExist_thanThrowNotFound()
    {
        //Arrange
        _fixture.MockArticleRepository
            .Setup(repo => repo.GetById(
                It.IsAny<int>(), 
                It.IsAny<string>()))
            .ReturnsAsync(() => null);
        
        //Act
        var result = _fixture.MockArticleService.GetByIdAsync(10);
        
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }
}