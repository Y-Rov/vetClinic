using System.Drawing;
using Azure.Storage.Blobs;
using Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class ImageService : IImageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _configuration;

    public ImageService(
        BlobServiceClient blobServiceClient,
        IConfiguration configuration)
    {
        _blobServiceClient = blobServiceClient;
        _configuration = configuration;
    }
    
    /// <summary>
    /// Uploads image to the Azure blob storage
    /// </summary>
    /// <param name="image">Image to upload</param>
    /// <param name="folder">Folder on your blob where the image will be uploaded. For example - <b>articles</b> , without slash</param>
    /// <param name="imageFormat">Image format, like <b>png</b></param>
    /// <param name="fileName">Optionally name of the file. If not provided - a file will be named with new guid. Note that <b>fileName</b> be unique</param>
    /// <example>var resultName = await UploadFromImageAsync(<b><br></br>image: readImage, <br></br>folder: "articles", <br></br>imageFormat: "png", <br></br>fileName: "{firstName}-{email}.{imageFormat}"</b>)</example>
    /// <returns>A path to the file, including folder, file name and format: <b>$"{folder}/{fileName}.{imageFormat}"</b></returns>
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

    /// <summary>
    /// Converts base64 string to an image and uploads it to the Azure blob storage
    /// </summary>
    /// <param name="base64">Base 64 string to convert and than upload</param>
    /// <param name="folder">Folder on your blob where the image will be uploaded. For example - <b>articles</b> , without slash</param>
    /// <param name="imageFormat">Image format, like <b>png</b></param>
    /// <param name="fileName">Optionally name of the file. If not provided - a file will be named with new guid. Note that <b>fileName</b> be unique</param>
    /// <example>var resultName = await UploadFromBase64Async(<b><br></br>base64: "*very large string here*", <br></br>folder: "articles", <br></br>imageFormat: "png", <br></br>fileName: "{firstName}-{email}.{imageFormat}"</b>)</example>
    /// <returns>A path to the file, including folder, file name and format: <b>$"{folder}/{fileName}.{imageFormat}"</b></returns>
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

    /// <summary>
    /// Deletes a file from the folder of Azure blob storage
    /// </summary>
    /// <param name="imageName">Image name to delete</param>
    /// <param name="folder">Folder on your blob from where the image will be deleted. For example - <b>articles</b> , without slash</param>
    /// <example>the file will be deleted from articles/0f8fad5b-d9cb-469f-a165-70867728950e.png: <br></br> await DeleteAsync(<b><br></br>imageName: "0f8fad5b-d9cb-469f-a165-70867728950e.png", <br></br>folder: "articles"</b>)</example>
    public async Task DeleteAsync(string imageName, string folder)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient(_configuration["Azure:ContainerName"]);
        var blobClient = blobContainer.GetBlobClient($"{folder}/{imageName}");
        await blobClient.DeleteAsync();
    }
    
    /// <summary>
    /// Deletes a file from the folder of Azure blob storage
    /// </summary>
    /// <param name="path">Path of the file to delete. Like "articles/0f8fad5b-d9cb-469f-a165-70867728950e.png"</param>
    /// <example>the file will be deleted from articles/0f8fad5b-d9cb-469f-a165-70867728950e.png: <br></br> await DeleteAsync(<b><br></br>path: "articles/0f8fad5b-d9cb-469f-a165-70867728950e.png"</b>)</example>
    public async Task DeleteAsync(string path)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient(_configuration["Azure:ContainerName"]);
        var blobClient = blobContainer.GetBlobClient(path);
        await blobClient.DeleteAsync();    
    }
}