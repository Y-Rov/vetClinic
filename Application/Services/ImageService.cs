using Azure;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class ImageService : IImageService
{
    private readonly IConfiguration _configuration;
    private readonly IImageRepository _imageRepository;
    private readonly IMemoryCache _memoryCache;
    private ILoggerManager _loggerManager;

    public ImageService(
        IConfiguration configuration,
        IImageRepository imageRepository, IMemoryCache memoryCache, ILoggerManager loggerManager)
    {
        _configuration = configuration;
        _imageRepository = imageRepository;
        _memoryCache = memoryCache;
        _loggerManager = loggerManager;
    }
    void ParseImgTag(ReadOnlyMemory<char> tag, out ReadOnlyMemory<char> link, out ReadOnlyMemory<char> fileName, out bool isOuterLink)
    {
        link = ReadOnlyMemory<char>.Empty;
        fileName = ReadOnlyMemory<char>.Empty;
        isOuterLink = false;
        int srcOffset = tag.Span.IndexOf("src", StringComparison.Ordinal) - 5;
        
        int possibleQueryIndex = tag.Span.Slice(11 + srcOffset).IndexOf("?", StringComparison.Ordinal) + 11 + srcOffset;
        int closingQuoteIndex = tag.Span.Slice(11 + srcOffset).IndexOf("\"", StringComparison.Ordinal) + 11 + srcOffset;
        int linkEndingIndex;
        if (possibleQueryIndex > 11 && possibleQueryIndex < closingQuoteIndex)
        {
            linkEndingIndex = possibleQueryIndex;
        }
        else
        {
            linkEndingIndex = closingQuoteIndex;
        }        
        link = tag.Slice(10 + srcOffset, linkEndingIndex - 10 - srcOffset);
        isOuterLink = link.Slice(0, _configuration["Azure:ContainerLink"].Length).ToString() != _configuration["Azure:ContainerLink"];

        if (!isOuterLink)
        {
            int nameStartIndex = link.Span.LastIndexOf("/", StringComparison.Ordinal);
            fileName = link.Slice(nameStartIndex + 1);        
        }
    }

    public string TrimArticleImages(string body)
    {
        var tagSplitter = new TagSplitter(body);
        while (tagSplitter.TryGetNextTag(out var tag, out var startIndex, out var length))
        {
            ParseImgTag(tag, out var link, out _ , out var isOuterLink);
            if (isOuterLink)
            {
                body = body.Remove(
                    startIndex: startIndex ,
                    count: length);
                body = body.Insert(startIndex, "<img src=\"" + link + '"');
            }
        }

        return body;
    }
    
    public async Task ClearOutdatedImagesAsync(string newBody, string oldBody)
    {
        if (oldBody == newBody)
        {
            return;
        }
        var tagSplitter = new TagSplitter(oldBody);
        while (tagSplitter.TryGetNextTag(out var tag))
        {
            ParseImgTag(tag, out _, out var possibleFileName, out _);
            var fileName = possibleFileName.ToString();
            if (!string.IsNullOrEmpty(fileName) && !newBody.Contains(fileName))
            {
                try
                {
                    await _imageRepository.DeleteAsync(possibleFileName.ToString(), "articles");
                }
                catch (RequestFailedException)
                {
                    _loggerManager.LogWarn("Error while deleting file from the blob");
                    throw new BadRequestException("Error while deleting file from the blob");
                }
            }
        }
    }
    
    
    public async Task<string> DeleteImagesAsync(string body)
    {
        var tagSplitter = new TagSplitter(body);
        while (tagSplitter.TryGetNextTag(out var tag, out var startIndex, out var length))
        {
            ParseImgTag(tag, out var link, out var fileName, out var isOuterLink);

            if (!isOuterLink)
            {
                try
                {
                    await _imageRepository.DeleteAsync(fileName.ToString(), folder: "articles");
                }
                catch (RequestFailedException)
                {
                    _loggerManager.LogWarn("Error while deleting file from the blob");
                    throw new BadRequestException("Error while deleting file from the blob");
                }
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
            try
            {
                await _imageRepository.DeleteAsync(fileName, "articles");
            }
            catch (RequestFailedException)
            {
                _loggerManager.LogWarn("Error while deleting file from the blob");
                throw new BadRequestException("Error while deleting file from the blob");
            }
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
                try
                {
                    await _imageRepository.DeleteAsync(fileName, "articles");
                }
                catch (RequestFailedException)
                {
                    _loggerManager.LogWarn("Error while deleting file from the blob");
                    throw new BadRequestException("Error while deleting file from the blob");
                }
            }
        }
        _memoryCache.Remove(authorId);
    }

    public async Task<string> UploadImageAsync(IFormFile file, int authorId)
    {
        var imageFormat = file.ContentType.Split('/').Last();
        var formats = new [] { "png", "jpg", "jpeg", "webp", "gif"};
        if (!formats.Contains(imageFormat))
        {
            throw new BadRequestException("Wrong image format!");
        }

        string link;
        try
        {
             link = await _imageRepository.UploadFromIFormFile(file, "articles");
        }
        catch (RequestFailedException)
        {
            _loggerManager.LogWarn("Error while uploading file to the blob");
            throw new BadRequestException("Error while uploading file to the blob");
        }
        var fileName = link.Split('/').Last();
        var currentList = _memoryCache.Get<List<string>>(authorId);
        if (currentList is null || currentList.Count == 0)
        {
            _memoryCache.Set(authorId, new List<string> { fileName },TimeSpan.FromMinutes(30));
        }
        else
        {
            currentList.Add(fileName);
            _memoryCache.Set(authorId, currentList,TimeSpan.FromMinutes(30));
        }

        return link;
    }
}