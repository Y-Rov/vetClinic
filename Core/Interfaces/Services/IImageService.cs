using Microsoft.AspNetCore.Http;

namespace Core.Interfaces.Services;

public interface IImageService
{
    public string TrimArticleImages(string body);
    Task ClearOutdatedImagesAsync(string newBody, string oldBody);
    Task<string> UploadImageAsync(IFormFile file, int authorId);
    Task<string> DeleteImagesAsync(string body);
    Task DiscardCachedImagesAsync(int authorId);
    Task ClearUnusedImagesAsync(string body, int authorId);
}