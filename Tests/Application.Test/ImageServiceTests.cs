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
}
//
//     [Fact]
//     public async Task UploadImageAsync_whenMemoryCacheIsInitialized_thenSuccess()
//     {
//         //Arrange
//         IFormFile file = new FormFile(
//             new MemoryStream(new byte[0]), 0, 0, "article_image", "new-image.png");
//
//         var mockFile = new Mock<IFormFile>();
//         mockFile.Setup(f => f.ContentType).Returns("image/png");
//
//         var currentFileNames = new List<string>()
//         {
//             "492a5cbb-4998-4bc4-94f6-6f5c97194f7c.png",
//             "0dce4f04-58a9-4c61-94a8-c4ced4ead76d.jpg",
//             "45025163-0b68-4ea0-9fd6-e74a5e49a894.png",
//             "e43f8e7a-b99a-4b53-811d-59bcdbe502aa.webp"
//         };
//         
//         _fixture.MockImageRepository
//             .Setup(repo => repo.UploadFromIFormFile(
//                 It.IsAny<IFormFile>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>()))
//             .ReturnsAsync("http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/439e7759-7de1-42e8-ad6d-8bed3723b676.png");
//         
//         //Act
//         var link = await _fixture.MockImageService.UploadImageAsync(mockFile.Object, 12);
//         //Assert
//         Assert.Equal("http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/439e7759-7de1-42e8-ad6d-8bed3723b676.png", link);
//     }
//
//     [Fact]
//     public void ParseImgTag_whenInnerLinkWithoutAttributes()
//     {
//         //Arrange
//         var tag = "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/439e7759-7de1-42e8-ad6d-8bed3723b676.png\">";
//     
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
//             .Returns("http://127.0.0.1:10000/devstoreaccount1");
//         
//         //Act
//         _fixture.MockImageService.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
//         
//         //Assert
//         Assert.False(isBase64);
//         Assert.Equal("http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/439e7759-7de1-42e8-ad6d-8bed3723b676.png", link);
//         Assert.Empty(format);
//         Assert.Empty(base64);
//         Assert.False(isOuter);
//     }
//     
//     [Fact]
//     public void ParseImgTag_whenOuterLinkWithoutAttributes()
//     {
//         //Arrange
//         var tag = "<img src=\"https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg\">";
//         
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
//             .Returns("http://127.0.0.1:10000/devstoreaccount1");
//         
//         //Act
//         _fixture.MockImageService.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
//         
//         //Assert
//         Assert.False(isBase64);
//         Assert.Equal("https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg", link);
//         Assert.Empty(format);
//         Assert.Empty(base64);
//         Assert.True(isOuter);
//     }
//     
//     [Fact]
//     public void ParseImgTag_whenOuterLinkWithAttributesAtTheBeginning()
//     {
//         //Arrange
//         var tag = "<img alt=\"hello\" style=\"hello: hello\" src=\"https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg\">";
//         
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
//             .Returns("http://127.0.0.1:10000/devstoreaccount1");
//         
//         //Act
//         _fixture.MockImageService.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
//         
//         //Assert
//         Assert.False(isBase64);
//         Assert.Equal("https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg", link);
//         Assert.Empty(format);
//         Assert.Empty(base64);
//         Assert.True(isOuter);
//     }
//     
//     [Fact]
//     public void ParseImgTag_whenOuterLinkWithAttributesAtTheEnd()
//     {
//         //Arrange
//         var tag = "<img    src=\"https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg\" alt=\"hello\" style=\"hello: hello\">";
//         
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
//             .Returns("http://127.0.0.1:10000/devstoreaccount1");
//         
//         //Act
//         _fixture.MockImageService.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
//         
//         //Assert
//         Assert.False(isBase64);
//         Assert.Equal("https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg", link);
//         Assert.Empty(format);
//         Assert.Empty(base64);
//         Assert.True(isOuter);
//     }
//     
//     [Fact]
//     public void ParseImgTag_whenOuterLinkWithAttributesAtTheBeginningAndAtTheEnd()
//     {
//         //Arrange
//         var tag = "<img  alt=\"hello\"  src=\"https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg\"  style=\"hello: hello\">";
//         
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
//             .Returns("http://127.0.0.1:10000/devstoreaccount1");
//         
//         //Act
//         _fixture.MockImageService.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
//         
//         //Assert
//         Assert.False(isBase64);
//         Assert.Equal("https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg", link);
//         Assert.Empty(format);
//         Assert.Empty(base64);
//         Assert.True(isOuter);
//     }
//     
//     [Fact]
//     public void ParseImgTag_whenOuterLinkWithQueryParametersAndAttributesAtTheBeginningAndAtTheEnd()
//     {
//         //Arrange
//         var tag = "<img alt=\"hello\"  src=\"https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg?fit=fill&amp;w=800&amp;h=300\"  style=\"hello: hello\">";
//         
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
//             .Returns("http://127.0.0.1:10000/devstoreaccount1");
//         
//         //Act
//         _fixture.MockImageService.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
//         
//         //Assert
//         Assert.False(isBase64);
//         Assert.Equal("https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg", link);
//         Assert.Empty(format);
//         Assert.Empty(base64);
//         Assert.True(isOuter);
//     }
//     
//     [Fact]
//     public async Task UploadImages_when1Base64ImagesWithAttributesAtTheBeginningAndAtTheEnd_surroundedByOtherTags()
//     {
//         //Arrange
//         var tag =
//             "<div class=\"div class\">Plain text plain text<span class=\"mat-display-1\"></span>" +
//             "<img class=\"div class\"    alt=\"222hello222\" src=\"data:image/png;base64,***the SECOND string***\" style=\"222hello222: hello333\">" +
//             "<div><ul><li>Hello</li><li>World</li></ul><div></div>";
//         
//         _fixture.MockImageRepository
//             .Setup(r => r.UploadFromBase64Async(
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>()))
//             .ReturnsAsync("folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format")
//             .Verifiable();
//         
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
//             .Returns("http://127.0.0.1:10000/devstoreaccount1");
//         
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerName")])
//             .Returns("vet-clinic");
//         
//         //Act
//         var result = await _fixture.MockImageService.ClearOutdatedImagesAsync(tag);
//         //Assert
//         _fixture.MockImageRepository.Verify(repo => repo.UploadFromBase64Async(                
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>()),
//             Times.Exactly(1));
//     
//         Assert.Equal("<div class=\"div class\">Plain text plain text<span class=\"mat-display-1\"></span>" +
//                      "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\">" +
//                      "<div><ul><li>Hello</li><li>World</li></ul><div></div>", result);
//         _fixture.MockImageRepository.ResetCalls();
//     }
//     
//     [Fact]
//     public async Task UploadImages_when3Base64ImagesWithAttributesAtTheBeginningAndAtTheEnd_inARow()
//     {
//         //Arrange
//         var tag =
//             "<div class=\"div class\"><img alt=\"111hello111\" src=\"data:image/jpeg;base64,***the FIRST string***\" style=\"111hello111: hello111\">" +
//             "<img class=\"div class\"    alt=\"222hello222\" src=\"data:image/png;base64,***the SECOND string***\" style=\"222hello222: hello333\">" +
//             "<img class=\"div class\" alt=\"333hello333\" src=\"data:image/webp;base64,***the THIRD string***\" style=\"333hello333: hello333\"></div>";
//         
//         _fixture.MockImageRepository
//             .Setup(r => r.UploadFromBase64Async(
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>()))
//             .ReturnsAsync("folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format")
//             .Verifiable();
//         
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
//             .Returns("http://127.0.0.1:10000/devstoreaccount1");
//         
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerName")])
//             .Returns("vet-clinic");
//         
//         //Act
//         var result = await _fixture.MockImageService.ClearOutdatedImagesAsync(tag);
//         
//         //Assert
//         _fixture.MockImageRepository.Verify(repo => repo.UploadFromBase64Async(                
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>()),
//             Times.Exactly(3));
//     
//         Assert.Equal("<div class=\"div class\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\">" +
//                             "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\">" +
//                             "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\"></div>", result);
//         _fixture.MockImageRepository.ResetCalls();
//     }
//     
//     [Fact]
//     public async Task UploadImages_when3Base64ImagesWithAttributesAtTheBeginningAndAtTheEnd_insideSeparateTags()
//     {
//         //Arrange
//         var tag =
//             "<div class=\"div class\"><img alt=\"111hello111\" src=\"data:image/jpeg;base64,***the FIRST string***\" style=\"111hello111: hello111\"></div>" +
//             "<div class=\"div class\"><img alt=\"222hello222\" src=\"data:image/png;base64,***the SECOND string***\" style=\"222hello222: hello333\"></div>" +
//             "<div class=\"div class\"><img alt=\"333hello333\" src=\"data:image/webp;base64,***the THIRD string***\" style=\"333hello333: hello333\"></div>";
//         
//         _fixture.MockImageRepository
//             .Setup(r => r.UploadFromBase64Async(
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>()))
//             .ReturnsAsync("folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format")
//             .Verifiable();
//         
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
//             .Returns("http://127.0.0.1:10000/devstoreaccount1");
//         
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerName")])
//             .Returns("vet-clinic");
//         
//         //Act
//         var result = await _fixture.MockImageService.ClearOutdatedImagesAsync(tag);
//         
//         //Assert
//         _fixture.MockImageRepository.Verify(repo => repo.UploadFromBase64Async(                
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>()),
//             Times.Exactly(3));
//     
//         Assert.Equal("<div class=\"div class\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\"></div>" +
//                             "<div class=\"div class\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\"></div>" +
//                             "<div class=\"div class\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\"></div>", result);
//         _fixture.MockImageRepository.ResetCalls();
//     }
//     
//     [Fact]
//     public async Task UploadImages_when3DifferentImagesWithAttributesAtTheBeginningAndAtTheEnd_inARow()
//     {
//         //Arrange
//         var tag =
//             "<div class=\"div class\"><img alt=\"111hello111\" src=\"data:image/jpeg;base64,***the FIRST string***\" style=\"111hello111: hello111\">" +
//             "<img class=\"second image div class\" alt=\"222hello222\" src=\"https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg?fit=fill&amp;w=800&amp;h=300\" style=\"222hello222: hello333\">" +
//             "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/another-inner-file-name.format\"></div>";
//         
//         _fixture.MockImageRepository
//             .Setup(r => r.UploadFromBase64Async(
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>()))
//             .ReturnsAsync("folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format")
//             .Verifiable();
//         
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
//             .Returns("http://127.0.0.1:10000/devstoreaccount1");
//         
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerName")])
//             .Returns("vet-clinic");
//         
//         //Act
//         var result = await _fixture.MockImageService.ClearOutdatedImagesAsync(tag);
//         //Assert
//         _fixture.MockImageRepository.Verify(repo => repo.UploadFromBase64Async(                
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>()),
//             Times.Exactly(1));
//     
//         Assert.Equal("<div class=\"div class\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\">" +
//                             "<img src=\"https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg\">" +
//                             "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/another-inner-file-name.format\"></div>", result);
//         _fixture.MockImageRepository.ResetCalls();
//     }
//     
//     [Fact]
//     public async Task DeleteImages_when3InnerLinks_inARow()
//     {
//         //Arrange
//         var tag =
//             "<div class=\"div class\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\">" +
//             "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\">" +
//             "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\"></div>";
//         
//         _fixture.MockImageRepository
//             .Setup(r => r.DeleteAsync(
//                 It.IsAny<string>(),
//                 It.IsAny<string>()))
//             .Returns(Task.FromResult<object?>(null)).Verifiable();
//     
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
//             .Returns("http://127.0.0.1:10000/devstoreaccount1");
//         
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerName")])
//             .Returns("vet-clinic");
//         
//         //Act
//         var result = await _fixture.MockImageService.DeleteImagesAsync(tag);
//         
//         //Assert
//         _fixture.MockImageRepository.Verify(repo => repo.DeleteAsync(                
//                 It.IsAny<string>(),
//                 It.IsAny<string>()),
//             Times.Exactly(3));
//     
//         Assert.Equal("<div class=\"div class\"></div>", result);
//         
//         _fixture.MockImageRepository.ResetCalls();
//     }
//     
//     [Fact]
//     public async Task DeleteImages_when3InnerLinks_insideSeparateTags()
//     {
//         //Arrange
//         var tag =
//             "<div class=\"div class\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\"></div>" +
//             "<div class=\"div class\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\"></div>" +
//             "<div class=\"div class\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\"></div>";
//         
//         _fixture.MockImageRepository
//             .Setup(r => r.DeleteAsync(
//                 It.IsAny<string>(),
//                 It.IsAny<string>()))
//             .Returns(Task.FromResult<object?>(null)).Verifiable();
//     
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
//             .Returns("http://127.0.0.1:10000/devstoreaccount1");
//         
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerName")])
//             .Returns("vet-clinic");
//         
//         //Act
//         var result = await _fixture.MockImageService.DeleteImagesAsync(tag);
//         
//         //Assert
//         _fixture.MockImageRepository.Verify(repo => repo.DeleteAsync(                
//                 It.IsAny<string>(),
//                 It.IsAny<string>()),
//             Times.Exactly(3));
//     
//         Assert.Equal("<div class=\"div class\"></div><div class=\"div class\"></div><div class=\"div class\"></div>", result);
//         
//         _fixture.MockImageRepository.ResetCalls();
//     }
//     
//     [Fact]
//     public async Task DeleteImages_when3OuterLinks_inARow()
//     {
//         //Arrange
//         var tag =
//             "<div class=\"div class\"><img src=\"https://image.shutterstock.com/image-photo/mountains-under-mist-morning-amazing-260nw-1725825019.jpg\">" +
//             "<img src=\"https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg\">" +
//             "<img src=\"https://cdn.pixabay.com/photo/2014/02/27/16/10/tree-276014__340.jpg\"></div>";
//         
//         _fixture.MockImageRepository
//             .Setup(r => r.DeleteAsync(
//                 It.IsAny<string>(),
//                 It.IsAny<string>()))
//             .Returns(Task.FromResult<object?>(null)).Verifiable();
//         
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
//             .Returns("http://127.0.0.1:10000/devstoreaccount1");
//         
//         _fixture.MockConfiguration
//             .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerName")])
//             .Returns("vet-clinic");
//         
//         //Act
//         var result = await _fixture.MockImageService.DeleteImagesAsync(tag);
//         
//         //Assert
//         _fixture.MockImageRepository.Verify(repo => repo.DeleteAsync(                
//                 It.IsAny<string>(),
//                 It.IsAny<string>()),
//             Times.Never);
//     
//         Assert.Equal("<div class=\"div class\"></div>", result);
//         
//         _fixture.MockImageRepository.ResetCalls();
//     }
// }