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
}