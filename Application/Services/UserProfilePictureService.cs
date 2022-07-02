using Azure.Storage.Blobs;
using Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Drawing;

namespace Application.Services
{
    public class UserProfilePictureService : IUserProfilePictureService
    {
        private readonly IConfiguration _configuration;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public UserProfilePictureService(
            IConfiguration configuration,
            BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
            _containerName = _configuration["Azure:ContainerName"];
        }

        public async Task<string> UploadAsync( 
            Image image,
            string email,
            string imageFormat)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            var fileName = $"profile-pictures/{email}.{imageFormat}";

            var blobClient = blobContainer.GetBlobClient(fileName);

            if (await blobClient.ExistsAsync())
            {
                await DeleteAsync(fileName);
            }

            using (MemoryStream ms = new())
            {
                image.Save(ms, image.RawFormat);
                ms.Position = 0;

                await blobClient.UploadAsync(ms);
            }

            fileName = $"{_configuration["Azure:ContainerLink"]}/{_containerName}/{fileName}";

            return fileName;
        }

        public async Task DeleteAsync(string imageLink)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);

            if (imageLink.Contains(_containerName))
            {
                imageLink = imageLink.Split(_containerName)[1];
            }

            var blobClient = blobContainer.GetBlobClient(imageLink);

            await blobClient.DeleteAsync();
        }
    }
}
