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
        //Act
        var result = _fixture.MockImageService.TrimArticleImages(_fixture.BodyWithNoImages);
        //Assert
        Assert.Equal(_fixture.BodyWithNoImages, result);
    }

    [Fact]
    public void TrimArticleImages_whenBodyHasThreeInnerImages_thenUnchangedBodyReturned()
    {
        //Arrange
        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        //Act
        var result = _fixture.MockImageService.TrimArticleImages(_fixture.BodyWithThreeInnerImages);
        //Assert
        Assert.Equal(_fixture.BodyWithThreeInnerImages, result);
    }

    [Fact]
    public void TrimArticleImages_whenBodyHasThreeInnerAndOuterImagesWithAttributesAtTheEndAndBeginning_thenUnchangedBodyReturned()
    {
        //Arrange
        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        //Act
        var result = _fixture.MockImageService.TrimArticleImages(_fixture.BodyWithThreeInnerAndOuterImages);
        //Assert
        Assert.Equal(_fixture.TrimmedBodyWithThreeInnerAndOuterImages, result);
    }
    
    [Fact]
    public void TrimArticleImages_whenBodyHasThreeInnerAndOuterImagesWithQueryAndAttributesAtTheEndAndBeginning_thenTrimmedBodyReturned()
    {
        //Arrange
        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        //Act
        var result = _fixture.MockImageService.TrimArticleImages(_fixture.BodyWithThreeInnerAndOuterWithQueryImages);
        //Assert
        Assert.Equal(_fixture.TrimmedBodyWithThreeInnerAndOuterWithQueryImages, result);
    }
    
    [Fact]
    public void TrimArticleImages_whenBodyHasThreeInnerAndOuterImagesWithQueryAndAttributesAtTheEndAndBeginningInARow_thenTrimmedBodyReturned()
    {
        //Arrange
        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        //Act
        var result = _fixture.MockImageService.TrimArticleImages(_fixture.BodyWithThreeInnerAndOuterWithQueryAndAttributesImages);
        //Assert
        Assert.Equal(_fixture.TrimmedBodyWithThreeInnerAndOuterWithQueryAndAttributesImages, result);
    }
    
    [Fact]
    public async Task ClearOutdatedImagesAsync_whenBodyDidntChange_thenReturn()
    {
        //Arrange
         _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        //Act
        await _fixture.MockImageService.ClearOutdatedImagesAsync(
            _fixture.BodyWithThreeInnerImages, 
            _fixture.BodyWithThreeInnerImages);
        //Assert
        _fixture.MockImageRepository
            .Verify(
                repo => repo.DeleteAsync(
                    It.IsAny<string>(), 
                    It.IsAny<string>()), 
                Times.Never);
    }
    
    [Fact]
    public async Task ClearOutdatedImagesAsync_whenImagesDidntChange_thenReturn()
    {
        //Arrange
        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        //Act
        await _fixture.MockImageService.ClearOutdatedImagesAsync(
            _fixture.BodyWithThreeInnerImages, 
            _fixture.ChangedBodyWithTheSameImages);
        //Assert
        _fixture.MockImageRepository.Verify(
            repo => repo.DeleteAsync(
                It.IsAny<string>(), 
                It.IsAny<string>()), 
            Times.Never);
    }

    [Fact]
    public async Task ClearOutdatedImagesAsync_whenNewImagesAdded_thenReturn()
    {
        //Arrange
        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        //Act
        await _fixture.MockImageService.ClearOutdatedImagesAsync(
            _fixture.BodyWithAddedImages, 
            _fixture.BodyWithThreeInnerImages);
        //Assert
        _fixture.MockImageRepository.Verify(
            repo => repo.DeleteAsync(
                It.IsAny<string>(), 
                It.IsAny<string>()), 
            Times.Never);
    }

    [Fact]
    public async Task ClearOutdatedImagesAsync_whenAllImagesChanged_thenSuccess()
    {
        //Arrange
        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        _fixture.MockImageRepository
            .Setup(repo => repo.DeleteAsync(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        await _fixture.MockImageService.ClearOutdatedImagesAsync(
            _fixture.BodyWithAllImagesChanged, 
            _fixture.BodyWithThreeInnerImages);
        
        //Assert
        _fixture.MockImageRepository
            .Verify(
                repo => repo.DeleteAsync(
                    It.IsAny<string>(), 
                    It.IsAny<string>()), 
                Times.Exactly(3));
    }    
    
    [Fact]
    public async Task ClearOutdatedImagesAsync_whenRepositoryThrows_thenThrowBadRequestException()
    {
        //Arrange
        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");

        _fixture.MockImageRepository
            .Setup(repo => repo.DeleteAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws(new RequestFailedException(""));
        //Act
        var resultTask = _fixture.MockImageService.ClearOutdatedImagesAsync(
            _fixture.BodyWithAllImagesChanged, 
            _fixture.BodyWithThreeInnerImages);        //Assert
        await Assert.ThrowsAsync<BadRequestException>(() => resultTask);
    }

    [Fact]
    private async Task DeleteImagesAsync_whenBodyHasNoImages_thenReturn()
    {
        //Arrange
        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        _fixture.MockImageRepository
            .Setup(repo => repo.DeleteAsync(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        //Act
        await _fixture.MockImageService.DeleteImagesAsync(_fixture.BodyWithNoImages);
        //Assert
        _fixture.MockImageRepository.Verify(
            repo => repo.DeleteAsync(
                It.IsAny<string>(),
                It.IsAny<string>()), 
            Times.Never);
    }

    [Fact]
    private async Task DeleteImagesAsync_whenBodyHasThreeInnerAndOuter_thenDeleteInner()
    {
        //Arrange
        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");
        
        _fixture.MockImageRepository
            .Setup(repo => repo.DeleteAsync(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .Returns(Task.FromResult<object?>(null))
            .Verifiable();
        //Act
        await _fixture.MockImageService.DeleteImagesAsync(_fixture.BodyWithThreeInnerAndOuterImages);
        //Assert
        _fixture.MockImageRepository
            .Verify(
                repo => repo.DeleteAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>()), 
                Times.Exactly(3));
    }

    [Fact]
    private async Task DeleteImagesAsync_whenRepositoryThrows_thenThrowBadRequestException()
    {
        //Arrange
        _fixture.MockConfiguration
            .Setup(conf => conf[It.IsAny<string>()])
            .Returns("http://127.0.0.1:10000/devstoreaccount1");

        _fixture.MockImageRepository
            .Setup(repo => repo.DeleteAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws(new RequestFailedException(""));
        //Act
        var resultTask = _fixture.MockImageService.DeleteImagesAsync(_fixture.BodyWithThreeInnerImages);
        //Assert
        await Assert.ThrowsAsync<BadRequestException>(() => resultTask);
    }

    [Fact]
    public async Task UploadImageAsync_whenCacheIsNotEmpty_thenAddImageToCacheAndReturnLink()
    {
        //Arrange
        _fixture.MockFormFile
            .Setup(f => f.ContentType)
            .Returns("image/png");
        
        _fixture.MockImageRepository
            .Setup(repo => repo.UploadFromIFormFile(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(_fixture.ExpectedLink);

        _fixture.MockMemoryCache
             .Setup(cache => cache.TryGetValue(
                 It.IsAny<object>(), 
                 out _fixture.DefaultCachedFileNamesList));

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
         var actualLink = await _fixture.MockImageService.UploadImageAsync(_fixture.MockFormFile.Object, 12);
         
         //Assert
         Assert.NotNull(actualNewFileNames);
         Assert.Contains(
             actualNewFileNames!, 
             fileName => fileName == _fixture.ExpectedNewFileName);
         Assert.Equal(_fixture.ExpectedLink, actualLink);
    }
    
    [Fact]
    public async Task UploadImageAsync_whenCacheIsEmpty_thenAddImageToCacheAndReturnLink()
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
            .ReturnsAsync(_fixture.ExpectedLink);

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
         Assert.Contains(
             actualNewFileNames!, 
             fileName => fileName == _fixture.ExpectedNewFileName);
         Assert.Equal(_fixture.ExpectedLink, actualLink);
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
        _fixture.MockMemoryCache
            .Setup(cache => cache.TryGetValue(
                It.IsAny<object>(), 
                out _fixture.EmptyCachedFileNamesList));

        //Act
        await _fixture.MockImageService.ClearUnusedImagesAsync(_fixture.BodyWithThreeInnerImages, 17);
        
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
        _fixture.MockMemoryCache
            .Setup(cache => cache.TryGetValue(
                It.IsAny<object>(), 
                out _fixture.DefaultCachedFileNamesList));
        
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
        await _fixture.MockImageService.ClearUnusedImagesAsync(_fixture.BodyWithAllCachedImagesUsed, 17);
        
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
        //Arrange
        _fixture.MockMemoryCache
            .Setup(cache => cache.TryGetValue(
                It.IsAny<object>(), 
                out _fixture.CachedFileNamesListWithUnusedImages));
        
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
        await _fixture.MockImageService.ClearUnusedImagesAsync(_fixture.BodyWithUsedAndUnusedImages, 17);
        
        //Assert
        Assert.Equal(_fixture.ListOfImagesExpectedToDelete, actualDeletedImages);
        
        _fixture.MockMemoryCache
            .Verify(
                cache => cache.Remove(It.IsAny<Object>()), 
                Times.Once);
    }
    
    [Fact]
    public async Task ClearUnusedImagesAsync_whenCacheIsNotEmptyAndNoCachedImagesPresentInBody_thenClearCacheAndDeleteAllCachedImages()
    {
        //Arrange
        _fixture.MockMemoryCache
            .Setup(cache => cache.TryGetValue(
                It.IsAny<object>(), 
                out _fixture.CachedFileNamesListWithAllUnusedImages));
        
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
        await _fixture.MockImageService.ClearUnusedImagesAsync(_fixture.BodyWithUnusedCachedImages, 17);
        
        //Assert
        Assert.Equal(_fixture.ListOfImagesExpectedToDelete, actualDeletedImages);
        
        _fixture.MockMemoryCache
            .Verify(
                cache => cache.Remove(It.IsAny<Object>()), 
                Times.Once);
    }
    
    [Fact]
    public async Task ClearUnusedImagesAsync_whenCacheIsNotEmptyAndRepositoryThrows_thenThrowBadRequest()
    {
        //Arrange
        _fixture.MockMemoryCache
            .Setup(cache => cache.TryGetValue(
                It.IsAny<object>(), 
                out _fixture.CachedFileNamesListWithUnusedImages));

        _fixture.MockImageRepository
            .Setup(repo => repo.DeleteAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws(new RequestFailedException(""));
        
        //Act
        var resultTask = _fixture.MockImageService.ClearUnusedImagesAsync(_fixture.BodyWithUsedAndUnusedImages, 17);
        
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
        //Arrange
        _fixture.MockMemoryCache
            .Setup(cache => cache.TryGetValue(
                It.IsAny<object>(), 
                out _fixture.DefaultCachedFileNamesList));

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
        //Arrange
        _fixture.MockMemoryCache
            .Setup(cache => cache.TryGetValue(
                It.IsAny<object>(), 
                out _fixture.CachedFileNamesListWithAllUnusedImages));
        
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
        Assert.Equal(_fixture.ListOfImagesExpectedToDelete, actualDeletedImages);
        
        _fixture.MockMemoryCache
            .Verify(
                cache => cache.Remove(It.IsAny<Object>()), 
                Times.Once);
    }
}