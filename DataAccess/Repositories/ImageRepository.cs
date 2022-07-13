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

  public async Task<string> UploadFromIFormFile(IFormFile file, string folder, string fileName = "")
  {
      var blobContainer = _blobServiceClient.GetBlobContainerClient(_configuration["Azure:ContainerName"]);
      if(string.IsNullOrEmpty(fileName))
      { 
          fileName = Guid.NewGuid().ToString();
      }

      var imageFormat = file.ContentType.Split('/').Last();

      var fullFilePath = $"{folder}/{fileName}.{imageFormat}";
      var ms = new MemoryStream();
      var blobClient = blobContainer.GetBlobClient(fullFilePath);
      await file.CopyToAsync(ms);
      ms.Position = 0;

      await blobClient.UploadAsync(ms);

      return $"{_configuration["Azure:ContainerLink"]}/{_configuration["Azure:ContainerName"]}/{fullFilePath}";
  }

  public async Task DeleteAsync(string imageName, string folder)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient(_configuration["Azure:ContainerName"]);
        var blobClient = blobContainer.GetBlobClient($"{folder}/{imageName}");
        await blobClient.DeleteAsync();
    }
}