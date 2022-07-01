using System.Drawing;

namespace Core.Interfaces.Services
{
    public interface IAnimalImageManager
    {
        public Task<string> UploadAsync(Image image, string imageFormat);
        public Task DeleteAsync(string image);
    }
}
