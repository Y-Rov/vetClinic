using System.Drawing;

namespace Core.Interfaces.Services
{
    public interface IAnimalPhotoService
    {
        public Task<string> UploadAsync(
            Image image,
            string NickName,
            string imageFormat);
        public Task DeleteAsync(string imageLink);
    }
}
