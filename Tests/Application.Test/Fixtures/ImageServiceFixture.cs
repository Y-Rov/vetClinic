using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Application.Test.Fixtures;

public class ImageServiceFixture
{
    public ImageServiceFixture()
    {
        var fixture =
            new Fixture().Customize(new AutoMoqCustomization());

        MockConfiguration = fixture.Freeze<Mock<IConfiguration>>();
        MockImageRepository = fixture.Freeze<Mock<IImageRepository>>();
        MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();
        MockMemoryCache = fixture.Freeze<Mock<IMemoryCache>>();

        MockImageService = new ImageService(
            MockConfiguration.Object, 
            MockImageRepository.Object,
            MockMemoryCache.Object,
            MockLoggerManager.Object);
    }
    
    public Mock<IConfiguration> MockConfiguration { get; }
    public Mock<IImageRepository> MockImageRepository { get; }
    public Mock<IMemoryCache> MockMemoryCache { get; }
    public Mock<ILoggerManager> MockLoggerManager { get; }
    public ImageService MockImageService { get; }
}