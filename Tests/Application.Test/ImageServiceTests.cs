// using Application.Test.Fixtures;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Caching.Memory;
// using Moq;
//
// namespace Application.Test;
//
// public class ImageServiceTests : IClassFixture<ImageServiceFixture>
// {
//     private readonly ImageServiceFixture _fixture;
//
//     public ImageServiceTests(ImageServiceFixture fixture)
//     {
//         _fixture = fixture;
//     }
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