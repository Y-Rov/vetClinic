using System.Security.Claims;
using Core.Entities;
using Core.Exceptions;
using Core.Paginator;
using Core.Paginator.Parameters;
using Core.ViewModels;
using Core.ViewModels.ArticleViewModels;
using Core.ViewModels.CommentViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Test.Fixtures;

namespace WebApi.Test;

public class ArticleControllerTests : IClassFixture<ArticleControllerFixture>
{
    private readonly ArticleControllerFixture _fixture;

    public ArticleControllerTests(ArticleControllerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetArticleById_whenIdIsCorrect_thenStatusCodeOkReturned()
    {
        //Arrange
        _fixture.MockArticleService
            .Setup(service =>
                service.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(_fixture.Article);
        _fixture.MockReadMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<Article>()))
            .Returns(_fixture.ExpectedReadArticleViewModel);
        //Act
        var result = await _fixture.MockArticleController.GetByIdAsync(1);
        //Assert
        Assert.NotNull(result);
        Assert.Equal(result, _fixture.ExpectedReadArticleViewModel);
    }

    [Fact]
    public async Task GetArticleById_whenIdIsIncorrect_thenStatusCodeNotFoundReturned()
    {
        //Arrange
        _fixture.MockArticleService
            .Setup(service =>
                service.GetByIdAsync(It.IsAny<int>()))
            .Throws<NotFoundException>();
        _fixture.MockReadMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<Article>()))
            .Returns(_fixture.ExpectedReadArticleViewModel);
        //Act
        var result = _fixture.MockArticleController.GetByIdAsync(1);
        //  Assert
        Assert.NotNull(result);
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }
    
    [Fact]
    public async Task GetAll_whenArticlesListIsNotEmpty_thenStatusOkReturned()
    {
        //  Arrange
        _fixture.MockArticleService
            .Setup(service =>
                service.GetArticlesAsync(It.IsAny<ArticleParameters>()))
            .ReturnsAsync(_fixture.PagedArticles);

        _fixture.MockReadPagedMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<PagedList<Article>>()))
            .Returns(_fixture.PagedReadViewModel);

        //  Act
        var result = await _fixture.MockArticleController.GetAsync(_fixture.Parameters);

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, _fixture.PagedReadViewModel);
    }

    [Fact]
    public async Task GetAll_whenArticlesListIsEmpty_thenStatusOkReturned()
    {
        //  Arrange
        var emptyArticles = new PagedList<Article>(new List<Article>(), 0, 0, 0);

        var emptyArticleReadViewModels = new PagedReadViewModel<ReadArticleViewModel>();

        _fixture.MockArticleService
            .Setup(service =>
                service.GetArticlesAsync(It.IsAny<ArticleParameters>()))
            .ReturnsAsync(emptyArticles);

        _fixture.MockReadPagedMapper
            .Setup(mapper =>
                mapper.Map(It.Is<PagedList<Article>>(p => p.Equals(emptyArticles))))
            .Returns(emptyArticleReadViewModels);

        //  Act
        var result = await _fixture.MockArticleController.GetAsync(_fixture.Parameters);

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, emptyArticleReadViewModels);
    }
    
    [Fact]
    public async Task GetPublished_whenArticlesListIsNotEmpty_thenStatusOkReturned()
    {
        //  Arrange
        _fixture.MockArticleService
            .Setup(service =>
                service.GetPublishedArticlesAsync(It.IsAny<ArticleParameters>()))
            .ReturnsAsync(_fixture.PagedArticles);

        _fixture.MockReadPagedMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<PagedList<Article>>()))
            .Returns(_fixture.PagedReadViewModel);

        //  Act
        var result = await _fixture.MockArticleController.GetPublishedAsync(_fixture.Parameters);

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, _fixture.PagedReadViewModel);
    }

    [Fact]
    public async Task GetPublished_whenArticlesListIsEmpty_thenStatusOkReturned()
    {
        //  Arrange
        var emptyArticles = new PagedList<Article>(new List<Article>(), 0, 0, 0);

        var emptyArticleReadViewModels = new PagedReadViewModel<ReadArticleViewModel>();

        _fixture.MockArticleService
            .Setup(service =>
                service.GetPublishedArticlesAsync(It.IsAny<ArticleParameters>()))
            .ReturnsAsync(emptyArticles);

        _fixture.MockReadPagedMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<PagedList<Article>>()))
            .Returns(emptyArticleReadViewModels);

        //  Act
        var result = await _fixture.MockArticleController.GetPublishedAsync(_fixture.Parameters);

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, emptyArticleReadViewModels);
    }
    
    [Fact]
    public async Task Delete_whenArticleExist_thenStatusOkReturned()
    {
        //  Arrange
        _fixture.MockArticleService
            .Setup(service =>
                service.DeleteArticleAsync(
                    It.IsAny<int>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //  Act
        await _fixture.MockArticleController.DeleteAsync(1);

        //  Assert
        _fixture.MockArticleService.Verify();
    }
    
    [Fact]
    public async Task Delete_whenArticleDoesNotExist_thenStatusNotFoundReturned()
    {
        //  Arrange
        _fixture.MockArticleService
            .Setup(service =>
                service.DeleteArticleAsync(
                    It.IsAny<int>()))
            .Throws<NotFoundException>();
        
        //  Act
        var result =  _fixture.MockArticleController.DeleteAsync(65);

        //  Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }
    
    [Fact]
    public async Task Create_whenOk_thenStatusOkReturned()
    {
        //  Arrange
        _fixture.MockCreateMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<CreateArticleViewModel>()))
            .Returns(_fixture.Article);

        _fixture.MockArticleService
            .Setup(service =>
                service.CreateArticleAsync(
                    It.IsAny<Article>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //  Act
        await _fixture.MockArticleController.CreateAsync(_fixture.CreateArticleViewModel);

        //  Assert
        _fixture.MockCreateMapper.Verify(m => m.Map(_fixture.CreateArticleViewModel), Times.Once);
        _fixture.MockArticleService.Verify(s => s.CreateArticleAsync(_fixture.Article), Times.Once);
    }
    
    [Fact]
    public async Task Update_whenOk_thenStatusOkReturned()
    {
        //  Arrange
        _fixture.MockUpdateMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<UpdateArticleViewModel>()))
            .Returns(_fixture.Article);

        _fixture.MockArticleService
            .Setup(service =>
                service.UpdateArticleAsync(
                    It.IsAny<Article>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //  Act
        await _fixture.MockArticleController.UpdateAsync(_fixture.UpdateArticleViewModel);

        //  Assert
        _fixture.MockUpdateMapper.Verify(m => m.Map(_fixture.UpdateArticleViewModel), Times.Once);
        _fixture.MockArticleService.Verify(s => s.UpdateArticleAsync(_fixture.Article), Times.Once);
    }

    [Fact]
    public async Task Update_whenArticleDoesNotExist_thenStatusNotFoundReturned()
    {
        //  Arrange
        _fixture.MockArticleService
            .Setup(service =>
                service.UpdateArticleAsync(
                    It.IsAny<Article>()))
            .Throws<NotFoundException>();
        
        var updateArticleViewModel = new UpdateArticleViewModel()
        {
            Id = 1,
            Body = "article body",
            Title = "article title",
            Published = true
        };        
        //  Act
        var result =  _fixture.MockArticleController.UpdateAsync(updateArticleViewModel);

        //  Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }

    [Fact]
    public async Task UploadImage_whenOk_thenImageLinkViewModelReturned()
    {
        //Arrange
        _fixture.MockUserManager
            .Setup(um => um
                .GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(_fixture.RequestUser).Verifiable();
        
        _fixture.MockArticleController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal()
            }
        };

        _fixture.MockImageService
            .Setup(img => img.UploadImageAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<int>()))
            .ReturnsAsync(_fixture.ImageLink);
        //Act
        var result = await _fixture.MockArticleController.UploadImage(It.IsAny<IFormFile>());
        //Assert
        Assert.Equal(_fixture.ImageLink, result.ImageUrl);
    }
    
    [Fact]
    public async Task DeleteImage_whenOk_thenSuccess()
    {
        //Arrange
        _fixture.MockUserManager
            .Setup(um => um
                .GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(_fixture.RequestUser).Verifiable();
        
        _fixture.MockArticleController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal()
            }
        };

        _fixture.MockImageService
            .Setup(img => img.DiscardCachedImagesAsync(It.IsAny<int>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        //Act
        await _fixture.MockArticleController.DiscardEditing();
        //Assert
        _fixture.MockImageService
            .Verify(img => img.DiscardCachedImagesAsync(It.IsAny<int>()), Times.Once);
    }
}