using Application.Test.Fixtures;
using Moq;

namespace Application.Test;

public class ImageParserTests : IClassFixture<ImageParserFixture>
{
    private readonly ImageParserFixture _fixture;

    public ImageParserTests(ImageParserFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public void ParseImgTag_whenBase64PngWithoutAttributes()
    {
        //Arrange
        var tag = "<img src=\"data:image/png;base64,***base 64 string***\">";
        
        //Act
        _fixture.ImageParser.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
        
        //Assert
        Assert.True(isBase64);
        Assert.Equal("***base 64 string***", base64);
        Assert.Equal("png", format);
        Assert.Empty(link);
        Assert.False(isOuter);
    }
    
    [Fact]
    public void ParseImgTag_whenBase64PngWithAttributesAtTheBeginning()
    {
        //Arrange
        var tag = "<img alt=\"hello\" src=\"data:image/png;base64,***base 64 string***\">";
        
        //Act
        _fixture.ImageParser.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
        
        //Assert
        Assert.True(isBase64);
        Assert.Equal("***base 64 string***", base64);
        Assert.Equal("png", format);
        Assert.Empty(link);
        Assert.False(isOuter);
    }
    
    [Fact]
    public void ParseImgTag_whenBase64PngWithAttributesAtTheEnd()
    {
        //Arrange
        var tag = "<img src=\"data:image/png;base64,***base 64 string***\" alt=\"hello\" style=\"hello: hello\">";
        
        //Act
        _fixture.ImageParser.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
        
        //Assert
        Assert.True(isBase64);
        Assert.Equal("***base 64 string***", base64);
        Assert.Equal("png", format);
        Assert.Empty(link);
        Assert.False(isOuter);
    }
    
    [Fact]
    public void ParseImgTag_whenBase64PngWithAttributesAtTheBeginningAndAtTheEnd()
    {
        //Arrange
        var tag = "<img alt=\"hello\" src=\"data:image/png;base64,***base 64 string***\" style=\"hello: hello\">";
        
        //Act
        _fixture.ImageParser.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
        
        //Assert
        Assert.True(isBase64);
        Assert.Equal("***base 64 string***", base64);
        Assert.Equal("png", format);
        Assert.Empty(link);
        Assert.False(isOuter);
    }
    
    [Fact]
    public void ParseImgTag_whenBase64JpegWithoutAttributes()
    {
        //Arrange
        var tag = "<img src=\"data:image/jpeg;base64,***base 64 string***\">";
        
        //Act
        _fixture.ImageParser.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
        
        //Assert
        Assert.True(isBase64);
        Assert.Equal("***base 64 string***", base64);
        Assert.Equal("jpeg", format);
        Assert.Empty(link);
        Assert.False(isOuter);
    }
    
    [Fact]
    public void ParseImgTag_whenBase64JpegWithAttributesAtTheBeginning()
    {
        //Arrange
        var tag = "<img alt=\"hello\" src=\"data:image/jpeg;base64,***base 64 string***\">";
        
        //Act
        _fixture.ImageParser.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
        
        //Assert
        Assert.True(isBase64);
        Assert.Equal("***base 64 string***", base64);
        Assert.Equal("jpeg", format);
        Assert.Empty(link);
        Assert.False(isOuter);
    }
    
    [Fact]
    public void ParseImgTag_whenBase64JpegWithAttributesAtTheEnd()
    {
        //Arrange
        var tag = "<img src=\"data:image/jpeg;base64,***base 64 string***\" alt=\"hello\" style=\"hello: hello\">";
        
        //Act
        _fixture.ImageParser.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
        
        //Assert
        Assert.True(isBase64);
        Assert.Equal("***base 64 string***", base64);
        Assert.Equal("jpeg", format);
        Assert.Empty(link);
        Assert.False(isOuter);
    }
    
    [Fact]
    public void ParseImgTag_whenBase64JpegWithAttributesAtTheBeginningAndAtTheEnd()
    {
        //Arrange
        var tag = "<img alt=\"hello\" src=\"data:image/jpeg;base64,***base 64 string***\" style=\"hello: hello\">";
        
        //Act
        _fixture.ImageParser.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
        
        //Assert
        Assert.True(isBase64);
        Assert.Equal("***base 64 string***", base64);
        Assert.Equal("jpeg", format);
        Assert.Empty(link);
        Assert.False(isOuter);
    }
    
    [Fact]
    public void ParseImgTag_whenInnerLinkWithoutAttributes()
    {
        //Arrange
        var tag = "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/439e7759-7de1-42e8-ad6d-8bed3723b676.png\">";

        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        //Act
        _fixture.ImageParser.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
        
        //Assert
        Assert.False(isBase64);
        Assert.Equal("http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/439e7759-7de1-42e8-ad6d-8bed3723b676.png", link);
        Assert.Empty(format);
        Assert.Empty(base64);
        Assert.False(isOuter);
    }
    
    [Fact]
    public void ParseImgTag_whenOuterLinkWithoutAttributes()
    {
        //Arrange
        var tag = "<img src=\"https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg\">";
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        //Act
        _fixture.ImageParser.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
        
        //Assert
        Assert.False(isBase64);
        Assert.Equal("https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg", link);
        Assert.Empty(format);
        Assert.Empty(base64);
        Assert.True(isOuter);
    }

    [Fact]
    public void ParseImgTag_whenOuterLinkWithAttributesAtTheBeginning()
    {
        //Arrange
        var tag = "<img alt=\"hello\" style=\"hello: hello\" src=\"https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg\">";
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        //Act
        _fixture.ImageParser.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
        
        //Assert
        Assert.False(isBase64);
        Assert.Equal("https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg", link);
        Assert.Empty(format);
        Assert.Empty(base64);
        Assert.True(isOuter);
    }

    [Fact]
    public void ParseImgTag_whenOuterLinkWithAttributesAtTheEnd()
    {
        //Arrange
        var tag = "<img    src=\"https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg\" alt=\"hello\" style=\"hello: hello\">";
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        //Act
        _fixture.ImageParser.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
        
        //Assert
        Assert.False(isBase64);
        Assert.Equal("https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg", link);
        Assert.Empty(format);
        Assert.Empty(base64);
        Assert.True(isOuter);
    }

    [Fact]
    public void ParseImgTag_whenOuterLinkWithAttributesAtTheBeginningAndAtTheEnd()
    {
        //Arrange
        var tag = "<img  alt=\"hello\"  src=\"https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg\"  style=\"hello: hello\">";
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        //Act
        _fixture.ImageParser.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
        
        //Assert
        Assert.False(isBase64);
        Assert.Equal("https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg", link);
        Assert.Empty(format);
        Assert.Empty(base64);
        Assert.True(isOuter);
    }
    
    [Fact]
    public void ParseImgTag_whenOuterLinkWithQueryParametersAndAttributesAtTheBeginningAndAtTheEnd()
    {
        //Arrange
        var tag = "<img alt=\"hello\"  src=\"https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg?fit=fill&amp;w=800&amp;h=300\"  style=\"hello: hello\">";
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        //Act
        _fixture.ImageParser.ParseImgTag(tag, out var isBase64, out var base64, out var format, out var link, out var isOuter );
        
        //Assert
        Assert.False(isBase64);
        Assert.Equal("https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg", link);
        Assert.Empty(format);
        Assert.Empty(base64);
        Assert.True(isOuter);
    }
    
    [Fact]
    public async Task UploadImages_when1Base64ImagesWithAttributesAtTheBeginningAndAtTheEnd_surroundedByOtherTags()
    {
        //Arrange
        var tag =
            "<div class=\"div class\">Plain text plain text<span class=\"mat-display-1\"></span>" +
            "<img class=\"div class\"    alt=\"222hello222\" src=\"data:image/png;base64,***the SECOND string***\" style=\"222hello222: hello333\">" +
            "<div><ul><li>Hello</li><li>World</li></ul><div></div>";
        
        _fixture.MockImageRepository
            .Setup(r => r.UploadFromBase64Async(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync("folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format")
            .Verifiable();
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerName")])
            .Returns("vet-clinic");
        
        //Act
        var result = await _fixture.ImageParser.UploadImages(tag);
        //Assert
        _fixture.MockImageRepository.Verify(repo => repo.UploadFromBase64Async(                
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Exactly(1));

        Assert.Equal("<div class=\"div class\">Plain text plain text<span class=\"mat-display-1\"></span>" +
                     "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\">" +
                     "<div><ul><li>Hello</li><li>World</li></ul><div></div>", result);
        _fixture.MockImageRepository.ResetCalls();
    }

    [Fact]
    public async Task UploadImages_when3Base64ImagesWithAttributesAtTheBeginningAndAtTheEnd_inARow()
    {
        //Arrange
        var tag =
            "<div class=\"div class\"><img alt=\"111hello111\" src=\"data:image/jpeg;base64,***the FIRST string***\" style=\"111hello111: hello111\">" +
            "<img class=\"div class\"    alt=\"222hello222\" src=\"data:image/png;base64,***the SECOND string***\" style=\"222hello222: hello333\">" +
            "<img class=\"div class\" alt=\"333hello333\" src=\"data:image/webp;base64,***the THIRD string***\" style=\"333hello333: hello333\"></div>";
        
        _fixture.MockImageRepository
            .Setup(r => r.UploadFromBase64Async(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync("folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format")
            .Verifiable();
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerName")])
            .Returns("vet-clinic");
        
        //Act
        var result = await _fixture.ImageParser.UploadImages(tag);
        
        //Assert
        _fixture.MockImageRepository.Verify(repo => repo.UploadFromBase64Async(                
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Exactly(3));

        Assert.Equal("<div class=\"div class\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\">" +
                            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\">" +
                            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\"></div>", result);
        _fixture.MockImageRepository.ResetCalls();
    }
    
    [Fact]
    public async Task UploadImages_when3Base64ImagesWithAttributesAtTheBeginningAndAtTheEnd_insideSeparateTags()
    {
        //Arrange
        var tag =
            "<div class=\"div class\"><img alt=\"111hello111\" src=\"data:image/jpeg;base64,***the FIRST string***\" style=\"111hello111: hello111\"></div>" +
            "<div class=\"div class\"><img alt=\"222hello222\" src=\"data:image/png;base64,***the SECOND string***\" style=\"222hello222: hello333\"></div>" +
            "<div class=\"div class\"><img alt=\"333hello333\" src=\"data:image/webp;base64,***the THIRD string***\" style=\"333hello333: hello333\"></div>";
        
        _fixture.MockImageRepository
            .Setup(r => r.UploadFromBase64Async(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync("folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format")
            .Verifiable();
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerName")])
            .Returns("vet-clinic");
        
        //Act
        var result = await _fixture.ImageParser.UploadImages(tag);
        
        //Assert
        _fixture.MockImageRepository.Verify(repo => repo.UploadFromBase64Async(                
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Exactly(3));

        Assert.Equal("<div class=\"div class\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\"></div>" +
                            "<div class=\"div class\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\"></div>" +
                            "<div class=\"div class\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\"></div>", result);
        _fixture.MockImageRepository.ResetCalls();
    }
    
    [Fact]
    public async Task UploadImages_when3DifferentImagesWithAttributesAtTheBeginningAndAtTheEnd_inARow()
    {
        //Arrange
        var tag =
            "<div class=\"div class\"><img alt=\"111hello111\" src=\"data:image/jpeg;base64,***the FIRST string***\" style=\"111hello111: hello111\">" +
            "<img class=\"second image div class\" alt=\"222hello222\" src=\"https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg?fit=fill&amp;w=800&amp;h=300\" style=\"222hello222: hello333\">" +
            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/another-inner-file-name.format\"></div>";
        
        _fixture.MockImageRepository
            .Setup(r => r.UploadFromBase64Async(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync("folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format")
            .Verifiable();
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerName")])
            .Returns("vet-clinic");
        
        //Act
        var result = await _fixture.ImageParser.UploadImages(tag);
        //Assert
        _fixture.MockImageRepository.Verify(repo => repo.UploadFromBase64Async(                
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Exactly(1));

        Assert.Equal("<div class=\"div class\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\">" +
                            "<img src=\"https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg\">" +
                            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/another-inner-file-name.format\"></div>", result);
        _fixture.MockImageRepository.ResetCalls();
    }
    
    [Fact]
    public async Task DeleteImages_when3InnerLinks_inARow()
    {
        //Arrange
        var tag =
            "<div class=\"div class\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\">" +
            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\">" +
            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\"></div>";
        
        _fixture.MockImageRepository
            .Setup(r => r.DeleteAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();

        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerName")])
            .Returns("vet-clinic");
        
        //Act
        var result = await _fixture.ImageParser.DeleteImages(tag);
        
        //Assert
        _fixture.MockImageRepository.Verify(repo => repo.DeleteAsync(                
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Exactly(3));

        Assert.Equal("<div class=\"div class\"></div>", result);
        
        _fixture.MockImageRepository.ResetCalls();
    }
    
    [Fact]
    public async Task DeleteImages_when3InnerLinks_insideSeparateTags()
    {
        //Arrange
        var tag =
            "<div class=\"div class\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\"></div>" +
            "<div class=\"div class\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\"></div>" +
            "<div class=\"div class\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/folder/439e7759-7de1-42e8-ad6d-8bed3723b676.format\"></div>";
        
        _fixture.MockImageRepository
            .Setup(r => r.DeleteAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();

        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerName")])
            .Returns("vet-clinic");
        
        //Act
        var result = await _fixture.ImageParser.DeleteImages(tag);
        
        //Assert
        _fixture.MockImageRepository.Verify(repo => repo.DeleteAsync(                
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Exactly(3));

        Assert.Equal("<div class=\"div class\"></div><div class=\"div class\"></div><div class=\"div class\"></div>", result);
        
        _fixture.MockImageRepository.ResetCalls();
    }
    
    [Fact]
    public async Task DeleteImages_when3OuterLinks_inARow()
    {
        //Arrange
        var tag =
            "<div class=\"div class\"><img src=\"https://image.shutterstock.com/image-photo/mountains-under-mist-morning-amazing-260nw-1725825019.jpg\">" +
            "<img src=\"https://helpx.adobe.com/content/dam/help/en/stock/how-to/visual-reverse-image-search/jcr_content/main-pars/image/visual-reverse-image-search-v2_intro.jpg\">" +
            "<img src=\"https://cdn.pixabay.com/photo/2014/02/27/16/10/tree-276014__340.jpg\"></div>";
        
        _fixture.MockImageRepository
            .Setup(r => r.DeleteAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerLink")])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        _fixture.MockConfiguration
            .Setup(conf => conf[It.Is<string>(s => s == "Azure:ContainerName")])
            .Returns("vet-clinic");
        
        //Act
        var result = await _fixture.ImageParser.DeleteImages(tag);
        
        //Assert
        _fixture.MockImageRepository.Verify(repo => repo.DeleteAsync(                
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Never);

        Assert.Equal("<div class=\"div class\"></div>", result);
        
        _fixture.MockImageRepository.ResetCalls();
    }
}