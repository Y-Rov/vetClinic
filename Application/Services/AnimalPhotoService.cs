using Azure.Storage.Blobs;
using Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AnimalPhotoService : IAnimalPhotoService
    {
        private readonly IConfiguration _configuration;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public AnimalPhotoService(
            IConfiguration configuration,
            BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
            _containerName = _configuration["Azure:ContainerName"];
        }

        public async Task<string> UploadAsync(
            Image image,
            string NickName,
            string imageFormat)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            var fileName = $"animal-pictures/{NickName}.{imageFormat}";

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
