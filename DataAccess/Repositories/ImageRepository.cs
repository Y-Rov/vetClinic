using System.Drawing;
using Azure.Storage.Blobs;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _configuration;

    public ImageRepository(
        BlobServiceClient blobServiceClient,
        IConfiguration configuration)
    {
        _blobServiceClient = blobServiceClient;
        _configuration = configuration;
    }
    public async Task<string> UploadFromImageAsync(Image image, string folder, string imageFormat, string fileName = "")
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient(_configuration["Azure:ContainerName"]);
        if(string.IsNullOrEmpty(fileName))
        { 
            fileName = Guid.NewGuid().ToString();
        }        
        fileName = $"{folder}/{fileName}.{imageFormat}";
        var blobClient = blobContainer.GetBlobClient(fileName);
        var ms = new MemoryStream();
        image.Save(ms, image.RawFormat);
        ms.Position = 0;

        await blobClient.UploadAsync(ms);
        return fileName;    
    }

  public async Task<string> UploadFromBase64Async(string base64, string folder, string imageFormat, string fileName = "")
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient(_configuration["Azure:ContainerName"]);
        if(string.IsNullOrEmpty(fileName))
        { 
            fileName = Guid.NewGuid().ToString();
        }        
        fileName = $"{folder}/{fileName}.{imageFormat}";
        var blobClient = blobContainer.GetBlobClient(fileName);
        var bytes = Convert.FromBase64String(base64);
        var ms = new MemoryStream(bytes);
        ms.Position = 0;

        await blobClient.UploadAsync(ms);
        return fileName;          
    }

   public async Task DeleteAsync(string imageName, string folder)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient(_configuration["Azure:ContainerName"]);
        var blobClient = blobContainer.GetBlobClient($"{folder}/{imageName}");
        await blobClient.DeleteAsync();
    }
    
   public async Task DeleteAsync(string path)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient(_configuration["Azure:ContainerName"]);
        var blobClient = blobContainer.GetBlobClient(path);
        await blobClient.DeleteAsync();    
    }
}