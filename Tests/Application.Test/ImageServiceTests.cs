using Application.Test.Fixtures;
using Azure;
using Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace Application.Test;

public class ImageServiceTests : IClassFixture<ImageServiceFixture>, IDisposable
{
    private readonly ImageServiceFixture _fixture;
    private bool _disposed;

    public ImageServiceTests(ImageServiceFixture fixture)
    {
        _fixture = fixture;
    }
    
    public void Dispose()
    {
        _fixture.MockImageRepository.ResetCalls();
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
            _fixture.MockImageRepository.ResetCalls();
            _fixture.MockMemoryCache.ResetCalls();
        }

        _disposed = true;
    }

    [Fact]
    public void TrimArticleImages_whenBodyHasNoImages_thenUnchangedBodyReturned()
    {
        //Arrange
        var body = "<ul><li><font face=\"Times New Roman\">hello</font></li><li><font face=\"Times New Roman\">for</font></li><li><u><font face=\"Times New Roman\">test example</font></u></li></ul>";
        //Act
        var result = _fixture.MockImageService.TrimArticleImages(body);
        //Assert
        Assert.Equal(body, result);
    }

    [Fact]
    public void TrimArticleImages_whenBodyHasThreeInnerImages_thenUnchangedBodyReturned()
    {
        //Arrange
        var body =
            "<ul><li><font face=\"Times New Roman\">hello</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><font face=\"Times New Roman\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for</font></li><li><u><font face=\"Times New Roman\">test&#160;<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\">&#160;example</font></u></li></ul>";

        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        //Act
        var result = _fixture.MockImageService.TrimArticleImages(body);
        //Assert
        Assert.Equal(body, result);
    }

    [Fact]
    public void TrimArticleImages_whenBodyHasThreeInnerAndOuterImagesWithAttributesAtTheEndAndBeginning_thenUnchangedBodyReturned()
    {
        //Arrange
        var body =
            "<ul><li><font face=\"Times New Roman\">helo</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><font face=\"Times New Roman\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for</font></li><li><u><font face=\"Times New Roman\">test&#160;<img class=\"image-wrapper\" src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg\" alt=\"What Is Image Processing: Overview, Applications, Benefits, and Who Should Learn It [2022 Edition]\">&#160;example</font></u></li><ul><ul><li><font face=\"Times New Roman\"><u><i>Let&#160;</i></u></font><img class=\"hello-image-class\" src=\"https://img.freepik.com/premium-photo/astronaut-outer-open-space-planet-earth-stars-provide-background-erforming-space-planet-earth-sunrise-sunset-our-home-iss-elements-this-image-furnished-by-nasa_150455-16829.jpg\" alt=\"Premium Photo | Astronaut in outer open space over the planet earth.stars provide the background.erforming a space above planet earth.sunrise,sunset.our home. iss.elements of this image furnished by nasa.\"></li><li><font face=\"Times New Roman\"><u><i>They<br></i></u></font></li></ul><li><u><i>Be&#160;<img style=\"width: 40px\" src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg\" alt=\"Clipping Magic: Remove Background From Image\">&#160;Or Not To Be</i></u><br></li></ul><li><font face=\"Times New Roman\"><i><u>With</u></i></font></li><ul><ul><ul><ul><ul><ul><li><font face=\"Times New Roman\"><i><u>Images&#160;</u></i></font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\"></li></ul></ul></ul></ul></ul></ul></ul>";
        
        var trimmedBody = 
            "<ul><li><font face=\"Times New Roman\">helo</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><font face=\"Times New Roman\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for</font></li><li><u><font face=\"Times New Roman\">test&#160;<img src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg\">&#160;example</font></u></li><ul><ul><li><font face=\"Times New Roman\"><u><i>Let&#160;</i></u></font><img src=\"https://img.freepik.com/premium-photo/astronaut-outer-open-space-planet-earth-stars-provide-background-erforming-space-planet-earth-sunrise-sunset-our-home-iss-elements-this-image-furnished-by-nasa_150455-16829.jpg\"></li><li><font face=\"Times New Roman\"><u><i>They<br></i></u></font></li></ul><li><u><i>Be&#160;<img src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg\">&#160;Or Not To Be</i></u><br></li></ul><li><font face=\"Times New Roman\"><i><u>With</u></i></font></li><ul><ul><ul><ul><ul><ul><li><font face=\"Times New Roman\"><i><u>Images&#160;</u></i></font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\"></li></ul></ul></ul></ul></ul></ul></ul>";

        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        //Act
        var result = _fixture.MockImageService.TrimArticleImages(body);
        //Assert
        Assert.Equal(trimmedBody, result);
    }
    
    [Fact]
    public void TrimArticleImages_whenBodyHasThreeInnerAndOuterImagesWithQueryAndAttributesAtTheEndAndBeginning_thenTrimmedBodyReturned()
    {
        //Arrange
        var body =
            "<ul><li><font face=\"Times New Roman\">helo</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><font face=\"Times New Roman\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for</font></li><li><u><font face=\"Times New Roman\">test&#160;<img class=\"image-wrapper\" src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg?name=ferret&color=purple\" alt=\"What Is Image Processing: Overview, Applications, Benefits, and Who Should Learn It [2022 Edition]\">&#160;example</font></u></li><ul><ul><li><font face=\"Times New Roman\"><u><i>Let&#160;</i></u></font><img class=\"hello-image-class\" src=\"https://img.freepik.com/premium-photo/astronaut-outer-open-space-planet-earth-stars-provide-background-erforming-space-planet-earth-sunrise-sunset-our-home-iss-elements-this-image-furnished-by-nasa_150455-16829.jpg?param=hello&height=123\" alt=\"Premium Photo | Astronaut in outer open space over the planet earth.stars provide the background.erforming a space above planet earth.sunrise,sunset.our home. iss.elements of this image furnished by nasa.\"></li><li><font face=\"Times New Roman\"><u><i>They<br></i></u></font></li></ul><li><u><i>Be&#160;<img style=\"width: 40px\" src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg?width=17&color-name=purple\" alt=\"Clipping Magic: Remove Background From Image\">&#160;Or Not To Be</i></u><br></li></ul><li><font face=\"Times New Roman\"><i><u>With</u></i></font></li><ul><ul><ul><ul><ul><ul><li><font face=\"Times New Roman\"><i><u>Images&#160;</u></i></font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\"></li></ul></ul></ul></ul></ul></ul></ul>";
        
        var trimmedBody = 
            "<ul><li><font face=\"Times New Roman\">helo</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><font face=\"Times New Roman\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for</font></li><li><u><font face=\"Times New Roman\">test&#160;<img src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg\">&#160;example</font></u></li><ul><ul><li><font face=\"Times New Roman\"><u><i>Let&#160;</i></u></font><img src=\"https://img.freepik.com/premium-photo/astronaut-outer-open-space-planet-earth-stars-provide-background-erforming-space-planet-earth-sunrise-sunset-our-home-iss-elements-this-image-furnished-by-nasa_150455-16829.jpg\"></li><li><font face=\"Times New Roman\"><u><i>They<br></i></u></font></li></ul><li><u><i>Be&#160;<img src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg\">&#160;Or Not To Be</i></u><br></li></ul><li><font face=\"Times New Roman\"><i><u>With</u></i></font></li><ul><ul><ul><ul><ul><ul><li><font face=\"Times New Roman\"><i><u>Images&#160;</u></i></font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\"></li></ul></ul></ul></ul></ul></ul></ul>";

        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        //Act
        var result = _fixture.MockImageService.TrimArticleImages(body);
        //Assert
        Assert.Equal(trimmedBody, result);
    }
    
    [Fact]
    public void TrimArticleImages_whenBodyHasThreeInnerAndOuterImagesWithQueryAndAttributesAtTheEndAndBeginningInARow_thenTrimmedBodyReturned()
    {
        //Arrange
        var body =
            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\"><img class=\"image-wrapper\" src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg?name=ferret&color=purple\" alt=\"What Is Image Processing: Overview, Applications, Benefits, and Who Should Learn It [2022 Edition]\"><img class=\"hello-image-class\" src=\"https://img.freepik.com/premium-photo/astronaut-outer-open-space-planet-earth-stars-provide-background-erforming-space-planet-earth-sunrise-sunset-our-home-iss-elements-this-image-furnished-by-nasa_150455-16829.jpg?param=hello&height=123\" alt=\"Premium Photo | Astronaut in outer open space over the planet earth.stars provide the background.erforming a space above planet earth.sunrise,sunset.our home. iss.elements of this image furnished by nasa.\"><img style=\"width: 40px\" src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg?width=17&color-name=purple\" alt=\"Clipping Magic: Remove Background From Image\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\">";        
        var trimmedBody = 
            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\"><img src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg\"><img src=\"https://img.freepik.com/premium-photo/astronaut-outer-open-space-planet-earth-stars-provide-background-erforming-space-planet-earth-sunrise-sunset-our-home-iss-elements-this-image-furnished-by-nasa_150455-16829.jpg\"><img src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\">";        
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        //Act
        var result = _fixture.MockImageService.TrimArticleImages(body);
        //Assert
        Assert.Equal(trimmedBody, result);
    }
    
    [Fact]
    public async Task ClearOutdatedImagesAsync_whenBodyDidntChange_thenReturn()
    {
        //Arrange
        var oldBody =
            "<ul><li><font face=\"Times New Roman\">hello</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><font face=\"Times New Roman\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for</font></li><li><u><font face=\"Times New Roman\">test&#160;<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\">&#160;example</font></u></li></ul>";
        var newBody = 
            "<ul><li><font face=\"Times New Roman\">hello</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><font face=\"Times New Roman\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for</font></li><li><u><font face=\"Times New Roman\">test&#160;<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\">&#160;example</font></u></li></ul>";
          
        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        //Act
        await _fixture.MockImageService.ClearOutdatedImagesAsync(newBody, oldBody);
        //Assert
        _fixture.MockImageRepository.Verify(repo => repo.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
    
    [Fact]
    public async Task ClearOutdatedImagesAsync_whenImagesDidntChange_thenReturn()
    {
        //Arrange
        var oldBody =
            "<ul><li><font face=\"Times New Roman\">hello</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><font face=\"Times New Roman\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for</font></li><li><u><font face=\"Times New Roman\">test&#160;<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\">&#160;example</font></u></li></ul>";
        var newBody = 
            "<ul><li><font face=\"Times New Roman\">hello</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for<li><font face=\"Times New Roman\">test&#160;<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\">&#160;example</font></ul>";          
        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        //Act
        await _fixture.MockImageService.ClearOutdatedImagesAsync(newBody, oldBody);
        //Assert
        _fixture.MockImageRepository.Verify(repo => repo.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ClearOutdatedImagesAsync_whenNewImagesAdded_thenReturn()
    {
        //Arrange
        var oldBody =
            "<ul><li><font face=\"Times New Roman\">hello</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><font face=\"Times New Roman\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for</font></li><li><u><font face=\"Times New Roman\">test&#160;<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\">&#160;example</font></u></li></ul>";
        var newBody = 
            "<ul><li><font face=\"Times New Roman\">hello</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><font face=\"Times New Roman\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for</font></li><li><u><font face=\"Times New Roman\">test&#160;<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\">&#160;example</font></u></li></ul><ol><li><font face=\"Arial\"><i><strike>hello <img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/14f462e3-3700-44dd-b3d6-a7b685508e37.png\"></strike></i></font></li><ol><ol><li><font face=\"Arial\"><i><u>Pre&#160;</u></i></font><i><strike><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c0b59c6b-432e-4f22-be04-0006f8747136.png\"></strike></i></li></ol></ol></ol><i><p><i>Tsts&#160;</i></p></i>";
          
        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        //Act
        await _fixture.MockImageService.ClearOutdatedImagesAsync(newBody, oldBody);
        //Assert
        _fixture.MockImageRepository.Verify(repo => repo.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ClearOutdatedImagesAsync_whenAllImagesChanged_thenSuccess()
    {
        //Arrange
        var oldBody = 
            "<ul><li><font face=\"Times New Roman\">hello</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><font face=\"Times New Roman\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for</font></li><li><u><font face=\"Times New Roman\">test&#160;<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\">&#160;example</font></u></li></ul>";
        var newBody = 
            "<ul><li><font face=\"Times New Roman\">hello</font><li><li><font face=\"Times New Roman>for</font></li><li><u><font face=\"Times New Roman\">test&#160;&#160;example</font></u></li></ul><ol><li><font face=\"Arial\"><i><strike>hello <img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/14f462e3-3700-44dd-b3d6-a7b685508e37.png\"></strike></i></font></li><ol><ol><li><font face=\"Arial\"><i><u>Pre&#160;</u></i></font><i><strike><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c0b59c6b-432e-4f22-be04-0006f8747136.png\"></strike></i></li></ol></ol></ol><i><p><i>Tsts&#160;</i></p></i>";
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        _fixture.MockImageRepository
            .Setup(repo => repo.DeleteAsync(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        await _fixture.MockImageService.ClearOutdatedImagesAsync(newBody, oldBody);
        
        //Assert
        _fixture.MockImageRepository
            .Verify(repo => repo.DeleteAsync(
                It.IsAny<string>(), 
                It.IsAny<string>()), Times.Exactly(3));
    }    
    
    [Fact]
    public async Task ClearOutdatedImagesAsync_whenRepositoryThrows_thenThrowBadRequestException()
    {
        //Arrange
        var oldBody = 
            "<ul><li><font face=\"Times New Roman\">hello</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><font face=\"Times New Roman\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for</font></li><li><u><font face=\"Times New Roman\">test&#160;<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\">&#160;example</font></u></li></ul>";
        var newBody = 
            "<ul><li><font face=\"Times New Roman\">hello</font><li><li><font face=\"Times New Roman>for</font></li><li><u><font face=\"Times New Roman\">test&#160;&#160;example</font></u></li></ul><ol><li><font face=\"Arial\"><i><strike>hello <img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/14f462e3-3700-44dd-b3d6-a7b685508e37.png\"></strike></i></font></li><ol><ol><li><font face=\"Arial\"><i><u>Pre&#160;</u></i></font><i><strike><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c0b59c6b-432e-4f22-be04-0006f8747136.png\"></strike></i></li></ol></ol></ol><i><p><i>Tsts&#160;</i></p></i>";
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");

        _fixture.MockImageRepository
            .Setup(repo => repo.DeleteAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws(new RequestFailedException(""));
        //Act
        var resultTask = _fixture.MockImageService.ClearOutdatedImagesAsync(newBody, oldBody);
        //Assert
        await Assert.ThrowsAsync<BadRequestException>(() => resultTask);
    }

    [Fact]
    private async Task DeleteImagesAsync_whenBodyHasNoImages_thenReturn()
    {
        //Arrange
        var body = "<ol><li><font face=\"Arial\"><i><u><strike>akh</strike></u></i></font></li><li><font face=\"Arial\"><i><u><strike>aervasdv</strike></u></i></font></li><ul><li><font face=\"Arial\"><u><strike>awefdafvqers</strike></u></font></li><li><font face=\"Arial\"><u><strike>qwgasdvearwsd</strike></u></font></li></ul></ol>";
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        _fixture.MockImageRepository
            .Setup(repo => repo.DeleteAsync(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        //Act
        await _fixture.MockImageService.DeleteImagesAsync(body);
        //Assert
        _fixture.MockImageRepository.Verify(repo => repo.DeleteAsync(
            It.IsAny<string>(), 
            It.IsAny<string>()), Times.Never);
    }

    [Fact]
    private async Task DeleteImagesAsync_whenBodyHasThreeInnerAndOuter_thenDeleteInner()
    {
        //Arrange
        var body =            
            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\"><img src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg\"><img src=\"https://img.freepik.com/premium-photo/astronaut-outer-open-space-planet-earth-stars-provide-background-erforming-space-planet-earth-sunrise-sunset-our-home-iss-elements-this-image-furnished-by-nasa_150455-16829.jpg\"><img src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\">";

        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        _fixture.MockImageRepository
            .Setup(repo => repo.DeleteAsync(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        //Act
        await _fixture.MockImageService.DeleteImagesAsync(body);
        //Assert
        _fixture.MockImageRepository
            .Verify(repo => repo.DeleteAsync(
                It.IsAny<string>(), 
                It.IsAny<string>()), Times.Exactly(3));
    }

    [Fact]
    private async Task DeleteImagesAsync_whenRepositoryThrows_thenThrowBadRequestException()
    {
        //Arrange
        var body =            
            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\"><img src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg\"><img src=\"https://img.freepik.com/premium-photo/astronaut-outer-open-space-planet-earth-stars-provide-background-erforming-space-planet-earth-sunrise-sunset-our-home-iss-elements-this-image-furnished-by-nasa_150455-16829.jpg\"><img src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\">";

        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");

        _fixture.MockImageRepository
            .Setup(repo => repo.DeleteAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws(new RequestFailedException(""));
        //Act
        var resultTask = _fixture.MockImageService.DeleteImagesAsync(body);
        //Assert
        await Assert.ThrowsAsync<BadRequestException>(() => resultTask);
    }

    [Fact]
    public async Task UploadImageAsync_whenCacheIsNotEmpty_thenAddImageToCacheAndReturnLink()
    {
        //Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile
            .Setup(f => f.ContentType)
            .Returns("image/png");

        var expectedNewFileName = "439e7759-7de1-42e8-ad6d-8bed3723b676.png";
        var expectedLink = $"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/{expectedNewFileName}";
        
        _fixture.MockImageRepository
            .Setup(repo => repo.UploadFromIFormFile(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(expectedLink);

        var currentFileNames = new List<string>()
        {
            "492a5cbb-4998-4bc4-94f6-6f5c97194f7c.png",
            "0dce4f04-58a9-4c61-94a8-c4ced4ead76d.jpg",
            "45025163-0b68-4ea0-9fd6-e74a5e49a894.png", 
            "e43f8e7a-b99a-4b53-811d-59bcdbe502aa.webp"
        } as object;

         _fixture.MockMemoryCache
             .Setup(cache => cache.TryGetValue(
                 It.IsAny<object>(), 
                 out currentFileNames));

         var mockCacheEntry = new Mock<ICacheEntry>();
         mockCacheEntry
             .SetupSet(entry => entry.AbsoluteExpirationRelativeToNow = It.IsAny<TimeSpan>());

         List<string>? actualNewFileNames = null;
         mockCacheEntry
             .SetupSet(entry => entry.Value = It.IsAny<object>())
             .Callback<object>((o) => actualNewFileNames = o as List<string>);

         _fixture.MockMemoryCache
             .Setup(cache => cache.CreateEntry(It.IsAny<object>()))
             .Returns(mockCacheEntry.Object);
         
         //Act
         var actualLink = await _fixture.MockImageService.UploadImageAsync(mockFile.Object, 12);
         
         //Assert
         Assert.NotNull(actualNewFileNames);
         Assert.Contains(actualNewFileNames!, fileName => fileName == expectedNewFileName);
         Assert.Equal(expectedLink, actualLink);
    }
    
    [Fact]
    public async Task UploadImageAsync_whenCacheIsEmpty_thenAddImageToCacheAndReturnLink()
    {
        //Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile
            .Setup(f => f.ContentType)
            .Returns("image/png");

        var expectedNewFileName = "439e7759-7de1-42e8-ad6d-8bed3723b676.png";
        var expectedLink = $"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/{expectedNewFileName}";
        
        _fixture.MockImageRepository
            .Setup(repo => repo.UploadFromIFormFile(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(expectedLink);

        var currentFileNames = null as object;

         _fixture.MockMemoryCache
             .Setup(cache => cache.TryGetValue(
                 It.IsAny<object>(), 
                 out currentFileNames));

         var mockCacheEntry = new Mock<ICacheEntry>();
         mockCacheEntry
             .SetupSet(entry => entry.AbsoluteExpirationRelativeToNow = It.IsAny<TimeSpan>());

         List<string>? actualNewFileNames = null;
         mockCacheEntry
             .SetupSet(entry => entry.Value = It.IsAny<object>())
             .Callback<object>((o) => actualNewFileNames = o as List<string>);

         _fixture.MockMemoryCache
             .Setup(cache => cache.CreateEntry(It.IsAny<object>()))
             .Returns(mockCacheEntry.Object);
         
         //Act
         var actualLink = await _fixture.MockImageService.UploadImageAsync(mockFile.Object, 12);
         
         //Assert
         Assert.NotNull(actualNewFileNames);
         Assert.Contains(actualNewFileNames!, fileName => fileName == expectedNewFileName);
         Assert.Equal(expectedLink, actualLink);
    }
    
    [Fact]
    public async Task UploadImageAsync_whenRepoThrows_thenThrowBadRequest()
    {
        //Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile
            .Setup(f => f.ContentType)
            .Returns("image/png");
        
        _fixture.MockImageRepository
            .Setup(repo => repo.UploadFromIFormFile(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws(new RequestFailedException(""));
        
        //Act
        var resultTask = _fixture.MockImageService.UploadImageAsync(mockFile.Object, 12);
        //Assert
        await Assert.ThrowsAsync<BadRequestException>(() => resultTask);
    }
    
    [Fact]
    public async Task ClearUnusedImagesAsync_whenCacheIsEmpty_thenReturn()
    {
        //Arrange
        var currentFileNames = null as object;
        
        var body = 
            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/492a5cbb-4998-4bc4-94f6-6f5c97194f7c.png\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/0dce4f04-58a9-4c61-94a8-c4ced4ead76d.jpg\"><img src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e43f8e7a-b99a-4b53-811d-59bcdbe502aa.webp\">;<img src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/45025163-0b68-4ea0-9fd6-e74a5e49a894.png\">";

        _fixture.MockMemoryCache
            .Setup(cache => cache.TryGetValue(
                It.IsAny<object>(), 
                out currentFileNames));

        //Act
        await _fixture.MockImageService.ClearUnusedImagesAsync(body, 17);
        
        //Assert
        _fixture.MockImageRepository
            .Verify( 
                repo => repo.DeleteAsync(
                    It.IsAny<string>(), 
                    It.IsAny<string>()), 
                Times.Never);
        
        _fixture.MockMemoryCache
            .Verify(
                cache => cache.Remove(It.IsAny<Object>()), 
                Times.Never);
    }

    [Fact]
    public async Task ClearUnusedImagesAsync_whenCacheIsNotEmptyAndAllCachedImagesPresentInBody_thenClearCache()
    {
        //Arrange
        var currentFileNames = new List<string>()
        {
            "492a5cbb-4998-4bc4-94f6-6f5c97194f7c.png",
            "0dce4f04-58a9-4c61-94a8-c4ced4ead76d.jpg",
            "45025163-0b68-4ea0-9fd6-e74a5e49a894.png", 
            "e43f8e7a-b99a-4b53-811d-59bcdbe502aa.webp"
        } as object;
        
        var body = 
            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/492a5cbb-4998-4bc4-94f6-6f5c97194f7c.png\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/0dce4f04-58a9-4c61-94a8-c4ced4ead76d.jpg\"><img src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e43f8e7a-b99a-4b53-811d-59bcdbe502aa.webp\">;<img src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/45025163-0b68-4ea0-9fd6-e74a5e49a894.png\">";

        _fixture.MockMemoryCache
            .Setup(cache => cache.TryGetValue(
                It.IsAny<object>(), 
                out currentFileNames));
        
        _fixture.MockMemoryCache
            .Setup(cache => cache.Remove(It.IsAny<Object>()))
            .Verifiable();
        
        _fixture.MockImageRepository
            .Setup(repo => repo.DeleteAsync(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .Returns(Task.FromResult<object?>(null))
            .Verifiable();
        
        //Act
        await _fixture.MockImageService.ClearUnusedImagesAsync(body, 17);
        
        //Assert
        _fixture.MockImageRepository
            .Verify( 
                repo => repo.DeleteAsync(
                    It.IsAny<string>(), 
                    It.IsAny<string>()), 
                Times.Never);
        
        _fixture.MockMemoryCache
            .Verify(
                cache => cache.Remove(It.IsAny<Object>()), 
                Times.Once);
    }

    [Fact]
    public async Task ClearUnusedImagesAsync_whenCacheIsNotEmptyAndNotAllCachedImagesPresentInBody_thenClearCacheAndDeleteUnused()
    {
        var currentFileNames = new List<string>()
        {
            //Used
            "492a5cbb-4998-4bc4-94f6-6f5c97194f7c.png",
            "0dce4f04-58a9-4c61-94a8-c4ced4ead76d.jpg",
            "45025163-0b68-4ea0-9fd6-e74a5e49a894.png", 
            "e43f8e7a-b99a-4b53-811d-59bcdbe502aa.webp",
            //Unused
            "378b330c-5fe7-4ee2-aa4e-5668473bce5c.png",
            "aa84fb1c-f0e8-4925-a98e-94c67849605e.jpg",
            "6f850056-9032-4ecf-9e2c-b7f531ecdd62.png", 
            "79938366-258d-4f0f-8dd2-61efc84e19b2.webp"
        } as object;

        var expectedDeletedImages = new List<string>()
        {
            "378b330c-5fe7-4ee2-aa4e-5668473bce5c.png",
            "aa84fb1c-f0e8-4925-a98e-94c67849605e.jpg",
            "6f850056-9032-4ecf-9e2c-b7f531ecdd62.png",
            "79938366-258d-4f0f-8dd2-61efc84e19b2.webp"
        };
        
        var body = 
            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/492a5cbb-4998-4bc4-94f6-6f5c97194f7c.png\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/0dce4f04-58a9-4c61-94a8-c4ced4ead76d.jpg\"><img src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e43f8e7a-b99a-4b53-811d-59bcdbe502aa.webp\">;<img src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/45025163-0b68-4ea0-9fd6-e74a5e49a894.png\">";

        _fixture.MockMemoryCache
            .Setup(cache => cache.TryGetValue(
                It.IsAny<object>(), 
                out currentFileNames));
        
        _fixture.MockMemoryCache
            .Setup(cache => cache.Remove(It.IsAny<Object>()))
            .Verifiable();

        var actualDeletedImages = new List<string>();
        _fixture.MockImageRepository
            .Setup(repo => repo.DeleteAsync(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .Callback<string, string>((arg1, arg2) => actualDeletedImages.Add(arg1))
            .Returns(Task.FromResult<object?>(null))
            .Verifiable();
        
        //Act
        await _fixture.MockImageService.ClearUnusedImagesAsync(body, 17);
        
        //Assert
        Assert.Equal(expectedDeletedImages, actualDeletedImages);
        
        _fixture.MockMemoryCache
            .Verify(
                cache => cache.Remove(It.IsAny<Object>()), 
                Times.Once);
    }
    
    [Fact]
    public async Task ClearUnusedImagesAsync_whenCacheIsNotEmptyAndNoCachedImagesPresentInBody_thenClearCacheAndDeleteAllCachedImages()
    {
        var currentFileNames = new List<string>()
        {
            //Unused
            "378b330c-5fe7-4ee2-aa4e-5668473bce5c.png",
            "aa84fb1c-f0e8-4925-a98e-94c67849605e.jpg",
            "6f850056-9032-4ecf-9e2c-b7f531ecdd62.png", 
            "79938366-258d-4f0f-8dd2-61efc84e19b2.webp"
        } as object;

        var expectedDeletedImages = new List<string>()
        {
            "378b330c-5fe7-4ee2-aa4e-5668473bce5c.png",
            "aa84fb1c-f0e8-4925-a98e-94c67849605e.jpg",
            "6f850056-9032-4ecf-9e2c-b7f531ecdd62.png",
            "79938366-258d-4f0f-8dd2-61efc84e19b2.webp"
        };
        
        var body = 
            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/492a5cbb-4998-4bc4-94f6-6f5c97194f7c.png\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/0dce4f04-58a9-4c61-94a8-c4ced4ead76d.jpg\"><img src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e43f8e7a-b99a-4b53-811d-59bcdbe502aa.webp\">;<img src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/45025163-0b68-4ea0-9fd6-e74a5e49a894.png\">";

        _fixture.MockMemoryCache
            .Setup(cache => cache.TryGetValue(
                It.IsAny<object>(), 
                out currentFileNames));
        
        _fixture.MockMemoryCache
            .Setup(cache => cache.Remove(It.IsAny<Object>()))
            .Verifiable();

        var actualDeletedImages = new List<string>();
        _fixture.MockImageRepository
            .Setup(repo => repo.DeleteAsync(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .Callback<string, string>((arg1, arg2) => actualDeletedImages.Add(arg1))
            .Returns(Task.FromResult<object?>(null))
            .Verifiable();
        
        //Act
        await _fixture.MockImageService.ClearUnusedImagesAsync(body, 17);
        
        //Assert
        Assert.Equal(expectedDeletedImages, actualDeletedImages);
        
        _fixture.MockMemoryCache
            .Verify(
                cache => cache.Remove(It.IsAny<Object>()), 
                Times.Once);
    }
    
    [Fact]
    public async Task ClearUnusedImagesAsync_whenCacheIsNotEmptyAndRepositoryThrows_thenThrowBadRequest()
    {
        var currentFileNames = new List<string>()
        {
            //Used
            "492a5cbb-4998-4bc4-94f6-6f5c97194f7c.png",
            "0dce4f04-58a9-4c61-94a8-c4ced4ead76d.jpg",
            "45025163-0b68-4ea0-9fd6-e74a5e49a894.png", 
            "e43f8e7a-b99a-4b53-811d-59bcdbe502aa.webp",
            //Unused
            "378b330c-5fe7-4ee2-aa4e-5668473bce5c.png",
            "aa84fb1c-f0e8-4925-a98e-94c67849605e.jpg",
            "6f850056-9032-4ecf-9e2c-b7f531ecdd62.png", 
            "79938366-258d-4f0f-8dd2-61efc84e19b2.webp"
        } as object;

        var body = 
            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/492a5cbb-4998-4bc4-94f6-6f5c97194f7c.png\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/0dce4f04-58a9-4c61-94a8-c4ced4ead76d.jpg\"><img src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e43f8e7a-b99a-4b53-811d-59bcdbe502aa.webp\">;<img src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/45025163-0b68-4ea0-9fd6-e74a5e49a894.png\">";

        _fixture.MockMemoryCache
            .Setup(cache => cache.TryGetValue(
                It.IsAny<object>(), 
                out currentFileNames));

        _fixture.MockImageRepository
            .Setup(repo => repo.DeleteAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws(new RequestFailedException(""));
        
        //Act
        var resultTask = _fixture.MockImageService.ClearUnusedImagesAsync(body, 17);
        
        //Assert
        await Assert.ThrowsAsync<BadRequestException>(() => resultTask);
    }

    [Fact]
    public async Task DiscardCachedImagesAsync_whenCacheIsEmpty_thenReturn()
    {
        //Arrange
        var currentFileNames = null as object;
        
        _fixture.MockMemoryCache
            .Setup(cache => cache.TryGetValue(
                It.IsAny<object>(), 
                out currentFileNames));

        //Act
        await _fixture.MockImageService.DiscardCachedImagesAsync(17);
        
        //Assert
        _fixture.MockImageRepository
            .Verify( 
                repo => repo.DeleteAsync(
                    It.IsAny<string>(), 
                    It.IsAny<string>()), 
                Times.Never);
        
        _fixture.MockMemoryCache
            .Verify(
                cache => cache.Remove(It.IsAny<Object>()), 
                Times.Never);
    }
    
    [Fact]
    public async Task DiscardCachedImagesAsync_whenCacheIsNotEmptyAndRepositoryThrows_thenThrowBadRequest()
    {
        var currentFileNames = new List<string>()
        {
            "492a5cbb-4998-4bc4-94f6-6f5c97194f7c.png",
            "0dce4f04-58a9-4c61-94a8-c4ced4ead76d.jpg",
            "45025163-0b68-4ea0-9fd6-e74a5e49a894.png", 
            "e43f8e7a-b99a-4b53-811d-59bcdbe502aa.webp",
        } as object;
        
        _fixture.MockMemoryCache
            .Setup(cache => cache.TryGetValue(
                It.IsAny<object>(), 
                out currentFileNames));

        _fixture.MockImageRepository
            .Setup(repo => repo.DeleteAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws(new RequestFailedException(""));
        
        //Act
        var resultTask = _fixture.MockImageService.DiscardCachedImagesAsync(17);
        
        //Assert
        await Assert.ThrowsAsync<BadRequestException>(() => resultTask);
    }

    [Fact]
    public async Task DiscardCachedImagesAsync_whenCacheIsNotEmpty_thenDeleteAllImages()
    {
        var currentFileNames = new List<string>()
        {
            "378b330c-5fe7-4ee2-aa4e-5668473bce5c.png",
            "aa84fb1c-f0e8-4925-a98e-94c67849605e.jpg",
            "6f850056-9032-4ecf-9e2c-b7f531ecdd62.png", 
            "79938366-258d-4f0f-8dd2-61efc84e19b2.webp"
        } as object;

        var expectedDeletedImages = new List<string>()
        {
            "378b330c-5fe7-4ee2-aa4e-5668473bce5c.png",
            "aa84fb1c-f0e8-4925-a98e-94c67849605e.jpg",
            "6f850056-9032-4ecf-9e2c-b7f531ecdd62.png",
            "79938366-258d-4f0f-8dd2-61efc84e19b2.webp"
        };
        
        _fixture.MockMemoryCache
            .Setup(cache => cache.TryGetValue(
                It.IsAny<object>(), 
                out currentFileNames));
        
        _fixture.MockMemoryCache
            .Setup(cache => cache.Remove(It.IsAny<Object>()))
            .Verifiable();

        var actualDeletedImages = new List<string>();
        _fixture.MockImageRepository
            .Setup(repo => repo.DeleteAsync(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .Callback<string, string>((arg1, arg2) => actualDeletedImages.Add(arg1))
            .Returns(Task.FromResult<object?>(null))
            .Verifiable();
        
        //Act
        await _fixture.MockImageService.DiscardCachedImagesAsync(17);
        
        //Assert
        Assert.Equal(expectedDeletedImages, actualDeletedImages);
        
        _fixture.MockMemoryCache
            .Verify(
                cache => cache.Remove(It.IsAny<Object>()), 
                Times.Once);
    }
}