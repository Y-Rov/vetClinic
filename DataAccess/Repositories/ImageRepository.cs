using Azure.Storage.Blobs;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _memoryCache;

    public ImageRepository(
        BlobServiceClient blobServiceClient,
        IConfiguration configuration,
        IMemoryCache memoryCache)
    {
        _blobServiceClient = blobServiceClient;
        _configuration = configuration;
        _memoryCache = memoryCache;
    }

  public async Task<string> UploadFromIFormFile(IFormFile file, int authorId,  string folder, string fileName = "")
  {
      var blobContainer = _blobServiceClient.GetBlobContainerClient(_configuration["Azure:ContainerName"]);
      if(string.IsNullOrEmpty(fileName))
      { 
          fileName = Guid.NewGuid().ToString();
      }

      var imageFormat = file.ContentType.Split('/').Last();
      var formats = new String[]
      {
          "png", "jpg", "jpeg", "webp", "gif"
      };
      if (!formats.Contains(imageFormat))
      {
          throw new BadRequestException("Wrong image format!");
      }
      
      fileName = $"{folder}/{fileName}.{imageFormat}";
      var ms = new MemoryStream();
      var blobClient = blobContainer.GetBlobClient(fileName);
      await file.CopyToAsync(ms);
      ms.Position = 0;

      await blobClient.UploadAsync(ms);
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

      return $"{_configuration["Azure:ContainerLink"]}/{_configuration["Azure:ContainerName"]}/{fileName}";
  }

  public async Task DeleteAsync(string imageName, string folder)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient(_configuration["Azure:ContainerName"]);
        var blobClient = blobContainer.GetBlobClient($"{folder}/{imageName}");
        await blobClient.DeleteAsync();
    }
}