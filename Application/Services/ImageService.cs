using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class ImageService : IImageService
{
    private readonly IConfiguration _configuration;
    private readonly IImageRepository _imageRepository;
    private readonly IMemoryCache _memoryCache;

    public ImageService(
        IConfiguration configuration,
        IImageRepository imageRepository, IMemoryCache memoryCache)
    {
        _configuration = configuration;
        _imageRepository = imageRepository;
        _memoryCache = memoryCache;
    }
    public void ParseImgTag(string tag, out string link, out string fileName, out bool isOuterLink)
    {
        link = string.Empty;
        fileName = string.Empty;
        isOuterLink = false;
        int srcOffset = tag.IndexOf("src", StringComparison.Ordinal) - 5;

        int possibleQueryIndex = tag.IndexOf('?', 11 + srcOffset);
        int closingQuoteIndex = tag.IndexOf('"', 11+ srcOffset);
        int linkEndingIndex = possibleQueryIndex > 0 && possibleQueryIndex < closingQuoteIndex
            ? possibleQueryIndex
            : closingQuoteIndex;
        link = tag.Substring(10 + srcOffset, linkEndingIndex - 10 - srcOffset);
        isOuterLink = link.Substring(0, _configuration["Azure:ContainerLink"].Length) != _configuration["Azure:ContainerLink"];

        if (!isOuterLink)
        {
            fileName = link.Split('/').Last();
        }
    }

    public string TrimArticleImages(string body)
    {
        var tagSplitter = new TagSplitter(body);
        while (tagSplitter.TryGetNextTag(out string tag, out var startIndex, out var length))
        {
            ParseImgTag(tag, out var link, out _ , out var isOuterLink);
            if (isOuterLink)
            {
                //avoid writing
                body = body.Remove(
                    startIndex: startIndex ,
                    count: length);
                body = body.Insert(startIndex, "<img src=\"" +link + '"');
            }
        }

        return body;
    }
    
    public async Task<string> UpdateArticleImagesAsync(string newBody, string oldBody)
    {
        var tagSplitter = new TagSplitter(newBody);
        while (tagSplitter.TryGetNextTag(out string tag, out var startIndex, out var length))
        {
            ParseImgTag(tag, out var link, out _ , out var isOuterLink);
            if (isOuterLink)
            {
                //avoid writing
                newBody = newBody.Remove(
                    startIndex: startIndex ,
                    count: length);
                newBody = newBody.Insert(startIndex, "<img src=\"" +link + '"');
            }
        }

        tagSplitter.Reset(oldBody);
        while (tagSplitter.TryGetNextTag(out var tag))
        {
            ParseImgTag(tag, out _, out var possibleFileName, out _);
            if (!string.IsNullOrEmpty(possibleFileName) && newBody.Contains(possibleFileName))
            {
                await _imageRepository.DeleteAsync(possibleFileName, "articles");
            }
        }
        return newBody;
    }
    
    
    public async Task<string> DeleteImagesAsync(string body)
    {
        var tagSplitter = new TagSplitter(body);
        while (tagSplitter.TryGetNextTag(out string tag, out var startIndex, out var length))
        {
            ParseImgTag(tag, out var link, out var fileName, out var isOuterLink);

            if (!isOuterLink)
            {
                await _imageRepository.DeleteAsync( fileName, folder: "articles");
            }

            body = body.Remove(
                startIndex: startIndex ,
                count: length + 1);        
        }

        return body;
    }

    public async Task DiscardCachedImagesAsync(int authorId)
    {
        var cachedList = _memoryCache.Get<List<string>>(authorId);
        if (cachedList is null || cachedList.Count == 0)
        {
            return;
        }

        foreach (var fileName in cachedList)
        {
            await _imageRepository.DeleteAsync(fileName, "articles");
        }
        _memoryCache.Remove(authorId);
    }

    public async Task ClearUnusedImagesAsync(string body, int authorId)
    {
        var cachedList = _memoryCache.Get<List<string>>(authorId);
        if (cachedList is null || cachedList.Count == 0)
        {
            return;
        }

        foreach (var fileName in cachedList)
        {
            if (!body.Contains(fileName))
            {
                await _imageRepository.DeleteAsync(fileName, "articles");
            }
        }
        _memoryCache.Remove(authorId);
    }
}