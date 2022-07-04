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
        MockImageParser = fixture.Freeze<Mock<IImageParser>>();

        MockArticleService = new ArticleService(
            MockArticleRepository.Object,
            MockLoggerManager.Object,
            MockImageParser.Object);
    }
    
    public ArticleService MockArticleService { get; }
    public Mock<IArticleRepository> MockArticleRepository { get; }
    public Mock<ILoggerManager> MockLoggerManager { get; }
    public Mock<IImageParser> MockImageParser { get; }

}