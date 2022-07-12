using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Application.Test.Fixtures;

public class ImageParserFixture
{
    public ImageParserFixture()
    {
        var fixture =
            new Fixture().Customize(new AutoMoqCustomization());

        MockConfiguration = fixture.Freeze<Mock<IConfiguration>>();
        MockImageRepository = fixture.Freeze<Mock<IImageRepository>>();

        ImageService = new ImageService(MockConfiguration.Object, MockImageRepository.Object);
    }
    
    public Mock<IConfiguration> MockConfiguration { get; }
    public Mock<IImageRepository> MockImageRepository { get; }
    public ImageService ImageService { get; }
}