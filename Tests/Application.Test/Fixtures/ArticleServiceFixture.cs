using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
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
        MockConfiguration = fixture.Freeze<Mock<IConfiguration>>();
        MockImageService = fixture.Freeze<Mock<IImageService>>();

        MockArticleService = new ArticleService(
            MockArticleRepository.Object,
            MockLoggerManager.Object,
            MockConfiguration.Object,
            MockImageService.Object);
    }
    
    public ArticleService MockArticleService { get; }
    public Mock<IArticleRepository> MockArticleRepository { get; }
    public Mock<ILoggerManager> MockLoggerManager { get; }
    public Mock<IConfiguration> MockConfiguration { get; }
    public Mock<IImageService> MockImageService { get; }

}