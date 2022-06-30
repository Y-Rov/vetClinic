using System.Drawing;
using Azure.Storage.Blobs;
using Core.Interfaces.Services;

namespace Application.Services;

public class ArticleImageManager : IArticleImageManager
{
    private readonly BlobServiceClient _blobServiceClient;

    public ArticleImageManager(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadAsync(Image image, string imageFormat)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient("vet-clinic");
        var fileName = "articles/" + Guid.NewGuid().ToString() + "." + imageFormat;
        var blobClient = blobContainer.GetBlobClient(fileName);

        var ms = new MemoryStream();
        image.Save(ms, image.RawFormat);
        ms.Position = 0;

        await blobClient.UploadAsync(ms);
        return fileName;
    }

    public async Task DeleteAsync(string image)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient("vet-clinic");
        var blobClient = blobContainer.GetBlobClient("articles/" + image);
        await blobClient.DeleteAsync();
    }
}