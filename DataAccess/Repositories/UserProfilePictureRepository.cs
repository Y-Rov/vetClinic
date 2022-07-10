using Azure.Storage.Blobs;
using Core.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System.Drawing;

namespace DataAccess.Repositories
{
    public class UserProfilePictureRepository : IUserProfilePictureRepository
    {
        private readonly IConfiguration _configuration;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public UserProfilePictureRepository(
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
            var fileName = $"profile-pictures/{email}-{Guid.NewGuid()}.{imageFormat}";

            var blobItems = blobContainer.GetBlobs(prefix: $"profile-pictures/{email}");

            if (blobItems.Count() > 0)
            {
                await DeleteAsync(blobItems.First().Name);
            }

            using (MemoryStream ms = new())
            {
                image.Save(ms, image.RawFormat);
                ms.Position = 0;

                await blobContainer.UploadBlobAsync(fileName, ms);
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
