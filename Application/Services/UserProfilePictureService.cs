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

        public UserProfilePictureService(
            IConfiguration configuration,
            BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
        }

        public async Task<string> UploadAsync( 
            Image image,
            string email,
            string imageFormat)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_configuration["Azure:ContainerName"]);
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

            fileName = $"{_configuration["Azure:ContainerLink"]}/{_configuration["Azure:ContainerName"]}/{fileName}";

            return fileName;
        }

        public async Task DeleteAsync(string image)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("vet-clinic");
            var blobClient = blobContainer.GetBlobClient(image);

            await blobClient.DeleteAsync();
        }
    }
}
