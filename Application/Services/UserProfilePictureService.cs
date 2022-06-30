using Azure.Storage.Blobs;
using Core.Interfaces.Services;
using Core.ViewModels.User;
using System.Drawing;

namespace Application.Services
{
    public class UserProfilePictureService : IUserProfilePictureService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private const string BLOBLINK = "http://127.0.0.1:10000/devstoreaccount1/vet-clinic/profile-pictures/";

        public UserProfilePictureService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> UploadAsync( 
            Image image,
            string firstName,
            string email,
            string imageFormat)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("vet-clinic");
            var fileName = $"profile-pictures/{firstName}-{email}.{imageFormat}";
            var blobClient = blobContainer.GetBlobClient(fileName);

            MemoryStream ms = new();
            image.Save(ms, image.RawFormat);
            ms.Position = 0;

            await blobClient.UploadAsync(ms);
            fileName = $"{BLOBLINK}/{fileName}";

            return fileName;
        }

        public async Task DeleteAsync(string image)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("vet-clinic");
            var blobClient = blobContainer.GetBlobClient($"profile-pictures/{image}");
            await blobClient.DeleteAsync();
        }
    }
}
