using Microsoft.AspNetCore.Http;

namespace Core.Interfaces.Services;

public interface IImageService
{
    public void ParseImgTag(string tag, out string link, out string fileName, out bool isOuterLink);
    public string TrimArticleImages(string body);
    Task ClearOutdatedImagesAsync(string newBody, string oldBody);
    Task<string> UploadImageAsync(IFormFile file, int authorId);
    Task<string> DeleteImagesAsync(string body);
    Task DiscardCachedImagesAsync(int authorId);
    Task ClearUnusedImagesAsync(string body, int authorId);
}