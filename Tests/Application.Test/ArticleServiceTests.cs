using System.Linq.Expressions;
using Application.Test.Fixtures;
using Azure;
using Core.Entities;
using Core.Exceptions;
using Core.Paginator;
using Core.Paginator.Parameters;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.Test;

public class ArticleServiceTests : IClassFixture<ArticleServiceFixture>, IDisposable
{
    private readonly ArticleServiceFixture _fixture;
    private bool _disposed;

    public ArticleServiceTests(ArticleServiceFixture fixture)
    {
        _fixture = fixture;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _fixture.MockArticleRepository.ResetCalls();
        }

        _disposed = true;
    }
    
    [Fact]
    public async Task GetAllArticlesAsync_whenArticlesListIsNotEmpty_thanReturnArticlesList()
    {
        _fixture.MockArticleRepository
            .Setup(repo => repo.GetPaged(
                It.IsAny<ArticleParameters>(),
                It.IsAny<Expression<Func<Article, bool>>>(),
                It.IsAny<Func<IQueryable<Article>, IOrderedQueryable<Article>>>(),
                It.IsAny<string>()))
            .ReturnsAsync(_fixture.PagedArticles
            );
        
        //Act
        var result = await _fixture.MockArticleService.GetArticlesAsync(new ArticleParameters());
        
        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(_fixture.ExpectedArticles, result);
    }
    
    [Fact]
    public async Task GetAllArticlesAsync_whenArticlesListIsEmpty_thanReturnEmptyArticlesList()
    {
        //Arrange
        var emptyArticles = new PagedList<Article>(new List<Article>(), 0, 0, 0);
        
        _fixture.MockArticleRepository
            .Setup(repo => repo.GetPaged(
                It.IsAny<ArticleParameters>(),
                It.IsAny<Expression<Func<Article, bool>>>(),
                It.IsAny<Func<IQueryable<Article>, IOrderedQueryable<Article>>>(),
                It.IsAny<string>()))
            .ReturnsAsync(emptyArticles);
        
        //Act
        var result = await _fixture.MockArticleService.GetArticlesAsync(new ArticleParameters());
        
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
                It.Is<int>(x => x == _fixture.ExpectedArticle.Id), 
                It.IsAny<string>()))
            .ReturnsAsync(_fixture.ExpectedArticle);
        
        //Act
        var result = await _fixture.MockArticleService.GetByIdAsync(_fixture.ExpectedArticle.Id);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(_fixture.ExpectedArticle, result);
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

    [Fact]
    public async Task GetPublishedArticlesAsync_whenArticlesListIsNotEmpty_thanReturnArticlesList()
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
                Published = true,
                Title = "Hello"
            }
        };

        var published = new PagedList<Article>(articles, 2, 1, 5);

        _fixture.MockArticleRepository
            .Setup(repo => repo.GetPaged(
                It.IsAny<ArticleParameters>(),
                It.IsAny<Expression<Func<Article, bool>>>(),
                It.IsAny<Func<IQueryable<Article>, IOrderedQueryable<Article>>>(),
                It.IsAny<string>()))
            .ReturnsAsync(published);
        
        //Act
        var result = await _fixture.MockArticleService.GetArticlesAsync(new ArticleParameters());
        
        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(published, result);
    }

    [Fact]
    public async Task CreateArticleAsync_whenNormal_thenSuccess()
    {
        //Arrange
        _fixture.MockImageParser
            .Setup(ip => ip.UploadImages(It.IsAny<string>()))
            .ReturnsAsync("article body");
        
        _fixture.MockArticleRepository
            .Setup(r => r.InsertAsync(It.IsAny<Article>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        _fixture.MockArticleRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();

        //Act
        await _fixture.MockArticleService.CreateArticleAsync(_fixture.ExpectedArticle);
        
        //Assert
        _fixture.MockArticleRepository
            .Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateArticleAsync_whenBlobError_thenThrowBadRequest()
    {
        //Arrange
        _fixture.MockImageParser
            .Setup(ip => ip.UploadImages(It.IsAny<string>()))
            .ThrowsAsync(new BadRequestException());
        
        _fixture.MockArticleRepository
            .Setup(r => r.InsertAsync(It.IsAny<Article>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        _fixture.MockArticleRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        var result = _fixture.MockArticleService.CreateArticleAsync(_fixture.ExpectedArticle);
        
        //Assert
        _fixture.MockArticleRepository
            .Verify(r => r.InsertAsync(_fixture.ExpectedArticle), Times.Never);
        await Assert.ThrowsAsync<BadRequestException>(() => result);
    }

    [Fact]
    public async Task UpdateArticleAsync_whenNormal_thenSuccess()
    {
        //Arrange
        _fixture.MockArticleRepository
            .Setup(r => r.GetById(
                It.IsAny<int>(), 
                It.IsAny<string>()))
            .ReturnsAsync(_fixture.ExpectedArticle);

        _fixture.MockImageParser
            .Setup(ip => ip.UploadImages(It.IsAny<string>()))
            .ReturnsAsync("article body");
        
        _fixture.MockArticleRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();

        //Act
        await _fixture.MockArticleService.UpdateArticleAsync(_fixture.ExpectedArticle);
        
        //Assert
        _fixture.MockArticleRepository
            .Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateArticleAsync_whenBlobError_thenThrowBadRequest()
    {
        //Arrange
        _fixture.MockArticleRepository
            .Setup(r => r.GetById(
                It.IsAny<int>(), 
                It.IsAny<string>()))
            .ReturnsAsync(_fixture.ExpectedArticle);
        
        _fixture.MockImageParser
            .Setup(ip => ip.UploadImages(It.IsAny<string>()))
            .ThrowsAsync(new RequestFailedException(""));

        _fixture.MockArticleRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        var result = _fixture.MockArticleService.UpdateArticleAsync(_fixture.ExpectedArticle);
        
        //Assert
        _fixture.MockArticleRepository
            .Verify(r => r.GetById(        
                It.IsAny<int>(), 
                It.IsAny<string>()), Times.Once);
        _fixture.MockArticleRepository
            .Verify(r => r.SaveChangesAsync(), Times.Never);
        
        await Assert.ThrowsAsync<BadRequestException>(() => result);
    }

    [Fact]
    public async Task UpdateArticleAsync_whenArticleDontExist_thenThrowNotFound()
    {
        //Arrange
        _fixture.MockArticleRepository
            .Setup(r => r.GetById(
                It.IsAny<int>(),
                It.IsAny<string>()))
            .ThrowsAsync(new NotFoundException());
        
        _fixture.MockImageParser
            .Setup(ip => ip.UploadImages(It.IsAny<string>()))
            .ReturnsAsync("article body");

        _fixture.MockArticleRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        var result = _fixture.MockArticleService.UpdateArticleAsync(_fixture.ExpectedArticle);
        
        //Assert
        _fixture.MockArticleRepository
            .Verify(r => r.GetById(        
                It.IsAny<int>(), 
                It.IsAny<string>()), Times.Once);
        _fixture.MockArticleRepository
            .Verify(r => r.SaveChangesAsync(), Times.Never);
        
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }

    [Fact]
    public async Task DeleteArticleAsync_whenNormal_thenSuccess()
    {
        //Arrange
        _fixture.MockArticleRepository
            .Setup(r => r.GetById(
                It.IsAny<int>(), 
                It.IsAny<string>()))
            .ReturnsAsync(_fixture.ExpectedArticle);

        _fixture.MockImageParser
            .Setup(ip => ip.DeleteImages(It.IsAny<string>()))
            .ReturnsAsync("article body");
        
        _fixture.MockArticleRepository
            .Setup(r => r.Delete(It.IsAny<Article>()))
            .Verifiable();
        
        _fixture.MockArticleRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();

        //Act
        await _fixture.MockArticleService.DeleteArticleAsync(17);
        
        //Assert
        _fixture.MockArticleRepository
            .Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteArticleAsync_whenBlobError_thenThrowBadRequest()
    {
        //Arrange
        _fixture.MockArticleRepository
            .Setup(r => r.GetById(
                It.IsAny<int>(), 
                It.IsAny<string>()))
            .ReturnsAsync(_fixture.ExpectedArticle);
        
        _fixture.MockImageParser
            .Setup(ip => ip.DeleteImages(It.IsAny<string>()))
            .ThrowsAsync(new RequestFailedException(""));
        
        _fixture.MockArticleRepository
            .Setup(r => r.Delete(It.IsAny<Article>()))
            .Verifiable();
        
        _fixture.MockArticleRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        var result = _fixture.MockArticleService.DeleteArticleAsync(17);
        
        //Assert
        _fixture.MockArticleRepository
            .Verify(r => r.GetById(        
                It.IsAny<int>(), 
                It.IsAny<string>()), Times.Once);
        _fixture.MockArticleRepository
            .Verify(r => r.SaveChangesAsync(), Times.Never);
        
        await Assert.ThrowsAsync<BadRequestException>(() => result);
    }

    [Fact]
    public async Task DeleteArticleAsync_whenArticleDontExist_thenThrowNotFound()
    {
        //Arrange
        _fixture.MockArticleRepository
            .Setup(r => r.GetById(
                It.IsAny<int>(),
                It.IsAny<string>()))
            .ThrowsAsync(new NotFoundException());
        
        _fixture.MockImageParser
            .Setup(ip => ip.UploadImages(It.IsAny<string>()))
            .ReturnsAsync("article body");
        
        _fixture.MockArticleRepository
            .Setup(r => r.Delete(It.IsAny<Article>()))
            .Verifiable();
        
        _fixture.MockArticleRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        var result = _fixture.MockArticleService.DeleteArticleAsync(17);
        
        //Assert
        _fixture.MockArticleRepository
            .Verify(r => r.GetById(        
                It.IsAny<int>(), 
                It.IsAny<string>()), Times.Once);
        _fixture.MockArticleRepository
            .Verify(r => r.SaveChangesAsync(), Times.Never);
        
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }
}