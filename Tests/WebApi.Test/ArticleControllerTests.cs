using Core.Entities;
using Core.Exceptions;
using Core.ViewModels.ArticleViewModels;
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
    
    private readonly Article _article = new Article()
    {
        Id = 1,
        AuthorId = 1,
        Body = "article body",
        Title = "article title",
        CreatedAt = new DateTime(2020, 10, 10, 10, 10, 10),
        Published = true,
        Edited = false,
    };

    private readonly ReadArticleViewModel _readViewModel = new ReadArticleViewModel()
    {
        Id = 1,
        AuthorId = 1,
        Body = "article body",
        Title = "article title",
        CreatedAt = new DateTime(2020, 10, 10, 10, 10, 10),
        Published = true,
        Edited = false,
    };

    private readonly IEnumerable<ReadArticleViewModel> _readViewModels = new List<ReadArticleViewModel>()
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
    
    private readonly IEnumerable<Article> _articles = new List<Article>()
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
    
    [Fact]
    public async Task GetArticleById_whenIdIsCorrect_thenStatusCodeOkReturned()
    {
        //Arrange
        _fixture.MockArticleService
            .Setup(service =>
                service.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(_article);
        _fixture.MockReadMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<Article>()))
            .Returns(_readViewModel);
        //Act
        var result = await _fixture.MockArticleController.GetByIdAsync(1);
        //Assert
        Assert.NotNull(result);
        Assert.Equal(result, _readViewModel);
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
            .Returns(_readViewModel);
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
                service.GetArticlesAsync())
            .ReturnsAsync(_articles);

        _fixture.MockEnumerableViewModelMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<IEnumerable<Article>>()))
            .Returns(_readViewModels);

        //  Act
        var result = await _fixture.MockArticleController.GetAsync();

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, _readViewModels);
    }

    [Fact]
    public async Task GetAll_whenArticlesListIsEmpty_thenStatusOkReturned()
    {
        //  Arrange
        var emptyArticles = new List<Article>();

        var emptyArticleReadViewModels = new List<ReadArticleViewModel>();

        _fixture.MockArticleService
            .Setup(service =>
                service.GetArticlesAsync())
            .ReturnsAsync(emptyArticles);

        _fixture.MockEnumerableViewModelMapper
            .Setup(mapper =>
                mapper.Map(It.Is<IEnumerable<Article>>(p => p.Equals(emptyArticles))))
            .Returns(emptyArticleReadViewModels);

        //  Act
        var result = await _fixture.MockArticleController.GetAsync();

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
                service.GetPublishedArticlesAsync())
            .ReturnsAsync(_articles);

        _fixture.MockEnumerableViewModelMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<IEnumerable<Article>>()))
            .Returns(_readViewModels);

        //  Act
        var result = await _fixture.MockArticleController.GetPublishedAsync();

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, _readViewModels);
    }

    [Fact]
    public async Task GetPublished_whenArticlesListIsEmpty_thenStatusOkReturned()
    {
        //  Arrange
        var emptyPublishedArticles = new List<Article>();

        var emptyPublishedArticleReadViewModels = new List<ReadArticleViewModel>();

        _fixture.MockArticleService
            .Setup(service =>
                service.GetPublishedArticlesAsync())
            .ReturnsAsync(emptyPublishedArticles);

        _fixture.MockEnumerableViewModelMapper
            .Setup(mapper =>
                mapper.Map(It.Is<IEnumerable<Article>>(p => p.Equals(emptyPublishedArticles))))
            .Returns(emptyPublishedArticleReadViewModels);

        //  Act
        var result = await _fixture.MockArticleController.GetPublishedAsync();

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, emptyPublishedArticleReadViewModels);
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
        var createArticleViewModel = new CreateArticleViewModel()
        {
            AuthorId = 1,
            Body = "article body",
            Title = "article title",
            Published = true
        };
        
        _fixture.MockCreateMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<CreateArticleViewModel>()))
            .Returns(_article);

        _fixture.MockArticleService
            .Setup(service =>
                service.CreateArticleAsync(
                    It.IsAny<Article>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //  Act
        await _fixture.MockArticleController.CreateAsync(createArticleViewModel);

        //  Assert
        _fixture.MockCreateMapper.Verify(m => m.Map(createArticleViewModel), Times.Once);
        _fixture.MockArticleService.Verify(s => s.CreateArticleAsync(_article), Times.Once);
    }
    
    [Fact]
    public async Task Update_whenOk_thenStatusOkReturned()
    {
        //  Arrange
        var updateArticleViewModel = new UpdateArticleViewModel()
        {
            Id = 1,
            Body = "article body",
            Title = "article title",
            Published = true
        };
        
        _fixture.MockUpdateMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<UpdateArticleViewModel>()))
            .Returns(_article);

        _fixture.MockArticleService
            .Setup(service =>
                service.UpdateArticleAsync(
                    It.IsAny<Article>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //  Act
        await _fixture.MockArticleController.UpdateAsync(updateArticleViewModel);

        //  Assert
        _fixture.MockUpdateMapper.Verify(m => m.Map(updateArticleViewModel), Times.Once);
        _fixture.MockArticleService.Verify(s => s.UpdateArticleAsync(_article), Times.Once);
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
}