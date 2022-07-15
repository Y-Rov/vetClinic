using System.Drawing;

namespace Core.Interfaces.Services
{
    public interface IUserProfilePictureService
    {
        Task<string> UploadAsync(Image image, string email, string imageFormat);
        Task DeleteAsync(string imageLink);
    }
}
